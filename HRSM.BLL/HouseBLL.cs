using Common;
using HRSM.DAL;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public class HouseBLL
        {
                ViewHouseDAL vhouseDAL = new ViewHouseDAL();
                HouseDAL houseDAL = new HouseDAL();
                HouseTradeDAL htDAL = new HouseTradeDAL();
                ViewHouseCountSatisticsDAL vhcSatDAL = new ViewHouseCountSatisticsDAL();
                ViewSaleHouseStatisticsDAL vshSatDAL = new ViewSaleHouseStatisticsDAL();
                /// <summary>
                /// 添加房屋信息
                /// </summary>
                /// <param name="houseInfo"></param>
                /// <returns></returns>
                public bool AddHouseInfo(HouseInfo houseInfo)
                {
                        return houseDAL.AddHouseInfo(houseInfo);
                }

                /// <summary>
                /// 修改房屋信息
                /// </summary>
                /// <param name="houseInfo"></param>
                /// <returns></returns>
                public bool UpdateHouseInfo(HouseInfo houseInfo)
                {
                        return houseDAL.UpdateHouseInfo(houseInfo);
                }
                /// <summary>
                /// 条件查询已发布房屋列表
                /// </summary>
                /// <param name="houseName"></param>
                /// <param name="rsType"></param>
                /// <param name="direction"></param>
                /// <param name="layout"></param>
                /// <returns></returns>
                public List<ViewHouseInfo> GetShowHouseList(string houseName, string rsType, string direction, string layout)
                {
                        List<ViewHouseInfo> houseList = vhouseDAL.GetShowHouseList(houseName, rsType, direction, layout);
                        //图片   路径问题
                        foreach (ViewHouseInfo model in houseList)
                        {
                                if (!string.IsNullOrEmpty(model.HousePic))
                                {
                                        model.HousePic = "pack://siteoforigin:,,,/"+ model.HousePic;
                                }
                                else
                                        model.HousePic = "../imgs/house.jpg";
                        }
                        return houseList;
                }

                public ViewHouseInfo GetHouseInfo(int houseId)
                {
                        return vhouseDAL.GetHouseInfo(houseId);
                }

                public HouseInfo GetHouse(int houseId)
                {
                        return houseDAL.GetHouseInfo(houseId);
                }

                /// <summary>
                /// 条件查询房屋信息列表
                /// </summary>
                /// <param name="keywords"></param>
                /// <param name="ownerType"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<ViewHouseInfo> GetHouseList(string keywords, string rentSaleName, string houseDirection, string houseLayout, string houseState, int isPublish,bool isShowDel)
                {
                        int isDeleted = isShowDel ? 1 : 0;
                        return vhouseDAL.GetHouseList(keywords, rentSaleName, houseDirection, houseLayout, houseState, isPublish, isDeleted);
                }


                /// <summary>
                ///  批量导入房屋数据
                /// </summary>
                /// <param name="excelFile">excel文件路径</param>
                /// <param name="sheetName">sheet名称</param>
                /// <param name="isFirstRowColumn">第一行是否列名</param>
                /// <returns></returns>
                public int ImportHouseData(string excelFile, string sheetName, bool isFirstRowColumn)
                {
                        DataTable dt = ExcelHelper.ExcelToDataTable(excelFile, sheetName, isFirstRowColumn);
                        if (dt.Rows.Count > 0)
                        {
                                bool bl = houseDAL.AddHouseInfos(dt);
                                if (bl)
                                        return 1;
                                else
                                        return 0;
                        }
                        else
                                return -1;
                }

                /// <summary>
                /// 房屋发布
                /// </summary>
                /// <param name="houseIds"></param>
                /// <param name="pubUser"></param>
                /// <returns></returns>
                public bool PublishHouse(List<int> houseIds, string pubUser)
                {
                        return houseDAL.UpdateHousePublishState(houseIds, 1, pubUser);
                }

                public bool PublishHouse(int houseId, string pubUser)
                {
                        List<int> houseIds = new List<int>();
                        houseIds.Add(houseId);
                        return PublishHouse(houseIds, pubUser);
                }

                /// <summary>
                /// 取消发布
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public bool UnPublishHouse(List<int> houseIds)
                {
                        return houseDAL.UpdateHousePublishState(houseIds, 0, "");
                }

                public bool UnPublishHouse(int houseId)
                {
                        List<int> houseIds = new List<int>();
                        houseIds.Add(houseId);
                        return UnPublishHouse(houseIds);
                }


                /// <summary>
                /// 获取指定房屋中已发布房屋数
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public List<int> GetPublishedHouseIds(List<int> houseIds)
                {
                        List<int> hasHouseIds = new List<int>();
                        foreach (int houseId in houseIds)
                        {
                                if (IsHousePublished(houseId))
                                        hasHouseIds.Add(houseId);
                        }
                        return hasHouseIds;

                }

                /// <summary>
                /// 判断指定房屋是否已发布
                /// </summary>
                /// <param name="houseId"></param>
                /// <returns></returns>
                public bool IsHousePublished(int houseId)
                {
                        List<int> houseIds = new List<int>();
                        houseIds.Add(houseId);
                        return houseDAL.GetHousePublishedCount(houseIds) > 0;

                }

                /// <summary>
                /// 获取已交易的房屋数
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public List<int> GetTradeHouseIds(List<int> houseIds)
                {
                        List<int> hasTradeHouseIds = new List<int>();
                        foreach (int houseId in houseIds)
                        {
                                if (IsTradeHouse(houseId))
                                        hasTradeHouseIds.Add(houseId);
                        }
                        return hasTradeHouseIds;
                }

                /// <summary>
                /// 判断是否为交易房屋
                /// </summary>
                /// <param name="houseId"></param>
                /// <returns></returns>
                public bool IsTradeHouse(int houseId)
                {
                        List<int> houseIds = new List<int>();
                        houseIds.Add(houseId);
                        return htDAL.GetTradeHouseCount(houseIds) > 0;
                }

                /// <summary>
                /// 判断房屋是否已存在
                /// </summary>
                /// <param name="houseName"></param>
                /// <returns></returns>
                public bool ExistsHouse(string houseName)
                {
                        return houseDAL.ExistsHouse(houseName);
                }


                /// <summary>
                /// 逻辑删除房屋列表
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public bool LogicDelHouseList(List<int> houseIds)
                {
                        return houseDAL.DeleteList(houseIds, 0, 1);
                }
                /// <summary>
                /// 恢复房屋列表
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public bool RecoverHouseList(List<int> houseIds)
                {
                        return houseDAL.DeleteList(houseIds, 0, 0);
                }
                /// <summary>
                /// 移除房屋列表
                /// </summary>
                /// <param name="houseIds"></param>
                /// <returns></returns>
                public bool RemoveHouseList(List<int> houseIds)
                {
                        return houseDAL.DeleteList(houseIds, 1, 2);
                }

                #region 房屋数据统计

                /// <summary>
                /// 获取房屋统计数据
                /// </summary>
                /// <returns></returns>
                public ViewHouseCountSatisticsInfo GetHouseCountStatistics()
                {
                        return vhcSatDAL.GetHouseCountStatistics();
                }

                /// <summary>
                /// 获取所有销售员的销售量统计数据
                /// </summary>
                /// <returns></returns>
                public List<ViewSaleHouseStatisticsInfo> GetSaleHouseStatisticsData()
                {
                        return vshSatDAL.GetSaleHouseStatisticsData();
                }

                /// <summary>
                /// 条件获取销售统计列表
                /// </summary>
                /// <returns></returns>
                public List<ViewSaleHouseStatisticsInfo> GetSaleTimeHouseStatisticsData(string saleName, DateTime? startTime, DateTime? endTime)
                {
                        return vshSatDAL.GetSaleTimeHouseStatisticsData(saleName, startTime, endTime);
                }

                #endregion
        }
}
