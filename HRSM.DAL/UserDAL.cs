using Common;
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
    public class UserDAL:BaseDAL<UserInfo>
    {
        /// <summary>
        /// 添加用户信息(没有设置角色时)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool AddUserInfo(UserInfo userInfo)
        {
            string cols = "UserName,UserPwd,UserState,UserFName,UserPhone";
            return Add(userInfo, cols, 0) > 0;
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urList"></param>
        /// <returns></returns>
        public bool AddUserInfo(UserInfo userInfo,List<UserRoleInfo> urList)
        {
            string cols = "UserName,UserPwd,UserState,UserFName,UserPhone";
            return SqlHelper.ExecuteTrans<bool>(cmd =>
            {
                try
                {
                    //添加用户信息
                    SqlModel insert = CreateSql.CreateInsertSql<UserInfo>(userInfo, cols, 1);
                    SqlHelper.InitIdbCommand(cmd, insert.Sql, 1, insert.Paras);
                    object oId = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    int userId = oId.GetInt();
                    if(userId>0)
                    {
                        //添加用户角色关系
                        string urCols = "UserId,RoleId";
                        foreach(var ur in urList)
                        {
                            ur.UserId = userId;
                            SqlModel urInsert = CreateSql.CreateInsertSql(ur, urCols, 0);
                            SqlHelper.InitIdbCommand(cmd, urInsert.Sql, 1, urInsert.Paras);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                    cmd.Transaction.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    cmd.Transaction.Rollback();
                    throw new Exception("添加用户信息执行事务异常！", ex);
                }
                finally
                {
                    cmd.Connection.Close();
                }
            });
        }

        /// <summary>
        /// 修改用户信息(没有修改角色设置时)
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(UserInfo userInfo)
        {
            string cols = "UserId,UserName";
            if (!string.IsNullOrEmpty(userInfo.UserPwd))
                cols += ",UserPwd";
            cols += ",UserState,UserFName,UserPhone";
            return Update(userInfo, cols, "");
        }

        /// <summary>
        /// 修改用户信息和用户角色关系
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urList">当前的角色设置列表</param>
        /// <param name="urListNew">新加角色设置列表</param>
        /// <returns></returns>
        public bool UpdateUserInfo(UserInfo userInfo, List<UserRoleInfo> urList, List<UserRoleInfo> urListNew)
        {
            string cols = "UserId,UserName";
            if (!string.IsNullOrEmpty(userInfo.UserPwd))
                cols += ",UserPwd";
            cols += ",UserState,UserFName,UserPhone";
            List<CommandInfo> comList = new List<CommandInfo>();
            SqlModel upUser = CreateSql.CreateUpdateSql(userInfo, cols, "");
            //修改用户信息
            comList.Add(new CommandInfo()
            {
                CommandText = upUser.Sql,
                IsProc = false,
                Paras = upUser.Paras
            });
            if (urList != null && urList.Count > 0)//当前的角色列表
            {
                //删除取消的角色关系数据
                string roleIds = string.Join(",", urList.Select(ur => ur.RoleId));
                comList.Add(new CommandInfo()
                {
                    CommandText = $"delete from UserRoleInfos where RoleId not in ({roleIds}) and UserId={userInfo.UserId}",
                    IsProc = false
                });
            }
            if (urListNew.Count > 0)//如果存在新加的角色
            {
                //新增新设置的角色关系  
                string colsUserRole = "UserId,RoleId";
                foreach (var ur in urListNew)
                {
                    SqlModel inUserRole = CreateSql.CreateInsertSql(ur, colsUserRole, 0);
                    comList.Add(new CommandInfo()
                    {
                        CommandText = inUserRole.Sql,
                        IsProc = false,
                        Paras = inUserRole.Paras
                    });
                }
            }
            return SqlHelper.ExecuteTrans(comList);
        }

        /// <summary>
        /// 获取指定的用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserInfo GetUserInfo(int userId)
        {
            string cols = PropertyHelper.GetColNames<UserInfo>("IsDeleted");
            return GetById(userId, cols);
        }

        /// <summary>
        ///用户登录
        /// </summary>
        /// <param name="userInfo">账号与密码</param>
        /// <returns></returns>
        public UserInfo UserLogin(UserInfo userInfo)
        {
            string strWhere = "UserName=@userName and UserPwd=@userPwd and IsDeleted=0";
            SqlParameter[] paras =
            {
                new SqlParameter("@userName",userInfo.UserName),
                new SqlParameter("@userPwd",userInfo.UserPwd)
            };
            UserInfo user = GetModel(strWhere, "UserId,UserState", paras);
            return user;
        }

        /// <summary>
        /// 条件查询用户信息
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="userState"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserList(string keywords, int userState, int isDeleted)
        {
            string cols =PropertyHelper.GetColNames<UserInfo>("CreateTime,UserPwd,IsDeleted");
            string strWhere = $"IsDeleted={isDeleted} and UserState={userState}";
            if(!string.IsNullOrEmpty(keywords))
            {
                strWhere += " and (UserName like @keywords or UserFName like @keywords)";
                SqlParameter paraKeywords = new SqlParameter("@keywords", $"%{keywords}%");
                return GetModelList(strWhere, cols, "", paraKeywords);
            }
            return GetModelList(strWhere, cols, "");
        }

        /// <summary>
        /// 批量修改用户信息的删除状态
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="delType"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public bool UpdateUsersState(List<int> userIds, int delType, int isDeleted)
        {
            List<string> sqlList = new List<string>();
            string[] tableNames = { "UserInfos", "UserRoleInfos" };
            sqlList = GetDeleteListSql(delType, userIds, isDeleted, tableNames);
            return SqlHelper.ExecuteTrans(sqlList);
        }

        /// <summary>
        /// 检查用户是否已存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUser(string userName)
        {
            return ExistsByName("UserName", userName);
        }
    }
}
