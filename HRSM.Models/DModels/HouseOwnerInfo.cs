using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 业主信息表
    /// </summary>
    [Table("HouseOwnerInfos")]
    [PrimaryKey("OwnerId", autoIncrement = true)]
    public class HouseOwnerInfo
    {

        /// <summary>
        /// 编号
        /// </summary>		
        public int OwnerId { get; set; }
        /// <summary>
        /// 业主名称
        /// </summary>		
        public string OwnerName { get; set; }
        /// <summary>
        /// 业主类型
        /// </summary>		
        public string OwnerType { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>		
        public string Contactor { get; set; }
        /// <summary>
        /// 业主电话
        /// </summary>		
        public string OwnerPhone { get; set; }
        /// <summary>
        /// 业主地址
        /// </summary>		
        public string OwnerAddress { get; set; }
        /// <summary>
        /// 备注
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

