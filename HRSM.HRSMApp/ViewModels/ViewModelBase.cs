using HRSM.HRSMApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static HRSM.HRSMApp.MsgBoxWindow;

namespace HRSM.HRSMApp.ViewModels
{
        public class ViewModelBase : INotifyPropertyChanged
        {
                /// <summary>
                /// 属性值发生更改时触发
                /// </summary>
                public event PropertyChangedEventHandler PropertyChanged;

                /// <summary>
                /// 执行更改
                /// </summary>
                /// <param name="propertyName"></param>
                //protected void OnPropertyChanged(string propertyName)
                //{
                //    if (PropertyChanged != null)
                //        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                //}

                protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
                {
                        if (PropertyChanged != null)
                                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }

                /// <summary>
                /// 成功消息提示
                /// </summary>
                /// <param name="message"></param>
                /// <param name="title"></param>
                /// <param name="buttons"></param>
                public void ShowMessage(string message, string title = "提示", CustomMessageBoxButton buttons = CustomMessageBoxButton.OK)
                {
                        MsgBoxWindow.Show(message, title, buttons, CustomMessageBoxIcon.Infomation);
                }

                /// <summary>
                /// 错误消息框
                /// </summary>
                /// <param name="message"></param>
                /// <param name="title"></param>
                /// <param name="buttons"></param>
                public void ShowError(string message, string title = "错误", CustomMessageBoxButton buttons = CustomMessageBoxButton.OK)
                {
                        MsgBoxWindow.Show(message, title, buttons, CustomMessageBoxIcon.Error);
                }

                /// <summary>
                /// 询问消息框
                /// </summary>
                /// <param name="message"></param>
                /// <param name="title"></param>
                /// <param name="buttons"></param>
                /// <returns></returns>
                public CustomMessageBoxResult ShowQuestion(string message, string title = "询问", CustomMessageBoxButton buttons = CustomMessageBoxButton.OKCancel)
                {
                        return MsgBoxWindow.Show(message, title, buttons, CustomMessageBoxIcon.Question);
                }

                /// <summary>
                /// 模式化打开窗口页面
                /// </summary>
                /// <param name="key"></param>
                /// <param name="VM"></param>
                public void ShowDialog(string key, object VM)
                {
                        WindowManager.ShowWindow(key, VM, true);
                }

                /// <summary>
                /// 非模式化打开窗口页面
                /// </summary>
                /// <param name="key"></param>
                /// <param name="VM"></param>
                public void Show(string key, object VM)
                {
                        WindowManager.ShowWindow(key, VM, false);
                }

                /// <summary>
                /// 关闭窗口
                /// </summary>
                /// <param name="key"></param>
                /// <param name="win"></param>
                public void CloseWindow(string key, object win)
                {
                        WindowManager.CloseWindow(key, win);
                }

                /// <summary>
                /// 工具条中关闭项的命令
                /// </summary>
                public RelayCommand CloseTabPage
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        UserControl uc = o as UserControl;
                                        var mainWin = CommonUtility.GetAncestor<Window>(uc);
                                        MainViewModel mainVM = mainWin.DataContext as MainViewModel;
                                        mainVM.PageList.Remove(mainVM.SelectedPage);
                                });
                        }
                }

                /// <summary>
                /// 获取登录名
                /// </summary>
                /// <param name="uc"></param>
                /// <returns></returns>
                public string GetLoginUser(object uc)
                {
                        UserControl ucontrol = uc as UserControl;
                        var mainWin = CommonUtility.GetAncestor<Window>(ucontrol);
                        MainViewModel mainVM = mainWin.DataContext as MainViewModel;
                        string userName = mainVM.LoginUser;
                        return userName;
                }
        }
}
