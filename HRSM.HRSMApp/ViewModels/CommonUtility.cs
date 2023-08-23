using HRSM.HRSMApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HRSM.HRSMApp.ViewModels
{
    public class CommonUtility
    {
        /// <summary>
        /// 租售类型
        /// </summary>
        public enum RSTypes
        {
            出租,
            出售
        }

        /// <summary>
        /// 获取出售类型列表
        /// </summary>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public static List<string> GetRSTypes(bool isDefault)
        {
            List<string> list = new List<string>();
            if (isDefault)
                list.Add("请选择");
            foreach (string str in Enum.GetNames(typeof(RSTypes)))
            {
                list.Add(str);
            }
            return list;
        }

        /// <summary>
        /// 获取房屋朝向列表
        /// </summary>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public static List<string> GetHouseDirectionList(bool isDefault)
        {
            List<string> list = new List<string>();
            if (isDefault)
                list.Add("请选择");
            list.Add("坐南朝北");
            list.Add("坐北朝南");
            list.Add("坐东朝西");
            list.Add("坐西朝东");
            return list;
        }

        /// <summary>
        /// 获取子元素的父类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T GetAncestor<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (!(parent is T) && parent != null)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            if (parent != null)
                return (T)parent;
            else
                return null;
        }

        /// <summary>
        /// 将用户控件添加到TabControl中的方法
        /// </summary>
        /// <param name="uc"></param>
        /// <param name="header"></param>
        /// <param name="key"></param>
        /// <param name="VM"></param>
        public static void AddUCTabItem(UserControl uc, string header, string key, object VM)
        {
            var mainWin = CommonUtility.GetAncestor<Window>(uc);//主页面
            MainViewModel mainVM = mainWin.DataContext as MainViewModel;//主页面的ViewModel
            TabItem tabItem0 = mainVM.PageList.Where(p => p.Header.ToString() == header).FirstOrDefault();//查找是否存在相同的页面
            Frame frame = new Frame();
            if (tabItem0 != null)
            {
                frame = tabItem0.Content as Frame;
                UserControl instance = frame.Content as UserControl;
                instance.DataContext = VM;
                mainVM.SelectedPage = tabItem0;
            }
            else
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = header;
                tabItem.Name = "tab" + key;
                tabItem.Style = (Style)Application.Current.Resources["TabItemStyle"];//应用TabItem样式
                object page = WindowManager.CreateUCInstance(key, VM);//创建用户控件的实例
                frame.Content = page;
                tabItem.Content = frame;
                mainVM.PageList.Add(tabItem);
                mainVM.SelectedPage = tabItem;
            }
        }
    }
}
