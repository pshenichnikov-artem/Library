using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.DTO;
using Library.Core.DTO.Book;
using Library.Core.DTO.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Automapper
{
	public class ViewBookToViewBookResponseConverter : ITypeConverter<IEnumerable<UserBookView>, UserBookViewResponse>
	{
		private readonly IMapper _mapper;

		public ViewBookToViewBookResponseConverter(IMapper mapper)
		{
			_mapper = mapper;
		}

		public UserBookViewResponse Convert(IEnumerable<UserBookView> source, UserBookViewResponse destination, ResolutionContext context)
		{
			if (source == null || !source.Any())
			{
				return new UserBookViewResponse
				{
					Books = new Dictionary<DateTime, IEnumerable<BookResponse>>()
				};
			}

			return new UserBookViewResponse
			{
				User = source.First().User,
				Books = source
			.GroupBy(x => x.ViewDate.Date)
			.ToDictionary(
				g => g.Key,
				g => g.Select(x => _mapper.Map<BookResponse>(x.Book)) 
			)
			};
		}
	}
}
