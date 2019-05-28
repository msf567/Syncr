using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Syncr
{
    public class FunctionButtonFactory
    {
        public static FunctionButton CreateFunctionButton(SyncrParamType t)
        {
            switch (t)
            {
                case SyncrParamType.VOID:
                    return new FunctionButton();
                case SyncrParamType.STRING:
                    return new FunctionButtonString();
                case SyncrParamType.INT:
                    return new FunctionButtonInt();
                case SyncrParamType.FLOAT:
                    return new FunctionButtonFloat();
                case SyncrParamType.ENUM:
                    return new FunctionButtonEnum();
                case SyncrParamType.BOOL:
                    return new FunctionButtonBool();
                default:
                    return new FunctionButtonString();
            }
        }
    }
}
