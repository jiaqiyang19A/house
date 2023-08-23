using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HRSM.HRSMApp.ViewModels.BM
{
    public class HouseCheckInfo:InfoCheckViewModelBase
    {
                private ViewHouseInfo vhouseInfo;

                public ViewHouseInfo ViewHouseInfo
                {
                        get { return vhouseInfo; }
                        set { vhouseInfo = value;
                                OnPropertyChanged();
                        }
                }

     
                private Visibility isTradeVisibility = Visibility.Hidden;

                public Visibility IsTradeVisibility
                {
                        get {
                                isTradeVisibility = (vhouseInfo.IsPublish && (vhouseInfo.HouseState == "未出租" || vhouseInfo.HouseState == "未出售")) ? Visibility.Visible : Visibility.Hidden;
                                return isTradeVisibility; }
                        set { isTradeVisibility = value;
                                OnPropertyChanged();
                        }
                }


        }
}
