using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
       public  class HouseLayoutBLL
        {

                private HouseLayoutDAL hlDAL = new HouseLayoutDAL();
                /// <summary>
                /// 获取房屋户型列表
                /// </summary>
                /// <returns></returns>
                public List<HouseLayoutInfo> GetHouseLayouts()
                {
                        return hlDAL.GetHouseLayouts();
                }
        }
}
