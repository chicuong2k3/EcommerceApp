﻿namespace EcommerceApp.Domain.Models
{
    public class PageVisit
    {
        public Guid Id { get; set; }
        public required string AppUserId { get; set; }
        public int Times { get; set; }
        public DateTime FirstTimeVisit { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
