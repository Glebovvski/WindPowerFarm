using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class AsynchronousClient
{
    private int _port;

    private ManualResetEvent connectDone = new ManualResetEvent(false);
    private ManualResetEvent sendDone = new ManualResetEvent(false);
    private ManualResetEvent receiveDone = new ManualResetEvent(false);

    private string response;
    private bool _releaseSockets;

    public string SendMessage(int port, string message, bool releaseSockects = false)
    {
        try
        {
            _port = port;
            response = string.Empty;
            _releaseSockets = releaseSockects;

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList.First(x => x.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, _port);

            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            Send(client, message + "<EOF>");
            sendDone.WaitOne();

            Receive(client);
            receiveDone.WaitOne();

            //Console.WriteLine("Response received : {0}", response);

            if (releaseSockects)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return response;
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            client.EndConnect(ar);

            Console.WriteLine($"Socket connected to {client.RemoteEndPoint.ToString()}");

            connectDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void Receive(Socket client)
    {
        try
        {
            StateObject state = new StateObject();
            state.workSocket = client;

            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                //state.sb = state.buffer.Take(bytesRead).ToArray();
                client.BeginReceive(state.buffer, 0, bytesRead/*StateObject.BufferSize*/, 0,
                    /*new AsyncCallback(ReceiveCallback)*/ReceiveCallback, state);
            }
            else
            {
                if (state.sb.Length > 1)
                {
                    response = state.sb.ToString();
                }

                receiveDone.Set();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }


    private void Send(Socket client, String data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), client);
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            int bytesSent = client.EndSend(ar);

            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            sendDone.Set();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}

public class StateObject
{
    // Client  socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 1024;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();

//public byte[] sb = new byte[0];
}
