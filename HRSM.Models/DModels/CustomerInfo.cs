using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 客户信息表
    /// </summary>
    [Table("CustomerInfos")]
    [PrimaryKey("CustomerId", autoIncrement = true)]
    public class CustomerInfo
    {
        /// <summary>
        /// 客户编号
        /// </summary>		
        public int CustomerId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>		
        public string CustomerName { get; set; }
        /// <summary>
        /// 客户类别
        /// </summary>
        public string CustomerType { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>		
        public string Contactor { get; set; }
        /// <summary>
        /// 联系号码
        /// </summary>		
        public string CustomerPhone { get; set; }
        /// <summary>
        /// 客户地址
        /// </summary>		
        public string CustomerAddress { get; set; }
        /// <summary>
        /// 客户描述
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
        /// <summary>
        /// 客户状态
        /// </summary>
        public string  CustomerState { get; set; }
    }
}

