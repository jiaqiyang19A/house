using Common;
using DbUtility;
using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public class HouseDAL : BaseDAL<HouseInfo>
        {
                /// <summary>
                /// 添加房屋信息
                /// </summary>
                /// <param name="houseInfo"></param>
                /// <returns></returns>
                public bool AddHouseInfo(HouseInfo houseInfo)
                {
                        string cols = "HouseName,Building,HouseAddress,RentSale,HousePrice,PriceUnit,HouseDirection,HouseLayout,OwnerId,HouseFloor,HouseArea,HouseState,Remark,IsPublish";
                        if (houseInfo.PublishTime != null)
                                cols += ",PublishTime";
                        if (!string.IsNullOrEmpty(houseInfo.PublishUser))
                                cols += ",PublishUser";
                        if (!string.IsNullOrEmpty(houseInfo.HousePic))
                                cols += ",HousePic";
                        return  Add(houseInfo, cols, 0)>0;
                }

                /// <summary>
                /// 修改房屋信息
                /// </summary>
                /// <param name="houseInfo"></param>
                /// <returns></returns>
                public bool UpdateHouseInfo(HouseInfo houseInfo)
                {
                        string cols = "HouseId,HouseName,Building,HouseAddress,RentSale,HousePrice,PriceUnit,HouseDirection,HouseLayout,OwnerId,HouseFloor,HouseArea,HouseState,Remark,IsPublish";
                        if (houseInfo.PublishTime != null)
                                cols += ",PublishTime";
                        if (!string.IsNullOrEmpty(houseInfo.PublishUser))
                                cols += ",PublishUser";
                        if (!string.IsNullOrEmpty(houseInfo.HousePic))
                                cols += ",HousePic";
                        return Update(houseInfo, cols, "");
                }



                /// <summary>
                /// 批量导入房屋数据
                /// </summary>
                /// <param name="dtInfos"></param>
                /// <returns></returns>
                public bool AddHouseInfos(DataTable dt)
                {
                        string cols = "HouseName,Building,HouseAddress,RentSale,HousePrice,PriceUnit,HouseDirection,HouseLayout,OwnerId,HouseFloor,HouseArea,HouseState,Remark";
                        foreach (DataColumn dc in dt.Columns)
                        {
                                dc.ColumnName = GetDtColumnName(dc.ColumnName);
                        }
                        dt.Columns.Add("OwnerId", typeof(int));
                        dt.Columns.Add("HouseState", typeof(string));
                        OwnerDAL ownerDAL = new OwnerDAL();
                        foreach (DataRow dr in dt.Rows)
                        {
                                SqlParameter[] paras0 =
                                   {
                                        new SqlParameter("@ownerName",dr["OwnerName"].ToString()),
                                        new SqlParameter("@ownerPhone",dr["OwnerPhone"].ToString())
                                  };
                                int ownerId = 0;
                                HouseOwnerInfo owner = ownerDAL.GetModel("OwnerName=@ownerName and OwnerPhone=@ownerPhone", "OwnerId", paras0);
                                if (owner != null)
                                        ownerId = owner.OwnerId;
                                dr["OwnerId"] = ownerId;
                                if (dr["RentSale"].ToString() == "出租")
                                        dr["HouseState"] = "未出租";
                                else
                                        dr["HouseState"] = "未出售";
                        }
                        List<HouseInfo> houseList = DbConvert.DataTableToList<HouseInfo>(dt, cols);
                        if (houseList.Count > 0)
                        {
                                List<CommandInfo> list = new List<CommandInfo>();
                                foreach (HouseInfo house in houseList)
                                {
                                        if (!ExistsHouse(house.HouseName))
                                        {
                                                SqlModel insert = CreateSql.CreateInsertSql(house, cols, 0);
                                                list.Add(new CommandInfo()
                                                {
                                                        CommandText = insert.Sql,
                                                        IsProc = false,
                                                        Paras = insert.Paras
                                                });
                                        }
                                }
                                return SqlHelper.ExecuteTrans(list);
                        }
                        return false;
                }
                /// <summary>
                /// 获取对应的列名
                /// </summary>
                /// <param name="zwStr"></param>
                /// <returns></returns>
                private string GetDtColumnName(string zwStr)
                {
                        string reName = "";
                        switch (zwStr)
                        {
                                case "房屋名称": reName = "HouseName"; break;
                                case "所属楼宇": reName = "Building"; break;
                                case "房屋地址": reName = "HouseAddress"; break;
                                case "租售类别": reName = "RentSale"; break;
                                case "价格": reName = "HousePrice"; break;
                                case "单位": reName = "PriceUnit"; break;
                                case "朝向": reName = "HouseDirection"; break;
                                case "户型": reName = "HouseLayout"; break;
                                case "业主": reName = "OwnerName"; break;
                                case "电话": reName = "OwnerPhone"; break;
                                case "楼层": reName = "HouseFloor"; break;
                                case "面积": reName = "HouseArea"; break;
                                case "备注": reName = "Remark"; break;
                        }
                        return reName;
                }

                /// <summary>
                /// 判断房屋是否已存在
                /// </summary>
                /// <param name="houseName"></param>
                /// <returns></returns>
                public bool ExistsHouse(string houseName)
                {
                        return ExistsByName("HouseName", houseName);
                }

                /// <summary>
                /// 修改房屋发布状态
                /// </summary>
                /// <param name="houseIds"></param>
                /// <param name="isPublish"></param>
                /// <param name="loginUser"></param>
                /// <returns></returns>
                public bool UpdateHousePublishState(List<int> houseIds, int isPublish, string loginUser)
                {
                        string strIds = string.Join(",", houseIds);
                        string sql = $"update HouseInfos set IsPublish={isPublish},PublishTime=@publishTime,PublishUser=@publishUser where HouseId in ({strIds})";
                        SqlParameter[] paras = {
                                new SqlParameter("@publishTime",DateTime.Now),
                                new SqlParameter("@publishUser",loginUser)
                            };
                        if (isPublish == 0)//取消发布
                        {
                                paras[0].Value = DBNull.Value;//数据库中的NULL
                                paras[1].Value = DBNull.Value;
                        }

                        List<CommandInfo> sqlList = new List<CommandInfo>();
                        sqlList.Add(new CommandInfo()
                        {
                                CommandText = sql,
                                IsProc = false,
                                Paras = paras
                        });
                        return SqlHelper.ExecuteTrans(sqlList);//事务执行修改
                }

                /// <summary>
                /// 获取指定的房屋信息
                /// </summary>
                /// <param name="houseId"></param>
                /// <returns></returns>
                public HouseInfo GetHouseInfo(int houseId)
                {
                      string cols =PropertyHelper.GetColNames<HouseInfo>("IsDeleted");
                      return   GetById(houseId, cols);
                }

                /// <summary>
                /// 获取指定房屋编号中已发布的房屋数
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public int GetHousePublishedCount(List<int> houseIds)
                {
                        string strIds = string.Join(",", houseIds);
                        string sql = $"select count(1) from HouseInfos where IsPublish=1 and HouseId in ({strIds})";
                        object oCount = SqlHelper.ExecuteScalar(sql, 1);
                        return oCount.GetInt();
                }
        }
}
