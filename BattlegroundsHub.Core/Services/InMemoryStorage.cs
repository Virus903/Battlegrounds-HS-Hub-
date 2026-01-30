using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using BattlegroundsHubHS.Core.Models;
using System;

namespace BattlegroundsHubHS.Core.Services
{
    public static class InMemoryStorage
    {
        public static List<Hero> Heroes { get; set; } = new();
        public static List<Minion> Minions { get; set; } = new();

        private static int _nextHeroId = 1;
        private static int _nextMinionId = 1;

        static InMemoryStorage()
        {
            InitializeRealData();
            UpdateNextIds();
        }

        private static void InitializeRealData()
        {
            Heroes.AddRange(new List<Hero>
            {
                new Hero { Id = 1, Name = "А.Ф.Ка", Title = "Герой", Tier = HeroTier.B, Description = "Боевой клич: Открывает по карте с каждого уровня таверны. Выберите одну." },
                new Hero { Id = 2, Name = "Ал'акир", Title = "Герой", Tier = HeroTier.B, Description = "Пока ты не улучшил таверну, твои слуги стоят на 1 золотой дешевле." },
                new Hero { Id = 3, Name = "Алекстраза", Title = "Герой", Tier = HeroTier.A, Description = "В начале боя призывает двух 1/1 драконов." },
                new Hero { Id = 4, Name = "Аранна Звездочет", Title = "Герой", Tier = HeroTier.S, Description = "Когда ты переходишь на новый уровень таверны, навсегда получаешь +1 к золоту за обновление." },
                new Hero { Id = 5, Name = "Артанис", Title = "Герой", Tier = HeroTier.A, Description = "В конце хода твой левый и правый слуга получают +1/+1." },
                new Hero { Id = 6, Name = "Ахалаймахалай", Title = "Герой", Tier = HeroTier.C, Description = "Раз в ход: твой первый слуга навсегда получает +1/+1." },
                new Hero { Id = 7, Name = "Безымянный", Title = "Герой", Tier = HeroTier.A, Description = "Ты можешь выбирать награды из трёх чаш. Выбери одну." },
                new Hero { Id = 8, Name = "Билетикус", Title = "Герой", Tier = HeroTier.S, Description = "Когда ты покупает слугу, тот навсегда получает +1/+2." },
                new Hero { Id = 9, Name = "Бру'кан", Title = "Герой", Tier = HeroTier.B, Description = "В конце хода случайный твой слуга получает +1/+1." },
                new Hero { Id = 10, Name = "Бурекрыл", Title = "Герой", Tier = HeroTier.C, Description = "Боевой клич: Даёт +1/+1 трем твоим слугам." },
                new Hero { Id = 11, Name = "Вандар Грозовая Вершина", Title = "Герой", Tier = HeroTier.A, Description = "В конце хода самый левый слуга навсегда получает +2/+2." },
                new Hero { Id = 12, Name = "Варден Луч Рассвета", Title = "Герой", Tier = HeroTier.B, Description = "Боевой клич: Слуга в твоей таверне получает +1/+2." },
                new Hero { Id = 13, Name = "Вестник смерти Черношип", Title = "Герой", Tier = HeroTier.A, Description = "После того как ты улучшаешь таверну, получаешь 1 золотой." },
                new Hero { Id = 14, Name = "Вечная Токи", Title = "Герой", Tier = HeroTier.C, Description = "В конце хода твой самый правый слуга получает +1/+1." },
                new Hero { Id = 15, Name = "Воевода Саурфанг", Title = "Герой", Tier = HeroTier.B, Description = "Боевой клич: Даёт +2/+2 твоему слуге." },
                new Hero { Id = 16, Name = "Вол'джин", Title = "Герой", Tier = HeroTier.A, Description = "Раз в ход: Можешь поменять атаку и здоровье слуги." },
                new Hero { Id = 17, Name = "Галакронд", Title = "Герой", Tier = HeroTier.S, Description = "Когда ты покупаешь дракона, получаешь 1 золотой." },
                new Hero { Id = 18, Name = "Галл", Title = "Герой", Tier = HeroTier.B, Description = "Раз в ход: Слуга в таверне получает +2/+1." },
                new Hero { Id = 19, Name = "Гафф Рунический Тотем", Title = "Герой", Tier = HeroTier.C, Description = "В конце хода случайный твой слуга получает +1/+2." },
                new Hero { Id = 20, Name = "Грибомант Флургл", Title = "Герой", Tier = HeroTier.A, Description = "Когда ты покупаешь мурлока, получаешь 1 золотой." }
            });

            Minions.AddRange(new List<Minion>
            {
                new Minion { Id = 1, Name = "Бликостраж", TavernTier = 1, Types = new List<string> { "Дракон" }, Attack = 1, Health = 4, Effect = "Боевой раж: получает +2 к атаке." },
                new Minion { Id = 2, Name = "Бродячий кот", TavernTier = 1, Types = new List<string> { "Зверь" }, Attack = 1, Health = 1, Effect = "Боевой клич: призывает кошку 1/1." },
                new Minion { Id = 3, Name = "Вертлявый разведчик", TavernTier = 1, Types = new List<string> { "Мурлок" }, Attack = 3, Health = 3, Effect = "Начало боя: если это существо у вас в руке, призывает его копию." },
                new Minion { Id = 4, Name = "Восставший всадник", TavernTier = 1, Types = new List<string> { "Нежить" }, Attack = 2, Health = 1, Effect = "Провокация Перерождение" },
                new Minion { Id = 5, Name = "Гадкий дракончик", TavernTier = 1, Types = new List<string> { "Дракон" }, Attack = 2, Health = 1, Effect = "Начало боя: получает характеристики, равные уровню вашей таверны." },
                new Minion { Id = 6, Name = "Глубоководный удильщик", TavernTier = 1, Types = new List<string> { "Нага" }, Attack = 2, Health = 2, Effect = "Чародейство: выбранное существо получает +2 к здоровью и «Провокация» до следующего хода." },
                new Minion { Id = 7, Name = "Гнолл из стаи Гнилошкуров", TavernTier = 1, Types = new List<string> { "Нежить" }, Attack = 1, Health = 4, Effect = "Получает +1 к атаке за каждое ваше существо, погибшее в течение боя." },
                new Minion { Id = 8, Name = "Дюнный силач", TavernTier = 1, Types = new List<string> { "Элементаль" }, Attack = 3, Health = 2, Effect = "Боевой клич: элементали в таверне получают +1/+1 до конца матча." },
                new Minion { Id = 9, Name = "Загорающий свинобраз", TavernTier = 1, Types = new List<string> { "Свинобраз" }, Attack = 2, Health = 3, Effect = "Когда вы продаете это существо, вы получаете 2 кровавых самоцвета." },
                new Minion { Id = 10, Name = "Зловещая пророчица", TavernTier = 1, Types = new List<string> { "Демон", "Нага" }, Attack = 2, Health = 1, Effect = "Боевой клич: следующее заклинание из таверны стоит на (1) меньше." },
                new Minion { Id = 11, Name = "Золотой призер", TavernTier = 1, Types = new List<string> { "Пират" }, Attack = 1, Health = 1, Effect = "«Божественный щит» Боевой клич: делает это существо золотым." },
                new Minion { Id = 12, Name = "Клыкастый походник", TavernTier = 1, Types = new List<string> { "Свинобраз" }, Attack = 2, Health = 3, Effect = "Боевой раж: применяет к себе кровавый самоцвет." },
                new Minion { Id = 13, Name = "Манапард", TavernTier = 1, Types = new List<string> { "Зверь" }, Attack = 4, Health = 1, Effect = "Предсмертный хрип: призывает двух детенышей 0/1 с «Провокацией»." },
                new Minion { Id = 14, Name = "Мерзкий бес", TavernTier = 1, Types = new List<string> { "Демон" }, Attack = 1, Health = 1, Effect = "Предсмертный хрип: призывает двух бесов 1/1." },
                new Minion { Id = 15, Name = "Музыкант Южных Морей", TavernTier = 1, Types = new List<string> { "Пират" }, Attack = 3, Health = 1, Effect = "Боевой клич: вы получаете 1 золотой на следующем ходу." },
                new Minion { Id = 16, Name = "Алый Череп", TavernTier = 2, Types = new List<string> { "Нежить" }, Attack = 2, Health = 1, Effect = "Перерождение. Предсмертный хрип: ваше существо-нежить получает +1/+2." },
                new Minion { Id = 17, Name = "Бес на бесе", TavernTier = 2, Types = new List<string> { "Демон" }, Attack = 4, Health = 1, Effect = "Предсмертный хрип: призывает беса 4/1." },
                new Minion { Id = 18, Name = "Бродячий сатир", TavernTier = 2, Types = new List<string> { "Демон" }, Attack = 1, Health = 6, Effect = "После того как ваш демон наносит урон, получает +1 к атаке." },
                new Minion { Id = 19, Name = "Вечный рыцарь", TavernTier = 2, Types = new List<string> { "Нежить" }, Attack = 5, Health = 1, Effect = "Получает +1/+1 за каждого вашего вечного рыцаря, погибшего в течение матча (где бы он ни был)." },
                new Minion { Id = 20, Name = "Вокалибри", TavernTier = 2, Types = new List<string> { "Зверь" }, Attack = 1, Health = 4, Effect = "Начало боя: в этом бою все ваши звери получают +1 к атаке." },
                new Minion { Id = 21, Name = "Голован", TavernTier = 2, Types = new List<string> { "Мурлок" }, Attack = 2, Health = 2, Effect = "Когда вы продаете это существо, вы получаете случайного мурлока." },
                new Minion { Id = 22, Name = "Пиратский капитан", TavernTier = 5, Types = new List<string> { "Пират" }, Attack = 5, Health = 4, Effect = "Даёт +1/+1 всем пиратам в бою." },
                new Minion { Id = 23, Name = "Гончая Термопласта", TavernTier = 4, Types = new List<string> { "Механизм" }, Attack = 4, Health = 4, Effect = "В конце хода даёт магнит соседним мехам." },
                new Minion { Id = 24, Name = "Свирепый голем", TavernTier = 6, Types = new List<string> { "Механизм" }, Attack = 6, Health = 8, Effect = "Божественный щит. В конце хода даёт +2/+2 соседним механизмам." },
                new Minion { Id = 25, Name = "Лорд Вак'ри", TavernTier = 6, Types = new List<string> { "Демон" }, Attack = 7, Health = 7, Effect = "Начало боя: призывает копию случайного вашего демона." },
                new Minion { Id = 26, Name = "Калисея", TavernTier = 6, Types = new List<string> { "Нага" }, Attack = 8, Health = 6, Effect = "Чародейство: получает +3/+3 до конца боя." },
                new Minion { Id = 27, Name = "Магматический бастион", TavernTier = 2, Types = new List<string> { "Элементаль" }, Attack = 2, Health = 4, Effect = "В конце хода соседние элементали получают +1/+1." },
                new Minion { Id = 28, Name = "Гниющий зомби", TavernTier = 2, Types = new List<string> { "Нежить" }, Attack = 2, Health = 3, Effect = "Возрождается с 1 здоровьем после смерти." },
                new Minion { Id = 29, Name = "Тонкий торговец", TavernTier = 3, Types = new List<string> { "Пират" }, Attack = 4, Health = 3, Effect = "Когда вы продаете этого пирата, получаете 2 золотых." },
                new Minion { Id = 30, Name = "Часовой механизм", TavernTier = 3, Types = new List<string> { "Механизм" }, Attack = 3, Health = 6, Effect = "Божественный щит. В конце хода даёт +1/+1 соседнему механизму." }
            });
        }

