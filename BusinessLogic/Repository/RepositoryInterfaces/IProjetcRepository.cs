using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IProjetcRepository : IGenericRepository<Project>
    {
        List<string> GetCurrencyValues();
        List<string> GetProjectTypeValues();
        int GetProjectsCount(string searchText = null, bool showArchived = false);
        List<Project> GetPaginatedProjects(int page, int pageSize, string searchText = null , bool showArchived = false);

    }
}
