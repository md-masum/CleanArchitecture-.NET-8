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
            return Ok(await Mediator.Send(new GetAllExampleQuery(exampleParams.PageNumber, exampleParams.PageSize)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExampleById(int id)
        {
            var result = await Mediator.Send(new GetExampleByIdQuery(id));
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExample(CreateExampleCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExample(int id, UpdateExampleCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await Mediator.Send(command);
            return Ok(result.Data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExample(int id)
        {
            var result = await Mediator.Send(new DeleteExampleCommand(id));
            return Ok(result.Data);
        }


    }
}
