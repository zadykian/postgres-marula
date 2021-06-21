using System.Linq;
using Postgres.Marula.App.Control.UIElements.Buttons.Base;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.App.Control.ValuesExport;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Buttons
{
	/// <summary>
	/// Button to export calculated values to .sql file.
	/// </summary>
	internal class ExportValuesButton : ButtonBase
	{
		private readonly IMessageBox messageBox;
		private readonly IParameterValues parameterValues;
		private readonly IValuesExport valuesExport;

		public ExportValuesButton(
			IMessageBox messageBox,
			IParameterValues parameterValues,
			IValuesExport valuesExport)
		{
			this.messageBox = messageBox;
			this.parameterValues = parameterValues;
			this.valuesExport = valuesExport;
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
					.OnConfirmed(async () =>
					{
						var values = await parameterValues.MostRecentAsync().ToArrayAsync();
						var fileName = await valuesExport.ExportAsync(values);
						messageBox.Show("values are exported successfully", $"script file name: '{fileName}'");
					});

			return this;
		}
	}
}