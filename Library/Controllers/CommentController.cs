using Library.Core.DTO.Comment;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.UI.Controllers
{
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService ?? throw new ArgumentNullException(nameof(commentService));
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddCommentAsync([FromBody] CommentAddRequest? request)
        {
            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not valid.");
            }

            try
            {
                request.UserID = parsedUserId;

                var commentResponse = await _commentService.AddCommentAsync(request);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPost("{commentID}/update")]
        [Authorize]
        public async Task<IActionResult> UpdateCommentAsync(Guid? commentId, [FromBody] CommentUpdateRequest? request)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                return BadRequest("Comment ID cannot be null or empty.");
            }

            if (request == null)
            {
                return BadRequest("Request body cannot be null.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not valid.");
            }

            try
            {
                var existingComment = await _commentService.GetCommentByIdAsync(commentId.Value);
                if (existingComment == null)
                {
                    return NotFound("Comment not found.");
                }

                if (existingComment.UserID != parsedUserId)
                {
                    return Forbid("You do not have permission to update this comment.");
                }

                await _commentService.UpdateCommentAsync(commentId.Value, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("{commentID}/delete")]
        [Authorize]
        public async Task<IActionResult> DeleteCommentAsync(Guid? commentId)
        {
            if (commentId == null || commentId == Guid.Empty)
            {
                return BadRequest("Comment ID cannot be null or empty.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                return Unauthorized("User ID is not valid.");
            }

            try
            {
                var existingComment = await _commentService.GetCommentByIdAsync(commentId.Value);
                if (existingComment == null)
                {
                    return NotFound("Comment not found.");
                }

                if (existingComment.UserID != parsedUserId)
                {
                    return Forbid("You do not have permission to delete this comment.");
                }

                await _commentService.DeleteCommentAsync(commentId.Value);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
