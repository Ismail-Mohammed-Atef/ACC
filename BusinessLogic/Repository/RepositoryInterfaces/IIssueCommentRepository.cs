using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IIssueCommentRepository : IGenericRepository<IssueComment>
    {
        Task<List<IssueComment>> GetCommentsByIssueIdAsync(int issueId);
    }
}
