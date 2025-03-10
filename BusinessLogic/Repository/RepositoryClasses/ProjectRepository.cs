using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class ProjectRepository : GenericRepository<Project> ,IProjetcRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }
    }
}
