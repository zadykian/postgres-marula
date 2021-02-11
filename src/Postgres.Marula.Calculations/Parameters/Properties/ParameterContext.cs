namespace Postgres.Marula.Calculations.Parameters.Properties
{
	/// <summary>
	/// Database parameter context.
	/// Corresponds to 'pg_catalog.pg_settings.context' value.
	/// </summary>
	internal enum ParameterContext : byte
	{
		/// <summary>
		/// These settings can only be applied when the server starts,
		/// so any change requires restarting the server.
		/// </summary>
		Postmaster = 1,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// SIGHUP signal to postmaster process is required to re-read configuration.
		/// </summary>
		Sighup = 2,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session (only by superuser).
		/// However, these settings never change in a session after it is started.
		/// The new values will only affect subsequently-launched sessions.
		/// </summary>
		SuperuserBackend = 3,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session (by any user).
		/// However, these settings never change in a session after it is started.
		/// The new values will only affect subsequently-launched sessions.
		/// </summary>
		Backend = 4,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session via the SET command (only by superuser).
		/// Global changes will affect existing sessions only if no session-local value has been established with SET.
		/// </summary>
		Superuser = 5,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session via the SET command (by any user).
		/// Global changes will affect existing sessions only if no session-local value has been established with SET.
		/// </summary>
		User = 6
	}
}