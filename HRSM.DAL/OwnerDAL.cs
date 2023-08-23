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
        public class OwnerDAL : BaseDAL<HouseOwnerInfo>
        {
                /// <summary>
                /// 新增业主信息
                /// </summary>
                /// <param name="ownerInfo"></param>
                /// <returns></returns>
                public bool AddOwnerInfo(HouseOwnerInfo ownerInfo)
                {
                        string cols = PropertyHelper.GetColNames<HouseOwnerInfo>("OwnerId,IsDeleted,CreateTime");
                        return Add(ownerInfo, cols, 0) > 0;
                }

                /// <summary>
                /// 更新业主信息
                /// </summary>
                /// <param name="ownerInfo"></param>
                /// <returns></returns>
                public bool UpdateOwnerInfo(HouseOwnerInfo ownerInfo)
                {
                        string cols = PropertyHelper.GetColNames<HouseOwnerInfo>("IsDeleted,CreateTime");
                        return Update(ownerInfo, cols);
                }

                /// <summary>
                /// 条件查询业主信息
                /// </summary>
                /// <param name="keywords"></param>
                /// <param name="ownerType"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<HouseOwnerInfo> GetOwnerList(string keywords, string ownerType, int isDeleted)
                {
                        string cols = "OwnerId,OwnerName,OwnerType,Contactor,OwnerPhone,OwnerAddress";
                        string strWhere = $"IsDeleted={isDeleted}";
                        List<SqlParameter> listParas = new List<SqlParameter>();
                        if (!string.IsNullOrEmpty(keywords))
                        {
                                strWhere += " and (OwnerName like @keywords or Contactor like @keywords or OwnerPhone like @keywords or OwnerAddress like @keywords)";
                                listParas.Add(new SqlParameter("@keywords", $"%{keywords}%"));
                        }
                        if (!string.IsNullOrEmpty(ownerType))
                        {
                                strWhere += " and OwnerType=@ownerType";
                                listParas.Add(new SqlParameter("@ownerType", ownerType));
                        }
                        return GetModelList(strWhere, cols, "", listParas.ToArray());
                }

                /// <summary>
                /// 获取指定业主的房屋数
                /// </summary>
                /// <param name="ownerId"></param>
                /// <returns></returns>
                public int GetOwnerHouseCount(int ownerId)
                {
                        string sql = $"select count(1) from HouseInfos where OwnerId={ownerId} and IsDeleted=0";
                        object oCount = SqlHelper.ExecuteScalar(sql, 1);
                        return oCount.GetInt();
                }

                /// <summary>
                /// 批量导入业主数据
                /// </summary>
                /// <param name="dt">导入到datatable中的业主信息   列名：中文   事先提供excel模板</param>
                /// <returns></returns>
                public bool AddOwnerInfos(DataTable dt)
                {
                        string cols = "OwnerName,OwnerType,Contactor,OwnerPhone,OwnerAddress,Remark";
                        foreach (DataColumn dc in dt.Columns)
                        {
                                dc.ColumnName = GetDtColumnName(dc.ColumnName);
                        }
                        List<HouseOwnerInfo> ownerList = DbConvert.DataTableToList<HouseOwnerInfo>(dt, cols);
                        if (ownerList.Count > 0)
                        {
                                List<CommandInfo> list = new List<CommandInfo>();
                                foreach (HouseOwnerInfo owner in ownerList)
                                {
                                        if (!Exists(owner.OwnerName, owner.OwnerPhone))
                                        {
                                                SqlModel insert = CreateSql.CreateInsertSql(owner, cols, 0);
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
                                case "业主名": reName = "OwnerName"; break;
                                case "类型": reName = "OwnerType"; break;
                                case "联系人": reName = "Contactor"; break;
                                case "联系电话": reName = "OwnerPhone"; break;
                                case "业主地址": reName = "OwnerAddress"; break;
                                case "备注": reName = "Remark"; break;
                        }
                        return reName;
                }

                /// <summary>
                /// 判断业主是否已存在
                /// </summary>
                /// <param name="ownerName"></param>
                /// <param name="ownerPhone"></param>
                /// <returns></returns>
                public bool Exists(string ownerName, string ownerPhone)
                {
                        SqlParameter[] paras =
                        {
                new SqlParameter("@ownerName",ownerName),
                new SqlParameter("@ownerPhone",ownerPhone)
            };
                        return Exists("OwnerName=@ownerName and OwnerPhone=@ownerPhone and IsDeleted=0", paras);
                }

                /// <summary>
                /// 获取指定的业主信息
                /// </summary>
                /// <param name="ownerId"></param>
                /// <returns></returns>
                public HouseOwnerInfo GetOwnerInfo(int ownerId)
                {
                        string cols = "OwnerId,OwnerName,OwnerType,Contactor,OwnerPhone,OwnerAddress,Remark";
                        return GetById(ownerId, cols);
                }

                /// <summary>
                /// 获取所有业主列表（下拉框）
                /// </summary>
                /// <returns></returns>
                public List<HouseOwnerInfo> GetAllOwners()
                {
                        return GetModelList("OwnerId,OwnerName", "", 0);
                }
        }
}
