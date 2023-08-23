using Common;
using HRSM.BLL;
using HRSM.HRSMApp.Models;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.BM
{
        public class HouseListViewModel : ListViewModelBase
        {
                HouseStateBLL hsBLL = new HouseStateBLL();
                HouseLayoutBLL hlBLL = new HouseLayoutBLL();
                HouseBLL houseBLL = new HouseBLL();
                public HouseListViewModel()
                {
                        this.HouseList = GetHouseList();
                        InitToolbarInfo();
                        this.CheckAllCmd = GetCheckAllCmd(this.HouseList);
                        this.CheckItemCmd = GetCheckItemCmd(this.HouseList);
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


                public string RSName
                {
                        get; set;
                }

                private int rsId;

                public int RSId
                {
                        get { return rsId; }
                        set
                        {
                                rsId = value;
                                OnPropertyChanged();
                        }
                }


                public List<RentSaleModel> CboRentSales
                {
                        get
                        {
                                return new List<RentSaleModel>()
                                {
                                        new RentSaleModel(){ RSId=0,RSName= "全部" },
                                        new RentSaleModel(){RSId=1,RSName="出租" },
                                        new RentSaleModel(){RSId=2,RSName="出售" }
                                };
                        }
                }

                private string stateName="全部";

                public string StateName
                {
                        get { return stateName; }
                        set
                        {
                                stateName = value;
                                OnPropertyChanged();
                        }
                }

                private List<HouseStateInfo> houseStates = new List<HouseStateInfo>();
                public List<HouseStateInfo> CboStates
                {
                        get
                        {
                                houseStates = GetHouseStates();
                                return houseStates;
                        }
                        set
                        {
                                houseStates = value;
                                OnPropertyChanged();
                        }
                }

              

                private string layoutName="全部";
 
                public string LayoutName
                {
                        get { return layoutName; }
                        set
                        {
                                layoutName = value;
                                OnPropertyChanged();
                        }
                }

                public List<HouseLayoutInfo> CboLayouts
                {
                        get
                        {
                                List<HouseLayoutInfo> list = hlBLL.GetHouseLayouts();
                                list.Insert(0, new HouseLayoutInfo()
                                {
                                        HLId = 0,
                                        HLName = "全部"
                                });
                                return list;
                        }
                }

       
                public List<string> CboDirections
                {
                        get
                        {
                                return CommonUtility.GetHouseDirectionList(true);
                        }
                }

                private string directionName="请选择";
      
                public string DirectionName
                {
                        get { return directionName; }
                        set
                        {
                                directionName = value;
                                OnPropertyChanged();
                        }
                }

                private int isPublishedSel=-1;

                public int IsPublishedSel
                {
                        get { return isPublishedSel; }
                        set
                        {
                                isPublishedSel = value;
                                OnPropertyChanged();
                        }
                }

                private ObservableCollection<HouseCheckInfo> houseList;
   
                public ObservableCollection<HouseCheckInfo> HouseList
                {
                        get { return houseList; }
                        set
                        {
                                houseList = value;
                                OnPropertyChanged();
                        }
                }

                #region 命令定义 

                public ICommand SelPublishedCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        if (o != null)
                                        {
                                                int intPubVal = o.GetInt();
                                                this.IsPublishedSel = intPubVal;
                                                ReloadHouseList();
                                        }
                                });
                        }
                }

      
                public RelayCommand FindHouseListCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        ReloadHouseList();
                                });
                        }
                }

                public RelayCommand LoadHouseStatesCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        CboStates = GetHouseStates();
                                        this.StateName = "全部";
                                        if (this.RSId == 0)
                                                this.RSName = "";
                                        else if (RSId == 1)
                                                this.RSName = "出租";
                                        else
                                                RSName = "出售";
                                        this.HouseList = GetHouseList();
                                });
                        }
                }


                public RelayCommand TradeHouseCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        if (o != null)
                                        {
                                                object[] paras = o as object[];
                                                int houseId = (int)paras[0];

                                                HouseTradeViewModel houseTradeVM = new HouseTradeViewModel(houseId);
                                                houseTradeVM.ReloadListEvent += ReloadHouseList;//刷新房屋列表
                                                AddUCTabItem((UserControl)paras[1],"房屋交易页面","houseTrade", houseTradeVM);
                                        }

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
                                ShowImport = Visibility.Visible,
                                ShowPublish = Visibility.Visible,
                                ShowUnPublish = Visibility.Visible,
                                AddCommand = new RelayCommand(o => ShowHouseInfoPage(1, 0, o), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowHouseInfoPage(2, o), b => IsEditable(this.HouseList)),
                                DeleteCommand = new RelayCommand(o => DeleteHouses(1), b => IsEditable(this.HouseList)),
                                RecoverCommand = new RelayCommand(o => DeleteHouses(0), b => IsDeletedable(this.HouseList)),
                                RemoveCommand = new RelayCommand(o => DeleteHouses(2), b => IsDeletedable(this.HouseList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowHouseInfoPage(4, o), b => this.HouseList.Count > 0),
                                FindCommand = FindHouseListCmd,
                                ImportCommand = new RelayCommand(o => ImportHouseInfos()),
                                PublishCommand = new RelayCommand(o => PublishHouses(1, o), b => IsEditable(this.HouseList) && IsPublishedSel == 0),
                                UnPublishCommand = new RelayCommand(o => PublishHouses(0, o), b => IsEditable(this.HouseList) && IsPublishedSel == 1)
                        };
                }

                private List<HouseStateInfo> GetHouseStates()
                {
                        List<HouseStateInfo> list = hsBLL.GetHouseStates(this.RSId);
                        list.Insert(0, new HouseStateInfo()
                        {
                                RSId = 0,
                                StateId = 0,
                                StateName = "全部"
                        });
                        return list;
                }

                private ObservableCollection<HouseCheckInfo> GetHouseList()
                {
                        ObservableCollection<HouseCheckInfo> reList = new ObservableCollection<HouseCheckInfo>();
                        string hdName = this.DirectionName == "请选择" ? "" : this.DirectionName;
                        string hstate = this.StateName == "全部" ? "" : StateName;
                        string hlName = this.LayoutName == "全部" ? "" : LayoutName;
                        List<ViewHouseInfo> hList = houseBLL.GetHouseList(this.KeyWords, this.RSName, hdName, hlName, hstate, this.IsPublishedSel, IsShowDel);
                        hList.ForEach(h => reList.Add(new HouseCheckInfo()
                        {
                                ViewHouseInfo = h,
                                IsCheck = false
                        }));
                        return reList;
                }

       
                private void ShowHouseInfoPage(int actType, int houseId, object uc)
                {
                        HouseInfoViewModel houseVM = new HouseInfoViewModel(actType, houseId);
                        houseVM.ReloadListEvent += ReloadHouseList;
                        string typeName = GetInfoTypeName(actType);
                        AddUCTabItem((UserControl)uc, $"房屋信息——{typeName}页面", "houseInfoView", houseVM);
                }


                public void ShowHouseInfoPage(int actType, object uc)
                {
                        if (this.CurrentItem != null)
                        {
                                HouseCheckInfo info = this.CurrentItem as HouseCheckInfo;
                                ShowHouseInfoPage(actType, info.ViewHouseInfo.HouseId, uc);
                        }
                }

    
                private void ReloadHouseList()
                {
                        this.HouseList = GetHouseList();
                        this.CheckAllCmd = GetCheckAllCmd(this.HouseList);
                        this.ToolBarInfo.OnCanExecuteChanged();
                }

                private void ImportHouseInfos()
                {
                        var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx| Excel Files 2003 (*.xls)|*.xls" };
                        var result = ofd.ShowDialog();
                        if (result == false) return;
                        string fileName = ofd.FileName;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                                string msgTitle = "导入房屋信息";
                                int re = houseBLL.ImportHouseData(fileName, "房屋数据", true);
                                if (re == 1)
                                {
                                        //导入成功
                                        ShowMessage("房屋信息导入成功！", msgTitle);
                                        ReloadHouseList();
                                }
                                else if (re == 0)
                                {
                                        //导入失败
                                        ShowError("房屋信息导入失败，请检查导入格式！", msgTitle);
                                        return;
                                }
                                else
                                {
                                        //没有数据可导入
                                        ShowError("导入文件中没有数据！", msgTitle);
                                        return;
                                }
                        }
                }


                public void DeleteHouses(int isDeleted)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"房屋{delType}";
                        List<int> delIds = new List<int>();
                        delIds = this.HouseList.Where(r => r.IsCheck == true).Select(r => r.ViewHouseInfo.HouseId).ToList();
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        List<int> hasPubIds = houseBLL.GetPublishedHouseIds(delIds);//返回包括已发布房屋id
                                        //List<int> hasTradeIds = houseBLL.GetTradeHouseIds(delIds);//返回已交易的房屋id
                                        if (hasPubIds.Count > 0)
                                        {
                                                ShowError($"选择的房屋信息中包含已发布的房屋，它们的编号是：{string.Join(",", hasPubIds)}", msgTile);
                                                return;
                                        }
                                        //else if (hasTradeIds.Count > 0)
                                        //{
                                        //        ShowError($"选择的房屋信息中包含已交易的房屋，它们的编号是：{string.Join(",", hasTradeIds)}", msgTile);
                                        //        return;
                                       // }
                                }
                                if (ShowQuestion($"您确定要{delType}选择的房屋信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = houseBLL.LogicDelHouseList(delIds);
                                                        break;
                                                case 0:
                                                        isDel = houseBLL.RecoverHouseList(delIds);
                                                        break;
                                                case 2:
                                                        isDel = houseBLL.RemoveHouseList(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的房屋信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                                //刷新列表数据
                                                ReloadHouseList();
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


                public void PublishHouses(int isPublish, object uc)
                {
                        string typeName = isPublish == 1 ? "发布" : "取消发布";
                        string msgTitle = $"{typeName}房屋信息";
                        List<int> Ids = new List<int>();
                        //获取勾选的房屋列表
                        List<HouseCheckInfo> selList = this.HouseList.Where(h => h.IsCheck == true).ToList();
                        Ids = selList.Select(h => h.ViewHouseInfo.HouseId).ToList();
                        if (Ids.Count > 0)
                        {
                                if (ShowQuestion($"您确定要{typeName}选择的房屋信息吗？", msgTitle) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        string userName = GetLoginUser(uc);//记录发布者  获取登录者名称
                                        bool bl = isPublish == 1 ? houseBLL.PublishHouse(Ids, userName) : houseBLL.UnPublishHouse(Ids);//发布或取消发布  
                                        string suc = bl ? "成功" : "失败！";
                                        string msg = $"选择的房屋信息{typeName} {suc}！";
                                        if (bl)
                                        {
                                                ShowMessage(msg, msgTitle);
                                                ReloadHouseList();
                                        }
                                        else
                                        {
                                                ShowError(msg, msgTitle);
                                                return;
                                        }
                                }
                        }
                }

        }
}
