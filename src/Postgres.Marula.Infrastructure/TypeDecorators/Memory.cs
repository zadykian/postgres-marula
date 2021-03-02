using System;

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
		/// Implicit cast operator <see cref="ulong"/> -> <see cref="Memory"/>.
		/// </summary>
		public static implicit operator Memory(ulong bytes) => new(bytes);

		/// <summary>
		/// Implicit cast operator <see cref="Memory"/> -> <see cref="ulong"/>.
		/// </summary>
		public static implicit operator ulong(Memory memory) => memory.TotalBytes;
	}
}