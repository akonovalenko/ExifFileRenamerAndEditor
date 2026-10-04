using ExifFileRenamer;
using System.Collections.Generic;

namespace ExifImageRenamer.Code
{
    /// <summary>
    /// Singleton class that holds the application state, including the list of image files and settings.
    /// </summary>
    public sealed class ApplicationState
    {
        #region Private fields
        private IList<IProcessingFileInfo> _imagesFiles;
        private readonly ISettingsProvider _settingsProvider;
        private static ApplicationState _instance;
        private static readonly object _instanceLock = new object();
        private int _fileTypesSelectedIndex;
        private Settings _settings;
        #endregion

        #region Constructors

        // The Singleton's constructor should always be private to prevent direct construction calls with the `new` operator.
        private ApplicationState() 
        {
            this._imagesFiles = new List<IProcessingFileInfo>();
            this._settingsProvider = new RegistrySettingsProvider();
            this._settings = this._settingsProvider.LoadSettings();
        }

        /// <summary>
        /// Gets the singleton instance of the ApplicationState class.
        /// </summary>
        /// <returns>The singleton instance of the ApplicationState class.</returns>
        public static ApplicationState GetInstance()
        {
            if (_instance == null)
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                        _instance = new ApplicationState();
                }
            }
            return _instance;
        }
        #endregion

        #region Properties
        internal IList<IProcessingFileInfo> ImagesFiles
        {
            get { 
                return this._imagesFiles; 
            }

            set { 
                this._imagesFiles = value; 
            }

        }

        public Settings Settings => _settings;

        public int FileTypesSelectedIndex { 
            get => _fileTypesSelectedIndex; 
            set => _fileTypesSelectedIndex = value; 
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Saves the provided settings using the settings provider and updates the current settings in the application state.
        /// </summary>
        /// <param name="settings">The settings to save.</param>
        public void SaveSettings(Settings settings)
        {
            this._settingsProvider.SaveSettings(settings);
            this._settings = settings;
        }
        #endregion

    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 