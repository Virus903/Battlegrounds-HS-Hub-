using BattlegroundsHub.Storage;
using BattlegroundsHubHS.Core.Models;
using BattlegroundsHubHS.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Battlegrounds_HS_Hub
{
    class Program
    {
        private const string Value = "1 - По возрастанию (А-Я}";
        private static JsonStorageService? _storage;

        static void Main(string[] args)
        {
            Console.WriteLine("=== BATTLEGROUNDS HUB - ДИАГНОСТИКА ===");
            Console.WriteLine($"Текущая директория: {Environment.CurrentDirectory}");
            Console.WriteLine($"Файл initial_data.json существует: {File.Exists("initial_data.json")}");
            Console.WriteLine($"Путь к initial_data.json: {Path.Combine(Environment.CurrentDirectory, "initial_data.json")}");

            Console.WriteLine("\nФайлы в текущей директории:");
            try
            {
                var files = Directory.GetFiles(Environment.CurrentDirectory, "*.json");
                foreach (var file in files)
                {
                    Console.WriteLine($"  - {Path.GetFileName(file)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файлов: {ex.Message}");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("=== BATTLEGROUNDS HUB ===");
            Console.WriteLine("Инициализация хранилища...");

            try
            {
                _storage = new JsonStorageService();
                Console.WriteLine($"Служба хранилища создана.");

                _storage.LoadData();

                Console.WriteLine($"\nДанные успешно загружены.");
                Console.WriteLine($"Загружено героев: {InMemoryStorage.Heroes.Count}, миньонов: {InMemoryStorage.Minions.Count}");

                if (InMemoryStorage.Heroes.Count > 0)
                {
                    Console.WriteLine("\nПримеры героев:");
                    foreach (var hero in InMemoryStorage.Heroes.Take(3))
                    {
                        Console.WriteLine($"  - {hero.Name} (ID: {hero.Id}, Рейтинг: {hero.Tier})");
                    }
                }

                if (InMemoryStorage.Minions.Count > 0)
                {
                    Console.WriteLine("\nПримеры миньонов:");
                    foreach (var minion in InMemoryStorage.Minions.Take(3))
                    {
                        Console.WriteLine($"  - {minion.Name} (Ур. {minion.TavernTier}, Типы: {minion.TypesDisplay})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка при загрузке данных: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine("Продолжаем с пустыми данными...");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("==============================================");
                Console.WriteLine("         BATTLEGROUNDS HUB - МЕНЮ");
                Console.WriteLine("==============================================");
                Console.WriteLine(" 1. Показать всех героев");
                Console.WriteLine(" 2. Показать всех миньонов");
                Console.WriteLine(" 3. Добавить нового героя");
                Console.WriteLine(" 4. Добавить нового миньона");
                Console.WriteLine(" 5. Найти героя по имени");
                Console.WriteLine("--- Основные операции ---");
                Console.WriteLine(" 6. Изменить рейтинг героя");
                Console.WriteLine(" 7. Изменить типы миньона");
                Console.WriteLine(" 8. Удалить героя");
                Console.WriteLine(" 9. Удалить миньона");
                Console.WriteLine("--- Полное редактирование ---");
                Console.WriteLine("10. Полное редактирование героя");
                Console.WriteLine("11. Полное редактирование миньона");
                Console.WriteLine("--- Поиск и фильтрация ---");
                Console.WriteLine("12. Поиск миньона по имени");
                Console.WriteLine("13. Фильтр героев по рейтингу");
                Console.WriteLine("14. Фильтр миньонов по уровню таверны");
                Console.WriteLine("15. Фильтр миньонов по типу (строка)");
                Console.WriteLine("16. Сортировка героев по имени");
                Console.WriteLine("17. Сортировка героев по рейтингу");
                Console.WriteLine("18. Сортировка миньонов по имени");
                Console.WriteLine("19. Сортировка миньонов по уровню таверны");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine(" 0. Выход");
                Console.WriteLine("20. ДИАГНОСТИКА (проверить данные)");
                Console.WriteLine("==============================================");
                Console.Write("Выберите действие (0-20): ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllHeroes(); break;
                    case "2": ShowAllMinions(); break;
                    case "3": AddNewHero(); break;
                    case "4": AddNewMinion(); break;
                    case "5": SearchHeroByName(); break;
                    case "6": UpdateHeroTier(); break;
                    case "7": UpdateMinionTypes(); break;
                    case "8": DeleteHero(); break;
                    case "9": DeleteMinion(); break;
                    case "10": EditHeroFull(); break;
                    case "11": EditMinionFull(); break;
                    case "12": SearchMinionByName(); break;
                    case "13": FilterHeroesByTierMenu(); break;
                    case "14": FilterMinionsByTavernTierMenu(); break;
                    case "15": FilterMinionsByTypeStringMenu(); break;
                    case "16": SortHeroesByNameMenu(); break;
                    case "17": SortHeroesByTierMenu(); break;
                    case "18": SortMinionsByNameMenu(); break;
                    case "19": SortMinionsByTavernTierMenu(); break;
                    case "20": RunDiagnostics(); break;
                    case "0":
                        exit = true;
                        Console.WriteLine("Выход из программы...");
                        SaveDataBeforeExit();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Нажмите любую клавишу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void SortMinionsByTavernTierMenu()
        {
            throw new NotImplementedException();
        }

        static void RunDiagnostics()
        {
            Console.Clear();
            Console.WriteLine("=== ДИАГНОСТИКА СИСТЕМЫ ===");

            Console.WriteLine("\n1. Проверка хранилища:");
            Console.WriteLine($"   Героев в памяти: {InMemoryStorage.Heroes.Count}");
            Console.WriteLine($"   Миньонов в памяти: {InMemoryStorage.Minions.Count}");

            if (InMemoryStorage.Heroes.Count > 0)
            {
                Console.WriteLine("\n   Первые 3 героя:");
                foreach (var hero in InMemoryStorage.Heroes.Take(3))
                {
                    Console.WriteLine($"     - ID: {hero.Id}, Имя: {hero.Name}, Рейтинг: {hero.Tier}");
                }
            }

            if (InMemoryStorage.Minions.Count > 0)
            {
                Console.WriteLine("\n   Первые 3 миньона:");
                foreach (var minion in InMemoryStorage.Minions.Take(3))
                {
                    Console.WriteLine($"     - ID: {minion.Id}, Имя: {minion.Name}, Уровень: {minion.TavernTier}");
                }
            }

            Console.WriteLine("\n2. Проверка файлов:");
            var currentDir = Environment.CurrentDirectory;
            Console.WriteLine($"   Текущая директория: {currentDir}");

            Console.WriteLine("\n3. Поиск файлов JSON:");
            try
            {
                var jsonFiles = Directory.GetFiles(currentDir, "*.json", SearchOption.AllDirectories);
                if (jsonFiles.Length > 0)
                {
                    foreach (var file in jsonFiles)
                    {
                        var fileInfo = new FileInfo(file);
                        Console.WriteLine($"   - {file} ({fileInfo.Length} байт)");

                        try
                        {
                            var content = File.ReadAllText(file);
                            Console.WriteLine($"     Содержит 'Heroes': {content.Contains("\"Heroes\"")}");
                            Console.WriteLine($"     Содержит 'Minions': {content.Contains("\"Minions\"")}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"     Ошибка чтения: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("   Файлы JSON не найдены!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   Ошибка поиска файлов: {ex.Message}");
            }

            WaitForContinue();
        }

        private static void WaitForContinue()
        {
            throw new NotImplementedException();
        }

        static void SaveDataBeforeExit()
        {
            try
            {
                if (_storage != null)
                {
                    _storage.SaveData();
                    Console.WriteLine("Данные сохранены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
            }
        }

        static void SaveDataAfterChange()
        {
            try
            {
                if (_storage != null)
                {
                    _storage.SaveData();
                    Console.WriteLine("Изменения сохранены в файл.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        static void ShowAllHeroes()
        {
            Console.Clear();
            Console.WriteLine("=== СПИСОК ГЕРОЕВ ===");

            var heroes = InMemoryStorage.Heroes;

            if (heroes.Count == 0)
            {
                Console.WriteLine("Героев пока нет.");
            }
            else
            {
                foreach (var hero in heroes)
                {
                    Console.WriteLine($"[ID: {hero.Id}] {hero.Name}");
                    Console.WriteLine($"   Титул: {hero.Title}");
                    Console.WriteLine($"   Рейтинг: {hero.Tier}");
                    Console.WriteLine($"   Описание: {hero.Description}");
                    Console.WriteLine("----------------------------------");
                }
            }

            Console.WriteLine($"Всего героев: {heroes.Count}");
            WaitForContinue();
        }

        static void ShowAllMinions()
        {
            Console.Clear();
            Console.WriteLine("=== СПИСОК МИНЬОНОВ ===");

            var minions = InMemoryStorage.Minions;

            if (minions.Count == 0)
            {
                Console.WriteLine("Миньонов пока нет.");
            }
            else
            {
                foreach (var minion in minions)
                {
                    Console.WriteLine($"[ID: {minion.Id}] {minion.Name}");
                    Console.WriteLine($"   Уровень таверны: {minion.TavernTier}");
                    Console.WriteLine($"   Типы: {minion.TypesDisplay}");
                    Console.WriteLine($"   Статы: {minion.Attack}/{minion.Health}");
                    Console.WriteLine($"   Эффект: {minion.Effect}");
                    Console.WriteLine("----------------------------------");
                }
            }

            Console.WriteLine($"Всего миньонов: {minions.Count}");
            WaitForContinue();
        }

        static void AddNewHero()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО ГЕРОЯ ===");

            try
            {
                Console.Write("Введите имя героя: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Ошибка: имя не может быть пустым!");
                    WaitForContinue();
                    return;
                }

                Console.Write("Введите титул (например, 'Пират', 'Демон'): ");
                string title = Console.ReadLine();

                Console.WriteLine("Выберите рейтинг:");
                Console.WriteLine("S - Самый сильный");
                Console.WriteLine("A - Очень сильный");
                Console.WriteLine("B - Средний");
                Console.WriteLine("C - Слабый");
                Console.WriteLine("D - Очень слабый");
                Console.WriteLine("F - Самый слабый");
                Console.Write("Рейтинг (S/A/B/C/D/F): ");
                string tierInput = Console.ReadLine().ToUpper();

                var newHero = new Hero
                {
                    Name = name,
                    Title = title,
                    Description = "Новый герой, описание можно добавить позже"
                };

                if (Enum.TryParse<HeroTier>(tierInput, out var tier))
                {
                    newHero.Tier = tier;
                }
                else
                {
                    newHero.Tier = HeroTier.B;
                }

                InMemoryStorage.AddHero(newHero);
                SaveDataAfterChange();

                Console.WriteLine($"\n✅ Герой '{name}' успешно добавлен! ID: {newHero.Id}");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        private static void ShowError(string v)
        {
            throw new NotImplementedException();
        }

        static void AddNewMinion()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО МИНЬОНА ===");

            try
            {
                Console.Write("Введите имя миньона: ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowError("Ошибка: имя не может быть пустым!");
                    WaitForContinue();
                    return;
                }

                Console.Write("Уровень таверны (1-6): ");
                if (!int.TryParse(Console.ReadLine(), out int tavernTier) || tavernTier < 1 || tavernTier > 6)
                {
                    ShowError("Уровень таверны должен быть от 1 до 6!");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine("Введите типы через запятую (например: Демон,Нага):");
                Console.Write("Типы: ");
                string typesInput = Console.ReadLine();

                var types = typesInput?.Split(',', ';')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList() ?? new List<string>();

                Console.Write("Атака: ");
                if (!int.TryParse(Console.ReadLine(), out int attack) || attack < 0)
                {
                    ShowError("Атака должна быть положительным числом!");
                    WaitForContinue();
                    return;
                }

                Console.Write("Здоровье: ");
                if (!int.TryParse(Console.ReadLine(), out int health) || health < 1)
                {
                    ShowError("Здоровье должно быть не менее 1!");
                    WaitForContinue();
                    return;
                }

                Console.Write("Эффект (способность): ");
                string effect = Console.ReadLine();

                var newMinion = new Minion
                {
                    Name = name,
                    TavernTier = tavernTier,
                    Types = types,
                    Attack = attack,
                    Health = health,
                    Effect = effect
                };

                InMemoryStorage.AddMinion(newMinion);
                SaveDataAfterChange();

                Console.WriteLine($"\n✅ Миньон '{name}' успешно добавлен! ID: {newMinion.Id}");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void SearchHeroByName()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК ГЕРОЯ ПО ИМЕНИ ===");

            Console.Write("Введите часть имени для поиска: ");
            string searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ShowError("Поисковый запрос не может быть пустым!");
                WaitForContinue();
                return;
            }

            var foundHeroes = InMemoryStorage.Heroes
                .Where(h => h.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (foundHeroes.Count == 0)
            {
                Console.WriteLine($"Героев с именем содержащим '{searchTerm}' не найдено.");
            }
            else
            {
                Console.WriteLine($"Найдено героев: {foundHeroes.Count}");
                foreach (var hero in foundHeroes)
                {
                    Console.WriteLine($"- {hero.Name} (ID: {hero.Id}, Рейтинг: {hero.Tier})");
                }
            }

            WaitForContinue();
        }

        static void UpdateHeroTier()
        {
            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ РЕЙТИНГА ГЕРОЯ ===");

            try
            {
                Console.WriteLine("Текущие герои:");
                foreach (var hero in InMemoryStorage.Heroes.Take(5))
                {
                    Console.WriteLine($"  ID: {hero.Id} - {hero.Name} (Рейтинг: {hero.Tier})");
                }

                Console.Write("\nВведите ID героя: ");
                if (!int.TryParse(Console.ReadLine(), out int heroId))
                {
                    ShowError("ID должен быть числом!");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine("Выберите новый рейтинг:");
                Console.WriteLine("S - Самый сильный");
                Console.WriteLine("A - Очень сильный");
                Console.WriteLine("B - Средний");
                Console.WriteLine("C - Слабый");
                Console.WriteLine("D - Очень слабый");
                Console.WriteLine("F - Самый слабый");
                Console.Write("Новый рейтинг (S/A/B/C/D/F): ");
                string tierInput = Console.ReadLine().ToUpper();

                if (!Enum.TryParse<HeroTier>(tierInput, out var newTier))
                {
                    ShowError("Неверный рейтинг!");
                    WaitForContinue();
                    return;
                }

                bool success = InMemoryStorage.UpdateHeroTier(heroId, newTier);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Рейтинг успешно обновлён!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void UpdateMinionTypes()
        {
            Console.Clear();
            Console.WriteLine("=== ИЗМЕНЕНИЕ ТИПОВ МИНЬОНА ===");

            try
            {
                Console.WriteLine("Текущие миньоны:");
                foreach (var Minion in InMemoryStorage.Minions.Take(5))
                {
                    Console.WriteLine($"  ID: {Minion.Id} - {Minion.Name} (Типы: {Minion.TypesDisplay})");
                }

                Console.Write("\nВведите ID миньона: ");
                if (!int.TryParse(Console.ReadLine(), out int minionId))
                {
                    ShowError("ID должен быть числом!");
                    WaitForContinue();
                    return;
                }

                var minion = InMemoryStorage.FindMinionById(minionId);
                if (minion == null)
                {
                    ShowError($"Миньон с ID {minionId} не найден!");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine($"Текущие типы: {minion.TypesDisplay}");
                Console.WriteLine("Введите новые типы через запятую (например: Демон,Нага):");
                Console.Write("Новые типы: ");
                string typesInput = Console.ReadLine();

                var newTypes = typesInput?.Split(',', ';')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList() ?? new List<string>();

                bool success = InMemoryStorage.UpdateMinionTypes(minionId, newTypes);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Типы успешно обновлены!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void DeleteHero()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ ГЕРОЯ ===");

            try
            {
                Console.WriteLine("Текущие герои:");
                foreach (var hero in InMemoryStorage.Heroes)
                {
                    Console.WriteLine($"  ID: {hero.Id} - {hero.Name} (Рейтинг: {hero.Tier})");
                }

                Console.Write("\nВведите ID героя для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int heroId))
                {
                    ShowError("ID должен быть числом!");
                    WaitForContinue();
                    return;
                }

                Console.Write($"Вы уверены, что хотите удалить героя с ID {heroId}? (д/н): ");
                string confirm = Console.ReadLine().ToLower();

                if (confirm != "д" && confirm != "y" && confirm != "да")
                {
                    Console.WriteLine("Удаление отменено.");
                    WaitForContinue();
                    return;
                }

                bool success = InMemoryStorage.DeleteHero(heroId);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Герой успешно удалён!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void DeleteMinion()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ МИНЬОНА ===");

            try
            {
                Console.WriteLine("Текущие миньоны:");
                foreach (var minion in InMemoryStorage.Minions)
                {
                    Console.WriteLine($"  ID: {minion.Id} - {minion.Name} (Типы: {minion.TypesDisplay})");
                }

                Console.Write("\nВведите ID миньона для удаления: ");
                if (!int.TryParse(Console.ReadLine(), out int minionId))
                {
                    ShowError("ID должен быть числом!");
                    WaitForContinue();
                    return;
                }

                Console.Write($"Вы уверены, что хотите удалить миньона с ID {minionId}? (д/н): ");
                string confirm = Console.ReadLine().ToLower();

                if (confirm != "д" && confirm != "y" && confirm != "да")
                {
                    Console.WriteLine("Удаление отменено.");
                    WaitForContinue();
                    return;
                }

                bool success = InMemoryStorage.DeleteMinion(minionId);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Миньон успешно удалён!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void EditHeroFull()
        {
            Console.Clear();
            Console.WriteLine("=== ПОЛНОЕ РЕДАКТИРОВАНИЕ ГЕРОЯ ===");

            try
            {
                Console.WriteLine("Текущие герои:");
                foreach (var hero in InMemoryStorage.Heroes.Take(5))
                {
                    Console.WriteLine($"  ID: {hero.Id} - {hero.Name} ({hero.Tier})");
                }

                Console.Write("\nВведите ID героя для редактирования: ");
                if (!int.TryParse(Console.ReadLine(), out int heroId))
                {
                    ShowError("ID должен быть числом!");
                    WaitForContinue();
                    return;
                }

                var heroToEdit = InMemoryStorage.FindHeroById(heroId);
                if (heroToEdit == null)
                {
                    ShowError($"Герой с ID {heroId} не найден!");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine("\nТекущие данные:");
                Console.WriteLine($"Имя: {heroToEdit.Name}");
                Console.WriteLine($"Титул: {heroToEdit.Title}");
                Console.WriteLine($"Рейтинг: {heroToEdit.Tier}");
                Console.WriteLine($"Описание: {heroToEdit.Description}");
                Console.WriteLine("\nВведите новые данные (нажмите Enter чтобы оставить текущее):");

                Console.Write($"Новое имя [{heroToEdit.Name}]: ");
                string newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName)) newName = heroToEdit.Name;

                Console.Write($"Новый титул [{heroToEdit.Title}]: ");
                string newTitle = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newTitle)) newTitle = heroToEdit.Title;

                Console.Write($"Новый рейтинг (S/A/B/C/D/F) [{heroToEdit.Tier}]: ");
                string tierInput = Console.ReadLine().ToUpper();
                HeroTier newTier = heroToEdit.Tier;
                if (!string.IsNullOrWhiteSpace(tierInput) &&
                    Enum.TryParse<HeroTier>(tierInput, out var parsedTier))
                {
                    newTier = parsedTier;
                }

                Console.Write($"Новое описание [{heroToEdit.Description}]: ");
                string newDescription = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newDescription)) newDescription = heroToEdit.Description;

                bool success = InMemoryStorage.UpdateHero(heroId, newName, newTitle, newTier, newDescription);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Герой успешно обновлён!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void EditMinionFull()
        {
            Console.Clear();
            Console.WriteLine("=== ПОЛНОЕ РЕДАКТИРОВАНИЕ МИНЬОНА ===");

            try
            {
                Console.WriteLine("Текущие миньоны:");
                foreach (var minion in InMemoryStorage.Minions.Take(5))
                {
                    Console.WriteLine($"  ID: {minion.Id} - {minion.Name} (Ур.{minion.TavernTier}, Типы: {minion.TypesDisplay})");
                }

                Console.Write("\nВведите ID миньона для редактирования: ");
                if (!int.TryParse(Console.ReadLine(), out int minionId))
                {
                    ShowError("ID должен быть числом!");
                    WaitForContinue();
                    return;
                }

                var minionToEdit = InMemoryStorage.FindMinionById(minionId);
                if (minionToEdit == null)
                {
                    ShowError($"Миньон с ID {minionId} не найден!");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine("\nТекущие данные:");
                Console.WriteLine($"Имя: {minionToEdit.Name}");
                Console.WriteLine($"Уровень таверны: {minionToEdit.TavernTier}");
                Console.WriteLine($"Типы: {minionToEdit.TypesDisplay}");
                Console.WriteLine($"Атака/Здоровье: {minionToEdit.Attack}/{minionToEdit.Health}");
                Console.WriteLine($"Эффект: {minionToEdit.Effect}");
                Console.WriteLine("\nВведите новые данные (нажмите Enter чтобы оставить текущее):");

                Console.Write($"Новое имя [{minionToEdit.Name}]: ");
                string newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName)) newName = minionToEdit.Name;

                Console.Write($"Новый уровень таверны (1-6) [{minionToEdit.TavernTier}]: ");
                string tavernInput = Console.ReadLine();
                int newTavernTier = minionToEdit.TavernTier;
                if (!string.IsNullOrWhiteSpace(tavernInput) &&
                    int.TryParse(tavernInput, out int parsedTavern) && parsedTavern >= 1 && parsedTavern <= 6)
                {
                    newTavernTier = parsedTavern;
                }

                Console.WriteLine($"Текущие типы: {minionToEdit.TypesDisplay}");
                Console.WriteLine("Введите новые типы через запятую (например: Демон,Нага):");
                Console.Write($"Новые типы [{minionToEdit.TypesDisplay}]: ");
                string typesInput = Console.ReadLine();
                List<string> newTypes = minionToEdit.Types?.ToList() ?? new List<string>();
                if (!string.IsNullOrWhiteSpace(typesInput))
                {
                    newTypes = typesInput.Split(',', ';')
                        .Select(t => t.Trim())
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList();
                }

                Console.Write($"Новая атака [{minionToEdit.Attack}]: ");
                string attackInput = Console.ReadLine();
                int newAttack = minionToEdit.Attack;
                if (!string.IsNullOrWhiteSpace(attackInput) &&
                    int.TryParse(attackInput, out int parsedAttack) && parsedAttack >= 0)
                {
                    newAttack = parsedAttack;
                }

                Console.Write($"Новое здоровье [{minionToEdit.Health}]: ");
                string healthInput = Console.ReadLine();
                int newHealth = minionToEdit.Health;
                if (!string.IsNullOrWhiteSpace(healthInput) &&
                    int.TryParse(healthInput, out int parsedHealth) && parsedHealth >= 1)
                {
                    newHealth = parsedHealth;
                }

                Console.Write($"Новый эффект [{minionToEdit.Effect}]: ");
                string newEffect = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newEffect)) newEffect = minionToEdit.Effect;

                bool success = InMemoryStorage.UpdateMinion(minionId, newName, newTavernTier,
                    newTypes, newAttack, newHealth, newEffect);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Миньон успешно обновлён!");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void SearchMinionByName()
        {
            Console.Clear();
            Console.WriteLine("=== ПОИСК МИНЬОНА ПО ИМЕНИ ===");

            Console.Write("Введите часть имени для поиска: ");
            string searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ShowError("Поисковый запрос не может быть пустым!");
                WaitForContinue();
                return;
            }

            var foundMinions = InMemoryStorage.FindMinionsByName(searchTerm);

            if (foundMinions.Count == 0)
            {
                Console.WriteLine($"Миньонов с именем содержащим '{searchTerm}' не найдено.");
            }
            else
            {
                Console.WriteLine($"Найдено миньонов: {foundMinions.Count}\n");
                foreach (var minion in foundMinions)
                {
                    Console.WriteLine($"[ID: {minion.Id}] {minion.Name}");
                    Console.WriteLine($"   Уровень: {minion.TavernTier}, Типы: {minion.TypesDisplay}");
                    Console.WriteLine($"   Статы: {minion.Attack}/{minion.Health}");
                    Console.WriteLine($"   Эффект: {minion.Effect}");
                    Console.WriteLine("----------------------------------");
                }
            }

            WaitForContinue();
        }

        static void FilterHeroesByTierMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ФИЛЬТР ГЕРОЕВ ПО РЕЙТИНГУ ===");

            Console.WriteLine("Выберите рейтинг для фильтрации:");
            Console.WriteLine("1 - S (Самый сильный)");
            Console.WriteLine("2 - A (Очень сильный)");
            Console.WriteLine("3 - B (Средний)");
            Console.WriteLine("4 - C (Слабый)");
            Console.WriteLine("5 - D (Очень слабый)");
            Console.WriteLine("6 - F (Самый слабый)");
            Console.Write("Ваш выбор (1-6): ");

            HeroTier selectedTier = Console.ReadLine() switch
            {
                "1" => HeroTier.S,
                "2" => HeroTier.A,
                "3" => HeroTier.B,
                "4" => HeroTier.C,
                "5" => HeroTier.D,
                "6" => HeroTier.F,
                _ => HeroTier.S
            };

            var filteredHeroes = InMemoryStorage.FilterHeroesByTier(selectedTier);

            Console.WriteLine($"\nГероев с рейтингом {selectedTier}: {filteredHeroes.Count}\n");
            DisplayHeroesList(filteredHeroes);

            WaitForContinue();
        }

        private static void DisplayHeroesList(List<Hero> filteredHeroes)
        {
            throw new NotImplementedException();
        }

        static void FilterMinionsByTavernTierMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ФИЛЬТР МИНЬОНОВ ПО УРОВНЮ ТАВЕРНЫ ===");

            Console.Write("Введите уровень таверны (1-6): ");
            if (!int.TryParse(Console.ReadLine(), out int tier) || tier < 1 || tier > 6)
            {
                ShowError("Уровень таверны должен быть от 1 до 6!");
                WaitForContinue();
                return;
            }

            var filteredMinions = InMemoryStorage.FilterMinionsByTavernTier(tier);

            Console.WriteLine($"\nМиньонов уровня {tier}: {filteredMinions.Count}\n");
            DisplayMinionsList(filteredMinions);

            WaitForContinue();
        }

        private static void DisplayMinionsList(List<Minion> filteredMinions)
        {
            throw new NotImplementedException();
        }

        static void FilterMinionsByTypeStringMenu()
        {
            Console.Clear();
            Console.WriteLine("=== ФИЛЬТР МИНЬОНОВ ПО ТИПУ (СТРОКА) ===");

            Console.Write("Введите тип для фильтрации (например: Демон): ");
            string type = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(type))
            {
                ShowError("Тип не может быть пустым!");
                WaitForContinue();
                return;
            }

            var filteredMinions = InMemoryStorage.FilterMinionsByType(type);

            Console.WriteLine($"\nМиньонов типа '{type}': {filteredMinions.Count}\n");
            DisplayMinionsList(filteredMinions);

            WaitForContinue();
        }

        static void SortHeroesByNameMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОРТИРОВКА ГЕРОЕВ ПО ИМЕНИ ===");

            Console.WriteLine("1 - По возрастанию (А-Я)");
            Console.WriteLine("2 - По убыванию (Я-А)");
            Console.Write("Ваш выбор: ");

            bool ascending = Console.ReadLine() == "1";
            var sortedHeroes = InMemoryStorage.GetHeroesSortedByName(ascending);

            Console.WriteLine($"\nГерои отсортированы по имени ({(ascending ? "А-Я" : "Я-А")}):\n");
            DisplayHeroesList(sortedHeroes);

            WaitForContinue();
        }

        static void SortHeroesByTierMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОРТИРОВКА ГЕРОЕВ ПО РЕЙТИНГУ ===");

            Console.WriteLine("1 - От лучшего к худшему (S → F)");
            Console.WriteLine("2 - От худшего к лучшему (F → S)");
            Console.Write("Ваш выбор: ");

            bool descending = Console.ReadLine() == "1";
            var sortedHeroes = InMemoryStorage.GetHeroesSortedByTier(descending);

            Console.WriteLine($"\nГерои отсортированы по рейтингу ({(descending ? "S→F" : "F→S")}):\n");
            DisplayHeroesList(sortedHeroes);

            WaitForContinue();
        }

        static void SortMinionsByNameMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОРТИРОВКА МИНЬОНОВ ПО ИМЕНИ ===");
            Console.WriteLine(Value);
        }
    }
}