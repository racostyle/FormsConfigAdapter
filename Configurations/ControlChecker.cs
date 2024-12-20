using System.Windows.Forms;

namespace FormsUtils.Configurations
{
    internal class ControlChecker
    {
        internal static readonly ControlChecker Shared = new ControlChecker();
        internal bool RespectNamingConvention = false;

        internal bool Check<T>(Control control, T type, string prefix)
        {
            if (RespectNamingConvention)
            {
                if (control.Name.StartsWith(prefix))
                    return true;
            }
            else
            {
                if (control is T)
                    return true;
            }
            return false;
        }
    }
}
