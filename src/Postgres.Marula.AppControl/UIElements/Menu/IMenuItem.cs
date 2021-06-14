using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <summary>
	/// Control application menu item.
	/// </summary>
	internal interface IMenuItem
	{
		/// <summary>
		/// Order of item in menu.
		/// </summary>
		byte Order { get; }
		
		/// <summary>
		/// Displayed menu item name.
		/// </summary>
		NonEmptyString Name { get; }

		/// <summary>
		/// Handle click action.
		/// </summary>
		Task HandleClickAsync();

		// /// <summary>
		// /// Source of data related to menu item.
		// /// </summary>
		// IPageContent PageContent { get; }
	}
}