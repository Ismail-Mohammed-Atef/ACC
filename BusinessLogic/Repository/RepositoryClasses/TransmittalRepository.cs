using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class TransmittalRepository : GenericRepository<Transmittal>, ITransmittalRepository
    {
        private readonly AppDbContext _context;

        public TransmittalRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Transmittal> GetTransmittalsByProjectId(int projectId)
        {
            return _context.Transmittals
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.TransmittalDocuments)
                .ThenInclude(td => td.DocumentVersion)
                .ThenInclude(dv => dv.Document)
                .ToList();
        }

        public void AddDocumentToTransmittal(int transmittalId, int documentVersionId, string notes)
        {
            var transmittalDocument = new TransmittalDocument
            {
                TransmittalId = transmittalId,
                DocumentVersionId = documentVersionId,
                Notes = notes
            };
            _context.TransmittalDocuments.Add(transmittalDocument);
            _context.SaveChanges();
        }
    }
}
