using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
    private static string GUIDownLoadBundleFilePanel = "MainCanvas/DownLoadBundleFilePanel";        // UI prefab 路径
    public static string PhonePlatform = "";        // 手机平台，全局bundle的名字
    
    
    private static string BundleVersionUrlPath = BundleServerUrl + BundleVersionFileName;        // 网络保存路径
    private string LocalBundlePath;                        // 本地保存路径
    private string LocalVersionPath;                     // version本地保存的目录
    private string webBundleVersionFileTxt;                     // 保存网络上下载的最新的Version文件内容
    private List<string> downloadList = new List<string>();        // 需要更新的列表
    private float progress;                    //进度
    private float progressStep;                    //进度增量
    private static bool DoNotDownload = true;     // 是否下载bundle    
    
    
    void Awake()
    {
#if UNITY_IPHONE  
        PhonePlatform = "IOS";                // 按照平台进行判断
#else
//        PhonePlatform = "Android";                // 按照平台进行判断
        PhonePlatform = "StandaloneWindows64";                // 按照平台进行判断
#endif
        LocalBundlePath = Application.persistentDataPath;                // 本地保存路径
        LocalVersionPath =  Path.Combine( LocalBundlePath , BundleVersionFileName);     // version本地保存的目录
    }
   
    IEnumerator Start()
    {
        if (DoNotDownload)
        {
            yield return StartCoroutine(DoNext());        // 不下载bundle，直接进入游戏
        }
        else
        {
            yield return StartCoroutine(VersionFileDownLoadAndCheck()); //开始加载网络Version文件
        }
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
//                            Debug.Log(webBundleName + " 不需要更新" );
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
    //-------------------------------------------------------------------------------------
    // -----------2. 按照更新列表开始下载bundle文件----------------
    //-------------------------------------------------------------------------------------
    public void DownLoadBundleFiles(List<string> list)
    {
        if (list.Count > 0)
        {
            // 有需要更新的，显示更新界面
            progressStep = 100f / list.Count * 0.01f;        // 根据文件的数量来决定进度条每个step的度
            StartCoroutine(downLoadAndSaveAssetBundle(BundleServerUrl, PhonePlatform));
            StartCoroutine(downLoadAndSaveAssetBundle(BundleServerUrl, PhonePlatform+".manifest"));
            foreach (var fileName in list)
            {
                StartCoroutine(downLoadAndSaveAssetBundle(BundleServerUrl, fileName));  
            }
        }
        else
        {
            // 不需要更新，直接进入游戏
            StartCoroutine(DoNext());
        }
        
    }
    //-------------------------------------------------------------------------------------
    //----------2 .下载bundle并缓存到本地  -------------
    //-------------------------------------------------------------------------------------
    IEnumerator downLoadAndSaveAssetBundle(string url,string fileName)    
    {
        Debug.Log(fileName+"开始下载");
        WWW w = new WWW(url+fileName);
        yield return w;
        if (w.isDone)
        {
            Debug.Log(fileName+"下载完成");
            byte[] model = w.bytes;
            int length = model.Length;
            Stream sw;            //文件流信息
            
            // 判断一下文件夹是否存在
            var localPath = System.IO.Path.Combine(LocalBundlePath , fileName);
            CheckDirIsExist(localPath);        // 这里增加一个判断，如果没有目录，那么先创建目录
            FileInfo t = new FileInfo(localPath);
            sw = !t.Exists ? t.Create() : t.OpenWrite();    // 判断本地是否有该文件
            sw.Write(model, 0, length);
            sw.Close();//关闭流  
            sw.Dispose(); //销毁流
            
            // 判断所有文件是否都下载完成
            if (downloadList.Contains(fileName))
            {
                SetDownLoadSliderShow();    // 显示进度条
                downloadList.Remove(fileName);    // 更新列表移除已经完成的文件
                if (downloadList.Count == 0)
                {
                    Debug.Log("----------全部下载完成，更新本地version文件------------");
                    SaveVersionFileToLocal();
                    // 开始进入游戏
                    StartCoroutine(DoNext());
                }
            }
        }
    }

    // 2. 这是一个辅助函数，用来判断目录是否存在的，不存在就创建先， 不然会报没有该目录的错误
    public void CheckDirIsExist(string localPath)
    {
        var pathDir = Path.GetDirectoryName(localPath);
        if (!Directory.Exists(pathDir))
        {
            Directory.CreateDirectory(pathDir);            
            Debug.Log(pathDir + "  目录被创建了");
        }
    }
    
    //-------------------------------------------------------------------------------------
    // 3 . 将最新的Version文件更新到本地
    //-------------------------------------------------------------------------------------
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

    //-------------------------------------------------------------------------------------
    // 显示进度条进度
    //-------------------------------------------------------------------------------------
    public void SetDownLoadSliderShow()
    {
        progress += progressStep;
        var obj = GameObject.Find(GUIDownLoadBundleFilePanel);
        if (!obj) return;
        var slider = obj.GetComponentInChildren<UnityEngine.UI.Slider>();
        slider.value = progress;
//        Debug.Log("progress " + progress);
    }
    //-------------------------------------------------------------------------------------
    // 下载完成进入游戏
    //-------------------------------------------------------------------------------------
    IEnumerator DoNext()
    {
        // 开始进入游戏
        progress = 1;
        progressStep = 1;
        SetDownLoadSliderShow();    //显示100%
        yield return new WaitForSeconds(0.01f);
        
        ReadBundles.Start();
    }
    
    
    void Update()
    {
        if (ReadBundles.luaenv != null)
        {
            ReadBundles.luaenv.Tick();
        }
    }

    void OnDestroy()
    {
        if (ReadBundles.luaenv != null)
        {
//            ReadBundles.luaenv.Dispose();
        }
    }
    
}
