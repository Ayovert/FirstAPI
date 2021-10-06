using System;
using System.Collections.Generic;

namespace APIDemo.Models
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        public ICollection<PointsOfInterestDTO> PointsOfInterest { get; set; } =
            new List<PointsOfInterestDTO>();

    }
}