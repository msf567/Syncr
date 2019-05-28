using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Syncr
{
    public partial class SyncrCommandLine : Form
    {
        SyncrControlForm control;
        public SyncrCommandLine(SyncrControlForm f)
        {
            control = f;
            InitializeComponent();
        }

        private void SendCommand()
        {
            control.BroadcastMessage(commandLine.Text);
            commandLine.Text = "";
        }

        private void commandLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                SendCommand();
        }
    }
}
