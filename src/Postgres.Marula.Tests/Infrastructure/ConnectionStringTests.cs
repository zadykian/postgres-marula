using System;
using NUnit.Framework;
using Postgres.Marula.Infrastructure.Extensions;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Infrastructure
{
	/// <summary>
	/// Database connection string tests.
	/// </summary>
	[TestFixture]
	internal class ConnectionStringTests
	{
		[Test]
		public void SingleKeyValueTest()
			=> Assert.IsNotEmpty(new ConnectionString("server=localhost;"));

		[Test]
		public void KeyWithoutValueTest()
			=> Assert.Throws<ArgumentException>(() => _ = new ConnectionString("server="));

		[Test]
		public void DefaultLocalConnectionStringTest()
			=> Assert.IsNotEmpty(
				new ConnectionString("server=localhost; port=5432; database=postgres; username=postgres; password=postgres;"));

		[Test]
		public void InvalidCharInKeyTest()
			=> Assert.Throws<ArgumentException>(() => _ = new ConnectionString("ser;ver=localhost;"));

		[Test]
		public void InvalidCharInValueTest()
			=> Assert.Throws<ArgumentException>(() => _ = new ConnectionString("server=local=host;"));

		[Test]
		public void WhiteSpacesTest()
			=> Assert.IsNotEmpty(new ConnectionString("\tserver = localhost ; "));

		[Test]
		public void FromStringValueTest()
			=> "\tserver = localhost ; "
				.To(stringValue => (ConnectionString) stringValue)
				.To(connectionString => Assert.IsNotEmpty(connectionString));
	}
}