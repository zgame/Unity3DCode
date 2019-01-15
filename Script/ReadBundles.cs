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

//[LuaCallCSharp]
public class ReadBundles
{
    public static Dictionary<string, AssetBundle>LoadAssetBundleDictionary = new Dictionary<string, AssetBundle>(); // 已经加载的bundle文件，防止重复加载用的
    public static LuaEnv luaenv = null;
    public static bool DebugMode = true; // 是否是debug模式 

    public static void Start()
    {
        luaenv = new LuaEnv();
        luaenv.AddLoader(CustomLoader);

        luaenv.DoString("require 'Lua/main'");
    }

    // lua文件的加载器
    public static byte[] CustomLoader(ref string luaFileName)
    {
        string fileText = "";
        if (!DebugMode)
        {
            // 正式模式从AssetBundle里面读取
            var myLuaBundle = GetAssetBundle("lua"); // lua的bundle文件名，一般不要改动
            if (myLuaBundle != null)
            {
                TextAsset prefab = myLuaBundle.LoadAsset<TextAsset>("assets/assetpackage/" + luaFileName + ".lua.txt");
                fileText = prefab.text;
            }
        }
        else
        {
            // 调试模式， 直接读文件了，不用bundle
            var url = Application.dataPath + "/assetpackage/" + luaFileName + ".lua.txt";
//            Debug.Log("" + url);
            fileText = File.ReadAllText(url);
        }

        return System.Text.Encoding.Default.GetBytes(fileText);
    }


    // 获得对应的assetBundle
    public static AssetBundle GetAssetBundle(string bundlename)
    {
        if (LoadAssetBundleDictionary.ContainsKey(bundlename))
        {
            return LoadAssetBundleDictionary[bundlename]; // 已经加载就返回
        }
        // 没有加载过就加载
        AssetBundle myLoadedAssetBundle;
        if (DebugMode)
            myLoadedAssetBundle =AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" +bundlename); // 调试模式开启，读取streamingAssetsPath    
        else
            myLoadedAssetBundle =AssetBundle.LoadFromFile(Application.persistentDataPath + "/" +bundlename); // 正式模式，读取Application.persistentDataPath

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("加载本地 AssetBundle 出错了! " + bundlename);
            return null;
        }
        LoadAssetBundleDictionary[bundlename] = myLoadedAssetBundle;    // 加载之后把已加载的bundle文件记录一下
        return myLoadedAssetBundle;
    }

    public static string getsss()
    {
        return "ssssssss";
    }

    public static void LoadGameObject(string fileName, string prefabName, string parentDir, bool instantiate)
    {
        var myLuaBundle = GetAssetBundle(fileName);
        GameObject prefab = myLuaBundle.LoadAsset<GameObject>(prefabName);
        if (!instantiate)
            return;
        var obj = GameObject.Instantiate(prefab, GameObject.Find(parentDir).gameObject.transform, true);
        obj.GetComponent<RectTransform>().offsetMin = new Vector2(0.0f, 0.0f);
        obj.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);

    }

}