using System;
using System.Globalization;
using System.Linq;
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
		IGeneralConfiguration ICalculationsConfiguration.General()
			=> ConfigurationSection
				.GetSection("General")
				.To(subSection => new GeneralConfiguration(subSection));

		/// <inheritdoc />
		IPeriodicLoggingConfiguration ICalculationsConfiguration.Autovacuum()
			=> ConfigurationSection
				.GetSection("Autovacuum")
				.To(subSection => new PeriodicLoggingConfiguration(subSection));

		/// <inheritdoc />
		IPeriodicLoggingConfiguration ICalculationsConfiguration.Wal()
			=> ConfigurationSection
				.GetSection("Wal")
				.To(subSection => new PeriodicLoggingConfiguration(subSection));

		/// <summary>
		/// Parse value from <paramref name="configurationSection"/> to <see cref="PositiveTimeSpan"/> value. 
		/// </summary>
		private static PositiveTimeSpan ParseFromSeconds(IConfigurationSection configurationSection)
			=> configurationSection
				.Value
				.To(stringValue => double.Parse(stringValue, CultureInfo.InvariantCulture))
				.To(TimeSpan.FromSeconds);

		/// <inheritdoc />
		private sealed class GeneralConfiguration : IGeneralConfiguration
		{
			private readonly IConfigurationSection configurationSection;

			public GeneralConfiguration(IConfigurationSection configurationSection)
				=> this.configurationSection = configurationSection;

			/// <inheritdoc />
			PositiveTimeSpan IGeneralConfiguration.RecalculationInterval()
				=> configurationSection
					.GetSection("RecalculationIntervalInSeconds")
					.To(ParseFromSeconds);

			/// <inheritdoc />
			bool IGeneralConfiguration.AutoAdjustmentIsEnabled()
				=> configurationSection
					.GetSection("AutoAdjustParams")
					.Value
					.To(bool.Parse);

			/// <inheritdoc />
			Uri IGeneralConfiguration.AgentApiUri()
				=> configurationSection
					.GetSection("AgentEndpoint")
					.Value
					.To(uriString => new Uri(uriString));
		}

		/// <inheritdoc />
		private sealed class PeriodicLoggingConfiguration : IPeriodicLoggingConfiguration
		{
			private readonly IConfigurationSection configurationSection;

			public PeriodicLoggingConfiguration(IConfigurationSection configurationSection)
				=> this.configurationSection = configurationSection;

			/// <inheritdoc />
			PositiveTimeSpan IPeriodicLoggingConfiguration.Interval()
				=> configurationSection
					.GetChildren()
					.Single(section => section.Key.Contains("IntervalInSeconds"))
					.To(ParseFromSeconds);

			/// <inheritdoc />
			PositiveTimeSpan IPeriodicLoggingConfiguration.MovingAverageWindow()
				=> configurationSection
					.GetSection("MovingAverageWindowInSeconds")
					.To(ParseFromSeconds);
		}
	}
}