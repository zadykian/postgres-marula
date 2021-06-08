using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

namespace Postgres.Marula.HwInfo
{
	/// <inheritdoc />
	public class HwInfoAppComponent : IAppComponent
	{
		/// <inheritdoc />
		void IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> serviceCollection
				.AddSingleton<IHardwareInfo, BashHardwareInfo>();
	}
}