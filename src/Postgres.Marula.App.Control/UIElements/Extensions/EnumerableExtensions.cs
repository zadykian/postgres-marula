using System.Collections.Generic;
using System.Linq;
using Terminal.Gui;

namespace Postgres.Marula.App.Control.UIElements.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IEnumerable{T}"/> type
	/// related to UI elements.
	/// </summary>
	internal static class EnumerableExtensions
	{
		/// <summary>
		/// Create <see cref="ListView"/> based on data from <paramref name="enumerable"/>. 
		/// </summary>
		public static ListView AsListView<T>(this IEnumerable<T> enumerable)
			=> new(enumerable.ToArray());
	}
}