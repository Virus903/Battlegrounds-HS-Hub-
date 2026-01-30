using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BattlegroundsHubHS.Core.Models
{
    public class Minion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя миньона обязательно")]
        public string Name { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Уровень таверны должен быть от 1 до 6")]
        public int TavernTier { get; set; } = 1;

        public List<string> Types { get; set; } = new List<string>();

        public int Attack { get; set; } = 1;
        public int Health { get; set; } = 1;

        [StringLength(500)]
        public string Effect { get; set; } = string.Empty;

        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public string TypesDisplay => Types != null && Types.Count > 0
            ? string.Join(", ", Types)
            : "Нейтральный";
    }
}