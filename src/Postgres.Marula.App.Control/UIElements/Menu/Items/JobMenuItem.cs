using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;
using Postgres.Marula.Calculations.PeriodicJobs.PublicApi;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <summary>
	/// Job menu item.
	/// </summary>
	internal class JobMenuItem : MenuItemBase
	{
		public JobMenuItem(IJobInfo jobInfo, byte order)
			: base(jobInfo.Name, order)
		{
		}

		/// <inheritdoc />
		public override Task HandleClickAsync() => Task.CompletedTask;
	}
}