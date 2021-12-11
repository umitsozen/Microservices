using AutoMapper;
using MongoDB.Driver;
using Project.Shared.Dtos;
using Services.Catalog.Dtos;
using Services.Catalog.Models;
using Services.Catalog.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Services.Catalog.Services
{
    public class CourseService: ICourseService
    {
        public IMongoCollection<Course> _courseCollection;
        public IMongoCollection<Category> _categoryCollection;
        public IMapper _mapper;

        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnctionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }

        public async Task<Response<List<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find<Course>(course => true).ToListAsync();

            if (courses.Any())
            {
                foreach (var item in courses)
                {
                    item.Category = await _categoryCollection.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();
                }
            }
            else
                courses = new List<Course>();

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course != null)
            {
               // if (course.Category == null)
                //    course.Category = new Category();
                course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                return Response<CourseDto>.Success(_mapper.Map<CourseDto>(course), 200);
            }
            else
                return Response<CourseDto>.Fail("Course not found", 404);

        }

        public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();
            if (!courses.Any())
                foreach (var item in courses)
                    item.Category = await _categoryCollection.Find<Category>(x => x.Id == item.CategoryId).FirstAsync();
            else
                courses = new List<Course>();

            return Response<List<CourseDto>>.Success(_mapper.Map<List<CourseDto>>(courses), 200);
        }

        public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
        {
            var newCourse = _mapper.Map<Course>(courseCreateDto);
            newCourse.CreatedTime = DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);
            return Response<CourseDto>.Success(_mapper.Map<CourseDto>(newCourse), 200);
        }
        public async Task<Response<NoContentResult>> UpdateAsync(CourseUpdateDto courseUpdateDto)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDto);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDto.Id, updateCourse);
            if (result == null)
                return Response<NoContentResult>.Fail("Course not found", 404);
            return Response<NoContentResult>.Success(204);
        }
        public async Task<Response<NoContentResult>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
                return Response<NoContentResult>.Success(204);
            return Response<NoContentResult>.Fail("Course not found", 404);

        }
    }
}
