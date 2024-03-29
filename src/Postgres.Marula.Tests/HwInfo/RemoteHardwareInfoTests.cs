using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.App.Agent.Api;
using Postgres.Marula.Calculations.HardwareInfo;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.JsonSerialization;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault

namespace Postgres.Marula.Tests.HwInfo
{
	/// <summary>
	/// Tests of <see cref="RemoteHardwareInfo"/> which performs HTTP requests to remote agent's API.
	/// </summary>
	internal class RemoteHardwareInfoTests : HardwareInfoTestBase
	{
		private readonly Process remoteAgentApiProcess = CreateAgentProcess();

		/// <inheritdoc/>
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection.AddSingleton<IJsonConverters, JsonConverters>();
			serviceCollection.AddSingleton<IHardwareInfo, RemoteHardwareInfo>();
		}

		/// <summary>
		/// Start agent process.
		/// </summary>
		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			TestContext.WriteLine("agent process is starting.");

			remoteAgentApiProcess.Start();
			var errorOutputTask = remoteAgentApiProcess.StandardError.ReadToEndAsync();

			// check if agent process hadn't started successfully.
			if (await Task.WhenAny(
				errorOutputTask,
				Task.Delay(millisecondsDelay: 5000)) == errorOutputTask)
			{
				Assert.Fail($"failed to start agent process: {errorOutputTask.Result}.");
			}

			TestContext.WriteLine("agent process is started.");
		}

		/// <inheritdoc cref="HardwareInfoTestBase.GetTotalMemoryTestImpl"/>
		[Test]
		public async Task GetRemoteTotalMemoryTest() => await GetTotalMemoryTestImpl();

		/// <inheritdoc cref="HardwareInfoTestBase.GetCpuCoresCountTestImpl"/>
		[Test]
		public async Task GetRemoteCpuCoresCountTest() => await GetCpuCoresCountTestImpl();

		/// <summary>
		/// Kill agent process.
		/// </summary>
		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
			remoteAgentApiProcess.Kill();
			TestContext.WriteLine("agent process is killed.");
		}

		/// <summary>
		/// Create agent web api process. 
		/// </summary>
		private static Process CreateAgentProcess()
			=> new()
			{
				StartInfo = new()
				{
					FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AgentExecutableName()),
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}
			};

		/// <summary>
		/// Get agent executable file name. 
		/// </summary>
		private static NonEmptyString AgentExecutableName()
			=> typeof(HardwareInfoController)
				.Assembly
				.GetName()
				.Name!
				.To(assemblyName => Environment.OSVersion.Platform switch
				{
					PlatformID.Unix    => assemblyName,
					PlatformID.Win32NT => $"{assemblyName}.exe",
					_ => throw new ApplicationException()
				});
	}
}