using Dapper;
using Microservice.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        //postgresql
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection; //herhangi bir db ile iletişime geçmek istediğimizde kullanacağımzı interface IDbConnection

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<ResponseDto<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("DELETE FROM discount where id = @Id",
                                new { Id = id });

            return status > 0 ? ResponseDto<NoContent>.Success(204) : ResponseDto<NoContent>.Failure("discount not found", 404);

        }

        public async Task<ResponseDto<List<Model.Discount>>> GetAll()
        {
            var discount = await _dbConnection.QueryAsync<Model.Discount>("Select * From discount");

            return ResponseDto<List<Model.Discount>>.Success(discount.ToList(),200);
        }

        public async Task<ResponseDto<Model.Discount>> GetByCodeAndUserId(string code, string userid)
        {
            var discount = await _dbConnection.QueryAsync<Model.Discount>("Select * From discount where userid = @UserId and code = @Code",
                                                                            new { Code = code, UserId = userid});
            var hasDiscount = discount.FirstOrDefault();

            if (hasDiscount == null)
            {
                return ResponseDto<Model.Discount>.Failure("discount not found",404);
            }
            return ResponseDto<Model.Discount>.Success(hasDiscount,200);
        }

        public async Task<ResponseDto<Model.Discount>> GetById(int id)
        {
            var discount = (await _dbConnection.QueryAsync<Model.Discount>("select * from discount where id = @id", new { id })).SingleOrDefault();

            if (discount == null)
            {
                return ResponseDto<Model.Discount>.Failure("Discount not found",404);
            }
            return ResponseDto<Model.Discount>.Success(discount, 200);
        }

        public async Task<ResponseDto<NoContent>> Save(Model.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("Insert Into discount (userid,rate,code) Values(@userid,@rate,@code)",discount); //nesneyi verirsek kendi kendi map'leyip eşitler

            if (saveStatus > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }
            return ResponseDto<NoContent>.Failure("an error occured while adding",500);
        }

        public async Task<ResponseDto<NoContent>> Update(Model.Discount discount)
        {
            var status = await _dbConnection.ExecuteAsync("update discount set userid = @UserId, code = @Code, rate=@Rate where id = @Id", 
                                new { UserId = discount.UserId, Code=discount.Code, Rate = discount.Rate, Id = discount.Id });

            if (status > 0)
            {
                return ResponseDto<NoContent>.Success(204);
            }

            return ResponseDto<NoContent>.Failure("discount not found", 404);

        }
    }
}
