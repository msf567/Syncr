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
    public partial class FunctionButtonBool : FunctionButton
    {
        public FunctionButtonBool()
        {
            InitializeComponent();
        }

        public FunctionButtonBool(string[] enumTypes)
        {
            InitializeComponent();
            comboBox1.DataSource = new string[2] { "TRUE", "FALSE" };
            comboBox1.SelectedIndex = 0;
        }

        public override string GetParameterValue()
        {
            return comboBox1.Text;
        }

        public override Control GetParameterControl()
        {
            return comboBox1;
        }
    }
}
