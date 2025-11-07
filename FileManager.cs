using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Setup_RadioPlayer
{
    public static class FileManager
    {
        public static async Task ExtractFilesAsync(string installPath, Action<int> progressCallback)
        {
            // Создаем директорию установки, если не существует
            Directory.CreateDirectory(installPath);

            // Получаем данные zip из ресурсов
            byte[] zipData = Properties.Resources.data;

            // Путь к временному файлу
            string tempZipPath = Path.Combine(Path.GetTempPath(), "data.zip");

            try
            {
                // Записываем zip данные во временный файл
                await Task.Run(() => System.IO.File.WriteAllBytes(tempZipPath, zipData));

                // Распаковываем zip файл
                using (ZipArchive archive = ZipFile.OpenRead(tempZipPath))
                {
                    int totalEntries = archive.Entries.Count;
                    int processedEntries = 0;

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string filePath = Path.Combine(installPath, entry.FullName);

                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            // Это директория
                            Directory.CreateDirectory(filePath);
                        }
                        else
                        {
                            // Создаем директорию, если не существует
                            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                            // Распаковываем файл
                            await Task.Run(() => entry.ExtractToFile(filePath, true));
                        }

                        processedEntries++;
                        int progress = (int)((double)processedEntries / totalEntries * 100);
                        progressCallback(progress);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при распаковке файлов: " + ex.Message, ex);
            }
            finally
            {
                // Удаляем временный zip файл
                try
                {
                    if (System.IO.File.Exists(tempZipPath))
                    {
                        System.IO.File.Delete(tempZipPath);
                    }
                }
                catch (Exception ex)
                {
                    // Логируем, но не выбрасываем исключение
                    System.IO.File.AppendAllText("install.log", $"Ошибка при удалении временного файла: {ex}\n");
                }
            }
        }
    }
}
