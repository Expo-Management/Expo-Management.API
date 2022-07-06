﻿using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities.Events
{
    public class EventInput
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Details { get; set; }
        public int FairId { get; set; }
    }
}
