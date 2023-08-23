using HRSM.DAL;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.BLL
{
        public   class CustRequestBLL
        {
                private ViewCustReuqestDAL vcrDAL = new ViewCustReuqestDAL();
                private CustRequestDAL requestDAL = new CustRequestDAL();

                /// <summary>
                /// 添加客户需求信息
                /// </summary>
                /// <param name="custRequestInfo"></param>
                /// <returns></returns>
                public bool AddCustomerRequestInfo(CustomerRequestInfo custRequestInfo)
                {
                        return requestDAL.AddCustomerRequestInfo(custRequestInfo);
                }

                /// <summary>
                /// 修改客户需求信息
                /// </summary>
                /// <param name="custRequestInfo"></param>
                /// <param name="oldCustId"></param>
                /// <returns></returns>
                public bool UpdateCustomerRequestInfo(CustomerRequestInfo custRequestInfo, int oldCustId)
                {
                        return requestDAL.UpdateCustomerRequestInfo(custRequestInfo, oldCustId);
                }

                public bool UpdateCustomerRequestInfo(CustomerRequestInfo custRequestInfo)
                {
                        return requestDAL.UpdateCustomerRequestInfo(custRequestInfo, 0);
                }

                /// <summary>
                /// 获取指定的客户需求信息
                /// </summary>
                /// <param name="custRequestId"></param>
                /// <returns></returns>
                public CustomerRequestInfo GetCustomerRequestInfo(int custRequestId)
                {
                        return requestDAL.GetCustomerRequestInfo(custRequestId);
                }

                /// <summary>
                /// 检查客户需求是否已存在
                /// </summary>
                /// <param name="custId"></param>
                /// <param name="requestContent"></param>
                /// <returns></returns>
                public bool ExistsRequest(int custId, string requestContent)
                {
                        return requestDAL.ExistsRequest(custId, requestContent);
                }

                /// <summary>
                /// 条件查询客户需求列表
                /// </summary>
                /// <param name="custId"></param>
                /// <param name="custName"></param>
                /// <param name="custType"></param>
                /// <param name="followUpUser"></param>
                /// <param name="content"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<ViewCustomerRequestInfo> GetCustRequests(int custId, string custName, string custType, string followUpUser, string content, bool showDel)
                {
                        int isDeleted = showDel ? 1 : 0;
                        return vcrDAL.GetCustRequests(custId, custName, custType, followUpUser, content, isDeleted);
                }

                /// <summary>
                /// 获取成功或跟进中的需求编号集合
                /// </summary>
                /// <param name="custRequestIds">选择的需求编号集合</param>
                /// <returns></returns>
                public List<int> GetSucAndFollowUpRequestIds(List<int> custRequestIds)
                {
                      return requestDAL.GeRequestIdsByCustIdAndState(custRequestIds, "'成交','跟进中'");
                }

                /// <summary>
                /// 判断一个需求是否在跟进中
                /// </summary>
                /// <param name="requestId"></param>
                /// <returns></returns>
                public bool IsFollowUpRequest(int requestId)
                {
                        CustomerRequestInfo requestInfo = GetCustomerRequestInfo(requestId);
                        if(requestInfo!=null)
                        {
                                if (requestInfo.RequestState != "跟进中")
                                        return false;

                        }
                        return true;
                }

                /// <summary>
                /// 批量删除需求信息
                /// </summary>
                /// <param name="custRequestIds"></param>
                /// <returns></returns>
                public bool LogicDelCustomerRequestList(List<int> custRequestIds)
                {
                        return requestDAL.UpdateCustRequestInfosState(custRequestIds, 0, 1);
                }

                /// <summary>
                /// 批量恢复需求信息
                /// </summary>
                /// <param name="custRequestIds"></param>
                /// <returns></returns>
                public bool RecoverCustomerRequestList(List<int> custRequestIds)
                {
                        return requestDAL.UpdateCustRequestInfosState(custRequestIds, 0, 0);
                }

                /// <summary>
                /// 批量移除需求信息
                /// </summary>
                /// <param name="custRequestIds"></param>
                /// <returns></returns>
                public bool RemoveCustomerRequestList(List<int> custRequestIds)
                {
                        return requestDAL.UpdateCustRequestInfosState(custRequestIds, 1, 2);
                }

                /// <summary>
                /// 获取意向需求列表
                /// </summary>
                /// <returns></returns>
                public List<CustomerRequestInfo> GetCustIntentedRequests(bool isIntended)
                {
                        if(isIntended)
                          return requestDAL.GetCustRequests(1);
                        else
                                return requestDAL.GetCustRequests(-1);
                }
        }
}
