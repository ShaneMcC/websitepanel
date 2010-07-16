using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
    /// <summary>
    /// Version 1.0.1
    /// </summary>
    public class Server101 : Server10
    {
        public static new DialogResult Install(object obj)
        {
            return Server10.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Server10.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Server10.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0", false);
        }
    }

    /// <summary>
    /// Version 1.0
    /// </summary>
    public class Server10 : Server
    {
        public static new DialogResult Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Server.Setup(obj);
        }
    }
}
