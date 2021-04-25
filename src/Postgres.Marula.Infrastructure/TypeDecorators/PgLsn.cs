namespace Postgres.Marula.Infrastructure.TypeDecorators
{
	/// <summary>
	/// 'pg_catalog.pg_lsn' data type object representation. 
	/// </summary>
	public readonly struct PgLsn
	{
		private readonly uint major;
		private readonly uint minor;

		/// <param name="major">
		/// Major LSN part.
		/// </param>
		/// <param name="minor">
		/// Minor LSN part.
		/// </param>
		public PgLsn(uint major, uint minor)
		{
			this.major = major;
			this.minor = minor;
		}

		/// <inheritdoc />
		public override string ToString() => $"{major:x8}/{minor:x8}";

		/// <summary>
		/// Operator which calculates the difference in bytes
		/// between two LSN values. 
		/// </summary>
		public static ulong operator -(PgLsn left, PgLsn right)
		{
			var majorDiff = (ulong) (left.major - right.major) << 32;
			var minorDiff = left.minor - right.minor;
			return majorDiff + minorDiff;
		}
	}
}