namespace Configurations
{
    public class Handler_RichTextBox : IControlHandler 
    {
        public void AssignValueToControl(Control ctrl, string value)
        {
            ((RichTextBox)ctrl).Text = value;
        }
        public string GetControlValue(Control ctrl) => ((RichTextBox)ctrl).Text ?? "";
        public bool DoesMatchTo(Control ctrl) => ctrl is RichTextBox;
        public string GetControlNameWithoutPrefix(Control ctrl) => new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());
    }
}