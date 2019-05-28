using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Syncr
{
    public partial class WindowBar : UserControl
    {
        public Window window;
        public WindowManager manager;

        public WindowBar()
        {
            InitializeComponent();
        }

        public WindowBar(Window w, WindowManager m)
        {
            window = w;
            manager = m;
            InitializeComponent();
        }

        private void topButton_Click(object sender, EventArgs e)
        {
            Win32.SetWindowTopMost(window.handle);
        }

        private void bottomButton_Click(object sender, EventArgs e)
        {
            Win32.SetWindowBottom(window.handle);
        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Win32.CloseWindow(window.handle);
        }
    }
}
