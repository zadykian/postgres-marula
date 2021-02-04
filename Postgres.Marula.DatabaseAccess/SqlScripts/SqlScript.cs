namespace Postgres.Marula.DatabaseAccess.SqlScripts
{
	/// <summary>
	/// SQL script (objects creation, system data modification and so on).
	/// </summary>
	internal readonly struct SqlScript
	{
		public SqlScript(string content) => Content = content;

		/// <summary>
		/// Script's content - sequence of SQL commands, separated by ';' symbol.
		/// </summary>
		public string Content { get; }
	}
}