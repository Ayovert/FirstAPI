using System;
using APIDemo.Entities;
using APIDemo.Models;
using AutoMapper;

namespace APIDemo.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDTO>();
        }
    }
}
