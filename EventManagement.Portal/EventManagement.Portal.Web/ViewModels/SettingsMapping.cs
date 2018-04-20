namespace EventManagement.Portal.Web.ViewModels
{
    public static class SettingsMapping
    {
        public static SettingsVM FromUserSettings(KPMG.TaxRay.Portal.Core.Entities.UserSetting settings)
        {
            var result = new SettingsVM();

            if (settings != null)
            {
                result.Language = settings.LanguageId;
                result.DateFormat = settings.DateTimeFormatId;
            }
            else
            {
                result.Language = 1;
                result.DateFormat = 1;
            }

            return result;
        }
    }
}
