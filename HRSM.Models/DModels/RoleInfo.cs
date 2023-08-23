using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 角色信息表 
    /// </summary>
    [Table("RoleInfos")]
    [PrimaryKey("RoleId", autoIncrement = true)]
    public class RoleInfo
    {

        /// <summary>
        /// 角色编号
        /// </summary>		
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>		
        public string RoleName { get; set; }
        /// <summary>
        /// 是否是超级管理员 
        /// </summary>		
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 角色描述
        /// </summary>		
        public string Remark { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>		
        public int IsDeleted { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

    }
}

