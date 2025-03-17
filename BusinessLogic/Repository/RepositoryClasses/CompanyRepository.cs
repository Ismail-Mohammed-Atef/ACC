using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Enums;
using DataLayer.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {

        AppDbContext Context;
        public CompanyRepository(AppDbContext context) : base(context)
        {
            Context = context; ;
        }

       


    }
}
