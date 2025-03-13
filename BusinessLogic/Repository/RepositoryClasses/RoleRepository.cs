using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
