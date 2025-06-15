using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seedgrow : MonoBehaviour
{
    private float growtime;
    public Material undergrownMaterial;
    public Material fullgrownMaterial;

    // Start is called before the first frame update
    void Start()
    {
        growtime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        growtime -= Time.deltaTime;
        Renderer renderer = GetComponent<Renderer>();

        if (growtime < 5 && growtime >= 0)
        {
            renderer.material = undergrownMaterial;
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        else if (growtime < 0)
        {
            renderer.material = fullgrownMaterial;
            transform.localScale = new Vector3(1f, 1f, 1f);
            gameObject.tag = "full_grown_plant";
        }
    }
}
