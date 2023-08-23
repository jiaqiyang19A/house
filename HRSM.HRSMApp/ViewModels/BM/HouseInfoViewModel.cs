using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.BM
{
        public  class HouseInfoViewModel:InfoViewModelBase
        {
                HouseBLL houseBLL = new HouseBLL();
                PriceUnitBLL punitBLL = new PriceUnitBLL();
                HouseLayoutBLL hlBLL = new HouseLayoutBLL();
                OwnerBLL ownerBLL = new OwnerBLL();
                HouseStateBLL hsBLL = new HouseStateBLL();
                public HouseInfoViewModel()
                { }

                private string oldHouseName = "";
                private string oldPic = "";
                private bool blUpdatePic = false;//是否更新图片

                public HouseInfoViewModel(int actType, int houseId)
                {
                        this.ActType = actType;
                        this.HouseId = houseId;
                        this.IsRent = true;
                        this.HouseDirection = "请选择";
                        this.HouseLayout = "请选择户型";
                        this.PriceUnit = "元/平方";
                        this.HouseState = "未出租";
                        switch (this.ActType)
                        {
                                case 1:
                                        this.ConfirmBtnContent = "新增";
                                        this.IsConfirmBtnEnabled = true;
                                        break;
                                case 2:
                                        this.ConfirmBtnContent = "修改";
                                        this.HouseInfo = GetHouseInfo();
                                        this.IsRent = HouseInfo.RentSale == "出租" ? true : false;
                                        this.isSale = HouseInfo.RentSale == "出售" ? true : false;
                                        this.oldHouseName = this.HouseInfo.HouseName;
                                        this.IsConfirmBtnEnabled = true;
                                        this.oldPic = this.HousePic;
                                        break;
                                case 4:
                                        this.HouseInfo = GetHouseInfo();
                                        this.IsRent = HouseInfo.RentSale == "出租" ? true : false;
                                        this.isSale = HouseInfo.RentSale == "出售" ? true : false;
                                        this.ConfirmBtnVisible = System.Windows.Visibility.Hidden;
                                        break;
                                default: break;
                        }

                }

                private int houseId;

                public int HouseId
                {
                        get { return houseId; }
                        set { houseId = value; }
                }



                private HouseInfo houseInfo = new HouseInfo();
                public HouseInfo HouseInfo
                {
                        get { return houseInfo; }
                        set
                        {
                                houseInfo = value;
                                OnPropertyChanged();
                        }
                }

                public string PriceUnit
                {
                        get { return HouseInfo.PriceUnit; }
                        set
                        {
                                HouseInfo.PriceUnit = value;
                                OnPropertyChanged();
                        }
                }


                public List<PriceUnitInfo> CboUnits
                {
                        get {
                                List<PriceUnitInfo> list = punitBLL.GetPriceUnits();
                                return list;
                        }
                }


                private bool isRent = true;
                public bool IsRent
                {
                        get
                        {
                                return isRent;
                        }
                        set
                        {
                                isRent = value;
                                OnPropertyChanged();
                        }
                }

     
                private bool isSale = false;
                public bool IsSale
                {
                        get
                        {
                                return isSale;
                        }
                        set
                        {
                                isSale = value;
                                OnPropertyChanged();
                        }
                }

      
                public string HouseDirection
                {
                        get { return HouseInfo.HouseDirection; }
                        set
                        {
                                HouseInfo.HouseDirection = value;
                                OnPropertyChanged();
                        }
                }

                public List<string> CboDirections
                {
                        get
                        {
                                return CommonUtility.GetHouseDirectionList(true);
                        }
                }


                public string HouseLayout
                {
                        get { return HouseInfo.HouseLayout; }
                        set
                        {
                                HouseInfo.HouseLayout = value;
                                OnPropertyChanged();
                        }
                }


                public List<HouseLayoutInfo> CboLayouts
                {
                        get {

                                List<HouseLayoutInfo> layoutList = hlBLL.GetHouseLayouts();
                                layoutList.Insert(0, new HouseLayoutInfo()
                                {
                                        HLId = 0,
                                        HLName = "请选择户型"
                                });
                                return layoutList;
                        }
                }


                public int OwnerId
                {
                        get { return HouseInfo.OwnerId; }
                        set
                        {
                                HouseInfo.OwnerId = value;
                                OnPropertyChanged();
                        }
                }


                public List<HouseOwnerInfo> CboOwners
                {
                        get {
                                List<HouseOwnerInfo> list = ownerBLL.GetAllOwners();
                                list.Insert(0, new HouseOwnerInfo()
                                {
                                        OwnerId = 0,
                                        OwnerName = "请选择业主"
                                });
                                return list;
                        }
                }


                public string HouseState
                {
                        get { return HouseInfo.HouseState; }
                        set
                        {
                                HouseInfo.HouseState = value;
                                OnPropertyChanged();
                        }
                }


                private List<HouseStateInfo> cboStates = new List<HouseStateInfo>();
                public List<HouseStateInfo> CboStates
                {
                        get { return GetHouseStates(); }
                        set
                        {
                                cboStates = value;
                                OnPropertyChanged();
                        }
                }

                public string HousePic
                {
                        get
                        { 
                                return HouseInfo.HousePic;
                        }
                        set
                        {
                                HouseInfo.HousePic = value;
                                OnPropertyChanged();
                        }
                }



                private bool unPublish = false;
                public bool UnPublish
                {
                        get
                        {
                                return unPublish;
                        }
                        set
                        {
                                unPublish = value;
                                OnPropertyChanged();
                        }
                }

   
                public ICommand CheckRentSaleCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        this.CboStates = GetHouseStates();//重新加房屋状态下拉框
                                        this.HouseState = this.IsRent ? "未出租" : "未出售";

                                });
                        }
                }

     
                public ICommand ChoosePicCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "png Files (*.png)|*.png| jpg Files  (*.jpg)|*.jpg|bmp Files  (*.bmp)|*.bmp" };
                                        var result = ofd.ShowDialog();
                                        if (result == false) return;
                                        string fileName = ofd.FileName;
                                        if (!string.IsNullOrEmpty(fileName))
                                        {
                                                this.HousePic = fileName;
                                        }
                                });
                        }
                }

                public ICommand ConfirmCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        var info = this.HouseInfo;
                                        this.HouseInfo.RentSale = this.IsRent ? "出租" : "出售";
                                        SetUpdatePic();//图片保存   ----图片保存到项目文件夹中，把地址存储到房屋信息实体中
                                        string houseName = this.HouseInfo.HouseName;
                                        string actMsg = this.ActType == 2 ? "修改" : "添加";
                                        string msgTitle = $"房屋{actMsg}";
                                        if (string.IsNullOrEmpty(houseName))
                                        {
                                                ShowError("请输入房屋名！", msgTitle);
                                                return;
                                        }
                                        if (this.OwnerId == 0)
                                        {
                                                ShowError("请选择业主！", msgTitle);
                                                return;
                                        }
                                        if (this.HouseId == 0 || (oldHouseName != "" && oldHouseName != houseName))
                                        {
                                                if (houseBLL.ExistsHouse(houseName))
                                                {
                                                        ShowError("该房屋已存在！", msgTitle);
                                                        return;
                                                }
                                        }

                                        if ( this.HouseInfo.IsPublish && string.IsNullOrEmpty(this.HouseInfo.PublishUser))
                                        {
                                                this.HouseInfo.PublishUser = GetLoginUser(o);
                                                this.HouseInfo.PublishTime = DateTime.Now;
                                        }
                                        bool bl = ActType == 1 ? houseBLL.AddHouseInfo(this.HouseInfo) : houseBLL.UpdateHouseInfo(this.HouseInfo);
                                        string sucType = bl ? "成功" : "失败";
                                        string msgInfo = $"房屋：{houseName} {actMsg}{sucType}!";
                                        if (bl)
                                        {
                                                ShowMessage(msgInfo, msgTitle);
                                                if(blUpdatePic)
                                                {
                                                        this.HousePic = "pack://siteoforigin:,,,/" + this.HousePic;
                                                }
                                              
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

                private void SetUpdatePic()
                {
                        if (oldPic != this.HousePic && this.HousePic != "imgs/house.jpg")
                                blUpdatePic = true;
                        if (blUpdatePic)
                        {
                                string fileName = Path.GetFileNameWithoutExtension(this.HousePic);
                                fileName += DateTime.Now.ToString("yyyyMMddhhMMss") + Path.GetExtension(this.HousePic);
                                if (!Directory.Exists(@"imgs/house"))
                                {
                                        Directory.CreateDirectory( @"imgs/house");
                                }
                                string url =  @"imgs/house/" + fileName;
                                if (!File.Exists(url))
                                {
                                        File.Copy(this.HousePic, url);
                                }
                                this.HousePic = null;
                                this.HousePic = "imgs/house/" + fileName;//要存到房屋信息实体中的路径
                        }
                        else if (oldPic == this.HousePic&&this.HousePic!= " imgs/house.jpg")
                        {
                                string fileName = Path.GetFileName(this.HousePic);
                                this.HousePic = "imgs/house/" + fileName;
                        }
                        else
                                this.HousePic = null;
                }
     

                private List<HouseStateInfo> GetHouseStates()
                {
                        int rsId = this.IsRent ? 1 : 2;
                        List<HouseStateInfo> list = hsBLL.GetHouseStates(rsId);
                        return list;
                }

                /// <summary>
                /// 获取房屋信息
                /// </summary>
                /// <returns></returns>
                private HouseInfo GetHouseInfo()
                {
                        HouseInfo houseinfo = houseBLL.GetHouse(this.HouseId);
                        if (houseinfo != null)
                        {
                                if (!string.IsNullOrEmpty(houseinfo.HousePic))
                                {
                                        houseinfo.HousePic = "pack://siteoforigin:,,,/" + houseinfo.HousePic;
                                }
                                else
                                        houseinfo.HousePic = "../imgs/house.jpg";

                        }
                        return houseinfo;
                }

        }
}
