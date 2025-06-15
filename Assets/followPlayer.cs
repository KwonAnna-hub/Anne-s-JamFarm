using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Rigidbody player;
    private float positionX;
    private float positionY;
    private float positionZ;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Management management = GameObject.Find("player").GetComponent<Management>();
        if (!management.End)
        {
            transform.rotation = Quaternion.Euler(27.8f, 180f, 0f);
            positionX = player.position.x;
            positionY = player.position.y + 5f;
            positionZ = player.position.z + 9f;
            transform.position = new Vector3(positionX, positionY, positionZ);
        }
        else
        {
            transform.position = new Vector3(-1.6f, 10.22f, 22.5f);
            transform.rotation = Quaternion.Euler(32.9f, 180, 0f);
        }
    }
}
