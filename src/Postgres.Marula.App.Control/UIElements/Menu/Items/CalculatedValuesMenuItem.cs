using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.MainViews;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <summary>
	/// Calculated values menu item.
	/// </summary>
	internal class CalculatedValuesMenuItem : MenuItem
	{
		private readonly IParameterValues parameterValues;
		private readonly IOutputWindow outputWindow;
		private readonly IMessageBox messageBox;

		public CalculatedValuesMenuItem(
			IParameterValues parameterValues,
			IOutputWindow outputWindow,
			IMessageBox messageBox) : base("view calculated values", 1)
		{
			this.parameterValues = parameterValues;
			this.outputWindow = outputWindow;
			this.messageBox = messageBox;
		}

		/// <inheritdoc />
		public override async Task HandleClickAsync()
		{
			var valueViews = await parameterValues
				.MostRecentAsync()
				.OrderBy(view => view.Link.Name)
				.Select(view => $"{view.Link.Name,-36}{view.Value}")
				.ToArrayAsync();

			outputWindow
				.Titled("calculated values")
				.Show(valueViews);

			if (!valueViews.Any())
			{
				messageBox.Show(
					"unable to load parameter values",
					"there are no any calculated values found!");
			}
		}
	}
}