        private static void UpdateNextIds()
        {
            _nextHeroId = Heroes.Count > 0 ? Heroes.Max(h => h.Id) + 1 : 1;
            _nextMinionId = Minions.Count > 0 ? Minions.Max(m => m.Id) + 1 : 1;
        }

        public static void AddHero(Hero hero)
        {
            hero.Id = _nextHeroId++;
            Heroes.Add(hero);
        }

        public static void AddMinion(Minion minion)
        {
            if (minion.Types == null || minion.Types.Count == 0)
            {
                minion.Types = new List<string> { "Нейтральный" };
            }

            minion.Id = _nextMinionId++;
            Minions.Add(minion);
        }

        public static Hero? FindHeroById(int id) => Heroes.FirstOrDefault(h => h.Id == id);
        public static Minion? FindMinionById(int id) => Minions.FirstOrDefault(m => m.Id == id);

        public static bool UpdateHeroTier(int heroId, HeroTier newTier)
        {
            var hero = FindHeroById(heroId);
            if (hero == null) return false;

            hero.Tier = newTier;
            return true;
        }

        public static bool UpdateMinionTypes(int minionId, List<string> newTypes)
        {
            var minion = FindMinionById(minionId);
            if (minion == null) return false;

            minion.Types = newTypes ?? new List<string>();
            return true;
        }

