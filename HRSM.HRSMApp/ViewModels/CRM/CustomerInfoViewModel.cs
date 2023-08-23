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
        public class CustomerInfoViewModel:InfoViewModelBase
        {
                CustomerBLL custBLL = new CustomerBLL();
                public CustomerInfoViewModel()
                {

                }
                string oldCustName = "";
                string oldCustPhone = "";

                public CustomerInfoViewModel(int actType,int custId)
                {
                        this.ActType = actType;
                        this.CustomerId = custId;
                        switch (ActType)
                        {
                                case 1:
                                        this.ConfirmBtnContent = "新增";
                                        this.IsPersonal = true;
                                        this.IsUnit = false;
                                        this.IsTextVisable = Visibility.Hidden;
                                        this.IsConfirmBtnEnabled = true;
                                        break;
                                case 2:
                                        this.ConfirmBtnContent = "修改";
                                        this.CustInfo = custBLL.GetCustomerInfo(custId);
                                        SetRbtnTypes();
                                        this.IsConfirmBtnEnabled = true;
                                        oldCustName = this.CustInfo.CustomerName;
                                        oldCustPhone = this.CustInfo.CustomerPhone;
                                        this.IsTextVisable = Visibility.Visible;
                                        break;
                                case 4:
                                        this.CustInfo = custBLL.GetCustomerInfo(custId);
                                        SetRbtnTypes();
                                        this.ConfirmBtnVisible = Visibility.Hidden;
                                        this.IsTextVisable = Visibility.Visible;
                                        break;

                        }

                }

                private void SetRbtnTypes()
                {
                        this.IsPersonal = this.CustInfo.CustomerType == "个人" ? true : false;
                        this.IsUnit = !this.IsPersonal;
                }

                private int customerId;
   
                public int CustomerId
                {
                        get { return customerId; }
                        set { customerId = value; }
                }

                private CustomerInfo custInfo=new CustomerInfo();

                public CustomerInfo CustInfo
                {
                        get { return custInfo; }
                        set
                        {
                                custInfo = value;
                                OnPropertyChanged();
                        }
                }

                private bool  isPersonal;

                public bool  IsPersonal
                {
                        get { return isPersonal; }
                        set { isPersonal = value;
                                custInfo.CustomerType = isPersonal ? "个人" : "单位";
                                OnPropertyChanged();
                        }
                }

                private bool isUnit;
   
                public bool IsUnit
                {
                        get { return isUnit; }
                        set
                        {
                                isUnit = value;
                                OnPropertyChanged();
                        }
                }

                private Visibility isTextVisable;
        
                public Visibility IsTextVisable
                {
                        get { return isTextVisable; }
                        set { isTextVisable = value;
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
                                        string msgTitle = $"客户{actMsg}";
                                        if (string.IsNullOrEmpty(this.CustInfo.CustomerName))
                                        {
                                                ShowError("请输入客户名！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(CustInfo.CustomerPhone))
                                        {
                                                ShowError("请输入客户电话！", msgTitle);
                                                return;
                                        }
                                        if (CustomerId == 0 || (oldCustName != "" && oldCustName != this.CustInfo.CustomerName)||(oldCustPhone!="" && oldCustPhone!=this.CustInfo.CustomerPhone))
                                        {
                                                if (custBLL.ExistsCustomer(this.CustInfo.CustomerName, this.CustInfo.CustomerPhone))
                                                {
                                                        ShowError("该客户已存在！", msgTitle);
                                                        return;
                                                }
                                        }

                                        bool bl = ActType == 1 ? custBLL.AddCustomerInfo(CustInfo) : custBLL.UpdateCustomerInfo(CustInfo);
                                        string sucType = bl ? "成功" : "失败";
                                        string msgInfo = $"客户：{CustInfo.CustomerName} {actMsg}{sucType}!";
                                        if (bl)
                                        {
                                                ShowMessage(msgInfo, msgTitle);
                                                InvokeReload();//刷新列表页
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
