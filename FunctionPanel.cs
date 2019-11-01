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
    public partial class FunctionPanel : UserControl
    {
        string procName;
        SyncrControlForm control;
        public FunctionPanel(string groupName, string proc, SyncrControlForm form)
        {
            InitializeComponent();
            groupNameLabel.Text = groupName;
            procName = proc;
            control = form;
    //        Console.WriteLine("adding panel for " + groupName);
        }

        // public FunctionPanel(string groupName)
        // {
        //      InitializeComponent();
        //  groupNameLabel.Text = groupName;
        //  }

        public void AddButton(FunctionButton button)
        {
            functionButtonPanel.Controls.Add(button);
        }

        public void RemoveButton(FunctionButton button)
        {
            if (functionButtonPanel.Controls.Contains(button))
            {
                functionButtonPanel.Controls.Remove(button);
            }
        }

        private void groupNameLabel_Click(object sender, EventArgs e)
        {
            control.runner.FlagProcessForRestart(procName);
        }
    }

}
