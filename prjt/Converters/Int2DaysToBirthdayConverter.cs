using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace prjt.Converters
{
    public class Int2DaysToBirthdayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int d = (int)value;
            if (d == 0) {
                return "Slaví narozeniny právě dnes!";
            }
            if (d == 1) {
                return "Má narozeniny již zítra!";
            }
            return string.Format("{0} {1} {2} do narozenin", d > 1 && d < 5 ? "Zbývají" : "Zbývá", d, d > 1 && d < 5 ? "dny" : "dní");
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
