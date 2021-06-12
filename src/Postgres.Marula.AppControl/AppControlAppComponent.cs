using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.AppControl
{
	/// <inheritdoc />
	internal class AppControlAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
		{
		}
	}
}