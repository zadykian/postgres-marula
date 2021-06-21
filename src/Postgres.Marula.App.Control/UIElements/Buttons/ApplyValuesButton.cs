using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Messages;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Button to apply most recent calculated values to database server configuration. 
	/// </summary>
	internal class ApplyValuesButton : ButtonBase
	{
		private readonly IMessageBox messageBox;

		public ApplyValuesButton(IMessageBox messageBox)
		{
			this.messageBox = messageBox;
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		public ApplyValuesButton Initialize()
		{
			Text = "apply calculated values";
			ColorScheme = ButtonColorScheme();

			Clicked += async ()
				=> await messageBox
					.QueryAsync("apply values", "apply calculated values to database server configuration?")
					.OnConfirmed(() =>
					{
						// todo
						return Task.CompletedTask;
					});

			return this;
		}
	}
}