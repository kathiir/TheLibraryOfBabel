using System.Collections.Generic;

namespace Library.DAL.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Book> Books { get; set; }

        public Genre()
        {
            Books = new List<Book>();
        }
    }
}