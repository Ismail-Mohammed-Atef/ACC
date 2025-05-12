using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models.Transmittals;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface ITransmittalRepository : IGenericRepository<Transmittal>
    {

        IEnumerable<Transmittal> GetTransmittalsByProjectId(int projectId);
        IEnumerable<Transmittal> GetAllWithIncludes();
    }
}
