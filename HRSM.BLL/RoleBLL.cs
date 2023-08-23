using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
    public class RoleBLL
    {
        RoleDAL roleDAL = new RoleDAL();
        RoleMenuDAL rmDAL = new RoleMenuDAL();
        /// <summary>
        /// 新增角色信息
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool AddRoleInfo(RoleInfo roleInfo)
        {
            return roleDAL.AddRoleInfo(roleInfo);
        }
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool UpdateRoleInfo(RoleInfo roleInfo)
        {
            return roleDAL.UpdateRoleInfo(roleInfo);
        }
        /// <summary>
        /// 判断角色名是否已存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool ExistsRole(string roleName)
        {
            return roleDAL.ExistsRole(roleName);
        }

        public bool IsAdmin(int roleId)
        {
            return roleDAL.IsRoleAdmin(roleId);
        }

        public bool IsRolesAdmin(List<int> roleIds)
        {
            return roleDAL.IsRolesAdmin(roleIds);
        }

        /// <summary>
        /// 获取指定的角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public RoleInfo GetRoleInfo(int roleId)
        {
            return roleDAL.GetRoleInfo(roleId);
        }

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public string GetRoleName(List<int> roleIds)
        {
            string roleName = "";
            foreach (int roleId in roleIds)
            {
                 if(IsAdmin(roleId))
                {
                    return GetRoleName(roleId);
                }
                 else
                {
                    if (roleName != "")
                        roleName += ",";
                    roleName += GetRoleName(roleId);
                }
            }
            return roleName;
        }

        public string GetRoleName(int roleId)
        {
            return roleDAL.GetRoleName(roleId);    
        }

        /// <summary>
        /// 获取所有角色列表（绑定下拉框或列表框）
        /// </summary>
        /// <returns></returns>
        public List<RoleInfo> GetAllRoles()
        {
            return roleDAL.GetAllRoles();
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public List<RoleInfo> GetRoleList(bool isShowDel)
        {
            int isDeleted = isShowDel ? 1 : 0;
            return roleDAL.GetRoleList(isDeleted);
        }

        #region 角色删除与恢复
        /// <summary>
        /// 逻辑删除角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
          public bool LogicDelRole(int roleId)
        {
            return roleDAL.UpdateRoleInfoState(roleId, 0, 1);
        }

        /// <summary>
        /// 批量逻辑删除角色信息
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
       public bool    LogicDelRoles(List<int> roleIds)
        {
            return roleDAL.UpdateRolesState(roleIds, 0, 1);
        }

        /// <summary>
        /// 恢复角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool RecoverRole(int roleId)
        {
            return roleDAL.UpdateRoleInfoState(roleId, 0, 0);
        }

        /// <summary>
        /// 批量恢复角色信息
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public bool RecoverRoles(List<int> roleIds)
        {
            return roleDAL.UpdateRolesState(roleIds, 0, 0);
        }

        /// <summary>
        /// 真删除角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public   bool RemoveRole(int roleId)
        {
            return roleDAL.UpdateRoleInfoState(roleId, 1, 2);
        }

        /// <summary>
        /// 批量移除角色信息
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public bool RemoveRoles(List<int> roleIds)
        {
            return roleDAL.UpdateRolesState(roleIds, 1, 2);
        }

        #endregion

        /// <summary>
        /// 获取指定角色的菜单编号集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> GetRoleMenuIds(int roleId)
        {
            return rmDAL.GetRoleMenuIds(roleId);
        }

        /// <summary>
        /// 保存权限设置
        /// </summary>
        /// <param name="menuIds"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SaveRoleRightSet(List<int> menuIds,int roleId)
        {
            if(menuIds.Count>0&&roleId>0)
            {
                List<RoleMenuInfo> rmList = new List<RoleMenuInfo>();
                foreach (int  menuId in menuIds)
                {
                    rmList.Add(new RoleMenuInfo()
                    {
                        RoleId = roleId,
                        MenuId = menuId
                    });
                }
                return rmDAL.SaveRoleMenuRelations(rmList, roleId);
            }
            return false;
        }

    }
}
