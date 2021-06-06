using System.Collections.Generic;

namespace Library.DAL.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
}