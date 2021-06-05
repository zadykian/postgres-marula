using System.Linq;

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
	}
}