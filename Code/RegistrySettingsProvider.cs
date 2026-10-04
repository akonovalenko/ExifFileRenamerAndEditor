using Microsoft.Win32;
using System;

namespace ExifFileRenamer
{
    /// <summary>
    /// Windows registry settings provider 
    /// </summary>
    /// <author>Alexey Konovalenko</created>
    /// <created>18/02/2009</created>
    /// <version>1.1.1.0</version>
    public sealed class RegistrySettingsProvider : ISettingsProvider
    {
        #region Private members
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

        /// <summary>
        /// Saves the application settings to the Windows registry.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void SaveSettings(Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            using var root = Registry.CurrentUser.CreateSubKey(Constants.SOFTWARE_KEY);
            using var appKey = root?.CreateSubKey(Constants.APP_SOFT_KEY);
            if (appKey == null)
                throw new InvalidOperationException("Unable to open application settings registry key.");

            appKey.SetValue(APP_WIDTH, settings.Width);
            appKey.SetValue(APP_HEIGHT, settings.Height);
            appKey.SetValue(APP_TOP, settings.Top);
            appKey.SetValue(APP_LEFT, settings.Left);
            appKey.SetValue(APP_LAST_BROWSED_FOLDER, settings.SelectedPath ?? string.Empty);
            appKey.SetValue(FILE_TYPES_SELECTED_INDEX, settings.FileTypesSelectedIndex);
        }

        /// <summary>
        /// Loads the application settings from the Windows registry. If the settings are not found, default values are returned.
        /// </summary>
        /// <returns>The loaded settings.</returns>
        public Settings LoadSettings()
        {
            using var root = Registry.CurrentUser.OpenSubKey(Constants.SOFTWARE_KEY);
            using var appKey = root?.OpenSubKey(Constants.APP_SOFT_KEY);
            if (appKey == null)
                return CreateDefaultSettings();

            return new Settings
            {
                Width = ReadInt(appKey, APP_WIDTH, APP_DEFAULT_WIDTH),
                Height = ReadInt(appKey, APP_HEIGHT, APP_DEFAULT_HEIGHT),
                Top = ReadInt(appKey, APP_TOP, APP_DEFAULT_TOP),
                Left = ReadInt(appKey, APP_LEFT, APP_DEFAULT_LEFT),
                SelectedPath = ReadString(appKey, APP_LAST_BROWSED_FOLDER),
                FileTypesSelectedIndex = ReadInt(appKey, FILE_TYPES_SELECTED_INDEX, FILE_TYPES_SELECTED_INDEX_DEFAULT)
            };
        }

        /// <summary>
        /// Creates a new instance of Settings with default values.
        /// </summary>
        /// <returns>The default settings.</returns>
        private static Settings CreateDefaultSettings() => new Settings
        {
            Width = APP_DEFAULT_WIDTH,
            Height = APP_DEFAULT_HEIGHT,
            Top = APP_DEFAULT_TOP,
            Left = APP_DEFAULT_LEFT,
            SelectedPath = string.Empty,
            FileTypesSelectedIndex = FILE_TYPES_SELECTED_INDEX_DEFAULT
        };

        /// <summary>
        /// Reads an integer value from the specified registry key. If the value is not found or cannot be parsed, the provided default value is returned.
        /// </summary>
        /// <param name="key">The registry key.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <param name="defaultValue">The default value to return if the specified value is not found or cannot be parsed.</param>
        /// <returns>The read value or the default value.</returns>

        private static int ReadInt(RegistryKey key, string name, int defaultValue)
        {
            return int.TryParse(key.GetValue(name)?.ToString(), out var value) ? value : defaultValue;
        }

        /// <summary>
        /// Reads a string value from the specified registry key. If the value is not found, an empty string is returned.
        /// </summary>
        /// <param name="key">The registry key.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <returns>The read value or an empty string.</returns>
        private static string ReadString(RegistryKey key, string name)
        {
            return key.GetValue(name)?.ToString() ?? string.Empty;
        }
    }
}
