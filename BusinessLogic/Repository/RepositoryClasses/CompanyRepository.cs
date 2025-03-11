using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class CompanyRepository :  GenericRepository<Company> , ICompanyRepository
    {

        AppDbContext Context;
        public CompanyRepository(AppDbContext context) : base(context)
        {
          Context = context; ;
        }

        public IEnumerable<Company> SearchCompanies(string searchTerm, CompanyType? companyType)
        {
            var query = Context.Companies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm) || c.Description.Contains(searchTerm));
            }

            if (companyType.HasValue)
            {
                query = query.Where(c => c.CompanyType == companyType);
            }

            return query.ToList();
        }

       
    }
}
