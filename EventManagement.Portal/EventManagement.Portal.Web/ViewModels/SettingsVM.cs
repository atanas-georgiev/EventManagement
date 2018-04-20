namespace EventManagement.Portal.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    
    public class SettingsVM
    {
        [Required]
        public int? DateFormat { get; set; }
        
        [Required]
        public int? Language { get; set; }
    }
}