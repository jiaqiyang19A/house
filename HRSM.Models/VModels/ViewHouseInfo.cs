using Common.CustAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.Models.VModels
{
    /// <summary>
    /// 房屋信息视图模型类
    /// </summary>
    [Table("ViewHouseInfos")] 
    public class ViewHouseInfo
    {
        public int HouseId { get; set; }
        public string HouseName { get; set; }
        public string Building { get; set; }
        public string HouseAddress { get; set; }
        public string RentSale { get; set; }
        public string HouseDirection { get; set; }
        public string HouseLayout { get; set; }
        public int? OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string HouseState { get; set; }
        public bool IsPublish { get; set; }
        public int IsDeleted { get; set; }
        public decimal HouseArea { get; set; }
        public int HouseFloor { get; set; }
        public decimal?  HousePrice { get; set; }
        public string PriceUnit { get; set; }
        public string HousePic { get; set; }
        public string Remark { get; set; }
        public string OwnerPhone { get; set; }
    }
}
