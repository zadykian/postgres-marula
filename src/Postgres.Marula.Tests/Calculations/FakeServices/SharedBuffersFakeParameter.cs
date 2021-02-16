using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Calculations.FakeServices
{
	/// <inheritdoc />
	internal class SharedBuffersFakeParameter : IParameter
	{
		NonEmptyString IParameterLink.Name => "shared_buffers";

		/// <inheritdoc />
		Task<ParameterContext> IParameter.GetContextAsync() => Task.FromResult(ParameterContext.Postmaster);

		/// <inheritdoc />
		IParameterValue IParameter.Calculate() => new MemoryParameterValue(this.GetLink(), 1024 * 1024 * 1024);
	}
}