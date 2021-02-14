using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.Parameters.Properties;
using Postgres.Marula.Calculations.Parameters.Values;
using Postgres.Marula.Calculations.Parameters.Values.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Calculations.FakeParameters
{
	/// <inheritdoc />
	internal class SharedBuffersFakeParameter : IParameter
	{
		NonEmptyString IParameterLink.Name => "shared_buffers";

		/// <inheritdoc />
		ParameterContext IParameter.Context => ParameterContext.Postmaster;

		/// <inheritdoc />
		IParameterValue IParameter.Calculate() => new MemoryParameterValue(this.GetLink(), 1024 * 1024 * 1024);
	}
}