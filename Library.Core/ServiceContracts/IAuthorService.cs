using Library.Core.DTO.Author;

namespace Library.Core.ServiceContracts
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync();
        Task<AuthorResponse?> GetAuthorByIdAsync(Guid? authorId);
        Task<IEnumerable<AuthorResponse>> GetAuthorByNameAsync(string? firstName, string? lastName);
        Task<AuthorResponse> AddAuthorAsync(AuthorAddRequest? authorAddRequest);
        Task<AuthorResponse?> UpdateAuthorAsync(AuthorUpdateRequest? authorUpdateRequest, Guid? authorID);
        Task<bool> DeleteAuthorAsync(Guid? authorId);
    }
}
