using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace HomeBanking.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Card FindByNumber(string number)
        {
            return FindByCondition(card => card.Number == number).FirstOrDefault();
        }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
    }
}
