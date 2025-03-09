using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class ProjectRepository : IProjetcRepository
    {
        private readonly AppDbContext appContext;

        public ProjectRepository(AppDbContext appContext)
        {
            this.appContext = appContext;
        }
        public void Delete(Project obj)
        {
            appContext.Projects.Remove(obj);
        }

        public IList<Project> GetAll()
        {
            return appContext.Projects.ToList();
        }

        public Project? GetById(int id)
        {
            return appContext.Projects.FirstOrDefault(p => p.Id == id);
        }

        public void Insert(Project obj)
        {
            appContext.Projects.Add(obj);
        }

        public void Save()
        {
            appContext.SaveChanges();
        }

        public void Update(Project obj)
        {
            appContext.Projects.Update(obj);
        }
    }
}
