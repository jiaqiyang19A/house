using HRSM.DAL.Base;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public class ViewSaleHouseStatisticsDAL:BQuery<ViewSaleHouseStatisticsInfo>
        {

                public List<ViewSaleHouseStatisticsInfo> GetSaleHouseStatisticsData()
                {
                        return GetModelList("","", "");
                }

                public List<ViewSaleHouseStatisticsInfo> GetSaleTimeHouseStatisticsData(string saleName,DateTime? startTime,DateTime? endTime)
                {
                        string strStTime = "", strEtTime = "";
                        if (startTime != null)
                                strStTime = startTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        if (endTime != null)
                                strEtTime = endTime.Value.ToString("yyyy-MM-dd ")+"23:59:59";
                        string sql = $"select * from [dbo].[StatisticsSaleHouseByTime]('{saleName}','{strStTime}','{strEtTime}')";
                        DataTable dt = GetDt(sql, 1);
                        List<ViewSaleHouseStatisticsInfo> list = DbConvert.DataTableToList<ViewSaleHouseStatisticsInfo>(dt, "");
                        return list;
                }
        }
}
