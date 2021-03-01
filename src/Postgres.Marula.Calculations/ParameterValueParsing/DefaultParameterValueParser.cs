using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValueParsing
{
	/// <inheritdoc />
	internal class DefaultParameterValueParser : IParameterValueParser
	{
		/// <inheritdoc />
		IParameterValue IParameterValueParser.Parse(NonEmptyString parameterName, RawParameterValue rawParameterValue)
		{
			var parameterLink = new ParameterLink(parameterName);

			return rawParameterValue.Value switch
			{
				{ } when Regex.IsMatch(rawParameterValue.Value, "^[0-9]+(ms|s|min|h)$")
					=> ParseTimeSpan(rawParameterValue.Value)
						.To(timeSpan => new TimeSpanParameterValue(parameterLink, timeSpan)),

				{ } when Regex.IsMatch(rawParameterValue.Value, "^[0-9]+(B|kB|MB|GB)$")
					=> ParseMemory(rawParameterValue.Value)
						.To(memory => new MemoryParameterValue(parameterLink, memory)),

				{ } when decimal.TryParse(rawParameterValue.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var decimalValue)
				         && rawParameterValue is RawRangeParameterValue rawRangeParameterValue
					=> ToFraction(decimalValue, rawRangeParameterValue.ValidRange)
						.To(fraction => new FractionParameterValue(parameterLink, fraction)),

				{ } when rawParameterValue.Value == "on"
					=> new BooleanParameterValue(parameterLink, value: true),

				{ } when rawParameterValue.Value == "off"
					=> new BooleanParameterValue(parameterLink, value: false),

				_ => throw new ParameterValueParseException(
					$"Failed to parse value '{rawParameterValue.Value}' of parameter '{parameterName}'.")
			};
		}

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to timespan value.
		/// </summary>
		private static PositiveTimeSpan ParseTimeSpan(string stringToParse)
		{
			var (totalMilliseconds, unit) = ParseToTokens(stringToParse);

			var multiplier = unit switch
			{
				"ms"  => 1,
				"s"   => 1000,
				"min" => 60 * 1000,
				"h"   => 60 * 60 * 1000,
				_     => throw new ArgumentOutOfRangeException(nameof(stringToParse), stringToParse, message: null)
			};

			return TimeSpan.FromMilliseconds(totalMilliseconds * (ulong) multiplier);
		}

		/// <summary>
		/// Convert string <paramref name="stringToParse"/> to memory value.
		/// </summary>
		private static Memory ParseMemory(string stringToParse)
		{
			var (totalBytes, unit) = ParseToTokens(stringToParse);

			var multiplier = unit switch
			{
				"B"  => 1,
				"kB" => 1024,
				"MB" => 1024 * 1024,
				"GB" => 1024 * 1024 * 1024,
				_    => throw new ArgumentOutOfRangeException(nameof(stringToParse), stringToParse, message: null)
			};

			return new Memory(totalBytes * (ulong) multiplier);
		}

		/// <summary>
		/// Parse string <paramref name="stringToParse"/> to number and unit tokens.
		/// </summary>
		private static (ulong Value, string Unit) ParseToTokens(string stringToParse)
		{
			var value = stringToParse
				.TakeWhile(char.IsDigit)
				.ToArray()
				.To(charArray => new string(charArray))
				.To(ulong.Parse);

			var unit = stringToParse
				.SkipWhile(char.IsDigit)
				.ToArray()
				.To(charArray => new string(charArray));

			return (Value: value, Unit: unit);
		}

		/// <summary>
		/// Convert decimal value with range of valid values to <see cref="Fraction"/> instance.
		/// </summary>
		private static Fraction ToFraction(decimal value, Range<decimal> validRange)
		{
			var multiplier = (validRange.LeftBound, validRange.RightBound) switch
			{
				(decimal.Zero, decimal.One) => decimal.One,
				(decimal.Zero,         100) => 0.01M,
				_ => throw new ArgumentOutOfRangeException(nameof(validRange), validRange, message: null)
			};

			return new Fraction(value * multiplier);
		}
	}
}