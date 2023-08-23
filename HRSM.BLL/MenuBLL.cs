using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
    public class MenuBLL
    {
        MenuDAL menuDAL = new MenuDAL();
        RoleDAL roleDAL = new RoleDAL();

        /// <summary>
        /// 新增菜单信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        public bool AddMenuInfo(MenuInfo menuInfo)
        {
            return menuDAL.AddMenuInfo(menuInfo);
        }
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        public bool UpdateMenuInfo(MenuInfo menuInfo)
        {
            return menuDAL.UpdateMenuInfo(menuInfo);
        }

        /// <summary>
        /// 判断菜单名称是否已存在
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public bool ExistsMenu(string menuName)
        {
            return menuDAL.ExistsMenu(menuName);
        }

        /// <summary>
        /// 获取指定角色集合的菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public List<MenuInfo> GetRoleMenuList(List<int> roleIds)
        {
            if(roleIds!=null && roleIds.Count >0)
            {
                bool isAdmin = false;
                isAdmin = roleDAL.IsRolesAdmin(roleIds);
                if(isAdmin)
                {
                    return menuDAL.GetAllMenuList();
                }
                else
                {
                    return menuDAL.GetRoleMenuList(roleIds);
                }
            }
            return new List<MenuInfo>();
        }

        /// <summary>
        /// 获取所有菜单列表（下拉框）
        /// </summary>
        /// <returns></returns>
        public List<MenuInfo> GetAllMenus()
        {
            return menuDAL.GetAllMenus();
        }

        /// <summary>
        /// 条件查询菜单列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public List<MenuInfo> GetMenuList(string keywords, bool isShowDel)
        {
            int isDeleted = isShowDel ? 1 : 0;
            return menuDAL.GetMenuList(keywords, isDeleted);
        }


        /// <summary>
        /// 判断菜单是否包含子菜单
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public bool HasChildMenus(int menuId)
        {
            int count = menuDAL.GetSubMenus(menuId);
            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断菜单列表中是否包含子菜单
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public bool HasChildMenus(List<int> menuIds)
        {
            foreach (int menuId in menuIds)
            {
                if (HasChildMenus(menuId))
                    return true;
            }
            return false;
        }

        public bool IsSystemMenu(List<int> menuIds)
        {
            foreach (int menuId in menuIds)
            {
                if (GetMenuInfo(menuId).MenuName=="系统管理")
                    return true;
            }
            return false;
        }


        /// <summary>
        /// 获取指定 菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public MenuInfo GetMenuInfo(int menuId)
        {
            return menuDAL.GetMenuInfo(menuId);
        }

        /// <summary>
        /// 删除菜单信息（假删除）
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="delType">0-假删除 1-真删除 </param>
        /// <returns></returns>
        public bool LogicDelMenuList(List<int> menuIds)
        {
            return menuDAL.UpdateMenuInfosState(menuIds, 0, 1);
        }

        /// <summary>
        /// 恢复菜单信息
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public bool RecoverMenuList(List<int> menuIds)
        {
            return menuDAL.UpdateMenuInfosState(menuIds, 0, 0);
        }

        /// <summary>
        /// 删除菜单信息（真删除）
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public bool RemoveMenuList(List<int> menuIds)
        {
            return menuDAL.UpdateMenuInfosState(menuIds, 1, 2);
        }


    }
}
