using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public static class ShortcutManager
    {
        public static void CreateShortcuts(string targetDir, bool desktopShortcut, bool startMenuShortcut)
        {
            string exePath = Path.Combine(targetDir, "data", "RadioPlayerV2.exe");

            if (desktopShortcut)
            {
                // Создание или замена ярлыка на рабочем столе
                string desktopShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RadioPlayerV2.lnk");
                CreateOrReplaceShortcut(desktopShortcutPath, exePath);
            }

            if (startMenuShortcut)
            {
                // Создание или замена ярлыка в меню Пуск
                string startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "RadioPlayer");
                Directory.CreateDirectory(startMenuPath);
                string startMenuShortcutPath = Path.Combine(startMenuPath, "RadioPlayerV2.lnk");
                CreateOrReplaceShortcut(startMenuShortcutPath, exePath);
            }
        }

        private static void CreateOrReplaceShortcut(string shortcutPath, string targetPath)
        {
            try
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.Description = "Запуск RadioPlayer";
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
                shortcut.Save();
            }
            catch (Exception ex)
            {
                // Используем полное имя System.IO.File
                System.IO.File.AppendAllText("install.log", $"Ошибка при создании ярлыка: {ex}\n");
                MessageBox.Show($"Не удалось создать ярлык по пути {shortcutPath}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
