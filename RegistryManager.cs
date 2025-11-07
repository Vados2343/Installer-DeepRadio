using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public static class RegistryManager
    {
        public static void AddToRegistry(string targetDir)
        {
            // ИСПРАВЛЕНО: Добавлена подпапка "data"
            string exePath = Path.Combine(targetDir, "data", "RadioPlayerV2.exe");

            try
            {
                // Проверяем существование файла перед добавлением в автозапуск
                if (!System.IO.File.Exists(exePath))
                {
                    throw new FileNotFoundException($"Файл '{exePath}' не знайдено. Неможливо додати в автозапуск.");
                }

                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    key.SetValue("RadioPlayer", exePath);
                    key.Close();

                    // Логирование успеха
                    string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");
                    System.IO.File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: Успішно додано в автозапуск: {exePath}\n");
                }
                else
                {
                    throw new Exception("Не вдалося відкрити ключ реєстру автозапуску.");
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");
                System.IO.File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: ПОМИЛКА при додаванні в автозапуск: {ex}\n");

                MessageBox.Show("Не вдалося додати програму в автозапуск: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для удаления программы из автозапуска (для деинсталлятора)
        public static void RemoveFromRegistry()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    if (key.GetValue("RadioPlayer") != null)
                    {
                        key.DeleteValue("RadioPlayer");
                    }
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");
                System.IO.File.AppendAllText(logPath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: ПОМИЛКА при видаленні з автозапуску: {ex}\n");
            }
        }
    }
}