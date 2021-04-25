using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Configuration
{
	/// <inheritdoc cref="ICalculationsConfiguration"/>
	internal class DefaultCalculationsConfiguration :
		ConfigurationBase<CalculationsAppComponent>,
		ICalculationsConfiguration
	{
		public DefaultCalculationsConfiguration(IConfiguration configuration) : base(configuration)
		{
		}

		/// <inheritdoc />
		PositiveTimeSpan ICalculationsConfiguration.RecalculationInterval()
			=> ConfigurationSection
				.GetSection("General:RecalculationIntervalInSeconds")
				.Value
				.To(stringValue => double.Parse(stringValue, CultureInfo.InvariantCulture))
				.To(TimeSpan.FromSeconds);

		/// <inheritdoc />
		bool ICalculationsConfiguration.AutoAdjustmentIsEnabled()
			=> ConfigurationSection
				.GetSection("General:AutoAdjustParams")
				.Value
				.To(bool.Parse);

		/// <inheritdoc />
		PositiveTimeSpan ICalculationsConfiguration.LsnTrackInterval()
			=> ConfigurationSection
				.GetSection("Wal:LsnTrackIntervalInSeconds")
				.Value
				.To(double.Parse)
				.To(TimeSpan.FromSeconds);
	}
}