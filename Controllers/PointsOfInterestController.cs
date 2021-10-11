using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading;
using APIDemo.Models;
using APIDemo.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace APIDemo.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;


        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));

            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));

          
            





        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {


                var cityExists = _cityInfoRepository.CityExists(cityId);


                if (!cityExists)
                {
                    _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest");
                    return NotFound("City not found" );
                }


                var pointsOfInterest = _cityInfoRepository.GetPointsOfInterest(cityId);
                if (pointsOfInterest == null)
                {
                    
                    return NotFound("Point Of Interest not found");
                }

                var points = new List<PointsOfInterestDTO>();

                foreach (var poi in pointsOfInterest)
                {
                    points.Add(new PointsOfInterestDTO()
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    });
                }



                return Ok(points);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var cityExists = _cityInfoRepository.CityExists(cityId);

            if (!cityExists)
            {
                _logger.LogInformation($"City with id {cityId} was not found when accessing points of interest");
                return NotFound("City not found");
            }

            var pointOfInterestFromCity = _cityInfoRepository.GetPointOfInterest(cityId, id);

            if (pointOfInterestFromCity == null)
            {
                return NotFound($"Point of Interest {id} not found");
            }

            var pointOfInterest = new List<PointsOfInterestDTO>();

            pointOfInterest.Add(
                new PointsOfInterestDTO()
                {
                    Id = pointOfInterestFromCity.Id,
                    Name = pointOfInterestFromCity.Name,
                    Description = pointOfInterestFromCity.Description

                }
                );

          


            return Ok(pointOfInterest);

        }

        [HttpPost]
        public IActionResult CreatePointsOfInterest(int cityId, [FromBody] PointOfInterestCreationDTO pointsOfInterest)
        {
            if (pointsOfInterest.Name == pointsOfInterest.Description)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var city = CitiesDatastore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }


            var maxPointsOfInterestId = CitiesDatastore.Current.Cities.SelectMany(
                c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointsOfInterestDTO()
            {
                Id = ++maxPointsOfInterestId,
                Name = pointsOfInterest.Name,
                Description = pointsOfInterest.Description
            };
            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new { cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);
        }



        [HttpPut("{id}")]
        public IActionResult UpdatePointsOfInterest(int cityId, int Id, [FromBody] PointOfInterestUpdateDTO pointOfInterest)
        {
            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var city = CitiesDatastore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }


            var pointOfInterestForCity = city.PointsOfInterest.FirstOrDefault(p => p.Id == Id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            pointOfInterestForCity.Name = pointOfInterest.Name;
            pointOfInterestForCity.Description = pointOfInterest.Description;

            return NoContent();
        }


        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointsOfInterest(int cityId, int Id, [FromBody]
        JsonPatchDocument<PointOfInterestUpdateDTO> patchDocument)
        {

            var city = CitiesDatastore.Current.Cities.FirstOrDefault(x => x.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }


            var pointOfInterestForCity = city.PointsOfInterest.FirstOrDefault(p => p.Id == Id);

            if (pointOfInterestForCity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch =
                new PointOfInterestUpdateDTO()
                {
                    Name = pointOfInterestForCity.Name,
                    Description = pointOfInterestForCity.Description
                };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name");
            }


            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }
            pointOfInterestForCity.Name = pointOfInterestToPatch.Name;
            pointOfInterestForCity.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int Id)
        {
            var city = CitiesDatastore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound("City not found");
            }

            var pointOfInterestForCity = city.PointsOfInterest.FirstOrDefault(c => c.Id == Id);

            if (pointOfInterestForCity == null)
            {
                return NotFound("Point of Interest not found ");
            }


            city.PointsOfInterest.Remove(pointOfInterestForCity);
            _mailService.Send("Point Of Interest Deleted",$"Point of interest {pointOfInterestForCity.Name} with id {pointOfInterestForCity.Id} was deleted");

            _logger.LogInformation($"Mail was sent via {_mailService.ToString()}");
            return NoContent();


        }






    }
}
