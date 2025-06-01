using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(T obj)
        {
            var model = _context.Set<T>().Remove(obj);
        }
       
        public IList<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public void Insert(T obj)
        {
            _context.Set<T>().Add(obj);
        }

        public void Update(T obj)
        {
            _context.Set<T>().Update(obj);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
