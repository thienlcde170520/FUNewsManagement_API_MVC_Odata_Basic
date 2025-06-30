using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllNews();
        Task<IEnumerable<NewsArticle>> GetAllActiveAsync();
        Task<NewsArticle> GetNewsById(string id);
        Task Add(NewsArticle news, List<int>? tagIds = null);
        Task Update(NewsArticle news);
        Task Delete(string id);
        Task<IEnumerable<NewsArticle>> GetByAuthor(short authorId);
    }
}
