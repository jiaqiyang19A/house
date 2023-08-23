using Common;
using DbUtility;
using HRSM.DAL.Base;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public class CustomerFollowUpLogDAL:BaseDAL<CustomerFollowUpLogInfo>
        {

                public bool  AddCustomerFLogInfo(CustomerFollowUpLogInfo custFollowUpLogInfo)
                {
                        
                        string cols = PropertyHelper.GetColNames<CustomerFollowUpLogInfo>("FLogId,IsDeleted");
                        return AddOrUpdateLogAndCustState(custFollowUpLogInfo, cols, 1);
                }
    
                public bool UpdateCustomerFLogInfo(CustomerFollowUpLogInfo custFollowUpLogInfo)
                {
                        string cols = PropertyHelper.GetColNames<CustomerFollowUpLogInfo>("IsDeleted");
                        return AddOrUpdateLogAndCustState(custFollowUpLogInfo, cols, 2);
                }

                private bool AddOrUpdateLogAndCustState(CustomerFollowUpLogInfo logInfo,string cols,int actType)
                {
                        List<CommandInfo> list = new List<CommandInfo>();
                        SqlModel inModel = null;
                        if (actType == 2)
                                inModel = CreateSql.CreateUpdateSql(logInfo, cols, "");
                        else if(actType ==1)
                                inModel = CreateSql.CreateInsertSql(logInfo, cols, 0);
                        list.Add(new CommandInfo()
                        {
                                CommandText = inModel.Sql,
                                IsProc = false,
                                Paras = inModel.Paras
                        });
                 
                        if(logInfo.FollowUpState!="跟进中")
                        {
                                list.Add(new CommandInfo()
                                {
                                        CommandText = $"update CustomerRequestInfos set RequestState='{logInfo.FollowUpState}' where CustRequestId={logInfo.CustRequestId}",
                                        IsProc = false
                                });
                                CustRequestDAL crDAL = new CustRequestDAL();
                                CustomerRequestInfo requestInfo = crDAL.GetCustomerRequestInfo(logInfo.CustRequestId);
                                if (requestInfo != null)
                                {
                                        string sql = $"update CustomerInfos set CustomerState='普通客户' where CustomerId={requestInfo.CustomerId}";
                                        list.Add(new CommandInfo()
                                        {
                                                CommandText = sql,
                                                IsProc = false
                                        });
                                }
                        }
                        return SqlHelper.ExecuteTrans(list);

                }

                public CustomerFollowUpLogInfo GetCustomerFULogInfo(int fLogId)
                {
                        string cols = PropertyHelper.GetColNames<CustomerFollowUpLogInfo>("IsDeleted");
                        return  GetById(fLogId, cols);
                }
        }
}
