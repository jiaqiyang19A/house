using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 角色菜单关系表
    /// </summary>
    [Table("RoleMenuInfos")]
    [PrimaryKey("RMId", autoIncrement = true)]
    public class RoleMenuInfo
    {

        /// <summary>
        /// 关系编号
        /// </summary>		
        public int RMId { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>		
        public int RoleId { get; set; }
        /// <summary>
        /// 菜单编号
        /// </summary>		
        public int MenuId { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>		
        public int IsDeleted { get; set; }

    }
}

