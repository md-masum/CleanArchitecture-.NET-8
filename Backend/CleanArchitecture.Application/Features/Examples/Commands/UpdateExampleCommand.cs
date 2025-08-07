using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Examples;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Features.Examples.Commands
{
    public class UpdateExampleCommand : IRequest<Response<ExampleToReturnDto>>, IMapFrom<Example>
    {
        public int Id { get; set; }
        public string? Title1 { get; set; }
        public string? Title2 { get; set; }
        public string? Title3 { get; set; }
    }

    public class UpdateExampleCommandHandler(IExampleRepository exampleRepository, IMapper mapper)
        : IRequestHandler<UpdateExampleCommand, Response<ExampleToReturnDto>>
    {
        public async Task<Response<ExampleToReturnDto>> Handle(UpdateExampleCommand request, CancellationToken cancellationToken)
        {
            var example = await exampleRepository.GetExampleById(request.Id);
            if (example is null)
            {
                throw new ApiException($"Example Not Found.");
            }

            var updateExample = await exampleRepository.UpdateExample(mapper.Map<Example>(request));
            return new Response<ExampleToReturnDto>(mapper.Map<ExampleToReturnDto>(updateExample));
        }
    }

    public class UpdateExampleCommandValidator : AbstractValidator<UpdateExampleCommand>
    {
        private readonly IExampleRepository _exampleRepository;
        public UpdateExampleCommandValidator(IExampleRepository exampleRepository)
        {
            _exampleRepository = exampleRepository;

            RuleFor(p => p.Title1)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Title2)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Title3)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress().WithMessage("invalid email address").CustomAsync(IsUniqueEmailForUpdate);
        }
        private async Task IsUniqueEmailForUpdate(string email, ValidationContext<UpdateExampleCommand> context, CancellationToken cancellationToken)
        {
            var result = await _exampleRepository.IsUniqueEmailForUpdateAsync(context.InstanceToValidate.Id, email);
            if (!result)
            {
                context.AddFailure("email already exist");
            }
        }
    }
}
