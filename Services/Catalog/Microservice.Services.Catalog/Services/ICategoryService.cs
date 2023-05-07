using Microservice.Services.Catalog.Dtos;
using Microservice.Services.Catalog.Models;
using Microservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<ResponseDto<List<CategoryDto>>> GetAllAsync();
        Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto category);
        Task<ResponseDto<CategoryDto>> GetByIdAsync(string id);
    }
}
