using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IWorkflowRepository : IGenericRepository<WorkflowTemplate>
    {

         List<WorkflowTemplate> GetAllWithSteps();
        WorkflowTemplate GetById(int Id);
        

    }
}
