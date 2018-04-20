namespace EventManagement.Portal.Business.UserSettings
{
    using System.Threading.Tasks;

    using KPMG.TaxRay.Portal.Core.Entities;

    /// <summary>
    /// Provides information about user settings.
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Get settings of particular user.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <returns>Settings of the users.</returns>
        Task<UserSetting> GetSettingsAsync(string userName);

        /// <summary>
        /// Update settings of particular user.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="language">User selected language.</param>
        /// <param name="dateFormat">User selected date format.</param>
        /// <returns>Nothing to return.</returns>
        Task UpdateSettingsAsync(string userName, int language, int dateFormat);
    }
}