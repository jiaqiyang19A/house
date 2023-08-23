using DbUtility;
using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
    public  class MenuDAL:BaseDAL<MenuInfo>
    {
        /// <summary>
        /// 新增菜单信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        public bool AddMenuInfo(MenuInfo menuInfo)
        {
            string cols = "MenuName,ParentId,MenuUrl,MOrder";
            return Add(menuInfo, cols, 0) > 0;
        }
        /// <summary>
        /// 修改菜单信息
        /// </summary>
        /// <param name="menuInfo"></param>
        /// <returns></returns>
        public bool UpdateMenuInfo(MenuInfo menuInfo)
        {
            string cols = "MenuId,MenuName,ParentId,MenuUrl,MOrder";
            return Update(menuInfo, cols);
        }

        /// <summary>
        /// 判断菜单名称是否已存在
        /// </summary>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public bool ExistsMenu(string menuName)
        {
            return ExistsByName("MenuName", menuName);
        }

        /// <summary>
        /// 获取所有有菜单列表（有效数据）
        /// </summary>
        /// <returns></returns>
         public List<MenuInfo> GetAllMenuList()
        {
            string cols = "MenuId,MenuName,ParentId,MenuUrl";
            return GetModelList(cols, "ParentId,MOrder", 0);
        }

        /// <summary>
        /// 获取所有菜单列表（下拉框）
        /// </summary>
        /// <returns></returns>
        public   List<MenuInfo> GetAllMenus()
        {
            string cols = "MenuId,MenuName,ParentId";
            return GetModelList(cols, "", 0);
        }

        /// <summary>
        /// 根据角色编号集合获取菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public List<MenuInfo>  GetRoleMenuList(List<int> roleIds)
        {
            string cols = "MenuId,MenuName,ParentId,MenuUrl";
            string strIds = string.Join(",", roleIds);
            string strWhere = $"IsDeleted=0 and MenuId in (select MenuId from RoleMenuInfos where RoleId in ({strIds}))";
            return GetModelList(strWhere, cols, "ParentId,MOrder");
        }

        /// <summary>
        /// 获取指定 菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public MenuInfo GetMenuInfo(int menuId)
        {
            string cols = "MenuId,MenuName,ParentId,MenuUrl,MOrder";
            return GetById(menuId, cols);
        }


        /// <summary>
        /// 批量修改菜单信息(并连同关系数据)的删除状态（真删除即为删除操作）
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="delType">0-假删除 1-真删除 </param>
        /// <returns></returns>
        public bool UpdateMenuInfosState(List<int> menuIds, int delType, int isDeleted)
        {
            List<string> sqlList = new List<string>();
            string[] tableNames = { "MenuInfos", "RoleMenuInfos" };
            sqlList = GetDeleteListSql(delType, menuIds, isDeleted, tableNames);
            return SqlHelper.ExecuteTrans(sqlList);
        }

        /// <summary>
        /// 获取指定菜单的子菜单数量
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public int   GetSubMenus(int menuId)
        {
            //获取菜单子菜单编号集合
            List<MenuInfo> list = GetModelList($"ParentId={menuId} and IsDeleted=0", "MenuId","");
            if (list != null && list.Count > 0)
                return list.Count;
            else
                return 0;
        }

        /// <summary>
        /// 条件查询菜单列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
       public List<MenuInfo>  GetMenuList(string keywords, int isDeleted)
        {
            string cols = "MenuId,MenuName,ParentId,MenuUrl,MOrder";
            string strWhere = $"IsDeleted={isDeleted}";
            string strOrder = " ParentId,MOrder";
            if (!string.IsNullOrEmpty(keywords))
            {
                strWhere += " and (MenuName like @keywords or ParentId in (select MenuId from MenuInfos where MenuName like @keywords))";
                SqlParameter paraKeywords = new SqlParameter("@keywords", $"%{keywords}%");
                return GetModelList(strWhere, cols,strOrder, paraKeywords);
            }
            return GetModelList(strWhere, cols,strOrder);
        }

      

    }
}
