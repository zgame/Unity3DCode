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


public class ReadBundles : MonoBehaviour
{
    private static string BundleServerUrl = "http://118.89.188.193:8080/zswt/Android/";        // 版本服务器的web地址
    private static string BundleVersionFileName = "BundleVersion.txt";
    private static string BundleVersionUrlPath = BundleServerUrl + BundleVersionFileName;        // 网络保存路径
    private string LocalBundlePath;                        // 本地保存路径
    private string LocalVersionPath;                     // version本地保存的目录
    private string webBundleVersionFileTxt;                     // 保存网络上下载的最新的Version文件内容
    private List<string> downloadList = new List<string>();        // 需要更新的列表
    
    
    public static AssetBundle bundleLua;    // lua bundle 文件
    LuaEnv luaenv = null;    
    
    const string variantSceneAssetBundle = "canvas";
    const string variantSceneName = "canvas";

    // The following are used only if app slicing is not enabled.
    private string[] activeVariants;
    private bool bundlesLoaded;             // used to remove the loading buttons

    void Awake()
    {
        LocalBundlePath = Application.persistentDataPath;                // 本地保存路径
        LocalVersionPath = LocalBundlePath + "/" + BundleVersionFileName;     // version本地保存的目录
    
        activeVariants = new string[1];
        bundlesLoaded = false;
        
        luaenv = new LuaEnv();
        luaenv.AddLoader(CustomLoader);
    }

  
    
    
    IEnumerator Start()
    {
        
        Debug.Log("开始加载网络Version文件");
        yield return StartCoroutine(VersionFileDownLoadAndCheck());
        
//        Debug.Log("开始加载本地文件");
//        yield return StartCoroutine(DownloadAssetBundleFileLua());  // 加载lua bundle文件


        
//        yield return StartCoroutine(Initialize());
        // Load level.
//        yield return StartCoroutine(InitializeLevelAsync(variantSceneName, true));
//        yield return StartCoroutine(InstantiateGameObjectAsync(variantSceneAssetBundle,variantSceneName));
//        yield return StartCoroutine(InstantiateTextAsync("lua","test2.lua"));

//        luaenv.DoString("require 'Lua/test1'");
    }
    

    // Initialize the downloading URL.
    // eg. Development server / iOS ODR / web URL
    void InitializeSourceURL()
    {
        // If ODR is available and enabled, then use it and let Xcode handle download requests.
        #if ENABLE_IOS_ON_DEMAND_RESOURCES
        if (UnityEngine.iOS.OnDemandResources.enabled)
        {
            AssetBundleManager.overrideBaseDownloadingURL += OverrideDownloadingURLForLocalBundles;
            AssetBundleManager.SetSourceAssetBundleURL("odr://");
            return;
        }
        #endif
        #if DEVELOPMENT_BUILD || UNITY_EDITOR
        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project.
        //      Another approach would be to make this configurable in the standalone player.)
        AssetBundleManager.SetDevelopmentAssetBundleServer();
        return;
        #else
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
        // Or customize the URL based on your deployment or configuration
        //AssetBundleManager.SetSourceAssetBundleURL("http://www.MyWebsite/MyAssetBundles");
        return;
        #endif
    }

    // Initialize the downloading url and AssetBundleManifest object.
    protected IEnumerator Initialize()
    {
        // Don't destroy the game object as we base on it to run the loading script.
        DontDestroyOnLoad(gameObject);

        InitializeSourceURL();

        // Initialize AssetBundleManifest which loads the AssetBundleManifest object.
        var request = AssetBundleManager.Initialize();

        if (request != null)
            yield return StartCoroutine(request);
    }

    protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive)
    {
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;

        // Load level from assetBundle.
        AssetBundleLoadOperation request = AssetBundleManager.LoadLevelAsync(variantSceneAssetBundle, levelName, isAdditive);
        if (request == null)
            yield break;

        yield return StartCoroutine(request);

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
    }
    
    protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName)
    {
//        var obj = GameObject.Find("Canvas1");
//        var txt = obj.GetComponentInChildren<UnityEngine.UI.Text>();
//        txt.text = "加载bundle";
        
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;

        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(GameObject));
        if (request == null)
        {
//            txt.text = "LoadAssetAsync request == null";
            Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + assetBundleName + ".");
            yield break;
        }
//        txt.text = "yield return StartCoroutine(request)";
        yield return StartCoroutine(request);

        // Get the Asset.
        GameObject prefab = request.GetAsset<GameObject>();

        // Instantiate the Asset, or log an error.
        if (prefab != null)
        {
//            txt.text = "Instantiate(prefab)";
            GameObject.Instantiate(prefab);
        }
        else
        {
//            txt.text = "prefab  == null";
            Debug.LogError("Failed to GetAsset from request");
        }

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log(assetName + (prefab == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
    }
    
    protected IEnumerator InstantiateTextAsync(string assetBundleName, string assetName)
    {
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;

        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(TextAsset));
        if (request == null)
        {
            Debug.LogError("Failed AssetBundleLoadAssetOperation on " + assetName + " from the AssetBundle " + assetBundleName + ".");
            yield break;
        }
        yield return StartCoroutine(request);

        // Get the Asset.
        TextAsset prefab = request.GetAsset<TextAsset>();
//        Debug.Log("-----------InstantiateTextAsync   lua file -------------:"+  prefab.text);       
        
        // Instantiate the Asset, or log an error.
        if (prefab != null)
        {
          
//            GameObject.Instantiate(prefab);
//            luaenv = new LuaEnv();
//            luaenv.AddLoader(prefab.bytes);
//            luaenv.DoString(prefab.text);
//            luaenv.AddLoader(CustomLoader);

          
        }
        else
            Debug.LogError("Failed to GetAsset from request");

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log(assetName + (prefab == null ? " was not" : " was") + " loaded successfully in " + elapsedTime + " seconds");
    }

    public static byte[] CustomLoader(ref string filepath)
    {
        var str = LoadFromFile(filepath);
        str = "print('ssssssssss')";
        return System.Text.Encoding.Default.GetBytes ( str );

    }
   
    public static string LoadFromFile(string assetName) {
        
        TextAsset prefab = bundleLua.LoadAsset<TextAsset>("assets/assetpackage/lua/test2.lua.txt");
        Debug.Log(prefab.text);
        
        return prefab.text;
    }


    // 从网络下载bundle文件
    IEnumerator DownloadAssetBundleFileLua()
    {
        string uri = BundleServerUrl + "lua";        
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, 0);
        yield return request.Send();
        bundleLua = DownloadHandlerAssetBundle.GetContent(request);
        Debug.Log(bundleLua.GetAllAssetNames()[0]);
        
        
        TextAsset prefab = bundleLua.LoadAsset<TextAsset>("assets/assetpackage/lua/test2.lua.txt");
        Debug.Log(prefab.text);
        var obj = GameObject.Find("Canvas1");
        var txt = obj.GetComponentInChildren<UnityEngine.UI.Text>();
        txt.text = prefab.text;
    }
    
    
    //---------------------------------------------------------------------------------
    // 先下载version文件，然后跟本地文件进行比对
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

    // -----------按照更新列表开始下载bundle文件----------------
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
        
    }
    
    //----------下载bundle并缓存到本地  -------------
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
                    // 开始加载游戏bundle
                    
                }
            }
        }
    }

    // 将最新的Version文件更新到本地
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



//            var obj = GameObject.Find("Canvas1");
//            var txt = obj.GetComponentInChildren<UnityEngine.UI.Text>();
//            txt.text = wwwlogin.text;

    