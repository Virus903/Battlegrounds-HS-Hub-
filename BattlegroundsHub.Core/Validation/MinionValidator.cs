using BattlegroundsHubHS.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace BattlegroundsHubHS.Core.Validation
{
    public static class MinionValidator
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;
        public const int EffectMaxLength = 500;
        public const int ImageUrlMaxLength = 200;
        public const int MinTavernTier = 1;
        public const int MaxTavernTier = 6;
        public const int MinAttack = 0;
        public const int MinHealth = 1;
        public const int MaxAttack = 99;
        public const int MaxHealth = 99;

        public static List<string> Validate(Minion minion)
        {
            var errors = new List<string>();

            if (minion == null)
            {
                errors.Add("Миньон не может быть null");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(minion.Name))
            {
                errors.Add("Имя миньона обязательно");
            }
            else
            {
                var name = minion.Name.Trim();
                if (name.Length < NameMinLength)
                {
                    errors.Add($"Имя слишком короткое. Минимум {NameMinLength} символа");
                }
                if (name.Length > NameMaxLength)
                {
                    errors.Add($"Имя слишком длинное. Максимум {NameMaxLength} символов");
                }
            }

            if (minion.TavernTier < MinTavernTier || minion.TavernTier > MaxTavernTier)
            {
                errors.Add($"Уровень таверны должен быть от {MinTavernTier} до {MaxTavernTier}");
            }

            if (minion.Types == null || minion.Types.Count == 0)
            {
                errors.Add("Миньон должен иметь хотя бы один тип");
            }
            else
            {
                foreach (var type in minion.Types)
                {
                    if (string.IsNullOrWhiteSpace(type))
                    {
                        errors.Add("Тип не может быть пустой строкой");
                    }
                    else if (type.Trim().Length > 20)
                    {
                        errors.Add($"Тип '{type}' слишком длинный. Максимум 20 символов");
                    }
                }
            }

            if (minion.Attack < MinAttack)
            {
                errors.Add($"Атака не может быть меньше {MinAttack}");
            }
            if (minion.Attack > MaxAttack)
            {
                errors.Add($"Атака слишком большая. Максимум {MaxAttack}");
            }

            if (minion.Health < MinHealth)
            {
                errors.Add($"Здоровье не может быть меньше {MinHealth}");
            }
            if (minion.Health > MaxHealth)
            {
                errors.Add($"Здоровье слишком большое. Максимум {MaxHealth}");
            }

            if (!string.IsNullOrWhiteSpace(minion.Effect) && minion.Effect.Trim().Length > EffectMaxLength)
            {
                errors.Add($"Эффект слишком длинный. Максимум {EffectMaxLength} символов");
            }

            if (!string.IsNullOrWhiteSpace(minion.ImageUrl) && minion.ImageUrl.Trim().Length > ImageUrlMaxLength)
            {
                errors.Add($"URL изображения слишком длинный. Максимум {ImageUrlMaxLength} символов");
            }

            return errors;
        }

        public static List<string> ValidateForCreation(string name, int tavernTier, List<string> types,
            int attack, int health, string effect)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add("Имя миньона обязательно");
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

            if (tavernTier < MinTavernTier || tavernTier > MaxTavernTier)
            {
                errors.Add($"Уровень таверны должен быть от {MinTavernTier} до {MaxTavernTier}");
            }

            if (types == null || types.Count == 0)
            {
                errors.Add("Миньон должен иметь хотя бы один тип");
            }
            else
            {
                foreach (var type in types)
                {
                    if (string.IsNullOrWhiteSpace(type))
                    {
                        errors.Add("Тип не может быть пустой строкой");
                    }
                    else if (type.Trim().Length > 20)
                    {
                        errors.Add($"Тип '{type}' слишком длинный. Максимум 20 символов");
                    }
                }
            }

            if (attack < MinAttack)
            {
                errors.Add($"Атака не может быть меньше {MinAttack}");
            }
            if (attack > MaxAttack)
            {
                errors.Add($"Атака слишком большая. Максимум {MaxAttack}");
            }

            if (health < MinHealth)
            {
                errors.Add($"Здоровье не может быть меньше {MinHealth}");
            }
            if (health > MaxHealth)
            {
                errors.Add($"Здоровье слишком большое. Максимум {MaxHealth}");
            }

            if (!string.IsNullOrWhiteSpace(effect) && effect.Trim().Length > EffectMaxLength)
            {
                errors.Add($"Эффект слишком длинный. Максимум {EffectMaxLength} символов");
            }

            return errors;
        }

        public static string? ValidateQuick(Minion minion)
        {
            var errors = Validate(minion);
            return errors.Count > 0 ? errors.First() : null;
        }
    }
}