using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items
{
	/// <inheritdoc />
	internal class MenuItem : IMenuItem
	{
		public MenuItem(NonEmptyString name, byte order)
		{
			Name = name;
			Order = order;
		}

		/// <inheritdoc />
		public byte Order { get; }

		/// <inheritdoc />
		public NonEmptyString Name { get; }

		/// <inheritdoc />
		public virtual async Task HandleClickAsync() => await Task.CompletedTask;

		/// <inheritdoc />
		public override string ToString() => $"> {Name}";
	}
}