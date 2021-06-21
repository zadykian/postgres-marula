using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Messages;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Button to export calculated values to .sql file.
	/// </summary>
	internal class ExportValuesButton : ButtonBase
	{
		private readonly IMessageBox messageBox;

		public ExportValuesButton(IMessageBox messageBox)
		{
			this.messageBox = messageBox;
		}

		/// <summary>
		/// Perform initialization.
		/// </summary>
		public ExportValuesButton Initialize()
		{
			Text = "export values to .sql";
			ColorScheme = ButtonColorScheme();

			Clicked += async ()
				=> await messageBox
					.QueryAsync("export values", "export parameter values to .sql file?")
					.OnConfirmed(() =>
					{
						// todo
						return Task.CompletedTask;
					});

			return this;
		}
	}
}