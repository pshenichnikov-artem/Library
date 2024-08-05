
namespace Library.Core.DTO.Rating
{
    public class RatingResponse
    {
        public Guid BookID { get; set; }
        public float Value { get; set; }
        public IDictionary<Guid, float> UsersRaiting { get; set; }
    }

    public static class RatingExtension
    {
        public static RatingResponse ToRatingResponse(this IEnumerable<Library.Core.Domain.Entities.Rating> ratings)
        {
            return new RatingResponse()
            {
                BookID = ratings.First().BookID,
                Value = (float)ratings.Average(r => r.Value),
                UsersRaiting = ratings.ToDictionary(r => r.UserID, r => r.Value)
            };
        }
    }
}
