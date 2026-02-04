using BattlegroundsHub.Storage;
using BattlegroundsHub.Storage.Services;
using BattlegroundsHubHS.Core.Models;
using BattlegroundsHubHS.Core.Services;
using BattlegroundsHubHS.Core.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Battlegrounds_HS_Hub
{
    class Program
    {
        private static JsonStorageService? _storage;
        private static string _backupsFolder = string.Empty;
        private static string _exportsFolder = string.Empty;

        static void Main(string[] args)
        {
            Console.WriteLine("=== BATTLEGROUNDS HUB ===");
            Console.WriteLine("Импорт/Экспорт/Backup");
            Console.WriteLine("Валидация данных Hero/Minion");
            Console.WriteLine("\nИнициализация хранилища...");

            try
            {
                _storage = new JsonStorageService();

                var appFolder = AppContext.BaseDirectory;
                _backupsFolder = Path.Combine(appFolder, "backups");
                _exportsFolder = Path.Combine(appFolder, "exports");
                Directory.CreateDirectory(_backupsFolder);
                Directory.CreateDirectory(_exportsFolder);

                Console.WriteLine($"Бэкапы: {_backupsFolder}");
                Console.WriteLine($"Экспорты: {_exportsFolder}");

                _storage.LoadData();

                Console.WriteLine($"\n✅ Данные успешно загружены.");
                Console.WriteLine($"   Героев: {InMemoryStorage.Heroes.Count}");
                Console.WriteLine($"   Миньонов: {InMemoryStorage.Minions.Count}");

                Console.WriteLine("\n=== ПРАВИЛА ВАЛИДАЦИИ ===");
                Console.WriteLine($"Герои:");
                Console.WriteLine($"  • Имя: {HeroValidator.NameMinLength}-{HeroValidator.NameMaxLength} символов");
                Console.WriteLine($"  • Титул: до {HeroValidator.TitleMaxLength} символов");
                Console.WriteLine($"  • Описание: до {HeroValidator.DescriptionMaxLength} символов");
                Console.WriteLine($"\nМиньоны:");
                Console.WriteLine($"  • Имя: {MinionValidator.NameMinLength}-{MinionValidator.NameMaxLength} символов");
                Console.WriteLine($"  • Уровень таверны: {MinionValidator.MinTavernTier}-{MinionValidator.MaxTavernTier}");
                Console.WriteLine($"  • Атака: {MinionValidator.MinAttack}-{MinionValidator.MaxAttack}");
                Console.WriteLine($"  • Здоровье: {MinionValidator.MinHealth}-{MinionValidator.MaxHealth}");
                Console.WriteLine($"  • Эффект: до {MinionValidator.EffectMaxLength} символов");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка при загрузке данных: {ex.Message}");
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
                Console.WriteLine("           (Backup/Export/Import)");
                Console.WriteLine("           (Валидация данных)");
                Console.WriteLine("==============================================");
                Console.WriteLine(" 1. Показать всех героев");
                Console.WriteLine(" 2. Показать всех миньонов");
                Console.WriteLine(" 3. Добавить нового героя (с валидацией)");
                Console.WriteLine(" 4. Добавить нового миньона (с валидацией)");
                Console.WriteLine(" 5. Найти героя по имени");
                Console.WriteLine("--- Основные операции ---");
                Console.WriteLine(" 6. Изменить рейтинг героя");
                Console.WriteLine(" 7. Изменить типы миньона");
                Console.WriteLine(" 8. Удалить героя");
                Console.WriteLine(" 9. Удалить миньона");
                Console.WriteLine("--- Полное редактирование ---");
                Console.WriteLine("10. Полное редактирование героя (с валидацией)");
                Console.WriteLine("11. Полное редактирование миньона (с валидацией)");
                Console.WriteLine("--- Поиск и фильтрация ---");
                Console.WriteLine("12. Поиск миньона по имени");
                Console.WriteLine("13. Фильтр героев по рейтингу");
                Console.WriteLine("14. Фильтр миньонов по уровню таверны");
                Console.WriteLine("15. Фильтр миньонов по типу (строка)");
                Console.WriteLine("16. Сортировка героев по имени");
                Console.WriteLine("17. Сортировка героев по рейтингу");
                Console.WriteLine("18. Сортировка миньонов по имени");
                Console.WriteLine("19. Сортировка миньонов по уровню таверны");
                Console.WriteLine("--- Импорт/Экспорт/Backup ---");
                Console.WriteLine("20. Создать резервную копию (Backup)");
                Console.WriteLine("21. Экспорт в файл (Export)");
                Console.WriteLine("22. Импорт из файла (Import с валидацией)");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine(" 0. Выход");
                Console.WriteLine("99. ДИАГНОСТИКА (проверить данные)");
                Console.WriteLine("==============================================");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowAllHeroes(); break;
                    case "2": ShowAllMinions(); break;
                    case "3": AddNewHeroWithValidation(); break;
                    case "4": AddNewMinionWithValidation(); break;
                    case "5": SearchHeroByName(); break;
                    case "6": UpdateHeroTier(); break;
                    case "7": UpdateMinionTypes(); break;
                    case "8": DeleteHero(); break;
                    case "9": DeleteMinion(); break;
                    case "10": EditHeroFullWithValidation(); break;
                    case "11": EditMinionFullWithValidation(); break;
                    case "12": SearchMinionByName(); break;
                    case "13": FilterHeroesByTierMenu(); break;
                    case "14": FilterMinionsByTavernTierMenu(); break;
                    case "15": FilterMinionsByTypeStringMenu(); break;
                    case "16": SortHeroesByNameMenu(); break;
                    case "17": SortHeroesByTierMenu(); break;
                    case "18": SortMinionsByNameMenu(); break;
                    case "19": SortMinionsByTavernTierMenu(); break;
                    case "20": CreateBackup(); break;
                    case "21": CreateExport(); break;
                    case "22": ImportFromFileWithValidation(); break;
                    case "99": RunDiagnostics(); break;
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

        static void AddNewHeroWithValidation()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО ГЕРОЯ (с валидацией) ===");
            Console.WriteLine($"Правила: Имя {HeroValidator.NameMinLength}-{HeroValidator.NameMaxLength} симв.");

            try
            {
                Console.Write("Введите имя героя: ");
                string name = Console.ReadLine();

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

                HeroTier tier = HeroTier.B;
                if (!string.IsNullOrWhiteSpace(tierInput) && Enum.TryParse<HeroTier>(tierInput, out var parsedTier))
                {
                    tier = parsedTier;
                }

                Console.Write("Введите описание: ");
                string description = Console.ReadLine();

                var validationErrors = HeroValidator.ValidateForCreation(name, title, description);

                if (validationErrors.Count > 0)
                {
                    Console.WriteLine("\n❌ Ошибки валидации:");
                    foreach (var error in validationErrors)
                    {
                        ShowError($"  • {error}");
                    }
                    WaitForContinue();
                    return;
                }

                var newHero = new Hero
                {
                    Name = name.Trim(),
                    Title = title?.Trim() ?? "",
                    Tier = tier,
                    Description = description?.Trim() ?? "Новый герой"
                };

                var (success, errors) = InMemoryStorage.AddHeroWithValidation(newHero);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine($"\n✅ Герой '{name}' успешно добавлен! ID: {newHero.Id}");
                }
                else
                {
                    Console.WriteLine("\n❌ Ошибки при добавлении:");
                    foreach (var error in errors)
                    {
                        ShowError($"  • {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void AddNewMinionWithValidation()
        {
            Console.Clear();
            Console.WriteLine("=== ДОБАВЛЕНИЕ НОВОГО МИНЬОНА (с валидацией) ===");
            Console.WriteLine($"Правила: Имя {MinionValidator.NameMinLength}-{MinionValidator.NameMaxLength} симв.");
            Console.WriteLine($"        Уровень таверны: {MinionValidator.MinTavernTier}-{MinionValidator.MaxTavernTier}");

            try
            {
                Console.Write("Введите имя миньона: ");
                string name = Console.ReadLine();

                Console.Write("Уровень таверны (1-6): ");
                if (!int.TryParse(Console.ReadLine(), out int tavernTier))
                {
                    ShowError("Уровень таверны должен быть числом!");
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
                if (!int.TryParse(Console.ReadLine(), out int attack))
                {
                    ShowError("Атака должна быть числом!");
                    WaitForContinue();
                    return;
                }

                Console.Write("Здоровье: ");
                if (!int.TryParse(Console.ReadLine(), out int health))
                {
                    ShowError("Здоровье должно быть числом!");
                    WaitForContinue();
                    return;
                }

                Console.Write("Эффект (способность): ");
                string effect = Console.ReadLine();

                var validationErrors = MinionValidator.ValidateForCreation(name, tavernTier, types, attack, health, effect);

                if (validationErrors.Count > 0)
                {
                    Console.WriteLine("\n❌ Ошибки валидации:");
                    foreach (var error in validationErrors)
                    {
                        ShowError($"  • {error}");
                    }
                    WaitForContinue();
                    return;
                }

                var newMinion = new Minion
                {
                    Name = name.Trim(),
                    TavernTier = tavernTier,
                    Types = types,
                    Attack = attack,
                    Health = health,
                    Effect = effect?.Trim() ?? ""
                };

                var (success, errors) = InMemoryStorage.AddMinionWithValidation(newMinion);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine($"\n✅ Миньон '{name}' успешно добавлен! ID: {newMinion.Id}");
                }
                else
                {
                    Console.WriteLine("\n❌ Ошибки при добавлении:");
                    foreach (var error in errors)
                    {
                        ShowError($"  • {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void EditHeroFullWithValidation()
        {
            Console.Clear();
            Console.WriteLine("=== ПОЛНОЕ РЕДАКТИРОВАНИЕ ГЕРОЯ (с валидацией) ===");

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

                var (success, errors) = InMemoryStorage.UpdateHeroWithValidation(
                    heroId, newName, newTitle, newTier, newDescription);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Герой успешно обновлён!");
                }
                else
                {
                    Console.WriteLine("\n❌ Ошибки валидации:");
                    foreach (var error in errors)
                    {
                        ShowError($"  • {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void EditMinionFullWithValidation()
        {
            Console.Clear();
            Console.WriteLine("=== ПОЛНОЕ РЕДАКТИРОВАНИЕ МИНЬОНА (с валидацией) ===");

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

                var (success, errors) = InMemoryStorage.UpdateMinionWithValidation(
                    minionId, newName, newTavernTier, newTypes, newAttack, newHealth, newEffect);

                if (success)
                {
                    SaveDataAfterChange();
                    Console.WriteLine("\n✅ Миньон успешно обновлён!");
                }
                else
                {
                    Console.WriteLine("\n❌ Ошибки валидации:");
                    foreach (var error in errors)
                    {
                        ShowError($"  • {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка: {ex.Message}");
            }

            WaitForContinue();
        }

        static void ImportFromFileWithValidation()
        {
            Console.Clear();
            Console.WriteLine("=== ИМПОРТ ДАННЫХ (с валидацией ЛР8) ===");
            Console.WriteLine("ВНИМАНИЕ: Импорт заменит текущий список героев и миньонов!");
            Console.WriteLine("Все данные будут проверены валидатором перед импортом.");

            try
            {
                Console.Write("Введите путь к JSON-файлу для импорта: ");
                var importPath = (Console.ReadLine() ?? "").Trim();

                if (string.IsNullOrWhiteSpace(importPath))
                {
                    ShowError("Ошибка: путь пустой.");
                    WaitForContinue();
                    return;
                }

                if (!File.Exists(importPath))
                {
                    ShowError($"Ошибка: файл не найден: {importPath}");
                    WaitForContinue();
                    return;
                }

                Console.Write("Сделать backup перед импортом? (д/н): ");
                var backupAnswer = (Console.ReadLine() ?? "").Trim().ToLower();

                if (backupAnswer == "д" || backupAnswer == "y" || backupAnswer == "да")
                {
                    try
                    {
                        if (_storage != null)
                        {
                            _storage.SaveData();
                            var backupPath = BackupService.CreateBackup(_storage.FilePath, _backupsFolder);
                            Console.WriteLine($"✅ Backup создан: {backupPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Не удалось сделать backup: {ex.Message}");
                        Console.Write("Продолжить импорт без backup? (д/н): ");
                        var continueAnswer = (Console.ReadLine() ?? "").Trim().ToLower();
                        if (continueAnswer != "д" && continueAnswer != "y" && continueAnswer != "да")
                        {
                            Console.WriteLine("Импорт отменён.");
                            WaitForContinue();
                            return;
                        }
                    }
                }

                var json = File.ReadAllText(importPath);
                var data = JsonSerializer.Deserialize<JsonStorageService.GameData>(json);

                if (data == null)
                {
                    ShowError("Ошибка: не удалось прочитать JSON файл.");
                    WaitForContinue();
                    return;
                }

                Console.WriteLine($"\n📊 Найдено в файле: {data.Heroes.Count} героев, {data.Minions.Count} миньонов");
                Console.WriteLine("Проверяем валидацию...");

                Console.Write("\nТочно импортировать и заменить все данные? (д/н): ");
                var sure = (Console.ReadLine() ?? "").Trim().ToLower();

                if (sure != "д" && sure != "y" && sure != "да")
                {
                    Console.WriteLine("Импорт отменён.");
                    WaitForContinue();
                    return;
                }

                var (success, errors, validHeroes, validMinions) =
                    InMemoryStorage.ReplaceAllWithValidation(data.Heroes, data.Minions);

                if (success)
                {
                    if (_storage != null)
                    {
                        _storage.SaveData();
                    }

                    Console.WriteLine($"\n✅ Импорт выполнен успешно!");
                    Console.WriteLine($"   Валидных героев: {validHeroes}/{data.Heroes.Count}");
                    Console.WriteLine($"   Валидных миньонов: {validMinions}/{data.Minions.Count}");
                }
                else
                {
                    Console.WriteLine($"\n❌ Импорт отменён из-за ошибок валидации!");
                    Console.WriteLine($"   Валидных героев: {validHeroes}/{data.Heroes.Count}");
                    Console.WriteLine($"   Валидных миньонов: {validMinions}/{data.Minions.Count}");
                    Console.WriteLine("\nОшибки валидации:");

                    foreach (var error in errors.Take(10))
                    {
                        ShowError($"  • {error}");
                    }

                    if (errors.Count > 10)
                    {
                        Console.WriteLine($"  ... и ещё {errors.Count - 10} ошибок");
                    }

                    Console.WriteLine("\nИсправьте ошибки в файле и попробуйте импорт снова.");
                }
            }
            catch (JsonException ex)
            {
                ShowError($"Ошибка JSON: {ex.Message}");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка импорта: {ex.Message}");
            }

            WaitForContinue();
        }

        static void CreateBackup()
        {
            Console.Clear();
            Console.WriteLine("=== СОЗДАНИЕ РЕЗЕРВНОЙ КОПИИ ===");

            try
            {
                if (_storage == null)
                {
                    ShowError("Служба хранилища не инициализирована!");
                    WaitForContinue();
                    return;
                }

                _storage.SaveData();

                var backupPath = BackupService.CreateBackup(_storage.FilePath, _backupsFolder);
                Console.WriteLine($"✅ Бэкап создан: {backupPath}");
                Console.WriteLine($"   Файл: {Path.GetFileName(backupPath)}");
                Console.WriteLine($"   Размер: {new FileInfo(backupPath).Length} байт");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при создании бэкапа: {ex.Message}");
            }

            WaitForContinue();
        }

        static void CreateExport()
        {
            Console.Clear();
            Console.WriteLine("=== ЭКСПОРТ ДАННЫХ ===");

            try
            {
                if (_storage == null)
                {
                    ShowError("Служба хранилища не инициализирована!");
                    WaitForContinue();
                    return;
                }

                var data = new
                {
                    Heroes = InMemoryStorage.Heroes,
                    Minions = InMemoryStorage.Minions,
                    ExportDate = DateTime.Now,
                    TotalHeroes = InMemoryStorage.Heroes.Count,
                    TotalMinions = InMemoryStorage.Minions.Count,
                    ValidationRules = new
                    {
                        HeroNameMin = HeroValidator.NameMinLength,
                        HeroNameMax = HeroValidator.NameMaxLength,
                        MinionTavernMin = MinionValidator.MinTavernTier,
                        MinionTavernMax = MinionValidator.MaxTavernTier
                    }
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(data, options);
                var exportPath = BackupService.CreateExport(json, _exportsFolder);

                Console.WriteLine($"✅ Экспорт выполнен: {exportPath}");
                Console.WriteLine($"   Героев: {InMemoryStorage.Heroes.Count}");
                Console.WriteLine($"   Миньонов: {InMemoryStorage.Minions.Count}");
                Console.WriteLine($"   Размер файла: {json.Length} символов");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при экспорте: {ex.Message}");
            }

            WaitForContinue();
        }

        static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {message}");
            Console.ResetColor();
        }

        static void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

        static void RunDiagnostics()
        {
            Console.Clear();
            Console.WriteLine("=== ДИАГНОСТИКА СИСТЕМЫ (ЛР7-8) ===");

            Console.WriteLine("\n1. Проверка хранилища:");
            Console.WriteLine($"   Героев в памяти: {InMemoryStorage.Heroes.Count}");
            Console.WriteLine($"   Миньонов в памяти: {InMemoryStorage.Minions.Count}");

            Console.WriteLine("\n2. Валидация текущих данных (ЛР8):");

            int heroErrors = 0;
            foreach (var hero in InMemoryStorage.Heroes)
            {
                var errors = HeroValidator.Validate(hero);
                if (errors.Count > 0) heroErrors++;
            }

            int minionErrors = 0;
            foreach (var minion in InMemoryStorage.Minions)
            {
                var errors = MinionValidator.Validate(minion);
                if (errors.Count > 0) minionErrors++;
            }

            Console.WriteLine($"   Героев с ошибками: {heroErrors}/{InMemoryStorage.Heroes.Count}");
            Console.WriteLine($"   Миньонов с ошибками: {minionErrors}/{InMemoryStorage.Minions.Count}");

            if (heroErrors == 0 && minionErrors == 0)
            {
                Console.WriteLine("   ✅ Все данные валидны");
            }

            Console.WriteLine("\n3. Проверка файлов:");
            Console.WriteLine($"   Текущая директория: {Environment.CurrentDirectory}");

            if (_storage != null)
            {
                Console.WriteLine($"   Основной файл: {_storage.FilePath}");
                Console.WriteLine($"   Существует: {File.Exists(_storage.FilePath)}");
            }

            Console.WriteLine($"\n4. Папки ЛР7:");
            Console.WriteLine($"   Backups: {_backupsFolder}");
            Console.WriteLine($"   Exports: {_exportsFolder}");

            Console.WriteLine("\n5. Список бэкапов:");
            try
            {
                if (Directory.Exists(_backupsFolder))
                {
                    var backupFiles = Directory.GetFiles(_backupsFolder, "*.json");
                    foreach (var file in backupFiles.Take(3))
                    {
                        var info = new FileInfo(file);
                        Console.WriteLine($"   - {Path.GetFileName(file)} ({info.Length} байт)");
                    }
                    Console.WriteLine($"   Всего бэкапов: {backupFiles.Length}");
                }
                else
                {
                    Console.WriteLine("   Папка backups не существует");
                }
            }
            catch { }

            Console.WriteLine("\n6. Список экспортов:");
            try
            {
                if (Directory.Exists(_exportsFolder))
                {
                    var exportFiles = Directory.GetFiles(_exportsFolder, "*.json");
                    foreach (var file in exportFiles.Take(3))
                    {
                        var info = new FileInfo(file);
                        Console.WriteLine($"   - {Path.GetFileName(file)} ({info.Length} байт)");
                    }
                    Console.WriteLine($"   Всего экспортов: {exportFiles.Length}");
                }
                else
                {
                    Console.WriteLine("   Папка exports не существует");
                }
            }
            catch { }

            WaitForContinue();
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

                    var error = HeroValidator.ValidateQuick(hero);
                    if (error != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"   ⚠️  Проблема: {error}");
                        Console.ResetColor();
                    }

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

                    var error = MinionValidator.ValidateQuick(minion);
                    if (error != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"   ⚠️  Проблема: {error}");
                        Console.ResetColor();
                    }

                    Console.WriteLine("----------------------------------");
                }
            }

            Console.WriteLine($"Всего миньонов: {minions.Count}");
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
                else
                {
                    ShowError($"Герой с ID {heroId} не найден!");
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
                else
                {
                    ShowError($"Герой с ID {heroId} не найден!");
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
                else
                {
                    ShowError($"Миньон с ID {minionId} не найден!");
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
            if (filteredHeroes.Count == 0)
            {
                Console.WriteLine("Героев не найдено.");
                return;
            }

            foreach (var hero in filteredHeroes)
            {
                Console.WriteLine($"[ID: {hero.Id}] {hero.Name}");
                Console.WriteLine($"   Титул: {hero.Title}");
                Console.WriteLine($"   Рейтинг: {hero.Tier}");
                Console.WriteLine($"   Описание: {hero.Description}");
                Console.WriteLine("----------------------------------");
            }
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
            if (filteredMinions.Count == 0)
            {
                Console.WriteLine("Миньонов не найдено.");
                return;
            }

            foreach (var minion in filteredMinions)
            {
                Console.WriteLine($"[ID: {minion.Id}] {minion.Name}");
                Console.WriteLine($"   Уровень таверны: {minion.TavernTier}");
                Console.WriteLine($"   Типы: {minion.TypesDisplay}");
                Console.WriteLine($"   Статы: {minion.Attack}/{minion.Health}");
                Console.WriteLine($"   Эффект: {minion.Effect}");
                Console.WriteLine("----------------------------------");
            }
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
            Console.WriteLine("1 - По возрастанию (А-Я)");
            Console.WriteLine("2 - По убыванию (Я-А)");
            Console.Write("Ваш выбор: ");

            bool ascending = Console.ReadLine() == "1";
            var sortedMinions = InMemoryStorage.GetMinionsSortedByName(ascending);

            Console.WriteLine($"\nМиньоны отсортированы по имени ({(ascending ? "А-Я" : "Я-А")}):\n");
            DisplayMinionsList(sortedMinions);

            WaitForContinue();
        }

        static void SortMinionsByTavernTierMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СОРТИРОВКА МИНЬОНОВ ПО УРОВНЮ ТАВЕРНЫ ===");

            Console.WriteLine("1 - От высокого к низкому (6 → 1)");
            Console.WriteLine("2 - От низкого к высокому (1 → 6)");
            Console.Write("Ваш выбор: ");

            bool descending = Console.ReadLine() == "1";
            var sortedMinions = InMemoryStorage.GetMinionsSortedByTavernTier(descending);

            Console.WriteLine($"\nМиньоны отсортированы по уровню таверны ({(descending ? "6→1" : "1→6")}):\n");
            DisplayMinionsList(sortedMinions);

            WaitForContinue();
        }
    }
}