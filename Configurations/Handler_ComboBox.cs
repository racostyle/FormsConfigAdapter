namespace Configurations
{
    public class Handler_ComboBox : IControlHandler
    {
        public void AssignValueToControl(Control ctrl, string value)
        {
            ((ComboBox)ctrl).Text = value;
        }
        public string GetControlValue(Control ctrl) => ((ComboBox)ctrl).Text ?? "";
        public string GetControlNameWithoutPrefix(Control ctrl) => new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());

        public bool DoesMatchTo(Control ctrl)
        {
            if (ctrl is ComboBox)
            {
                if (ctrl.Name.StartsWith("cbb"))
                    return true;
            }
            return false;
        }
    }
}