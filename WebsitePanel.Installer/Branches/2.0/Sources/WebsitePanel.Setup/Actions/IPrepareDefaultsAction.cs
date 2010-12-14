using System;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Setup.Actions
{
	public interface IPrepareDefaultsAction
	{
		void Run(WebsitePanel.Setup.SetupVariables vars);
	}
}
