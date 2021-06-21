using System.Collections.Generic;
using System.Threading.Tasks;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.TypeDecorators;

namespace Postgres.Marula.App.Control.ValuesExport
{
	/// <inheritdoc />
	internal class LocalValuesExport : IValuesExport
	{
		/// <inheritdoc />
		async Task<NonEmptyString> IValuesExport.ExportAsync(IEnumerable<IValueView> valueViews)
		{
			await Task.CompletedTask;
			return "test.sql";
		}
	}
}