        public static bool DeleteHero(int heroId)
        {
            var hero = FindHeroById(heroId);
            if (hero == null) return false;

            Heroes.Remove(hero);
            return true;
        }

        public static bool DeleteMinion(int minionId)
        {
            var minion = FindMinionById(minionId);
            if (minion == null) return false;

            Minions.Remove(minion);
            return true;
        }

        public static bool UpdateHero(int heroId, string newName, string newTitle,
                                      HeroTier newTier, string newDescription)
        {
            var hero = FindHeroById(heroId);
            if (hero == null) return false;

            if (string.IsNullOrWhiteSpace(newName)) return false;

            hero.Name = newName;
            hero.Title = newTitle;
            hero.Tier = newTier;
            hero.Description = newDescription;
            return true;
        }

        public static bool UpdateMinion(int minionId, string newName, int newTavernTier,
                                        List<string> newTypes, int newAttack, int newHealth,
                                        string newEffect)
        {
            var minion = FindMinionById(minionId);
            if (minion == null) return false;

            if (string.IsNullOrWhiteSpace(newName)) return false;
            if (newTavernTier < 1 || newTavernTier > 6) return false;
            if (newAttack < 0) return false;
            if (newHealth < 1) return false;

            minion.Name = newName;
            minion.TavernTier = newTavernTier;
            minion.Types = newTypes ?? new List<string>();
            minion.Attack = newAttack;
            minion.Health = newHealth;
            minion.Effect = newEffect;
            return true;
        }

