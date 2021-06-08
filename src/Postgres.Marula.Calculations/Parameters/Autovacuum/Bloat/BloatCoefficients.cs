using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat
{
	/// <summary>
	/// Bloat coefficients.
	/// </summary>
	public readonly struct BloatCoefficients
	{
		public BloatCoefficients(double trendCoefficient, Fraction bloatConstant)
		{
			TrendCoefficient = trendCoefficient;
			BloatConstant = bloatConstant;
		}

		/// <summary>
		/// Derivative of approximated linear function obtained by linear regression
		/// of average bloat selection.
		/// </summary>
		public double TrendCoefficient { get; }

		/// <summary>
		/// Constant part of approximated linear function. 
		/// </summary>
		public Fraction BloatConstant { get; }
	}
}