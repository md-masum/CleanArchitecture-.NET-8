using AutoMapper;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Examples;
using CleanArchitecture.Application.Interfaces.Repositories;
using MediatR;

namespace CleanArchitecture.Application.Features.Examples.Queries
{
    public class GetExampleByIdQuery(int id) : IRequest<Response<ExampleToReturnDto>>
    {
        public int Id { get; set; } = id;
    }

    public class GetExampleByIdQueryHandler(IExampleRepository exampleRepository, IMapper mapper)
        : IRequestHandler<GetExampleByIdQuery, Response<ExampleToReturnDto>>
    {
        public async Task<Response<ExampleToReturnDto>> Handle(GetExampleByIdQuery request, CancellationToken cancellationToken)
        {
            var example = await exampleRepository.GetExampleById(request.Id);
            var exampleToReturn = mapper.Map<ExampleToReturnDto>(example);
            return new Response<ExampleToReturnDto>(exampleToReturn);
        }
    }
}
