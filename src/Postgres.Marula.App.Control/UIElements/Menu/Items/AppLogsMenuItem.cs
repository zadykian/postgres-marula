using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.MainViews.Output;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <summary>
	/// Control application logs menu item.
	/// </summary>
	internal class AppLogsMenuItem : MenuItemBase
	{
		private readonly IOutputWindow outputWindow;

		public AppLogsMenuItem(IOutputWindow outputWindow) : base("view ctl app logs", 0)
			=> this.outputWindow = outputWindow;

		/// <inheritdoc />
		public override async Task HandleClickAsync()
		{
			// todo
			await Task.CompletedTask;
			outputWindow
				.Titled("application logs")
				.Show(new[] {"there are no any errors occured"});
		}
	}
}