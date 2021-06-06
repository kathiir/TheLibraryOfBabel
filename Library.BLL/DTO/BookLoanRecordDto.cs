using System;

namespace Library.BLL.DTO
{
    public class BookLoanRecordDto
    {
        public int Id { get; set; }
        public BookDto Book { get; set; }
        public ReaderDto Reader { get; set; }
        public StaffDto Staff { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}