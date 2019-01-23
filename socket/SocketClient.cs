using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Net;

public enum DisType
{
    Exception,
    Disconnect,
}

public class SocketClient
{
    private TcpClient client = null;
    private NetworkStream outStream = null;
    private MemoryStream memStream;
    private BinaryReader reader;

    private const int MAX_READ = 8192;
    private byte[] byteBuffer = new byte[MAX_READ];

    // Use this for initialization
    public SocketClient()
    {
    }

    /// 注册代理
    public void OnRegister()
    {
        memStream = new MemoryStream();
        reader = new BinaryReader(memStream);
    }

    /// 移除代理
    public void OnRemove()
    {
        this.Close();
        reader.Close();
        memStream.Close();
    }

//---------------------------------------- 核心处理 -------------------------------------------------

    /// 连接服务器
    void ConnectServer(string host, int port)
    {
        client = null;
        client = new TcpClient();
        client.SendTimeout = 1000;
        client.ReceiveTimeout = 1000;
        client.NoDelay = true;
        try
        {
            client.BeginConnect(host, port, new AsyncCallback(OnConnect), null);
        }
        catch (Exception e)
        {
            Close();
            Debug.LogError(e.Message);
        }
    }

    /// 连接上服务器
    void OnConnect(IAsyncResult asr)
    {
        outStream = client.GetStream();
        client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
        Debug.Log("连接成功");
//        NetManager.Instance.OnConnect();
    }

    /// 写数据
    void WriteMessage(byte[] message)
    {
        if (client == null)
        {
            Debug.Log("client == null");
        }

        if (!client.Connected)
        {
            Debug.Log("client.Connected   " + client.Connected);
        }

        if (client != null && client.Connected)
        {
            outStream.BeginWrite(message, 0, message.Length, new AsyncCallback(OnWrite), null);
        }
        else
        {
            Debug.LogError("client.connected----->>false");
        }
    }

    /// 读取消息
    void OnRead(IAsyncResult asr)
    {
        int bytesRead = 0;
        try
        {
            lock (client.GetStream())
            {
                //读取字节流到缓冲区
                bytesRead = client.GetStream().EndRead(asr);
            }

            if (bytesRead < 1)
            {
                //包尺寸有问题，断线处理
                OnDisconnected(DisType.Disconnect, "bytesRead < 1");
                return;
            }

            OnReceive(byteBuffer, bytesRead); //分析数据包内容，抛给逻辑层

            lock (client.GetStream())
            {
                //分析完，再次监听服务器发过来的新消息
                Array.Clear(byteBuffer, 0, byteBuffer.Length); //清空数组
                client.GetStream().BeginRead(byteBuffer, 0, MAX_READ, new AsyncCallback(OnRead), null);
            }
        }
        catch (Exception ex)
        {
//            PrintBytes();
            OnDisconnected(DisType.Exception, ex.Message);
        }
    }

    /// 丢失链接
    void OnDisconnected(DisType dis, string msg)
    {
        Debug.Log("OnDisconnected" + msg);
        Close(); //关掉客户端链接
//        NetManager.Instance.OnDisConnect();
    }

    /// 向链接写入数据流
    void OnWrite(IAsyncResult r)
    {
        try
        {
            outStream.EndWrite(r);
        }
        catch (Exception ex)
        {
            Debug.LogError("OnWrite--->>>" + ex.Message);
        }
    }


    /// 接收到消息
    void OnReceive(byte[] bytes, int length)
    {
        Debug.Log("OnReceive:--------" + System.Text.Encoding.Default.GetString(bytes));

        memStream.Seek(0, SeekOrigin.End);
        memStream.Write(bytes, 0, length);
        //Reset to beginning
        memStream.Seek(0, SeekOrigin.Begin);


        while (RemainingBytes() > 11) // 首先头部要完整
        {
            var Position = memStream.Position; // 记录一下位置，方便后面恢复
            // 读取头部数据
            var header = TCPHeader.DealRecvTcpHeaderData(memStream);

            // 判断头部标识
            if (header.DateStart != TCPHeader.TCPHead)
            {
                Debug.Log("数据包头部标识错误");
                PrintBytes(bytes);
                return;
            }

            // 判断数据包完整性
            if (RemainingBytes() < header.PackSize + header.CheckCode + 1)
            {
                Debug.Log("接收数据不完整");
                memStream.Position = Position; // 退回去
                Debug.Log(" memStream.Position  " + memStream.Position);
                break;
            }

            // 读取数据包内容
            var dataBuffer = reader.ReadBytes(header.PackSize);
            var msgBuffer = reader.ReadBytes(header.CheckCode);
            OnReceivedMessage(dataBuffer, msgBuffer, header); // 传递给逻辑处理

            // 判断尾部标识
            var tail = reader.ReadByte();
            if (tail != TCPHeader.TCPEnd)
            {
                Debug.Log("数据包尾部标识错误");
                PrintBytes(bytes);
                return;
            }
        }

        // 剩下的是不全的数据， 留着下次拼接，然后继续解析
        byte[] leftover = reader.ReadBytes((int) RemainingBytes());
        memStream.SetLength(0);
        memStream.Write(leftover, 0, leftover.Length);
    }

