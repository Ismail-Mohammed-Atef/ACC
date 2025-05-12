using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models.Transmittals;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class TransmittalRepository : GenericRepository<Transmittal>, ITransmittalRepository
    {
        private readonly AppDbContext Context;

        public TransmittalRepository(AppDbContext context) : base(context)
        {
            Context = context;
        }

        public IEnumerable<Transmittal> GetTransmittalsByProjectId(int projectId)
        {
            return Context.Transmittals
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.SenderCompany)
                .Include(t => t.Recipients).ThenInclude(r => r.RecipientCompany)
                .Include(t => t.Files)
                .ToList();
        }

        public IEnumerable<Transmittal> GetAllWithIncludes()
        {
            return Context.Transmittals
                .Include(t => t.Project)
                .Include(t => t.SenderCompany)
                .Include(t => t.Recipients).ThenInclude(r => r.RecipientCompany)
                .ToList();
        }
    }
}
