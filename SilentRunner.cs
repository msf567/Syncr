using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Syncr
{
    public class SilentRunner
    {
        public SyncrPlayer syncr;
        Score score;
        FastLoop _fastLoop;
        Tempo tempo;
        List<Trigger> triggers;
        List<string> restartProcs = new List<string>();
        public SyncrUDPHost udpHost;
        static List<string> StartedProcs = new List<string>();
        public ProcessLoadDelagete ProcessDelegate;
        bool AutoStart = false;
        public delegate void UpdateCallback(double e);
        public UpdateCallback OnUpdate;
        public delegate void TempoTickCallback();
        public TempoTickCallback OnTempo;

        public SilentRunner(Score s, ProcessLoadDelagete processDelegate, bool autoStart)
        {
            ProcessDelegate = processDelegate;
            syncr = new SyncrPlayer();
            tempo = new Tempo();
            triggers = new List<Trigger>();
            udpHost = new SyncrUDPHost(syncr);
            _fastLoop = new FastLoop(Update);
            score = s;
            AutoStart = autoStart;
        }

        public void Init()
        {
            udpHost.verbose = true;
            syncr.LoadScore(score);
            syncr.BindFunctionToTrigger("TAP", new SyncrPlayer.TriggerFunction(Tap));
            syncr.BindFunctionToTrigger("STARTPROCESS", new SyncrPlayer.TriggerFunction((p) => { Process.Start(p); }));
            syncr.BindFunctionToTrigger("QUIT", new SyncrPlayer.TriggerFunction(Close));
            tempo.OnTempoTick += TempoTick;

            if (ProcessDelegate != null)
                ProcessDelegate();

            if (AutoStart)
            {
                syncr.SoundOut.Volume = 1.0f;
                syncr.StartCurrentScore();
            }

            _fastLoop.Start();
        }

        public static void StartProcess(string fileName, bool debugMode = false, string arg = "")
        {
            Console.WriteLine("Attempting to start " + fileName);

            if (!File.Exists(fileName + ".exe"))
            {
                Console.WriteLine("No program found for " + fileName);
                return;
            }

            Process[] srPRocs = Process.GetProcessesByName(fileName);
            if (srPRocs.Length > 0)
                for (int x = 0; x < srPRocs.Length; x++)
                    srPRocs[x].Kill();

            var sfPRoc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arg,
                    UseShellExecute = debugMode,
                    RedirectStandardOutput = !debugMode,
                    CreateNoWindow = !debugMode,
                    Verb = "runas"
                }
            };
            sfPRoc.Start();

            if (!StartedProcs.Contains(fileName))
                StartedProcs.Add(fileName);
        }

        private void Update(double elapsedTime)
        {
            syncr.Update((float)elapsedTime);

            triggers.Clear();
            triggers = syncr.GetCurrentTriggers();

            foreach (Trigger t in triggers)
            {
                Console.WriteLine("going off trigger");
                BroadcastMessage(t.name + " " + t.param);
            }

            //  if (syncr.IsPlaying)
            //  {
            tempo.Update(elapsedTime);
            //  }

            foreach (string p in restartProcs)
                RestartClientProcess(p);
            restartProcs.Clear();

            if (OnUpdate != null)
                OnUpdate(elapsedTime);
        }

        private void Tap(string b)
        {
            if (OnTempo != null)
                OnTempo();
            tempo.Tap();
        }

        private void TempoTick(int beatNum)
        {
            if (OnTempo != null)
                OnTempo();
            BroadcastMessage("TAP");
        }

        public void BroadcastMessage(string msg)
        {
            udpHost.BroadcastMessage(msg);
        }

        public void Close(string param = "")
        {
            syncr.StopScore();
            BroadcastMessage("QUIT");
            Console.WriteLine("quitting");
            Thread.Sleep(100);
            //  foreach (string s in StartedProcs)
            //     KillProcess(s);

            udpHost.CloseSocket();
            _fastLoop.Close();
        }

        public void FlagProcessForRestart(string procName)
        {
            restartProcs.Add(procName);
        }

        public void RestartClientProcess(string procName)
        {
            Console.WriteLine("Restarting " + procName);
            //KillProcess(procName);
            udpHost.UnRegisterClientConnection(procName);
            StartProcess(procName);
        }

        private void KillProcess(string procName)
        {
            Process[] procs = Process.GetProcessesByName(procName);
            for (int x = 0; x < procs.Length; x++)
                procs[x].Kill();
        }

        public void Play()
        {
            if (syncr.IsPlaying)
                return;

            syncr.StartCurrentScore();

        }

        public void Tap()
        {
            tempo.Tap();
            BroadcastMessage("TAP");
        }

        public void Stop()
        {
            syncr.PauseScore();
            BroadcastMessage("STOP");
        }

        void ExtractResourceToPath(string resource, string path)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(path, bytes);
        }


    }
}


//deploy payload (alternate version)
/*
var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
var complete = Path.Combine(systemPath, "Show");

string path = Path.Combine(complete, "ScreenFlasher.exe");
System.IO.Directory.CreateDirectory(complete);
File.WriteAllBytes(path, Properties.Resources.ScreenFlasher);
Console.WriteLine("Starting Process " + path);
*/
