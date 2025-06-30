using BusinessObjects.Models;
using FUnewsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticleDTO>> GetNewsArticles();
        Task<IEnumerable<NewsArticleDTO>> GetNewsArticlesActiveAsync();
        Task<NewsArticleDTO> GetNewsArticleById(string id);
        Task Create(NewsArticleDTO newsArticle);
        Task Update(NewsArticleDTO newsArticle);
        Task Delete(string id);
        Task<IEnumerable<NewsArticleDTO>> GetNewsArticlesByAuthor();
    }
}
