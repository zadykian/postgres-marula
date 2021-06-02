using NUnit.Framework;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Parsing;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Tests.Calculations.Base;

namespace Postgres.Marula.Tests.Calculations
{
	/// <summary>
	/// <see cref="IParameterValueParser"/> tests.
	/// </summary>
	internal class ParameterValueParserTests : CalculationsTestFixtureBase
	{
		/// <summary>
		/// Parse timespan parameter value.
		/// </summary>
		[Test]
		public void ParseTimeSpanParameterValueTest()
		{
			var parameterLink = new ParameterLink("autovacuum_naptime");
			var rawParameterValue = new RawRangeParameterValue("30s", (1, 2147483));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<TimeSpanParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.Milliseconds, parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse memory parameter value.
		/// </summary>
		[Test]
		public void ParseMemoryParameterValueTest()
		{
			var parameterLink = new ParameterLink("effective_cache_size");
			var rawParameterValue = new RawRangeParameterValue("4GB", (1, 2147483647));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<MemoryParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.Bytes, parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse value of parameter represented as fraction in range [0..1]. 
		/// </summary>
		[Test]
		public void ParseFactionParameterValueTest()
		{
			var parameterLink = new ParameterLink("checkpoint_completion_target");
			var rawParameterValue = new RawRangeParameterValue("0.5", (0, 1));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<FractionParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.None, parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse value of parameter represented as percents in range [0..100]. 
		/// </summary>
		[Test]
		public void ParsePercentsFactionParameterValueTest()
		{
			var parameterLink = new ParameterLink("autovacuum_vacuum_scale_factor");
			var rawParameterValue = new RawRangeParameterValue("0.8", (0, 100));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<FractionParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.None, parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse boolean parameter value. 
		/// </summary>
		[Test]
		public void ParseBooleanParameterValueTest([Values(true, false)] bool underlyingValue)
		{
			var parameterLink = new ParameterLink("autovacuum");
			var rawParameterValue = new RawParameterValue(underlyingValue ? "on" : "off");

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<BooleanParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.None, parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
			Assert.AreEqual(underlyingValue, ((BooleanParameterValue) parameterValue).Value);
		}
	}
}