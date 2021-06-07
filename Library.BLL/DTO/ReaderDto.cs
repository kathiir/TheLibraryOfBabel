using System.Collections.Generic;

namespace Library.BLL.DTO
{
    public class ReaderDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookLoanRecordDto> BookLoanRecords { get; set; }

        public ReaderDto()
        {
            BookLoanRecords = new List<BookLoanRecordDto>();
        }
    }
}