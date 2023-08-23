using HRSM.BLL;
using HRSM.HRSMApp.Models;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class MenuInfoViewModel : InfoViewModelBase
    {
        MenuInfo menuInfo = new MenuInfo();
        MenuBLL menuBLL = new MenuBLL();
        private string oldName = "";//修改前的菜单名称
        public MenuInfoViewModel(int actType, int menuId)
        {
            this.ActType = actType;
            this.MenuId = menuId;
            this.IsConfirmBtnEnabled = true;
            this.CloseWindowCmd = GetCloseWindowCmd("menuInfo");
            switch (ActType)
            {
                case 1:
                    this.ConfirmBtnContent = "新增";
                    this.MOrder = 1;
                    break;
                case 2:
                    this.menuInfo = menuBLL.GetMenuInfo(menuId);
                    this.oldName = this.menuInfo.MenuName;
                    this.ConfirmBtnContent = "修改";

                    break;
                case 3:
                    this.MOrder = 1;
                    this.ParentId = menuId;
                    this.MenuId = 0;
                    this.oldName = "";
                    this.IsParentEnabled = false;
                    this.ConfirmBtnContent = "添加";
                    break;
                case 4:
                    this.menuInfo = menuBLL.GetMenuInfo(menuId);
                    this.ConfirmBtnVisible = Visibility.Hidden;
                    break;
            }
        }


        public int MenuId
        {
            get
            {
                return menuInfo.MenuId;
            }
            set
            {
                menuInfo.MenuId = value;
            }
        }

     
        public string MenuName
        {
            get
            {
                return menuInfo.MenuName;
            }
            set
            {
                menuInfo.MenuName = value;
                OnPropertyChanged();
            }
        }

 
        public string MenuUrl
        {
            get
            {
                return menuInfo.MenuUrl;
            }
            set
            {
                menuInfo.MenuUrl = value;
                OnPropertyChanged();
            }
        }

    
        public int ParentId
        {
            get { return menuInfo.ParentId; }
            set
            {
                menuInfo.ParentId = value;
                OnPropertyChanged();
            }
        }

  
        public int MOrder
        {
            get
            {
                return menuInfo.MOrder;
            }
            set
            {
                menuInfo.MOrder = value;
                OnPropertyChanged();
            }
        }

      
        public List<MenuInfo> ParentList
        {
            get
            {
                List<MenuInfo> menuList = menuBLL.GetAllMenus();
                menuList.Add(new MenuInfo()
                {
                    MenuId = 0,
                    MenuName = "--请选择父菜单--"
                });
                return menuList;
            }
        }


        private bool isParentEnabled = true;
        public bool IsParentEnabled
        {
            get { return isParentEnabled; }
            set
            {
                isParentEnabled = value;
                OnPropertyChanged();
            }
        }

 
        public List<PageInfo> CboPages
        {
            get
            {
                Assembly ass = Application.ResourceAssembly;
                int length = ass.GetName().Name.Length;
                List<PageInfo> types = ass.GetTypes().Where(t => t.BaseType == typeof(UserControl) && !t.FullName.Contains("UControls")).Select(t => new PageInfo()
                {
                    Name = t.Name,
                    FullName = t.FullName.Substring(length + 1).Replace('.', '/') + ".xaml"
                }).ToList();
                types.Insert(0, new PageInfo()
                {
                    Name = "请选择",
                    FullName = ""
                });
                return types;
            }

        }

        public ICommand ConfirmCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    string actMsg = ActType == 2 ? "修改" : "添加";
                    string msgTitle = $"菜单{actMsg}页面";
                                    //检查输入
                     if (string.IsNullOrEmpty(this.MenuName))
                    {
                        ShowError("请输入菜单名称！", msgTitle);
                        return;
                    }
                    else if (this.MenuId == 0 || (this.MenuId > 0 && this.MenuName != this.oldName))
                    {
                        if (menuBLL.ExistsMenu(this.MenuName))
                        {
                            ShowError("该菜单名已存在！", msgTitle);
                            return;
                        }
                    }
                    bool bl = false;
                                    
                       if (this.ActType == 2)
                        bl = menuBLL.UpdateMenuInfo(this.menuInfo);
                    else
                        bl = menuBLL.AddMenuInfo(this.menuInfo);
                    string sucType = bl ? "成功" : "失败";
                    string msgInfo = $"菜单：{this.MenuName} {actMsg}{sucType}!";
                    if (bl)
                    {
                        ShowMessage(msgInfo, msgTitle);
                        InvokeReload();
                      }
                    else
                    {
                        ShowError(msgInfo, msgTitle);
                        return;
                    }
                });
            }
        }


    }
}
