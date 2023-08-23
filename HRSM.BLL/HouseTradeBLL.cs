using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public class HouseTradeBLL
        {
                private HouseTradeDAL htDAL = new HouseTradeDAL();

                /// <summary>
                /// 添加交易记录
                /// </summary>
                /// <param name="houseTradeInfo"></param>
                /// <returns></returns>
                public bool AddHouseTradeInfo(HouseTradeInfo houseTradeInfo)
                {
                        return htDAL.AddHouseTradeInfo(houseTradeInfo);
                }

        }
}
