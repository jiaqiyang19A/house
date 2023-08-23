using Common.CustAttributes;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.Models.VModels
{
        [Table("ViewCustomerRequestInfos")]
        public  class ViewCustomerRequestInfo:CustomerRequestInfo
        {
                public string CustomerName { get; set; }
                public string CustomerType { get; set; }
                public string CustomerState { get; set; }
        }
}
