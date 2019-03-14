using UnityEngine;
using System.Collections;

public class Delay : MonoBehaviour
{
    public bool runOnce = false;
    public float delayTime = 1.0f;
    int count;
    void Awake()
    {
        count = 0;
    }

    void OnEnable()
    {
        if (count == 0)
        {
            gameObject.SetActive(false);
            CancelInvoke("DelayFunc");
            Invoke("DelayFunc", delayTime);
        }
        else
        {
            count = 0;
        }
    }

    void DelayFunc()
    {
        count++;
        gameObject.SetActive(true);
        if(runOnce)
        {
            Destroy(this);
        }
    }

}
