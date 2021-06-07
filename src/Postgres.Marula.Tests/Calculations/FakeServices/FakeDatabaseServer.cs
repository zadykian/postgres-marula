using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using TuplesCount = System.UInt32;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc cref="IDatabaseServer"/>
	internal class FakeDatabaseServer : IDatabaseServer, IDatabaseServerAccessTracker
	{
		/// <inheritdoc />
		Task IDatabaseServer.ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues)
		{
			ApplyMethodWasCalled = true;
			return Task.CompletedTask;
		}

		/// <inheritdoc />
		Task<RawParameterValue> IDatabaseServer.GetRawParameterValueAsync(IParameterLink parameterLink)
			=> throw new InvalidOperationException();

		/// <inheritdoc />
		ValueTask<ParameterContext> IDatabaseServer.GetParameterContextAsync(IParameterLink parameterLink)
			=> ValueTask.FromResult(ParameterContext.Sighup);

		/// <inheritdoc />
		Task<LogSeqNumber> IDatabaseServer.GetCurrentLogSeqNumberAsync()
			=> Task.FromResult(new LogSeqNumber());

		/// <inheritdoc />
		ValueTask<Version> IDatabaseServer.GetPostgresVersionAsync()
			=> ValueTask.FromResult(new Version(12, 5));

		/// <inheritdoc />
		Task<TuplesCount> IDatabaseServer.GetAverageTableSizeAsync() => Task.FromResult(10_240U);

		public bool ApplyMethodWasCalled { get; private set; }
	}

	internal interface IDatabaseServerAccessTracker
	{
		bool ApplyMethodWasCalled { get; }
	}
}