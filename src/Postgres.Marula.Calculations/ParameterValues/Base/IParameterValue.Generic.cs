namespace Postgres.Marula.Calculations.ParameterValues.Base
{
	/// <inheritdoc />
	public interface IParameterValue<out T> : IParameterValue
	{
		/// <summary>
		/// Value or parameter.
		/// </summary>
		T Value { get; }
	}
}