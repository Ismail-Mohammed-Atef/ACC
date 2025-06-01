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
    public class ReviewDocumentService 
    {
        private readonly AppDbContext _context;

        public ReviewDocumentService(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(ReviewDocument obj)
        {
            var model = _context.Set<ReviewDocument>().Remove(obj);
        }

        public IList<ReviewDocument> GetAll()
        {
            return _context.Set<ReviewDocument>().ToList();
        }

        public ReviewDocument GetById(int id)
        {
            return _context.Set<ReviewDocument>().Find(id);
        }

        public void Insert(ReviewDocument obj)
        {
            _context.Set<ReviewDocument>().Add(obj);
        }

        public void Update(ReviewDocument obj)
        {
            _context.Set<ReviewDocument>().Update(obj);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
