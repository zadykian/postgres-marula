using System.Collections.Generic;
using System.Linq;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <inheritdoc />
	internal class AppMenu : IAppMenu
	{
		private readonly IReadOnlyCollection<IMenuItem> generalMenuItems;
		private readonly IJobs jobs;

		public AppMenu(
			IEnumerable<IMenuItem> generalMenuItems,
			IJobs jobs)
		{
			this.jobs = jobs;
			this.generalMenuItems = generalMenuItems.ToArray();
		}

		/// <inheritdoc />
		IEnumerable<IMenuItem> IAppMenu.LoadGeneral()
			=> generalMenuItems
				.OrderBy(menuItem => menuItem.Order)
				.ToArray();

		/// <inheritdoc />
		IAsyncEnumerable<IMenuItem> IAppMenu.LoadJobsAsync()
			=> jobs
				.InfoAboutAllAsync()
				.Select((jobInfo, index) => new MenuItem(jobInfo.Name, (byte) index));
	}
}