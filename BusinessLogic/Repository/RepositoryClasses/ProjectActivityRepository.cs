using BusinessLogic.Repository.RepositoryClasses;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer;

public class ProjectActivityRepository : GenericRepository<ProjectActivities>, IProjectActivityRepository
{
    AppDbContext context;

    public ProjectActivityRepository(AppDbContext context) : base(context)
    {
        this.context = context;
    }


    public List<ProjectActivities> GetByProjectId(int Proid)
    {
        return context.ProjectActivities.Where(p=>p.projectId == Proid).ToList();   
    }
    public void AddNewActivity(object newObject , int? id)
    {
        ProjectActivities newActivity;
        if (newObject is Project project)
        {
            newActivity = new ProjectActivities
            {
                projectId = project.Id,
                Date = DateTime.Now,
                ActivityType = "Project Created",
                ActivityDetail = $"{project.Name} has been created.",
            };

            context.Add(newActivity);
            context.SaveChanges();
        }
        else if (newObject is Company company)
            {
            newActivity = new ProjectActivities
            {
                projectId = id??0,
                Date = DateTime.Now,
                ActivityType = "Company Added",
                ActivityDetail = $"{company.Name} has been added."
            };
            context.Add(newActivity);
            context.SaveChanges();
        }
    }

    public void RemoveActivity(object RemovedObject)
    {
        if (RemovedObject is Project project)
        {
            ProjectActivities newActivity = new ProjectActivities
            {
                Date = DateTime.Now,
                ActivityType = "Project Removed",
                ActivityDetail = $"Project '{project.Name}' has been Removed.",
            };

            context.Add(newActivity);
            context.SaveChanges();
        }
        else if (RemovedObject is Company company)
        {
            ProjectActivities newActivity = new ProjectActivities
            {

                Date = DateTime.Now,
                ActivityType = "Company Removed",
                ActivityDetail = $"{company.Name} has been Removed.",
            };
            context.Add(newActivity);
            context.SaveChanges();
        }
    }

    
}
