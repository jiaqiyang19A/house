using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.CRM
{
        public  class CustomerRequestInfoViewModel:InfoViewModelBase
        {
                CustomerBLL custBLL = new CustomerBLL();
                CustRequestBLL custRequestBLL = new CustRequestBLL();
                public CustomerRequestInfoViewModel() { }
                public CustomerRequestInfoViewModel(int actType,int custRequestId)
                {
                        this.ActType = actType;
                        this.CustRequestId = custRequestId;
                        this.CloseWindowCmd = GetCloseWindowCmd("custRequestInfo");
                        switch (ActType)
                        {
                                case 1:
                                        this.ConfirmBtnContent = "添加";
                                        this.CustomerId = 0;
                                        this.IsConfirmBtnEnabled = true;
                                        break;
                                case 2:
                                        this.ConfirmBtnContent = "修改";
                                        this.custRequest = custRequestBLL.GetCustomerRequestInfo(custRequestId);
                                        this.IsConfirmBtnEnabled = true;
                                        this.oldRequestContent = this.custRequest.RequestContent;
                                        this.oldCustId = this.custRequest.CustomerId;
                                        break;
                                case 4:
                                        this.custRequest = custRequestBLL.GetCustomerRequestInfo(custRequestId);
                                        this.ConfirmBtnVisible = Visibility.Hidden;
                                        break;
                                default: break;
                        }
                }
                private CustomerRequestInfo custRequest = new CustomerRequestInfo();
                string oldRequestContent = "";
                int oldCustId = 0;


                public int CustRequestId
                {
                        get { return custRequest.CustRequestId; }
                        set { custRequest.CustRequestId = value; }
                }


                public int CustomerId
                {
                        get { return custRequest.CustomerId; }
                        set {
                                custRequest.CustomerId = value;
                                OnPropertyChanged();
                        }
                }

                public List<CustomerInfo> AllCustomers
                {
                        get
                        {
                                List<CustomerInfo> customers = custBLL.GetNormalCustomers(ActType);
                                customers.Insert(0, new CustomerInfo()
                                {
                                        CustomerId = 0,
                                        CustomerName = "请选择客户"
                                });
                                return customers;
                        }
                }

        
                public string FollowUpUser
                {
                        get { return custRequest.FollowUpUser; }
                        set
                        {
                                custRequest.FollowUpUser = value;
                                OnPropertyChanged();
                        }
                }

                public string RequestContent
                {
                        get { return custRequest.RequestContent; }
                        set
                        {
                                custRequest.RequestContent = value;
                                OnPropertyChanged();
                        }
                }

                public ICommand ConfirmCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        string actMsg = ActType == 2 ? "修改" : "新增";
                                        string msgTitle = $"客户需求{actMsg}页面";
                                        if (this.CustomerId == 0)
                                        {
                                                ShowError("请选择客户！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(this.RequestContent))
                                        {
                                                ShowError("请输入客户需求！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(this.FollowUpUser))
                                        {
                                                ShowError("请输入跟进人！", msgTitle);
                                                return;
                                        }
                                        if(CustRequestId==0||(!string.IsNullOrEmpty(oldRequestContent)&&oldRequestContent!=RequestContent)||(oldCustId>0&&oldCustId!=CustomerId))
                                        {
                                                if(custRequestBLL.ExistsRequest(CustomerId,RequestContent))
                                                {
                                                        ShowError("该客户需求已存在！", msgTitle);
                                                        return;
                                                }
                                        }
                                        bool bl = false;
                                        if (this.ActType == 2)
                                        {
                                                if(oldCustId==0)
                                                        bl = custRequestBLL.UpdateCustomerRequestInfo(this.custRequest);
                                                else
                                                        bl = custRequestBLL.UpdateCustomerRequestInfo(this.custRequest,oldCustId);
                                        }    
                                        else
                                                bl = custRequestBLL.AddCustomerRequestInfo(this.custRequest);

                                        string sucType = bl ? "成功" : "失败";
                                        string msgInfo = $"客户需求 {actMsg}{sucType}!";
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
