using AutoMapper;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Application.Common.Wrappers;
using CleanArchitecture.Application.DTOs.Examples;
using CleanArchitecture.Application.Interfaces.Repositories;
using CleanArchitecture.Domain.Entities;
using FluentValidation;
using MediatR;

namespace CleanArchitecture.Application.Features.Examples.Commands
{
    public class CreateExampleCommand : IRequest<Response<ExampleToReturnDto>>, IMapFrom<Example>
    {
        public string? Title1 { get; set; }
        public string? Title2 { get; set; }
        public string? Title3 { get; set; }
    }

    public class CreateExampleCommandHandler(IExampleRepository exampleRepository, IMapper mapper)
        : IRequestHandler<CreateExampleCommand, Response<ExampleToReturnDto>>
    {
        public async Task<Response<ExampleToReturnDto>> Handle(CreateExampleCommand request, CancellationToken cancellationToken)
        {
            var example = await exampleRepository.CreateExample(mapper.Map<Example>(request));
            return new Response<ExampleToReturnDto>(mapper.Map<ExampleToReturnDto>(example));
        }
    }

    public class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
    {
        private readonly IExampleRepository _exampleRepository;
        public CreateExampleCommandValidator(IExampleRepository exampleRepository)
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
                .EmailAddress().WithMessage("invalid email address")
                .MustAsync(IsUniqueEmail).WithMessage("{PropertyName} already exists.");
        }

        private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _exampleRepository.IsUniqueEmailAsync(email);
        }
    }
}
