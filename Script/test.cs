using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using XLua;

public class test : MonoBehaviour
{
    LuaEnv luaenv = null;
    // Use this for initialization
    void Start()
    {
        luaenv = new LuaEnv();
        
        
//        luaenv.AddLoader(LuaPathLoader);
      
        
        luaenv.DoString("require 'Lua/test1'");
    }
    private byte[] LuaPathLoader(string filePath)
    {
        string fullPath = Application.persistentDataPath + "/lua/" + filePath + ".lua.txt" ;
        return Encoding.UTF8.GetBytes(File.ReadAllText(fullPath));
    } 

    // Update is called once per frame
    void Update()
    {
        if (luaenv != null)
        {
            luaenv.Tick();
        }
    }

    void OnDestroy()
    {
        luaenv.Dispose();
    }
}
