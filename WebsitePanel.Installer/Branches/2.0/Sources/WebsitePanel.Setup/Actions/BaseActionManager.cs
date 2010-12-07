using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WebsitePanel.Setup.Actions
{
	public class ActionProgressEventArgs : EventArgs
	{
		public string Text { get; set; }
		public int Value { get; set; }
		public bool Indeterminate { get; set; }
	}

	public class ProgressEventArgs : EventArgs
	{
		public int Value { get; set; }
	}

	public class ActionErrorEventArgs : EventArgs
	{
		public string ErrorMessage { get; set; }
	}

	public abstract class Action
	{
		private Dictionary<string, object> actionProperties;

		public abstract void Run(SetupVariables vars);

		public abstract void Rollback(SetupVariables vars);

		public string Text
		{
			get { return GetPropertyValue<string>("Text"); }
			set { SetPropertyValue<string>("Text", value); }
		}

		public string RollbackText
		{
			get { return GetPropertyValue<string>("RollbackText"); }
			set { SetPropertyValue<string>("RollbackText", value); }
		}

		public bool Indeterminate
		{
			get { return GetPropertyValue<bool>("Indeterminate"); }
			set { SetPropertyValue<bool>("Indeterminate", value); }
		}

		protected void UpdateProgress(string text, int value)
		{
			OnProgressChanged(this, new ActionProgressEventArgs
			{
				Text = text,
				Value = value,
				Indeterminate = this.Indeterminate
			});
		}

		protected T GetPropertyValue<T>(string propertyName)
		{
			if (String.IsNullOrEmpty(propertyName))
			{
				Log.WriteInfo(String.Format("Empty property has requested by '{1}' action.", GetType()));
				//
				return default(T);
			}
			//
			if (!actionProperties.ContainsKey(propertyName))
			{
				Log.WriteInfo(String.Format("Uknown property {0} has requested by '{1}' action.", propertyName, GetType()));
				//
				return default(T);
			}
			//
			return (T)actionProperties[propertyName];
		}

		protected void SetPropertyValue<T>(string propertyName, T value)
		{
			// Lazy initialization
			if (actionProperties == null)
				actionProperties = new Dictionary<string, object>();
			//
			actionProperties[propertyName] = value;
		}

		public void WriteDebugInfo()
		{
			var markerDbgStr = String.Format("{0}", GetType());
			//
			Log.WriteStart(markerDbgStr);
			//
			foreach (var item in actionProperties)
			{
				Log.WriteLine(String.Format("({0}){1} = {2};", item.Value.GetType(), item.Key, item.Value));
			}
			//
			Log.WriteEnd(markerDbgStr);
		}

		// The event   
		public event EventHandler<ActionProgressEventArgs> ProgressChanged;
		// Fire the Event   
		private void OnProgressChanged(object sender, ActionProgressEventArgs args)
		{
			// Check if there are any Subscribers   
			if (ProgressChanged != null)
			{
				// Call the Event   
				ProgressChanged(sender, args);
			}
		}
	}

	public class BaseActionManager
	{
		private List<Action> actions;
		private SetupVariables sessionVariables;

		public SetupVariables SessionVariables
		{
			get
			{
				return sessionVariables;
			}
		}

		#region Events

		public event EventHandler<ProgressEventArgs> TotalProgressChanged;
		public event EventHandler<ActionProgressEventArgs> ActionProgressChanged;
		public event EventHandler<ActionErrorEventArgs> ActionError;
		public event EventHandler PreInit;

		#endregion

		protected BaseActionManager(SetupVariables sessionVariables)
		{
			if (sessionVariables == null)
				throw new ArgumentNullException("sessionVariables");
			//
			actions = new List<Action>();
			//
			this.sessionVariables = sessionVariables;
			//
			PreInit += new EventHandler(BaseActionManager_PreInit);
		}

		void BaseActionManager_PreInit(object sender, EventArgs e)
		{
			AppConfig.LoadConfiguration();

			//LoadSetupVariablesFromParameters(wizard, args);

			SessionVariables.SetupAction = SetupActions.Install;
			SessionVariables.InstallationFolder = Path.Combine("C:\\WebsitePanel", SessionVariables.ComponentName);
			SessionVariables.ComponentId = Guid.NewGuid().ToString();
			SessionVariables.Instance = String.Empty;

			//create component settings node
			SessionVariables.ComponentConfig = AppConfig.CreateComponentConfig(SessionVariables.ComponentId);
		}

		/// <summary>
		/// Adds action into the list of actions to be executed in the current action manager's session and attaches to its ProgressChange event 
		/// to track the action's execution progress.
		/// </summary>
		/// <param appPoolName="action">Action to be executed</param>
		/// <exception cref="ArgumentNullException"/>
		public virtual void AddAction(Action action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			action.ProgressChanged += new EventHandler<ActionProgressEventArgs>(OnActionProgressChanged);
			actions.Add(action);
		}

		private void UpdateActionProgress(string actionText, int actionValue, bool indeterminateAction)
		{
			OnActionProgressChanged(this, new ActionProgressEventArgs
			{
				Text = actionText,
				Value = actionValue,
				Indeterminate = indeterminateAction
			});
		}

		private void UpdateTotalProgress(int value)
		{
			OnTotalProgressChanged(this, new ProgressEventArgs { Value = value });
		}

		// Fire the Event   
		private void OnTotalProgressChanged(object sender, ProgressEventArgs args)
		{
			// Check if there are any Subscribers   
			if (TotalProgressChanged != null)
			{
				// Call the Event   
				TotalProgressChanged(sender, args);
			}
		}

		// Fire the Event   
		private void OnActionProgressChanged(object sender, ActionProgressEventArgs args)
		{
			// Check if there are any Subscribers   
			if (ActionProgressChanged != null)
			{
				// Call the Event   
				ActionProgressChanged(sender, args);
			}
		}

		private void OnActionError()
		{
			//
			if (ActionError == null)
				return;
			//
			var args = new ActionErrorEventArgs
			{
				ErrorMessage = "An unexpected error has occurred. We apologize for this inconvenience.\n" +
				"Please contact Technical Support at support@websitepanel.net.\n\n" +
				"Make sure you include a copy of the Installer.log file from the\n" +
				"WebsitePanel Installer home directory."
			};
			//
			ActionError(this, args);
		}

		private void OnPreInit()
		{
			if (PreInit == null)
				return;
			//
			PreInit(this, EventArgs.Empty);
		}

		/// <summary>
		/// Starts action execution.
		/// </summary>
		public virtual void Start()
		{
			var currentActionType = default(Type);
			var currentActionIndex = 0;

			#region Phase 0. PreInit (notifying external clients about pre-initialization life stage)
			// 
			try
			{
				OnPreInit();
			}
			catch (Exception ex)
			{
				Log.WriteError("Pre-initialization phase has been failed.", ex);
				//
				return;
			}
			#endregion

			#region Phase 1. Executing the installation session
			//
			try
			{
				//
				int totalValue = 0;
				for (int i = 0; i < actions.Count; i++)
				{
					currentActionIndex = i;
					// Get the next action from the queue
					var action = actions[currentActionIndex];
					// Take the action's type to log as much information about it as possible
					currentActionType = action.GetType();
					// Action is about to start - 0% complete
					UpdateActionProgress(action.Text, 0, action.Indeterminate);
					// Execute an install action
					action.Run(SessionVariables);
					// Action has completed - 100% complete
					UpdateActionProgress(action.Text, 100, action.Indeterminate);
					// Calculate overall current progress status
					totalValue = Convert.ToInt32((currentActionIndex + 1) * 100 / actions.Count);
					// Update overall progress status
					UpdateTotalProgress(totalValue);
				}
				//
				totalValue = 100;
				//
				UpdateTotalProgress(totalValue);
			}
			catch (Exception ex)
			{
				//
				if (currentActionType != default(Type))
				{
					Log.WriteError(String.Format("Failed to execute '{0}' type of action.", currentActionType));
				}
				//
				Log.WriteError("Here is the original exception...", ex);
				//
				if (Utils.IsThreadAbortException(ex))
					return;
				// Notify external clients
				OnActionError();
				// Rolling back all changes
				Rollback(currentActionIndex);
				//
				return;
			}
			#endregion
		}

		public virtual void Rollback(int currentActionIndex)
		{
			var currentActionType = default(Type);

			//
			Log.WriteStart("Rolling back");
			//
			var totalValue = Convert.ToInt32((currentActionIndex + 1) * 100 / actions.Count);
			//
			while (currentActionIndex >= 0)
			{
				//
				var action = actions[currentActionIndex];
				//
				currentActionType = action.GetType();
				//
				UpdateActionProgress(action.RollbackText, 0, action.Indeterminate);
				//
				try
				{
					action.Rollback(SessionVariables);
				}
				catch (Exception ex)
				{
					if (currentActionType != default(Type))
						Log.WriteError(String.Format("Failed to rollback '{0}' action.", currentActionType));
					//
					Log.WriteError("Here is the original exception...", ex);
					//
				}
				//
				UpdateActionProgress(action.RollbackText, 100, action.Indeterminate);
				//
				currentActionIndex--;
				//
				totalValue = Convert.ToInt32((currentActionIndex + 1) * 100 / actions.Count);
				//
				UpdateTotalProgress(totalValue);
			}
			//
			Log.WriteEnd("Rolled back");
		}
	}
}