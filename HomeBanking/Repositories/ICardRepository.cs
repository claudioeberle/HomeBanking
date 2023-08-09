using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface ICardRepository
    {
        Card FindByNumber(string number);
        void Save(Card card);
    }
}
