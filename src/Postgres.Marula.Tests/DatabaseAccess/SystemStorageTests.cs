using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.DatabaseAccess.ConnectionFactory;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// System storage tests.
	/// </summary>
	internal class SystemStorageTests : DatabaseAccessTestFixtureBase
	{
		/// <summary>
		/// Insert empty collection of values. 
		/// </summary>
		[Test]
		public async Task InsertEmptyCollectionTest()
		{
			var systemStorage = GetService<ISystemStorage>();
			await systemStorage.SaveParameterValuesAsync(ImmutableArray<ParameterValueWithStatus>.Empty);
			Assert.Pass();
		}

		/// <summary>
		/// Insert calculated values to system table. 
		/// </summary>
		[Test]
		public async Task InsertCalculatedValuesTest()
		{
			await InsertTestParameters();

			var parameterValues = new ParameterValueWithStatus[]
			{
				new (
					new TimeSpanParameterValue(
						new ParameterLink("deadlock_timeout"),
						TimeSpan.FromMilliseconds(value: 800)),
					CalculationStatus.Applied),

				new (
					new MemoryParameterValue(
						new ParameterLink("log_rotation_size"),
						new Memory(16 * 1024 * 1024)),
					CalculationStatus.RequiresConfirmation),

				new (
					new MemoryParameterValue(
						new ParameterLink("wal_buffers"),
						new Memory(8 * 1024 * 1024)),
					CalculationStatus.RequiresServerRestart),

				new (
					new MemoryParameterValue(
						new ParameterLink("shared_buffers"),
						new Memory(2 * 1024 * 1024 * 1024UL)),
					CalculationStatus.RequiresConfirmationAndRestart)
			};

			var systemStorage = GetService<ISystemStorage>();
			await systemStorage.SaveParameterValuesAsync(parameterValues);
			Assert.Pass();
		}

		/// <summary>
		/// Insert (if not exists) test parameters to system dictionary.
		/// </summary>
		private async Task InsertTestParameters()
		{
			var namingConventions = GetService<INamingConventions>();

			var commandText = $@"
				insert into {namingConventions.SystemSchemaName}.{namingConventions.ParametersTableName}
					(name, unit)
				values
					('deadlock_timeout', 'ms'),
					('log_rotation_size', 'bytes'),
					('wal_buffers', 'bytes'),
					('shared_buffers', 'bytes')
				on conflict (name) do nothing;";

			var dbConnection = await GetService<IPreparedDbConnectionFactory>().GetPreparedConnectionAsync();
			await dbConnection.ExecuteAsync(commandText);
		}
	}
}