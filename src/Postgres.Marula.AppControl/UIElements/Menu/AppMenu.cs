using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <inheritdoc />
	internal class AppMenu : IAppMenu
	{
		/// <inheritdoc />
		async IAsyncEnumerable<IMenuItem> IAppMenu.LoadAsync()
		{
			await Task.CompletedTask;
			yield return new MenuItem("logs");
			// todo: get jobs from main host
			yield return new MenuItem("calculate immediately");
			yield return new MenuItem("calculated values");
			yield return new MenuItem("export values to .sql");
			yield return new MenuItem("quit");
		}
	}
}