using System.Diagnostics;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetaController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet("info")]
        public ActionResult<string> Info()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        }
    }
}
