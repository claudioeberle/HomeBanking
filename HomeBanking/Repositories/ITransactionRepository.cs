using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();
        void Save(Transaction transaction);
        IEnumerable<Transaction> GetTransactionsByClient(long clientId);
    }
}
