using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using DataLayer.Models;
using DataLayer;


namespace BusinessLogic.Repository.RepositoryClasses
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
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


        public Company GetCompanyByName(string companyName)
        {
            return Context.Companies.FirstOrDefault(c => c.Name.ToLower() == companyName.ToLower());
        }

        public Company GetCompanyByEmail(string website)
        {
            return Context.Companies.FirstOrDefault(c => c.Website.ToLower() == website.ToLower());
        }
    }
}

