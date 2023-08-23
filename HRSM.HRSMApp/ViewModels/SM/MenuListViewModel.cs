using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
        public class MenuListViewModel : ListViewModelBase
        {
                MenuBLL menuBLL = new MenuBLL();

                public MenuListViewModel()
                {
                        this.MenuList = GetMenuList();
                        InitToolbarInfo();//初始化工具栏
                        FindMenuListCmd = new RelayCommand(o =>
                        {
                                ReloadMenuList();
                        });
                        this.CheckAllCmd = GetCheckAllCmd(this.MenuList);
                        this.CheckItemCmd = GetCheckItemCmd(this.MenuList);
                }


                /// <summary>
                /// 查询关键词
                /// </summary>
                private string keywords = "";
                public string KeyWords
                {
                        get { return keywords; }
                        set
                        {
                                keywords = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 菜单列表
                /// </summary>
                private ObservableCollection<MenuCheckInfo> menuList = new ObservableCollection<MenuCheckInfo>();
                public ObservableCollection<MenuCheckInfo> MenuList
                {
                        get
                        {
                                return menuList;
                        }
                        set
                        {
                                menuList = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 加载菜单列表
                /// </summary>
                /// <returns></returns>
                private ObservableCollection<MenuCheckInfo> GetMenuList()
                {
                        ObservableCollection<MenuCheckInfo> reList = new ObservableCollection<MenuCheckInfo>();
                        List<MenuInfo> list = menuBLL.GetMenuList(this.KeyWords, IsShowDel);
                        list.ForEach(m => reList.Add(new MenuCheckInfo()
                        {
                                IsCheck = false,
                                MenuInfo = m
                        }));
                        return reList;
                }

                /// <summary>
                /// 配置工具栏项
                /// </summary>
                private void InitToolbarInfo()
                {
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                AddCommand = new RelayCommand(o => ShowMenuInfoPage(1, 0, o), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowMenuInfoPage(2, o), b => IsEditable(this.MenuList)),
                                DeleteCommand = new RelayCommand(o => DeleteMenu(1, o, false), b => IsEditable(this.MenuList)),
                                RecoverCommand = new RelayCommand(o => DeleteMenu(0, o, false), b => IsDeletedable(this.MenuList)),
                                RemoveCommand = new RelayCommand(o => DeleteMenu(2, o, false), b => IsDeletedable(this.MenuList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowMenuInfoPage(4, o), b => this.MenuList.Count > 0)
                        };
                }

                /// <summary>
                /// 打开修改、查看、添加子项页面
                /// </summary>
                /// <param name="actType"></param>
                private void ShowMenuInfoPage(int actType, object uc)
                {
                        if (this.CurrentItem != null)
                        {
                                MenuCheckInfo curMenu = this.CurrentItem as MenuCheckInfo;
                                ShowMenuInfoPage(actType, curMenu.MenuInfo.MenuId, uc);
                        }
                        else
                        {
                                ShowError("请选择要查看的菜单");
                                return;
                        }
                }

                /// <summary>
                /// 打开菜单信息页面
                /// </summary>
                private void ShowMenuInfoPage(int actType, int mId, object uc)
                {
                        MenuInfoViewModel menuVM = new MenuInfoViewModel(actType, mId);
                        menuVM.ReloadListEvent += () =>
                        {
                                ReloadMenuList();
                                ReloadMainMenus(uc);
                        };
                        ShowDialog("menuInfo", menuVM);//打开菜单信息页面
                }

                /// <summary>
                /// 刷新主页导航菜单列表
                /// </summary>
                /// <param name="uc"></param>
                private void ReloadMainMenus(object uc)
                {
                        UserControl ucontrol = uc as UserControl;
                        var mainWin = CommonUtility.GetAncestor<Window>(ucontrol);
                        MainViewModel mainVM = mainWin.DataContext as MainViewModel;
                        mainVM.ReloadMainMenus();//刷新导航菜单
                }

                /// <summary>
                /// 刷新菜单列表页数据
                /// </summary>
                private void ReloadMenuList()
                {
                        this.MenuList = GetMenuList();
                        this.CheckAllCmd = GetCheckAllCmd(this.MenuList);
                        this.ToolBarInfo.OnCanExecuteChanged();
                }


                /// <summary>
                /// 单个删除/恢复/移除方法
                /// </summary>
                /// <param name="oId"></param>
                /// <param name="isDeleted"></param>
                private void SingleDelete(object o, int isDeleted)
                {
                        if (o != null)
                        {
                                object[] paras = o as object[];
                                int menuId = (int)paras[0];
                                MenuCheckInfo info = this.MenuList.Where(r => r.MenuInfo.MenuId == menuId).FirstOrDefault();
                                if (this.SelectedItem == null)
                                        this.SelectedItem = new RoleCheckInfo();
                                this.SelectedItem = info;
                                DeleteMenu(isDeleted, paras[1], true);
                        }
                }


                /// <summary>
                /// 删除/恢复/移除（彻底删除）
                /// </summary>
                /// <param name="isDeleted"></param>
                private void DeleteMenu(int isDeleted, object uc, bool isSingle)
                {
                        string typeName = GetDelTypeName(isDeleted);
                        string msgTitle = $"菜单{typeName}";
                        List<int> delIds = new List<int>();
                        if (isSingle)
                        {
                                MenuCheckInfo selInfo = this.SelectedItem as MenuCheckInfo;
                                delIds.Add(selInfo.MenuInfo.MenuId);
                        }
                        else
                        {
                                delIds = this.MenuList.Where(r => r.IsCheck == true).Select(r => r.MenuInfo.MenuId).ToList();
                        }
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        if (menuBLL.IsSystemMenu(delIds))
                                        {
                                                ShowError("系统管理菜单不能删除！", msgTitle);
                                                return;
                                        }
                                }
                                if (ShowQuestion($"您确定要{typeName}选择的菜单信息吗？", msgTitle) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool blDel = false;
                                        bool hasChild = menuBLL.HasChildMenus(delIds);
                                        switch (isDeleted)
                                        {
                                                case 1://假删除
                                                        if (!hasChild)
                                                                blDel = menuBLL.LogicDelMenuList(delIds);
                                                        else
                                                        {
                                                                ShowError("包含有已添加的子菜单，不能直接删除！", msgTitle);
                                                                return;
                                                        }
                                                        break;
                                                case 0://恢复
                                                        blDel = menuBLL.RecoverMenuList(delIds);
                                                        break;
                                                case 2://移除
                                                        blDel = menuBLL.RemoveMenuList(delIds);
                                                        break;
                                        }
                                        string sucMsg = blDel ? "成功" : "失败";
                                        string msg = $"选择的菜单信息{typeName}{sucMsg}！";
                                        if (blDel)
                                        {
                                                ShowMessage(msg, msgTitle);
                                                ReloadMenuList();
                                                ReloadMainMenus(uc);
                                        }
                                        else
                                        {
                                                ShowError(msg, msgTitle);
                                                return;
                                        }
                                }
                        }
                        else
                        {
                                ShowError($"没有要{typeName}的菜单信息！", msgTitle);
                                return;
                        }
                }



                #region  命令
                private ICommand findMenuListCmd;
                public ICommand FindMenuListCmd
                {
                        get { return findMenuListCmd; }
                        set
                        {
                                findMenuListCmd = value;
                                OnPropertyChanged();
                        }
                }


                public ICommand EditMenuCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        object[] paras = o as object[];
                                        int menuId = (int)paras[0];
                                        ShowMenuInfoPage(2, menuId, paras[1]);
                                });
                        }
                }


                public ICommand DelMenuCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        SingleDelete(o, 1);
                                });
                        }
                }

                public ICommand RecoverMenuCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        SingleDelete(o, 0);
                                });
                        }
                }

                public ICommand RemoveMenuCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        SingleDelete(o, 2);
                                });
                        }
                }

                public ICommand AddChildMenuCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        object[] paras = o as object[];
                                        int menuId = (int)paras[0];
                                        ShowMenuInfoPage(3, menuId, paras[1]);
                                });
                        }
                }
                #endregion
        }
}
