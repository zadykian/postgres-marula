using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Postgres.Marula.DatabaseAccess.Conventions;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Tests.DatabaseAccess.Base;

namespace Postgres.Marula.Tests.DatabaseAccess
{
	/// <summary>
	/// Database objects naming conventions tests.
	/// </summary>
	internal class NamingConventionsTests : DatabaseAccessTestFixtureBase
	{
		/// <inheritdoc />
		protected override void ConfigureServices(IServiceCollection serviceCollection)
		{
			base.ConfigureServices(serviceCollection);
			serviceCollection.AddSingleton<INamingConventions, DefaultNamingConventions>();
		}

		/// <summary>
		/// System schema name format test.
		/// </summary>
		[Test]
		public void SystemSchemaNameHasValidFormatTest()
			=> GetService<INamingConventions>()
				.To(namingConventions => Regex.IsMatch(
					namingConventions.SystemSchemaName,
					@"^[a-z|A-Z|_]{1}[a-z|A-Z|_|\d]{1,62}$"))
				.To(hasValidFormat => Assert.IsTrue(
					hasValidFormat,
					$"{nameof(INamingConventions.SystemSchemaName)} must be valid postgres object name."));

		/// <summary>
		/// System schema name prefix test.
		/// </summary>
		[Test]
		public void SystemSchemaNameDoesNotHaveReservedPrefixTest()
			=> GetService<INamingConventions>()
				.To(namingConventions => namingConventions
					.SystemSchemaName
					.ToString()
					.StartsWith("pg_"))
				.To(hasReservedPrefix => Assert.IsFalse(
					hasReservedPrefix,
					$"{nameof(INamingConventions.SystemSchemaName)} can't have prefix 'pg_' which is reserved for postgres internals."));
	}
}