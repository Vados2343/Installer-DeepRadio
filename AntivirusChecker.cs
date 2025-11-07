using System;
using System.Management;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public class AntivirusChecker
    {
        public static string GetAntivirusName()
        {
            try
            {
                string wmiPath = @"\\.\root\SecurityCenter2";
                string query = "SELECT * FROM AntiVirusProduct";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiPath, query);
                foreach (ManagementObject virusChecker in searcher.Get())
                {
                    return virusChecker["displayName"].ToString();
                }
            }
            catch (Exception ex)
            {
                return "Ошибка при проверке антивируса: " + ex.Message;
            }
            return null;
        }
    }
}
