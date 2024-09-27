namespace Configurations
{
    public class Handler_CheckBox : IControlHandler
    {
        public void AssignValueToControl(Control ctrl, string value)
        {
            value = value.Trim().ToLower();
            ((CheckBox)ctrl).Checked = value == "true";
        }
        public string GetControlValue(Control ctrl) => ((CheckBox)ctrl).Checked ? "true" : "false";
        public bool DoesMatchTo(Control ctrl) => ctrl is CheckBox;
        public string GetControlNameWithoutPrefix(Control ctrl) => new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());
    }
}