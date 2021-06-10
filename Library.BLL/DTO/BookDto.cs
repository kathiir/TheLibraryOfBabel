using System.Collections.Generic;
using Library.DAL.Entities;

namespace Library.BLL.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AuthorDto> Authors { get; set; }
        public int NumberOfCopies { get; set; }
        // public int NumberOfCopiesCurrent { get; set; }
        public List<BookLoanRecordDto> BookLoanRecords { get; set; }
        public GenreDto Genre { get; set; }

        public BookDto()
        {
            Authors = new List<AuthorDto>();
            BookLoanRecords = new List<BookLoanRecordDto>();
        }
    }
}