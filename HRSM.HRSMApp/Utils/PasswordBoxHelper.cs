using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HRSM.HRSMApp.Utils
{
    public class PasswordBoxHelper
    {
        //附加属性  Attach   Password  IsUpdate  声明与注册
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, AttachChanged));
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnPasswordChanged));
        public static readonly DependencyProperty IsUpdateProperty = DependencyProperty.RegisterAttached(" IsUpdate", typeof(bool), typeof(PasswordBoxHelper));
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pbox = d as PasswordBox;
            pbox.PasswordChanged -= PasswordChanged;
            if (!GetIsUpdate(pbox))
                pbox.Password = e.NewValue==null?"":e.NewValue.ToString();
            pbox.PasswordChanged += PasswordChanged;
        }

     
        private static void AttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox pbox = d as PasswordBox;
            if (pbox == null)
                return;
            if ((bool)e.OldValue)
                pbox.PasswordChanged -= PasswordChanged;
            if ((bool)e.NewValue)
                pbox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pbox = sender as PasswordBox;
            SetIsUpdate(pbox, true);
            SetPassword(pbox, pbox.Password);
            SetIsUpdate(pbox, false);
        }


        public static void SetAttach(DependencyObject dp,bool value)
        {
            dp.SetValue(AttachProperty, value);
        }
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }
        public static string GetPassword(DependencyObject dp)
        {
            return dp.GetValue(PasswordProperty).ToString();
        }

        public static void SetIsUpdate(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdateProperty, value);
        }
        public static bool GetIsUpdate(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdateProperty);
        }

    }
}
