using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgres.Marula.App.Control.UIElements.Menu.Items.Base;

namespace Postgres.Marula.App.Control.UIElements.Menu
{
	/// <summary>
	/// Control application menu.
	/// </summary>
	internal interface IAppMenu
	{
		/// <summary>
		/// Load all general menu items. 
		/// </summary>
		IEnumerable<IMenuItem> LoadGeneral();

		/// <summary>
		/// Load all main host job items. 
		/// </summary>
		IAsyncEnumerable<IMenuItem> LoadJobsAsync();
	}

	/// <summary>
	/// Extension methods for <see cref="IAppMenu"/> type.
	/// </summary>
	internal static class AppMenuExtensions
	{
		/// <summary>
		/// Get total width based on all <paramref name="appMenu"/> items.
		/// </summary>
		public static async Task<int> TotalWidthAsync(this IAppMenu appMenu)
		{
			var generalMenuItems = appMenu
				.LoadGeneral()
				.ToArray();

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync()
				.ConfigureAwait(false);

			return generalMenuItems
				.Concat(jobMenuItems)
				.Max(item => item.Name.Length) + 4;
		}
	}
}