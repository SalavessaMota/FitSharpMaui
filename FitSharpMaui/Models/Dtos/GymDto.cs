﻿namespace FitSharpMaui.Models.Dtos
{
    internal class GymDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfEquipments { get; set; }
        public double Rating { get; set; }
        public string ImageUrl { get; set; }
    }
}