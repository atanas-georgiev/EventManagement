namespace EventManagement.Resources.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Resource
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Location { get; set; }

        public int PlacesCount { get; set; }

        public double Rent { get; set; }
    }
}
