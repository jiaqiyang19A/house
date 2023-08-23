using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HRSM.HRSMApp.Utils
{
    public static  class WindowExt
    {
        /// <summary>
        /// 注册页面  泛型方法
        /// </summary>
        /// <typeparam name="T">要注册的页面类型</typeparam>
        /// <param name="win">所在的页面的实例</param>
        /// <param name="key"></param>
        public static void Register<T>(this Window win,string key)
        {
            WindowManager.Register<T>(key);
        }

        /// <summary>
        /// 注册页面  Type对象
        /// </summary>
        /// <param name="win"></param>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public static void Register(this Window win, string key,Type type)
        {
            WindowManager.Register(key,type);
        }

        /// <summary>
        /// 在用户控件页面注册页面
        /// </summary>
        /// <typeparam name="T">要注册的页面类型</typeparam>
        /// <param name="uc"></param>
        /// <param name="key"></param>
        public static void Register<T>(this UserControl uc, string key)
        {
            WindowManager.Register<T>(key);
        }

        public static void Register(this UserControl uc, string key,Type type)
        {
            WindowManager.Register(key,type);
        }
    }
}
