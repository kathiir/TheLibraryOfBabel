using System;

namespace WebApplication.Models
{
    public class BookLoanRecordViewModel
    {
        public int Id { get; set; }
        public BookViewModel Book { get; set; }
        public ReaderViewModel Reader { get; set; }
        public StaffViewModel Staff { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}