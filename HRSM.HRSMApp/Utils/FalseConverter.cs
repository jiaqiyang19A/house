using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HRSM.HRSMApp.Utils
{
    public class FalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value!=null)
            {
                switch((bool)value)
                {
                    case true:
                        return System.Windows.Visibility.Visible;
                    case false:
                        return System.Windows.Visibility.Collapsed;
                    default:
                        return System.Windows.Visibility.Visible;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
