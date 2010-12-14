using System;
namespace WebsitePanel.Setup.Actions
{
	public interface IUninstallAction
	{
		void Run(WebsitePanel.Setup.SetupVariables vars);
		//
		event EventHandler<ActionProgressEventArgs<int>> ProgressChange;
	}
}
