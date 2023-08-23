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
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.BM
{
        public class OwnerListViewModel : ListViewModelBase
        {
                OwnerBLL ownerBLL = new OwnerBLL();
                public OwnerListViewModel()
                {
                        this.OwnerList = GetOwnerList();
                        this.FindOwnerListCmd = new RelayCommand(o =>
                        {
                                ReloadOwnerList();
                        });
                        InitToolbarInfo();//初始化工具栏---配置
                        this.CheckAllCmd = GetCheckAllCmd(this.OwnerList);
                        this.CheckItemCmd = GetCheckItemCmd(this.OwnerList);
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

                private string ownerType;
     
                public string OwnerType
                {
                        get { return ownerType; }
                        set
                        {
                                ownerType = value;
                                OnPropertyChanged();
                        }
                }

            
                public List<string> CboOwnerTypes
                {
                        get
                        {
                                return new List<string> { "全部", "单位", "个人" };
                        }
                }

                private ObservableCollection<OwnerCheckInfo> ownerList;
         
                public ObservableCollection<OwnerCheckInfo> OwnerList
                {
                        get { return ownerList; }
                        set
                        {
                                ownerList = value;
                                OnPropertyChanged();
                        }
                }

        
                private ObservableCollection<OwnerCheckInfo> GetOwnerList()
                {
                        ObservableCollection<OwnerCheckInfo> list = new ObservableCollection<OwnerCheckInfo>();
                        List<HouseOwnerInfo> reList = ownerBLL.GetOwnerList(this.KeyWords, OwnerType, IsShowDel);
                        reList.ForEach(own => list.Add(new OwnerCheckInfo()
                        {
                                OwnerInfo = own,
                                IsCheck = false
                        }));
                        return list;
                }

       
                private void ShowOwnerInfoPage(int actType, int ownerId, object uc)
                {
                        OwnerInfoViewModel ownerVM = new OwnerInfoViewModel(actType, ownerId);
                        ownerVM.ReloadListEvent += ReloadOwnerList;
                        string typeName = GetInfoTypeName(actType);
                        AddUCTabItem((UserControl)uc, $"业主信息——{typeName}页面", "ownerInfo", ownerVM);
                }
   
                private void ShowOwnerInfoPage(int actType, object uc)
                {
                        if (this.CurrentItem != null)
                        {
                                OwnerCheckInfo info = this.CurrentItem as OwnerCheckInfo;
                                ShowOwnerInfoPage(actType, info.OwnerInfo.OwnerId, uc);
                        }
                }

         
                private void ReloadOwnerList()
                {
                        this.OwnerList = GetOwnerList();
                        this.CheckAllCmd = GetCheckAllCmd(this.OwnerList);
                        this.ToolBarInfo.OnCanExecuteChanged();//刷新工具栏

                }

                private void InitToolbarInfo()
                {
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                ShowFind = Visibility.Visible,
                                ShowImport = Visibility.Visible,
                                AddCommand = new RelayCommand(o => ShowOwnerInfoPage(1, 0, o), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowOwnerInfoPage(2, o), b => IsEditable(this.OwnerList)),
                                DeleteCommand = new RelayCommand(o => DeleteOwners(1), b => IsEditable(this.OwnerList)),
                                RecoverCommand = new RelayCommand(o => DeleteOwners(0), b => IsDeletedable(this.OwnerList)),
                                RemoveCommand = new RelayCommand(o => DeleteOwners(2), b => IsDeletedable(this.OwnerList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowOwnerInfoPage(4, o), b => this.OwnerList.Count > 0),
                                FindCommand = FindOwnerListCmd,
                                ImportCommand = new RelayCommand(o => ImportOwnerInfos())
                        };
                }


                private void DeleteOwners(int isDeleted)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"业主{delType}";
                        List<int> delIds = new List<int>();
                        delIds = this.OwnerList.Where(r => r.IsCheck == true).Select(r => r.OwnerInfo.OwnerId).ToList();
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        List<int> hasOwnerIds = ownerBLL.GetOwnersHasHousesIds(delIds);
                                        if (hasOwnerIds.Count > 0)
                                        {
                                                ShowError($"选择的业主信息中包含已有房屋的业主，它们的编号是：{string.Join(",", hasOwnerIds)}", msgTile);
                                                return;
                                        }
                                }
                                if (ShowQuestion($"您确定要{delType}选择的业主信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = ownerBLL.LogicDelOwnerList(delIds);
                                                        break;
                                                case 0:
                                                        isDel = ownerBLL.RecoverOwnerList(delIds);
                                                        break;
                                                case 2:
                                                        isDel = ownerBLL.RemoveOwnerList(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的业主信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                                //刷新列表数据
                                                ReloadOwnerList();
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

  
                private void ImportOwnerInfos()
                {
                        var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "Excel Files (*.xlsx)|*.xlsx| Excel Files 2003 (*.xls)|*.xls" };
                        var result = ofd.ShowDialog();
                        if (result == false) return;
                        string fileName = ofd.FileName;
                        if (!string.IsNullOrEmpty(fileName))
                        {
                                string msgTitle = "导入业主信息";
                                int re = ownerBLL.ImportOwnerInfos(fileName, "业主数据", true);
                                if (re == 1)
                                {
                                        //导入成功
                                        ShowMessage("业主信息导入成功！", msgTitle);
                                        ReloadOwnerList();
                                }
                                else if (re == 0)
                                {
                                        //导入失败
                                        ShowError("业主信息导入失败，请检查导入格式！", msgTitle);
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


                #region 命令

                private RelayCommand findOwnerListCmd;
                public RelayCommand FindOwnerListCmd
                {
                        get { return findOwnerListCmd; }
                        set
                        {
                                findOwnerListCmd = value;
                                OnPropertyChanged();
                        }
                }

          
                #endregion
        }
}
