using System;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.AppComponents;

// ReSharper disable SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault

namespace Postgres.Marula.HwInfo
{
	/// <inheritdoc />
	public class HwInfoAppComponent : IAppComponent
	{
		/// <inheritdoc />
		IServiceCollection IAppComponent.RegisterServices(IServiceCollection serviceCollection)
			=> _ = Environment.OSVersion.Platform switch
			{
				PlatformID.Unix    => Use<BashHardwareInfo>(serviceCollection),
				PlatformID.Win32NT => Use<PowershellHardwareInfo>(serviceCollection),
				_ => throw new ApplicationException(
					$"Operating system '{Environment.OSVersion.VersionString}' is not supported.")
			};

		/// <summary>
		/// Use type <typeparamref name="THardwareInfo"/>
		/// as implementation of <see cref="IHardwareInfo"/> interface.
		/// </summary>
		private static IServiceCollection Use<THardwareInfo>(IServiceCollection serviceCollection)
			where THardwareInfo : class, IHardwareInfo
			=> serviceCollection.AddSingleton<IHardwareInfo, THardwareInfo>();
	}
}