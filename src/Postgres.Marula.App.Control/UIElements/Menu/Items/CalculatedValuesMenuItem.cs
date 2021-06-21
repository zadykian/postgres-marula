using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.MainViews;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <summary>
	/// Calculated values menu item.
	/// </summary>
	internal class CalculatedValuesMenuItem : MenuItem
	{
		private readonly IParameterValues parameterValues;
		private readonly IOutputWindow outputWindow;
		
		public CalculatedValuesMenuItem(
			IParameterValues parameterValues,
			IOutputWindow outputWindow) : base("view calculated values", 1)
		{
			this.parameterValues = parameterValues;
			this.outputWindow = outputWindow;
		}

		/// <inheritdoc />
		public override async Task HandleClickAsync()
		{
			var valueViews = await parameterValues
				.MostRecentAsync()
				.OrderBy(view => view.Link.Name)
				.Select(view => $"{view.Link.Name,-32}{view.Value}")
				.Select(line => (NonEmptyString) line)
				.ToArrayAsync();

			outputWindow
				.Title("calculated values")
				.Show(valueViews);
		}
	}
}