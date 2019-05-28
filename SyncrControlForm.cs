using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
namespace Syncr
{
    public partial class SyncrControlForm : Form
    {
        public SyncrPlayer syncr;
        Score score;
        FastLoop _fastLoop;
        Tempo tempo;
        List<Trigger> triggers;
        List<string> restartProcs = new List<string>();
        bool isClosing = false;
        public SyncrUDPHost udpHost;
        Syncr.TrackBar trackBar;
        private Dictionary<string, FunctionPanel> functionPanels = new Dictionary<string, FunctionPanel>();
        static List<string> StartedProcs = new List<string>();
        public delegate void ProcessLoadDelagete();
        public ProcessLoadDelagete ProcessDelegate;
        bool AutoStart = false;
        bool ShowTrackBar = true;

        SyncrCommandLine cmd;
        public MidiManager midi;
        public SyncrControlForm(Score s, ProcessLoadDelagete processDelegate, bool autoStart = false, bool trackBar = true)
        {
            InitializeComponent();
            ProcessDelegate = processDelegate;
            syncr = new SyncrPlayer();
            tempo = new Tempo();
            triggers = new List<Trigger>();
            udpHost = new SyncrUDPHost(syncr);
            _fastLoop = new FastLoop(Update);
            cmd = new SyncrCommandLine(this);
            Text = s.TrackName;
            score = s;
            AutoStart = autoStart;
            ShowTrackBar = trackBar;
            midi = new MidiManager(this);
            // StreamReader r = new StreamReader("score.txt");
        }

        public void SetIcon(Icon icon)
        {
            Icon = icon;
        }

        public void LoadMidiMap(SyncrMidiMap map)
        {
            midi.SetMidiMap(map);
        }

        private void ShowForm_Load(object sender, EventArgs e)
        {
            udpHost.OnRegisterFunction += AddButton;
            udpHost.verbose = true;
            syncr.LoadScore(score);
            syncr.BindFunctionToTrigger("TAP", new SyncrPlayer.TriggerFunction(Tap));
            syncr.BindFunctionToTrigger("STARTPROCESS", new SyncrPlayer.TriggerFunction((p) => { Process.Start(p); }));
            syncr.BindFunctionToTrigger("QUIT", new SyncrPlayer.TriggerFunction(CloseShow));
            Console.WriteLine("processdelegate");
            if(ShowTrackBar)
            {
                trackBar = new Syncr.TrackBar(syncr);
                trackBar.Show();
            }

            tempo.OnTempoTick += TempoTick;

            if (ProcessDelegate != null)
                ProcessDelegate();

            Visible = false;

            if (AutoStart)
            {
                WindowState = FormWindowState.Minimized;
                syncr.SoundOut.Volume = 1.0f;
                syncr.StartCurrentScore();
            }
        }

        private void ShowCommandLine()
        {
            cmd.Show();
            cmd.Focus();

//             Thread winManThread = new Thread(() =>
//             {
//                 var winMan = new WindowManager();
//                 Application.Run(winMan);
//             });
// 
//             winManThread.SetApartmentState(ApartmentState.STA);
//             winManThread.Start();
        }

        private void HideCommandLine()
        {
            cmd.Hide();
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
            if(srPRocs.Length > 0)
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

            if(!StartedProcs.Contains(fileName))
            StartedProcs.Add(fileName);
        }

        private delegate void ButtonSet(string triggerName, SyncrParamType type, string enumTypes);

