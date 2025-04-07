using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IProjectActivityRepository : IGenericRepository<ProjectActivities>
    {
        public void AddNewActivity(object _object);
        public void RemoveActivity(object _object);
        public List<ProjectActivities> GetByProjectId(int Proid);





    }
}
