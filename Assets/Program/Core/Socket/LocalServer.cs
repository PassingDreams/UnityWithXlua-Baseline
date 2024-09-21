using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// 用于本地IPC用途，例如接收一些指令进行热更用
/// </summary>
public class LocalServer 
{
    // Start is called before the first frame update
    private Socket listenSocket;
    private Thread listenThread;
    public void Listen()
    {
        if (listenSocket!=null) return;
        // 本地IP地址和端口
        string localhost = "127.0.0.1";
        IPAddress localIP = IPAddress.Parse(localhost);
        int port = 9999;
        
        // 创建监听套接字
        listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        // 绑定IP地址和端口
        listenSocket.Bind(new IPEndPoint(localIP, port)); 
        
        listenSocket.Listen(5);
        
        Debug.Log($"[{localhost}:{port}]Server started, waiting for connections...");

        listenThread=ThreadPool.Instance.GetThread((obj) =>
        {
            // 接受客户端连接
            try
            {
                while (true)
                {
                    Socket clientSocket = listenSocket.Accept(); //这里会同步阻断线程，等待连接
                    Debug.Log($"Accepted connection from {clientSocket.RemoteEndPoint}");

                    // 为每个客户端连接启动一个新的线程进行处理
                    Thread clientThread =ThreadPool.Instance.GetThread(HandleClient);
                    clientThread.Start(clientSocket);
                }
            }
            catch (SocketException e)
            {
                
            }
        });
        listenThread.Start();
    }
    public void Stop()
    {
        Debug.Log("结束LocalHost服务");
        listenSocket?.Close();
        listenSocket = null;
        listenThread?.Abort();
        listenThread = null;
    }
    
    static void HandleClient(object clientObj)
    {
        Socket clientSocket = (Socket)clientObj;

        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = clientSocket.Receive(buffer)) > 0) //Receive方法将阻塞直到有可用数据，因此它永远不会执行一个无所事事的热循环
            {
                string receivedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Array.Clear(buffer,0,buffer.Length); //清空buffer
                
                //Handle Msg
                Debug.Log($"Received: {receivedText}");
                
                // 回显收到的数据
                //clientSocket.Send(buffer, bytesRead, SocketFlags.None);
            }
        }
        catch (SocketException)
        {
            // 处理异常
        }
        finally
        {
            clientSocket.Close();
        }
    }



}
