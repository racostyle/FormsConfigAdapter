namespace Configurations
{
    public class Handler_TextBox : IControlHandler 
    {
        public void AssignValueToControl(Control ctrl, string value)
        {
            ((TextBox)ctrl).Text = value;
        }
        public string GetControlValue(Control ctrl) => ((TextBox)ctrl).Text ?? "";
        public bool DoesMatchTo(Control ctrl) => ctrl is TextBox;
        public string GetControlNameWithoutPrefix(Control ctrl) => new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());
    }
}