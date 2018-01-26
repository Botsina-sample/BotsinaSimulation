using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gu.Wpf;
using Gu.Wpf.UiAutomation;

namespace BotsinaAgent.Actions
{
    abstract class AbsAction
    {
        public Control Control {get; set;}
        public UiElement UiElement;
        public bool result;
        public abstract void DoExecute();
    }
}
