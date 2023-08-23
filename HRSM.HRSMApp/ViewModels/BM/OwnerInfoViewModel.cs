using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.BM
{
        public class OwnerInfoViewModel : InfoViewModelBase
        {
                OwnerBLL ownerBLL = new OwnerBLL();
                public OwnerInfoViewModel()
                { }


                public OwnerInfoViewModel(int actType, int ownerId)
                {
                        this.ActType = actType;
                        this.OwnerId = ownerId;
                        switch (ActType)
                        {
                                case 1:
                                        this.ConfirmBtnContent = "新增";
                                        this.OwnerType = "个人";
                                        this.IsConfirmBtnEnabled = true;
                                        break;
                                case 2:
                                        this.ConfirmBtnContent = "修改";
                                        this.ownerInfo = ownerBLL.GetOwnerInfo(ownerId);
                                        this.IsConfirmBtnEnabled = true;
                                        oldName = this.ownerInfo.OwnerName;
                                        oldPhone = this.ownerInfo.OwnerPhone;
                                        break;
                                case 4:
                                        this.ownerInfo = ownerBLL.GetOwnerInfo(ownerId);
                                        this.ConfirmBtnVisible = Visibility.Hidden;
                                        break;
                                default:
                                        break;
                        }

                }

                private HouseOwnerInfo ownerInfo = new HouseOwnerInfo();
                private string oldName = "";//修改前的业主名称
                private string oldPhone = "";//修改前的业主电话
                private int ownerId;
                public int OwnerId
                {
                        get { return ownerId; }
                        set { ownerId = value; }
                }


                public List<string> CboOwnerTypes
                {
                        get
                        {
                                return new List<string>() { "个人", "单位"  };
                        }
                }

                public string OwnerType
                {
                        get { return ownerInfo.OwnerType; }
                        set
                        {
                                ownerInfo.OwnerType = value;
                                OnPropertyChanged();
                        }
                }

   
                public string OwnerName
                {
                        get { return ownerInfo.OwnerName; }
                        set
                        {
                                ownerInfo.OwnerName = value;
                                OnPropertyChanged();
                        }
                }

      
                public string Contactor
                {
                        get { return ownerInfo.Contactor; }
                        set
                        {
                                ownerInfo.Contactor = value;
                                OnPropertyChanged();
                        }
                }

       
                public string OwnerPhone
                {
                        get { return ownerInfo.OwnerPhone; }
                        set
                        {
                                ownerInfo.OwnerPhone = value;
                                OnPropertyChanged();
                        }
                }
   
                public string OwnerAddress
                {
                        get { return ownerInfo.OwnerAddress; }
                        set
                        {
                                ownerInfo.OwnerAddress = value;
                                OnPropertyChanged();
                        }
                }

   
                public string Remark
                {
                        get { return ownerInfo.Remark; }
                        set
                        {
                                ownerInfo.Remark = value;
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
                                        string msgTitle = $"业主{actMsg}";
                                        if(string.IsNullOrEmpty(OwnerName))
                                        {
                                                ShowError("请输入业主名称！", msgTitle);
                                                return;
                                        }
                                        if (string.IsNullOrEmpty(OwnerPhone))
                                        {
                                                ShowError("请输入业主电话！", msgTitle);
                                                return;
                                        }
                                        if(ownerId==0||(oldName!=""&&oldName!=OwnerName)||(oldPhone != "" && oldPhone != OwnerPhone))
                                        {
                                                if(ownerBLL.Exists(OwnerName,OwnerPhone))
                                                {
                                                        ShowError("该业主已存在！", msgTitle);
                                                        return;
                                                }
                                        }
                                        bool bl = ActType == 1 ? ownerBLL.AddOwnerInfo(ownerInfo) : ownerBLL.UpdateOwnerInfo(ownerInfo);
                                        string sucType = bl ? "成功" : "失败";
                                        string msgInfo = $"业主：{OwnerName} {actMsg}{sucType}!";
                                        if(bl)
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
