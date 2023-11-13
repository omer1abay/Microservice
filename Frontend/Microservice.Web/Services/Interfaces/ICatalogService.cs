using Microservice.Web.Models.Catalog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Web.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCourses();
        Task<List<CategoryViewModel>> GetAllCategories();
        Task<List<CourseViewModel>> GetCoursesByUserId(string userId);
        Task<List<CourseViewModel>> GetCourseByCourseId(string courseId);
        Task<bool> DeleteCourses(string coursesId);
        Task<bool> CreateCourse(CourseCreateInput courseCreateInput);
        Task<bool> UpdateCourse(CourseUpdateInput courseUpdateInput);
    }
}
