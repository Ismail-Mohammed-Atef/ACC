using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using DataLayer.Models.Enums;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class ProjectRepository : GenericRepository<Project> ,IProjetcRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public List<string> GetCurrencyValues()
        {
            return Enum.GetNames(typeof(Currency)).ToList();
        }

        public List<string> GetProjectTypeValues()
        {
            return Enum.GetNames(typeof(ProjectType)).ToList();
        }
    }
}
