using Common;
using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public class OwnerBLL
        {
                OwnerDAL ownerDAL = new OwnerDAL();

                /// <summary>
                /// 新增业主信息
                /// </summary>
                /// <param name="ownerInfo"></param>
                /// <returns></returns>
                public bool AddOwnerInfo(HouseOwnerInfo ownerInfo)
                {
                        if (ownerInfo == null)
                                throw new Exception("业主信息不能为空！");
                        return ownerDAL.AddOwnerInfo(ownerInfo);
                }

                /// <summary>
                /// 更新业主信息
                /// </summary>
                /// <param name="ownerInfo"></param>
                /// <returns></returns>
                public bool UpdateOwnerInfo(HouseOwnerInfo ownerInfo)
                {
                        if (ownerInfo == null)
                                throw new Exception("业主信息不能为空！");
                        return ownerDAL.UpdateOwnerInfo(ownerInfo);
                }


                /// <summary>
                /// 判断指定业主是否有房屋
                /// </summary>
                /// <param name="ownerId"></param>
                /// <returns></returns>
                public bool IsOwnerHasHouses(int ownerId)
                {
                        int count = ownerDAL.GetOwnerHouseCount(ownerId);
                        return count > 0;
                }

                /// <summary>
                /// 返回已有房屋的业主编号
                /// </summary>
                /// <param name="ownerIds"></param>
                /// <returns></returns>
                public List<int> GetOwnersHasHousesIds(List<int> ownerIds)
                {
                        List<int> hasOwnerIds = new List<int>();
                        foreach (int ownerId in ownerIds)
                        {
                                if (IsOwnerHasHouses(ownerId))
                                        hasOwnerIds.Add(ownerId);
                        }
                        return hasOwnerIds;
                }

                /// <summary>
                /// 条件查询业主信息
                /// </summary>
                /// <param name="keywords"></param>
                /// <param name="ownerType"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<HouseOwnerInfo> GetOwnerList(string keywords, string ownerType, bool isShowDel)
                {
                        int isDeleted = isShowDel ? 1 : 0;
                        return ownerDAL.GetOwnerList(keywords, ownerType, isDeleted);
                }

                /// <summary>
                /// 判断业主是否已存在
                /// </summary>
                /// <param name="ownerName"></param>
                /// <param name="ownerPhone"></param>
                /// <returns></returns>
                public bool Exists(string ownerName, string ownerPhone)
                {
                        return ownerDAL.Exists(ownerName, ownerPhone);
                }

                /// <summary>
                /// 获取指定的业主信息
                /// </summary>
                /// <param name="ownerId"></param>
                /// <returns></returns>
                public HouseOwnerInfo GetOwnerInfo(int ownerId)
                {
                        return ownerDAL.GetOwnerInfo(ownerId);
                }

                /// <summary>
                /// 获取所有业主列表（下拉框）
                /// </summary>
                /// <returns></returns>
                public List<HouseOwnerInfo> GetAllOwners()
                {
                        return ownerDAL.GetAllOwners();
                }

                /// <summary>
                /// 业主信息导入
                /// </summary>
                /// <param name="excelFile"></param>
                /// <param name="sheetName"></param>
                /// <param name="isFirstRowColumn"></param>
                /// <returns></returns>
                public int ImportOwnerInfos(string excelFile, string sheetName, bool isFirstRowColumn)
                {
                        DataTable dt = ExcelHelper.ExcelToDataTable(excelFile, sheetName, isFirstRowColumn);
                        if (dt.Rows.Count > 0)
                        {
                                bool bl = ownerDAL.AddOwnerInfos(dt);
                                if (bl)
                                        return 1;
                                else
                                        return 0;
                        }
                        else
                                return -1;
                }

                /// <summary>
                /// 逻辑批量删除业主信息
                /// </summary>
                /// <param name="ownerIds"></param>
                /// <returns></returns>
                public bool LogicDelOwnerList(List<int> ownerIds)
                {
                        return ownerDAL.DeleteList(ownerIds, 0, 1);
                }

                /// <summary>
                ///批量恢复业主信息
                /// </summary>
                /// <param name="ownerIds"></param>
                /// <returns></returns>
                public bool RecoverOwnerList(List<int> ownerIds)
                {
                        return ownerDAL.DeleteList(ownerIds, 0, 0);
                }

                /// <summary>
                /// 批量移除业主信息
                /// </summary>
                /// <param name="ownerIds"></param>
                /// <returns></returns>
                public bool RemoveOwnerList(List<int> ownerIds)
                {
                        return ownerDAL.DeleteList(ownerIds, 1, 2);
                }
        }
}
