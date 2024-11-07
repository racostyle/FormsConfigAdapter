using System.Windows.Forms;

namespace Configurations
{
    public interface IControlHandler
    {
        void AssignValueToControl(Control ctrl, string value);
        string GetControlValue(Control ctrl);
        bool DoesMatchTo(Control ctrl);
        string GetControlNameWithoutPrefix(Control ctrl);
    }
}
