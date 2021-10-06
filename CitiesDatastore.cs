using System;
using System.Collections.Generic;
using APIDemo.Models;

namespace APIDemo
{
    public class CitiesDatastore
    {
        public static CitiesDatastore Current { get; } = new CitiesDatastore();

        public List<CityDTO> Cities { get; set; }

        public CitiesDatastore()
        {
            Cities = new List<CityDTO>()
            {
                new CityDTO()
                {
                     Id = 1,
                     Name = "New York City",
                     Description = "The one with that big park.",
                     PointsOfInterest = new List<PointsOfInterestDTO>()
                     {
                         new PointsOfInterestDTO() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PointsOfInterestDTO() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },
                new CityDTO()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                    PointsOfInterest = new List<PointsOfInterestDTO>()
                     {
                         new PointsOfInterestDTO() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointsOfInterestDTO() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },
                new CityDTO()
                {
                    Id= 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    PointsOfInterest = new List<PointsOfInterestDTO>()
                     {
                         new PointsOfInterestDTO() {
                             Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointsOfInterestDTO() {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum." },
                     }
                }
            };

        }
    }
}
