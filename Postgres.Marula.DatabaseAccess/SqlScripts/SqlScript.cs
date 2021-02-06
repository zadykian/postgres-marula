using Postgres.Marula.Infrastructure.Types;

namespace Postgres.Marula.DatabaseAccess.SqlScripts
{
	/// <summary>
	/// SQL script (objects creation, system data modification and so on).
	/// </summary>
	internal readonly struct SqlScript
	{
		public SqlScript(
			NonEmptyString name,
			NonEmptyString content)
		{
			Name = name;
			Content = content;
		}

		/// <summary>
		/// Name of a script.
		/// </summary>
		public NonEmptyString Name { get; }
		
		/// <summary>
		/// Script's content - sequence of SQL commands, separated by ';' symbol.
		/// </summary>
		public NonEmptyString Content { get; }
	}
}