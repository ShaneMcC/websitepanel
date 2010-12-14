using System;
namespace WebsitePanel.Setup.Actions
{
	public interface IPrerequisiteAction
	{
		bool Run(WebsitePanel.Setup.SetupVariables vars);
		// The event
		event EventHandler<ActionProgressEventArgs<bool>> Complete;
	}
}
