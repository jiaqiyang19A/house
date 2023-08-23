using HRSM.DAL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public class CustomerBLL
        {
                private CustomerDAL custDAL = new CustomerDAL();
                private HouseTradeDAL htDAL = new HouseTradeDAL();

                /// <summary>
                /// 添加客户信息
                /// </summary>
                /// <param name="custInfo"></param>
                /// <returns></returns>
                public bool AddCustomerInfo(CustomerInfo custInfo)
                {
                        return custDAL.AddCustomerInfo(custInfo);
                }

                /// <summary>
                /// 修改客户信息
                /// </summary>
                /// <param name="custInfo"></param>
                /// <returns></returns>
                public bool UpdateCustomerInfo(CustomerInfo custInfo)
                {
                        return custDAL.UpdateCustomerInfo(custInfo);
                }


                /// <summary>
                /// 获取所有意向客户列表
                /// </summary>
                /// <returns></returns>
                public List<CustomerInfo> GetIntentedCustomers()
                {
                        return custDAL.GetAllCustomers(1);
                }

                /// <summary>
                /// 获取所有普通客户列表
                /// </summary>
                /// <returns></returns>
                public List<CustomerInfo> GetNormalCustomers(int actType)
                {
                        if (actType == 1)
                                return custDAL.GetAllCustomers(0);
                        else
                                return custDAL.GetAllCustomers(-1);
                }

                /// <summary>
                /// 条件查询客户列表
                /// </summary>
                /// <param name="keywords"></param>
                /// <param name="custType"></param>
                /// <param name="custState"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<CustomerInfo> GetCustList(string keywords, string custType, string custState, bool  showDel)
                {
                        int isDeleted = showDel ? 1 : 0;
                        return custDAL.GetCustList(keywords, custType, custState, isDeleted);
                }

                /// <summary>
                /// 指定客户列表中意向客户的编号
                /// </summary>
                /// <param name="custIds"></param>
                /// <returns></returns>
                public List<int>  GetIntendedCustomerIds(List<int> custIds)
                {
                        return custDAL.GetCustIdstByState(custIds, "意向客户");
                }
                /// <summary>
                /// 指定客户列表中包含交易客户的编号
                /// </summary>
                /// <param name="custIds"></param>
                /// <returns></returns>
                public List<int>  GetTradeCustomerIds(List<int> custIds)
                {
                        return htDAL.GetTradeCustomerIds(custIds);
                }
                /// <summary>
                /// 逻辑删除客户信息
                /// </summary>
                /// <param name="custIds"></param>
                /// <returns></returns>
               public bool   LogicDelCustomerList(List<int> custIds)
                {
                        return custDAL.DeleteList(custIds, 0, 1);
                }
                /// <summary>
                /// 恢复客户信息
                /// </summary>
                /// <param name="custIds"></param>
                /// <returns></returns>
                public bool RecoverCustomerList(List<int> custIds)
                {
                        return custDAL.DeleteList(custIds, 0, 0);
                }
                /// <summary>
                /// 移除客户信息
                /// </summary>
                /// <param name="custIds"></param>
                /// <returns></returns>
                public bool RemoveCustomerList(List<int> custIds)
                {
                        return custDAL.DeleteList(custIds, 1,2);
                }

                /// <summary>
                /// 根据指定的客户编号 获取客户信息
                /// </summary>
                /// <param name="custId"></param>
                /// <returns></returns>
                public CustomerInfo GetCustomerInfo(int custId)
                {
                        return custDAL.GetCustomerInfo(custId);
                }

                /// <summary>
                /// 判断客户是否已存在
                /// </summary>
                /// <param name="customerName"></param>
                /// <param name="customerPhone"></param>
                /// <returns></returns>
                public bool ExistsCustomer(string customerName, string customerPhone)
                {
                        return custDAL.ExistsCustomer(customerName, customerPhone);
                }
        }
}
