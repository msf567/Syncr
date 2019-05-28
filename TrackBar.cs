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
    public partial class TrackBar : Form
    {
        SyncrPlayer sync;

        public TrackBar(SyncrPlayer s)
        {
            InitializeComponent();
            sync = s;
        }

        delegate void VoidDel();
        public void UpdateProg()
        {
            if (trackProg.InvokeRequired)
            {
                VoidDel d = new VoidDel(UpdateProg);
                this.Invoke(d, new object[] { });
            }
            else
            {
                float pos = (float)sync.Music.Position / (float)sync.Music.Length;
                trackProg.Value = (int)((float)trackProg.Maximum * pos);
            }
        }

        private void TrackBar_Load(object sender, EventArgs e)
        {

        }

        private void trackProg_Scroll(object sender, EventArgs e)
        {
            float pos = (float)trackProg.Value / (float)trackProg.Maximum;

            sync.SeekScore(pos);
        }

        private void trackProg_MouseDown(object sender, MouseEventArgs e)
        {
            float pos = (float)e.X / (float)Width;
            Console.WriteLine(pos);
            sync.SeekScore(pos);
            trackProg.Value = (int)(trackProg.Maximum * pos);
            trackProg.Refresh();
        }
    }
}
