namespace EventManagement.Portal.Business.AvailableSettings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EventManagement.Portal.Data;

    using KPMG.TaxRay.Portal.Core.Entities;

    using Microsoft.EntityFrameworkCore;

    public class AvailableSettingsService : IAvailableSettingsService
    {
        private readonly PortalDbContext portalDbContext;

        public AvailableSettingsService(PortalDbContext portalDbContext)
        {
            this.portalDbContext = portalDbContext;
        }

        public async Task<List<UserDateTimeFormat>> GetAvailableDateFormatsAsync()
        {
            return await this.portalDbContext.UserDateTimeFormats.ToListAsync();
        }

        public async Task<List<UserLanguage>> GetAvailableLanguagesAsync()
        {
            return await this.portalDbContext.UserLanguages.ToListAsync();
        }
    }
}
