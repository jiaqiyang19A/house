using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.Models.VModels
{
    /// <summary>
    /// 用户信息和角色编号集合
    /// </summary>
    public class UserRoleIdsInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<int> RoleIds { get; set; }
        public string RoleName { get; set; }
        public string UserPhone { get; set; }
        public string UserPwd { get; set; }
        public bool UserState { get; set; }
        public string UserFName { get; set; }
    }
}
