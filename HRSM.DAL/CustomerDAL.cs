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
       public  class CustomerDAL:BaseDAL<CustomerInfo>
        {
                /// <summary>
                /// 添加客户信息
                /// </summary>
                /// <param name="custInfo"></param>
                /// <returns></returns>
                public bool AddCustomerInfo(CustomerInfo custInfo)
                {
                        string cols = PropertyHelper.GetColNames<CustomerInfo>("CustomerId,IsDeleted,CustomerState,CreateTime");
                        return Add(custInfo, cols, 0)>0;
                }

                /// <summary>
                /// 修改客户信息
                /// </summary>
                /// <param name="custInfo"></param>
                /// <returns></returns>
                public bool UpdateCustomerInfo(CustomerInfo custInfo)
                {
                        string cols = PropertyHelper.GetColNames<CustomerInfo>("IsDeleted,CustomerState,CreateTime");
                        return Update(custInfo, cols) ;
                }



                /// <summary>
                /// 获取客户列表(根据客户状态)
                /// </summary>
                /// <param name="custState">0 普通客户  1-意向客户  -1  全部客户</param>
                /// <returns></returns>
                public List<CustomerInfo> GetAllCustomers(int custState)
                {
                        string cols = "CustomerId,CustomerName,CustomerPhone";
                        string strWhere = "IsDeleted=0";
                        if (custState>-1)
                                strWhere += $" and CustomerState = '{GetCustStateName(custState)}'";
                        return GetModelList(strWhere, cols, "");
                }

                private string GetCustStateName(int custState)
                {
                        return custState == 1 ? "意向客户" : "普通客户";
                }
                /// <summary>
                /// 条件查询客户列表
                /// </summary>
                /// <param name="keywords"></param>
                /// <param name="custType"></param>
                /// <param name="custState"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<CustomerInfo> GetCustList(string keywords, string custType, string custState,int  isDeleted)
                {
                        string cols = PropertyHelper.GetColNames<CustomerInfo>("Remark,IsDeleted");
                        string strWhere = $"IsDeleted={isDeleted}";
                        List<SqlParameter> listParas = new List<SqlParameter>();
                        if (!string.IsNullOrEmpty(keywords))
                        {
                                strWhere += " and (CustomerName like @keywords  or Contactor like @keywords or CustomerAddress  like @keywords or  CustomerPhone like @keywords)";
                                listParas.Add(new SqlParameter("@keywords", $"%{keywords}%"));
                        }
                        if (!string.IsNullOrEmpty(custType))
                        {
                                strWhere += " and CustomerType=@custType";
                                listParas.Add(new SqlParameter("@custType", custType));
                        }
                        if (!string.IsNullOrEmpty(custState))
                        {
                                strWhere += " and CustomerState=@custState";
                                listParas.Add(new SqlParameter("@custState", custState));
                        }
                        return GetModelList(strWhere, cols, "", listParas.ToArray());
                }

                /// <summary>
                /// 获取指定客户列表中的 指定状态的客户编号
                /// </summary>
                /// <param name="custIds"></param>
                /// <param name="custState"></param>
                /// <returns></returns>
                public List<int> GetCustIdstByState(List<int> custIds,string custState)
                {
                        string strIds = string.Join(",", custIds);
                        string sql = $"select CustomerId from CustomerInfos where CustomerState=@custState  and  CustomerId in ({strIds}) and IsDeleted=0";
                        SqlParameter paraState = new SqlParameter("@custState", custState);
                        DataTable dt = SqlHelper.GetDataTable(sql, 1,paraState);
                        List<int>reCustIds = new List<int>();
                        foreach (DataRow dr in dt.Rows)
                        {
                                reCustIds.Add(dr["CustomerId"].GetInt());
                        }
                        return reCustIds;
                }

                /// <summary>
                /// 根据指定的客户编号 获取客户信息
                /// </summary>
                /// <param name="custId"></param>
                /// <returns></returns>
                public CustomerInfo GetCustomerInfo(int custId)
                {
                        string cols = PropertyHelper.GetColNames<CustomerInfo>("IsDeleted");
                        return GetById(custId, cols);
                }

                /// <summary>
                /// 判断客户是否已存在
                /// </summary>
                /// <param name="customerName"></param>
                /// <param name="customerPhone"></param>
                /// <returns></returns>
               public bool  ExistsCustomer(string customerName,string  customerPhone)
                {
                        string strWhere = "CustomerName=@custName and CustomerPhone=@custPhone and IsDeleted=0";
                        SqlParameter[] paras =
                        {
                                new SqlParameter("@custName",customerName),
                                new SqlParameter("@custPhone",customerPhone)
                        };
                        return Exists(strWhere, paras);
                }



        }
}
