using BusinessObjects.Models;
using FUnewsDTO;
using Repositories.Interfaces;
using Repositories.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class CatergoryService : ICatergoryService
    {
        private readonly HttpClient _httpClient;

        public CatergoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>("Category");
        }

        public async Task<CategoryDTO> GetCategoryById(short id)
        {
#pragma warning disable CS8603
            return await _httpClient.GetFromJsonAsync<CategoryDTO>($"Category/{id}");
#pragma warning restore CS8603
        }

        public async Task Create(CategoryDTO category)
        {
            await _httpClient.PostAsJsonAsync("Category", category);
        }

        public async Task Update(CategoryDTO category)
        {
            await _httpClient.PutAsJsonAsync($"Category/{category.CategoryId}", category);
        }

        public async Task Delete(short id)
        {
            await _httpClient.DeleteAsync($"Category/{id}");
        }
    }
}
