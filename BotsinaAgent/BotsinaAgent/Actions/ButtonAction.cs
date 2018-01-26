using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BotsinaAgent.Actions
{
    class ButtonAction : AbsAction
    {
        public override void DoExecute()
        {
            switch (Control.action)
            {
                case "Click":
                        UiElement.AsButton().Click();
                break;
                case "DoubleClick":
                    UiElement.AsButton().DoubleClick();
                    break;
                default: Thread.Sleep(2000);
                    break;
            }
        }
    }
}
