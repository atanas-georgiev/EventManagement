namespace KPMG.TaxRay.Portal.Business.Links
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    using KPMG.TaxRay.Portal.Core.Entities;

    using EventManagement.Portal.Data;

    using Microsoft.EntityFrameworkCore;

    public class LinksService : ILinksService
    {
        private readonly PortalDbContext portalDbContext;

        public LinksService(PortalDbContext portalDbContext)
        {
            this.portalDbContext = portalDbContext;
        }

        public async Task<List<PortalLink>> GetLinksAsync(int languageId, bool admin = false)
        {
            if (admin)
            {
                return await this.portalDbContext.PortalLinks.Include(x => x.Language).Where(link => link.Language.Id == languageId).ToListAsync();
            }

            return await this.portalDbContext.PortalLinks.Include(x => x.Language).Where(link => link.Language.Id == languageId && !link.IsAdmin).ToListAsync();
        }
    }
}