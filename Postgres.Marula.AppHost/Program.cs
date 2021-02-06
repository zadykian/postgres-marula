using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure.SolutionComponents.Factory;

namespace Postgres.Marula.AppHost
{
	internal static class Program
	{
		/// <summary>
		/// Application entry point. 
		/// </summary>
		private static async Task Main(string[] args)
		{
			var componentsFactory = new SolutionComponentsFactory();
			var hostBuilder = Host.CreateDefaultBuilder(args);
			await new Application(componentsFactory, hostBuilder).RunAsync();
		}
	}
}