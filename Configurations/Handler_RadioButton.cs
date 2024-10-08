﻿namespace Configurations
{
    public class Handler_RadioButton : IControlHandler 
    {
        public void AssignValueToControl(Control ctrl, string value)
        {
            value = value.Trim().ToLower();
            ((RadioButton)ctrl).Checked = value == "true";
        }
        public string GetControlValue(Control ctrl) => ((RadioButton)ctrl).Checked ? "true" : "false";
        public bool DoesMatchTo(Control ctrl) => ctrl is RadioButton;
        public string GetControlNameWithoutPrefix(Control ctrl) => new string(ctrl.Name.ToArray().SkipWhile(x => char.IsLower(x)).ToArray());
    }
}