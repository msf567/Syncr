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
    public partial class FunctionButton : UserControl
    {
        public FunctionButton()
        {
            InitializeComponent();
        }

        public virtual string GetParameterValue()
        {
            return "";
        }

        public virtual Control GetParameterControl()
        {
            return null;
        }
    }
}
