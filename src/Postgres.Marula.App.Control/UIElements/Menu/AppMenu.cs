using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Postgres.Marula.Calculations.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Menu
{
	/// <inheritdoc />
	internal class AppMenu : IAppMenu
	{
		private readonly IReadOnlyCollection<IMenuItem> generalMenuItems;
		private readonly IJobs jobs;
		private readonly ILogger<AppMenu> logger;

		public AppMenu(
			IEnumerable<IMenuItem> generalMenuItems,
			IJobs jobs,
			ILogger<AppMenu> logger)
		{
			this.generalMenuItems = generalMenuItems.ToArray();
			this.jobs = jobs;
			this.logger = logger;
		}

		/// <inheritdoc />
		IEnumerable<IMenuItem> IAppMenu.LoadGeneral()
			=> generalMenuItems
				.OrderBy(menuItem => menuItem.Order)
				.ToArray();

		/// <inheritdoc />
		async IAsyncEnumerable<IMenuItem> IAppMenu.LoadJobsAsync()
		{
			IMenuItem[] jobInfoItems;
			try
			{
				jobInfoItems = await jobs
					.InfoAboutAllAsync()
					.Select((jobInfo, index) => new MenuItem(jobInfo.Name, (byte) index))
					.ToArrayAsync()
					.ConfigureAwait(false); // todo
			}
			catch (Exception exception)
			{
				logger.LogError(exception, "failed to get info about jobs from main host.");
				yield break;
			}

			foreach (var jobInfo in jobInfoItems) yield return jobInfo;
		}
	}
}