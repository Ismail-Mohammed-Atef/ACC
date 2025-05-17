using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class IfcFileRepository
    {
        private readonly AppDbContext _context;

        public IfcFileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<IfcFile>> GetByProjectIdAsync(int projectId)
        {
            return await _context.IfcFiles
                .Where(f => f.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IfcFile> GetByIdAsync(int id)
        {
            return await _context.IfcFiles.FindAsync(id);
        }

        public async Task AddAsync(IfcFile ifcFile)
        {
            _context.IfcFiles.Add(ifcFile);
            await _context.SaveChangesAsync();
        }
    }
}
