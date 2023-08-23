using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels
{
    public class InfoViewModelBase : ViewModelBase
    {
        public event Action ReloadListEvent;//刷新列表页数据事件
        /// <summary>
        /// 调用事件
        /// </summary>
        public void InvokeReload()
        {
            ReloadListEvent?.Invoke();
        }

        private int actType;
        /// <summary>
        /// 页面类别标识
        /// </summary>
        public int ActType
        {
            get { return actType; }
            set
            {
                actType = value;
            }
        }

        private string confirmBtnContent;
        /// <summary>
        /// 提交按钮文本
        /// </summary>
        public string ConfirmBtnContent
        {
            get { return confirmBtnContent; }
            set
            {
                confirmBtnContent = value;
                OnPropertyChanged();
            }
        }


        private Visibility confirmBtnVisible;
        /// <summary>
        /// 提交按钮显示
        /// </summary>
        public Visibility ConfirmBtnVisible
        {
            get { return confirmBtnVisible; }
            set
            {
                confirmBtnVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 提交按钮是否可用
        /// </summary>
        private bool isConfirmBtnEnabled = false;
        public bool IsConfirmBtnEnabled
        {
            get { return isConfirmBtnEnabled; }
            set
            {
                isConfirmBtnEnabled = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// 关闭窗口命令
        /// </summary>
        private ICommand closeWindowCmd;
        public ICommand CloseWindowCmd
        {
            get
            {
                return closeWindowCmd;
            }
            set
            {
                closeWindowCmd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 关闭命令
        /// </summary>
        public ICommand GetCloseWindowCmd(string key)
        {
            return new RelayCommand(o =>
            {
                CloseWindow(key, o);
            });
        }

    }
}
