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
		async Task<RawParameterValue> IDatabaseServer.GetRawParameterValueAsync(IParameterLink parameterLink)
		{
			await Task.CompletedTask;
			return parameterLink.Name.ToString() switch
			{
				"max_connections" => new RawParameterValue("100"),
				_ => throw new NotSupportedException($"Parameter '{parameterLink.Name}' can't be handled by fake service.")
			};
		}

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

		/// <inheritdoc />
		Task<Fraction> IDatabaseServer.GetAverageBloatFractionAsync() => Task.FromResult((Fraction) 0.5M);

		/// <inheritdoc />
		async IAsyncEnumerable<ParentToChild> IDatabaseServer.GetAllHierarchicalLinks()
		{
			await Task.CompletedTask;
			yield return new("public.test_table", "public.partition_0");
			yield return new("public.test_table", "public.partition_1");
			yield return new("public.test_table", "public.partition_2");
			yield return new("public.test_table", "other_schema.partition_3");
			yield return new("public.partition_0", "public.partition_0_0");
		}

		public bool ApplyMethodWasCalled { get; private set; }
	}

	internal interface IDatabaseServerAccessTracker
	{
		bool ApplyMethodWasCalled { get; }
	}
}