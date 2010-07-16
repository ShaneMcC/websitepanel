using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
    /// <summary>
    /// Version 1.0.1
    /// </summary>
    public class EnterpriseServer101 : EnterpriseServer10
    {
        public static new DialogResult Install(object obj)
        {
            return EnterpriseServer10.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer10.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer10.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0.0", true);
        }
    }

    /// <summary>
    /// Version 1.0
    /// </summary>
    public class EnterpriseServer10 : EnterpriseServer
    {
        public static new DialogResult Install(object obj)
        {
            return EnterpriseServer.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer.Setup(obj);
        }
    }
}
