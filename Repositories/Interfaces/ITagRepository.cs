using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllTags();
        Task<Tag> GetTagById(int id);
        Task Add(Tag tag);
        Task Update(Tag tag);
        Task Delete(int id);
    }
}
