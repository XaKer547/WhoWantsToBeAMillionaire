using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace WhoWantsToBeAMillionaire.Converters
{
    public class LevelTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isFixed = (bool)values[1];
            bool isCompleted = (bool)values[0];

            if (isCompleted)
                return new SolidColorBrush(Convert(Color.LightGreen));

            if (isFixed)
                return new SolidColorBrush(Convert(Color.White));

            return new SolidColorBrush(Convert(Color.Orange));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private System.Windows.Media.Color Convert(Color color)
        {
            return new System.Windows.Media.Color()
            {
                A = color.A,
                B = color.B,
                G = color.G,
                R = color.R,
            };
        }
    }
}
