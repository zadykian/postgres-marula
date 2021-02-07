using System.Text.RegularExpressions;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Tests.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// Database objects naming conventions tests.
	/// </summary>
	internal class NamingConventionsTests : SingleComponentTestFixtureBase<DatabaseAccessSolutionComponent>
	{
		/// <summary>
		/// System schema name format test.
		/// </summary>
		[Test]
		public void SystemSchemaNameHasValidFormatTest()
		{
			var namingConventions = GetService<INamingConventions>();

			Regex
				.IsMatch(namingConventions.SystemSchemaName, @"^[a-z|A-Z|_]{1}[a-z|A-Z|_|\d]{1,62}$")
				.To(hasValidFormat => Assert.IsTrue(
					hasValidFormat,
					$"{nameof(INamingConventions.SystemSchemaName)} must be valid postgres object name."));
		}

		/// <summary>
		/// System schema name prefix test.
		/// </summary>
		[Test]
		public void SystemSchemaNameDoesNotHaveReservedPrefixTest()
		{
			var namingConventions = GetService<INamingConventions>();

			namingConventions
				.SystemSchemaName
				.StartsWith("pg_")
				.To(hasReservedPrefix => Assert.IsFalse(
					hasReservedPrefix,
					$"{nameof(INamingConventions.SystemSchemaName)} can't have prefix 'pg_' which is reserved for postgres internals."));
		}
	}
}