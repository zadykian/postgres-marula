using System.Collections.Generic;
using System.Net.Http;
using Postgres.Marula.App.Control.Configuration;
using Postgres.Marula.Calculations.PublicApi;
using Postgres.Marula.Infrastructure.Http;

namespace Postgres.Marula.App.Control.HostAccess
{
	/// <inheritdoc cref="IParameterValues" />
	/// <remarks>
	/// This implementations accesses remote host via HTTP.
	/// </remarks>
	internal class RemoteParameterValues : HttpComponentBase, IParameterValues
	{
		public RemoteParameterValues(IControlAppConfiguration configuration) : base(configuration.HostApiUri())
		{
		}

		/// <inheritdoc />
		async IAsyncEnumerable<IValueView> IParameterValues.MostRecentAsync()
		{
			var valueViews = await PerformRequestAsync<IEnumerable<ValueView>>(HttpMethod.Get, "ParameterValues/MostRecent");
			foreach (var valueView in valueViews) yield return valueView;
		}
	}
}