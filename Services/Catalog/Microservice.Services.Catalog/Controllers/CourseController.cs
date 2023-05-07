using Microservice.Services.Catalog.Dtos;
using Microservice.Services.Catalog.Services;
using Microservice.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string id)
        {
            var response = await _courseService.GetAllAsync();
            return CreateActionResultInstance(response); //generic vermedik çünkü kendi parametresindeki generic ifadeden otomatik çıkacak.
        }

        [HttpGet("{id}")] //course/id
        public async Task<IActionResult> GetByUserId(string id)
        {
            var response = await _courseService.GetAllByUserId(id);
            return CreateActionResultInstance(response); //generic vermedik çünkü kendi parametresindeki generic ifadeden otomatik çıkacak.
        }

        [Route("getbyid/{id}")]
        [HttpGet()] //course/id
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetById(id);
            return CreateActionResultInstance(response); //generic vermedik çünkü kendi parametresindeki generic ifadeden otomatik çıkacak.
        }

        [HttpPost]
        public async Task<IActionResult> Crate(CourseCreateDto course)
        {
            var response = await _courseService.CreateAsync(course);
            return CreateActionResultInstance(response); //generic vermedik çünkü kendi parametresindeki generic ifadeden otomatik çıkacak.
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto course)
        {
            var response = await _courseService.UpdateAsync(course);
            return CreateActionResultInstance(response); //generic vermedik çünkü kendi parametresindeki generic ifadeden otomatik çıkacak.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteAsync(id);
            return CreateActionResultInstance(response); //generic vermedik çünkü kendi parametresindeki generic ifadeden otomatik çıkacak.
        }

    }
}
