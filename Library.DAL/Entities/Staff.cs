using System.Collections.Generic;

namespace Library.DAL.Entities
{
    public class Staff : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<BookLoanRecord> BookLoanRecords { get; set; }

        public Staff()
        {
            BookLoanRecords = new List<BookLoanRecord>();
        }

    }
}