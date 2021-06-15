using System;
using Microsoft.Extensions.Configuration;
using Postgres.Marula.Infrastructure.Configuration;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.App.Control.Configuration
{
	/// <inheritdoc cref="IControlAppConfiguration" />
	internal class ControlAppConfiguration : ConfigurationBase<ControlAppComponent>, IControlAppConfiguration
	{
		public ControlAppConfiguration(IConfiguration configuration) : base(configuration)
		{
		}

		/// <inheritdoc />
		Uri IControlAppConfiguration.HostApiUri()
			=> ConfigurationSection
				.GetSection("HostApiUri")
				.Value
				.To(uriString => new Uri(uriString));
	}
}