    /// 剩余的字节
    private long RemainingBytes()
    {
        return memStream.Length - memStream.Position;
    }


//---------------------------------------- 外部调用 -------------------------------------------------

    public void Close()
    {
        if (client != null)
        {
            if (client.Connected) client.Close();
            client = null;
        }
    }

    public void SendConnect(string host, int port)
    {
        OnRegister();
        ConnectServer(host,  port);
    }


    public void SendMessage(UInt16 maincmd, UInt16 childcmd, UInt16 token, string buffer)
    {
        var sendBuffer = TCPHeader.GetSendTcpHeaderData(maincmd, childcmd, token, System.Text.Encoding.Default.GetBytes(buffer));
        WriteMessage(sendBuffer);
//        buffer.Close();
    }

    // 接收到消息的逻辑处理
    public void OnReceivedMessage(byte[] dataBuffer, byte[] msgBuffer, TCPHeader header)
    {
        Debug.Log(""+System.Text.Encoding.Default.GetString(dataBuffer));
        Debug.Log(""+System.Text.Encoding.Default.GetString(msgBuffer));
        Debug.Log(""+header.MainCMDID);
        Debug.Log(""+header.SubCMDID);
    }


    // 打印字节
    public static void PrintBytes(byte[] byteBuffer)
    {
        string returnStr = string.Empty;
        for (int i = 0; i < byteBuffer.Length; i++)
        {
            returnStr += byteBuffer[i].ToString("x2");
        }

        Debug.LogError(returnStr);
    }
}


//------------------------------------------------------------------------------------------------------------------------------
//-------------------------------------------------TCPHeader-----------------------------------------------------------------------------
//------------------------------------------------------------------------------------------------------------------------------

public class TCPHeader
{
    public Byte DateStart; //数据类型			始终是fe
    public UInt16 CheckCode; //效验字段			msg错误消息的长度
    public UInt16 PackSize; //数据大小			数据包的大小
    public UInt16 MainCMDID; // 主命令码
    public UInt16 SubCMDID; // 子命令码
    public UInt16 PackerVer; // 封包版本号		数据包的一个客户端编号，用来防止重复的，客户端可以自己记录，然后到9999清零

    public static Byte TCPHead = 254; // FE
    public static Byte TCPEnd = 238; //EE

    // 拼接组成数据包，发送
    public static byte[] GetSendTcpHeaderData(UInt16 maincmd, UInt16 childcmd, UInt16 token, byte[] data)
    {
        MemoryStream ms = null;
        ms = new MemoryStream();

        ms.Position = 0;
        BinaryWriter writer = new BinaryWriter(ms);
        writer.Write(TCPHead);
        writer.Write((UInt16)0);
        writer.Write((UInt16)data.Length);
        writer.Write(maincmd);
        writer.Write(childcmd);
        writer.Write(token);
        writer.Write(data, 0, data.Length);
        writer.Write(TCPEnd);
        writer.Flush();

        byte[] re = ms.ToArray();
        return re;
    }

    // 接收分析数据包
    public static TCPHeader DealRecvTcpHeaderData(MemoryStream ms)
    {
        BinaryReader br = new BinaryReader(ms);
        var header = new TCPHeader();
        header.DateStart = br.ReadByte();
        header.CheckCode = br.ReadUInt16();
        header.PackSize = br.ReadUInt16();
        header.MainCMDID = br.ReadUInt16();
        header.SubCMDID = br.ReadUInt16();
        header.PackerVer = br.ReadUInt16();
        return header;
    }
}