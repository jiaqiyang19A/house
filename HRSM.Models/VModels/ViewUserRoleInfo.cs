using Common.CustAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.Models.VModels
{
    /// <summary>
    /// ViewUserRoleInfos 实体模型
    /// </summary>
   [Table("ViewUserRoleInfos")]
    public  class ViewUserRoleInfo
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool UserState { get; set; }
        public string UserFName { get; set; }
        public string RoleName { get; set; }
        public bool IsAdmin { get; set; }
        public int UserDeleted { get; set; }
        public string UserPhone { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }
}
