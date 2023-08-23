using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HRSM.HRSMApp.Utils
{
    public class ObjectConverter : IMultiValueConverter
    {
        //public static object ConverterObject;
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
           // ConverterObject = values;
            //string str = values.GetType().ToString();
            return values.ToArray();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
