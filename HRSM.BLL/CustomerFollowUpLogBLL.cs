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
        public class CustomerFollowUpLogBLL
        {
                private ViewCustomerFollowUpLogDAL vcustFULogDAL = new ViewCustomerFollowUpLogDAL();
                private CustomerFollowUpLogDAL custFULogDAL = new CustomerFollowUpLogDAL();

                /// <summary>
                /// 新增日志信息
                /// </summary>
                /// <param name="custFollowUpLogInfo"></param>
                /// <returns></returns>
                public bool AddCustomerFLogInfo(CustomerFollowUpLogInfo custFollowUpLogInfo)
                {
                        return custFULogDAL.AddCustomerFLogInfo(custFollowUpLogInfo);
                }
                /// <summary>
                /// 修改日志信息
                /// </summary>
                /// <param name="custFollowUpLogInfo"></param>
                /// <returns></returns>
                public bool UpdateCustomerFLogInfo(CustomerFollowUpLogInfo custFollowUpLogInfo)
                {
                        return custFULogDAL.UpdateCustomerFLogInfo(custFollowUpLogInfo);
                }

                /// <summary>
                /// 条件查询客户日志列表
                /// </summary>
                /// <param name="requestId"></param>
                /// <param name="custName"></param>
                /// <param name="followUpUser"></param>
                /// <param name="requestContent"></param>
                /// <param name="fContent"></param>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public List<ViewCustomerFollowUpLogInfo> GetCustFLogs(int requestId, string custName, string followUpUser, string requestContent, string fContent,bool showDel)
                {
                        int isDeleted = showDel ? 1 : 0;
                        return vcustFULogDAL.GetCustFLogs(requestId, custName, followUpUser, requestContent, fContent, isDeleted);
                }

                /// <summary>
                /// 获取跟进中或已成交的日志编号（与之对应的需求是成交）
                /// </summary>
                /// <param name="selList"></param>
                /// <returns></returns>
               public   List<int> GetUseOrSuccessLog(List<ViewCustomerFollowUpLogInfo> selList)
                {
                        List<int> hasLogIds = new List<int>();
                        foreach (var log in selList)
                        {
                                if(log.RequestState=="成交")
                                {
                                        hasLogIds.Add(log.FLogId);
                                }
                        }
                        return hasLogIds;
                }

                /// <summary>
                /// 批量逻辑删除客户日志列表
                /// </summary>
                /// <param name="fLogIds"></param>
                /// <returns></returns>
                public bool LogicDelCustomerFULogList(List<int> fLogIds)
                {
                        return custFULogDAL.DeleteList(fLogIds, 0, 1);
                }
                /// <summary>
                /// 批量恢复客户日志列表
                /// </summary>
                /// <param name="fLogIds"></param>
                /// <returns></returns>
                public bool RecoverCustomerFULogList(List<int> fLogIds)
                {
                        return custFULogDAL.DeleteList(fLogIds, 0, 0);
                }
                /// <summary>
                /// 批量移除客户日志列表
                /// </summary>
                /// <param name="fLogIds"></param>
                /// <returns></returns>
                public bool RemoveCustomerFULogList(List<int> fLogIds)
                {
                        return custFULogDAL.DeleteList(fLogIds, 1, 2);
                }

                /// <summary>
                /// 获取指定的客户跟进日志信息
                /// </summary>
                /// <param name="fLogId"></param>
                /// <returns></returns>
                public CustomerFollowUpLogInfo GetCustomerFULogInfo(int fLogId)
                {
                        return custFULogDAL.GetCustomerFULogInfo(fLogId);
                }
        }
}
