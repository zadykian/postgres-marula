using System;
using System.Text.RegularExpressions;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Memory volume.
	/// </summary>
	public readonly struct Memory : IEquatable<Memory>, IComparable<Memory>, IComparable
	{
		public Memory(ulong totalBytes) => TotalBytes = totalBytes;

		/// <summary>
		/// Memory value in bytes.
		/// </summary>
		public ulong TotalBytes { get; }

		/// <summary>
		/// Normalize memory value based on its' size. 
		/// </summary>
		public (ulong Value, Unit Unit) Normalized()
			=> TotalBytes switch
			{
				< 10UL * 1024                      => (TotalBytes, Unit.Bytes),
				< 10UL * 1024 * 1024               => (TotalBytes / Kilobyte.TotalBytes, Unit.Kilobytes),
				< 10UL * 1024 * 1024 * 1024        => (TotalBytes / Megabyte.TotalBytes, Unit.Megabytes),
				< 10UL * 1024 * 1024 * 1024 * 1024 => (TotalBytes / Gigabyte.TotalBytes, Unit.Gigabytes),
				_                                  => (TotalBytes / Terabyte.TotalBytes, Unit.Terabytes)
			};

		/// <inheritdoc />
		public override string ToString()
		{
			var (value, unit) = Normalized();

			var unitString = unit switch
			{
				Unit.Bytes     => "B",
				Unit.Kilobytes => "kB",
				Unit.Megabytes => "MB",
				Unit.Gigabytes => "GB",
				Unit.Terabytes => "TB",
				_ => throw new ArgumentOutOfRangeException()
			};

			return $"{value}{unitString}";
		}

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(Memory other) => TotalBytes == other.TotalBytes;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is Memory other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => TotalBytes.GetHashCode();

		/// <inheritdoc />
		public int CompareTo(Memory other) => TotalBytes.CompareTo(other.TotalBytes);

		/// <inheritdoc />
		int IComparable.CompareTo(object? obj)
		{
			if (ReferenceEquals(null, obj)) return 1;
			return obj is Memory other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Memory)}");
		}

		#endregion

		#region Constants

		/// <summary>
		/// Zero memory size - 0B.
		/// </summary>
		public static Memory Zero => new(0);

		/// <summary>
		/// One byte - 1B.
		/// </summary>
		public static Memory Byte => new(1);

		/// <summary>
		/// One kilobyte - 1024B.
		/// </summary>
		public static Memory Kilobyte => 1024 * Byte;

		/// <summary>
		/// One megabyte - 1024kB.
		/// </summary>
		public static Memory Megabyte => 1024 * Kilobyte;

		/// <summary>
		/// One gigabyte - 1024MB.
		/// </summary>
		public static Memory Gigabyte => 1024 * Megabyte;

		/// <summary>
		/// One terabyte - 1024GB.
		/// </summary>
		public static Memory Terabyte => 1024 * Gigabyte;

		#endregion

		/// <summary>
		/// Parse string <paramref name="stringToParse"/> to memory value.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Occurs when <paramref name="stringToParse"/> has invalid format.
		/// </exception>
		public static Memory Parse(NonEmptyString stringToParse)
		{
			ArgumentException InvalidFormat()
				=> new($"Input string '{stringToParse}' has invalid format.", nameof(stringToParse));

			if (!Regex.IsMatch(stringToParse, @"^[0-9]+\s*(B|kB|MB|GB|TB)$"))
			{
				throw InvalidFormat();
			}

			var (numericValue, unit) = stringToParse.ParseToTokens();

			var multiplier = unit switch
			{
				"B"  => Byte,
				"kB" => Kilobyte,
				"MB" => Megabyte,
				"GB" => Gigabyte,
				"TB" => Terabyte,
				_    => throw InvalidFormat()
			};

			return numericValue * multiplier;
		}

		#region Operators

		/// <summary>
		/// Equality operator. 
		/// </summary>
		public static bool operator ==(Memory left, Memory right) => left.Equals(right);

		/// <summary>
		/// Inequality operator. 
		/// </summary>
		public static bool operator !=(Memory left, Memory right) => !(left == right);

		/// <summary>
		/// Comparison operator.
		/// </summary>
		public static bool operator <(Memory left, Memory right) => left.CompareTo(right) < 0;

		/// <inheritdoc cref="op_LessThan"/>
		public static bool operator >(Memory left, Memory right) => left.CompareTo(right) > 0;

		/// <inheritdoc cref="op_LessThan"/>
		public static bool operator <=(Memory left, Memory right) => left.CompareTo(right) <= 0;

		/// <inheritdoc cref="op_LessThan"/>
		public static bool operator >=(Memory left, Memory right) => left.CompareTo(right) >= 0;

		/// <summary>
		/// Multiplication operator.
		/// </summary>
		public static Memory operator *(Memory memory, ulong coefficient) => new(memory.TotalBytes * coefficient);

		/// <inheritdoc cref="op_Multiply(Memory,ulong)"/>
		public static Memory operator *(ulong coefficient, Memory memory) => memory * coefficient;

		/// <inheritdoc cref="op_Multiply(Memory,ulong)"/>
		public static Memory operator *(Memory memory, double coefficient) => new((ulong) (memory.TotalBytes * coefficient));

		/// <inheritdoc cref="op_Multiply(Memory,ulong)"/>
		public static Memory operator *(double coefficient, Memory memory) => memory * coefficient;

		/// <inheritdoc cref="op_Multiply(Memory,ulong)"/>
		public static Memory operator *(Memory memory, decimal coefficient) => new((ulong) (memory.TotalBytes * coefficient));

		/// <inheritdoc cref="op_Multiply(Memory,ulong)"/>
		public static Memory operator *(decimal coefficient, Memory memory) => memory * coefficient;

		/// <summary>
		/// Division operator.
		/// </summary>
		public static Memory operator /(Memory memory, ulong coefficient) => new(memory.TotalBytes / coefficient);

		/// <inheritdoc cref="op_Division(Memory,ulong)"/>
		public static Memory operator /(Memory memory, double coefficient) => new((ulong) (memory.TotalBytes / coefficient));

		/// <inheritdoc cref="op_Division(Memory,ulong)"/>
		public static Memory operator /(Memory memory, decimal coefficient) => new((ulong) (memory.TotalBytes / coefficient));

		#endregion

		/// <summary>
		/// Memory unit.
		/// </summary>
		public enum Unit : byte
		{
			/// <summary>
			/// Bytes.
			/// </summary>
			Bytes,

			/// <summary>
			/// Kilobytes.
			/// </summary>
			Kilobytes,

			/// <summary>
			/// Megabytes.
			/// </summary>
			Megabytes,

			/// <summary>
			/// Gigabytes.
			/// </summary>
			Gigabytes,

			/// <summary>
			/// Terabytes.
			/// </summary>
			Terabytes
		}
	}
}