using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Comment;
using Library.Core.ServiceContracts;

namespace Library.Core.Services
{

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<CommentResponse> AddCommentAsync(CommentAddRequest? request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "CommentAddRequest cannot be null.");
            }

            var comment = _mapper.Map<Comment>(request);
            comment.CommentID = Guid.NewGuid();
            comment.CreatedAt = DateTime.Now;

            await _commentRepository.AddAsync(comment);
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<CommentResponse?> GetCommentByIdAsync(Guid? commentId)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                throw new ArgumentException("Comment ID cannot be null or empty.", nameof(commentId));
            }

            var comment = await _commentRepository.GetByIdAsync(commentId.Value);
            return comment == null ? null : _mapper.Map<CommentResponse>(comment);
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsByBookIdAsync(Guid? bookId)
        {
            if (bookId == null || bookId == Guid.Empty)
            {
                throw new ArgumentException("Book ID cannot be null or empty.", nameof(bookId));
            }

            var comments = await _commentRepository.GetAllAsync();
            var bookComments = comments.Where(c => c.BookID == bookId.Value);
            return _mapper.Map<IEnumerable<CommentResponse>>(bookComments);
        }

        public async Task<IEnumerable<CommentResponse>> GetCommentsByUserIdAsync(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));
            }

            var comments = await _commentRepository.GetAllAsync();
            var userComments = comments.Where(c => c.UserID == userId.Value);
            return _mapper.Map<IEnumerable<CommentResponse>>(userComments);
        }

        public async Task<bool> UpdateCommentAsync(Guid? commentId, CommentUpdateRequest? request)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                throw new ArgumentException("Comment ID cannot be null or empty.", nameof(commentId));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "CommentUpdateRequest cannot be null.");
            }

            var comment = await _commentRepository.GetByIdAsync(commentId.Value);
            if (comment == null)
            {
                return false;
            }

            _mapper.Map(request, comment);
            return await _commentRepository.UpdateAsync(comment);
        }

        public async Task<bool> DeleteCommentAsync(Guid? commentId)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                throw new ArgumentException("Comment ID cannot be null or empty.", nameof(commentId));
            }

            var comment = await _commentRepository.GetByIdAsync(commentId.Value);
            if (comment == null)
            {
                return false;
            }

            return await _commentRepository.DeleteAsync(comment);
        }
    }
}
