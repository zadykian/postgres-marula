using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.Agent;
using Postgres.Marula.Calculations.HardwareInfo;
using Postgres.Marula.HwInfo;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.HwInfo
{
	/// <summary>
	/// <see cref="RemoteHardwareInfo"/> tests.
	/// </summary>
	internal class RemoteHardwareInfoTests : HardwareInfoTests
	{
		private readonly Process agentApiProcess = CreateAgentProcess();

		/// <inheritdoc/>
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection.AddSingleton<IHardwareInfo, RemoteHardwareInfo>();
		}

		/// <summary>
		/// Start agent process.
		/// </summary>
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			agentApiProcess.Start();

			agentApiProcess
				.StandardError
				.ReadToEndAsync()
				.ContinueWith(task =>
				{
					if (string.IsNullOrWhiteSpace(task.Result)) return;
					TestContext.Error.WriteLine(task.Result);
				});
		}

		/// <inheritdoc cref="HardwareInfoTests.GetTotalMemoryTest"/>
		[Test]
		public new async Task GetTotalMemoryTest() => await base.GetTotalMemoryTest();

		/// <inheritdoc cref="HardwareInfoTests.GetCpuCoresCountTest"/>
		[Test]
		public new async Task GetCpuCoresCountTest() => await base.GetCpuCoresCountTest();

		/// <summary>
		/// Kill agent process.
		/// </summary>
		[OneTimeTearDown]
		public void OneTimeTearDown() => agentApiProcess.Kill();

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