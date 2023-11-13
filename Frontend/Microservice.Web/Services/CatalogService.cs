using Microservice.Shared.Dtos;
using Microservice.Web.Models;
using Microservice.Web.Models.Catalog;
using Microservice.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Microservice.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<bool> CreateCourse(CourseCreateInput courseCreateInput)
        {
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course",courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourses(string coursesId)
        {
            var response = await _httpClient.DeleteAsync($"course/{coursesId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CategoryViewModel>> GetAllCategories()
        {
            var response = await _httpClient.GetAsync("categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<ResponseDto<List<CategoryViewModel>>>();

            return data.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourses()
        {
            //http://localhost:5000/service/catalog/course
            var response = await _httpClient.GetAsync("course");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<ResponseDto<List<CourseViewModel>>>();

            return data.Data; 
        }

        public async Task<List<CourseViewModel>> GetCourseByCourseId(string courseId)
        {
            var response = await _httpClient.GetAsync($"course/getbyid/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<ResponseDto<List<CourseViewModel>>>();

            return data.Data;
        }

        public async Task<List<CourseViewModel>> GetCoursesByUserId(string userId)
        {
            var response = await _httpClient.GetAsync($"course/GetByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var data = await response.Content.ReadFromJsonAsync<ResponseDto<List<CourseViewModel>>>();

            return data.Data;

        }

        public async Task<bool> UpdateCourse(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("course", courseUpdateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
