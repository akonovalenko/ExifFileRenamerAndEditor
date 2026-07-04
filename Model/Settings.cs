using System;
using System.Collections.Generic;
using System.Text;

namespace ExifFileRenamer
{
    /// <summary>
    /// Application state settings 
    /// </summary>
    public class Settings
    {
        #region Public properties

        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public int Top { get; internal set; }
        public int Left { get; internal set; }
        public string SelectedPath { get; internal set; }
        public int FileTypesSelectedIndex { get; internal set; }

        #endregion
    }
}
