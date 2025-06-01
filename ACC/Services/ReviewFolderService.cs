using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class ReviewFolderService 
    {
        private readonly AppDbContext _context;

        public ReviewFolderService(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(ReviewFolder obj)
        {
            var model = _context.Set<ReviewFolder>().Remove(obj);
        }

        public IList<ReviewFolder> GetAll()
        {
            return _context.Set<ReviewFolder>().ToList();
        }

        public ReviewFolder GetById(int id)
        {
            return _context.Set<ReviewFolder>().Find(id);
        }

        public void Insert(ReviewFolder obj)
        {
            _context.Set<ReviewFolder>().Add(obj);
        }

        public void Update(ReviewFolder obj)
        {
            _context.Set<ReviewFolder>().Update(obj);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
