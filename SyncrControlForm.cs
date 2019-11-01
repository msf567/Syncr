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
    public delegate void ProcessLoadDelagete();

    public partial class SyncrControlForm : Form
    {
        bool isClosing = false;

        Syncr.TrackBar trackBar;
        private Dictionary<string, FunctionPanel> functionPanels = new Dictionary<string, FunctionPanel>();
        bool ShowTrackBar = true;

        public MidiManager midi;
        public SilentRunner runner;
        SyncrCommandLine cmd;

        public SyncrControlForm(Score s, ProcessLoadDelagete processDelegate, bool autoStart = false, bool trackBar = true)
        {
            InitializeComponent();

            runner = new SilentRunner(s, processDelegate, autoStart);
            runner.OnUpdate += Update;
            runner.OnTempo += OnTempoTick;

            cmd = new SyncrCommandLine(this);
            Text = s.TrackName;
            ShowTrackBar = trackBar;
            midi = new MidiManager(this);
            // StreamReader r = new StreamReader("score.txt");

        }

        public SyncrControlForm(Score s, SilentRunner r, ProcessLoadDelagete processDelegate, bool autoStart = false, bool trackBar = true)
        {
            InitializeComponent();

            runner = r;
            runner.OnUpdate += Update;
            runner.OnTempo += OnTempoTick;

            cmd = new SyncrCommandLine(this);
            Text = s.TrackName;
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
            runner.Init();
            runner.udpHost.OnRegisterFunction += AddButton;

            if (ShowTrackBar)
            {
                trackBar = new Syncr.TrackBar(runner.syncr);
                trackBar.Show();
            }

            Visible = false;
        }

        public void BroadcastMessage(string msg)
        {
            runner.BroadcastMessage(msg);
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

        private delegate void ButtonSet(string triggerName, SyncrParamType type, string enumTypes);

        private void AddButton(string triggerName, string procName, SyncrFunctionType funcType)
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
                    if (runner.syncr.BoundFunctions.ContainsKey(func))
                        runner.syncr.BoundFunctions[func](b.GetParameterValue());
                    runner.BroadcastMessage(func + " " + b.GetParameterValue());
                });

                if (b.GetParameterControl() != null && funcType.type != SyncrParamType.STRING)
                    b.GetParameterControl().TextChanged += new EventHandler(delegate (Object o, EventArgs a)
                    {
                        if (runner.syncr.BoundFunctions.ContainsKey(func))
                            runner.syncr.BoundFunctions[func](b.GetParameterValue());
                        runner.BroadcastMessage(func + " " + b.GetParameterValue());
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

            if (runner.syncr.IsPlaying && trackBar != null)
                trackBar.UpdateProg();

        }

        void OnTempoTick()
        {
            tapButton.BackColor = tapButton.BackColor != Color.MediumSeaGreen ? Color.MediumSeaGreen : Color.Crimson;
        }

        void CloseShow(string param = "")
        {
            isClosing = true;
            runner.Close();
        }

        private void ShowForm_FormClosed(object sender, FormClosingEventArgs e)
        {
            runner.Close();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            runner.Stop();
        }

        private void tapClick(object sender, EventArgs e)
        {
            runner.Tap();
        }

        private void play_Click(object sender, EventArgs e)
        {
            runner.syncr.SoundOut.Volume = (float)volumeBar.Value / 100.0f;
            runner.Play();
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            runner.BroadcastMessage("NS-COLORCONTROL");
        }

        private void volumeBar_Scroll(object sender, EventArgs e)
        {
            runner.syncr.SoundOut.Volume = (float)volumeBar.Value / 100.0f;
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

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

