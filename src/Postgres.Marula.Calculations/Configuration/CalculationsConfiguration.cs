using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Configuration
{
	/// <inheritdoc cref="ICalculationsConfiguration"/>
	internal class CalculationsConfiguration :
		ConfigurationBase<CalculationsAppComponent>,
		ICalculationsConfiguration
	{
		public CalculationsConfiguration(IConfiguration configuration) : base(configuration)
		{
		}

		/// <inheritdoc />
		PositiveTimeSpan ICalculationsConfiguration.RecalculationInterval()
			=> ConfigurationSection
				.GetSection("General:RecalculationIntervalInSeconds")
				.To(ParseFromSeconds);

		/// <inheritdoc />
		bool ICalculationsConfiguration.AutoAdjustmentIsEnabled()
			=> ConfigurationSection
				.GetSection("General:AutoAdjustParams")
				.Value
				.To(bool.Parse);

		/// <inheritdoc />
		PositiveTimeSpan ICalculationsConfiguration.LsnTrackingInterval()
			=> ConfigurationSection
				.GetSection("Wal:MaxWalSize:LsnTrackingIntervalInSeconds")
				.To(ParseFromSeconds);

		/// <inheritdoc />
		PositiveTimeSpan ICalculationsConfiguration.MovingAverageWindow()
			=> ConfigurationSection
				.GetSection("Wal:MaxWalSize:MovingAverageWindowInSeconds")
				.To(ParseFromSeconds);

		/// <summary>
		/// Parse value from <paramref name="configurationSection"/> to <see cref="PositiveTimeSpan"/> value. 
		/// </summary>
		private static PositiveTimeSpan ParseFromSeconds(IConfigurationSection configurationSection)
			=> configurationSection
				.Value
				.To(stringValue => double.Parse(stringValue, CultureInfo.InvariantCulture))
				.To(TimeSpan.FromSeconds);
	}
}