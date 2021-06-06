using System.Collections.Generic;

namespace Library.BLL.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookDto> Books { get; set; }
    }
}