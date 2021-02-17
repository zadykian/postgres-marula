namespace Postgres.Marula.Calculations.ParameterProperties
{
	/// <summary>
	/// Database parameter context.
	/// Corresponds to 'pg_catalog.pg_settings.context' value.
	/// </summary>
	public enum ParameterContext : byte
	{
		/// <summary>
		/// These settings can only be applied when the server starts,
		/// so any change requires restarting the server.
		/// </summary>
		[StringRepresentation("postmaster")]
		Postmaster = 1,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// SIGHUP signal to postmaster process is required to re-read configuration.
		/// </summary>
		[StringRepresentation("sighup")]
		Sighup = 2,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session (only by superuser).
		/// However, these settings never change in a session after it is started.
		/// The new values will only affect subsequently-launched sessions.
		/// </summary>
		[StringRepresentation("superuser-backend")]
		SuperuserBackend = 3,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session (by any user).
		/// However, these settings never change in a session after it is started.
		/// The new values will only affect subsequently-launched sessions.
		/// </summary>
		[StringRepresentation("backend")]
		Backend = 4,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session via the SET command (only by superuser).
		/// Global changes will affect existing sessions only if no session-local value has been established with SET.
		/// </summary>
		[StringRepresentation("superuser")]
		Superuser = 5,

		/// <summary>
		/// Changes to these settings can be made without restarting the server.
		/// They can also be set for a particular session via the SET command (by any user).
		/// Global changes will affect existing sessions only if no session-local value has been established with SET.
		/// </summary>
		[StringRepresentation("user")]
		User = 6
	}

	/// <summary>
	/// Extension methods for <see cref="ParameterContext"/> type.
	/// </summary>
	internal static class ParameterContextExtensions
	{
		/// <summary>
		/// Check if database parameter with context <paramref name="parameterContext"/>
		/// can be changed without server restart.
		/// </summary>
		public static bool RestartIsRequired(this ParameterContext parameterContext)
			=> parameterContext == ParameterContext.Postmaster;
	}
}