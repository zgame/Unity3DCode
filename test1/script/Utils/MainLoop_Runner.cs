/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;


[LuaCallCSharp]
public class MainLoop_Runner : MonoBehaviour {

    private static float lastGCTime = 0;
    private const float GCInterval = 1;//1 second 


    [CSharpCallLua]
    public delegate void FDelegateMainLoop();
    private FDelegateMainLoop funcUpdate;
    
 
	// Use this for initialization
	void Start ()
    {
 
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (ReadBundles.luaenv.luaEnvLock)
#endif
            {
                funcUpdate = ReadBundles.luaenv.Global.Get<FDelegateMainLoop>("MainLoop");
            }
	}
	
	// Update is called once per frame
	void Update ()
    {
        funcUpdate();
        if (Time.time - lastGCTime > GCInterval)
        {
            ReadBundles.luaenv.Tick();
            lastGCTime = Time.time;
        }
	}

    void OnDestroy()
    {
//        if (luaOnDestroy != null)
//        {
//            luaOnDestroy();
//        }
//        luaOnDestroy = null;
//        luaUpdate = null;
//        luaStart = null;
//        scriptEnv.Dispose();
//        injections = null;
    }
}
