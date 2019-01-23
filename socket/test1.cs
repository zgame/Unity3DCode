using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Jobs.LowLevel.Unsafe;

public class test1 : MonoBehaviour
{
    private void Start()
    {
//        test();
        SocketClient so = new SocketClient();
        so.SendConnect("172.16.140.131 ", 8123);
        Debug.Log("-----------------------------------------");
        so.SendMessage(1,2,3, "");
    }


    void test()
    {
//        var obj = new TCPHeader();
//        obj.DateStart = 254;
//        obj.CheckCode = 0;
//        obj.PackSize = 0;
//        obj.MainCMDID = 1;
//        obj.SubCMDID = 9;
//        obj.PackerVer = 200;
        
        
        
//        using (MemoryStream ms = new MemoryStream())
//        {
//            IFormatter formatter = new BinaryFormatter();
//            formatter.Serialize(ms, obj);
//            Debug.Log(""+ ms.GetBuffer());
//            SocketClient.PrintBytes(ms.GetBuffer());

//            var bys = TCPHeader.GetSendTcpHeaderData(1, 9, 0, 0, 200);
//            SocketClient.PrintBytes(bys);
//            SocketClient.PrintBytes(System.Text.Encoding.Default.GetBytes ( "st"));

//        }
        
    }
}