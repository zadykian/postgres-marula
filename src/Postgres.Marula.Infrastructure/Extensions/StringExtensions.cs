using System.Linq;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Infrastructure.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="string"/> type.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Transform PascalCase string to snake_case. 
		/// </summary>
		public static string ToSnakeCase(this string pascalCaseString)
			=> pascalCaseString
				.Select((c, index) => char.IsUpper(c) && index > 0 ? $"_{c}" : c.ToString())
				.To(string.Concat)
				.ToLower();

		/// <summary>
		/// Parse string <paramref name="stringToParse"/> to number and unit tokens.
		/// </summary>
		internal static (ulong Value, string Unit) ParseToTokens(this NonEmptyString stringToParse)
		{
			var value = ((string) stringToParse)
				.TakeWhile(char.IsDigit)
				.ToArray()
				.To(charArray => new string(charArray))
				.To(ulong.Parse);

			var unit = ((string) stringToParse)
				.SkipWhile(char.IsDigit)
				.ToArray()
				.To(charArray => new string(charArray));

			return (Value: value, Unit: unit);
		}
	}
}