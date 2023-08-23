using Common;
using HRSM.BLL;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.HS
{
    public class HouseShowListViewModel:ViewModelBase
    {
        HouseBLL houseBLL = new HouseBLL();
        public HouseShowListViewModel()
        {
            houseList = new List<ViewHouseInfo>();
            this.HouseList = GetShowHouseList();
        }

       

        private string houseName;
        public string HouseName
        {
            get { return houseName; }
            set { houseName = value; 
            }
        }


        public List<string> RSList
        {
            get
            {
                return CommonUtility.GetRSTypes(true);
            }
        }


        private string rsType;

        public string RSType
        {
            get {
                return rsType; 
            }
            set { rsType = value;
                OnPropertyChanged();
            }
        }

        public List<string> DirectionList
        {
            get
            {
                return CommonUtility.GetHouseDirectionList(true);
            }
        }

    
        private string houseDirection;

        public string HouseDirection
        {
            get { return houseDirection; }
            set { houseDirection = value;
            }
        }

      
        private string houseLayout;

        public string HouseLayout
        {
            get { return houseLayout; }
            set { houseLayout = value; }
        }

       
        private List<ViewHouseInfo> houseList;
        public List<ViewHouseInfo> HouseList
        {
            get {
                return houseList; }
            set
            {
                houseList = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetHouseListCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                     this.HouseList = GetShowHouseList();
                });
            }
        }

   
        public ICommand CheckHouseInfoCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if(o!=null)
                    {
                        
                        object[] paras = o as object[];
                        int houseId = paras[1].GetInt();
                        UserControl ucHouseList = paras[0] as UserControl;
                        
                        HouseInfoViewModel houseVM = new HouseInfoViewModel(houseId);
                        
                        CommonUtility.AddUCTabItem(ucHouseList, "查看房屋详情", "houseInfo", houseVM);
                    }
                });
            }
        }

    
        private List<ViewHouseInfo> GetShowHouseList()
        {
            string rentsale = this.RSType == "请选择" ? null : this.RSType;
            string direction = this.HouseDirection == "请选择" ? null : this.HouseDirection;
            List<ViewHouseInfo> hList = houseBLL.GetShowHouseList(this.HouseName, rentsale, direction, this.HouseLayout);
            return hList;
        }


    }
}
