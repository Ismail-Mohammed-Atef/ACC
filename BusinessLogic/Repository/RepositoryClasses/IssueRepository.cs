using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Repository.RepositoryInterfaces;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class IssueRepository :GenericRepository<Issue> , IIssueRepository
    {
        private readonly AppDbContext _context;

        public IssueRepository(AppDbContext context) : base(context) 
        {
            _context = context;
        }



        public List<Issue> GetIssuesByUserId(string userID, int projectId)
        {
            return _context.Issues
                .Include(i => i.IssueReviwers)
                .Include(i => i.Document)
                    .ThenInclude(d => d.Versions) // ✅ مهم جدًا
                .Where(i =>
                    (i.InitiatorID == userID ||
                     i.IssueReviwers.Any(r => r.ReviewerId == userID))
                     && i.ProjectId == projectId)
                .ToList();
        }



    }
}