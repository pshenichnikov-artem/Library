namespace Library.Core.DTO.Comment
{
    public class CommentResponse
    {
        public Guid CommentID { get; set; }
        public Guid BookID { get; set; }
        public Guid UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
