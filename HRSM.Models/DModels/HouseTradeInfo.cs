using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 房屋交易信息表
    /// </summary>
    [Table("HouseTradeInfos")]
    [PrimaryKey("TradeId", autoIncrement = true)]
    public class HouseTradeInfo
    {

        /// <summary>
        /// 交易编号 
        /// </summary>		
        public int TradeId { get; set; }
        /// <summary>
        /// 房屋编号
        /// </summary>		
        public int HouseId { get; set; }
        /// <summary>
        /// 业主编号
        /// </summary>		
        public int OwnerId { get; set; }
        /// <summary>
        /// 客户编号
        /// </summary>		
        public int CustomerId { get; set; }
        /// <summary>
        /// 租售类型
        /// </summary>		
        public string RentSale { get; set; }
        /// <summary>
        /// 交易总价
        /// </summary>		
        public decimal TradeAmount { get; set; }	
        /// <summary>
        /// 单位
        /// </summary>
        public string PriceUnit { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>		
        public DateTime TradeTime { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string TradeWay { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>		
        public string DealUser { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>		
        public int IsDeleted { get; set; }

    }
}

