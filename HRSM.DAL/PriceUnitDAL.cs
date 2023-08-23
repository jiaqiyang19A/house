using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public class PriceUnitDAL : BaseDAL<PriceUnitInfo>
        {
                /// <summary>
                /// 获取价格单位列表
                /// </summary>
                /// <returns></returns>
                public List<PriceUnitInfo> GetPriceUnits()
                {
                        return GetModelList("","PUnitId,PUnitName", "");
                }
        }
}
