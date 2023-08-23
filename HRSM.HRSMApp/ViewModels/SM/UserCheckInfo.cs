using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class UserCheckInfo:InfoCheckViewModelBase
    {
        private UserInfo userInfo;
        public UserInfo UserInfo
        {
            get { return userInfo; }
            set { userInfo = value;
                OnPropertyChanged();
            }
        }
    }
}
