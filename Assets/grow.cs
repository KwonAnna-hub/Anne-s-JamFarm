using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grow : MonoBehaviour
{
    private Vector3 growthRate = new Vector3(25f, 25f, 25f); // 시간당 증가할 크기
    private Vector3 maxScale = new Vector3(140.0f, 140.0f, 140.0f); // 최대 크기
    private bool growing = false;
    private float delayTime = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        int currentIndex = transform.GetSiblingIndex();
        int nextSiblingIndex = currentIndex + 1;

        if (nextSiblingIndex < transform.parent.childCount && this.gameObject.activeSelf)
        {
            Transform nextSibling = transform.parent.GetChild(nextSiblingIndex);
            Vector3 newScale = transform.localScale + growthRate * Time.deltaTime;
            transform.localScale = newScale;

            if (newScale.x >= maxScale.x || newScale.y >= maxScale.y || newScale.z >= maxScale.z)
            {
                this.gameObject.SetActive(false);
                nextSibling.gameObject.SetActive(true);
            }
        }
    }
    
}
