using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.Extensions;

namespace Postgres.Marula.AppControl
{
	/// <summary>
	/// Application entry point.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		/// Entry point method.
		/// </summary>
		private static Task Main(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				.AddJsonConfig("marula-ctl-config")
				.UseDefaultServiceProvider(options =>
				{
					options.ValidateScopes = true;
					options.ValidateOnBuild = true;
				})
				.ConfigureServices(services => services.AddComponent<AppControlAppComponent>())
				// todo
				.Build()
				.RunAsync();
	}
}