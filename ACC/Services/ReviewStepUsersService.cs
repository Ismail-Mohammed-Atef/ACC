using DataLayer;
using DataLayer.Models;

namespace ACC.Services
{
    public class ReviewStepUsersService
    {
        private readonly AppDbContext _context;

        public ReviewStepUsersService(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(ReviewStepUser obj)
        {
            var model = _context.Set<ReviewStepUser>().Remove(obj);
        }

        public IList<ReviewStepUser> GetAll()
        {
            return _context.Set<ReviewStepUser>().ToList();
        }

        public ReviewStepUser GetById(int id)
        {
            return _context.Set<ReviewStepUser>().Find(id);
        }
        
        public ReviewStepUser Get(string UserId, int StepId, int ReviewId)
        {
            return _context.Set<ReviewStepUser>().FirstOrDefault(w => w.StepId == StepId && w.UserId == UserId && w.ReviewId == ReviewId);
        }


        public IList<ReviewStepUser> GetByStepId(int StepId)
        {
            return _context.Set<ReviewStepUser>().Where(w => w.StepId == StepId).ToList();
        }
        public void Insert(ReviewStepUser obj)
        {
            _context.Set<ReviewStepUser>().Add(obj);
        }

        public void Update(ReviewStepUser obj)
        {
            _context.Set<ReviewStepUser>().Update(obj);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
