using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Agent;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault

namespace Postgres.Marula.Tests
{
	/// <summary>
	/// Tests initialisation.
	/// </summary>
	[SetUpFixture]
	internal class SetUpFixture
	{
		private readonly Process agentApiProcess = CreateAgentProcess();

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

		/// <summary>
		/// Kill agent process.
		/// </summary>
		[OneTimeTearDown]
		public async Task OneTimeTearDown()
		{
			// let already running jobs to finish
			await Task.Delay(millisecondsDelay: 5000);
			agentApiProcess.Kill();
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