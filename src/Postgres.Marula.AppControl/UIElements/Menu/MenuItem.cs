using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <inheritdoc />
	internal class MenuItem : IMenuItem
	{
		public MenuItem(NonEmptyString name) => Name = name;

		/// <inheritdoc />
		public NonEmptyString Name { get; }

		/// <inheritdoc />
		public override string ToString() => $"> {Name}";
	}

	/// <summary>
	/// Quit menu item.
	/// </summary>
	internal class QuitMenuItem : MenuItem
	{
		public QuitMenuItem() : base("quit")
		{
		}
	}
}