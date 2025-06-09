using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISystemAccountRepository
    {
        Task<IEnumerable<SystemAccount>> GetAllAccounts();
        Task<SystemAccount> GetAccountById(short id);
        Task Add(SystemAccount account);
        Task Update(SystemAccount account);
        Task Delete(short id);
        Task<SystemAccount?> GetByEmail(string email);
        Task<SystemAccount?> Login(string email, string password);
        Task<bool> HasNewsArticle(int accountId);
    }
}
