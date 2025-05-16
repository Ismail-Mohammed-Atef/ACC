using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;
namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IIssueService
    {
        void CreateIssue(Issue issue);
        Issue GetIssueById(int id);
        void UpdateIssue(Issue issue);
        void DeleteIssue(int id);
        List<Issue> GetAllIssues();
        List<Issue> GetIssuesByProjectId(int projectId);

    }
}
