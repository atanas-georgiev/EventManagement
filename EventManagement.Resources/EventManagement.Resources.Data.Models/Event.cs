namespace EventManagement.Resources.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Event
    {
        [MaxLength(500)]
        public string AdditionalInfo { get; set; }

        public DateTime End { get; set; }

        public int Id { get; set; }

        [MaxLength(100)]
        public string LecturerName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public double Price { get; set; }

        public virtual Resource Resource { get; set; }

        public DateTime Start { get; set; }
    }
}