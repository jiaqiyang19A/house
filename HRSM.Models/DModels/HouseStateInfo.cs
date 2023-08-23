using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 房屋状态信息表
    /// </summary>
    [Table("HouseStateInfos")]
    [PrimaryKey("StateId", autoIncrement = true)]
    public class HouseStateInfo
    {

        /// <summary>
        /// 状态编号
        /// </summary>		
        public int StateId { get; set; }
        /// <summary>
        /// 状态名称
        /// </summary>		
        public string StateName { get; set; }
        /// <summary>
        /// 租售类型编号
        /// </summary>		
        public int RSId { get; set; }

    }
}

