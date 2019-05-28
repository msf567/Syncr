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
    public partial class FunctionButtonEnum : FunctionButton
    {
        public FunctionButtonEnum()
        {
            InitializeComponent();
        }

        public FunctionButtonEnum(string[] enumTypes)
        {
            InitializeComponent();
            comboBox1.DataSource = enumTypes;
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
