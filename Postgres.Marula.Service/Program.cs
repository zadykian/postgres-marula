using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Postgres.Marula.Service
{
	internal static class Program
	{
		private static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

		private static IHostBuilder CreateHostBuilder(string[] args)
			=> Host
				.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) => services.AddHostedService<Worker>());
	}
}