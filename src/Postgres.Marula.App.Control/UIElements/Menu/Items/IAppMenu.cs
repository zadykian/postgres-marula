using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
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

	internal static class AppMenuExtensions
	{
		/// <summary>
		///  
		/// </summary>
		public static async Task<int> TotalWidthAsync(this IAppMenu appMenu)
		{
			var generalMenuItems = appMenu
				.LoadGeneral()
				.ToArray();

			var jobMenuItems = await appMenu
				.LoadJobsAsync()
				.ToArrayAsync()
				.ConfigureAwait(false); // todo

			return generalMenuItems
				.Concat(jobMenuItems)
				.Max(item => item.Name.Length) + 4;
		}
	}
}