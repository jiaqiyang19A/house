using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using Common.CustAttributes;

namespace HRSM.Models.DModels
{
    /// <summary>
    /// 房屋户型信息表
    /// </summary>
    [Table("HouseLayoutInfos")]
    [PrimaryKey("HLId", autoIncrement = true)]
    public class HouseLayoutInfo
    {

        /// <summary>
        /// 编号
        /// </summary>		
        public int HLId { get; set; }
        /// <summary>
        /// 户型名称
        /// </summary>		
        public string HLName { get; set; }

    }
}

