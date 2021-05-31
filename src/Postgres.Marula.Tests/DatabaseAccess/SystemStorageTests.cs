using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.Calculations.ExternalDependencies;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// System storage tests.
	/// </summary>
	internal class SystemStorageTests : DatabaseAccessTestFixtureBase
	{
		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);

			serviceCollection
				.AddSingleton<IParameterLink>(new ParameterLink("deadlock_timeout"))
				.AddSingleton<IParameterLink>(new ParameterLink("log_rotation_size"))
				.AddSingleton<IParameterLink>(new ParameterLink("wal_buffers"))
				.AddSingleton<IParameterLink>(new ParameterLink("shared_buffers"));
		}

		/// <summary>
		/// Save empty collection of values. 
		/// </summary>
		[Test]
		public async Task SaveEmptyCollectionTest()
		{
			var systemStorage = GetService<ISystemStorage>();
			await systemStorage.SaveParameterValuesAsync(ImmutableArray<ParameterValueWithStatus>.Empty);
			Assert.Pass();
		}

		/// <summary>
		/// Save calculated values to system storage. 
		/// </summary>
		[Test]
		public async Task SaveCalculatedValuesTest()
		{
			var parameterValues = new ParameterValueWithStatus[]
			{
				new(
					new TimeSpanParameterValue(
						new ParameterLink("deadlock_timeout"),
						TimeSpan.FromMilliseconds(value: 800)),
					CalculationStatus.Applied),

				new(
					new MemoryParameterValue(
						new ParameterLink("log_rotation_size"),
						new Memory(16 * 1024 * 1024)),
					CalculationStatus.RequiresConfirmation),

				new(
					new MemoryParameterValue(
						new ParameterLink("wal_buffers"),
						new Memory(8 * 1024 * 1024)),
					CalculationStatus.RequiresServerRestart),

				new(
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
		/// Insert LSN to system storage. 
		/// </summary>
		[Test]
		public async Task SaveLogSeqNumberTest()
		{
			var logSeqNumber = new LogSeqNumber("16/1A0343D0");
			var systemStorage = GetService<ISystemStorage>();
			await systemStorage.SaveLogSeqNumberAsync(logSeqNumber);
			Assert.Pass();
		}

		/// <summary>
		/// Get most resent LSN values.
		/// </summary>
		[Test]
		public async Task GetLogSeqNumbersTest()
		{
			await SaveTestLsnHistoryValues();
			var systemStorage = GetService<ISystemStorage>();

			var lsnHistoryEntries = await systemStorage
				.GetLsnHistoryAsync(TimeSpan.FromHours(1))
				.ToArrayAsync();

			Assert.IsNotEmpty(lsnHistoryEntries);

			lsnHistoryEntries
				.Select(entry => entry.WalInsertLocation)
				.IsOrdered()
				.To(Assert.IsTrue);
		}

		/// <summary>
		/// Save test LSN history values.
		/// </summary>
		private async Task SaveTestLsnHistoryValues()
		{
			var lsnValues = new LogSeqNumber[]
			{
				new("16/1A0343D0"),
				new("16/1A0343E0"),
				new("16/2A0393F0"),
				new("17/00000001")
			};
			
			var systemStorage = GetService<ISystemStorage>();
			foreach (var lsn in lsnValues) await systemStorage.SaveLogSeqNumberAsync(lsn);
		}
	}
}