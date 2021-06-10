using System;
using System.Text.RegularExpressions;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Memory volume.
	/// </summary>
	public readonly struct Memory : IEquatable<Memory>
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
				< 10UL * 1024 * 1024               => (TotalBytes / Kilobyte, Unit.Kilobytes),
				< 10UL * 1024 * 1024 * 1024        => (TotalBytes / Megabyte, Unit.Megabytes),
				< 10UL * 1024 * 1024 * 1024 * 1024 => (TotalBytes / Gigabyte, Unit.Gigabytes),
				_                                  => (TotalBytes / Terabyte, Unit.Terabytes)
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

		/// <summary>
		/// Equality operator. 
		/// </summary>
		public static bool operator ==(Memory left, Memory right) => left.Equals(right);

		/// <summary>
		/// Inequality operator. 
		/// </summary>
		public static bool operator !=(Memory left, Memory right) => !(left == right);

		#endregion

		#region Constants

		/// <summary>
		/// One byte - 1B.
		/// </summary>
		public static Memory Byte => new(1);

		/// <summary>
		/// One kilobyte - 1024B.
		/// </summary>
		public static Memory Kilobyte => new(1024);

		/// <summary>
		/// One megabyte - 1024kB.
		/// </summary>
		public static Memory Megabyte => new(1024D * Kilobyte);

		/// <summary>
		/// One gigabyte - 1024MB.
		/// </summary>
		public static Memory Gigabyte => new(1024D * Megabyte);

		/// <summary>
		/// One terabyte - 1024GB.
		/// </summary>
		public static Memory Terabyte => new(1024D * Gigabyte);

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

			return new Memory(numericValue * (ulong) multiplier);
		}

		/// <summary>
		/// Multiplication operator.
		/// </summary>
		public static Memory operator *(Memory memory, double coefficient) => new(memory.TotalBytes *  (ulong) coefficient);


		/// <inheritdoc cref="op_Multiply(Memory,double)"/>
		public static Memory operator *(double coefficient, Memory memory) => memory * coefficient;

		/// <summary>
		/// Division operator.
		/// </summary>
		public static Memory operator /(Memory memory, double coefficient) => new(memory.TotalBytes /  (ulong) coefficient);

		/// <summary>
		/// Implicit cast operator <see cref="ulong"/> -> <see cref="Memory"/>.
		/// </summary>
		public static implicit operator Memory(ulong bytes) => new(bytes);

		/// <summary>
		/// Implicit cast operator <see cref="Memory"/> -> <see cref="ulong"/>.
		/// </summary>
		public static implicit operator ulong(Memory memory) => memory.TotalBytes;

		/// <summary>
		/// Memory unit.
		/// </summary>
		public enum Unit
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