using System.Collections.Generic;

namespace WebApplication.Models
{
    public class ReaderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookLoanRecordViewModel> BookLoanRecords { get; set; }

    }
}