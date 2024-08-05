using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Rating
{
    public class RatingRequest
    {
        public Guid? RatingID { get; set; }
        [Required]
        public Guid? UserID { get; set; }
        [Required]
        public Guid? BookID { get; set; }
        [Required]
        [Range(0,5)]
        public float? Value { get; set; }
    }
}
