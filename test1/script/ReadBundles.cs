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

//------------------------------------------------------------------------------------------------------------
// 把文件从本地加载进入内存
// 1 . 先读取本地bundle文件， 防止重复增加哈希表管理
// 2.  lua文件加载器，用来读取lua文件
// 3.  通用的bundle资源加载函数
// 4.  调试模式说明， 开启调试模式， lua文件不需要打asset bundle， 其他文件因为涉及到依赖关系，所以还是需要build asset bundle，而且需要点击copy to streamingAssetsPath， 从streamingAssetsPath读文件
//------------------------------------------------------------------------------------------------------------

//#define THREAD_SAFE 

[LuaCallCSharp]
public class ReadBundles
{
    private static string AssetPackagePath = "Assets/AssetPackage/";    //lua 默认的asset路径
    public static Dictionary<string, AssetBundle>LoadAssetBundleDictionary = new Dictionary<string, AssetBundle>(); // 已经加载的bundle文件，防止重复加载用的
    public static LuaEnv luaenv = null;                    // lua句柄
    private static AssetBundleManifest GlobalManifest;      //全局manifest，用来管理依赖的
    public static bool DebugMode = true; // 是否是debug模式 
    

    public static void Start()
    {
        // 首先读取全局的manifest，用来查找引用的
        var AndroidManifest = GetAssetBundle(DownLoadBundles.PhonePlatform);
        GlobalManifest = AndroidManifest.LoadAsset<AssetBundleManifest>("AssetBundleManifest");    // 该写法是固定的
        
      
        
        // 创建Lua环境
        luaenv = new LuaEnv();
        luaenv.AddLoader(CustomLoader);
//        luaenv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadPb);                    // protocol buffer 库
//        luaenv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);        //  rapid json  库3

        luaenv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        luaenv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
        luaenv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
        luaenv.AddBuildin("ffi", XLua.LuaDLL.Lua.LoadFFI);
        
        luaenv.DoString(@"require 'Lua/main'");
    }
    //----------------------------------------------------------------
    // lua文件的加载器
    //----------------------------------------------------------------
    public static byte[] CustomLoader(ref string luaFileName)
    {
        string fileText = "";
        if (!DebugMode)
        {
            // 正式模式从AssetBundle里面读取
            var myLuaBundle = GetAssetBundle("lua"); // lua的bundle文件名，一般不要改动
            if (myLuaBundle != null)
            {
                TextAsset prefab = myLuaBundle.LoadAsset<TextAsset>(AssetPackagePath + luaFileName + ".lua.txt");
                fileText = prefab.text;
            }
        }
        else
        {
            // 调试模式， 直接读文件了，不用bundle
            var url = Application.dataPath + "/AssetPackage/" + luaFileName + ".lua.txt";
//            Debug.Log("" + url);
            fileText = File.ReadAllText(url);
        }

        return System.Text.Encoding.Default.GetBytes(fileText);
    }


    //----------------------------------------------------- bundle ------------------------------------------------------
    // 获得对应的assetBundle
    //----------------------------------------------------------------
    public static AssetBundle GetAssetBundle(string bundleName)
    {
        if (LoadAssetBundleDictionary.ContainsKey(bundleName))
        {
            return LoadAssetBundleDictionary[bundleName]; // 已经加载就返回
        }
        // 没有加载过就加载
        AssetBundle myLoadedAssetBundle;
        if (DebugMode)
            myLoadedAssetBundle =AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.streamingAssetsPath ,bundleName)); // 调试模式开启，读取streamingAssetsPath    
        else
            myLoadedAssetBundle =AssetBundle.LoadFromFile(System.IO.Path.Combine(Application.persistentDataPath ,bundleName)); // 正式模式，读取Application.persistentDataPath

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("加载本地 AssetBundle 出错了! " + bundleName);
            return null;
        }
        LoadAssetBundleDictionary[bundleName] = myLoadedAssetBundle;    // 加载之后把已加载的bundle文件记录一下
        return myLoadedAssetBundle;
    }
    //----------------------------------------------------------------
    // 卸载不用的bundle
    //----------------------------------------------------------------
    public static void UnLoadAssetBundle(string bundleName)
    {
        if (LoadAssetBundleDictionary.ContainsKey(bundleName))
        {
            var bundle = LoadAssetBundleDictionary[bundleName];
            LoadAssetBundleDictionary.Remove(bundleName);
            bundle.Unload(true);
        }
    }
    
//    [LuaCallCSharp]
    //----------------------------------------------------- load game object ------------------------------------------------------
    // lua 调用该函数加载game object        加载物体名称         路径             父物体         是否instantiate
    public static GameObject LoadAndInstantiateGameObject(string bundleFileName, string assetPrefabPath, string parentDir, bool instantiate)
    {
        GameObject prefab = LoadAssetBundle(bundleFileName,assetPrefabPath);
        if (!instantiate)
            return null;
        // 创建物体，设置父物体，清理位置
        var obj = GameObject.Instantiate(prefab, GameObject.Find(parentDir).gameObject.transform, true);
        obj.name = obj.name.Replace("(Clone)", "");
        obj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        obj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.SetActive(true);
        return obj;
    }
    
    
    // 从bundle中加载game object
    public static GameObject LoadAssetBundle(string bundleFileName,string assetPrefabPath)
    {
        // 首先查找要加载bundle的引用， 提前加载引用， 最后加载要加载的bundle
        var dependenciesList = GlobalManifest.GetAllDependencies(bundleFileName);
        if (dependenciesList.Length > 0)
        {
            foreach (var dependeciesFile in dependenciesList)
            {
                // 按照依赖列表，提前加载依赖
//                Debug.Log(bundleFileName + " 需要依赖：" + dependeciesFile);
                GetAssetBundle(dependeciesFile);
            }
        }
        
        // 加载完依赖，加载自身
        var bundle = GetAssetBundle(bundleFileName);
        var objSelf = bundle.LoadAsset<GameObject>( assetPrefabPath);
        if (objSelf == null)
        {
            Debug.Log("加载game object出现问题 "+ assetPrefabPath + "     "+ bundleFileName);
        }
        return objSelf;
    }
    
    
    
    //--------------------------------------------------------------------------------------------------------------
   

}