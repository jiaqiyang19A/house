using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class MenuCheckInfo:InfoCheckViewModelBase
    {
        MenuBLL menuBLL = new MenuBLL();
        private MenuInfo menuInfo;
        public MenuInfo MenuInfo
        {
            get { return menuInfo; }
            set
            {
                menuInfo = value;
                OnPropertyChanged();
            }
        }
        public string ParentName
        {
            get
            {
                string pName = "";
                if (menuInfo.ParentId > 0)
                {
                    MenuInfo pMenu = menuBLL.GetMenuInfo(menuInfo.ParentId);
                    pName = pMenu.MenuName;
                }
                return pName;
            }
        }
    }
}
