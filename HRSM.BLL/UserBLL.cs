using HRSM.DAL;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
    public class UserBLL
    {
        ViewUserRoleDAL vurDAL = new ViewUserRoleDAL();
        UserDAL userDAL = new UserDAL();
        RoleBLL roleBLL = new RoleBLL();
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urList">角色设置列表</param>
        /// <returns></returns>
        public bool AddUserInfo(UserInfo userInfo, List<UserRoleInfo> urList)
        {
            if (urList != null && urList.Count > 0)
                return userDAL.AddUserInfo(userInfo, urList);
            else
                return userDAL.AddUserInfo(userInfo);
        }
        /// <summary> 
        /// 修改用户信息和用户角色关系
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="urList">当前的角色设置列表</param>
        /// <param name="urListNew">新增角色设置列表</param>
        /// <returns></returns>
        public bool UpdateUserInfo(UserInfo userInfo, List<UserRoleInfo> urList, List<UserRoleInfo> urListNew)
        {
            if (urList != null && urList.Count > 0)
                return userDAL.UpdateUserInfo(userInfo, urList, urListNew);
            else
                return userDAL.UpdateUserInfo(userInfo);

        }
        /// <summary>
        /// 获取指定用户的登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserRoleIdsInfo GetUserRoleIdsInfo(int userId)
        {
            UserRoleIdsInfo uRoleIds = default(UserRoleIdsInfo);
            //获取用户的角色列表
            List<ViewUserRoleInfo> userRoleList = vurDAL.GetUserRoleList(userId);
            if(userRoleList!=null &&userRoleList.Count>0)
            {
                List<int> roleIds = userRoleList.Select(r => r.RoleId).ToList();
                //List<int> roleIds1 = new List<int>();
                //foreach (ViewUserRoleInfo item in userRoleList)
                //{
                //    roleIds1.Add(item.RoleId);
                //}
                uRoleIds = new UserRoleIdsInfo();
                uRoleIds.UserId = userRoleList[0].UserId;
                uRoleIds.UserName = userRoleList[0].UserName;
                uRoleIds.RoleIds = roleIds;
                uRoleIds.RoleName = roleBLL.GetRoleName(roleIds);
                uRoleIds.UserFName = userRoleList[0].UserFName;
                uRoleIds.UserPhone= userRoleList[0].UserPhone;
                uRoleIds.UserState= userRoleList[0].UserState;
                uRoleIds.UserPwd= userRoleList[0].UserPwd;
            }
            else
            {
                UserInfo userInfo = userDAL.GetUserInfo(userId);
                if(userInfo!=null)
                {
                    uRoleIds.UserId = userInfo.UserId;
                    uRoleIds.UserName = userInfo.UserName;
                    uRoleIds.RoleIds = null;
                    uRoleIds.RoleName =null;
                    uRoleIds.UserFName = userInfo.UserFName;
                    uRoleIds.UserPhone = userInfo.UserPhone;
                    uRoleIds.UserState = userInfo.UserState;
                    uRoleIds.UserPwd = userInfo.UserPwd;
                }
            }
            return uRoleIds;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public int UserLogin(UserInfo userInfo)
        {
            UserInfo user = userDAL.UserLogin(userInfo);
            //对返回的用户信息进行处理
            if (user != null && user.UserId > 0)
            {
                if (user.UserState)
                {
                    return user.UserId;
                }
                else
                    return -1;//用户已冻结
            }
            else
                return 0;//账号或密码错误
        }

        /// <summary>
        /// 条件查询用户信息
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="userState"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserList(string keywords, bool isFrozen, bool  showDel)
        {
            int userState = isFrozen ? 0 : 1;
            int isDeleted = showDel ? 1 : 0;
            return userDAL.GetUserList(keywords, userState, isDeleted);
        }

        /// <summary>
        /// 获取指定用户的角色编号集合
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<int> GetUserRoleIds(int userId)
        {
            return vurDAL.GetUserRoleIds(userId);
        }

        /// <summary>
        /// 批量逻辑删除用户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public bool LogicDelUserList(List<int> userIds)
        {
            return userDAL.UpdateUsersState(userIds, 0, 1);
        }
        /// <summary>
        /// 批量恢复用户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public bool RecoverUserList(List<int> userIds)
        {
            return userDAL.UpdateUsersState(userIds, 0, 0);
        }
        /// <summary>
        /// 批量移除（真删除）用户信息
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public bool RemoveUserList(List<int> userIds)
        {
            return userDAL.UpdateUsersState(userIds, 1, 2);
        }

        /// <summary>
        /// 检查用户名是否已存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUser(string userName)
        {
            return userDAL.ExistsUser(userName);
        }
    }
}
