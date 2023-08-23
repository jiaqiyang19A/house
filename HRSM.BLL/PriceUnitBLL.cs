using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public  class PriceUnitBLL
        {
                private PriceUnitDAL puDAL = new PriceUnitDAL();

                /// <summary>
                /// 获取价格单位列表
                /// </summary>
                /// <returns></returns>
                public List<PriceUnitInfo> GetPriceUnits()
                {
                        return puDAL.GetPriceUnits();
                }
        }
}
