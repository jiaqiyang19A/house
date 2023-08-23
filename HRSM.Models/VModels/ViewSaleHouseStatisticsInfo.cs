using Common.CustAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.Models.VModels
{
        [Table("ViewSaleHouseStatistics")]
        public   class ViewSaleHouseStatisticsInfo
        {
                public string DealUser { get; set; }
                public string UserFName { get; set; }
                public int TotalCount { get; set; }
                public int RentCount { get; set; }
                public int SaleCount { get; set; }
        }
}
