using Library.BLL.DTO;

namespace Library.BLL.Interfaces
{
    public interface IBookLoanRecordService : IBaseService<BookLoanRecordDto>
    {
        void Return(int id);
        void AddTime(int id);
        


    }
}