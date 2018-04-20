namespace EventManagement.Resources.Shared.Events
{
    using System;

    public class AddEvent
    {
        public int EventId { get; set; }

        public string EventName { get; set; }

        public string Location { get; set; }

        public double Price { get; set; }

        public string AdditionalInfo { get; set; }

        public DateTime End { get; set; }

        public string LecturerName { get; set; }

        public string ResourceName { get; set; }

        public DateTime Start { get; set; }

        public int ResourcePlacesCount { get; set; }
    }
}
