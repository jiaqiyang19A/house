using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels
{
    /// <summary>
    /// 菜单节点实体模型
    /// </summary>
    public class MenuItemModel:ViewModelBase
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }

        /// <summary>
        /// 折叠与展开
        /// </summary>
        private bool isExpand = true;
        public bool IsExpand
        {
            get { return isExpand; }
            set
            {
                isExpand = value;
                OnPropertyChanged();
            }
        }

        public string MenuUrl { get; set; }
        //子菜单集合
        public ObservableCollection<MenuItemModel> SubMenuList { get; set; } = new ObservableCollection<MenuItemModel>();

    }
}
