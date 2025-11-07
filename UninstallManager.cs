using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public static class UninstallManager
    {
        private static string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uninstall.log");

        public static void CreateUninstaller(string installPath)
        {
            try
            {
                string uninstallerPath = Path.Combine(installPath, "Uninstall.exe");
                string currentExePath = Application.ExecutablePath;

                if (File.Exists(currentExePath))
                {
                    File.Copy(currentExePath, uninstallerPath, true);
                    Log($"Uninstaller created: {uninstallerPath}");
                }

                string startMenuPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                    "Programs",
                    "RadioPlayer"
                );

                if (Directory.Exists(startMenuPath))
                {
                    string uninstallShortcut = Path.Combine(startMenuPath, "Uninstall RadioPlayer.lnk");
                    CreateUninstallShortcut(uninstallShortcut, uninstallerPath, installPath);
                }

                AddUninstallRegistryEntry(installPath, uninstallerPath);
            }
            catch (Exception ex)
            {
                Log($"Error creating uninstaller: {ex.Message}");
            }
        }

        private static void CreateUninstallShortcut(string shortcutPath, string targetPath, string workingDir)
        {
            try
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);

                shortcut.Description = "Uninstall Old RadioPlayer";
                shortcut.TargetPath = targetPath;
                shortcut.Arguments = "/uninstall";
                shortcut.WorkingDirectory = workingDir;
                shortcut.IconLocation = targetPath + ",0";
                shortcut.Save();

                Log($"Uninstall shortcut created: {shortcutPath}");
            }
            catch (Exception ex)
            {
                Log($"Error creating uninstall shortcut: {ex.Message}");
            }
        }

        private static void AddUninstallRegistryEntry(string installPath, string uninstallerPath)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OldRadioPlayer"))
                {
                    if (key != null)
                    {
                        key.SetValue("DisplayName", "Old RadioPlayer");
                        key.SetValue("DisplayVersion", "1.0");
                        key.SetValue("Publisher", "DeepRadio");
                        key.SetValue("InstallLocation", installPath);
                        key.SetValue("UninstallString", $"\"{uninstallerPath}\" /uninstall");
                        key.SetValue("DisplayIcon", Path.Combine(installPath, "data", "RadioPlayerV2.exe"));
                        key.SetValue("NoModify", 1, RegistryValueKind.DWord);
                        key.SetValue("NoRepair", 1, RegistryValueKind.DWord);

                        Log("Uninstall registry entry created");
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Error adding uninstall registry entry: {ex.Message}");
            }
        }

        public static void Uninstall()
        {
            string installPath = GetInstallPath();

            if (string.IsNullOrEmpty(installPath) || !Directory.Exists(installPath))
            {
                var lang = LanguageManager.CurrentLanguage;
                string message = lang == LanguageManager.Language.Ukrainian ? "Програма не встановлена або вже видалена." :
                                lang == LanguageManager.Language.English ? "The program is not installed or has already been removed." :
                                "Il programma non è installato o è già stato rimosso.";

                string title = lang == LanguageManager.Language.Ukrainian ? "Інформація" :
                              lang == LanguageManager.Language.English ? "Information" :
                              "Informazione";

                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmLang = LanguageManager.CurrentLanguage;
            string confirmMessage = confirmLang == LanguageManager.Language.Ukrainian ? "Ви дійсно хочете видалити Old RadioPlayer?" :
                                   confirmLang == LanguageManager.Language.English ? "Do you really want to uninstall Old RadioPlayer?" :
                                   "Vuoi davvero disinstallare Old RadioPlayer?";

            string confirmTitle = confirmLang == LanguageManager.Language.Ukrainian ? "Підтвердження видалення" :
                                 confirmLang == LanguageManager.Language.English ? "Uninstall Confirmation" :
                                 "Conferma disinstallazione";

            var result = MessageBox.Show(confirmMessage, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                PerformUninstall(installPath);
            }
        }

        private static void PerformUninstall(string installPath)
        {
            try
            {
                Log("Starting uninstallation...");

                RemoveFromStartup();

                RemoveShortcuts();

                RemoveUninstallRegistryEntry();

                RemoveStartMenuFolder();

                ScheduleDirectoryDeletion(installPath);

                var lang = LanguageManager.CurrentLanguage;
                string successMessage = lang == LanguageManager.Language.Ukrainian ? "Old RadioPlayer успішно видалено з вашого комп'ютера." :
                                       lang == LanguageManager.Language.English ? "Old RadioPlayer has been successfully removed from your computer." :
                                       "Old RadioPlayer è stato rimosso con successo dal tuo computer.";

                string successTitle = lang == LanguageManager.Language.Ukrainian ? "Видалення завершено" :
                                     lang == LanguageManager.Language.English ? "Uninstall Complete" :
                                     "Disinstallazione completata";

                MessageBox.Show(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);

                Log("Uninstallation completed successfully");
            }
            catch (Exception ex)
            {
                Log($"Error during uninstallation: {ex}");

                var lang = LanguageManager.CurrentLanguage;
                string errorMessage = lang == LanguageManager.Language.Ukrainian ? "Помилка при видаленні: " + ex.Message :
                                     lang == LanguageManager.Language.English ? "Error during uninstallation: " + ex.Message :
                                     "Errore durante la disinstallazione: " + ex.Message;

                string errorTitle = lang == LanguageManager.Language.Ukrainian ? "Помилка" :
                                   lang == LanguageManager.Language.English ? "Error" :
                                   "Errore";

                MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GetInstallPath()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OldRadioPlayer"))
                {
                    if (key != null)
                    {
                        return key.GetValue("InstallLocation") as string;
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"Error getting install path: {ex.Message}");
            }
            return null;
        }

        private static void RemoveFromStartup()
        {
            try
            {
                RegistryManager.RemoveFromRegistry();
                Log("Removed from startup");
            }
            catch (Exception ex)
            {
                Log($"Error removing from startup: {ex.Message}");
            }
        }

        private static void RemoveShortcuts()
        {
            try
            {
                string desktopShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RadioPlayerV2.lnk");
                if (File.Exists(desktopShortcut))
                {
                    File.Delete(desktopShortcut);
                    Log($"Deleted desktop shortcut: {desktopShortcut}");
                }

                string startMenuShortcut = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                    "Programs",
                    "RadioPlayer",
                    "RadioPlayerV2.lnk"
                );
                if (File.Exists(startMenuShortcut))
                {
                    File.Delete(startMenuShortcut);
                    Log($"Deleted start menu shortcut: {startMenuShortcut}");
                }
            }
            catch (Exception ex)
            {
                Log($"Error removing shortcuts: {ex.Message}");
            }
        }

        private static void RemoveUninstallRegistryEntry()
        {
            try
            {
                Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\OldRadioPlayer", false);
                Log("Removed uninstall registry entry");
            }
            catch (Exception ex)
            {
                Log($"Error removing uninstall registry entry: {ex.Message}");
            }
        }

        private static void RemoveStartMenuFolder()
        {
            try
            {
                string startMenuPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                    "Programs",
                    "RadioPlayer"
                );

                if (Directory.Exists(startMenuPath))
                {
                    Directory.Delete(startMenuPath, true);
                    Log($"Deleted start menu folder: {startMenuPath}");
                }
            }
            catch (Exception ex)
            {
                Log($"Error removing start menu folder: {ex.Message}");
            }
        }

        private static void ScheduleDirectoryDeletion(string directory)
        {
            try
            {
                string batchContent = $@"@echo off
timeout /t 2 /nobreak > nul
rd /s /q ""{directory}""
del ""%~f0""
";
                string batchPath = Path.Combine(Path.GetTempPath(), "cleanup_radioplayer.bat");
                File.WriteAllText(batchPath, batchContent);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = batchPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(psi);
                Log($"Scheduled directory deletion: {directory}");
            }
            catch (Exception ex)
            {
                Log($"Error scheduling directory deletion: {ex.Message}");
            }
        }

        private static void Log(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: [Uninstaller] {message}\n";
                File.AppendAllText(logFilePath, logMessage);
            }
            catch
            {
            }
        }
    }
}
