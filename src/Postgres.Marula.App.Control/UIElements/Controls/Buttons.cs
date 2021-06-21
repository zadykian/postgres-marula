using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Messages;
using Postgres.Marula.Calculations.PublicApi;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Controls
{
	/// <inheritdoc />
	internal class Buttons : IButtons
	{
		private readonly IJobs jobs;
		private readonly IMessageBox messageBox;

		public Buttons(IJobs jobs, IMessageBox messageBox)
		{
			this.jobs = jobs;
			this.messageBox = messageBox;
		}

		/// <inheritdoc />
		Button IButtons.CalculateImmediately()
		{
			var calculateImmediatelyButton = new Button("calculate immediately")
			{
				ColorScheme = ButtonColorScheme()
			};

			calculateImmediatelyButton.Clicked += async ()
				=> await messageBox
					.QueryAsync("calculate values", "calculate parameter values immediately?")
					.OnConfirmed(() =>
					{
						// todo
						return Task.CompletedTask;
					});

			return calculateImmediatelyButton;
		}

		/// <inheritdoc />
		Button IButtons.ExportValues()
		{
			var exportValuesButton = new Button("export values to .sql")
			{
				ColorScheme = ButtonColorScheme()
			};

			exportValuesButton.Clicked += async ()
				=> await messageBox
					.QueryAsync("export values", "export parameter values to .sql file?")
					.OnConfirmed(() =>
					{
						// todo
						return Task.CompletedTask;
					});

			return exportValuesButton;
		}

		/// <inheritdoc />
		Button IButtons.ApplyCalculatedValues()
		{
			var applyValuesButton = new Button("apply calculated values")
			{
				ColorScheme = ButtonColorScheme()
			};

			applyValuesButton.Clicked += async ()
				=> await messageBox
					.QueryAsync("apply values", "apply calculated values to database server configuration?")
					.OnConfirmed(() =>
					{
						// todo
						return Task.CompletedTask;
					});

			return applyValuesButton;
		}

		/// <inheritdoc />
		Button IButtons.StartAllJobs()
		{
			var startAllButton = new Button("start all")
			{
				ColorScheme = ButtonColorScheme()
			};

			startAllButton.Clicked += async ()
				=> await messageBox
					.QueryAsync("start jobs", "send request to host app?")
					.OnConfirmed(jobs.StartAllAsync);

			return startAllButton;
		}

		/// <inheritdoc />
		Button IButtons.StopAllJobs()
		{
			var stopAllButton = new Button("stop all")
			{
				ColorScheme = ButtonColorScheme()
			};

			stopAllButton.Clicked += async ()
				=> await messageBox
					.QueryAsync("stop jobs", "send request to host app?")
					.OnConfirmed(jobs.StopAllAsync);

			return stopAllButton;
		}

		/// <summary>
		/// Create UI color scheme for buttons. 
		/// </summary>
		private static ColorScheme ButtonColorScheme()
		{
			var normal = Application.Driver.MakeAttribute(fore: Color.Black, back: Color.Gray);
			var focused = Application.Driver.MakeAttribute(fore: Color.White, back: Color.Cyan);

			return new ColorScheme
			{
				Normal = normal,
				Focus = focused,
				HotNormal = normal,
				HotFocus = normal
			};
		}
	}
}