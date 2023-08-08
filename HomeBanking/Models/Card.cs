using HomeBanking.Models.Enum;
using System;
using System.Globalization;

namespace HomeBanking.Models
{
    public class Card
    {
        public long Id { get; set; }
        public string CardHolder { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Number { get; set; }
        public int Cvv { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ThruDate { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }

        public static bool IsCardType(string cardType)
        {
            CardType result;
            return CardType.TryParse(cardType, out result);
        }

        public static bool IsCardColor(string cardColor)
        {
            CardColor result;
            return CardColor.TryParse(cardColor, out result);
        }
    }
}
