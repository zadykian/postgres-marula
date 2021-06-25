using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.App.Control.UIElements.Menu.Items.Base
{
	/// <inheritdoc />
	internal abstract class MenuItemBase : IMenuItem
	{
		protected MenuItemBase(NonEmptyString name, byte order)
		{
			Name = name;
			Order = order;
		}

		/// <inheritdoc />
		public byte Order { get; }

		/// <inheritdoc />
		public NonEmptyString Name { get; }

		/// <inheritdoc />
		public abstract Task HandleClickAsync();

		/// <inheritdoc />
		public override string ToString() => $"> {Name}";
	}
}