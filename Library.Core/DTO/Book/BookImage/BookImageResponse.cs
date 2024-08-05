namespace Library.Core.DTO.Book.BookImage
{
    public class BookImageResponse
    {
        public Guid BookImageID { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; } = "/bookImages";
    }
}
