using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.DTOs.Examples
{
    public class ExampleToReturnDto : IMapFrom<Example>
    {
        public int Id { get; set; }
        public string? Title1 { get; set; }
        public string? Title2 { get; set; }
        public string? Title3 { get; set; }
    }
}
