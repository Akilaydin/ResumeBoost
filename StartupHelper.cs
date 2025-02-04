// ReSharper disable LocalizableElement
namespace OriApps.ResumeBoost
{
    using Microsoft.Win32;

    public static class StartupHelper
    {
        private const string RegistryKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "ResumeBoost";
        
        private const string MinimizedRegistryKey = @"Software\" + AppName;
        private const string MinimizedValueName = "StartMinimized";
        
        public static void EnableAutoStart()
        {
            string exePath = Application.ExecutablePath;

            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);

                key?.SetValue(AppName, $"\"{exePath}\"");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении в автозапуск: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public static void DisableAutoStart()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, true);

                key?.DeleteValue(AppName, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении из автозапуска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsAutoStartEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(RegistryKey, false);

            return key?.GetValue(AppName) != null;
        }

        public static void SetStartMinimized(bool value)
        {
            using var key = Registry.CurrentUser.CreateSubKey(MinimizedRegistryKey);

            key.SetValue(MinimizedValueName, value ? 1 : 0);
        }
        
        public static bool IsStartMinimizedEnabled()
        {
            using var key = Registry.CurrentUser.OpenSubKey(MinimizedRegistryKey);

            return key?.GetValue(MinimizedValueName, 0) is 1;
        }
    }
}