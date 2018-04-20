namespace EventManagement.Portal.Business.UserSettings
{
    using System;
    using System.Threading.Tasks;

    using EventManagement.Portal.Data;

    using KPMG.TaxRay.Portal.Core.Entities;

    using Microsoft.EntityFrameworkCore;

    public class SettingsService : ISettingsService
    {
        private readonly PortalDbContext portalDbContext;

        public SettingsService(PortalDbContext portalDbContext)
        {
            this.portalDbContext = portalDbContext;
        }

        public async Task<UserSetting> GetSettingsAsync(string userName)
        {
            return await this.portalDbContext.UserSettings.FirstOrDefaultAsync(stng => stng.UserName == userName);
        }

        public async Task UpdateSettingsAsync(string userName, int language, int dateFormat)
        {
            var settings = await this.portalDbContext.UserSettings.FirstOrDefaultAsync(stng => stng.UserName == userName);
            var lang = await this.portalDbContext.UserLanguages.FirstOrDefaultAsync(l => l.Id == language);
            var df = await this.portalDbContext.UserDateTimeFormats.FirstOrDefaultAsync(d => d.Id == dateFormat);

            if (lang == null || df == null)
            {
                throw new NullReferenceException();
            }

            if (settings == null)
            {
                settings = new UserSetting
                               {
                                   UserName = userName,
                                   LanguageId = language,
                                   DateTimeFormatId = dateFormat
                               };

                await this.portalDbContext.UserSettings.AddAsync(settings);
            }
            else
            {
                settings.LanguageId = language;
                settings.DateTimeFormatId = dateFormat;

                this.portalDbContext.UserSettings.Update(settings);
            }

            await this.portalDbContext.SaveChangesAsync();
        }
    }
}