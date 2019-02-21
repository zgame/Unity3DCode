using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class test1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     
        GameObject cube = GameObject.Find("Cube");
        cube.transform.DOMoveX(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
