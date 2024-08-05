using Library.Core.DTO.Comment;

namespace Library.Core.ServiceContracts
{
    public interface ICommentService
    {
        Task<CommentResponse> AddCommentAsync(CommentAddRequest? request);
        Task<CommentResponse?> GetCommentByIdAsync(Guid? commentId);
        Task<IEnumerable<CommentResponse>> GetCommentsByBookIdAsync(Guid? bookId);
        Task<IEnumerable<CommentResponse>> GetCommentsByUserIdAsync(Guid? userId);
        Task<bool> UpdateCommentAsync(Guid? commentId, CommentUpdateRequest? request);
        Task<bool> DeleteCommentAsync(Guid? commentId);
    }
}
