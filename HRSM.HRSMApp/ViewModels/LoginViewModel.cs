using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels
{
    public  class LoginViewModel:ViewModelBase
    {
        UserBLL userBLL = new UserBLL();
        UserInfo userInfo;
        public LoginViewModel()
        {
            userInfo = new UserInfo();
        }
        /// <summary>
        /// 登录者的Id
        /// </summary>
        private int userId;
        public int UserId
        {
            get { return userId; }
            set { userId = value;
            }
        }
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName
        {
            get
            {
                return userInfo.UserName;
            }
            set
            {
                userInfo.UserName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd
        {
            get { return userInfo.UserPwd; }
            set
            {
                userInfo.UserPwd = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 账号文本框Focus
        /// </summary>
        private bool  userNameFocus=true;
        public bool UserNameFocus
        {
            get { return userNameFocus; }
            set { userNameFocus = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 密码框Focus
        /// </summary>
        private bool userPwdFocus = false;
        public bool UserPwdFocus
        {
            get { return userPwdFocus; }
            set
            {
                userPwdFocus = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 登录按钮的可用性
        /// </summary>
        private bool isLogin=true;
        public bool IsLogin
        {
            get { return isLogin; }
            set { isLogin = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 切换焦点
        /// </summary>
        public ICommand CheckIsFocused
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if(o!=null)
                    {
                        string p = o.ToString();
                        if(p=="T")
                        {
                            SetFocused(true);
                        }
                        else
                        {
                            SetFocused(false);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 检查是否可登录
        /// </summary>
        public ICommand CheckIsLogin
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (string.IsNullOrEmpty(UserPwd) || string.IsNullOrEmpty(UserName) )
                        this.IsLogin = false;
                    else if ((!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserPwd)) || (string.IsNullOrEmpty(UserName) && string.IsNullOrEmpty(UserPwd)))
                        this.IsLogin = true;
                });
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                      if(!string.IsNullOrEmpty(UserName)&&!string.IsNullOrEmpty(UserPwd))
                    {
                        //登录检验---账号与密码是否输入正确-----是否有这么一条用户数据，返回id; 返回0 

                        int id = 0;
                        id = userBLL.UserLogin(userInfo);
                        if(id==-1)//用户存在，但状态为冻结---不能登录
                        {
                            ShowError("该账号已冻结，不能登录系统！", "登录错误");
                            SetFocused(true);
                            return;
                        }
                        else if(id==0)
                        {
                            ShowError("登录账号或密码输入有误！", "登录错误");
                            SetFocused(false);
                            return;
                        }
                        if(id>0)
                        {
                            this.UserId = id;//登录者UserId
                            CloseWindow("loginWindow", o);//关闭登录页
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 设置控件的Focused
        /// </summary>
        /// <param name="bl"></param>
        private void SetFocused(bool bl)
        {
            this.UserNameFocus = bl;
            this.UserPwdFocus = !bl;
        }
    }
}
