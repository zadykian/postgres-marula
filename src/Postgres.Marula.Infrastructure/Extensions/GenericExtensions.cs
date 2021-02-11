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
			=> nullableValue.ThrowIf(
				value => value is null,
				() => new ArgumentException(message))!;

		/// <summary>
		/// Throw exception created by <paramref name="exceptionFactory"/>
		/// if value <paramref name="inputValue"/> matches predicate <paramref name="exceptionPredicate"/>.
		/// </summary>
		public static T ThrowIf<T>(this T inputValue,
			Predicate<T> exceptionPredicate,
			Func<Exception> exceptionFactory)
			=> exceptionPredicate(inputValue)
				? throw exceptionFactory()
				: inputValue;

		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="func"/>. 
		/// </summary>
		public static TOut To<TIn, TOut>(this TIn inputValue, Func<TIn, TOut> func) => func(inputValue);

		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="action"/>. 
		/// </summary>
		public static void To<TIn>(this TIn inputValue, Action<TIn> action) => action(inputValue);

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