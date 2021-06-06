using System.Collections.Generic;

namespace Library.BLL.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AuthorDto> Authors { get; set; }
        public int NumberOfCopies { get; set; }
        public int NumberOfCopiesCurrent { get; set; }
        public GenreDto Genre { get; set; }
    }
}