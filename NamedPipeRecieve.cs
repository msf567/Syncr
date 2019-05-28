using System;
using NamedPipeLib;
using System.Diagnostics;
using System.Collections.Generic;
namespace MusicICP
{
    public class NamedPipeRecieve
    {
        private NamedPipeReceiver messageReceiver;
        private NamedPipeSender messageSender;

        Random r;

        private string RECEIVERPIPENAME = "ShowServer";
        private static string SENDERPIPENAME { get { return "ClientPipe" + Process.GetCurrentProcess().Id.ToString(); } }

        public delegate void NewMessageEvent();

        private Dictionary<string, NewMessageEvent> RegisteredFunctions;

        public NamedPipeRecieve()
        {
            r = new Random();

            RECEIVERPIPENAME = RECEIVERPIPENAME + r.Next(0, 10).ToString();

            RegisteredFunctions = new Dictionary<string, NewMessageEvent>();
        }

        ~NamedPipeRecieve()
        {
            messageReceiver.Dispose();
            messageSender.Dispose();
        }

        public void Init()
        {
            RefreshReceiver();

            Log("Connecting to " + RECEIVERPIPENAME);
            messageSender = new NamedPipeSender(RECEIVERPIPENAME);
            Log("Created Sender");
            string request = "REGISTER " + Process.GetCurrentProcess().Id;
            var response = messageSender.SendRequest(request);
            Log("Response: " + response);
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
            string request = "EVENT " + Event;
            var response = messageSender.SendRequest(request);

            float res = 0.0f;
            if (float.TryParse(response, out res))
                return res;
            else
                return 0.0f;
        }

        private void OnNewMessage(object sender, PipeMsgEventArgs e)
        {
            ParseTrigger(e.Request);
            e.Response = "";
        }

        private void ParseTrigger(string s)
        {
            if (RegisteredFunctions.ContainsKey(s))
                RegisteredFunctions[s]();
            else
                Log("No bound function for trigger " + s);
        }

        private void RefreshReceiver()
        {
            if (messageReceiver != null)
                messageReceiver.Dispose();

            messageReceiver = new NamedPipeReceiver(SENDERPIPENAME);
            Log("Opening " + SENDERPIPENAME);
            messageReceiver.newRequestEvent += OnNewMessage;
        }

        private void Log(string Message)
        {
            Console.WriteLine("SYNCR: " + Message);
        }

    }
}
