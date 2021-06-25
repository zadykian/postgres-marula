using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.PublicApi;

using FileName = Postgres.Marula.Infrastructure.TypeDecorators.NonEmptyString;

namespace Postgres.Marula.App.Control.ValuesExport
{
	/// <summary>
	/// Service to export parameter values.
	/// </summary>
	internal interface IValuesExport
	{
		/// <summary>
		/// Export parameter values. 
		/// </summary>
		Task<FileName> ExportAsync(IEnumerable<IValueView> valueViews);
	}
}