using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.Interfaces.Repositories;
using MediatR;

namespace CleanArchitecture.Application.Features.Examples.Commands
{
    public class DeleteExampleCommand(int id) : IRequest<Response<int>>
    {
        public int Id { get; set; } = id;
    }

    public class DeleteExampleCommandHandler(IExampleRepository exampleRepository)
        : IRequestHandler<DeleteExampleCommand, Response<int>>
    {
        public async Task<Response<int>> Handle(DeleteExampleCommand command, CancellationToken cancellationToken)
        {
            var example = await exampleRepository.GetExampleById(command.Id);
            if (example == null) throw new ApiException($"Example Not Found.");
            await exampleRepository.DeleteExample(example);
            return new Response<int>(example.Id);
        }
    }
}
