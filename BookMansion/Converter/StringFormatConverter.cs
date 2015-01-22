using System;
using Windows.UI.Xaml.Data;

namespace BookMansion.Converter
{
    public class StringFormatConverter : IValueConverter
    {
        #region > Public Method

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
            {
                return value;
            }

            return String.Format((String)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        #endregion
    }
}
