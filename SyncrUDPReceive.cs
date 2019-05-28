using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Syncr
{
    public delegate void NewMessageEvent(string param = "");

    public interface IUDPReceive
    {
        bool Initialize(bool threaded);
        void Update();
        void SendMessage(string msg);
        void BindFunction(string trigger, NewMessageEvent func, SyncrParamType paramType = SyncrParamType.VOID, string[] enumTypes = null);
    }

    public class SyncrUDPReceive : IUDPReceive
    {
        UdpClient SendClient;
        UdpClient ReceiveClient;
        IPEndPoint TriggerEndPoint;

        string Host = "localhost";
        Int32 Port = 3001;
        Int32 MainPort = 6666;
        Int32 EventPort = 6667;
        Random r;
        public bool verbose = false;
        private bool closeFlag = false;
        private Dictionary<string, NewMessageEvent> RegisteredFunctions;
        private Queue messageQueue = Queue.Synchronized(new Queue());

        private bool connected = false;
        private int Timeout = 1000;

        int timeSinceHeartbeat = -100;
        int deathTime = 300;
        string GroupName = "";
        string ProcName = "";
        public SyncrUDPReceive(string _name)
        {
            GroupName = _name;
            ProcName = Process.GetCurrentProcess().ProcessName;
            RegisteredFunctions = new Dictionary<string, NewMessageEvent>();

            r = new Random(Guid.NewGuid().GetHashCode());
            MainPort = GetUniquePort();
            EventPort = GetUniquePort();
        }

        public bool Initialize(bool threaded = true)
        {
            InitSockets();
            Thread thread = new Thread(ListenThread);
            thread.IsBackground = true;
            thread.Start();
            if (threaded)
            {
                Thread updateThread = new Thread(Update);
                updateThread.IsBackground = true;
                updateThread.Start();
            }
            int timer = 0;
            while (!connected && timer < Timeout)
            {
                Thread.Sleep(50);
                timer += 10;
            }

            return true;
        }

        public void Update()
        {
            while (!closeFlag)
            {
                while (messageQueue.Count > 0)
                {
                    string bytes = messageQueue.Dequeue().ToString().TrimEnd('|');
                    foreach (string s in bytes.Split('|'))
                    {
                        ParseMessage(s);    
                    }

                    messageQueue.Clear();
                }

                Thread.Sleep(10);
                timeSinceHeartbeat++;
                if (timeSinceHeartbeat > deathTime)
                {
                    timeSinceHeartbeat = 0;
                    ParseMessage("QUIT");
                }
            }
        }

        private void InitSockets()
        {
            ReceiveClient = new UdpClient();
            ReceiveClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            ReceiveClient.Client.Bind(new IPEndPoint(IPAddress.Loopback, MainPort));

            SendClient = new UdpClient();
            SendClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            SendClient.Client.Bind(new IPEndPoint(IPAddress.Loopback, EventPort));

            SendMessage("REGISTER " + MainPort.ToString() + " " + EventPort.ToString() + " " + GroupName + " " + ProcName);
        }

        private void ListenThread()
        {
            Console.WriteLine("Starting up listener on ports " + MainPort + " + " + EventPort);
            TriggerEndPoint = new IPEndPoint(IPAddress.Loopback, MainPort);

            while (!closeFlag)
            {
                try
                {
                    byte[] bytes = ReceiveClient.Receive(ref TriggerEndPoint);
                    string msg = Encoding.UTF8.GetString(bytes);
                    if (msg != "")
                        if(messageQueue.Count == 0)
                            messageQueue.Enqueue(msg);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Thread.Sleep(10);
            }
        }

        public void SendMessage(string msg)
        {
            if (verbose)
                Console.WriteLine("Sending " + msg);
            SendClient.Connect(Host, Port);
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            SendClient.Send(bytes, bytes.Length);
        }

        public void CloseSocket()
        {
            SendMessage("UNREGISTER " + MainPort + " " + GroupName);
            closeFlag = true;
            ReceiveClient.Close();
        }

        public void BindFunction(string trigger, NewMessageEvent func, SyncrParamType paramType = SyncrParamType.VOID, string[] enumTypes = null)
        {
            if (RegisteredFunctions.ContainsKey(trigger))
                RegisteredFunctions[trigger] += func;
            else
                RegisteredFunctions.Add(trigger, func);

            string eTypes = "";

            if (enumTypes != null)
                foreach (string s in enumTypes)
                    eTypes += s + " ";

            eTypes = eTypes.Trim();

            SendMessage("FUNCTION " + GroupName + " " + ProcName + " "  + trigger + " " + paramType.ToString() + " " + eTypes);
        }

        private void ParseMessage(string msg)
        {
          //  if (msg != "HEARTBEAT")
           //     Console.WriteLine("Received-" + msg + ".");
            string[] args = msg.Split(' ');
            //Console.WriteLine(msg);
            string root = args[0];
            string param = "";
            if (args.Length > 1)
            {
                for (int x = 1; x < args.Length; x++)
                    param += args[x] + " ";

            }
            param = param.Trim();
            if (args[0] == "CONNECTED")
            {
                connected = true;
                return;
            }

            if (args[0] == "HEARTBEAT")
            {
                timeSinceHeartbeat = 0;
                return;
            }

            if (args[0] == "QUIT")
                CloseSocket();

            if (RegisteredFunctions.ContainsKey(args[0]))
                RegisteredFunctions[args[0]](param);
            else
                Log("No bound function for trigger " + args[0]);

            //if (args.Length == 2)
            //{
            //if (RegisteredEvents.ContainsKey(args[0]))
            //RegisteredEvents[args[0]](double.Parse(args[1]));
            //}
        }

        private void Log(string Message)
        {
            if (verbose)
                Console.WriteLine("SYNCR: " + Message);
        }

        private int GetUniquePort()
        {
            int myport = r.Next(1024, 65000);
            while (!IsOpen(myport) && myport != MainPort && myport != EventPort)
                myport = r.Next(1024, 65000);

            return myport;
        }

        private bool IsOpen(int port)
        {
            return !((from p in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners() where p.Port == port select p).Count() == 1);
        }
    }
}
