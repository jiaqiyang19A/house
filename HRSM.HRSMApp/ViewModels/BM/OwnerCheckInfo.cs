using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.BM
{
    public class OwnerCheckInfo:InfoCheckViewModelBase
    {
        private HouseOwnerInfo ownerInfo;
        public HouseOwnerInfo OwnerInfo
        {
            get { return ownerInfo; }
            set
            {
                ownerInfo = value;
                OnPropertyChanged();
            }
        }
    }
}
