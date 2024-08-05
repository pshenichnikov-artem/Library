using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.DTO;
using Library.Core.DTO.Author;
using Library.Core.DTO.Book;
using Library.Core.DTO.Book.BookFile;
using Library.Core.DTO.Book.BookImage;

namespace Library.Core.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<AuthorAddRequest, Author>()
            .ForMember(dest => dest.AuthorID, opt => opt.Ignore())
            .ForMember(dest => dest.AuthorImages, opt => opt.Ignore())
            .ForMember(dest => dest.BookAuthors, opt => opt.Ignore());

            // Mapping from Author to AuthorResponse
            CreateMap<Author, AuthorResponse>()
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AuthorImages, opt => opt.MapFrom(src => src.AuthorImages))
                .ForMember(dest => dest.Books, opt => opt.Ignore());

            // Additional mapping from AuthorUpdateRequest to Author if needed
            CreateMap<AuthorUpdateRequest, Author>()
                .ForMember(dest => dest.AuthorImages, opt => opt.Ignore())
                .ForMember(dest => dest.BookAuthors, opt => opt.Ignore());
            ///////////////////////////////////
            CreateMap<Rating, RatingResponse>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));

            CreateMap<Author, AuthorResponse>()
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src=>src.Description))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(stc => stc.BookAuthors.Select(b=>b.Book)));

            CreateMap<Comment, CommentResponse>()
                .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.FirstName)); ;

            // Маппинг для Book и BookResponse
            CreateMap<Book, BookResponse>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.Author)))
                .ForMember(dest => dest.BookImages, opt => opt.MapFrom(src => src.BookImages))
                .ForMember(dest => dest.BookFiles, opt => opt.MapFrom(src => src.BookFiles))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.Owner.Email));

            // Маппинг для BookAddRequest и Book
            CreateMap<BookAddRequest, Book>()
                .ForMember(dest => dest.OwnerID, opt => opt.Ignore());

            // Маппинг для BookUpdateRequest и Book
            CreateMap<BookUpdateRequest, Book>()
                .ForMember(dest => dest.BookID, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerID, opt => opt.Ignore());

            CreateMap<BookFile, BookFileResponse>();
            CreateMap<BookImage, BookImageResponse>();
        }
    }
}
