using System;

namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// Memory volume.
	/// </summary>
	public readonly struct Memory : IEquatable<Memory>
	{
		private readonly ulong totalBytes;

		public Memory(ulong totalBytes) => this.totalBytes = totalBytes;

		/// <inheritdoc />
		public override string ToString() => $"{totalBytes} bytes";

		#region EqualityMembers

		/// <inheritdoc />
		public bool Equals(Memory other) => totalBytes == other.totalBytes;

		/// <inheritdoc />
		public override bool Equals(object? obj) => obj is Memory other && Equals(other);

		/// <inheritdoc />
		public override int GetHashCode() => totalBytes.GetHashCode();

		/// <summary>
		/// <see cref="Memory"/> equality operator. 
		/// </summary>
		public static bool operator ==(Memory left, Memory right) => left.Equals(right);

		/// <inheritdoc cref="op_Equality"/>
		public static bool operator !=(Memory left, Memory right) => !left.Equals(right);

		#endregion

		/// <summary>
		/// Implicit cast operator <see cref="ulong"/> -> <see cref="Memory"/>. 
		/// </summary>
		public static implicit operator Memory(ulong bytes) => new (bytes);

		/// <summary>
		/// Implicit cast operator <see cref="Memory"/> -> <see cref="ulong"/>.
		/// </summary>
		public static implicit operator ulong(Memory memory) => memory.totalBytes;
	}
}