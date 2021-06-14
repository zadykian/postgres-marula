using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <inheritdoc />
	internal class AppMenu : IAppMenu
	{
		/// <inheritdoc />
		IEnumerable<IMenuItem> IAppMenu.LoadGeneral()
		{
			yield return new MenuItem("view ctl app logs");
			yield return new MenuItem("calculate immediately");
			yield return new MenuItem("view calculated values");
			yield return new MenuItem("export values to .sql");
			yield return new QuitMenuItem();
		}

		/// <inheritdoc />
		async IAsyncEnumerable<IMenuItem> IAppMenu.LoadJobsAsync()
		{
			// todo: get jobs from main host
			await Task.CompletedTask;
			yield return new MenuItem("values calculation");
			yield return new MenuItem("wal lsn logging");
			yield return new MenuItem("bloat fraction logging");
		}
	}
}