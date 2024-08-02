using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.DTO;

namespace Library.Core.Automapper
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            // Настройка преобразования от BookRequest к Book
            CreateMap<BookAddRequest, Book>()
                .ForMember(dest => dest.BookID, opt => opt.Ignore())
                .ForMember(dest => dest.CoverImageID, opt => opt.Ignore())
                .ForMember(dest => dest.BookFile, opt => opt.Ignore());

            // Настройка преобразования от Book к BookResponse
            CreateMap<Book, BookResponse>();
        }
    }
}
