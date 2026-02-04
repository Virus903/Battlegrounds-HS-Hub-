using BattlegroundsHubHS.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BattlegroundsHubHS.Core.Validation
{
    public static class HeroValidator
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;
        public const int TitleMaxLength = 30;
        public const int DescriptionMaxLength = 500;

        public static List<string> Validate(Hero hero)
        {
            var errors = new List<string>();

            if (hero == null)
            {
                errors.Add("Герой не может быть null");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(hero.Name))
            {
                errors.Add("Имя героя обязательно");
            }
            else
            {
                var name = hero.Name.Trim();
                if (name.Length < NameMinLength)
                {
                    errors.Add($"Имя слишком короткое. Минимум {NameMinLength} символа");
                }
                if (name.Length > NameMaxLength)
                {
                    errors.Add($"Имя слишком длинное. Максимум {NameMaxLength} символов");
                }
            }

            if (!string.IsNullOrWhiteSpace(hero.Title) && hero.Title.Trim().Length > TitleMaxLength)
            {
                errors.Add($"Титул слишком длинный. Максимум {TitleMaxLength} символов");
            }

            if (!string.IsNullOrWhiteSpace(hero.Description) && hero.Description.Trim().Length > DescriptionMaxLength)
            {
                errors.Add($"Описание слишком длинное. Максимум {DescriptionMaxLength} символов");
            }

            return errors;
        }

        public static List<string> ValidateForCreation(string name, string title, string description)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Имя героя обязательно");
            }
            else
            {
                var trimmedName = name.Trim();
                if (trimmedName.Length < NameMinLength)
                {
                    errors.Add($"Имя слишком короткое. Минимум {NameMinLength} символа");
                }
                if (trimmedName.Length > NameMaxLength)
                {
                    errors.Add($"Имя слишком длинное. Максимум {NameMaxLength} символов");
                }
            }

            if (!string.IsNullOrWhiteSpace(title) && title.Trim().Length > TitleMaxLength)
            {
                errors.Add($"Титул слишком длинный. Максимум {TitleMaxLength} символов");
            }

            if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > DescriptionMaxLength)
            {
                errors.Add($"Описание слишком длинное. Максимум {DescriptionMaxLength} символов");
            }

            return errors;
        }

        public static string? ValidateQuick(Hero hero)
        {
            var errors = Validate(hero);
            return errors.Count > 0 ? errors.First() : null;
        }
    }
}