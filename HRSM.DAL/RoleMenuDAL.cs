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
    public class RoleMenuDAL:BaseDAL<RoleMenuInfo>
    {
        /// <summary>
        /// 获取指定角色的菜单编号集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<int> GetRoleMenuIds(int roleId)
        {
            string strWhere = $"IsDeleted=0 and RoleId={roleId}";
            string cols = "MenuId";
            return GetModelList(strWhere, cols, "").Select(rm=>rm.MenuId).ToList();
        }

        /// <summary>
        /// 保存角色菜单关系（保存权限设置）
        /// </summary>
        /// <param name="rmInfos"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SaveRoleMenuRelations(List<RoleMenuInfo> rmInfos, int roleId)
        {
            List<CommandInfo> comList = new List<CommandInfo>();
            string strIds = string.Join(",", rmInfos.Select(rm => rm.MenuId));
            string strWhereDel = $"RoleId={roleId} and MenuId not in ({strIds}) and IsDeleted=0";
            comList.Add(new CommandInfo()
            {
                CommandText = CreateSql.CreateDeleteSql<RoleMenuInfo>(strWhereDel),
                IsProc=false
            });
            foreach(var rm in rmInfos)
            {
                string cols = "RoleId,MenuId";
                string strWhere = $"RoleId={roleId} and MenuId={rm.MenuId} and IsDeleted=0";
                if(!Exists(strWhere))
                {
                    SqlModel insert = CreateSql.CreateInsertSql(rm, cols, 0);
                    comList.Add(new CommandInfo()
                    {
                        CommandText = insert.Sql,
                        IsProc = false,
                        Paras = insert.Paras
                    });
                }
            }
            return SqlHelper.ExecuteTrans(comList);
        }
    }
}
