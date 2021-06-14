using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <summary>
	/// Control application menu item.
	/// </summary>
	internal interface IMenuItem
	{
		/// <summary>
		/// Displayed menu item name.
		/// </summary>
		NonEmptyString Name { get; }

		// /// <summary>
		// /// Source of data related to menu item.
		// /// </summary>
		// IPageContent PageContent { get; }
	}
}