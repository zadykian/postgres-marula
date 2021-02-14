using System;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class AutovacuumVacuumCostDelayFakeParameter : IParameter
	{
		/// <inheritdoc />
		NonEmptyString IParameterLink.Name => "autovacuum_vacuum_cost_delay";

		/// <inheritdoc />
		ParameterContext IParameter.Context => ParameterContext.Sighup;

		/// <inheritdoc />
		IParameterValue IParameter.Calculate() => new TimeSpanParameterValue(
			this.GetLink(),
			TimeSpan.FromSeconds(value: 1));
	}
}