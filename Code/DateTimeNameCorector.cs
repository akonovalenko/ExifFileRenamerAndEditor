using ExifFileRenamer.Model;
using System;

namespace ExifFileRenamer
{
    // temporary solution
    /// <summary>
    /// Adjust date time class
    /// </summary>
    /// <author>Alexey Konovalenko, aldev@ukr.net</author>
    /// version 1.1.0.0  25-apr-2009
    class DateTimeNameCorector
    {

        public string ModelNameExpr;
        public int TimeDiffMin;

        public DateTime GetCorectedTime(ExifInfo efh)
        {
            if (string.Compare(efh.ModelName, ModelNameExpr, true) == 0)
            {
                return efh.OriginalDateTime.AddMinutes(TimeDiffMin);
            }
            else return efh.OriginalDateTime;
        }
    }

}
