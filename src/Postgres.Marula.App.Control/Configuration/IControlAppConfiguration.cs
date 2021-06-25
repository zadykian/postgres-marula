using System;

namespace Postgres.Marula.App.Control.Configuration
{
	/// <summary>
	/// Control application configuration.
	/// </summary>
	internal interface IControlAppConfiguration
	{
		/// <summary>
		/// URI of host application API.
		/// </summary>
		Uri HostApiUri();
	}
}