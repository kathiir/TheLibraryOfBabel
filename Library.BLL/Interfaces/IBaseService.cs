using System.Collections.Generic;

namespace Library.BLL.Interfaces
{
    public interface IBaseService<T> where T: class
    {
        void AddOrUpdate(T dto);
        T Get(int? id);
        List<T> GetAll();
        void Delete(int? id);
    }
}