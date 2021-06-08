using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Postgres.Marula.Agent;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests
{
	[SetUpFixture]
	internal class SetUpFixture
	{
		private readonly Process agentApiProcess = CreateAgentProcess();

		[OneTimeSetUp]
		public void OneTimeSetUp() => agentApiProcess.Start();

		[OneTimeTearDown]
		public void OneTimeTearDown() => agentApiProcess.Kill();

		private static Process CreateAgentProcess()
			=> new()
			{
				StartInfo = new()
				{
					FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AgentExecutableName()),
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				}
			};

		private static NonEmptyString AgentExecutableName()
			=> typeof(HardwareInfoController)
				.Assembly
				.GetName()
				.Name!
				.To(assemblyName => $"{assemblyName}.exe");
	}
}