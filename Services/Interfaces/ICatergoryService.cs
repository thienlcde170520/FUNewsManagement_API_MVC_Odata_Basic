using BusinessObjects.Models;
using DataAccessObjects;
using FUnewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICatergoryService
    {
        Task<IEnumerable<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategoryById(short id);
        Task Create(CategoryDTO category);
        Task Update(CategoryDTO category);
        Task Delete(short id);
    }
}
