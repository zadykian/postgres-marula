using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Base;

// ReSharper disable UnusedType.Global

namespace Postgres.Marula.Calculations.Parameters.Autovacuum
{
	/// <summary>
	/// [autovacuum]
	/// Controls whether the server should run the autovacuum launcher daemon.
	/// </summary>
	internal class Autovacuum : BooleanParameterBase
	{
		/// <inheritdoc />
		protected override ValueTask<bool> CalculateValueAsync() => ValueTask.FromResult(true);
	}
}