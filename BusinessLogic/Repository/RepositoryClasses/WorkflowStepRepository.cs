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
    public class WorkflowStepRepository : GenericRepository<WorkflowStepTemplate> , IWorkFlowStepRepository
    {
        private readonly AppDbContext Context;

        public WorkflowStepRepository(AppDbContext context) : base(context) 
        {
            Context = context;
        }
    }
}
