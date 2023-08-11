using HomeBanking.Models.Enum;

namespace HomeBanking.Models
{
    public static class Validators
    {
        public static bool IsCardType(string cardType)
        {
            return CardType.TryParse(cardType, out CardType result);
        }

        public static bool IsCardColor(string cardColor)
        {
            return CardColor.TryParse(cardColor, out CardColor result);
        }


    }
}
