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
    public partial class FunctionButtonString : FunctionButton
    {
        public FunctionButtonString()
        {
            InitializeComponent();
        }

        public override string GetParameterValue()
        {
            return parameterText.Text;
        }

        public override Control GetParameterControl()
        {
            return parameterText;
        }
    }
}
