using System.Collections.Generic;

namespace Postgres.Marula.AppControl.UIElements
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