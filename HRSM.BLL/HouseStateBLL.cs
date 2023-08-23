using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public  class HouseStateBLL
        {
                private HouseStateDAL htDAL = new HouseStateDAL();

                /// <summary>
                /// 获取房屋状态列表
                /// </summary>
                /// <param name="rsId"></param>
                /// <returns></returns>
                public List<HouseStateInfo> GetHouseStates(int rsId)
                {
                        return htDAL.GetHouseStates(rsId);
                }
        }
}
