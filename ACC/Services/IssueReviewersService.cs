using DataLayer.Models;
using DataLayer;

namespace ACC.Services
{
    public class IssueReviewersService
    {
        private readonly AppDbContext _context;

        public IssueReviewersService(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(IssueReviwers obj)
        {
            var model = _context.Set<IssueReviwers>().Remove(obj);
        }

        public IList<IssueReviwers> GetAll()
        {
            return _context.Set<IssueReviwers>().ToList();
        }

        public IssueReviwers GetById(int id)
        {
            return _context.Set<IssueReviwers>().Find(id);
        }

        public void Insert(IssueReviwers obj)
        {
            _context.Set<IssueReviwers>().Add(obj);
        }

        public void Update(IssueReviwers obj)
        {
            _context.Set<IssueReviwers>().Update(obj);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
