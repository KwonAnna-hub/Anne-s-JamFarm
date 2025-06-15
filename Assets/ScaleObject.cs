using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    void Start()
    {
        // 객체의 크기를 반으로 줄임
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
}
