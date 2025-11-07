using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System;

namespace Setup_RadioPlayer
{
    public static class DependencyManager
    {
        public static bool IsDotNetFrameworkInstalled(string version)
        {
            string subkey = $@"SOFTWARE\Microsoft\NET Framework Setup\NDP\{version}";
            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(subkey))
            {
                return ndpKey != null;
            }
        }

        public static void InstallDotNetFramework()
        {
            string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dotNetFx48_Full_x86_x64.exe");
            if (System.IO.File.Exists(installerPath))
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = installerPath,
                        Arguments = "/q /norestart",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();
            }
            else
            {
                MessageBox.Show("Установщик .NET Framework не найден.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
