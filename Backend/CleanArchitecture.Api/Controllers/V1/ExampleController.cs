using CleanArchitecture.Application.Features.Examples;
using CleanArchitecture.Application.Features.Examples.Commands;
using CleanArchitecture.Application.Features.Examples.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    public class ExampleController(IMediator mediator) : BaseApiController(mediator)
    {
        [HttpGet]
        public async Task<IActionResult> GetAllExample([FromQuery]GetAllExampleParams exampleParams)
        {
            return Ok(await Mediator.Send(new GetAllExampleQuery(exampleParams.PageNumber, exampleParams.PageSize)));
        }
        
        [HttpGet("{id}", Name = "GetExampleById")]
        public async Task<IActionResult> GetExampleById(int id)
        {
            return Ok(await Mediator.Send(new GetExampleByIdQuery(id)));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateExample(CreateExampleCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtRoute("GetExampleById", new {id = result.Data?.Id}, result);
        }
        
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateExample(int id, UpdateExampleCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteExample(int id)
        {
            return Ok(await Mediator.Send(new DeleteExampleCommand(id)));
        }
    }
}
