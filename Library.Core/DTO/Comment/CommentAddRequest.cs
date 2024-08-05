using System.ComponentModel.DataAnnotations;

namespace Library.Core.DTO.Comment
{
    public class CommentAddRequest
    {
        [Required]
        public Guid BookID { get; set; }   
        public Guid UserID { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Content must have between 3 and 1000 symbol")]
        public string Content { get; set; } = string.Empty;
    }
}
