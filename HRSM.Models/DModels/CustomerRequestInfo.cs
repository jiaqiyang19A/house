using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 客户需求信息表
    /// </summary>
    [Table("CustomerRequestInfos")]
    [PrimaryKey("CustRequestId", autoIncrement = true)]
    public class CustomerRequestInfo
    {
        /// <summary>
        /// 需求编号
        /// </summary>		
        public int CustRequestId { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>		
        public int CustomerId { get; set; }
        /// <summary>
        /// 需求内容
        /// </summary>		
        public string RequestContent { get; set; }
        /// <summary>
        /// 跟进人员
        /// </summary>		
        public string FollowUpUser { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>		
        public int IsDeleted { get; set; }
        /// <summary>
        /// 跟进状态
        /// </summary>		
        public string RequestState { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        private DateTime createTime = DateTime.Now;
        public DateTime CreateTime
        {
            get
            {
                return createTime;
            }
            set
            {
                createTime = value;
            }
        }

    }
}

