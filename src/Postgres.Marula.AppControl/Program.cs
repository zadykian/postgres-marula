using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.Hosting;

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
			=> CommonHostBuilder
				.WithConfig(args, "marula-ctl-config")
				.ConfigureServices(services => services.AddComponent<AppControlAppComponent>())
				.RunConsoleAsync();
	}
}