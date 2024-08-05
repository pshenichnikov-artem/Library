
namespace Library.Core.DTO.Book.BookFile
{
    public class BookFileResponse
    {
        public Guid BookFileID { get; set; }
        public Guid BookID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
