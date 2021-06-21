using System;
using System.Threading.Tasks;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="ListView"/> type.
	/// </summary>
	internal static class ListViewExtensions
	{
		/// <summary>
		/// Add async function <paramref name="onSelection"/> as handler of <see cref="ListView.OpenSelectedItem"/>
		/// event if <see cref="ListViewItemEventArgs.Value"/> is assignable to <typeparamref name="TListItem"/>.
		/// </summary>
		public static ListView OnSelectionOf<TListItem>(this ListView listView, Func<TListItem, Task> onSelection)
		{
			listView.OpenSelectedItem += async eventArgs =>
			{
				if (eventArgs.Value is not TListItem value) return;
				await onSelection(value);
			};

			return listView;
		}
	}
}