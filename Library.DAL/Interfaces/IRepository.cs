using System;
using System.Collections.Generic;
using Library.DAL.Entities;

namespace Library.DAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void CreateOrUpdate(T item);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
        bool Exists(int? id);
    }
}