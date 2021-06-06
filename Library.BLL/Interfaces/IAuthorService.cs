using System.Collections.Generic;
using Library.BLL.DTO;

namespace Library.BLL.Interfaces
{
    public interface IAuthorService : IBaseService<AuthorDto>
    {
        public AuthorDto GetByNameOrCreate(string name);
        public List<AuthorDto> GetByNamesOrCreate(IEnumerable<string> names);

    }
}