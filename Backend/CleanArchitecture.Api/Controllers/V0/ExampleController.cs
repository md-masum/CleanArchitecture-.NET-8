using CleanArchitecture.Application.Features.Examples;
using CleanArchitecture.Application.Features.Examples.Commands;
using CleanArchitecture.Application.Features.Examples.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.V0
{
    [Route("api/v0/[controller]")]
    public class ExampleController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        public async Task<IActionResult> GetAllExample([FromQuery] GetAllExampleParams exampleParams)
        {
            return Ok("this is old api");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExampleById(int id)
        {
            return Ok("old api data");
        }
    }
}
