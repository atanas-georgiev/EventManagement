
namespace EventManagement.Portal.Web.ViewModels
{
    using System.Collections.Generic;

    using KPMG.TaxRay.Portal.Core.Entities;

    public class AvailableSettingsVM
    {
        public AvailableSettingsVM(
            List<UserDateTimeFormat> dateFormats,
            List<UserLanguage> languages)
        {
            this.DateFormats = new List<IdDescriptionVM>();
            foreach (var entity in dateFormats)
            {
                this.DateFormats.Add(new IdDescriptionVM() { Id = entity.Id, Description = entity.Description });
            }

            this.Languages = new List<IdDescriptionVM>();
            foreach (var entity in languages)
            {
                this.Languages.Add(new IdDescriptionVM() { Id = entity.Id, Description = entity.Description });
            }
        }

        public List<IdDescriptionVM> DateFormats { get; set; }

        public List<IdDescriptionVM> Languages { get; set; }
    }
}