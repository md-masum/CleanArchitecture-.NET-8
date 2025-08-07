using AutoMapper;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Examples;
using CleanArchitecture.Application.Interfaces.Repositories;
using MediatR;

namespace CleanArchitecture.Application.Features.Examples.Queries
{
    public class GetAllExampleQuery(int pageNumber, int pageSize) : IRequest<PagedResponse<IList<ExampleToReturnDto>>>
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }

    public class GetAllExampleQueryHandler(IExampleRepository exampleRepository, IMapper mapper)
        : IRequestHandler<GetAllExampleQuery, PagedResponse<IList<ExampleToReturnDto>>>
    {
        public async Task<PagedResponse<IList<ExampleToReturnDto>>> Handle(GetAllExampleQuery request, CancellationToken cancellationToken)
        {
            var examples = await exampleRepository.GetAllExample(request.PageNumber, request.PageSize);
            var studentViewModels = mapper.Map<IList<ExampleToReturnDto>>(examples.Data);
            return new PagedResponse<IList<ExampleToReturnDto>>(studentViewModels, request.PageNumber, request.PageSize, examples.TotalCount);
        }
    }
}
