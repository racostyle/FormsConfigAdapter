using FormsUtils.Configurations;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Configurations
{
    public class ConfigurationAdapter
    {
        public bool SavedByAdapter { get; private set; } = false;

        private readonly IControlHandler[] _acceptedHandlers;

        /// <summary>
        /// naming convention prefixes: tb - textbox, rtb - RichTextBox, chb - CheckBox, cbb - ComboBox, rbtn - RadioButton
        /// Example: rtbTextBox will come out from GetControlNameWithoutPrefix as TextBox
        /// </summary>
        public ConfigurationAdapter(bool respectNamingConvention, params IControlHandler[] acceptedHandlers)
        {
            ControlChecker.Shared.RespectNamingConvention = respectNamingConvention;
            _acceptedHandlers = acceptedHandlers;
        }

        /// <summary>
        /// Packs control values from the given parent control into a dictionary.
        /// </summary>
        public Dictionary<string, string> PackControls(Control parent)
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

        /// <summary>
        /// Unpacks and assigns values from the dictionary to controls within the given parent control.
        /// </summary>
        public void UnpackControls(Control parent, Dictionary<string, string> config)
        {
            if (config == null)
                return;

            SavedByAdapter = true;

            if (parent.InvokeRequired)
            {
                parent.BeginInvoke((MethodInvoker)delegate ()
                {
                    UnpackControls(parent, config);
                });
            }

            var recognized = GetAllRecognizedControls(parent);

            foreach (var cfg in config)
            {
                foreach (Control control in recognized)
                {
                    var handler = GetControlHandler(control).Handler;
                    var name = handler.GetControlNameWithoutPrefix(control);
                    if (cfg.Key == name)
                        handler.AssignValueToControl(control, cfg.Value);
                }
            }

            SavedByAdapter = false;
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
