using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.MainViews.Loader;
using Postgres.Marula.App.Control.UIElements.MainViews.Output;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <summary>
	/// Calculated values menu item.
	/// </summary>
	internal class CalculatedValuesMenuItem : MenuItemBase
	{
		private readonly IParameterValues parameterValues;
		private readonly IOutputWindow outputWindow;
		private readonly ILoader loader;
		private readonly IMessageBox messageBox;

		public CalculatedValuesMenuItem(
			IParameterValues parameterValues,
			IOutputWindow outputWindow,
			ILoader loader,
			IMessageBox messageBox) : base("view calculated values", 1)
		{
			this.parameterValues = parameterValues;
			this.outputWindow = outputWindow;
			this.loader = loader;
			this.messageBox = messageBox;
		}

		/// <inheritdoc />
		public override async Task HandleClickAsync()
		{
			loader.OnLoadingStarted();

			IReadOnlyCollection<string> valueViews;

			try
			{
				valueViews = await parameterValues
					.MostRecentAsync()
					.OrderBy(view => view.Link.Name)
					.Select(view => $"{view.Link.Name,-36}{view.Value}")
					.ToArrayAsync();
			}
			catch (Exception exception)
			{
				messageBox.Error("failed to load values from host app", exception);
				return;
			}
			finally
			{
				loader.OnLoadingFinished();
			}

			outputWindow
				.Titled("calculated values")
				.Show(valueViews);

			if (!valueViews.Any())
			{
				messageBox.Info("unable to load parameter values", "there are no any calculated values found!");
			}
		}
	}
}