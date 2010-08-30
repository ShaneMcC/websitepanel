using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
    /// <summary>
    /// Version 1.0.2
    /// </summary>
    public class StandaloneServerSetup102 : StandaloneServerSetup101
    {
        public static new DialogResult Install(object obj)
        {
            return StandaloneServerSetup.InstallBase(obj, "1.0.0");
        }
    }

    /// <summary>
    /// Version 1.0.1
    /// </summary>
    public class StandaloneServerSetup101 : StandaloneServerSetup
    {
        public static new DialogResult Install(object obj)
        {
            return StandaloneServerSetup.InstallBase(obj, "1.0.0");
        }
    }

    /// <summary>
    /// Version 1.0
    /// </summary>
    public class StandaloneServerSetup10 : StandaloneServerSetup
    {
        public static new DialogResult Install(object obj)
        {
            return StandaloneServerSetup.InstallBase(obj, "1.0.0");
        }
    }
}
