using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICatergoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<Category> GetCategoryById(short id);
        Task Add(Category category);
        Task Update(Category category);
        Task Delete(short id);
        Task<bool> HasNewsArticle(short categoryId);
    }
}
