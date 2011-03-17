using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Release 1.2.0
	/// </summary>
	public class StandaloneServerSetup120 : StandaloneServerSetup
	{
		public static new object Install(object obj)
		{
			return StandaloneServerSetup.InstallBase(obj, "1.2.0");
		}
	}

	/// <summary>
	/// Release 1.1.0
	/// </summary>
	public class StandaloneServerSetup110 : StandaloneServerSetup
	{
		public static new object Install(object obj)
		{
			return StandaloneServerSetup.InstallBase(obj, "1.1.0");
		}
	}

    /// <summary>
    /// Release 1.0.2
    /// </summary>
    public class StandaloneServerSetup102 : StandaloneServerSetup101
    {
		public static new object Install(object obj)
        {
            return StandaloneServerSetup.InstallBase(obj, "1.0.0");
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class StandaloneServerSetup101 : StandaloneServerSetup
    {
		public static new object Install(object obj)
        {
            return StandaloneServerSetup.InstallBase(obj, "1.0.0");
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class StandaloneServerSetup10 : StandaloneServerSetup
    {
        public static new object Install(object obj)
        {
            return StandaloneServerSetup.InstallBase(obj, "1.0.0");
        }
    }
}
