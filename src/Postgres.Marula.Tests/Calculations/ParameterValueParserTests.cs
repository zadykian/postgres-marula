using NUnit.Framework;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Parsing;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.TypeDecorators;
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
			var rawParameterValue = new RawRangeParameterValue("30s", RawValueType.Integer, (1, 2147483));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<TimeSpanParameterValue>(parameterValue);
			Assert.AreEqual(new IUnit.Milliseconds(), parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse memory parameter value.
		/// </summary>
		[Test]
		public void ParseMemoryParameterValueTest()
		{
			var parameterLink = new ParameterLink("effective_cache_size");
			var rawParameterValue = new RawRangeParameterValue("4GB", RawValueType.Integer, (1, 2147483647));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<MemoryParameterValue>(parameterValue);
			// 4GB is less then 10GB, so it's normalized to megabytes.
			Assert.AreEqual(new IUnit.Mem(Memory.Unit.Megabytes), parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse value of parameter represented as fraction in range [0..1].
		/// </summary>
		[Test]
		public void ParseFactionParameterValueTest()
		{
			var parameterLink = new ParameterLink("checkpoint_completion_target");
			var rawParameterValue = new RawRangeParameterValue("0.5", RawValueType.Real, (0, 1));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<FractionParameterValue>(parameterValue);
			Assert.AreEqual(new IUnit.None(), parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse value of parameter represented as percents in range [0..100].
		/// </summary>
		[Test]
		public void ParsePercentsFactionParameterValueTest()
		{
			var parameterLink = new ParameterLink("autovacuum_vacuum_scale_factor");
			var rawParameterValue = new RawRangeParameterValue("0.8", RawValueType.Real, (0, 100));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<FractionParameterValue>(parameterValue);
			Assert.AreEqual(new IUnit.None(), parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
		}

		/// <summary>
		/// Parse boolean parameter value.
		/// </summary>
		[Test]
		public void ParseBooleanParameterValueTest([Values(true, false)] bool underlyingValue)
		{
			var parameterLink = new ParameterLink("autovacuum");
			var rawParameterValue = new RawParameterValue(underlyingValue ? "on" : "off", RawValueType.Bool);

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<BooleanParameterValue>(parameterValue);
			Assert.AreEqual(new IUnit.None(), parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
			Assert.AreEqual(underlyingValue, ((BooleanParameterValue) parameterValue).Value);
		}

		/// <summary>
		/// Parse integer parameter value with range.
		/// </summary>
		[Test]
		public void ParseIntegerParameterValueWithRangeTest()
		{
			const int underlyingValue = 100;
			var parameterLink = new ParameterLink("max_connections");
			var rawParameterValue = new RawRangeParameterValue(underlyingValue.ToString(), RawValueType.Integer, (1,262143));

			var parameterValueParser = GetService<IParameterValueParser>();
			var parameterValue = parameterValueParser.Parse(parameterLink, rawParameterValue);

			Assert.IsInstanceOf<IntegerParameterValue>(parameterValue);
			Assert.AreEqual(new IUnit.None(), parameterValue.Unit);
			Assert.AreEqual(parameterLink, parameterValue.ParameterLink);
			Assert.AreEqual(underlyingValue, ((IntegerParameterValue) parameterValue).Value);
		}
	}
}