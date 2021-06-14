using System.Collections.Generic;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <summary>
	/// Control application menu.
	/// </summary>
	internal interface IAppMenu
	{
		/// <summary>
		/// Load all application menu items. 
		/// </summary>
		IAsyncEnumerable<IMenuItem> LoadAsync();
	}
}