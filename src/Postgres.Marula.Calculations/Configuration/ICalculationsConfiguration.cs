using System;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Configuration
{
	/// <summary>
	/// Application configuration.
	/// </summary>
	internal interface ICalculationsConfiguration
	{
		/// <inheritdoc cref="IGeneralConfiguration"/>
		IGeneralConfiguration General();

		/// <summary>
		/// Periodic bloat logging job configuration. 
		/// </summary>
		IPeriodicLoggingConfiguration Autovacuum();

		/// <summary>
		/// Periodic LSN logging job configuration. 
		/// </summary>
		IPeriodicLoggingConfiguration Wal();
	}

	/// <summary>
	/// General calculations configuration. 
	/// </summary>
	internal interface IGeneralConfiguration
	{
		/// <summary>
		/// Get parameters recalculation interval. 
		/// </summary>
		PositiveTimeSpan RecalculationInterval();

		/// <summary>
		/// Automatic database's parameters adjustment is enabled. 
		/// </summary>
		bool AutoAdjustmentIsEnabled();

		/// <summary>
		/// Remote agent API address. 
		/// </summary>
		Uri AgentApiUri();
	}

	/// <summary>
	/// Periodic logging process configuration.
	/// </summary>
	internal interface IPeriodicLoggingConfiguration
	{
		/// <summary>
		/// Interval of WAL insert location logging. 
		/// </summary>
		PositiveTimeSpan Interval();

		/// <summary>
		/// Window in seconds used to calculate average WAL traffic. 
		/// </summary>
		PositiveTimeSpan MovingAverageWindow();
	}
}