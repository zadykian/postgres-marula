using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Postgres.Marula.HttpApi.Common
{
	/// <summary>
	/// Base type for API controllers.
	/// </summary>
	[ApiController]
	[Route("[controller]/[action]")]
	[Produces(MediaTypeNames.Application.Json)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public abstract class ApiControllerBase : Controller
	{
	}
}