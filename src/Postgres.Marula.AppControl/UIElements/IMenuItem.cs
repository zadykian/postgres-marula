using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.AppControl.UIElements
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

		/// <summary>
		/// Source of data related to menu item.
		/// </summary>
		IDataSource DataSource { get; }
	}
}