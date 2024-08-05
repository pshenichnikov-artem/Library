using Microsoft.AspNetCore.Http;

namespace Library.Core.DTO.Author
{
    public class AuthorUpdateRequest
    {
        public Guid AuthorID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? Image { get; set; }
    }
}
