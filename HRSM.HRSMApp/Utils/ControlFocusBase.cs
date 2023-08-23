using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HRSM.HRSMApp.Utils
{
    public class TextBoxFocus : ControlFocusBase<TextBox> { }
    public class PasswordBoxFocus : ControlFocusBase<PasswordBox> { }
    /// <summary>
    ///  控件的附加IsFocus属性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ControlFocusBase<T> where T:Control
    {
        public static readonly DependencyProperty IsFocusProperty = DependencyProperty.RegisterAttached("IsFocus", typeof(bool), typeof(ControlFocusBase<T>), new PropertyMetadata(OnIsFocusChanged));

        /// <summary>
        /// IsFocus值改变后回调
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnIsFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as T;
            if (control == null)
                return;
            if (e.NewValue is bool ? (bool)e.NewValue : false)
                control.Focus();
        }

        public static bool GetIsFocus(T t)
        {
            return t.GetValue(IsFocusProperty) is bool ? (bool)t.GetValue(IsFocusProperty) : false;
        }

        public static void SetIsFocus(T t,bool value)
        {
            t.SetValue(IsFocusProperty, value);
        }
    }
}
