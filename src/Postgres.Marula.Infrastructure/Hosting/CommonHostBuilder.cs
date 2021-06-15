using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Hosting
{
	/// <summary>
	/// Application generic host builder shared
	/// among all executables - main host, agent, control app, etc.
	/// </summary>
	public static class CommonHostBuilder
	{
		/// <summary>
		/// Create <see cref="IHostBuilder"/> instance configured
		/// with json configuration file named <paramref name="jsonFileName"/>.
		/// </summary>
		public static IHostBuilder WithJsonConfig(
			IEnumerable<string> args,
			NonEmptyString jsonFileName)
			=> Host
				.CreateDefaultBuilder(args.ToArray())
				.AddJsonConfig(jsonFileName)
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				});
	}
}