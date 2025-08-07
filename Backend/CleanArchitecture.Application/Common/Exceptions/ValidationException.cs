using FluentValidation.Results;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class ValidationException() : Exception("One or more validation failures have occurred.")
    {
        public List<string?> Errors { get; } = new();

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            foreach (var failure in failures)
            {
                Errors.Add(failure.ErrorMessage);
            }
        }

    }
}
