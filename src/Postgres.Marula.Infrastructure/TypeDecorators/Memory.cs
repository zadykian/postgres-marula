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

		/// <inheritdoc />
		public override string ToString() => $"{TotalBytes} bytes";

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(Memory other) => TotalBytes == other.TotalBytes;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is Memory other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => TotalBytes.GetHashCode();

		#endregion

		/// <summary>
		/// Parse string <paramref name="stringToParse"/> to memory value.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// Occurs when <paramref name="stringToParse"/> has invalid format.
		/// </exception>
		public static Memory Parse(NonEmptyString stringToParse)
		{
			if (!Regex.IsMatch(stringToParse, @"^[0-9]+\s*(B|kB|MB|GB)$"))
			{
				throw new ArgumentException($"Input string '{stringToParse}' has invalid format.", nameof(stringToParse));
			}

			var (totalBytes, unit) = stringToParse.ParseToTokens();

	        var multiplier = unit switch
	        {
	            "B"  => 1,
	            "kB" => 1024,
	            "MB" => 1024 * 1024,
	            "GB" => 1024 * 1024 * 1024,
	            _    => throw new ArgumentOutOfRangeException(nameof(stringToParse), stringToParse, message: null)
	        };

	        return new Memory(totalBytes * (ulong) multiplier);
		}

		/// <summary>
		/// Multiplication operator. 
		/// </summary>
		public static Memory operator *(Memory memory, double coefficient) => new(memory.TotalBytes *  (ulong) coefficient);

		/// <summary>
		/// Implicit cast operator <see cref="ulong"/> -> <see cref="Memory"/>.
		/// </summary>
		public static implicit operator Memory(ulong bytes) => new(bytes);

		/// <summary>
		/// Implicit cast operator <see cref="Memory"/> -> <see cref="ulong"/>.
		/// </summary>
		public static implicit operator ulong(Memory memory) => memory.TotalBytes;
	}
}