        private void AddButton(string triggerName,string procName, SyncrFunctionType funcType)
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                SyncrUDPHost.FuncRegister d = new SyncrUDPHost.FuncRegister(AddButton);
                this.Invoke(d, new object[] { triggerName, procName, funcType });
            }
            else
            {
               // Console.WriteLine("Adding button: " + triggerName);
                FunctionButton b = FunctionButtonFactory.CreateFunctionButton(funcType.type);

                if (b is FunctionButtonEnum)
                {
                    b = new FunctionButtonEnum(funcType.enumChoices);
                }

                string func = triggerName;
                b.button.Text = func;
                b.button.Click += new EventHandler(delegate (Object o, EventArgs a)
                {
                    if (syncr.BoundFunctions.ContainsKey(func))
                        syncr.BoundFunctions[func](b.GetParameterValue());
                    BroadcastMessage(func + " " + b.GetParameterValue());
                });

                if (b.GetParameterControl() != null && funcType.type != SyncrParamType.STRING)
                    b.GetParameterControl().TextChanged += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        if (syncr.BoundFunctions.ContainsKey(func))
                            syncr.BoundFunctions[func](b.GetParameterValue());
                        BroadcastMessage(func + " " + b.GetParameterValue());
                    });


                if (!functionPanels.Keys.Contains(funcType.group))
                {
                   // Console.WriteLine("Adding button for " + procName);
                    FunctionPanel newPanel = new FunctionPanel(funcType.group, procName, this);
                    functionPanels.Add(funcType.group, newPanel);
                    flowLayoutPanel1.Controls.Add(newPanel);
                }

                functionPanels[funcType.group].AddButton(b);
                //flowLayoutPanel1.Controls.Add(b);
            }
        }

        private void Update(double elapsedTime)
        {
            if (isClosing)
                return;

            syncr.Update((float)elapsedTime);

            if (syncr.IsPlaying && trackBar != null)
                trackBar.UpdateProg();

            triggers.Clear();
            triggers = syncr.GetCurrentTriggers();

            foreach (Trigger t in triggers)
                BroadcastMessage(t.name + " " + t.param);

            //  if (syncr.IsPlaying)
            //  {
            tempo.Update(elapsedTime);
            //  }

            foreach (string p in restartProcs)
                RestartClientProcess(p);
            restartProcs.Clear();
        }

        private void Tap(string b)
        {
            tempo.Tap();
        }

        private void TempoTick(int beatNum)
        {
            BroadcastMessage("TAP");
            tapButton.BackColor = tapButton.BackColor != Color.MediumSeaGreen ? Color.MediumSeaGreen : Color.Crimson;
        }

        public void BroadcastMessage(string msg)
        {
            udpHost.BroadcastMessage(msg);
        }

        void CloseShow(string param = "")
        {
            isClosing = true;
            syncr.StopScore();
            BroadcastMessage("QUIT");
            Console.WriteLine("quitting");
            Thread.Sleep(100);
          //  foreach (string s in StartedProcs)
           //     KillProcess(s);
           
            udpHost.CloseSocket();
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
            //close current process
            //clear udp ports
            //restart program
        }

        private void KillProcess(string procName)
        {
            Process[] procs = Process.GetProcessesByName(procName);
            for (int x = 0; x < procs.Length; x++)
                procs[x].Kill();
        }

        private void ShowForm_FormClosed(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("STOP");
            CloseShow();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            syncr.PauseScore();
            BroadcastMessage("STOP");
        }

        private void tapClick(object sender, EventArgs e)
        {
            tempo.Tap();
            BroadcastMessage("TAP");
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (syncr.IsPlaying)
                return;

            syncr.StartCurrentScore();

            syncr.SoundOut.Volume = (float)volumeBar.Value / 100.0f;
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            BroadcastMessage("NS-COLORCONTROL");
        }

        void ExtractResource(string resource, string path)
        {
            Stream stream = GetType().Assembly.GetManifestResourceStream(resource);
            byte[] bytes = new byte[(int)stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            File.WriteAllBytes(path, bytes);
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            syncr.SoundOut.Volume = (float)volumeBar.Value / 100.0f;
        }

        private void cmdButton_Click(object sender, EventArgs e)
        {
            if (cmd.Visible)
                HideCommandLine();
            else
                ShowCommandLine();
        }

        private void midiEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (midiEnabled.Checked)
                midi.Play();
            else
                midi.Stop();
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
