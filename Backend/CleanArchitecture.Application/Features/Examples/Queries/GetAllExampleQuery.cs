using AutoMapper;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Examples;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Application.Interfaces.Services;
using MediatR;

namespace CleanArchitecture.Application.Features.Examples.Queries
{
    public class GetAllExampleQuery(int pageNumber, int pageSize) : IRequest<PagedResponse<IList<ExampleToReturnDto>>>
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }

    public class GetAllExampleQueryHandler(IExampleRepository exampleRepository, IMapper mapper, IRedisCacheService redisCacheService)
        : IRequestHandler<GetAllExampleQuery, PagedResponse<IList<ExampleToReturnDto>>>
    {
        public async Task<PagedResponse<IList<ExampleToReturnDto>>> Handle(GetAllExampleQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"GetAllExampleQuery-{request.PageNumber}-{request.PageSize}";
            var pagedResponse = await redisCacheService.GetAsync<PagedResponse<IList<ExampleToReturnDto>>>(cacheKey);
            if (pagedResponse is null)
            {
                var examples = await exampleRepository.GetAllExample(request.PageNumber, request.PageSize);
                var studentViewModels = mapper.Map<IList<ExampleToReturnDto>>(examples.Data);
                pagedResponse = new PagedResponse<IList<ExampleToReturnDto>>(studentViewModels, request.PageNumber, request.PageSize, examples.TotalCount);
                await redisCacheService.SetAsync(cacheKey, pagedResponse, TimeSpan.FromMinutes(5));
            }
            return pagedResponse;
        }
    }
}
