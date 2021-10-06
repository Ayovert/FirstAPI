using System;
using System.Collections.Generic;
using APIDemo.Entities;

namespace APIDemo.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool includePointOfInterest);

        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);

        PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId);

        bool CityExists(int CityId);
    }
}
