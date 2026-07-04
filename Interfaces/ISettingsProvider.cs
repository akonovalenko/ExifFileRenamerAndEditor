namespace ExifFileRenamer
{
    /// <summary>
    /// Provide user application settings
    /// </summary>
    /// <author>Alexey Konovalenko</created>
    /// <created>03/02/2008</created>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Loads the user settings
        /// </summary>
        /// <returns><see cref="Settings"/></returns>
        Settings LoadSettings();

        /// <summary>
        /// Save the user settings
        /// </summary>
        /// <param name="settings"><see cref="Settings"/></param>
        void SaveSettings(Settings settings);

    }
}