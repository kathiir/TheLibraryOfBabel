using System.Collections.Generic;

namespace WebApplication.Models
{
    public class StaffViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookLoanRecordViewModel> BookLoanRecords { get; set; }

        public StaffViewModel()
        {
            BookLoanRecords = new List<BookLoanRecordViewModel>();
        }
    }
}