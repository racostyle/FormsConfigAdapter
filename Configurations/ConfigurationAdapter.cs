namespace Configurations
{
    internal class ConfigurationAdapter
    {
        private readonly IControlHandler[] _acceptedHandlers;

        /// <summary>
        /// naming convention prefixes: tb - textbox, rtb - RichTextBox, cbx - CheckBox, cbb - ComboBox
        /// Example: rtbTextBox will come out form GetControlNameWithoutPrefix as TextBox
        /// </summary>
        public ConfigurationAdapter(params IControlHandler[] acceptedHandlers)
        {
            _acceptedHandlers = acceptedHandlers;
        }

        internal Dictionary<string, string> PackControls(Control parent)
        {
            var recognized = GetAllRecognizedControls(parent);
            var dict = new Dictionary<string, string>();

            foreach (Control rec in recognized)
            {
                var selected = GetControlHandler(rec);
                dict.Add(selected.Handler.GetControlNameWithoutPrefix(rec), selected.Handler.GetControlValue(rec));
            }
            return dict;
        }

        internal void UnpackControls(Control parent, Dictionary<string, string> config)
        {
            if (config == null)
                return;

            var recognized = GetAllRecognizedControls(parent);

            foreach (var cfg in config)
            {
                foreach (Control control in recognized)
                {
                    if (cfg.Key == GetControlHandler(control).Handler.GetControlNameWithoutPrefix(control))
                        control.Text = cfg.Value;
                }
            }
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;

                if (control.HasChildren)
                {
                    foreach (Control childControl in GetAllControls(control))
                        yield return childControl;
                }
            }
        }

        private List<Control> GetAllRecognizedControls(Control parent)
        {
            var controls = GetAllControls(parent);
            var recognized = new List<Control>();

            foreach (Control ctrl in controls)
            {
                var result = GetControlHandler(ctrl);
                if (result.Result)
                    recognized.Add(ctrl);
            }
            return recognized;
        }

        private (bool Result, IControlHandler Handler) GetControlHandler(Control control)
        {
            foreach (var handler in _acceptedHandlers)
            {
                if (handler.DoesMatchTo(control))
                    return (Result: true, Handler: handler);
            }
            return (Result: false, Handler: null);
        }
    }
}