using HRSM.BLL;
using HRSM.Models.DModels;
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
        public  class CustomerListViewModel:ListViewModelBase
        {
                CustomerBLL custBLL = new CustomerBLL();
                public CustomerListViewModel()
                {
                        this.CustomerList = FindCustomerList();
                        InitToolbarInfo();
                        this.CheckAllCmd = GetCheckAllCmd(this.CustomerList);
                        this.CheckItemCmd = GetCheckItemCmd(this.CustomerList);
                }

            

                private string keywords;

                public string KeyWords
                {
                        get { return keywords; }
                        set
                        {
                                keywords = value;
                                OnPropertyChanged();
                        }
                }

                private string customerType = "全部";
  
                public string CustomerType
                {
                        get { return customerType; }
                        set
                        {
                                customerType = value;
                                OnPropertyChanged();
                        }
                }
                private string customerState = "全部";
     
                public string CustomerState
                {
                        get { return customerState; }
                        set
                        {
                                customerState = value;
                                OnPropertyChanged();
                        }
                }

     
                public List<string> CboCustTypes
                {
                        get
                        {
                                return new List<string>() { "全部", "个人", "单位" };
                        }
                }

    
                public List<string> CboCustStates
                {
                        get
                        {
                                return new List<string>() { "全部", "普通客户", "意向客户" };
                        }
                }

                private ObservableCollection<CustomerCheckInfo> customerList=new ObservableCollection<CustomerCheckInfo>();

                public ObservableCollection<CustomerCheckInfo> CustomerList
                {
                        get { return customerList; }
                        set { customerList = value;
                                OnPropertyChanged();
                        }
                }

                #region 命令

                public RelayCommand FindCustomersCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        ReloadCustomerList();
                                });
                        }
                }


            
                #endregion

                /// <summary>
                /// 配置工具栏
                /// </summary>
                private void InitToolbarInfo()
                {
                   
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                ShowFind = Visibility.Visible,
                                ShowCustRequestList=Visibility.Visible,
                                AddCommand = new RelayCommand(o => ShowCustomerInfoPage(1, 0, o), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowCustomerInfoPage(2, o), b => IsEditable(this.CustomerList)),
                                DeleteCommand = new RelayCommand(o => DeleteCustomers(1), b => IsEditable(this.CustomerList)),
                                RecoverCommand = new RelayCommand(o => DeleteCustomers(0), b => IsDeletedable(this.CustomerList)),
                                RemoveCommand = new RelayCommand(o => DeleteCustomers(2), b => IsDeletedable(this.CustomerList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowCustomerInfoPage(4, o), b => this.CustomerList.Count > 0),
                                FindCommand = FindCustomersCmd,
                                CustRequestListCommand=new RelayCommand(o=>ShowCustomerRequestListPage(o),b => IsEditable(this.CustomerList))
                        };
                }
   
                private ObservableCollection<CustomerCheckInfo> FindCustomerList()
                {
                        
                        ObservableCollection<CustomerCheckInfo> reList = new ObservableCollection<CustomerCheckInfo>();
                        string custType = this.CustomerType == "全部" ? "" : this.CustomerType;
                        string custState = this.CustomerState == "全部" ? "" : this.CustomerState;
                        List<CustomerInfo> list = custBLL.GetCustList(this.KeyWords, custType, custState, this.IsShowDel);
                        list.ForEach(c => reList.Add(new CustomerCheckInfo()
                        {
                                IsCheck = false,
                                CustInfo = c,
                        }));
                        return reList;
                }

   
                private void ShowCustomerInfoPage(int actType, int custId, object uc)
                {
                        CustomerInfoViewModel custVM = new CustomerInfoViewModel(actType, custId);
                        custVM.ReloadListEvent += ReloadCustomerList;
                        string typeName = GetInfoTypeName(actType);
                        AddUCTabItem((UserControl)uc, $"客户信息——{typeName}页面", "customerInfoView", custVM);
                }

                public void ShowCustomerInfoPage(int actType, object uc)
                {
                        if (this.CurrentItem != null)
                        {
                                CustomerCheckInfo info = this.CurrentItem as CustomerCheckInfo;
                                ShowCustomerInfoPage(actType, info.CustInfo.CustomerId, uc);
                        }
                }

                private void ReloadCustomerList()
                {
                        this.CustomerList = FindCustomerList();
                        this.CheckAllCmd = GetCheckAllCmd(this.CustomerList);
                        this.ToolBarInfo.OnCanExecuteChanged();
                }

           
                public void ShowCustomerRequestListPage(object uc)
                {
                        CustomerRequestListViewModel custRequestVM = new CustomerRequestListViewModel();
                        if (this.CurrentItem != null)
                        {
                                CustomerCheckInfo info = this.CurrentItem as CustomerCheckInfo;
                                custRequestVM = new CustomerRequestListViewModel(info.CustInfo.CustomerId);
                            
                        }
                        AddUCTabItem((UserControl)uc, $"客户意向需求列表页面", "custReqList", custRequestVM);
                }

            
                public void DeleteCustomers(int isDeleted)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"客户{delType}";
                        List<int> delIds = new List<int>();
                        delIds = this.CustomerList.Where(r => r.IsCheck == true).Select(r => r.CustInfo.CustomerId).ToList();
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        List<int> intendedCustIds = custBLL.GetIntendedCustomerIds(delIds);//返回包括意向客户Id
                                        List<int> TradeCustIds = custBLL.GetTradeCustomerIds(delIds);//返回交易客户id
                                        if (intendedCustIds.Count > 0)
                                        {
                                                ShowError($"选择的客户信息中包含意向客户，它们的编号是：{string.Join(",", intendedCustIds)}", msgTile);
                                                return;
                                        }
                                        if (TradeCustIds.Count > 0)
                                        {
                                                ShowError($"选择的客户信息中包含已交易的客户，它们的编号是：{string.Join(",", TradeCustIds)}", msgTile);
                                                return;
                                        }
                                }
                                if (ShowQuestion($"您确定要{delType}选择的客户信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = custBLL.LogicDelCustomerList(delIds);
                                                        break;
                                                case 0:
                                                        isDel = custBLL.RecoverCustomerList(delIds);
                                                        break;
                                                case 2:
                                                        isDel = custBLL.RemoveCustomerList(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的客户信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                                //刷新列表数据
                                                ReloadCustomerList();
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
