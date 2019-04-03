using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallTriggerEnter : MonoBehaviour
{
    public Action<Collider> handler;
    public void OnTriggerEnter(Collider other)
    {
        if (handler != null)
            handler(other);
    }
}

// lua 调用范例:
// gameObject:AddComponent(typeof(CS.CallTriggerEnter)).handler = function(other) end