using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Postgres.Marula.Infrastructure;
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
			=> MarulaHost
				.WithConfig(args, "marula-ctl-config")
				.ConfigureServices(services => services.AddComponent<AppControlAppComponent>())
				// todo
				.Build()
				.RunAsync();
	}
}