using System.Threading.Tasks;
using NUnit.Framework;
using Postgres.Marula.Calculations.Parameters.Autovacuum.Bloat;
using Postgres.Marula.Infrastructure.TypeDecorators;
using Postgres.Marula.Tests.Calculations.Base;

namespace Postgres.Marula.Tests.Calculations
{
	/// <summary>
	/// Average bloat analysis tests.
	/// </summary>
	internal class BloatAnalysisTest : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Execute analysis.
		/// </summary>
		[Test]
		public async Task ExecuteAnalysisTest()
		{
			var bloatAnalysis = GetService<IBloatAnalysis>();
			var bloatCoefficients = await bloatAnalysis.ExecuteAsync();
			Assert.AreNotEqual(default(double), bloatCoefficients.TrendCoefficient);
			Assert.AreNotEqual(default(Fraction), bloatCoefficients.BloatConstant);
		}
	}
}