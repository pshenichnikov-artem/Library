using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.DTO.Author;
using Library.Core.DTO.Book;
using Library.Core.DTO.Book.BookFile;
using Library.Core.DTO.Book.BookImage;
using Library.Core.DTO.Comment;
using Library.Core.DTO.Rating;

namespace Library.Core.Automapper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            //Author
            CreateMap<AuthorAddRequest, Author>()
                .ForMember(dest => dest.AuthorID, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorImages, opt => opt.Ignore())
                .ForMember(dest => dest.BookAuthors, opt => opt.Ignore());
            CreateMap<Author, AuthorResponse>()
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AuthorImages, opt => opt.MapFrom(src => src.AuthorImages))
                .ForMember(dest => dest.Books, opt => opt.Ignore());
            CreateMap<AuthorUpdateRequest, Author>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Biography));

            //Rating
            CreateMap<IEnumerable<Rating>, RatingResponse>()
                .ConvertUsing(new RatingToRatingResponseConverter());

            CreateMap<Rating, RatingResponse>()
                .ForMember(dest => dest.BookID, opt => opt.MapFrom(src => src.BookID))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
                .ForMember(dest => dest.UsersRaiting, opt => opt.MapFrom(src => new Dictionary<Guid, float> { { src.UserID, src.Value } }));

            CreateMap<RatingRequest, Rating>();

            //Author
            CreateMap<Author, AuthorResponse>()
                .ForMember(dest => dest.Biography, opt => opt.MapFrom(src=>src.Description))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(stc => stc.BookAuthors.Select(b=>b.Book)));

            //Comment
            CreateMap<Comment, CommentResponse>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<CommentAddRequest, Comment>();
            CreateMap<CommentUpdateRequest, Comment>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));

            // Book
            CreateMap<Book, BookResponse>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.BookAuthors.Select(ba => ba.Author)))
                .ForMember(dest => dest.BookImages, opt => opt.MapFrom(src => src.BookImages))
                .ForMember(dest => dest.BookFiles, opt => opt.MapFrom(src => src.BookFiles))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                .ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.Owner.Email));
            CreateMap<BookAddRequest, Book>()
                .ForMember(dest => dest.OwnerID, opt => opt.Ignore());
            CreateMap<BookUpdateRequest, Book>()
                .ForMember(dest => dest.BookID, opt => opt.Ignore())
                .ForMember(dest => dest.OwnerID, opt => opt.Ignore());

            //BookFile
            CreateMap<BookFile, BookFileResponse>();

            //BookImage
            CreateMap<BookImage, BookImageResponse>();
        }
    }
}
