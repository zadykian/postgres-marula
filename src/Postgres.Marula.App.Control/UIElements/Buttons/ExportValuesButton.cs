using System;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Buttons.Base;
using Postgres.Marula.App.Control.UIElements.MainViews.Loader;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.App.Control.ValuesExport;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.TypeDecorators;

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
		private readonly ILoader loader;

		public ExportValuesButton(
			IMessageBox messageBox,
			IParameterValues parameterValues,
			IValuesExport valuesExport,
			ILoader loader)
		{
			this.messageBox = messageBox;
			this.parameterValues = parameterValues;
			this.valuesExport = valuesExport;
			this.loader = loader;
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
					.Query("export values", "export parameter values to .sql file?")
					.OnConfirmedAsync(ExportOnConfirmed);

			return this;
		}

		/// <summary>
		/// Export parameter values to file.
		/// </summary>
		private async Task ExportOnConfirmed()
		{
			loader.OnLoadingStarted();
			NonEmptyString fileName;

			try
			{
				var values = await parameterValues.MostRecentAsync().ToArrayAsync();
				fileName = await valuesExport.ExportAsync(values);
			}
			catch (Exception exception)
			{
				messageBox.Error("failed to export values", exception);
				return;
			}
			finally
			{
				loader.OnLoadingFinished();
			}

			messageBox.Info("values are exported successfully", $"script file name: '{fileName}'");
		}
	}
}