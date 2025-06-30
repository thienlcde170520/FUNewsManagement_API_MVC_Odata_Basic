using BusinessObjects.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class NewsArticleDAO : SingletonBase<NewsArticleDAO>
    {
        public async Task<List<NewsArticle>> GetAllAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                //.OrderByDescending(n => n.Category) // Sắp xếp theo ngày tạo mới nhất
                                                       //.Include(f => f.Category)
                                                       //.Include(a => a.NewsTags)
                                                       //.ThenInclude(nt => nt.Tag)
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetAllActiveAsync()
        {
            return await _context.NewsArticles
                .Include(n => n.Tags)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.NewsStatus == false) 
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetByIdAsync(string id)
        {
            return await _context.NewsArticles
                .Include(n => n.Category)           // Load thông tin Category
                .Include(n => n.CreatedBy)          // Load thông tin người tạo
                //.Include(n => n.UpdatedBy)          // Load thông tin người cập nhật
                .Include(n => n.Tags)               // Load danh sách tags

                //.Include(f => f.Category)
                //.Include(a => a.NewsTags)
                //.ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleId.Equals(id));
        }

        public async Task AddAsync(NewsArticle entity, List<int>? tagIds = null)
        {
            _context.NewsArticles.Add(entity);

            // Handle tags nếu có
            if (tagIds != null && tagIds.Any())
            {
                var tags = await _context.Tags
                    .Where(t => tagIds.Contains(t.TagId))
                    .ToListAsync();

                foreach (var tag in tags)
                {
                    entity.Tags.Add(tag);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle entity)
        {
            var existing = await _context.NewsArticles
                .FirstOrDefaultAsync(x => x.NewsArticleId == entity.NewsArticleId);

            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _context.NewsArticles.FindAsync(id);
            if (entity != null)
            {
                _context.NewsArticles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NewsArticle>> GetByAuthorAsync(short authorId)
        {
            return await _context.NewsArticles
                .Where(n => n.CreatedBy.AccountId == authorId)
                .Include(n => n.Tags)
                .Include(n => n.Category)
                .ToListAsync();
        }
    }
}
