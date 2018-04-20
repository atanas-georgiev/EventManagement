namespace EventManagement.Resources.Web.ViewModels.Events
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AddEventApiModel
    {
        [MaxLength(500)]
        public string AdditionalInfo { get; set; }

        public DateTime End { get; set; }

        [MaxLength(100)]
        public string LecturerName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public double Price { get; set; }

        public int ResourceId { get; set; }

        public DateTime Start { get; set; }
    }
}
