namespace EventManagement.Portal.Business.AvailableSettings
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KPMG.TaxRay.Portal.Core.Entities;

    /// <summary>
    /// Provides available settings.
    /// </summary>
    public interface IAvailableSettingsService
    {
        /// <summary>
        /// Gets available date formats.
        /// </summary>
        /// <returns>List of available date formats.</returns>
        Task<List<UserDateTimeFormat>> GetAvailableDateFormatsAsync();

        /// <summary>
        /// Gets available languages.
        /// </summary>
        /// <returns>List of available languages.</returns>
        Task<List<UserLanguage>> GetAvailableLanguagesAsync();
    }
}
