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
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;

        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Issue issue)
        {
            issue.CreatedAt = DateTime.Now;
            issue.UpdatedAt = DateTime.Now;
            _context.Issues.Add(issue);
            _context.SaveChanges();
        }

        public Issue GetById(int id)
        {
            return _context.Issues.Find(id);
        }

        public void Update(Issue issue)
        {
            issue.UpdatedAt = DateTime.Now;
            _context.Issues.Update(issue);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var issue = _context.Issues.Find(id);
            if (issue != null)
            {
                _context.Issues.Remove(issue);
                _context.SaveChanges();
            }
        }

        public List<Issue> GetAll()
        {
            return _context.Issues.ToList();
        }
        public List<Issue> GetIssuesByProjectId(int projectId)
        {
            return _context.Issues.Where(i => i.ProjectId == projectId).ToList();
        }
     
    }
}
