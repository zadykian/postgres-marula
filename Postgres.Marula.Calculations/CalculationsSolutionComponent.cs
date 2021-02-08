using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Postgres.Marula.Infrastructure.SolutionComponents;

// ReSharper disable UnusedType.Global

[assembly: InternalsVisibleTo("Postgres.Marula.Tests")]

namespace Postgres.Marula.Calculations
{
	/// <inheritdoc />
	internal class CalculationsSolutionComponent : ISolutionComponent
	{
		/// <inheritdoc />
		void ISolutionComponent.RegisterServices(IServiceCollection serviceCollection)
		{
		}
	}
}