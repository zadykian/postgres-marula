using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			var fileName = $"marula-profile-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.sql";
			var fullPath = Path.Combine(Environment.CurrentDirectory, fileName);
			var alterSystemLines = valueViews.Select(view => (string) view.AsAlterSystem());
			await File.WriteAllLinesAsync(fullPath, alterSystemLines);
			return fileName;
		}
	}
}