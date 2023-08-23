using HRSM.BLL;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HRSM.HRSMApp.ViewModels.CRM
{
         public  class CustomerFollowUpLogListViewModel:ListViewModelBase
        {
                CustomerFollowUpLogBLL custFULogBLL = new CustomerFollowUpLogBLL();
                public CustomerFollowUpLogListViewModel() {
                        InitPageInfo(0);
                }
                public CustomerFollowUpLogListViewModel(int requestId)
                {

                        InitPageInfo(requestId);
                }

                private void InitPageInfo(int custRequestId)
                {
                        this.CustRequestId = custRequestId;
                        this.FollowUpLogList = FindCustFollowUpLogList();
                        this.FindCustFollowUpLogListCmd = new RelayCommand(o =>
                          {
                                  ReloadFULogList();
                          });
                        InitToolBarInfo();
                        this.CheckAllCmd = GetCheckAllCmd(this.FollowUpLogList);
                        this.CheckItemCmd = GetCheckItemCmd(this.FollowUpLogList);
                }

               

                private int custRequestId;

                public int CustRequestId
                {
                        get { return custRequestId; }
                        set { custRequestId = value; }
                }

                private string custName;

                public string CustName
                {
                        get { return custName; }
                        set { custName = value;
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
                private string followUpContent;
  
                public string FollowUpContent
                {
                        get { return followUpContent; }
                        set
                        {
                                followUpContent = value;
                                OnPropertyChanged();
                        }
                }

                private ObservableCollection<CustomerFollowUpLogCheckInfo> followUpLogList = new ObservableCollection<CustomerFollowUpLogCheckInfo>();
    
                public ObservableCollection<CustomerFollowUpLogCheckInfo> FollowUpLogList
                {
                        get { return followUpLogList; }
                        set {
                                followUpLogList = value;
                                OnPropertyChanged();
                        }
                }

                private RelayCommand findCustFollowUpLogListCmd;
                public RelayCommand FindCustFollowUpLogListCmd
                {
                        get { return findCustFollowUpLogListCmd; }
                        set
                        {
                                findCustFollowUpLogListCmd = value;
                                OnPropertyChanged();
                        }
                }

   
                private ObservableCollection<CustomerFollowUpLogCheckInfo> FindCustFollowUpLogList()
                {
                        ObservableCollection<CustomerFollowUpLogCheckInfo> reList = new ObservableCollection<CustomerFollowUpLogCheckInfo>();
                        List<ViewCustomerFollowUpLogInfo> fLogList = custFULogBLL.GetCustFLogs(this.CustRequestId, CustName, FollowUpUser, RequestContent, FollowUpContent, IsShowDel);
                        fLogList.ForEach(f => reList.Add(new CustomerFollowUpLogCheckInfo()
                        {
                                IsCheck = false,
                                FollowUpLogInfo = f
                        }));
                        return reList;
                }


                private void InitToolBarInfo()
                {
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                ShowFind = Visibility.Visible,
                                ShowAllCustFULog =this.CustRequestId>0? Visibility.Visible:Visibility.Collapsed,
                                AddCommand = new RelayCommand(o => ShowFULogInfoPage(1, 0,o), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowFULogInfoPage(2,o), b => IsEditable(this.FollowUpLogList)),
                                DeleteCommand = new RelayCommand(o => DeleteCustomerFULogs(1), b => IsEditable(this.FollowUpLogList)),
                                RecoverCommand = new RelayCommand(o => DeleteCustomerFULogs(0), b => IsDeletedable(this.FollowUpLogList)),
                                RemoveCommand = new RelayCommand(o => DeleteCustomerFULogs(2), b => IsDeletedable(this.FollowUpLogList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowFULogInfoPage(4,o), b => this.FollowUpLogList.Count > 0),
                                FindCommand = FindCustFollowUpLogListCmd,
                                CustAllFULogCommand = new RelayCommand(o =>
                                {
                                        this.CustRequestId = 0;
                                        ReloadFULogList();
                                })
                        };
                }


              private void    ShowFULogInfoPage(int actType, int logId, object uc)
                {

                        CustomerFollowUpLogInfoViewModel logVM = new CustomerFollowUpLogInfoViewModel(actType, logId,CustRequestId);
                        logVM.LoginUser = GetLoginUser(uc);
                        logVM.ReloadListEvent += ReloadFULogList;
                        ShowDialog("custFULogInfo", logVM);
                }

                private void ShowFULogInfoPage(int actType,object uc)
                {
                        if(this.CurrentItem!=null)
                        {
                                CustomerFollowUpLogCheckInfo info = this.CurrentItem as CustomerFollowUpLogCheckInfo;
                                ShowFULogInfoPage(actType, info.FollowUpLogInfo.FLogId,uc);
                        }
                     
                }

                private void ReloadFULogList()
                {
                        this.FollowUpLogList = FindCustFollowUpLogList();
                        this.CheckAllCmd = GetCheckAllCmd(this.FollowUpLogList);
                        this.ToolBarInfo.OnCanExecuteChanged();
                }

               /// <summary>
               /// 客户跟进日志的删除  恢复  移除处理
               /// </summary>
               /// <param name="isDeleted"></param>
                private void DeleteCustomerFULogs(int isDeleted)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"客户跟进日志{delType}";
                        List<int> delIds = new List<int>();
                        delIds = this.FollowUpLogList.Where(r => r.IsCheck == true).Select(r => r.FollowUpLogInfo.FLogId).ToList();
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        List<ViewCustomerFollowUpLogInfo> selList = this.FollowUpLogList.Where(r => r.IsCheck == true).Select(r => r.FollowUpLogInfo).ToList();
                                        List<int> hasLogIds = custFULogBLL.GetUseOrSuccessLog(selList);//获取对应需求是成交的客户日志编号
                                        if (hasLogIds.Count > 0)
                                        {
                                                ShowError($"选择的客户日志信息中包含已成交需求的客户日志，它们的编号是：{string.Join(",", hasLogIds)}", msgTile);
                                                return;
                                        }
                                }
                                if (ShowQuestion($"您确定要{delType}选择的客户跟进日志信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = custFULogBLL.LogicDelCustomerFULogList(delIds);
                                                        break;
                                                case 0:
                                                        isDel = custFULogBLL.RecoverCustomerFULogList(delIds);
                                                        break;
                                                case 2:
                                                        isDel = custFULogBLL.RemoveCustomerFULogList(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的客户跟进日志信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                                //刷新列表数据
                                                ReloadFULogList();
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
