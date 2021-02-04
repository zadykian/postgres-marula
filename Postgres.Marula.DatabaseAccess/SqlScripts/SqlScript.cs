using Postgres.Marula.Infrastructure.Types;

namespace Postgres.Marula.DatabaseAccess.SqlScripts
{
	internal readonly struct SqlScript
	{
		public SqlScript(NonEmptyString content) => Content = content;

		public NonEmptyString Content { get; }
	}
}