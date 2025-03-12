using DataLayer.Models.Enums;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface ICompanyRepository
    {
        dynamic GetAll();
        void Insert(Company companyFromRequest);
        void Save();
        IEnumerable<Company> SearchCompanies(string searchTerm, CompanyType? companyType);
    }
}
