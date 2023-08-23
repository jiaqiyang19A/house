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
    public class CustRequestDAL : BaseDAL<CustomerRequestInfo>
    {
        /// <summary>
        /// 添加客户需求信息
        /// </summary>
        /// <param name="custRequestInfo"></param>
        /// <returns></returns>
        public bool AddCustomerRequestInfo(CustomerRequestInfo custRequestInfo)
        {
            string cols = "CustomerId,RequestContent,FollowUpUser";
            List<CommandInfo> list = new List<CommandInfo>();
            //添加客户需求 
            SqlModel inModel = CreateSql.CreateInsertSql(custRequestInfo, cols, 0);
            list.Add(new CommandInfo()
            {
                CommandText = inModel.Sql,
                IsProc = false,
                Paras = inModel.Paras
            });
            //修改客户状态
            string sql = $"update CustomerInfos set CustomerState='意向客户' where CustomerId={custRequestInfo.CustomerId}";
            list.Add(new CommandInfo()
            {
                CommandText = sql,
                IsProc = false
            });
            return SqlHelper.ExecuteTrans(list);
        }

        /// <summary>
        /// 修改客户需求信息
        /// </summary>
        /// <param name="custRequestInfo"></param>
        /// <param name="oldCustId"></param>
        /// <returns></returns>
        public bool UpdateCustomerRequestInfo(CustomerRequestInfo custRequestInfo, int oldCustId)
        {
            string cols = "CustRequestId,CustomerId,RequestContent,FollowUpUser";
            List<CommandInfo> list = new List<CommandInfo>();
            //修改客户需求 
            SqlModel upModel = CreateSql.CreateUpdateSql(custRequestInfo, cols, "");
            list.Add(new CommandInfo()
            {
                CommandText = upModel.Sql,
                IsProc = false,
                Paras = upModel.Paras
            });
            if (oldCustId > 0)
            {
                //修改新客户状态
                string sql = $"update CustomerInfos set CustomerState='意向客户' where CustomerId={custRequestInfo.CustomerId}";
                list.Add(new CommandInfo()
                {
                    CommandText = sql,
                    IsProc = false
                });
                //修改原来客户状态
                string sql1 = $"update CustomerInfos set CustomerState='普通客户' where CustomerId={oldCustId}";
                list.Add(new CommandInfo()
                {
                    CommandText = sql1,
                    IsProc = false
                });

            }

            return SqlHelper.ExecuteTrans(list);
        }

        /// <summary>
        /// 获取指定客户需求列表中的指定状态的需求编号
        /// </summary>
        /// <param name="custRequestIds"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        public List<int> GeRequestIdsByCustIdAndState(List<int> custRequestIds, string states)
        {
            string strIds = string.Join(",", custRequestIds);
            string sql = $"select CustRequestId from CustomerRequestInfos where CustRequestId in ({strIds}) and RequestState in ({states})";
            DataTable dt = SqlHelper.GetDataTable(sql, 1);
            List<int> requestIds = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                requestIds.Add(dr["CustRequestId"].GetInt());
            }
            return requestIds;
        }

        /// <summary>
        /// 修改客户需求信息删除状态  
        /// </summary>
        /// <param name="custRequestIds"></param>
        /// <param name="delType"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        public bool UpdateCustRequestInfosState(List<int> custRequestIds, int delType, int isDeleted)
        {
            List<string> sqlList = new List<string>();
            string[] tableNames = { "CustomerRequestInfos", "CustomerFollowUpLogInfos" };
            sqlList = GetDeleteListSql(delType, custRequestIds, isDeleted, tableNames);
            return SqlHelper.ExecuteTrans(sqlList);
        }
        /// <summary>
        /// 获取指定的客户需求信息
        /// </summary>
        /// <param name="custRequestId"></param>
        /// <returns></returns>
        public CustomerRequestInfo GetCustomerRequestInfo(int custRequestId)
        {
            string cols = PropertyHelper.GetColNames<CustomerRequestInfo>("IsDeleted");
            return GetById(custRequestId, cols);
        }

        /// <summary>
        /// 检查客户需求是否已存在
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="requestContent"></param>
        /// <returns></returns>
        public bool ExistsRequest(int custId, string requestContent)
        {
            SqlParameter[] paras =
            {
                                new SqlParameter("requestContent",requestContent)
                        };
            string strWhere = $"CustomerId={custId} and RequestContent=@requestContent and RequestState='跟进中' and IsDeleted=0 ";
            return Exists(strWhere, paras);
        }
        /// <summary>
        /// 获取指定状态 的需求列表
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<CustomerRequestInfo> GetCustRequests(int state)
        {
            string strWhere = "IsDeleted=0 ";
            if (state == 1)
            {
                strWhere += " and RequestState='跟进中'";
            }
            else if (state == 0)
            {
                strWhere += " and RequestState='放弃'";
            }
            else if (state == 2)
            {
                strWhere += " and RequestState='成功'";//2
            }
            return GetModelList(strWhere, "CustRequestId,RequestContent", "CreateTime desc");
        }

    }
}
