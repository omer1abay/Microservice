﻿using AutoMapper;
using Microservice.Services.Catalog.Dtos;
using Microservice.Services.Catalog.Models;
using Microservice.Services.Catalog.Settings;
using Microservice.Shared.Dtos;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Services.Catalog.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            //adım adım tanımlama yaptık..
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
        }


        public async Task<ResponseDto<List<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categoryCollection.Find(c => true).ToListAsync();
            return ResponseDto<List<CategoryDto>>.Success(_mapper.Map<List<CategoryDto>>(categories),200);
        }


        public async Task<ResponseDto<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var category= _mapper.Map<Category>(categoryDto);
            await _categoryCollection.InsertOneAsync(category);
            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(category),200);
        }

        public async Task<ResponseDto<CategoryDto>> GetByIdAsync(string id)
        {
            var result = await _categoryCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();
            if (result == null)
            {
                return ResponseDto<CategoryDto>.Failure("Categroy not found", 404);
            }

            return ResponseDto<CategoryDto>.Success(_mapper.Map<CategoryDto>(result), 200);

        }

    }
}
