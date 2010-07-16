using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
    /// <summary>
    /// Version 1.0.1
    /// </summary>
    public class Portal101 : Portal10
    {
        public static new DialogResult Install(object obj)
        {
            return Portal10.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Portal10.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Portal10.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0.0", false);
        }
    }

    /// <summary>
    /// Version 1.0
    /// </summary>
    public class Portal10 : Portal
    {
        public static new DialogResult Install(object obj)
        {
            return Portal.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Portal.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Portal.Setup(obj);
        }
    }
}
