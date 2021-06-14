using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <inheritdoc />
	internal class AppMenu : IAppMenu
	{
		private readonly IReadOnlyCollection<IMenuItem> generalMenuItems;

		public AppMenu(IEnumerable<IMenuItem> generalMenuItems)
			=> this.generalMenuItems = generalMenuItems.ToArray();

		/// <inheritdoc />
		IEnumerable<IMenuItem> IAppMenu.LoadGeneral() => generalMenuItems;

		/// <inheritdoc />
		async IAsyncEnumerable<IMenuItem> IAppMenu.LoadJobsAsync()
		{
			// todo: get jobs from main host
			await Task.CompletedTask;
			yield return new MenuItem("values calculation", 0);
			yield return new MenuItem("wal lsn logging", 1);
			yield return new MenuItem("bloat fraction logging", 2);
		}
	}
}