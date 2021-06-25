using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Buttons.Base;
using Postgres.Marula.App.Control.UIElements.Messages;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Button to calculate parameter values immediately. 
	/// </summary>
	internal class CalculateValuesButton : ButtonBase
	{
		private readonly IMessageBox messageBox;

		public CalculateValuesButton(IMessageBox messageBox)
		{
			this.messageBox = messageBox;
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		public CalculateValuesButton Initialize()
		{
			Text = "calculate immediately";
			ColorScheme = ButtonColorScheme();

			Clicked += async ()
				=> await messageBox
					.Query("calculate values", "calculate parameter values immediately?")
					.OnConfirmedAsync(() =>
					{
						// todo
						return Task.CompletedTask;
					});

			return this;
		}
	}
}