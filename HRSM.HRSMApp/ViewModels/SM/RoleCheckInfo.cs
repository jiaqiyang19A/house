using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public  class RoleCheckInfo:InfoCheckViewModelBase
    {
        private RoleInfo roleInfo;
        public RoleInfo RoleInfo
        {
            get { return roleInfo; }
            set { roleInfo = value;
                OnPropertyChanged();
            }
        }
    }
}
