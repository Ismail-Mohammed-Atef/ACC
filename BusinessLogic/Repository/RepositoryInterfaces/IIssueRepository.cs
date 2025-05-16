using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IIssueRepository
    {
        void Add(Issue issue);
        Issue GetById(int id);
        void Update(Issue issue);
        void Delete(int id);
        List<Issue> GetAll();
        List<Issue> GetIssuesByProjectId(int projectId);
    }
}
