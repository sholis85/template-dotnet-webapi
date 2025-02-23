using Microsoft.AspNetCore.Mvc;

namespace de.WebApi.Host.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}
