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
    public partial class FunctionButtonInt : FunctionButton
    {
        public FunctionButtonInt()
        {
            InitializeComponent();
        }

        public override string GetParameterValue()
        {
            return numericUpDown1.Value.ToString();
        }

        public override Control GetParameterControl()
        {
            return numericUpDown1;
        }

    }
}
