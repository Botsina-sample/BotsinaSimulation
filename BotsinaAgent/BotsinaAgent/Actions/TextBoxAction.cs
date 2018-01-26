using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace BotsinaAgent.Actions
{
    class TextBoxAction : AbsAction
    {
        public override void DoExecute()
        {
            switch (Control.action)
            {
                case "SetText":
                    try
                    {
                        UiElement.AsTextBox().Enter(Control.text);
                        result = true;
                    }
                    catch (Exception )
                    {
                        MessageBox.Show("Can not execute this action!");
                        result = false;
                        throw;
                    }
                    break;
                default:
                    Thread.Sleep(2000);
                    break;
            }
        }
    }
}
