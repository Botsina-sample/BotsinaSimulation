using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace BotsinaAgent.Actions
{
    class ComboBoxAction : AbsAction
    {

        public override void DoExecute()
        {
            AutomationElement a = UiElement.AutomationElement;
            Console.WriteLine(a.Current.AutomationId);

            ExpandCollapsePattern expandCollapsePattern = a.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            expandCollapsePattern.Expand();
            var comboBoxEditItemCondition = new System.Windows.Automation.PropertyCondition(AutomationElement.ClassNameProperty, "ListBoxItem");
            var listItems = a.FindAll(TreeScope.Subtree, comboBoxEditItemCondition);//It can only get one item in the list (the first one).
            var testItem = listItems[listItems.Count - 1];
         
            foreach (AutomationElement item in listItems)
            {
                if(item.Current.Name==Control.text)
                {
                    testItem = item;
                    break;
                }
            }
            (testItem.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Select();
            expandCollapsePattern.Collapse();
     

        }
    }
}
