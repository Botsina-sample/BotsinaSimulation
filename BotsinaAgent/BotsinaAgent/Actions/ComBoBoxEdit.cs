using System;
using System.Windows.Automation;
namespace BotsinaAgent.Actions
{
    class ComBoBoxEdit:AbsAction
    {
        public override void DoExecute()
        {

            AutomationElement a = UiElement.AutomationElement;
            Console.WriteLine(a.Current.AutomationId);

            ExpandCollapsePattern expandCollapsePattern = a.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            expandCollapsePattern.Expand();
            var comboBoxEditItemCondition = new System.Windows.Automation.PropertyCondition(AutomationElement.ClassNameProperty, "ComboBoxEditItem");
            var listItems = a.FindAll(TreeScope.Subtree, comboBoxEditItemCondition);//It can only get one item in the list (the first one).
            var testItem = listItems[listItems.Count - 1];
            int index1 = 0;
            int index = 0;
            foreach (AutomationElement item in listItems)
            {
                foreach (AutomationElement itemChild in item.FindAll(TreeScope.Subtree, new System.Windows.Automation.PropertyCondition(AutomationElement.ClassNameProperty, "TextBlock")))
                {
                    if(itemChild.Current.Name==Control.text)
                    {
                        //testItem=
                    }
                }
              
                
                index++;
            }
         
            (testItem.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Select();
            expandCollapsePattern.Collapse();
        }
    }
}
