using System;
using System.Collections.Generic;
using NUnit.Framework;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.Tests.Infrastructure
{
	/// <summary>
	/// Tests of <see cref="Memory"/> type.
	/// </summary>
	[TestFixture]
	internal class MemoryTests
	{
		/// <summary>
		/// Test of <see cref="Memory.Parse"/> method.
		/// </summary>
		[Test]
		[TestCaseSource(nameof(ParseTestCases))]
		public void ParseTest(ParseTestCase testCase)
		{
			var (input, expected) = testCase;
			var result = Memory.Parse(input);
			Assert.AreEqual(expected, result);
		}

		/// <summary>
		/// Test cases for <see cref="ParseTest"/>.
		/// </summary>
		private static IEnumerable<ParseTestCase> ParseTestCases() =>
			new ParseTestCase[]
			{
				new("0B",    Memory.Zero),
				new("1B",    Memory.Byte),
				new("1kB",   Memory.Kilobyte),
				new("1MB",   Memory.Megabyte),
				new("1GB",   Memory.Gigabyte),
				new("1TB",   Memory.Terabyte),
				new("32 GB", 32 * Memory.Gigabyte)
			};

		/// <summary>
		/// Test case for <see cref="ParseTest"/>.
		/// </summary>
		public sealed record ParseTestCase(NonEmptyString Input, Memory Expected);

		/// <summary>
		/// Test of operators defined in <see cref="Memory"/> type. 
		/// </summary>
		[Test]
		[TestCaseSource(nameof(OperatorTestCases))]
		public void OperatorTest(OperatorTestCase testCase)
		{
			var (operation, expected) = testCase;
			var result = operation();
			Assert.AreEqual(expected, result);
		}

		/// <summary>
		/// Test cases for <see cref="OperatorTest"/>.
		/// </summary>
		private static IEnumerable<OperatorTestCase> OperatorTestCases()
			=> new OperatorTestCase[]
			{
				new(() => 25 * Memory.Gigabyte,    Memory.Parse("25GB")),
				new(() => Memory.Terabyte * 32,    Memory.Parse("32TB")),
				new(() => Memory.Megabyte / 64,    Memory.Parse("16kB")),

				new(() => 0.25 * Memory.Kilobyte,  Memory.Parse("256B")),
				new(() => Memory.Megabyte * 0.125, Memory.Parse("128kB")),
				new(() => Memory.Gigabyte / 0.125, Memory.Parse("8GB")),

				new(() => 8M * Memory.Byte,        Memory.Parse("8B")),
				new(() => Memory.Megabyte * 48M,   Memory.Parse("48MB")),
				new(() => Memory.Gigabyte / 0.01M, Memory.Parse("100GB"))
			};

		/// <summary>
		/// Test case for <see cref="OperatorTest"/>.
		/// </summary>
		public sealed record OperatorTestCase(Func<Memory> Operation, Memory Expected);
	}
}