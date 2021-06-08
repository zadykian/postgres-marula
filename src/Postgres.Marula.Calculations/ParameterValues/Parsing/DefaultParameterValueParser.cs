using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Calculations.ParameterValues.Parsing
{
	/// <inheritdoc />
	internal class DefaultParameterValueParser : IParameterValueParser
	{
		/// <inheritdoc />
		IParameterValue IParameterValueParser.Parse(IParameterLink parameterLink, RawParameterValue rawParameterValue)
			=> rawParameterValue.Value switch
			{
				{ } when Regex.IsMatch(rawParameterValue.Value, "^[0-9]+(ms|s|min|h)$")
					=> PositiveTimeSpan
						.Parse(rawParameterValue.Value)
						.To(timeSpan => new TimeSpanParameterValue(parameterLink, timeSpan)),

				{ } when Regex.IsMatch(rawParameterValue.Value, "^[0-9]+(B|kB|MB|GB)$")
					=> Memory
						.Parse(rawParameterValue.Value)
						.To(memory => new MemoryParameterValue(parameterLink, memory)),

				{ } when TryParseDecimal(rawParameterValue.Value, out var decimalValue)
				         && rawParameterValue is RawRangeParameterValue rawRangeParameterValue
					=> ToFraction(decimalValue, rawRangeParameterValue.ValidRange)
						.To(fraction => new FractionParameterValue(parameterLink, fraction)),

				{ } when rawParameterValue.Value == "on"
					=> new BooleanParameterValue(parameterLink, value: true),

				{ } when rawParameterValue.Value == "off"
					=> new BooleanParameterValue(parameterLink, value: false),

				_ => throw new ParameterValueParseException(
					$"Failed to parse value '{rawParameterValue.Value}' of parameter '{parameterLink.Name}'.")
			};

		/// <summary>
		/// Try parse string <paramref name="input"/> to decimal value.
		/// </summary>
		private static bool TryParseDecimal(string input, out decimal decimalValue)
			=> decimal.TryParse(
				input,
				NumberStyles.Number,
				CultureInfo.InvariantCulture,
				out decimalValue);

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