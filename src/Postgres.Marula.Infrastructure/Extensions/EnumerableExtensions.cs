using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Postgres.Marula.Infrastructure.TypeDecorators;

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
		/// Transform collection <paramref name="enumerable"/> by application of <paramref name="selector"/> to each item.
		/// </summary>
		public static async Task<IEnumerable<TOut>> SelectAsync<TIn, TOut>(
			this IEnumerable<TIn> enumerable,
			Func<TIn, Task<TOut>> selector)
		{
			ICollection<TOut> resultCollection = new List<TOut>();

			foreach (var item in enumerable)
			{
				resultCollection.Add(await selector(item));
			}

			return resultCollection;
		}

		/// <summary>
		/// Join string values <paramref name="stringValues"/> with separator <paramref name="separator"/>. 
		/// </summary>
		public static string JoinBy(this IEnumerable<string> stringValues, string separator)
			=> string.Join(separator, stringValues);

		/// <summary>
		/// Join string values <paramref name="stringValues"/> with separator <paramref name="separator"/>. 
		/// </summary>
		public static NonEmptyString JoinBy(this IEnumerable<NonEmptyString> stringValues, NonEmptyString separator)
			=> string.Join(separator, stringValues);

		/// <summary>
		/// Append item <paramref name="itemToAppend"/> to enumerable <paramref name="enumerable"/>. 
		/// </summary>
		public static IEnumerable<T> Add<T>(this IEnumerable<T> enumerable, T itemToAppend)
		{
			foreach (var item in enumerable)
			{
				yield return item;
			}

			yield return itemToAppend;
		}
	}
}