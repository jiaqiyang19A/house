using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    [Table("UserRoleInfos")]
    [PrimaryKey("URId", autoIncrement = true)]
    public class UserRoleInfo
    {

        /// <summary>
        /// 关系编号
        /// </summary>		
        public int URId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>		
        public int UserId { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>		
        public int RoleId { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>		
        public int IsDeleted { get; set; }

    }
}

