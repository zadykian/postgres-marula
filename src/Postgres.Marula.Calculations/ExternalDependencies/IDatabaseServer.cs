using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.ParameterProperties;
using Postgres.Marula.Calculations.Parameters.Base;
using Postgres.Marula.Calculations.ParameterValues.Base;
using Postgres.Marula.Calculations.ParameterValues.Raw;
using Postgres.Marula.Infrastructure.TypeDecorators;

// ReSharper disable BuiltInTypeReferenceStyle
using TuplesCount = System.UInt32;

namespace Postgres.Marula.Calculations.ExternalDependencies
{
	/// <summary>
	/// Database server being tuned.
	/// </summary>
	public interface IDatabaseServer
	{
		/// <summary>
		/// Apply parameter values <paramref name="parameterValues"/>
		/// to database server configuration.
		/// </summary>
		Task ApplyToConfigurationAsync(IReadOnlyCollection<IParameterValue> parameterValues);

		/// <summary>
		/// Get raw value of parameter <paramref name="parameterLink"/>.
		/// </summary>
		Task<RawParameterValue> GetRawParameterValueAsync(IParameterLink parameterLink);

		/// <summary>
		/// Get context of parameter <paramref name="parameterLink"/>. 
		/// </summary>
		ValueTask<ParameterContext> GetParameterContextAsync(IParameterLink parameterLink);

		/// <summary>
		/// Get current Write-Ahead Log insert location.
		/// </summary>
		Task<LogSeqNumber> GetCurrentLogSeqNumberAsync();

		/// <summary>
		/// Get current PostgreSQL version. 
		/// </summary>
		ValueTask<Version> GetPostgresVersionAsync();

		/// <summary>
		/// Get average table size (in tuples) among non-empty tables.
		/// </summary>
		Task<TuplesCount> GetAverageTableSizeAsync();

		/// <summary>
		/// Get average bloat fraction among non-empty tables. 
		/// </summary>
		Task<Fraction> GetAverageBloatFractionAsync();

		/// <summary>
		/// Get all parent to child links between tables in database.
		/// </summary>
		IAsyncEnumerable<ParentToChild> GetAllHierarchicalLinks();
	}
}