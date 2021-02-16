using System;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class AutovacuumVacuumCostDelayFakeParameter : IParameter
	{
		/// <inheritdoc />
		NonEmptyString IParameterLink.Name => "autovacuum_vacuum_cost_delay";

		/// <inheritdoc />
		Task<ParameterContext> IParameter.GetContextAsync() => Task.FromResult(ParameterContext.Sighup);

		/// <inheritdoc />
		IParameterValue IParameter.Calculate() => new TimeSpanParameterValue(
			this.GetLink(),
			TimeSpan.FromSeconds(value: 1));
	}
}