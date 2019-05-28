using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace MusicICP
{
    public class TCPReceive
    {
        internal bool socketReady = false;

        public delegate void NewMessageEvent();

        TcpClient mySocket;
        NetworkStream theStream;
        StreamWriter theWriter;
        StreamReader theReader;
        string Host = "localhost";
        Int32 Port = 3001;
        private bool closeFlag = false;

        private Dictionary<string, NewMessageEvent> RegisteredFunctions;

        public TCPReceive()
        {
            RegisteredFunctions = new Dictionary<string, NewMessageEvent>();
        }

        ~TCPReceive()
        {
            closeSocket();
        }

        public void Init()
        {
            setupSocket();
        }

        public void Update()
        {
            if (!socketReady)
                return;

            string msg = getMessageString();
            if (msg != "")
                ParseTrigger(msg);

            if (msg == "QUIT")
                closeFlag = true;

            if (closeFlag)
                closeSocket();
        }

        private void ListenThread()
        {
            while (!closeFlag)
            {
                string msg = getMessageString();
                if (msg != "")
                    ParseTrigger(msg);

                if (msg == "QUIT")
                    closeFlag = true;
            }

            closeSocket();
        }

        private void ParseTrigger(string t)
        {
            if (RegisteredFunctions.ContainsKey(t))
                RegisteredFunctions[t]();
            else
                Log("No bound function for trigger " + t);
        }

        public void BindFunction(string trigger, NewMessageEvent func)
        {
            if (RegisteredFunctions.ContainsKey(trigger))
                RegisteredFunctions[trigger] += func;
            else
                RegisteredFunctions.Add(trigger, func);
        }

        public float GetEventValue(string Event)
        {
            return 0.0f;
        }

        private void setupSocket()
        {
            try
            {
                mySocket = new TcpClient(Host, Port);
                mySocket.ReceiveTimeout = 50;
                theStream = mySocket.GetStream();
                theWriter = new StreamWriter(theStream);
                theReader = new StreamReader(theStream);
                socketReady = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket error: " + e);
            }
        }

        private void writeSocket(string theLine)
        {
            if (!socketReady)
                return;
            String foo = theLine + "\r\n";
            theWriter.Write(foo);
            theWriter.Flush();
        }

        private String getMessageString()
        {
            if (!socketReady)
                return "";
            if (theStream.DataAvailable)
                return theReader.ReadLine();
            return "";
        }

        private void closeSocket()
        {
            if (!socketReady)
                return;
            theWriter.Close();
            theReader.Close();
            mySocket.Close();
            socketReady = false;
        }

        private void Log(string Message)
        {
            Console.WriteLine("SYNCR: " + Message);
        }

    }
}
