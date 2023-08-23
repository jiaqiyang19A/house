using Common;
using DbUtility;
using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public   class HouseTradeDAL:BaseDAL<HouseTradeInfo>
        {
                /// <summary>
                ///  获取指定房屋集合中存在已交易的房屋数
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public int GetTradeHouseCount(List<int> houseIds)
                {
                        string strIds = string.Join(",", houseIds);
                        string sql = $"select count(distinct HouseId) from HouseTradeInfos where   HouseId in ({strIds}) and IsDeleted=0";
                        object oCount = SqlHelper.ExecuteScalar(sql, 1);
                        return oCount.GetInt();
                }
                /// <summary>
                /// 获取指定客户列表中包含交易客户的编号
                /// </summary>
                /// <param name="custIds"></param>
                /// <returns></returns>
                public List<int>  GetTradeCustomerIds(List<int> custIds)
                {
                        string strIds = string.Join(",", custIds);//连接编号成字符串
                        //查询命令
                        string sql = $"select distinct CustomerId from HouseTradeInfos where   CustomerId in ({strIds}) and IsDeleted=0";
                        DataTable dt = SqlHelper.GetDataTable(sql, 1);
                        List<int> tradeCustIds = new List<int>();
                        foreach(DataRow dr in dt.Rows)
                        {
                                tradeCustIds.Add(dr["CustomerId"].GetInt());
                        }
                        return tradeCustIds;
                }

                /// <summary>
                /// 添加交易记录(并修改房屋的状态)
                /// </summary>
                /// <param name="houseTradeInfo"></param>
                /// <returns></returns>
                public bool AddHouseTradeInfo(HouseTradeInfo houseTradeInfo)
                {
                        string cols = "HouseId,OwnerId,CustomerId,RentSale,TradeAmount,PriceUnit,TradeTime,TradeWay,DealUser";
                        List<CommandInfo> list = new List<CommandInfo>();
                        //添加交易记录  入库
                        SqlModel inModel = CreateSql.CreateInsertSql(houseTradeInfo, cols, 0);
                        list.Add(new CommandInfo()
                        {
                                CommandText = inModel.Sql,
                                IsProc = false,
                                Paras = inModel.Paras
                        });
                        string houseState = "";
                        houseState = houseTradeInfo.RentSale == "出售" ? "已出售" : "已出租";
                        string sql = $"update HouseInfos set HouseState='{houseState}' where HouseId={houseTradeInfo.HouseId}";
                        //修改房屋状态
                        list.Add(new CommandInfo()
                        {
                                CommandText = sql,
                                IsProc = false
                        });
                        return SqlHelper.ExecuteTrans(list);
                }
        }
}
