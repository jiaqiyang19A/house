using HRSM.BLL;
using HRSM.HRSMApp.Models;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.BM
{
        public class HouseTradeViewModel : InfoViewModelBase
        {
                CustomerBLL custBLL = new CustomerBLL();
                HouseTradeBLL htBLL = new HouseTradeBLL();
                HouseBLL houseBLL = new HouseBLL();
                public HouseTradeViewModel()
                { }


                public HouseTradeViewModel(int houseId)
                {
                        this.HouseId = houseId;
                        this.HouseInfo = houseBLL.GetHouseInfo(houseId);
                        this.RSName = HouseInfo.RentSale;//房屋的租售类别
                        this.CustId = 0;
                        this.TradeWay = "现金";
                        decimal hPrice = this.HouseInfo.HousePrice.HasValue ? this.HouseInfo.HousePrice.Value : 0;
                        if (this.HouseInfo.RentSale =="出售" && houseInfo.PriceUnit == "元/平方")
                        {
                                
                                //计算总价   默认房屋价格
                                this.TradeAmount = decimal.Parse(decimal.Multiply(hPrice, this.HouseInfo.HouseArea).ToString("0.00"));
                        }
                        else 
                                this.TradeAmount = hPrice;
                        this.ConfirmBtnContent = "提交";
                        this.IsConfirmBtnEnabled = true;
                }


                public int HouseId
                {
                        get; set;
                }

                private ViewHouseInfo houseInfo = new ViewHouseInfo();
  
                public ViewHouseInfo HouseInfo
                {
                        get { return houseInfo; }
                        set
                        {
                                houseInfo = value;
                                OnPropertyChanged();
                        }
                }

       
                private string rsName;
                public string RSName
                {
                        get { return rsName; }
                        set
                        {
                                rsName = value;
                                OnPropertyChanged();
                        }
                }

                public List<RentSaleModel> CboRentSales
                {
                        get
                        {
                                return new List<RentSaleModel>()
                               {
                                        new RentSaleModel(){RSId=1,RSName="出租" },
                                        new RentSaleModel(){RSId=2,RSName="出售" }
                                };
                        }
                }

                private int custId = 0;
                public int CustId
                {
                        get { return custId; }
                        set
                        {
                                custId = value;
                                OnPropertyChanged();
                        }
                }

                public List<CustomerInfo> CboCustomers
                {
                        get {
                               List < CustomerInfo > customers = custBLL.GetIntentedCustomers();
                                customers.Insert(0, new CustomerInfo()
                                {
                                        CustomerId = 0,
                                        CustomerName = "请选择客户！"
                                });
                                return customers;
                        }
                }

      
                private string tradeWay = "现金";
                public string TradeWay
                {
                        get { return tradeWay; }
                        set
                        {
                                tradeWay = value;
                                OnPropertyChanged();
                        }
                }

     
                public List<string> CboTradeWays
                {
                        get
                        {
                                return new List<string>()
                                {
                                        "现金","分期","按揭"
                                };
                        }
                }

         
                private decimal tradeAmount;
                public decimal TradeAmount
                {
                        get { return tradeAmount; }
                        set
                        {
                                tradeAmount = value;
                                OnPropertyChanged();
                        }
                }

        
                private string dealUser = "";
                public string DealUser
                {
                        get { return dealUser; }
                        set
                        {
                                dealUser = value;
                                OnPropertyChanged();
                        }
                }


                private DateTime tradeTime = DateTime.Now;
                public DateTime TradeTime
                {
                        get { return tradeTime; }
                        set
                        {
                                tradeTime = value;
                                OnPropertyChanged();
                        }
                }

      
                public ICommand ConfirmCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        int ownerId = houseInfo.OwnerId.HasValue?houseInfo.OwnerId.Value:0;
                                        string msgTitle = "房屋交易";
                                        if (this.CustId == 0)
                                        {
                                                ShowError("请选择客户！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(dealUser))
                                        {
                                                ShowError("请填写办理人！", msgTitle);
                                                return;
                                        }
                                        if (tradeAmount <= 0)
                                        {
                                                ShowError("请填写交易总价！", msgTitle);
                                                return;
                                        }
                                        //提交交易信息
                                        HouseTradeInfo tradeInfo = new HouseTradeInfo()
                                        {
                                                HouseId = this.HouseId,
                                                OwnerId = ownerId,
                                                CustomerId = this.CustId,
                                                RentSale = this.RSName,
                                                TradeAmount = this.TradeAmount,
                                                TradeTime = this.TradeTime,
                                                TradeWay = this.TradeWay,
                                                DealUser = this.DealUser,
                                                PriceUnit = this.HouseInfo.PriceUnit
                                        };
                                        bool blAdd = htBLL.AddHouseTradeInfo(tradeInfo);//执行添加交易记录
                                        string suc = blAdd ? "成功" : "失败";
                                        string msg = $"房屋:{houseInfo.HouseName} 交易提交{suc}";
                                        if (blAdd)
                                        {
                                                ShowMessage(msg, msgTitle);
                                                InvokeReload();
                                        }
                                        else
                                        {
                                                ShowError(msg, msgTitle);
                                                return;
                                        }
                                });
                        }
                }

        }
}
