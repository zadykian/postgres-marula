using Postgres.Marula.Infrastructure.Types;

namespace Postgres.Marula.DatabaseAccess.SqlScripts
{
	/// <summary>
	/// SQL script (objects creation, system data modification and so on).
	/// </summary>
	internal readonly struct SqlScript
	{
		public SqlScript(NonEmptyString content) => Content = content;

		/// <summary>
		/// Script's content - sequence of SQL commands, separated by ';' symbol.
		/// </summary>
		public NonEmptyString Content { get; }
	}
}