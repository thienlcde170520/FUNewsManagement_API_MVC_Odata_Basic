using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public async Task Add(SystemAccount account)
        {
            await AccountDAO.Instance.AddAsync(account);
        }

        public async Task Delete(short id)
        {
            await AccountDAO.Instance.DeleteAsync(id);
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAccounts()
        {
            return await AccountDAO.Instance.GetAllAsync();
        }

        public async Task<SystemAccount> GetAccountById(short id)
        {
            return await AccountDAO.Instance.GetByIdAsync(id);
        }

        public async Task Update(SystemAccount account)
        {
            await AccountDAO.Instance.UpdateAsync(account);
        }

        public async Task<SystemAccount?> GetByEmail(string email)
        {
            return await AccountDAO.Instance.GetByEmail(email);
        }

        public async Task<SystemAccount?> Login(string email, string password)
        {
            return await AccountDAO.Instance.Login(email, password);
        }

        public async Task<bool> HasNewsArticle(int categoryId)
        {
            return await AccountDAO.Instance.HasNewsArticle(categoryId);
        }
    }
}
