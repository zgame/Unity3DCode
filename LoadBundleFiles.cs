using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadBundleFiles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadVersionFile());                // 先从网络上读取version文件
        
        StartCoroutine(ReadBundleFromWeb("lua"));    // 先从网络上下载lua文件
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
  
    
    // 网络读取BundleVersion.txt 文件，用来比对版本
    IEnumerator DownloadVersionFile() {
        Debug.Log("网络加载 Version 文件");
        string uri = "http://118.89.188.193:8080/zswt/Android/BundleVersion.txt";
        UnityWebRequest m_webRequest = UnityWebRequest.Get(uri);
        m_webRequest.timeout = 30;                                    //设置超时，若m_webRequest.SendWebRequest()连接超时会返回，且isNetworkError为true
        yield return m_webRequest.SendWebRequest();
 
        if(m_webRequest.isNetworkError) {
            Debug.Log("下载BundleVersion 出错，检查网络是否正常:" + m_webRequest.error);
        } else {
            string bytes = m_webRequest.downloadHandler.text;
            //创建文件
            Debug.Log("" + bytes);
        }
        
        // 读取本地version文件
        
        
        // 保存bundle文件到本地
        // 保存version文件到本地
 
       
    }
    
    
    // 下载bundle文件
    IEnumerator ReadBundleFromWeb(string filename)
    {
        Debug.Log("网络下载 bundle 文件");
        string uri = "http://118.89.188.193:8080/zswt/Android/"+filename;
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
        yield return request.Send();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        Debug.Log(bundle.LoadAllAssets()[0].GetType());
        
        TextAsset cube = bundle.LoadAsset<TextAsset>("test1.lua");
        Debug.Log(cube.text);
    }

   
}