        public static Hero? FindHeroByName(string name)
        {
            return Heroes.FirstOrDefault(h =>
                h.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public static List<Minion> FindMinionsByName(string name)
        {
            return Minions
                .Where(m => m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public static List<Hero> FilterHeroesByTier(HeroTier tier)
        {
            return Heroes.Where(h => h.Tier == tier).ToList();
        }

        public static List<Minion> FilterMinionsByTavernTier(int tier)
        {
            return Minions.Where(m => m.TavernTier == tier).ToList();
        }

        public static List<Minion> FilterMinionsByType(string type)
        {
            return Minions
                .Where(m => m.Types != null && m.Types.Any(t =>
                    t.Contains(type, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public static List<Hero> GetHeroesSortedByName(bool ascending = true)
        {
            return ascending
                ? Heroes.OrderBy(h => h.Name).ToList()
                : Heroes.OrderByDescending(h => h.Name).ToList();
        }

        public static List<Hero> GetHeroesSortedByTier(bool descending = true)
        {
            var tierOrder = new Dictionary<HeroTier, int>
            {
                [HeroTier.S] = 1,
                [HeroTier.A] = 2,
                [HeroTier.B] = 3,
                [HeroTier.C] = 4,
                [HeroTier.D] = 5,
                [HeroTier.F] = 6
            };

            return descending
                ? Heroes.OrderBy(h => tierOrder[h.Tier]).ToList()
                : Heroes.OrderByDescending(h => tierOrder[h.Tier]).ToList();
        }

        public static List<Minion> GetMinionsSortedByName(bool ascending = true)
        {
            return ascending
                ? Minions.OrderBy(m => m.Name).ToList()
                : Minions.OrderByDescending(m => m.Name).ToList();
        }

        public static List<Minion> GetMinionsSortedByTavernTier(bool descending = true)
        {
            return descending
                ? Minions.OrderByDescending(m => m.TavernTier).ToList()
                : Minions.OrderBy(m => m.TavernTier).ToList();
        }

        public static void ClearAll()
        {
            Heroes.Clear();
            Minions.Clear();
            _nextHeroId = 1;
            _nextMinionId = 1;
        }

        public static void LoadFromFile(List<Hero> heroes, List<Minion> minions)
        {
            ClearAll();

            if (heroes != null && heroes.Count > 0)
            {
                Heroes.AddRange(heroes);
                _nextHeroId = heroes.Max(h => h.Id) + 1;
            }

            if (minions != null && minions.Count > 0)
            {
                Minions.AddRange(minions);
                _nextMinionId = minions.Max(m => m.Id) + 1;
            }

            Console.WriteLine($"Загружено {Heroes.Count} героев и {Minions.Count} миньонов");
        }
    }
}