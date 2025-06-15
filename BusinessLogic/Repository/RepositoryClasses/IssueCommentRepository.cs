using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class IssueCommentRepository : GenericRepository<IssueComment>, IIssueCommentRepository
    {
        private readonly AppDbContext _context;
        public IssueCommentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<IssueComment>> GetCommentsByIssueIdAsync(int issueId)
        {
            return await _context.IssueComments
                .Where(c => c.IssueId == issueId)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
