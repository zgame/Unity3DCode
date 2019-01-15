using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetBundles;
using UnityEditor;
//using Microsoft.Win32.SafeHandles;
//using NUnit.Framework;
using UnityEngine.Networking;
using XLua;

//-------------------------------------------------------------------------------------------------
// 网络下载bundle文件，缓存到本地， 包含版本控制
// 1. 下载BundleVersion.txt文件， 跟本地文件进行比对，整理出需要下载更新的文件列表
// 2. 有需要更新的，就下载，保存到本地
// 3. 全部下载完成之后， 把BundleVersion.txt文件也更新成最新版本
// 4. 然后开始游戏，加载这些资源文件
//-------------------------------------------------------------------------------------------------

public class DownLoadBundles : MonoBehaviour
{
    private static string BundleServerUrl = "http://118.89.188.193:8080/zswt/Android/";        // 版本服务器的web地址
    private static string BundleVersionFileName = "BundleVersion.txt";
    private static string BundleVersionUrlPath = BundleServerUrl + BundleVersionFileName;        // 网络保存路径
    private string LocalBundlePath;                        // 本地保存路径
    private string LocalVersionPath;                     // version本地保存的目录
    private string webBundleVersionFileTxt;                     // 保存网络上下载的最新的Version文件内容
    private List<string> downloadList = new List<string>();        // 需要更新的列表
    
    
    void Awake()
    {
        LocalBundlePath = Application.persistentDataPath;                // 本地保存路径
        LocalVersionPath = LocalBundlePath + "/" + BundleVersionFileName;     // version本地保存的目录
    }
   
    IEnumerator Start()
    {
//        Debug.Log("开始加载网络Version文件");
        yield return StartCoroutine(VersionFileDownLoadAndCheck());
    }
    
    //---------------------------------------------------------------------------------
    // 1. 先下载version文件，然后跟本地文件进行比对
    //---------------------------------------------------------------------------------
    public IEnumerator VersionFileDownLoadAndCheck()
    {
        WWW wwwlogin = new WWW(BundleVersionUrlPath);
        yield return wwwlogin;
        
        if (wwwlogin.text != null)
        {
            webBundleVersionFileTxt = wwwlogin.text;
            // ---------------成功下载网络Version文件---------
            var webVersionList = (Dictionary<string,object>)MiniJSON.Json.Deserialize(webBundleVersionFileTxt);
            
            // ----------开始读取本地version文件----------
            FileInfo file= new FileInfo(LocalVersionPath);
            if(!file.Exists)
            {
                //----------如果本地文件不存,所有bundle文件的名字添加到要下载的bundle文件列表中-------------------
                foreach (var webVersion in webVersionList)
                {
                    downloadList.Add(webVersion.Key);   // 把文件名字添加到更新列表
                    downloadList.Add(webVersion.Key+ ".manifest");   // 把文件名字添加到更新列表
                }
                DownLoadBundleFiles(downloadList);  // 开始下载
            }
            else
            {
                // ---------------如果有本地文件，那么开始比对版本号-------------------
                var localVersionText = File.ReadAllText(LocalVersionPath);
                var localList =  (Dictionary<string,object>)MiniJSON.Json.Deserialize(localVersionText);
                // ------------开始进行对比-----------------
                foreach (var webValue in webVersionList)                            // 循环对比网络文件
                {
                    var webBundleName = webValue.Key;
                    var webVersion = ((Dictionary<string, object>) webValue.Value)["Version"];        // 网络版本号
                    if (localList.ContainsKey(webBundleName))
                    {
                        var localVersion = ((Dictionary<string, object>) localList[webBundleName])["Version"];    // 本地版本号
                        if (webVersion.Equals(localVersion))
                        {
                            Debug.Log(webBundleName + " 不需要更新" );
                            continue; // 跳过循环，下一个
                        }
                    }
                    // 版本号不存在，或者不匹配，需要更新
                    Debug.Log(webBundleName + " 需要更新" );
                    downloadList.Add(webBundleName);   // 把文件名字添加到更新列表
                    downloadList.Add(webBundleName+ ".manifest");   // 把文件名字添加到更新列表
                }
                // ------------对比结束，开始下载-----------------
                DownLoadBundleFiles(downloadList);  
            }
        }
        else
        {
            Debug.Log("无法连接版本服务器，请检查网络!");
        }
    }

    // -----------2. 按照更新列表开始下载bundle文件----------------
    public void DownLoadBundleFiles(List<string> list)
    {
        if (list.Count > 0)
        {
            StartCoroutine(downLoadAndSaveAssetBundle(BundleServerUrl, "Android"));
            StartCoroutine(downLoadAndSaveAssetBundle(BundleServerUrl, "Android.manifest"));
            foreach (var fileName in list)
            {
                StartCoroutine(downLoadAndSaveAssetBundle(BundleServerUrl, fileName));
            }
        }
        else
        {
            // 不需要更新，直接进入游戏
            ReadBundles.Start();
        }
        
    }
    
    //----------2 .下载bundle并缓存到本地  -------------
    IEnumerator downLoadAndSaveAssetBundle(string url,string fileName)    
    {
//        Debug.Log(fileName+"开始下载");
        WWW w = new WWW(url+fileName);
        yield return w;
        if (w.isDone)
        {
//            Debug.Log(fileName+"下载完成");
            byte[] model = w.bytes;
            int length = model.Length;
            Stream sw;            //文件流信息
            FileInfo t = new FileInfo(LocalBundlePath + "/" + fileName);
            sw = !t.Exists ? t.Create() : t.OpenWrite();
            sw.Write(model, 0, length);
            sw.Close();//关闭流  
            sw.Dispose(); //销毁流
            
            // 判断所有文件是否都下载完成
            if (downloadList.Contains(fileName))
            {
                downloadList.Remove(fileName);
                if (downloadList.Count == 0)
                {
                    Debug.Log("----------全部下载完成，更新本地version文件------------");
                    SaveVersionFileToLocal();
                    // 开始进入游戏
                    ReadBundles.Start();
                }
            }
        }
    }

    // 3 . 将最新的Version文件更新到本地
    public void SaveVersionFileToLocal()
    {
        // 全部下载完成，更新本地version文件
        FileInfo file= new FileInfo(LocalVersionPath);
        var str = file.Create();
        str.Write(System.Text.Encoding.Default.GetBytes(webBundleVersionFileTxt), 0, webBundleVersionFileTxt.Length);
        str.Close();
        str.Dispose(); //文件流释放
        Debug.Log("保存Version文件到本地成功");
    }
   
    
}
