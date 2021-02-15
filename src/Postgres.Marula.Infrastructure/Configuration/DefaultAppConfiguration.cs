using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Configuration
{
	/// <inheritdoc />
	internal class DefaultAppConfiguration : IAppConfiguration
	{
		private readonly IConfiguration configuration;

		public DefaultAppConfiguration(IConfiguration configuration) => this.configuration = configuration;

		/// <inheritdoc />
		ConnectionString IAppConfiguration.GetConnectionString()
			=> configuration
				.GetConnectionString("Default")
				.To(connectionString => new ConnectionString(connectionString));

		/// <inheritdoc />
		PositiveTimeSpan IAppConfiguration.GetRecalculationInterval()
			=> configuration
				.GetSection("DynamicCalculation")
				.GetChildren()
				.Single(section => section.Key == "RecalculationIntervalInSeconds")
				.Value
				.To(double.Parse)
				.To(TimeSpan.FromSeconds);

		/// <inheritdoc />
		bool IAppConfiguration.AutoAdjustIsEnabled()
			=> configuration
				.GetSection("DynamicCalculation")
				.GetChildren()
				.Single(section => section.Key == "AutoAdjustParams")
				.Value
				.To(bool.Parse);
	}
}