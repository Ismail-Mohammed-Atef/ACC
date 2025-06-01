using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class DocumentVersionRepository : GenericRepository<DocumentVersion> , IDocumentVersionRepository
    {
        public DocumentVersionRepository(AppDbContext context) : base(context)
        {

        }
    }

}
