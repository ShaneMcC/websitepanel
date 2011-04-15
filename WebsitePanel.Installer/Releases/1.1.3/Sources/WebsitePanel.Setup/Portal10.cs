using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WebsitePanel.Setup
{
	/// <summary>
	/// Release 1.1.3
	/// </summary>
	public class Portal113 : Portal
	{
		public static new object Install(object obj)
		{
			return Portal.InstallBase(obj, "1.1.0");
		}

		public static new DialogResult Uninstall(object obj)
		{
			return Portal.Uninstall(obj);
		}

		public static new DialogResult Setup(object obj)
		{
			return Portal.Setup(obj);
		}

		public static new DialogResult Update(object obj)
		{
			return UpdateBase(obj, "1.1.0", "1.1.2", false);
		}
	}

	/// <summary>
	/// Release 1.1.2
	/// </summary>
	public class Portal112 : Portal
	{
		public static new object Install(object obj)
		{
			return Portal.InstallBase(obj, "1.1.0");
		}

		public static new DialogResult Uninstall(object obj)
		{
			return Portal.Uninstall(obj);
		}

		public static new DialogResult Setup(object obj)
		{
			return Portal.Setup(obj);
		}

		public static new DialogResult Update(object obj)
		{
			return UpdateBase(obj, "1.1.0", "1.1.0", false);
		}
	}

	/// <summary>
	/// Release 1.1.1
	/// </summary>
	public class Portal111 : Portal
	{
		public static new object Install(object obj)
		{
			return Portal.InstallBase(obj, "1.1.0");
		}

		public static new DialogResult Uninstall(object obj)
		{
			return Portal.Uninstall(obj);
		}

		public static new DialogResult Setup(object obj)
		{
			return Portal.Setup(obj);
		}

		public static new DialogResult Update(object obj)
		{
			return UpdateBase(obj, "1.1.0", "1.1.0", false);
		}
	}

	/// <summary>
	/// Release 1.1.0
	/// </summary>
	public class Portal110 : Portal
	{
		public static new object Install(object obj)
		{
			return Portal.InstallBase(obj, "1.1.0");
		}

		public static new DialogResult Uninstall(object obj)
		{
			return Portal.Uninstall(obj);
		}

		public static new DialogResult Setup(object obj)
		{
			return Portal.Setup(obj);
		}

		public static new DialogResult Update(object obj)
		{
			return UpdateBase(obj, "1.1.0", "1.0.2", false, new InstallAction(ActionTypes.AddCustomErrorsPage));
		}
	}
    /// Release 1.0.2
    /// </summary>
    public class Portal102 : Portal101
    {
        public static new object Install(object obj)
        {
            return Portal101.InstallBase(obj, "1.0.0");
        }

        public static new DialogResult Uninstall(object obj)
        {
            return Portal101.Uninstall(obj);
        }

        public static new DialogResult Setup(object obj)
        {
            return Portal101.Setup(obj);
        }

        public static new DialogResult Update(object obj)
        {
            return UpdateBase(obj, "1.0.0", "1.0.1", false);
        }
    }

    /// <summary>
    /// Release 1.0.1
    /// </summary>
    public class Portal101 : Portal10
    {
        public static new object Install(object obj)
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
            return UpdateBase(obj, "1.0.0", "1.0", false);
        }
    }

    /// <summary>
    /// Release 1.0
    /// </summary>
    public class Portal10 : Portal
    {
        public static new object Install(object obj)
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
