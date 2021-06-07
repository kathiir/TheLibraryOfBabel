using System.Collections.Generic;
using Library.DAL.Entities;

namespace Library.BLL.DTO
{
    public class StaffDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookLoanRecordDto> BookLoanRecords { get; set; }

        public StaffDto()
        {
            BookLoanRecords = new List<BookLoanRecordDto>();
        }
    }
}