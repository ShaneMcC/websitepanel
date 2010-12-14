using System;
namespace WebsitePanel.Setup.Actions
{
	public interface IInstallAction
	{
		void Run(WebsitePanel.Setup.SetupVariables vars);
		// The event
		event EventHandler<ActionProgressEventArgs<int>> ProgressChange;
	}
}
