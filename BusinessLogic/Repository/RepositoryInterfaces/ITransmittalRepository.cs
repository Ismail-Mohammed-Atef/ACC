using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Models;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface ITransmittalRepository
    {
        IEnumerable<Transmittal> GetTransmittalsByProjectId(int projectId);
        void AddDocumentToTransmittal(int transmittalId, int documentVersionId, string notes);
    }
}
