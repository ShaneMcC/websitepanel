using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Release 1.1.1
	/// </summary>
	public class Server111 : Server
	{
		public static new object Install(object obj)
		{
			return Server.InstallBase(obj, "1.1.0");
		}

		public static new object Uninstall(object obj)
		{
			return Server.Uninstall(obj);
		}

		public static new object Setup(object obj)
		{
			return Server.Setup(obj);
		}

		public static new object Update(object obj)
		{
			return UpdateBase(obj, "1.1.0", "1.1.0", false);
		}
	}
	/// <summary>
	/// Release 1.1.0
	/// </summary>
	public class Server110 : Server
	{
		public static new object Install(object obj)
		{
			return Server.InstallBase(obj, "1.1.0");
		}

		public static new object Uninstall(object obj)
		{
			return Server.Uninstall(obj);
		}

		public static new object Setup(object obj)
		{
			return Server.Setup(obj);
		}

		public static new object Update(object obj)
		{
			return UpdateBase(obj, "1.1.0", "1.0.2", false);
		}
	}
    /// Release 1.0.2
    /// </summary>
    public class Server102 : Server101
    {
		public static new object Install(object obj)
        {
            return Server101.InstallBase(obj, "1.0.0");
        }

		public static new object Uninstall(object obj)
        {
            return Server101.Uninstall(obj);
        }

		public static new object Setup(object obj)
        {
            return Server101.Setup(obj);
        }

		public static new object Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0.1", false);
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class Server101 : Server10
    {
		public static new object Install(object obj)
        {
            return Server10.InstallBase(obj, "1.0.0");
        }

		public static new object Uninstall(object obj)
        {
            return Server10.Uninstall(obj);
        }

		public static new object Setup(object obj)
        {
            return Server10.Setup(obj);
        }

		public static new object Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0", false);
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class Server10 : Server
    {
		public static new object Install(object obj)
        {
            return Server.InstallBase(obj, "1.0.0");
        }

		public static new object Uninstall(object obj)
        {
            return Server.Uninstall(obj);
        }

        public static new object Setup(object obj)
        {
            return Server.Setup(obj);
        }
    }
}
