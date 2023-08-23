using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HRSM.HRSMApp
{
    /// <summary>
    /// MsgBoxWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MsgBoxWindow : Window
    {
        public MsgBoxWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            //所有按钮不显示（）
            OkButtonVisibility = Visibility.Collapsed;
            CancelButtonVisibility = Visibility.Collapsed;
            YesButtonVisibility = Visibility.Collapsed;
            NoButtonVisibility = Visibility.Collapsed;
            Result = CustomMessageBoxResult.None;
        }

        /// <summary>
        /// 显示按钮类型
        /// </summary>
        public enum CustomMessageBoxButton
        {
            OK=1,
            OKCancel=2,
            YesNo=3,
            YesNoCancel=4
        }

        /// <summary>
        /// 消息框返回值
        /// </summary>
        public enum  CustomMessageBoxResult
        {
            None=0,//用户直接关闭消息框
            OK=1,//用户点击了确定按钮
            Cancel=2,//用户点击了取消按钮
            Yes=3,//用户点击了是按钮
            No=4//用户点击了否按钮
        }

        /// <summary>
        /// 图标类型
        /// </summary>
        public enum CustomMessageBoxIcon
        {
            None=0,
            Error=1,
            Question=2,
            Infomation =3
        }

        #region 页面属性定义
        /// <summary>
        /// 消息文本
        /// </summary>
          public string MessageBoxText { get; set; }
        /// <summary>
        /// 消息框标题
        /// </summary>
        public string MessageTitle { get; set; }
        /// <summary>
        /// 图标路径
        /// </summary>
          public string ImagePath { get; set; }
        /// <summary>
        /// 显示确定
        /// </summary>
          public Visibility OkButtonVisibility { get; set; }
        /// <summary>
        /// 显示取消
        /// </summary>
        public Visibility CancelButtonVisibility { get; set; }
        /// <summary>
        /// 显示是
        /// </summary>
        public Visibility YesButtonVisibility { get; set; }
        /// <summary>
        /// 显示否
        /// </summary>
        public Visibility NoButtonVisibility { get; set; }
        /// <summary>
        /// 消息框返回值
        /// </summary>
        public CustomMessageBoxResult Result { get; set; }
        #endregion

        /// <summary>
        /// 消息框拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="msgText"></param>
        /// <param name="title"></param>
        /// <param name="msgBtn"></param>
        /// <param name="msgIcon"></param>
        /// <returns></returns>
        public static CustomMessageBoxResult Show(string msgText,string title,CustomMessageBoxButton msgBtn,CustomMessageBoxIcon msgIcon)
        {
            MsgBoxWindow msg = new MsgBoxWindow();
            msg.Topmost = true;
            msg.MessageBoxText = msgText;
            msg.MessageTitle = title;
            //消息框按钮显示
            switch(msgBtn)
            {
                case CustomMessageBoxButton.OK:
                    msg.OkButtonVisibility = Visibility.Visible;
                    break;
                case CustomMessageBoxButton.OKCancel:
                    msg.OkButtonVisibility = Visibility.Visible;
                    msg.CancelButtonVisibility = Visibility.Visible;
                    break;
                case CustomMessageBoxButton.YesNo:
                    msg.YesButtonVisibility = Visibility.Visible;
                    msg.NoButtonVisibility = Visibility.Visible;
                    break;
                case CustomMessageBoxButton.YesNoCancel:
                    msg.YesButtonVisibility = Visibility.Visible;
                    msg.NoButtonVisibility = Visibility.Visible;
                    msg.CancelButtonVisibility = Visibility.Visible;
                    break;
                default:
                    msg.OkButtonVisibility = Visibility.Visible;
                    break;
            }
            switch(msgIcon)
            {
                case CustomMessageBoxIcon.Infomation:
                    msg.ImagePath = @"imgs/success.jpg";
                    break;
                case CustomMessageBoxIcon.Error:
                    msg.ImagePath = @"imgs/error.jpg";
                    break;
                case CustomMessageBoxIcon.Question:
                    msg.ImagePath = @"imgs/question.jpg";
                    break;
            }
            msg.ShowDialog();
            return msg.Result;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.OK;
            this.Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.Yes;
            this.Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.No;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = CustomMessageBoxResult.Cancel;
            this.Close();
        }
    }
}
