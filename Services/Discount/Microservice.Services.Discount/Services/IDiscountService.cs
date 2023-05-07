using Microservice.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<ResponseDto<List<Model.Discount>>> GetAll();
        Task<ResponseDto<Model.Discount>> GetById(int id);
        Task<ResponseDto<NoContent>> Save(Model.Discount discount);
        Task<ResponseDto<NoContent>> Update(Model.Discount discount);
        Task<ResponseDto<NoContent>> Delete(int id);
        Task<ResponseDto<Model.Discount>> GetByCodeAndUserId(string code, string userid);
    }
}
