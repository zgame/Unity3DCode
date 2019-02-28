using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVirtualCamera()
    {
        Debug.Log("test cs");
        GameObject obj = GameObject.Find("CM vcam1");
        CinemachineVirtualCamera vir = obj.GetComponent<CinemachineVirtualCamera>();
        Debug.Log(""+ vir.Priority);
    }
}
