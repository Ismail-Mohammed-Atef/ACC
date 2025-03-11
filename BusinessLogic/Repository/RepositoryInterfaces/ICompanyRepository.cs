using DataLayer.Models;
using DataLayer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        IEnumerable<Company> SearchCompanies(string searchTerm, CompanyType? companyType);
    }
}
