using Microsoft.AspNetCore.Mvc;
using Project.Shared.Dtos;
using Services.Catalog.Dtos;

namespace Services.Catalog.Services
{
    public interface ICourseService
    {
        Task<Response<List<CourseDto>>> GetAllAsync();
        Task<Response<CourseDto>> GetByIdAsync(string id);
        Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId);
        Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto);
        Task<Response<NoContentResult>> UpdateAsync(CourseUpdateDto courseUpdateDto);
        Task<Response<NoContentResult>> DeleteAsync(string id);
    }
}
