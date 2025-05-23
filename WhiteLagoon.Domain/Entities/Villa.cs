﻿using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Price per night")]
        [Range(10, 100000)]
        public double Price { get; set; }

        public int Sqft { get; set; }

        [Range(1,10)]
        public int Occupancy { get; set; }

        [Display(Name="Image url")]
        public string? ImageUrl { get; set; }

        public DateTime Created_Date { get; set; }

        public DateTime? Updated_Date { get; set;}
    }
}
