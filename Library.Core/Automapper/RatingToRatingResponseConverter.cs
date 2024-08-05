using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.DTO.Rating;

namespace Library.Core.Automapper
{
    public class RatingToRatingResponseConverter : ITypeConverter<IEnumerable<Rating>, RatingResponse>
    {
        public RatingResponse Convert(IEnumerable<Rating> source, RatingResponse destination, ResolutionContext context)
        {
            if (source == null || !source.Any())
            {
                return new RatingResponse
                {
                    UsersRaiting = new Dictionary<Guid, float>()
                };
            }

            return new RatingResponse
            {
                BookID = source.First().BookID,
                Value = (float)source.Average(r => r.Value),
                UsersRaiting = source.ToDictionary(r => r.UserID, r => r.Value)
            };
        }
    }
}
