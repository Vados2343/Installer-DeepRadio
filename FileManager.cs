using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Setup_RadioPlayer
{
    public static class FileManager
    {
        private static string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");

        public static async Task ExtractFilesAsync(string installPath, Action<int> progressCallback)
        {
            Log("Початок розпакування файлів...");

            // Проверяем валидность пути установки
            if (string.IsNullOrEmpty(installPath))
            {
                throw new ArgumentException("Шлях установки не може бути порожнім.");
            }

            // Создаем директорию установки, если не существует
            try
            {
                Directory.CreateDirectory(installPath);
                Log($"Директорія створена/перевірена: {installPath}");
            }
            catch (Exception ex)
            {
                Log($"ПОМИЛКА створення директорії: {ex.Message}");
                throw new Exception($"Не вдалося створити директорію установки: {ex.Message}", ex);
            }

            // Получаем данные zip из ресурсов
            byte[] zipData = Properties.Resources.data;

            if (zipData == null || zipData.Length == 0)
            {
                throw new Exception("Файли установки пошкоджені або відсутні в ресурсах.");
            }

            Log($"Розмір архіву: {zipData.Length / 1024 / 1024} МБ");

            // Путь к временному файлу с уникальным именем для избежания конфликтов
            string tempZipPath = Path.Combine(Path.GetTempPath(), $"RadioPlayer_Setup_{Guid.NewGuid()}.zip");
            Log($"Тимчасовий файл: {tempZipPath}");

            try
            {
                // Записываем zip данные во временный файл
                await Task.Run(() => System.IO.File.WriteAllBytes(tempZipPath, zipData));
                Log("Тимчасовий файл створено успішно");

                // Проверяем что файл действительно создан
                if (!System.IO.File.Exists(tempZipPath))
                {
                    throw new Exception("Не вдалося створити тимчасовий файл для розпакування.");
                }

                // Распаковываем zip файл
                using (ZipArchive archive = ZipFile.OpenRead(tempZipPath))
                {
                    int totalEntries = archive.Entries.Count;
                    int processedEntries = 0;

                    Log($"Всього файлів для розпакування: {totalEntries}");

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // Защита от Zip Slip атаки
                        string filePath = Path.Combine(installPath, entry.FullName);
                        string normalizedPath = Path.GetFullPath(filePath);
                        string normalizedInstallPath = Path.GetFullPath(installPath);

                        if (!normalizedPath.StartsWith(normalizedInstallPath, StringComparison.OrdinalIgnoreCase))
                        {
                            Log($"БЕЗПЕКА: Блоковано спробу записати файл поза директорією установки: {entry.FullName}");
                            throw new Exception($"Виявлено небезпечний шлях у архіві: {entry.FullName}");
                        }

                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            // Это директория
                            Directory.CreateDirectory(filePath);
                            Log($"Створено директорію: {entry.FullName}");
                        }
                        else
                        {
                            try
                            {
                                // Создаем директорию, если не существует
                                string directory = Path.GetDirectoryName(filePath);
                                if (!Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }

                                // Распаковываем файл
                                await Task.Run(() => entry.ExtractToFile(filePath, true));

                                // Проверяем что файл действительно распакован
                                if (System.IO.File.Exists(filePath))
                                {
                                    FileInfo fileInfo = new FileInfo(filePath);
                                    Log($"Розпаковано: {entry.FullName} ({fileInfo.Length} байт)");
                                }
                                else
                                {
                                    throw new Exception($"Файл не був розпакований: {entry.FullName}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Log($"ПОМИЛКА при розпакуванні файлу {entry.FullName}: {ex.Message}");
                                throw;
                            }
                        }

                        processedEntries++;
                        int progress = (int)((double)processedEntries / totalEntries * 100);
                        progressCallback(progress);
                    }
                }

                Log("Розпакування завершено успішно!");
            }
            catch (Exception ex)
            {
                Log($"КРИТИЧНА ПОМИЛКА при розпакуванні: {ex}");
                throw new Exception("Помилка при розпакуванні файлів: " + ex.Message, ex);
            }
            finally
            {
                // Удаляем временный zip файл
                try
                {
                    if (System.IO.File.Exists(tempZipPath))
                    {
                        System.IO.File.Delete(tempZipPath);
                        Log($"Тимчасовий файл видалено: {tempZipPath}");
                    }
                }
                catch (Exception ex)
                {
                    // Логируем, но не выбрасываем исключение
                    Log($"Попередження: Не вдалося видалити тимчасовий файл: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Логирование
        /// </summary>
        private static void Log(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: [FileManager] {message}\n";
                System.IO.File.AppendAllText(logFilePath, logMessage);
            }
            catch
            {
                // Игнорируем ошибки логирования
            }
        }
    }
}
