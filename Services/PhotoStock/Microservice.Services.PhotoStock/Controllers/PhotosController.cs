using Microservice.Services.PhotoStock.DTOs;
using Microservice.Shared.ControllerBases;
using Microservice.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.Services.PhotoStock.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotosController : CustomControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile file, CancellationToken cancellationToken) //cancellationToken: işlemi yapan yer işlemi sonlandırırsa kaydetme işlemi de hata fırlatarak sonlansın
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream,cancellationToken);
                }
                var returnPath = "photos/" + file.FileName;

                PhotoDto photoDto = new() { Url = returnPath };
                return CreateActionResultInstance(ResponseDto<PhotoDto>.Success(photoDto,200));
            }

            return CreateActionResultInstance(ResponseDto<PhotoDto>.Failure("photo is empty", 400));

        }

        [HttpGet]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photoUrl);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ResponseDto<NoContent>.Failure("photo not found", 404));
            }
            System.IO.File.Delete(path);
            return CreateActionResultInstance(ResponseDto<NoContent>.Success(204));
        }

    }
}
