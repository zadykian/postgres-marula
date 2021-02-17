using System;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Range represented as left and right bounds. 
	/// </summary>
	public readonly struct Range<T> where T : IComparable<T>
	{
		public Range(T leftBound, T rightBound)
		{
			if (leftBound.CompareTo(rightBound) > 0)
			{
				throw new ArgumentException(
					$"{nameof(leftBound)} ({leftBound}) must be " +
					$"less or equal to {nameof(rightBound)} ({rightBound}).");
			}

			LeftBound = leftBound;
			RightBound = rightBound;
		}

		/// <summary>
		/// Left bound of the range. 
		/// </summary>
		public T LeftBound { get; }

		/// <summary>
		/// Right bound of the range.
		/// </summary>
		public T RightBound { get; }

		/// <inheritdoc />
		public override string ToString() => $"[{LeftBound}, {RightBound}]";
	}
}