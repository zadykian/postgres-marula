using System.Threading.Tasks;
using Postgres.Marula.Calculations.Parameters.Base.Dependencies;
using Postgres.Marula.Calculations.ParameterValues.Base;

namespace Postgres.Marula.Calculations.Parameters.Base
{
	/// <summary>
	/// Database server parameter.
	/// </summary>
	public interface IParameter : IParameterLink
	{
		/// <summary>
		/// Dependencies of parameter.
		/// All parameters which are configured as dependencies must be calculated before current one.
		/// </summary>
		IParameterDependencies Dependencies();

		/// <summary>
		/// Calculate parameter value. 
		/// </summary>
		Task<IParameterValue> CalculateAsync();
	}

	/// <summary>
	/// Extension methods for <see cref="IParameter"/> type.
	/// </summary>
	internal static class ParameterExtensions
	{
		/// <summary>
		/// Get link to database parameter <paramref name="parameter"/>. 
		/// </summary>
		public static IParameterLink GetLink(this IParameter parameter) => new ParameterLink(parameter.Name);
	}
}