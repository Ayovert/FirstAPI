using System;
using System.Collections.Generic;
using System.Linq;
using APIDemo.Entities;
using APIDemo.Models;
using APIDemo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APIDemo.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CityController: ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CityController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        public IActionResult GetCities()
        {
            var getCities = _cityInfoRepository.GetCities();

           /*   var result = new List<CityWithoutPointOfInterestDTO>();

              foreach( var city in getCities)
              {
                  result.Add(new CityWithoutPointOfInterestDTO()
                  {
                      Id = city.Id,
                      Name = city.Name,
                      Description = city.Description
                  });
              }


              return Ok(result);*/
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointOfInterestDTO>>(getCities));

        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointOfInterest = false)
        {

            var getCity = _cityInfoRepository.GetCity(id, includePointOfInterest);

            if(getCity == null)
            {
                return NotFound();
            }

            if(includePointOfInterest)
            {
                var cityResult =
                    new CityDTO()
                    {
                        Id = getCity.Id,
                        Name = getCity.Name,
                        Description = getCity.Description
                    };

                foreach( var result in getCity.PointsOfInterest )
                {
                    cityResult.PointsOfInterest.Add(
                        new PointsOfInterestDTO()
                        {
                            Id = result.Id,
                            Name = result.Name,
                            Description = result.Description
                        }
                        );
                }

                return Ok(cityResult);
            }

            var cityWithoutPOI =
                new CityWithoutPointOfInterestDTO()
                {
                    Id = getCity.Id,
                    Name = getCity.Name,
                    Description = getCity.Description
                };

            return Ok(cityWithoutPOI);
        }

    }
}
