using System.Collections.Generic;

namespace Library.DAL.Entities
{
    public class Book : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Author> Authors { get; set; }
        public int NumberOfCopies { get; set; }
        public int NumberOfCopiesCurrent { get; set; }
        public virtual Genre Genre { get; set; }

        public Book()
        {
            Authors = new List<Author>();
        }
    }
}