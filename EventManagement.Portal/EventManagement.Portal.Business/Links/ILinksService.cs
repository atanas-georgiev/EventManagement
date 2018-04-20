namespace KPMG.TaxRay.Portal.Business.Links
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KPMG.TaxRay.Portal.Core.Entities;
    
    public interface ILinksService
    {
        Task<List<PortalLink>> GetLinksAsync(int languageId, bool admin = false);
    }
}