using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 客户跟进日志信息表
    /// </summary>
    [Table("CustomerFollowUpLogInfos")]
    [PrimaryKey("FLogId",autoIncrement =true)]
    public class CustomerFollowUpLogInfo
    {

        /// <summary>
        /// 日志编号
        /// </summary>		
        public int FLogId { get; set; }
        /// <summary>
        /// 客户需求编号
        /// </summary>		
        public int CustRequestId { get; set; }
        /// <summary>
        /// 跟进时间
        /// </summary>		
        public DateTime FollowUpTime { get; set; }
        /// <summary>
        /// 跟讲内容
        /// </summary>		
        public string FollowUpContent { get; set; }
        /// <summary>
        /// 跟进人
        /// </summary>		
        public string FollowUpUser { get; set; }
        /// <summary>
        /// 跟进状态
        /// </summary>		
        public string FollowUpState { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>		
        public int IsDeleted { get; set; }

    }
}

