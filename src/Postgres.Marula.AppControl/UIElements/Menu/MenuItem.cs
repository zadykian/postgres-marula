using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.AppControl.UIElements.Menu
{
	/// <inheritdoc />
	internal class MenuItem : IMenuItem
	{
		public MenuItem(NonEmptyString name) => Name = name;

		/// <inheritdoc />
		public NonEmptyString Name { get; }
	}
}