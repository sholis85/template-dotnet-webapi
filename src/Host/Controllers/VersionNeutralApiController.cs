using Microsoft.AspNetCore.Mvc;

namespace de.WebApi.Host.Controllers;

[Route("api/[controller]")]
[ApiVersionNeutral]
public class VersionNeutralApiController : BaseApiController
{
}
