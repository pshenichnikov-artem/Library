namespace Library.Core.DTO
{
    public class CommentResponse
    {
        public Guid CommentID { get; set; }
        public string Content { get; set; }
        public string UserFirstName { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
