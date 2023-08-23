using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.CRM
{
        public class CustomerFollowUpLogInfoViewModel:InfoViewModelBase
        {
                CustRequestBLL custRequestBLL = new CustRequestBLL();
                CustomerFollowUpLogBLL custFULogBLL = new CustomerFollowUpLogBLL();
                public CustomerFollowUpLogInfoViewModel() { }
                public CustomerFollowUpLogInfoViewModel(int actType,int fULogId,int custRequestId)
                {
                        this.CloseWindowCmd = GetCloseWindowCmd("custFULogInfo");
                        this.ActType = actType;
                        this.FollowUpLogId = fULogId;
                        this.FollowUpUser = LoginUser;
                        this.CustRequestId = custRequestId;
                        switch (ActType)
                        {
                                case 1:
                                        if (custRequestBLL.IsFollowUpRequest(this.CustRequestId))
                                        {
                                                this.IsConfirmBtnEnabled = true;
                                        }
                                        else
                                        {
                                                this.IsConfirmBtnEnabled = false;
                                        }
                                        this.ConfirmBtnContent = "新增";
                                        this.FollowUpTime = DateTime.Now;
                                        if (CustRequestId > 0)
                                                this.IsCboRequestEnable = false;
                                        else
                                                this.IsCboRequestEnable = true;
                                        this.isIntended = true;
                                        this.FollowUpState = "跟进中";
                                        break;
                                case 2:
                                        this.ConfirmBtnContent = "修改";
                                        this.custFULogInfo = custFULogBLL.GetCustomerFULogInfo(fULogId);
                                        this.IsCboRequestEnable = false;
                                        this.isIntended = false;
                                        this.IsConfirmBtnEnabled = true;
                                        if (custRequestBLL.IsFollowUpRequest(this.CustRequestId))
                                        {
                                                this.ConfirmBtnVisible = System.Windows.Visibility.Visible;
                                        }
                                        else
                                                this.ConfirmBtnVisible = System.Windows.Visibility.Hidden;
                                        this.oldFollowUpContent = this.custFULogInfo.FollowUpContent;
                                        break;
                                case 4:
                                        this.custFULogInfo = custFULogBLL.GetCustomerFULogInfo(fULogId);
                                        this.IsCboRequestEnable = false;
                                        this.isIntended = false;
                                        this.ConfirmBtnVisible = System.Windows.Visibility.Hidden;
                                        break;
                                default: break;
                        }

                }
                private CustomerFollowUpLogInfo custFULogInfo = new CustomerFollowUpLogInfo();
                private bool isIntended = true;
                private string oldFollowUpContent = "";
                public string LoginUser { get; set; }

          

                public int FollowUpLogId
                {
                        get { return custFULogInfo.FLogId; }
                        set { custFULogInfo.FLogId = value; }
                }

                public int CustRequestId
                {
                        get { return custFULogInfo.CustRequestId; }
                        set
                        {
                                custFULogInfo.CustRequestId = value;
                                OnPropertyChanged();
                        }
                }

         
                public string FollowUpUser
                {
                        get { return custFULogInfo.FollowUpUser; }
                        set {
                                custFULogInfo.FollowUpUser = value;
                                OnPropertyChanged();
                        }
                }

              
                public DateTime FollowUpTime
                {
                        get { return custFULogInfo.FollowUpTime; }
                        set
                        {
                                custFULogInfo.FollowUpTime = value;
                                OnPropertyChanged();
                        }
                }

            
                public string FollowUpContent
                {
                        get { return custFULogInfo.FollowUpContent; }
                        set
                        {
                                custFULogInfo.FollowUpContent = value;
                                OnPropertyChanged();
                        }
                }

        
                public string FollowUpState
                {
                        get { return custFULogInfo.FollowUpState; }
                        set
                        {
                                custFULogInfo.FollowUpState = value;
                                OnPropertyChanged();
                        }
                }

                private bool isCboRequestEnable;
        
                public bool IsCboRequestEnable
                {
                        get { return isCboRequestEnable; }
                        set { isCboRequestEnable = value;
                                OnPropertyChanged();
                        }
                }

                public List<CustomerRequestInfo> CboCustRequests
                {
                        get
                        {
                                List<CustomerRequestInfo> requests = custRequestBLL.GetCustIntentedRequests(this.isIntended);
                                requests.Insert(0, new CustomerRequestInfo()
                                {
                                        CustRequestId = 0,
                                        RequestContent = "请选择意向需求！"
                                });
                                return requests;
                        }
                }
              
                public List<string> CboFUStates
                {
                        get
                        {
                                return new List<string>() { "跟进中", "成交", "放弃" };
                        }
                }

                public ICommand ConfirmCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        string actMsg = ActType == 2 ? "修改" : "新增";
                                        string msgTitle = $"客户日志{actMsg}";
                                        if (this.CustRequestId== 0)
                                        {
                                                ShowError("请选择客户需求！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(this.FollowUpContent))
                                        {
                                                ShowError("请输入日志跟进内容！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(FollowUpUser))
                                        {
                                                FollowUpUser = LoginUser;
                                        }
                                        bool bl = ActType == 1 ? custFULogBLL.AddCustomerFLogInfo(this.custFULogInfo) : custFULogBLL.UpdateCustomerFLogInfo(this.custFULogInfo);
                                        string sucType = bl ? "成功" : "失败";
                                        string msgInfo = $"客户日志{actMsg}{sucType}!";
                                        if (bl)
                                        {
                                                ShowMessage(msgInfo, msgTitle);
                                                InvokeReload();
                                        }
                                        else
                                        {
                                                ShowError(msgInfo, msgTitle);
                                                return;
                                        }
                                });
                        }
                }

        }
}
