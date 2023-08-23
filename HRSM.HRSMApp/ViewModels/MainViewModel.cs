using HRSM.BLL;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static HRSM.HRSMApp.MsgBoxWindow;

namespace HRSM.HRSMApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 登录用户信息
        /// </summary>
        UserRoleIdsInfo logUserInfo = null;
        UserBLL userBLL = new UserBLL();
        MenuBLL menuBLL = new MenuBLL();
        public Action ReloadMainMenus = null;//刷新导航菜单列表

        public MainViewModel()
        {
            logUserInfo = new UserRoleIdsInfo();
            if (UserId == 0)
            {
                AddTabItem("房屋列表信息", defaultUrl);
                SetShowTabControlAndClose();
            }
            ReloadMainMenus = ReloadMenus;
        }

        /// <summary>
        /// 用户编号 
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 是否显示关闭图标
        /// </summary>
        private Visibility showCloseImg = Visibility.Hidden;
        public Visibility ShowCloseImg
        {
            get { return showCloseImg; }
            set
            {
                showCloseImg = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否显示TabControl
        /// </summary>
        private Visibility showPages = Visibility.Hidden;
        public Visibility ShowPages
        {
            get { return showPages; }
            set
            {
                showPages = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 当前选择页
        /// </summary>
        private TabItem selectedPage;
        public TabItem SelectedPage
        {
            get
            {
                return selectedPage;
            }
            set
            {
                selectedPage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 登录用户名
        /// </summary>
        private string logUser;
        public string LoginUser
        {
            get { return logUser; }
            set
            {
                logUser = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 登录角色名
        /// </summary>
        private string roleName;
        public string RoleName
        {
            get
            {
                return roleName;
            }
            set
            {
                roleName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        public string LoginTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

        }

        /// <summary>
        /// 版权信息
        /// </summary>
        public string RightInfo
        {
            get
            {
                return "朝夕教育所有";
            }

        }

        /// <summary>
        /// 是否显示登录
        /// </summary>
        private Visibility showLoginBtn = Visibility.Visible;
        public Visibility ShowLoginBtn
        {
            get
            {
                if (UserId > 0)
                    showLoginBtn = Visibility.Hidden;
                else
                    showLoginBtn = Visibility.Visible;
                return showLoginBtn;
            }
            set
            {
                showLoginBtn = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 页面状态 
        /// </summary>
        private WindowState winState = WindowState.Maximized;
        public WindowState WinState
        {
            get
            {
                return winState;
            }
            set
            {
                winState = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MenuItemModel> menuList = new ObservableCollection<MenuItemModel>();
        //TreeView的数据源
        public ObservableCollection<MenuItemModel> MenuList
        {
            get
            {
                if (logUserInfo.RoleIds == null)
                {
                    ObservableCollection<MenuItemModel> list = new ObservableCollection<MenuItemModel>();
                    list = new ObservableCollection<MenuItemModel>()
                    {
                        new MenuItemModel()
                        {
                            MenuName="房屋展示",
                            IsExpand=true,
                            SubMenuList=new ObservableCollection<MenuItemModel>()
                            {
                                new MenuItemModel()
                                {
                                    MenuName="房屋列表信息",
                                    MenuUrl=defaultUrl,
                                    IsExpand=false
                                }
                            }
                        }
                    };
                    menuList = list;
                }
                return menuList;
            }
            set
            {
                menuList = value;
                OnPropertyChanged();
            }
        }


        private string defaultUrl = "HS/HouseShowList.xaml";//默认显示页地址
        private ObservableCollection<TabItem> pageList = new ObservableCollection<TabItem>();
        //TabControl的数据源  
        public ObservableCollection<TabItem> PageList
        {
            get { return pageList; }
            set
            {
                pageList = value;
                SetShowTabControlAndClose();//处理TabControl与关闭图标的显示或隐藏
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 登录按钮命令
        /// </summary>
        public ICommand ShowLoginPage
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //打开登录页
                    LoginViewModel loginVM = new LoginViewModel();
                    ShowDialog("loginWindow", loginVM);  //还需要登录页的布局与功能---做了
                    if (loginVM.UserId > 0)
                    {
                        this.UserId = loginVM.UserId;
                    }
                    if (UserId > 0)
                    {
                        //登录成功，重新加载菜单列表  状态栏中的登录信息加载  登录按钮隐藏
                        logUserInfo = userBLL.GetUserRoleIdsInfo(UserId);
                        LoginUser = logUserInfo.UserName;
                        RoleName = logUserInfo.RoleName;
                        this.showLoginBtn = Visibility.Hidden;
                        this.MenuList = GetMenuList();

                    }
                });
            }
        }

        /// <summary>
        /// 退出命令
        /// </summary>
        public ICommand ExitCommand
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (o != null)
                    {
                        CustomMessageBoxResult result = ShowQuestion("您确定要退出房屋租售管理平台系统吗？", "系统退出", MsgBoxWindow.CustomMessageBoxButton.OKCancel);
                        if (result == CustomMessageBoxResult.OK)
                        {
                            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                            Application.Current.Shutdown();
                        }
                    }
                });
            }

        }

        /// <summary>
        /// 菜单响应命令
        /// </summary>
        public ICommand MenuItemCommand
        {
            get
            {
                return new RelayCommand(mi =>
                {
                    MenuItemModel menuItem = mi as MenuItemModel;
                    if (menuItem.SubMenuList.Count > 0)
                    {
                        menuItem.IsExpand = !menuItem.IsExpand;//与先前的折叠状态相反
                    }
                    else  //打开相应的页面
                    {
                        if (CheckHasPage(menuItem.MenuName))//当前页已打开
                        {
                            TabItem tabItem = this.PageList.Where(p => p.Header.ToString() == menuItem.MenuName).FirstOrDefault();
                            this.SelectedPage = tabItem;
                            return;
                        }
                        AddTabItem(menuItem.MenuName, menuItem.MenuUrl);
                    }
                });
            }

        }

        /// <summary>
        /// 关闭页命令
        /// </summary>
        public ICommand ClosePageCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    PageList.Remove(this.SelectedPage);
                    SetShowTabControlAndClose();
                });
            }

        }


        /// <summary>
        /// 设置TabControl与关闭图标是否显示
        /// </summary>
        private void SetShowTabControlAndClose()
        {
            if (this.PageList.Count > 0)
            {
                this.ShowPages = Visibility.Visible;
                this.ShowCloseImg = Visibility.Visible;
            }
            else
            {
                this.ShowPages = Visibility.Collapsed;
                this.ShowCloseImg = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 加载菜单数据
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<MenuItemModel> GetMenuList()
        {
            ObservableCollection<MenuItemModel> menus = new ObservableCollection<MenuItemModel>();
            List<MenuInfo> mList = menuBLL.GetRoleMenuList(logUserInfo.RoleIds);
            var firstMenus = mList.Where(m => m.ParentId == 0).ToList();
            foreach (MenuInfo fMenu in firstMenus)
            {
                var secondList = mList.Where(m => m.ParentId == fMenu.MenuId).ToList();
                MenuItemModel group = new MenuItemModel()
                {
                    MenuName = fMenu.MenuName,
                    IsExpand = true
                };
                foreach (var secMenu in secondList)
                {
                    MenuItemModel subMenu = new MenuItemModel();
                    subMenu.MenuName = secMenu.MenuName;
                    subMenu.IsExpand = false;
                    subMenu.MenuUrl = secMenu.MenuUrl;
                    group.SubMenuList.Add(subMenu);
                }
                menus.Add(group);
            }
            return menus;
        }

        /// <summary>
        /// 添加子页到TabControl中
        /// </summary>
        /// <param name="header"></param>
        /// <param name="url"></param>
        private void AddTabItem(string header, string url)
        {
            //判断页面文件的存在
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "../../" + url))
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = header;
                tabItem.Name = "tab" + url.Split('/')[1].Split('.')[0];
                tabItem.Style = (Style)Application.Current.Resources["TabItemStyle"];//应用TabItem样式
                Frame frame = new Frame();
                frame.Source = new Uri(url, UriKind.Relative);//要在其中显示用户控件xaml地址
                tabItem.Content = frame;
                this.PageList.Add(tabItem);
                this.SelectedPage = tabItem;
                SetShowTabControlAndClose();
            }

        }

        /// <summary>
        /// 判断页面是否已打开
        /// </summary>
        /// <param name="pageName"></param>
        /// <returns></returns>
        private bool CheckHasPage(string pageName)
        {
            foreach (TabItem tabItem in this.PageList)
            {
                if (tabItem.Header.ToString() == pageName)
                    return true;
            }
            return false;
        }

        private void ReloadMenus()
        {
            this.MenuList = GetMenuList();
        }

    }
}
