using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Syncr;
using ExpanderApp;
namespace Syncr
{
    struct WindowPage
    {
        public FlowLayoutPanel panel;
        public TabPage page;
        public string group;
        public WindowPage(TabPage _page, FlowLayoutPanel _panel, string _group)
        {
            panel = _panel;
            page = _page;
            group = _group;
        }
    }

    public partial class WindowManager : Form
    {
        List<Window> CurrentWindows = new List<Window>();
        Dictionary<string, WindowPage> pages = new Dictionary<string, WindowPage>();
        public WindowManager()
        {
            InitializeComponent();
            UpdateTimer.Start();
        }

        public void SetWindowGroup(Window w ,string group)
        {
            w.group = group;
        }

        private void Update(object sender, EventArgs e)
        {
            List<Window> NewWindows = new List<Window>(Win32.GetDesktopWindowsTitles());

            foreach (Window w in NewWindows.Except(CurrentWindows).ToList())
            {
                WindowBar b = new WindowBar(w, this);
                Expander ex = BuildExpander(b);
                ex.Name = w.name;

                if (!pages.ContainsKey(w.group))
                    AddPage(w.group);

                pages[w.group].panel.Controls.Add(ex);
            }

            foreach (Window w in CurrentWindows.Except(NewWindows).ToList())
                pages[w.group].panel.Controls.RemoveByKey(w.name);

            foreach (string wp in pages.Keys)
                if (pages[wp].panel.Controls.Count == 0)
                    pages.Remove(wp);

            CurrentWindows = NewWindows;
        }

        private void AddPage (string groupName)
        {
            TabPage page = new TabPage(groupName);
            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            page.Controls.Add(panel);

            WindowPage winPage = new WindowPage(page, panel, groupName);
            pages.Add(groupName,winPage);
            tabControl.Controls.Add(winPage.page);
        }

        private Expander BuildExpander(WindowBar b)
        {
            Expander expander = new Expander();

            expander.Size = new Size(250, 400);
            expander.Left = 10;
            expander.Top = 10;
            expander.BorderStyle = BorderStyle.FixedSingle;

            ExpanderHelper.CreateLabelHeader(expander, b.window.handle.ToString() + " -- " +  b.window.name, backColor: SystemColors.ActiveBorder);

            Label labelContent = new Label();
            labelContent.Size = new System.Drawing.Size(expander.Width, 80);
            expander.Content = b;
            expander.Toggle();
            return expander;
           // this.Controls.Add(expander);
        }
    }
}
