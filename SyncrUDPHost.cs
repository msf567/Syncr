using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Threading;
using Syncr;
namespace Syncr
{
    public struct ClientConnection
    {
        public ClientConnection(string procName, int port)
        {
            ProcessName = procName;
            MainPort = port;
            client = new UdpClient();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.Connect(IPAddress.Loopback, port);
        }
        public UdpClient client;
        public string ProcessName;
        public int MainPort;
    }

    public class SyncrUDPHost
    {
        UdpClient ReceiveClient;
        int Port = 3001;
        private const String IPADDR = "127.0.0.1";
        public bool verbose = false;
        private Object ClientPortLock = new Object();
        public Dictionary<int, ClientConnection> ClientConnections = new Dictionary<int, ClientConnection>();

        private Queue recMessageQueue = Queue.Synchronized(new Queue());
        private UniqueQueue<string> sendBuffer = new UniqueQueue<string>();

        private bool closeFlag = false;
        IPEndPoint RemoteIpEndPoint;
        int timeSinceHeartbeat = 0;
        int heartRate = 20;

        public SyncrPlayer syncr;

        private List<string> RegisteredFunctions = new List<string>();
        public delegate void FuncRegister(string funcName, string procName, SyncrFunctionType type);
        public FuncRegister OnRegisterFunction;

        public SyncrUDPHost(SyncrPlayer sync)
        {
            syncr = sync;

            ReceiveClient = new UdpClient();
            ReceiveClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            ReceiveClient.Client.Bind(new IPEndPoint(IPAddress.Loopback, Port));

            //sendClient = new UdpClient();
            //sendClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            ClientConnections = new Dictionary<int, ClientConnection>();

            Thread thread = new Thread(ListenThread);
            thread.Start();

            Thread updateThread = new Thread(Update);
            updateThread.Start();

            Thread sendThread = new Thread(SendThread);
            sendThread.Start();
        }

        void Update()
        {
            while (!closeFlag)
            {
                while (recMessageQueue.Count > 0)
                {
                    string msg = recMessageQueue.Dequeue().ToString();
                    ParseMessage(msg);
                }

                Thread.Sleep(50);

                timeSinceHeartbeat++;
                if (timeSinceHeartbeat > heartRate)
                {
                    timeSinceHeartbeat = 0;
                    BroadcastMessage("HEARTBEAT");
                }
            }
        }

        private void ListenThread()
        {
            Console.WriteLine("Starting up listener on port " + Port);
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Loopback, 3001);

            while (!closeFlag)
            {
                string msg = Encoding.UTF8.GetString(GetMessage());
                if (msg != "")
                {
                    recMessageQueue.Enqueue(msg);
                }
                Thread.Sleep(50);
            }

            Console.WriteLine("exiting thread");
            CloseSocket();
        }

        private void SendThread()
        {
            Console.WriteLine("Starting up send thread");
            while (!closeFlag)
            {
                lock (sendBuffer)
                {
                    if (sendBuffer.Count > 0)
                    {
                        string msgPkt = CreateMessagePacket(sendBuffer);
                        SendMessagePacket(msgPkt);
                        sendBuffer.Clear();
                    }
                    Thread.Sleep(10);
                }
            }
        }

        private void RegisterFunction(ref string[] args)
        {
            string GroupName = args[1];
            string procName = args[2];
            string functionName = args[3];

            SyncrFunctionType funcType = new SyncrFunctionType(SyncrParamType.VOID, g: GroupName);

            if (args.Length >= 5)
                funcType.type = (SyncrParamType)Enum.Parse(typeof(SyncrParamType), args[4]);

            if (funcType.type == SyncrParamType.INTERNAL)
                return;

            if (funcType.type == SyncrParamType.ENUM)
            {
                string[] tArr = new string[args.Length - 5];

                for (int x = 5; x < args.Length; x++)
                    tArr[x - 5] = args[x];

                funcType.enumChoices = tArr;
            }

            if (!RegisteredFunctions.Contains(functionName))
            {
                RegisteredFunctions.Add(functionName);
                OnRegisterFunction?.Invoke(functionName, procName, funcType);
            }
        }

