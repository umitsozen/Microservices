using Project.Shared.Dtos;
using Services.Catalog.Dtos;
using Services.Catalog.Models;

namespace Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto);
        Task<Response<CategoryDto>> GetById(string id);


    }
}
