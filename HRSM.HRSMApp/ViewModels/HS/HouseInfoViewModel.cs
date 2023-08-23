using HRSM.BLL;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.HS
{
    public  class HouseInfoViewModel:ViewModelBase
    {
        HouseBLL houseBLL = new HouseBLL();
        private ViewHouseInfo houseInfo;
        public HouseInfoViewModel(int houseId)
        {
            houseInfo = new ViewHouseInfo();
            
            houseInfo = houseBLL.GetHouseInfo(houseId);
            if(houseInfo!=null)
            {
                if (!string.IsNullOrEmpty(houseInfo.HousePic))
                    houseInfo.HousePic = "pack://siteoforigin:,,,/"+ houseInfo.HousePic;
                else
                    houseInfo.HousePic = "../imgs/house.jpg";
            }
        }




        public ViewHouseInfo HouseInfoModel
        {
            get
            {
                return houseInfo;
            }
        }

    }
}
