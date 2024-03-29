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
			=> nullableValue ?? throw new ArgumentNullException(paramName: null, message);

		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="func"/>. 
		/// </summary>
		public static TOut To<TIn, TOut>(this TIn inputValue, Func<TIn, TOut> func) => func(inputValue);

		/// <summary>
		/// Apply value <paramref name="inputValue"/> to function <paramref name="action"/>. 
		/// </summary>
		public static void To<TIn>(this TIn inputValue, Action<TIn> action) => action(inputValue);

		/// <summary>
		/// Perform operation <paramref name="action"/> on value <paramref name="inputValue"/> and return it.
		/// </summary>
		public static T Then<T>(this T inputValue, Action<T> action)
		{
			action(inputValue);
			return inputValue;
		}

		/// <summary>
		/// Check if value <paramref name="value"/> belongs to range [<paramref name="leftBound"/> .. <paramref name="rightBound"/>].
		/// </summary>
		public static bool InRangeBetween<T>(this T value, T leftBound, T rightBound)
			where T : IComparable<T>
			=> value.CompareTo(leftBound) >= 0 && value.CompareTo(rightBound) <= 0;

		/// <summary>
		/// Limit <paramref name="value"/> with range [<paramref name="leftBound"/> .. <paramref name="rightBound"/>].
		/// </summary>
		public static T Limit<T>(this T value, T leftBound, T rightBound)
			where T : IComparable<T>
		{
			if (value.CompareTo(leftBound) < 0) return leftBound;
			if (value.CompareTo(rightBound) > 0) return rightBound;
			return value;
		}
	}
}