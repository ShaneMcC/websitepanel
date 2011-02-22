using System;

namespace WebsitePanel.Providers.ResultObjects
{
    [Serializable]
    public class HeliconApeStatus
    {
		private string registrationInfo = String.Empty;
		private string version = String.Empty;
		private string installDir = String.Empty;

		public static HeliconApeStatus Empty = new HeliconApeStatus
		{
			IsEnabled = false,
			IsInstalled = false,
			IsRegistered = false,
			RegistrationInfo = String.Empty,
			Version = String.Empty,
		};

        public bool IsEnabled { get; set; }
        public bool IsInstalled { get; set; }
        public bool IsRegistered { get; set; }

		public string InstallDir
		{
			get { return installDir; }
			set { installDir = value; }
		}

		public string Version
		{
			get { return version; }
			set { version = value; }
		}

		public string RegistrationInfo
		{
			get { return registrationInfo; }
			set { registrationInfo = value; }
		}
    }
}