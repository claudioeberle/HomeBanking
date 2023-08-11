
using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext) {}

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return FindAll();
        }

        public IEnumerable<Transaction> GetTransactionsByClient(long clientId)
        {
            return FindByCondition(transaction => transaction.Account.ClientId == clientId)
                    .ToList();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
    }
}
