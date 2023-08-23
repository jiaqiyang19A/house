using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class RightSetViewModel:ViewModelBase
    {
        RoleBLL roleBLL = new RoleBLL();
        MenuBLL menuBLL = new MenuBLL();
        public RightSetViewModel()
        {
            this.CboRoleList = GetCboRoleList();
            this.MenuList = GetMenuList();
        }
        public RightSetViewModel(int roleId)
        {
            this.CboRoleList = GetCboRoleList();
            this.RoleId = roleId;
            this.MenuList = GetMenuList();
            if(RoleId>0)
            {
                this.CboEnable = false;
                LoadHasRightSet();
            }
        }

      

        private int roleId;
        /// <summary>
        /// 当前角色编号
        /// </summary>
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<TreeMenuItem> menuList;
        /// <summary>
        /// 菜单列表
        /// </summary>
        public ObservableCollection<TreeMenuItem> MenuList
        {
            get { return menuList; }
            set { menuList = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<RoleInfo> CboRoleList { get; set; }

        private bool cboEnable=true;
        /// <summary>
        /// 角色下拉框可用
        /// </summary>
        public bool CboEnable
        {
            get { return cboEnable; }
            set { cboEnable = value;
                OnPropertyChanged();
            }
        }

        private bool isSave=true;
        /// <summary>
        /// 是否可保存
        /// </summary>
        public bool IsSave
        {
            get { return isSave; }
            set { isSave = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否全选
        /// </summary>
        private bool isCheckAll = false;
        public bool IsCheckAll
        {
            get { return isCheckAll; }
            set
            {
                isCheckAll = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 加载该角色的已有权限设置
        /// </summary>
        public ICommand LoadRoleRightCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    LoadHasRightSet();
                });
            }
        }

        /// <summary>
        /// 全选或全不选
        /// </summary>
        public ICommand CheckAllCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    CheckMenuListState(this.IsCheckAll);
                });
            }
        }

        /// <summary>
        /// 权限设置保存命令
        /// </summary>
        public ICommand SaveRightCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    //获取已勾选的菜单编号
                    List<int> checkedMenuList = new List<int>();
                    GetCheckedIds(checkedMenuList);
                    string msgTitle = "保存权限设置";
                    if(RoleId==0)
                    {
                        ShowError("请选择要设置权限的角色！", msgTitle);
                        return;
                    }
                   else if(checkedMenuList.Count ==0)
                    {
                        ShowError("你没有设置该角色的权限，不能保存！", msgTitle);
                        return;
                    }
                    else
                    {
                        //保存权限设置
                        bool blSave = roleBLL.SaveRoleRightSet(checkedMenuList, RoleId);
                        string sucType = blSave ? "成功" : "失败";
                        string msg = $"权限设置保存{sucType}!";
                        if(blSave)
                        {
                            ShowMessage(msg,msgTitle);
                        }
                        else
                        {
                            ShowError(msg, msgTitle);
                            return;
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        private List<RoleInfo> GetCboRoleList()
        {
            List<RoleInfo> roles = roleBLL.GetAllRoles();
            roles.Insert(0, new RoleInfo()
            {
                RoleId = 0,
                RoleName = "请选择角色"
            });
            return roles;
        }

        /// <summary>
        /// 加载菜单节点列表
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<TreeMenuItem> GetMenuList()
        {
            ObservableCollection<TreeMenuItem> reList = new ObservableCollection<TreeMenuItem>();
            List<MenuInfo> list = menuBLL.GetAllMenus();
            AddMenuItems(list, reList, null, 0);
            return reList;
        }

        /// <summary>
        /// 递归构建菜单列表
        /// </summary>
        /// <param name="mList"></param>
        /// <param name="allList"></param>
        /// <param name="pMenu"></param>
        /// <param name="pId"></param>
        private void AddMenuItems(List<MenuInfo> mList, ObservableCollection<TreeMenuItem> allList, TreeMenuItem pMenu, int pId)
        {
            var pList = mList.Where(m => m.ParentId == pId);//筛选出当前节点的子节点列表（菜单）
            foreach (MenuInfo mi in pList)
            {
                TreeMenuItem menu = new TreeMenuItem()
                {
                    MenuId = mi.MenuId,
                    MenuName = mi.MenuName
                };
                if (pMenu != null)
                {
                    pMenu.SubItems.Add(menu);
                    menu.ParentNode = pMenu;
                }
                else
                    allList.Add(menu);

                AddMenuItems(mList, allList, menu, mi.MenuId);
            }
        }

        /// <summary>
        /// 全选与全不选
        /// </summary>
        /// <param name="blCheck"></param>
        public void CheckMenuListState(bool blCheck)
        { 
            foreach(var menu in MenuList)
            {
                menu.IsCheck = blCheck;
                foreach (var subMenu in menu.SubItems)
                {
                    subMenu.IsCheck = menu.IsCheck;
                }
            }
        
        }

        /// <summary>
        /// 加载已有的权限设置
        /// </summary>
        private void LoadHasRightSet()
        {
            if (roleBLL.IsAdmin(this.RoleId))
            {
                CheckMenuListState(true);//超级管理员角色，菜单设置全选
                this.IsSave = false;//不能保存权限设置
                return;
            }
            else
            {
                CheckMenuListState(false);//清空所有勾选
                List<int> menuIds = roleBLL.GetRoleMenuIds(RoleId);//获取该角色的菜单编号集合
                foreach (var menu in MenuList)
                {
                    if (menuIds.Contains(menu.MenuId))
                    {
                        menu.IsCheck = true;
                        foreach (var subMenu in menu.SubItems)
                        {
                            if (menuIds.Contains(subMenu.MenuId))
                            {
                                subMenu.IsCheck = true;
                            }
                        }
                    }
                }
                IsSave = true;
            }
        }

        /// <summary>
        /// 获取已勾选的菜单编号集合
        /// </summary>
        /// <param name="checkedList"></param>
        public void GetCheckedIds(List<int> checkedList)
        {
            foreach (var menu in MenuList)
            {
                if (menu.IsCheck == true)
                    checkedList.Add(menu.MenuId);
                if(menu.SubItems.Count >0)
                {
                    checkedList.AddRange(menu.SubItems.Where(m => m.IsCheck == true).Select(m => m.MenuId));
                }
            }
        }
    }
}
