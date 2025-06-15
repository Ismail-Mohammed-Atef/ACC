using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IIssueRepository : IGenericRepository<Issue>
    {

        List<Issue> GetIssuesByUserId(string userID, int projectId);
        Task<Issue?> GetByIdAsync(int id);


    }
}