        private void RegisterClient(string[] args)
        {
            int mainPort = int.Parse(args[1]);
            int eventPort = int.Parse(args[2]);
            string procName = args[3];
            lock (ClientPortLock)
                if (!ClientConnections.ContainsKey(mainPort))
                    ClientConnections.Add(mainPort, new ClientConnection(procName, mainPort));
                else
                    Console.WriteLine(mainPort + " is already registered.");
            SendMessageToSpecificClient("CONNECTED", mainPort);
        }

        public void UnRegisterClientConnection(string procName)
        {
            lock (ClientPortLock)
            {
                List<int> clientKeys = new List<int>(ClientConnections.Keys);
                for (int x = clientKeys.Count - 1; x >= 0; x--)
                    if (ClientConnections[clientKeys[x]].ProcessName == procName)
                        ClientConnections.Remove(clientKeys[x]);
            }
        }

        public string CreateMessagePacket(UniqueQueue<string> commands)
        {
            string result = "";

            if (commands.Count == 1 && commands.Peek() != null)
                return commands.Dequeue();

            //for(int x = commands.Count -1; x >= 0; x--)
            foreach (string s in commands)
            {
                string ss = s;
                result += ss.Trim() + "|";
            }

            return result;
        }

        private void HermesMessage(ref string[] args)
        {
            string message = "";
            if (args.Length > 1)
            {
                for (int x = 1; x < args.Length; x++) // skip first arg (it's "HERMES")
                {
                    message += args[x] + " ";
                }

                BroadcastMessage(message);
            }
        }

        void ParseMessage(string msg)
        {
           // if (verbose)
           // {
            //    Console.WriteLine("Received: " + msg);
          //  }

            string[] args = msg.Split(' ');
            switch (args[0])
            {
                case "GROUP":

                    break;
                case "HERMES":
                    HermesMessage(ref args);
                    break;

                case "FUNCTION":
                    if (args.Length >= 3)
                        RegisterFunction(ref args);
                    break;

                case "PAUSE":
                    if (syncr != null)
                        syncr.PauseScore();
                    break;

                case "RESUME":
                    if (syncr != null)
                        syncr.ResumeScore();
                    break;

                case "REGISTER":
                    if (args.Length < 1)
                        break;
                    RegisterClient(args);
                    break;

                case "UNREGISTER":
                    if (args.Length < 1)
                        break;
                    int deadPort = int.Parse(args[1]);
                    if (ClientConnections.ContainsKey(deadPort))
                        ClientConnections.Remove(deadPort);
                    break;

                case "BOUNCE":
                    if (args.Length < 2)
                        break;
                    string cmd = "";
                    for (int x = 1; x < args.Length; x++)
                        cmd += " " + args[x];
                    BroadcastMessage(cmd);
                    break;
            }
        }

        public void CloseSocket()
        {
            Console.WriteLine("Closing Socket");
            closeFlag = true;
            foreach (ClientConnection c in ClientConnections.Values)
                c.client.Close();
        }

        private byte[] GetMessage()
        {
            return ReceiveClient.Receive(ref RemoteIpEndPoint);
        }

        private void SendMessageToSpecificClient(string msg, int port)
        {
            if (!ClientConnections.ContainsKey(port))
                return;
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
          //  Console.WriteLine("Sent: " + Encoding.UTF8.GetString(bytes) + " on port " + ClientConnections[port].MainPort);
            ClientConnections[port].client.Connect(IPAddress.Loopback, ClientConnections[port].MainPort);
            ClientConnections[port].client.Send(bytes, bytes.Length);
        }

        private void SendMessagePacket(string pkt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pkt.Trim());

            if (!pkt.Contains("HEARTBEAT") && !pkt.Contains("TAP") && verbose)
                Console.WriteLine("Broadcasting: " + Encoding.UTF8.GetString(bytes) + ".");
            lock (ClientPortLock)
                foreach (ClientConnection c in ClientConnections.Values)
                {
                    try
                    {
                        if (c.client != null)
                        {
                            c.client.Connect(IPAddress.Loopback, c.MainPort);
                            c.client.Send(bytes, bytes.Length);
                        }
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
        }

        public void BroadcastMessage(string msg)
        {
            lock (sendBuffer)
            {
                sendBuffer.Enqueue(msg);
            }
        }
    }
}


//we need to create a send queue for outgoing messages
//every 50 ms we send all of the accumulated messages as one package
//

//we can add logic for removing a lot of repeated messages, or make sure we are only sending one of each type per broadcast?