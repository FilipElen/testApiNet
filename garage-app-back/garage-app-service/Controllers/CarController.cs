﻿using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using garage_app_bl.Services;
using garage_app_entities;
using garage_app_service.AuthFilter;
using garage_app_service.DTOs.Request;
using garage_app_service.DTOs.Response;
using garage_app_service.Mappers;

namespace garage_app_service.Controllers
{
    public class CarController : ApiController
    {
        private readonly CarService _carService;
        private readonly CarsMapper _carsMapper;
        private readonly SpecificationMapper _specificationMapper;

        public CarController()
        {
            _carService = new CarService();
            _carsMapper = new CarsMapper();
            _specificationMapper = new SpecificationMapper();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("car")]
        public IHttpActionResult GetCars()
        {
            List<CarResponseDto> carResponseDtos = new List<CarResponseDto>();
            List<Product> products = _carService.GetCars();

            foreach (Product product in products)
            {
                carResponseDtos.Add(_carsMapper.ToDto(product));
            }

            return Ok(carResponseDtos);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("car/{productId}")]
        public IHttpActionResult GetCars(int productId)
        {
            Product product = _carService.FindCar(productId);
            return Ok(_carsMapper.ToDto(product));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("car/name/{productName}")]
        public IHttpActionResult GetCars(string productName)
        {
            Product product = _carService.FindCar(productName);
            return Ok(_carsMapper.ToDto(product));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("car/specification/{productId}")]
        public IHttpActionResult FindCarSpecifications(int productId)
        {
            List<Specification> findCarSpecifications = _carService.FindCarSpecifications(productId);

            List<SpecificationResponseDto> responseDtos = new List<SpecificationResponseDto>();

            foreach (Specification findCarSpecification in findCarSpecifications)
            {
                responseDtos.Add(_specificationMapper.ToDto(findCarSpecification));
            }

            return Ok(responseDtos);
        }

        [JwtAuthentication]
        [HttpPost]
        [Route("car")]
        public IHttpActionResult InsertCar(InsertCarRequestDto carRequestDto)
        {
            List<Specification> specifications = new List<Specification>();
            foreach (var insertSpecificationRequestDto in carRequestDto.Specifications)
            {
                specifications.Add(_specificationMapper.ToSpecification(insertSpecificationRequestDto));
            }

            Product insertCar = _carService.InsertCar(_carsMapper.ToProduct(carRequestDto), specifications);
            return Created($"car/{insertCar.Id}", _carsMapper.ToDto(insertCar));
        }

        [JwtAuthentication]
        [HttpPut]
        [Route("car")]
        public IHttpActionResult UpdateCar(UpdateCarRequestDto carRequestDto)
        {
            _carService.UpdateCar(_carsMapper.ToProduct(carRequestDto));
            return new StatusCodeResult(HttpStatusCode.NoContent, this);
        }

        [JwtAuthentication]
        [HttpDelete]
        [Route("car/{productId}")]
        public IHttpActionResult DeleteCar(int productId)
        {
            _carService.DeleteCar(productId);
            return new StatusCodeResult(HttpStatusCode.NoContent, this);
        }
    }
}