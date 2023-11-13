using AutoMapper;
using MassTransit;
using Microservice.Services.Catalog.Dtos;
using Microservice.Services.Catalog.Models;
using Microservice.Services.Catalog.Settings;
using Microservice.Shared.Dtos;
using Microservice.Shared.Messages;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private IMapper _mapper;
        private readonly IPublishEndpoint publishEndpoint;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllAsync()
        {

            //ilişkileri tek tek eklememiz lazım çünkü ilişkisel bir db kullanmıyoırz
            var courses = await _courseCollection.Find(c => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x=> x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<ResponseDto<CourseDto>> GetById(string id)
        {
            var course = await _courseCollection.Find<Course>(x=> x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return ResponseDto<CourseDto>.Failure("Course not found",404);
            }
            course.Category = await _categoryCollection.Find<Category>(x=> x.Id == course.CategoryId).FirstAsync();

            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);

        }

        public async Task<ResponseDto<List<CourseDto>>> GetAllByUserId(string userid)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userid).ToListAsync();

            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }

            return ResponseDto<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);

        }

        public async Task<ResponseDto<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);

            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return ResponseDto<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse),200);

        }

        public async Task<ResponseDto<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x=> x.Id == courseUpdateDto.Id, updateCourse); //id'ye göre değeri bul ve updateCourse ile değiştir. bulamazsa result null döner

            if (result == null)
            {
                return ResponseDto<NoContent>.Failure("Course not found", 404);
            }

            await publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent
            {
                CourseId = updateCourse.Id,
                UpdateName = courseUpdateDto.Name,
            });

            return ResponseDto<NoContent>.Success(204);

        }

        public async Task<ResponseDto<NoContent>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x=> x.Id == id);

            if (result.DeletedCount > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            else
            {
                return ResponseDto<NoContent>.Failure("Course not found",404);
            }

        }

    }
}
