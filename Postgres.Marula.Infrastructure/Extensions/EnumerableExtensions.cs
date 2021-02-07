using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IEnumerable{T}"/> type.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Perform action <paramref name="action"/> for each item in <paramref name="enumerable"/>. 
		/// </summary>
		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (var item in enumerable)
			{
				action(item);
			}
		}

		/// <summary>
		/// Perform action <paramref name="asyncFunc"/> for each item in <paramref name="enumerable"/>. 
		/// </summary>
		public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> asyncFunc)
		{
			foreach (var item in enumerable)
			{
				await asyncFunc(item);
			}
		}

		/// <summary>
		/// Join string values <paramref name="stringValues"/> with separator <paramref name="separator"/>. 
		/// </summary>
		public static string JoinBy(this IEnumerable<string> stringValues, string separator) => string.Join(separator, stringValues);
	}
}