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
            string exePath = Path.Combine(targetDir, "RadioPlayerV2.exe");
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    key.SetValue("RadioPlayer", exePath);
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось добавить программу в автозапуск: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}