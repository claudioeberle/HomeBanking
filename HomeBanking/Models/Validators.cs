using HomeBanking.Models.Enum;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        public static bool IsValidName(string name)
        {
            if(!string.IsNullOrEmpty(name) 
                && HasOnlyLetters(name)
                && name.Length >= 3)
            {
                return true;
            }
            return false;
        }

        public static bool HasOnlyLetters(string chain)
        {
            bool result = false;

            if(!string.IsNullOrEmpty(chain))
            {
                foreach(var letter in chain)
                {
                    if(!char.IsLetter(letter))
                    {
                        result = false;
                    }
                }
                result = true;
            }
            return result;
        }

        public static bool HasANumber(string chain)
        {
            if(!string.IsNullOrEmpty(chain))
            {
                foreach (var letter in chain)
                {
                    if (char.IsDigit(letter))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool HasCapitalLetter(string chain)
        {
            if(!string.IsNullOrEmpty (chain))
            {
                foreach( var letter in chain)
                {
                    if(char.IsUpper(letter))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool HasLowerLetter(string chain)
        {
            if (!string.IsNullOrEmpty(chain))
            {
                foreach (var letter in chain)
                {
                    if (char.IsLower(letter))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool IsValidPassword(string password)
        {
            if(!string.IsNullOrEmpty(password)
                && HasANumber(password)
                && HasCapitalLetter(password)
                && HasLowerLetter(password)
                && password.Length >= 8)
            {
                return true;
            }
            return false;
        }

        public static bool IsValidEmail(string email)
        {
            var validator = new EmailAddressAttribute();

            if(!string.IsNullOrEmpty(email)
                && validator.IsValid(email))
            {
                return true;
            }
            return false;
        }

    }
}
