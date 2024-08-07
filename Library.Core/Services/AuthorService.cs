using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Author.AuthorImage;
using Library.Core.DTO.Author;
using Library.Core.DTO.Book;
using Library.Core.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Library.Core.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AuthorResponse>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorResponse>>(authors);
        }

        public async Task<AuthorResponse?> GetAuthorByIdAsync(Guid? authorId)
        {
            if (authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId), "Author ID cannot be null.");
            }

            var author = await _authorRepository.GetByIdAsync(authorId.Value);
            if (author == null)
                return null;

            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task<IEnumerable<AuthorResponse>> GetAuthorByNameAsync(string? firstName, string? lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("First name and last name cannot be null or empty.");
            }

            var author = await _authorRepository.GetAllAsync();
            if (author == null)
                return null;

            var authorsFilterByName = author.Where(x => x.FirstName.Contains(firstName) || x.LastName.Contains(lastName));

            return _mapper.Map<IEnumerable<AuthorResponse>>(authorsFilterByName);
        }

        public async Task<AuthorResponse> AddAuthorAsync(AuthorAddRequest? authorAddRequest)
        {
            if (authorAddRequest == null)
            {
                throw new ArgumentNullException(nameof(authorAddRequest), "Author add request cannot be null.");
            }

            var author = new Author
            {
                AuthorID = Guid.NewGuid(),
                FirstName = authorAddRequest.FirstName,
                LastName = authorAddRequest.LastName,
                Description = authorAddRequest.Biography,
                DateOfBirth = authorAddRequest.DateOfBirth.Value
            };

            var success = await _authorRepository.AddAsync(author);
            if (!success)
            {
                throw new InvalidOperationException("Failed to add the author.");
            }

            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task<AuthorResponse?> UpdateAuthorAsync(AuthorUpdateRequest? authorUpdateRequest, Guid? authorID)
        {
            if (authorUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(authorUpdateRequest), "Author update request cannot be null.");
            }

            if (authorID == null)
            {
                throw new ArgumentNullException(nameof(authorID), "Author ID cannot be null.");
            }

            var existingAuthor = await _authorRepository.GetByIdAsync(authorID.Value);
            if (existingAuthor == null)
            {
                throw new KeyNotFoundException($"Author with ID {authorID} not found.");
            }

            existingAuthor.Description = authorUpdateRequest.Biography;

            var success = await _authorRepository.UpdateAsync(existingAuthor);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update the author.");
            }

            return _mapper.Map<AuthorResponse>(existingAuthor);
        }

        public async Task<bool> DeleteAuthorAsync(Guid? authorId)
        {
            if (authorId == null)
            {
                throw new ArgumentNullException(nameof(authorId), "Author ID cannot be null.");
            }

            Author author = await _authorRepository.GetByIdAsync(authorId.Value);
            if(author == null)
                return false;

            if (author.Books != null && author.Books.Count != 0)
                return false;

            var success = await _authorRepository.DeleteAsync(author);
            if (!success)
            {
                throw new InvalidOperationException("Failed to delete the author.");
            }

            return success;
        }
    }
}
