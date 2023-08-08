using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card FindById(long id);
        Card FindByNumber(string number);
        void Save(Card card);
        IEnumerable<Card> GetCardByClient(long clientId);
    }
}
