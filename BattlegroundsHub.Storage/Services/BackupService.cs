using System;
using System.IO;

namespace BattlegroundsHub.Storage.Services
{
    public static class BackupService
    {
        public static string CreateBackup(string sourceFilePath, string backupsFolder)
        {
            if (!File.Exists(sourceFilePath))
                throw new FileNotFoundException("Файл данных не найден.", sourceFilePath);

            Directory.CreateDirectory(backupsFolder);

            var fileName = $"battlegrounds_backup_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            var destPath = Path.Combine(backupsFolder, fileName);

            File.Copy(sourceFilePath, destPath, overwrite: false);

            return destPath;
        }

        public static string CreateExport(string data, string exportsFolder)
        {
            Directory.CreateDirectory(exportsFolder);

            var fileName = $"battlegrounds_export_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.json";
            var exportPath = Path.Combine(exportsFolder, fileName);

            File.WriteAllText(exportPath, data);

            return exportPath;
        }
    }
}