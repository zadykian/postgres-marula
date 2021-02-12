namespace Postgres.Marula.Calculations.Parameters.Properties
{
	/// <summary>
	/// Parameter calculation status.
	/// </summary>
	public enum ParameterStatus : byte
	{
		/// <summary>
		/// Parameter calculation is applied to database server.
		/// </summary>
		Applied = 1,

		/// <summary>
		/// Parameter calculation requires admin confirmation to be applied.
		/// </summary>
		RequiresConfirmation = 2,

		/// <summary>
		/// Parameter calculation applied to database
		/// but requires server restart to change actual value.
		/// </summary>
		RequiresServerRestart = 3,

		/// <summary>
		/// Parameter calculation requires both confirmation and server restart.
		/// </summary>
		RequiresConfirmationAndRestart = 4
	}
}