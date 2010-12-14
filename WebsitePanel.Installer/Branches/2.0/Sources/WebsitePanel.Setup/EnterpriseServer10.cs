using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Release 1.1.1
	/// </summary>
	public class EnterpriseServer111 : EnterpriseServer
	{
		public static new object Install(object obj)
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

		public static new DialogResult Update(object obj)
		{
			return UpdateBase(obj, "1.0.0", "1.1.0", true);
		}
	}

	/// <summary>
	/// Release 1.1.0
	/// </summary>
	public class EnterpriseServer110 : EnterpriseServer
	{
		public static new object Install(object obj)
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

		public static new DialogResult Update(object obj)
		{
			return UpdateBase(obj, "1.0.0", "1.0.2", true);
		}
	}

    /// Release 1.0.2
    /// </summary>
    public class EnterpriseServer102 : EnterpriseServer101
    {
        public static new object Install(object obj)
        {
            return EnterpriseServer101.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return EnterpriseServer101.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return EnterpriseServer101.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0.1", true);
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class EnterpriseServer101 : EnterpriseServer10
    {
        public static new object Install(object obj)
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
            return UpdateBase(obj, "1.0.0", "1.0", true);
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class EnterpriseServer10 : EnterpriseServer
    {
        public static new object Install(object obj)
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
