using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoTareas.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        void Save();

    }
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly DbSet<T> _set;
        protected readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _set.ToList();
        }
        public T GetById(int id)
        {
            return _set.Find(id);
        }
        public void Add(T entity)
        {
            _set.Add(entity);
        }
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _set.Remove(entity);
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
