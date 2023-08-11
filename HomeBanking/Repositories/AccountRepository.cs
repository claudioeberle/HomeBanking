using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public Account FindById(long id)
        {
            return FindByCondition(Account => Account.Id == id)
                .Include(Account => Account.Transactions)
                .FirstOrDefault();
        }

        public Account FindByNumber(string number)
        {
            return FindByCondition(ac => ac.Number.ToUpper() == number.ToUpper())
                .Include(Account => Account.Transactions)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll().Include(account => account.Transactions).ToList();
        }

        public void Save(Account account)
        {
            if(account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }
            SaveChanges();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
                .Include(acc => acc.Transactions)
                .ToList();
        }

        public bool ExistsInContext(Account account)
        {
            if(account != null)
            {
                Account accountInContext = FindByNumber(account.Number);
                if(accountInContext != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
