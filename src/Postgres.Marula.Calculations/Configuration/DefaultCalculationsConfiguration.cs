using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Configuration
{
	/// <inheritdoc cref="ICalculationsConfiguration"/>
	internal class DefaultCalculationsConfiguration : ConfigurationBase, ICalculationsConfiguration
	{
		public DefaultCalculationsConfiguration(IConfiguration configuration) : base(configuration)
		{
		}

		/// <inheritdoc />
		PositiveTimeSpan ICalculationsConfiguration.GetRecalculationInterval()
			=> Configuration
				.GetSection("DynamicCalculation")
				.GetChildren()
				.Single(section => section.Key == "RecalculationIntervalInSeconds")
				.Value
				.To(double.Parse)
				.To(TimeSpan.FromSeconds);

		/// <inheritdoc />
		bool ICalculationsConfiguration.AutoAdjustIsEnabled()
			=> Configuration
				.GetSection("DynamicCalculation")
				.GetChildren()
				.Single(section => section.Key == "AutoAdjustParams")
				.Value
				.To(bool.Parse);
	}
}