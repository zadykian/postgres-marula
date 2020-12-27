using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Postgres.Marula.AppHost
{
	internal static class Program
	{
		private static async Task Main(string[] args)
			=> await Host
				.CreateDefaultBuilder(args)
				.ConfigureServices((_, services) => services.AddHostedService<Worker>())
				.Build()
				.RunAsync();
	}
}