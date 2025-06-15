using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movehandcart : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(0f,0.7f,13f); // �̵��� ��ǥ ��ġ
    private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Management management = GameObject.Find("player").GetComponent<Management>();
        bool success = management.SuccessOn;
        bool end = management.End;

        moveSpeed = 3f;

        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);

        bool isMoving = distance > 1f && !end;

        if (isMoving)
        {
            // �̵� �ӵ��� ������ ���Ͽ� �̵�
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            //particleSystem.Play();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(10f, 0.7f,13f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            moveSpeed = 0f;
            targetPosition = new Vector3(0f, 0.7f, 13f);
        }
        else if (success)
        {
            targetPosition = new Vector3(-13f, 0.7f, 13f);

            moveSpeed = 3f;

            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
}
