using System;
using System.Collections.Generic;
using System.Linq;
using Library.DAL.EF;
using Library.DAL.Entities;
using Library.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private ApplicationContext db;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationContext context)
        {
            db = context;
            _entities = context.Set<T>();
        }

        public void CreateOrUpdate(T item)
        {
            if (item.Id == 0 || !_entities.Any(entity => item.Id == entity.Id))
            {
                _entities.Add(item);
            }

            db.SaveChanges();
        }

        public void Create(T item)
        {
            _entities.Add(item);
            db.SaveChanges();
        }

        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
            // db.Entry(item).State = !db.T.Any(f => f.Id == item.Id) ? EntityState.Added : EntityState.Modified;

            db.SaveChanges();
        }

        public void Delete(int id)
        {
            T item = Get(id);
            if (item != null)
            {
                _entities.Remove(item);
            }

            db.SaveChanges();
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _entities.AsEnumerable().Where(predicate).ToList();
        }

        public T Get(int id)
        {
            return _entities.FirstOrDefault(o => o.Id == id);
            
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public bool Exists(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var entity = _entities.Find(id.Value);
            if (entity != null)
            {
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public int Count()
        {
            return _entities.Count();
        }

        public int Count(Func<T, bool> predicate)
        {
            return _entities.Count(predicate);
        }
    }
}