using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HRSM.HRSMApp.Utils
{
    public  class WindowManager
    {
        private static Hashtable registerWindow = new Hashtable();//注册的页面  键值对

        /// <summary>
        /// 泛型方法注册页面（UserControl/Window）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public static void Register<T>(string key)
        {
            if (!registerWindow.Contains(key))
                registerWindow.Add(key, typeof(T));
        }
        /// <summary>
        /// 注册页面（Type对象）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        public static void Register(string key,Type type)
        {
            if (!registerWindow.Contains(key))
                registerWindow.Add(key, type);
        }

        /// <summary>
        /// 移除键
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            if (registerWindow.Contains(key))
                registerWindow.Remove(key);
        }

        /// <summary>
        /// 打开窗口页面
        /// </summary>
        /// <param name="key"></param>
        /// <param name="VM"></param>
        /// <param name="isDialog"></param>
        public static void ShowWindow(string key,object VM,bool isDialog)
        {
            if(!registerWindow.Contains(key))
            {
                throw (new Exception("没有注册此键！"));
            }
            var win = (Window)Activator.CreateInstance((Type)registerWindow[key]);//创建窗口对象
            win.DataContext = VM;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (isDialog)
                win.ShowDialog();
            else
                win.Show();
        }

        /// <summary>
        /// 打开窗口页面  传递另外的参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="VM"></param>
        /// <param name="para"></param>
        /// <param name="isDialog"></param>
        public static void ShowWindow(string key, object VM,object para, bool isDialog)
        {
            if (!registerWindow.Contains(key))
            {
                throw (new Exception("没有注册此键！"));
            }
            var win = (Window)Activator.CreateInstance((Type)registerWindow[key]);//创建窗口对象
            win.DataContext = VM;
            if (para != null)
                win.Tag = para;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if (isDialog)
                win.ShowDialog();
            else
                win.Show();
        }

        /// <summary>
        /// 关闭窗口页面
        /// </summary>
        /// <param name="key"></param>
        /// <param name="win"></param>
        public static void CloseWindow(string key,object  win)
        {
            if (!registerWindow.Contains(key))
            {
                throw (new Exception("没有注册此键！"));
            }
            if(win!=null)
            {
                Window window = win as Window;
                window.Close();
            }
        }

        /// <summary>
        /// 创建用户控件的实例（子页显示在TabItem）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="VM"></param>
        /// <returns></returns>
        public static object CreateUCInstance(string key,object VM)
        {
            if (!registerWindow.Contains(key))
            {
                throw (new Exception("没有注册此键！"));
            }
            Type type = (Type)registerWindow[key];
            var instance = (UserControl)Activator.CreateInstance(type);
            //设置DataContext
            PropertyInfo dataPro = type.GetProperty("DataContext");
            dataPro.SetValue(instance, VM);//设置属性值
            return instance;
        }


    }
}
