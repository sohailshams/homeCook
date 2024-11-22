using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeCook.Api.DTOs;

namespace HomeCook.Api.Services
{
    public interface ICategoriesService
    {
        public Task<List<CategoryDTO>> GetCategoriesAsync();
    }
}