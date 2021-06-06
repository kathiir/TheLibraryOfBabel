using System.Collections.Generic;

namespace Library.DAL.Entities
{
    public class Reader : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<BookLoanRecord> BookLoanRecords { get; set; }

        public Reader()
        {
            BookLoanRecords = new List<BookLoanRecord>();
        }
    }
}