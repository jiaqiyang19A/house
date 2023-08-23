using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public class HouseLayoutDAL:BaseDAL<HouseLayoutInfo>
        {
                /// <summary>
                /// 获取房屋户型列表
                /// </summary>
                /// <returns></returns>
                public List<HouseLayoutInfo>    GetHouseLayouts()
                {
                        return GetModelList("", "HLId,HLName", "");
                }
        }
}
