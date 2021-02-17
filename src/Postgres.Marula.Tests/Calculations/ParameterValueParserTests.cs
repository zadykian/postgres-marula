using NUnit.Framework;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.ParameterValueParsing;
using Postgres.Marula.Calculations.ParameterValues;
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
			const string parameterName = "autovacuum_naptime";
			var rawParameterValue = new RawParameterValue("30s", (1, 2147483));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterName, rawParameterValue);

			Assert.IsInstanceOf<TimeSpanParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.Milliseconds, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
		}

		/// <summary>
		/// Parse memory parameter value.
		/// </summary>
		[Test]
		public void ParseMemoryParameterValueTest()
		{
			const string parameterName = "effective_cache_size";
			var rawParameterValue = new RawParameterValue("4GB", (1, 2147483647));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterName, rawParameterValue);

			Assert.IsInstanceOf<MemoryParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.Bytes, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
		}

		/// <summary>
		/// Parse value of parameter represented as fraction in range [0..1]. 
		/// </summary>
		[Test]
		public void ParseFactionParameterValueTest()
		{
			const string parameterName = "checkpoint_completion_target";
			var rawParameterValue = new RawParameterValue("0.5", (0, 1));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterName, rawParameterValue);

			Assert.IsInstanceOf<FractionParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.None, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
		}

		/// <summary>
		/// Parse value of parameter represented as percents in range [0..100]. 
		/// </summary>
		[Test]
		public void ParsePercentsFactionParameterValueTest()
		{
			const string parameterName = "autovacuum_vacuum_scale_factor";
			var rawParameterValue = new RawParameterValue("0.8", (0, 100));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterName, rawParameterValue);

			Assert.IsInstanceOf<FractionParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.None, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
		}

		/// <summary>
		/// Parse boolean parameter value. 
		/// </summary>
		[Test]
		public void ParseBooleanParameterValueTest([Values(true, false)] bool underlyingValue)
		{
			const string parameterName = "autovacuum";
			var rawParameterValue = new RawParameterValue(underlyingValue ? "on" : "off");

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterName, rawParameterValue);

			Assert.IsInstanceOf<BooleanParameterValue>(parameterValue);
			Assert.AreEqual(ParameterUnit.None, parameterValue.Unit);
			Assert.AreEqual(parameterName, parameterValue.ParameterLink.Name.ToString());
			Assert.AreEqual(underlyingValue, ((BooleanParameterValue) parameterValue).Value);
		}
	}
}