using BattlegroundsHubHS.Core.Models;
using BattlegroundsHubHS.Core.Services;
using System.Text.Json;

namespace BattlegroundsHub.Storage
{
    public class JsonStorageService
    {
        private readonly string _filePath;
        private readonly string _initialDataPath;

        public JsonStorageService()
        {
            var appFolder = AppContext.BaseDirectory;
            var dataFolder = Path.Combine(appFolder, "data");
            _filePath = Path.Combine(dataFolder, "battlegrounds_data.json");
            _initialDataPath = Path.Combine(appFolder, "initial_data.json");

            Directory.CreateDirectory(dataFolder);

            Console.WriteLine($"Данные будут сохраняться в: {_filePath}");
            Console.WriteLine($"Начальные данные: {_initialDataPath}");
        }

        public class GameData
        {
            public List<Hero> Heroes { get; set; } = new List<Hero>();
            public List<Minion> Minions { get; set; } = new List<Minion>();
        }

        public void LoadData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var data = JsonSerializer.Deserialize<GameData>(json);

                    if (data != null && (data.Heroes.Count > 0 || data.Minions.Count > 0))
                    {
                        InMemoryStorage.LoadFromFile(data.Heroes, data.Minions);
                        Console.WriteLine($"✅ Загружено {data.Heroes.Count} героев и {data.Minions.Count} миньонов из основного файла.");
                        return;
                    }
                }

                Console.WriteLine("Основной файл не найден или пустой. Загружаем начальные данные...");
                LoadInitialData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при загрузке данных: {ex.Message}");
                Console.WriteLine("Пробуем загрузить начальные данные...");
                LoadInitialData();
            }
        }

        private void LoadInitialData()
        {
            try
            {
                if (!File.Exists(_initialDataPath))
                {
                    Console.WriteLine($"❌ Файл начальных данных не найден: {_initialDataPath}");
                    Console.WriteLine("Начинаем с пустого списка.");
                    return;
                }

                var json = File.ReadAllText(_initialDataPath);
                var data = JsonSerializer.Deserialize<GameData>(json);

                if (data == null || (data.Heroes.Count == 0 && data.Minions.Count == 0))
                {
                    Console.WriteLine("❌ Файл начальных данных пустой или повреждён.");
                    return;
                }

                InMemoryStorage.LoadFromFile(data.Heroes, data.Minions);
                Console.WriteLine($"✅ Загружено {data.Heroes.Count} героев и {data.Minions.Count} миньонов из начального файла.");

                SaveData();
                Console.WriteLine("Начальные данные сохранены в основной файл.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при загрузке начальных данных: {ex.Message}");
            }
        }

        public void SaveData()
        {
            try
            {
                var data = new GameData
                {
                    Heroes = InMemoryStorage.Heroes,
                    Minions = InMemoryStorage.Minions
                };

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(_filePath, json);

                Console.WriteLine($"✅ Данные сохранены: {data.Heroes.Count} героев, {data.Minions.Count} миньонов");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка при сохранении данных: {ex.Message}");
            }
        }

        public int HeroesCount => InMemoryStorage.Heroes.Count;
        public int MinionsCount => InMemoryStorage.Minions.Count;
        public string FilePath => _filePath;
        public string InitialDataPath => _initialDataPath;
    }
}