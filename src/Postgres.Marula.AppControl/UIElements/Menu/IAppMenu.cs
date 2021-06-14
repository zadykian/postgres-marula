using System.Collections.Generic;

namespace Postgres.Marula.AppControl.UIElements.Menu
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
}