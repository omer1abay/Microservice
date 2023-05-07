using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Microservice.Shared.Dtos
{
    //Başarılı mı Başarısız mı? ayrı ayrı dto nesnesi(ErrorDto, SuccessDto) dönebiliriz ikisi için ortak bir dto nesnesi de dönebiliriz
    public class ResponseDto<T>
    {
        public T Data { get; private set; }

        [JsonIgnore] //json'da görünmesine gerek yok response içinde
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccess { get; private set; }

        public List<string> Errors { get; set; }

        #region Methods

        //Static Factory Metotlar : static bir metotlar geriye bir nesne dönen metotlar
        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { Data = data, StatusCode = statusCode, IsSuccess = true };
        }

        public static ResponseDto<T> Success(int statusCode)
        {
            return new ResponseDto<T> { Data = default(T), StatusCode = statusCode, IsSuccess = true };
        }

        public static ResponseDto<T> Failure(List<string> errors, int statusCode)
        {
            return new ResponseDto<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccess = false
            };
        }

        public static ResponseDto<T> Failure(string error, int statusCode)
        {
            return new ResponseDto<T>
            {
                Errors = new List<string>() { error },
                StatusCode = statusCode,
                IsSuccess = false
            };
        }

        #endregion

    }
}
