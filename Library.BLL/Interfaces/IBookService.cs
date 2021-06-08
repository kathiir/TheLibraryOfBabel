using System;
using System.Collections.Generic;
using Library.BLL.DTO;

namespace Library.BLL.Interfaces
{
    public interface IBookService : IBaseService<BookDto>
    {
        int GetLoanedCopiesCount(int bookId);
        void UpdateCount(int id);
        void UpdateCountAndSave(int id);
        public void UpCount(int id);
        public void DownCount(int id);


    }
}