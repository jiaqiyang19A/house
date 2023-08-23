using Common;
using DbUtility;
using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
    public  class RoleDAL:BaseDAL<RoleInfo>
    {
        /// <summary>
        /// 新增角色信息
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool AddRoleInfo(RoleInfo roleInfo)
        {
            return Add(roleInfo, "RoleName,Remark", 0)>0;
        }
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public bool UpdateRoleInfo(RoleInfo roleInfo)
        {
            return Update(roleInfo, "RoleId,RoleName,Remark");
        }
        /// <summary>
        /// 判断角色名是否已存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool ExistsRole(string roleName)
        {
            return ExistsByName("RoleName", roleName);
        }
        /// <summary>
        /// 判断一个角色是否是超级管理员
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool IsRoleAdmin(int roleId)
        {
            RoleInfo roleInfo = GetById(roleId, "IsAdmin");
            if (roleInfo != null)
                return roleInfo.IsAdmin;
            return false;
        }

        /// <summary>
        /// 判断角色集合中是否包含管理员
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public bool IsRolesAdmin(List<int> roleIds)
        {
            bool isAdmin = false;
             foreach(int roleId in roleIds)
            {
                 if(IsRoleAdmin(roleId))
                {
                    isAdmin = true;
                    break;
                }
            }
            return isAdmin;
        }

        /// <summary>
        /// 获取指定的角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public RoleInfo GetRoleInfo(int roleId)
        {
            return GetById(roleId, PropertyHelper.GetColNames<RoleInfo>("IsDeleted"));
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetRoleName(int roleId)
        {
            RoleInfo roleInfo= GetById(roleId, "RoleName");
            if (roleInfo != null)
                return roleInfo.RoleName;
            return "";
        }

        /// <summary>
        /// 获取所有角色列表（绑定下拉框或列表框）
        /// </summary>
        /// <returns></returns>
        public List<RoleInfo> GetAllRoles()
        {
            return GetModelList("RoleId,RoleName", "", 0);
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public List<RoleInfo> GetRoleList(int isDeleted)
        {
            string cols = PropertyHelper.GetColNames<RoleInfo>("IsDeleted,CreateTime");
            return GetModelList(cols, "", isDeleted);
        }


        /// <summary>
        /// 修改角色信息的删除状态
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="delType">0   1  </param>
        /// <param name="isDeleted">1 0 2</param>
        /// <returns></returns>
        public bool   UpdateRoleInfoState(int roleId, int delType, int isDeleted)
        {
            string[] tables = { "RoleInfos", "RoleMenuInfos", "UserRoleInfos" };
            List<string> sqlList = GetDeleteSql(delType, roleId, isDeleted, tables);
            return SqlHelper.ExecuteTrans(sqlList);
        }

        /// <summary>
        /// 批量删除角色信息（真、假删除）
        /// </summary>
        /// <param name="roleIds"></param>
        /// <param name="delType"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
       public bool  UpdateRolesState(List<int> roleIds, int delType, int isDeleted)
        {
            string[] tables = { "RoleInfos", "RoleMenuInfos", "UserRoleInfos" };
            List<string> sqlList = GetDeleteListSql(delType, roleIds, isDeleted, tables);
            return SqlHelper.ExecuteTrans(sqlList);
        }
    }
}
