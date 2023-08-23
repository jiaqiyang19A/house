using HRSM.BLL;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HRSM.HRSMApp.ViewModels.CRM
{
        public class CustomerRequestListViewModel:ListViewModelBase
        {
                CustRequestBLL requestBLL = new CustRequestBLL();
                public CustomerRequestListViewModel()
                {
                        InitPageInfo(0);
                }

                public CustomerRequestListViewModel(int custId)
                {
                        InitPageInfo(custId);
                }


                private void InitPageInfo(int custId)
                {
                        this.CustomerId = custId;
                        this.CustRequestList = FindCustRequestList();
                        this.FindCustRequestListCmd = new RelayCommand(o =>
                        {
                                ReloadCustRequestList();
                        });
                        InitToolBars();
                      
                        this.CheckAllCmd = GetCheckAllCmd(this.CustRequestList);
                        this.CheckItemCmd = GetCheckItemCmd(this.CustRequestList);
                }

                private int customerId;
     
                public int CustomerId
                {
                        get { return customerId; }
                        set { customerId = value; }
                }

                private string custName;
  
                public string  CustName 
                {
                        get { return custName; }
                        set {
                                custName = value;
                                OnPropertyChanged();
                        }
                }

                private string followUpUser;

                public string FollowUpUser
                {
                        get { return followUpUser; }
                        set
                        {
                                followUpUser = value;
                                OnPropertyChanged();
                        }
                }

                private string customerType="全部";
   
                public string CustomerType
                {
                        get { return customerType; }
                        set
                        {
                                customerType = value;
                                OnPropertyChanged();
                        }
                }

               public List<string>   CboCustomerTypes
                {
                        get
                        {
                                return new List<string>() { "全部", "个人", "单位" };
                        }
                }

                private string requestContent;

                public string RequestContent
                {
                        get { return requestContent; }
                        set
                        {
                                requestContent = value;
                                OnPropertyChanged();
                        }
                }

                private ObservableCollection<CustomerRequestCheckInfo> custRequestList;

                public ObservableCollection<CustomerRequestCheckInfo> CustRequestList
                {
                        get { return custRequestList; }
                        set { custRequestList = value;
                                OnPropertyChanged();
                        }
                }


                private RelayCommand findCustRequestListCmd;
                public RelayCommand FindCustRequestListCmd
                {
                        get { return findCustRequestListCmd; }
                        set
                        {
                                findCustRequestListCmd = value;
                                OnPropertyChanged();
                        }
                }

                private void InitToolBars()
                {
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                ShowFind = Visibility.Visible,
                                ShowCustFULog = Visibility.Visible,
                                ShowAllRequestList=Visibility.Visible,
                                AddCommand = new RelayCommand(o => ShowCustomerRequestPage(1, 0), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowCustomerRequestPage(2), b => IsEditable(this.CustRequestList)),
                                DeleteCommand = new RelayCommand(o => DeleteCustomerRequests(1), b => IsEditable(this.CustRequestList)),
                                RecoverCommand = new RelayCommand(o => DeleteCustomerRequests(0), b => IsDeletedable(this.CustRequestList)),
                                RemoveCommand = new RelayCommand(o => DeleteCustomerRequests(2), b => IsDeletedable(this.CustRequestList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowCustomerRequestPage(4), b => this.CustRequestList.Count > 0),
                                FindCommand = FindCustRequestListCmd,
                                CustFULogCommand = new RelayCommand(o => ShowCustFollowUpLogPage(o), b => this.CustRequestList.Count>0),
                                AllRequestListCommand=new RelayCommand(o =>
                                {
                                        this.CustomerId = 0;
                                        ReloadCustRequestList();
                                })
                        };
                }


                private ObservableCollection<CustomerRequestCheckInfo> FindCustRequestList()
                {
                        ObservableCollection<CustomerRequestCheckInfo> reList = new ObservableCollection<CustomerRequestCheckInfo>();
                        string custType = this.CustomerType == "全部" ? "" : this.CustomerType;
                        List<ViewCustomerRequestInfo> list = requestBLL.GetCustRequests(this.CustomerId, this.CustName,custType, this.FollowUpUser, this.RequestContent, this.IsShowDel);
                        list.ForEach(r => reList.Add(new CustomerRequestCheckInfo()
                        {
                                IsCheck = false,
                                CustRequestInfo = r
                        }));
                        return reList;
                }

                private void  ShowCustomerRequestPage(int actType, int custRequestId)
                {
                        CustomerRequestInfoViewModel requestVM = new CustomerRequestInfoViewModel(actType, custRequestId);
                        requestVM.ReloadListEvent += ReloadCustRequestList;
                        ShowDialog("custRequestInfo", requestVM);
                }


                private void ShowCustomerRequestPage(int actType)
                {
                        if(this.CurrentItem!=null)
                        {
                                CustomerRequestCheckInfo info = this.CurrentItem as CustomerRequestCheckInfo;
                                ShowCustomerRequestPage(actType, info.CustRequestInfo.CustRequestId);
                        }
                }

    
                private void ShowCustFollowUpLogPage(object uc)
                {
                        int requestId = 0;
                        if (this.CurrentItem != null)
                                requestId = (this.CurrentItem as CustomerRequestCheckInfo).CustRequestInfo.CustRequestId;
                        CustomerFollowUpLogListViewModel logListVM = new CustomerFollowUpLogListViewModel(requestId);
                        AddUCTabItem((UserControl)uc, "客户跟踪管理", "custFollowUpLogList", logListVM);
                }

                private void ReloadCustRequestList()
                {
                        this.CustRequestList = FindCustRequestList();
                        this.CheckAllCmd = GetCheckAllCmd(this.CustRequestList);
                        //刷新工具栏
                        this.ToolBarInfo.OnCanExecuteChanged();
                }

                private void DeleteCustomerRequests(int isDeleted)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"客户需求{delType}";
                        List<int> delIds = new List<int>();
                        delIds = this.CustRequestList.Where(r => r.IsCheck == true).Select(r => r.CustRequestInfo.CustRequestId).ToList();
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        List<int> hasRequestIds= requestBLL.GetSucAndFollowUpRequestIds(delIds);
                                        if (hasRequestIds.Count > 0)
                                        {
                                                ShowError($"选择的客户需求信息中包含已成交或跟进中的需求，它们的编号是：{string.Join(",", hasRequestIds)}", msgTile);
                                                return;
                                        }
                                }
                                if (ShowQuestion($"您确定要{delType}选择的客户需求信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = requestBLL.LogicDelCustomerRequestList(delIds);
                                                        break;
                                                case 0:
                                                        isDel = requestBLL.RecoverCustomerRequestList(delIds);
                                                        break;
                                                case 2:
                                                        isDel = requestBLL.RemoveCustomerRequestList(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的客户需求信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                               
                                                ReloadCustRequestList();
                                        }
                                        else
                                        {
                                                ShowError(msg, msgTile);
                                                return;
                                        }
                                }
                        }
                        else
                        {
                                ShowError($"没有要{delType}的信息！", msgTile);
                                return;
                        }
                }
        }
}
