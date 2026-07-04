using Microsoft.Win32;

namespace ExifFileRenamer
{
    /// <summary>
    /// Windows registry settings provider 
    /// </summary>
    /// <author>Alexey Konovalenko</created>
    /// <created>18/02/2009</created>
    /// <version>1.1.0.0</version>
    public class RegistrySettingsProvider : ISettingsProvider
    {

        #region Constants

        private const string APP_WIDTH = "Width";
        private const string APP_HEIGHT = "Height";
        private const string APP_TOP = "Top";
        private const string APP_LEFT = "Left";
        private const string APP_LAST_BROWSED_FOLDER = "LastBrowsedFolder";
        private const string FILE_TYPES_SELECTED_INDEX = "FileTypesSelectedIndex";

        private const int APP_DEFAULT_WIDTH = 720;
        private const int APP_DEFAULT_HEIGHT = 500;
        private const int APP_DEFAULT_TOP = 100;
        private const int APP_DEFAULT_LEFT = 100;
        private const int FILE_TYPES_SELECTED_INDEX_DEFAULT = 1;

        #endregion

        #region Public methods

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void SaveSettings(Settings settings)
        {

            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Constants.SOFTWARE_KEY, true);
            RegistryKey appKey = rk.CreateSubKey(Constants.APP_SOFT_KEY);
            try
            {
                appKey.SetValue(APP_WIDTH, settings.Width);
                appKey.SetValue(APP_HEIGHT, settings.Height);
                appKey.SetValue(APP_TOP, settings.Top);
                appKey.SetValue(APP_LEFT, settings.Left);
                appKey.SetValue(APP_LAST_BROWSED_FOLDER, settings.SelectedPath);
                appKey.SetValue(FILE_TYPES_SELECTED_INDEX, settings.FileTypesSelectedIndex);
            }
            finally
            {
                appKey.Close();
                rk.Close();
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        public Settings LoadSettings()
        {
            var result = new Settings();
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(Constants.SOFTWARE_KEY, true);
            RegistryKey appKey = rk.OpenSubKey(Constants.APP_SOFT_KEY);

            if (appKey == null)
            {
                return null;
            };

            try
            {
                result.Width = int.Parse(appKey.GetValue(APP_WIDTH, APP_DEFAULT_WIDTH).ToString());
                result.Height = int.Parse(appKey.GetValue(APP_HEIGHT, APP_DEFAULT_HEIGHT).ToString());
                result.Top = int.Parse(appKey.GetValue(APP_TOP, APP_DEFAULT_TOP).ToString());
                result.Left = int.Parse(appKey.GetValue(APP_LEFT, APP_DEFAULT_LEFT).ToString());
                result.SelectedPath = appKey.GetValue(APP_LAST_BROWSED_FOLDER, string.Empty).ToString();
                result.FileTypesSelectedIndex = int.Parse(appKey.GetValue(FILE_TYPES_SELECTED_INDEX, FILE_TYPES_SELECTED_INDEX_DEFAULT).ToString());
            }
            finally
            {
                appKey.Close();
                rk.Close();
            }
            return result;
        }

        #endregion
    }
}
