using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Management : MonoBehaviour
{
    public Rigidbody playerRigid; 
    private float moveSpeed; 
    private float rotationSpeed = 180f;
    private bool isColliding = false;

    public Animator animator; // 애니메이터 컴포넌트를 할당받을 변수

    private int success_Score = 1;
    private int Score; //점수
    private float _time = 0f; // 타이머
    public bool End = true; // 끝여부
    public bool SuccessOn = false;
    public Text Text_time; // 타이머 UI 숫자
    public Text Text_score; // 점수 UI 숫자
    public Text Text_level; //레벨 UI
    public RectTransform levelTransform;
    private Vector3 levelOriginalPosition;

    //UI on/off
    public GameObject TitleGUI;
    public GameObject PlayGUI;
    public GameObject SuccessGUI;
    public GameObject FailGUI;
    public GameObject TimeIMG;
    public GameObject ScoreIMG;
    public GameObject ExplainGUI;

    private Vector3 CharacterPosition = new Vector3(5.5f, 0f, 9.5f);//첫 시작 위치
    private Vector2 ScorePosition;
    private Vector3 nowPosition;

    public GameObject Jam_bottle;
    private int count;
    private int j = 0;
    private int level = 1;

    public GameObject item;
    public GameObject jam;
    private bool crash_seed = false;
    private bool crash_handcart = false;
    private bool crash_jam_making_tool = false;
    private bool crash_full_grown_plant = false;
    private bool[] growing = new bool[25];
    private Collider collider = null;

    private Collider collidedObject;
    public Vector3[] plantposition;

    public AudioSource backGroundAudio;
    public AudioSource failAudio;
    private bool isFailAudio;
    public AudioSource itemAudio;
    public AudioSource jam_handcartAudio;
    public AudioSource plantedAudio;
    private bool isPlantedAudio;
    public AudioSource successAudio;
    private bool isSuccessAudio;
    public AudioSource strawberry_sotAudio;

    // Start is called before the first frame update
    void Start()
    {
        TitleGUI.SetActive(true);

        playerRigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        backGroundAudio.Play();

        //배열 초기화
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("land_garden");
        int arrayLength = objectsWithTag.Length;
        plantposition = new Vector3[arrayLength];

        levelOriginalPosition = levelTransform.localPosition;
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        nowPosition = transform.position;

        Vector3 movement = new Vector3(-horizontalInput, 0f, -verticalInput).normalized;
        if (_time == 30f) { moveSpeed = moveSpeed*1.25f; }
        else if (_time < 0) { moveSpeed = 0f; }
        if (movement.magnitude >= 0.1f)
        {
            // 충돌 중이지만 입력이 있는 경우 이동 가능
            if (isColliding && playerRigid.velocity.magnitude < 0.1f)
            {
                isColliding = false;
            }

            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerRigid.AddForce(moveDir.normalized * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if(nowPosition.x < -15)
        {
            playerRigid.position = new Vector3(nowPosition.x + 0.1f, nowPosition.y, nowPosition.z);
        }else if(nowPosition.x > 11)
        {
            playerRigid.position = new Vector3(nowPosition.x - 0.1f, nowPosition.y, nowPosition.z);
        }
        else if(nowPosition.z > 16)
        {
            playerRigid.position = new Vector3(nowPosition.x, nowPosition.y, nowPosition.z - 0.1f);
        }
        else if (nowPosition.z < -10)
        {
            playerRigid.position = new Vector3(nowPosition.x, nowPosition.y, nowPosition.z + 0.1f);
        }

        playerRigid.angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Text_score.text = Score + "/" + success_Score;
        Text_level.text = (level-1).ToString();
        if(End && (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)))
        {
            TitleGUI.SetActive(false);
            ExplainGUI.SetActive(true);
        }
        if (End && Input.GetKey(KeyCode.Space))
        {
            End = false;
            SuccessOn = false;
            Score = 0; //점수 초기화
            backGroundAudio.Play();
            isFailAudio = true;
            isSuccessAudio = true;    

            for (int i = 0; i < Jam_bottle.transform.childCount; i++)
            {
                GameObject child = Jam_bottle.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }

            DifficultSet(40f+(20f*level), 2+(3*level), 150f+(50f*level));

            TitleGUI.SetActive(false);
            ExplainGUI.SetActive(false);
            SuccessGUI.SetActive(false);
            FailGUI.SetActive(false);
            levelTransform.localPosition = levelOriginalPosition;
            levelTransform.localScale = new Vector3(1, 1, 1);
            PlayGUI.SetActive(true);

            ScoreIMG.SetActive(true);
        }
        else if (!End)
        {
            _time -= Time.deltaTime;
            Text_time.text = _time.ToString("F1"); //타이머
        }

        if (Score == success_Score)
        {
            SuccessOn = true;
            finish();
            Success();
        }
        else if (_time < 0)
        {
            finish();
            Fail();
        }

        if (Jam_bottle.transform.childCount > 0 && Jam_bottle != null)
        {
            count = 0;
            for (int i = 0; i < Jam_bottle.transform.childCount; i++)
            {
                GameObject child = Jam_bottle.transform.GetChild(i).gameObject;
                if (child.activeSelf)
                {
                    count++;
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (collidedObject != null && item.transform.GetChild(0).gameObject.activeSelf && !crash_full_grown_plant)//플레이어가 씨앗을 들고 있고 다 자란 식물과 충돌하지 않았으면
            {
                Onoff_Item();
                collidedObject = null;
                isPlantedAudio = true;
            }
            else if (crash_seed)//창고 씨앗 들기
            {
                GameObject child = item.transform.GetChild(0).gameObject;
                child.SetActive(true);
                item.transform.GetChild(1).gameObject.SetActive(false);
                item.transform.GetChild(2).gameObject.SetActive(false);
                itemAudio.Play();
            } 
            else if (crash_handcart && item.transform.GetChild(1).gameObject.activeSelf)//잼병을 들고 있고 수레랑 충돌
            {
                item.transform.GetChild(1).gameObject.SetActive(false);//잼병안보이게
                Score++;

                GameObject child = Jam_bottle.transform.GetChild(j).gameObject;//순서대로 수레 속 잼병 보이게
                child.SetActive(true);
                j++;

                jam_handcartAudio.Play();
            }
            else if(crash_jam_making_tool && item.transform.GetChild(2).gameObject.activeSelf)//솥이랑 충돌, 수확한 딸기 들고 있을 때
            {
                item.transform.GetChild(2).gameObject.SetActive(false);//수확한 딸기 안보이게
                jam.transform.gameObject.SetActive(true) ;
                StartCoroutine(ChangeBoolAfterTime(2f));
                strawberry_sotAudio.Play();
            }
            else if(crash_full_grown_plant && !item.transform.GetChild(2).gameObject.activeSelf && !item.transform.GetChild(1).gameObject.activeSelf)//다 자란 딸기랑 충돌, 수확한 딸기랑 잼병 안보일 때, 
            {
                GameObject[] strawberry = GameObject.FindGameObjectsWithTag("full_grown_plant");
                for (int i = 0; i < strawberry.Length; i++)
                {
                    if (collider.gameObject == strawberry[i].gameObject)//다시 심을 수 있게 자라고 있지 않은 상태로 만듦
                    {
                        growing[i] = false;
                    }
                }

                collider.gameObject.SetActive(false);
                itemAudio.Play();
                item.transform.GetChild(2).gameObject.SetActive(true);
                item.transform.GetChild(1).gameObject.SetActive(false);
                item.transform.GetChild(0).gameObject.SetActive(false);
            }
            crash_full_grown_plant = false;
        }
    }

    void DifficultSet(float time_set, int score_set, float speed_set)
    {
        _time = time_set; //타이머 설정
        success_Score = score_set;
        moveSpeed = speed_set;
        level++;
    }

    IEnumerator ChangeBoolAfterTime(float waitTime)
    {
        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(waitTime);

        if (crash_jam_making_tool)
        {
            item.transform.GetChild(1).gameObject.SetActive(true);//잼병 보이게
            jam.transform.gameObject.SetActive(false);
            itemAudio.Play();
        }
        else
        {
            jam.transform.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider Get)
    {
        //충돌 확인
        if(Get.tag == "seed")
        {
            crash_seed = true;
        }else if(Get.tag == "handcart")
        {
            crash_handcart = true;
        }else if(Get.tag == "Jam_making_tool")
        {
            crash_jam_making_tool = true;
        }else if(Get.tag == "full_grown_plant")
        {
            crash_full_grown_plant = true;
            collider = Get;
        }
    }

    void OnTriggerExit(Collider Get)
    {
        //나간 상태 확인
        if (Get.tag == "seed")
        {
            crash_seed = false;
        }
        else if (Get.tag == "handcart")
        {
            crash_handcart = false;
        }
        else if (Get.tag == "Jam_making_tool")
        {
            crash_jam_making_tool = false;
        }
        else if (Get.tag == "full_grown_plant")
        {
            crash_full_grown_plant = false;
        }
    }

    void OnCollisionEnter(Collision Get)
    {
        collidedObject = Get.collider;

        if (Get.gameObject.tag != "Ground" && Get.gameObject.tag != "land_garden")
        {
            isColliding = true;
        }
    }

    void Onoff_Item()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("land_garden");//land_garden태그를 가진 오브젝트 배열
        int arrayLength = objectsWithTag.Length; //길이 저장

        

        for (int i = 0; i < arrayLength; i++)
        {
            Collider obj = objectsWithTag[i].GetComponent<Collider>();//land_garden 태그 콜라이더들 차례대로 확인
            if (obj == collidedObject && growing[i] == false)
            {
                // 부모 오브젝트의 자식 오브젝트 개수를 가져옴
                int childCount = obj.transform.childCount;

                // 자식 오브젝트들을 처리
                for (int j = 0; j < childCount; j++)
                {
                    GameObject child = obj.transform.GetChild(0).gameObject; //씨앗 선택
                    child.transform.localScale = new Vector3(20f, 20f, 20f);
                    child.gameObject.SetActive(true);
                    if (isPlantedAudio)
                    {
                        plantedAudio.Play();
                        isPlantedAudio = false;
                    }
                    growing[i] = true;
                }
            }
        }
    }

    void Success()
    {
        if (isSuccessAudio)
        {
            successAudio.Play();
            isSuccessAudio = false;
        }
        levelTransform.localPosition = new Vector3(100f, -315f, 0f);
        levelTransform.localScale = new Vector3(2, 2, 2);
        SuccessGUI.SetActive(true);
    }

    void Fail()
    {
        if (isFailAudio)
        {
            failAudio.Play();
            isFailAudio = false;
        }
        FailGUI.SetActive(true);
        level = 1;
    }

    void finish()
    {
        End = true;
        PlayGUI.SetActive(false);
        transform.position = CharacterPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        crash_seed = false;
        crash_handcart = false;
        crash_jam_making_tool = false;
        crash_full_grown_plant = false;

        for (int i = 0; i < item.transform.childCount; i++)
        {
            item.transform.GetChild(i).gameObject.SetActive(false);
        }

        j = 0;


        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("land_garden");//land_garden태그를 가진 오브젝트 배열
        int arrayLength = objectsWithTag.Length; //길이 저장


        for (int i = 0; i < arrayLength; i++)
        {
            for(int j = 0; j < objectsWithTag[i].transform.childCount; j++)
            {
                objectsWithTag[i].transform.GetChild(j).gameObject.SetActive(false);
                growing[i] = false;
            }
        }
    }
}

