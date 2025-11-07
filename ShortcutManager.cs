using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public static class ShortcutManager
    {
        private static string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "install.log");

        /// <summary>
        /// Создает ярлыки для приложения с умной обработкой OneDrive
        /// </summary>
        public static void CreateShortcuts(string targetDir, bool desktopShortcut, bool startMenuShortcut)
        {
            // ИСПРАВЛЕНО: Правильный путь к exe файлу с подпапкой "data"
            string exePath = Path.Combine(targetDir, "data", "RadioPlayerV2.exe");

            Log($"Початок створення ярликів. Цільовий файл: {exePath}");

            // Проверяем существование exe файла
            if (!System.IO.File.Exists(exePath))
            {
                string errorMsg = $"КРИТИЧНА ПОМИЛКА: Файл програми не знайдено за шляхом: {exePath}";
                Log(errorMsg);
                MessageBox.Show(errorMsg, "Помилка створення ярликів", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool desktopSuccess = true;
            bool startMenuSuccess = true;

            if (desktopShortcut)
            {
                Log("Спроба створити ярлик на робочому столі...");
                desktopSuccess = CreateDesktopShortcut(exePath);
            }

            if (startMenuShortcut)
            {
                Log("Спроба створити ярлик в меню Пуск...");
                startMenuSuccess = CreateStartMenuShortcut(exePath);
            }

            // Показываем итоговое сообщение
            if (desktopShortcut || startMenuShortcut)
            {
                ShowCreationSummary(desktopShortcut, desktopSuccess, startMenuShortcut, startMenuSuccess);
            }
        }

        /// <summary>
        /// Создает ярлык на рабочем столе с обработкой OneDrive
        /// </summary>
        private static bool CreateDesktopShortcut(string exePath)
        {
            List<string> desktopPaths = GetAllPossibleDesktopPaths();

            Log($"Знайдено {desktopPaths.Count} можливих шляхів до робочого столу:");
            foreach (var path in desktopPaths)
            {
                Log($"  - {path}");
            }

            bool success = false;
            List<string> failedPaths = new List<string>();
            List<string> successPaths = new List<string>();

            foreach (string desktopPath in desktopPaths)
            {
                try
                {
                    // Проверяем доступность пути
                    if (!Directory.Exists(desktopPath))
                    {
                        Log($"Шлях не існує: {desktopPath}");
                        continue;
                    }

                    string shortcutPath = Path.Combine(desktopPath, "RadioPlayerV2.lnk");

                    if (CreateShortcutFile(shortcutPath, exePath))
                    {
                        success = true;
                        successPaths.Add(desktopPath);
                        Log($"✓ Ярлик успішно створено: {shortcutPath}");
                    }
                    else
                    {
                        failedPaths.Add(desktopPath);
                    }
                }
                catch (Exception ex)
                {
                    Log($"ПОМИЛКА при створенні ярлика в {desktopPath}: {ex.Message}");
                    failedPaths.Add(desktopPath);
                }
            }

            if (success)
            {
                Log($"✓ Ярлик на робочому столі створено успішно в {successPaths.Count} місці(ях)");
            }
            else
            {
                Log($"✗ НЕ ВДАЛОСЯ створити ярлик на робочому столі. Спробовано {desktopPaths.Count} шляхів.");
            }

            return success;
        }

        /// <summary>
        /// Создает ярлык в меню Пуск
        /// </summary>
        private static bool CreateStartMenuShortcut(string exePath)
        {
            try
            {
                // Получаем путь к меню Пуск
                string startMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                string programsPath = Path.Combine(startMenuPath, "Programs", "RadioPlayer");

                Log($"Шлях до меню Пуск: {programsPath}");

                // Создаем папку, если не существует
                Directory.CreateDirectory(programsPath);

                string shortcutPath = Path.Combine(programsPath, "RadioPlayerV2.lnk");

                if (CreateShortcutFile(shortcutPath, exePath))
                {
                    Log($"✓ Ярлик в меню Пуск створено успішно: {shortcutPath}");
                    return true;
                }
                else
                {
                    Log($"✗ Не вдалося створити ярлик в меню Пуск");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"ПОМИЛКА при створенні ярлика в меню Пуск: {ex}");
                MessageBox.Show($"Не вдалося створити ярлик в меню Пуск: {ex.Message}",
                    "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        /// <summary>
        /// Получает все возможные пути к рабочему столу (включая OneDrive)
        /// </summary>
        private static List<string> GetAllPossibleDesktopPaths()
        {
            List<string> desktopPaths = new List<string>();

            // 1. Стандартный Desktop
            try
            {
                string standardDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (!string.IsNullOrEmpty(standardDesktop))
                {
                    desktopPaths.Add(standardDesktop);
                }
            }
            catch (Exception ex)
            {
                Log($"Помилка при отриманні стандартного шляху Desktop: {ex.Message}");
            }

            // 2. Desktop из реестра (может указывать на локальный путь если OneDrive активен)
            try
            {
                string registryDesktop = GetDesktopPathFromRegistry();
                if (!string.IsNullOrEmpty(registryDesktop) && !desktopPaths.Contains(registryDesktop))
                {
                    desktopPaths.Add(registryDesktop);
                }
            }
            catch (Exception ex)
            {
                Log($"Помилка при читанні реєстру: {ex.Message}");
            }

            // 3. Проверяем OneDrive Desktop
            try
            {
                string onedriveDesktop = GetOneDriveDesktopPath();
                if (!string.IsNullOrEmpty(onedriveDesktop) && !desktopPaths.Contains(onedriveDesktop))
                {
                    desktopPaths.Add(onedriveDesktop);
                }
            }
            catch (Exception ex)
            {
                Log($"Помилка при перевірці OneDrive Desktop: {ex.Message}");
            }

            // 4. Резервный вариант - прямой путь к Desktop
            try
            {
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string fallbackDesktop = Path.Combine(userProfile, "Desktop");
                if (!desktopPaths.Contains(fallbackDesktop) && Directory.Exists(fallbackDesktop))
                {
                    desktopPaths.Add(fallbackDesktop);
                }
            }
            catch (Exception ex)
            {
                Log($"Помилка при створенні резервного шляху Desktop: {ex.Message}");
            }

            // Удаляем дубликаты (сравнение путей без учета регистра)
            desktopPaths = desktopPaths
                .Select(p => Path.GetFullPath(p))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Where(p => Directory.Exists(p))
                .ToList();

            return desktopPaths;
        }

        /// <summary>
        /// Получает путь к Desktop из реестра
        /// </summary>
        private static string GetDesktopPathFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("Desktop");
                        if (value != null)
                        {
                            string path = value.ToString();
                            // Разворачиваем переменные окружения
                            path = Environment.ExpandEnvironmentVariables(path);
                            return path;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Помилка читання реєстру Desktop: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Получает путь к OneDrive Desktop
        /// </summary>
        private static string GetOneDriveDesktopPath()
        {
            try
            {
                // Проверяем переменную окружения OneDrive
                string oneDrivePath = Environment.GetEnvironmentVariable("OneDrive");
                if (!string.IsNullOrEmpty(oneDrivePath))
                {
                    string onedriveDesktop = Path.Combine(oneDrivePath, "Desktop");
                    if (Directory.Exists(onedriveDesktop))
                    {
                        return onedriveDesktop;
                    }
                }

                // Проверяем OneDriveCommercial (для бизнес аккаунтов)
                string onedriveCommercial = Environment.GetEnvironmentVariable("OneDriveCommercial");
                if (!string.IsNullOrEmpty(onedriveCommercial))
                {
                    string commercialDesktop = Path.Combine(onedriveCommercial, "Desktop");
                    if (Directory.Exists(commercialDesktop))
                    {
                        return commercialDesktop;
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Помилка при перевірці OneDrive: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Создает файл ярлыка
        /// </summary>
        private static bool CreateShortcutFile(string shortcutPath, string targetPath)
        {
            try
            {
                // Удаляем старый ярлык, если существует
                if (System.IO.File.Exists(shortcutPath))
                {
                    Log($"Видалення старого ярлика: {shortcutPath}");
                    System.IO.File.Delete(shortcutPath);
                }

                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.Description = "Запуск RadioPlayer";
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);

                // Добавляем иконку
                shortcut.IconLocation = targetPath + ",0";

                shortcut.Save();

                // Проверяем, что ярлык действительно создан
                if (System.IO.File.Exists(shortcutPath))
                {
                    Log($"Ярлик успішно збережено та перевірено: {shortcutPath}");
                    return true;
                }
                else
                {
                    Log($"Ярлик не був створений (файл не існує після збереження): {shortcutPath}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log($"ПОМИЛКА при створенні файлу ярлика {shortcutPath}: {ex}");
                return false;
            }
        }

        /// <summary>
        /// Показывает итоговое сообщение о создании ярлыков
        /// </summary>
        private static void ShowCreationSummary(bool desktopRequested, bool desktopSuccess,
                                                bool startMenuRequested, bool startMenuSuccess)
        {
            List<string> successMessages = new List<string>();
            List<string> failMessages = new List<string>();

            if (desktopRequested)
            {
                if (desktopSuccess)
                {
                    successMessages.Add("✓ Ярлик на робочому столі");
                }
                else
                {
                    failMessages.Add("✗ Ярлик на робочому столі");
                }
            }

            if (startMenuRequested)
            {
                if (startMenuSuccess)
                {
                    successMessages.Add("✓ Ярлик в меню Пуск");
                }
                else
                {
                    failMessages.Add("✗ Ярлик в меню Пуск");
                }
            }

            // Формируем сообщение
            if (failMessages.Count > 0)
            {
                string message = "Результат створення ярликів:\n\n";

                if (successMessages.Count > 0)
                {
                    message += string.Join("\n", successMessages) + "\n\n";
                }

                message += string.Join("\n", failMessages) + "\n\n";
                message += "Деякі ярлики не вдалося створити. Перевірте права доступу або створіть ярлик вручну.";

                MessageBox.Show(message, "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (successMessages.Count > 0)
            {
                Log("Всі ярлики успішно створено!");
            }
        }

        /// <summary>
        /// Логирование
        /// </summary>
        private static void Log(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: [ShortcutManager] {message}\n";
                System.IO.File.AppendAllText(logFilePath, logMessage);
            }
            catch
            {
                // Игнорируем ошибки логирования
            }
        }
    }
}
