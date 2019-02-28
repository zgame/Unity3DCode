using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TestCS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test cs");
        GameObject obj = GameObject.Find("CM vcam1");
        CinemachineVirtualCamera vir = obj.GetComponent<CinemachineVirtualCamera>();
        Debug.Log(""+ vir.Priority);
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
