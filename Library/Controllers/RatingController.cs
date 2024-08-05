using Library.Core.DTO.Rating;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Library.UI.Controllers
{
    [Route("api/rate")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> UpdateRating([FromBody] RatingRequest? ratingRequest)
        {
            if (ratingRequest == null)
            {
                return BadRequest("RatingRequest cannot be null.");
            }

            if (ratingRequest.Value == 0)
            {
                bool deleted = await _ratingService.DeleteRatingAsync(ratingRequest.UserID.Value, ratingRequest.BookID.Value);
                if (!deleted)
                {
                    return NotFound("Rating not found.");
                }

                return Ok("Rating deleted successfully.");
            }

            var updatedRating = await _ratingService.UpdateRatingAsync(ratingRequest);
            if (updatedRating == null)
            {
                var newRating = await _ratingService.AddRatingAsync(ratingRequest);
                if(newRating == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the rating.");

                return Created();
            }

            return Ok();
        }

        [HttpGet("rating")]
        public async Task<IActionResult> GetRatingByUserIdAndBookIdAsync([FromQuery] Guid? userId, [FromQuery] Guid? bookId)
        {
            if (userId == null || bookId == null)
            {
                return BadRequest("User ID and Book ID must be provided.");
            }

            var rating = await _ratingService.GetRatingByUserIdAndBookIdAsync(userId, bookId);
            if (rating == null)
            {
                return NotFound("Rating not found.");
            }

            return Ok(rating);
        }
    }
}
