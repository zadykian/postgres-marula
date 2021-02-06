using System;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Generic extension methods.
	/// </summary>
	public static class GenericExtensions
	{
		/// <summary>
		/// Throw <see cref="ArgumentException"/> with message <paramref name="message"/>
		/// in case when <paramref name="nullableValue"/> is null.
		/// </summary>
		public static T ThrowIfNull<T>(this T? nullableValue, string? message = null)
			=> nullableValue is null
				? throw new ArgumentException(message)
				: nullableValue;

		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="func"/>. 
		/// </summary>
		public static TOut To<TIn, TOut>(this TIn inputValue, Func<TIn, TOut> func) => func(inputValue);

		/// <summary>
		/// Perform operation <paramref name="action"/> on value <paramref name="inputValue"/> and return it unmodified.
		/// </summary>
		public static T Then<T>(this T inputValue, Action<T> action)
		{
			action(inputValue);
			return inputValue;
		}
	}
}