using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class TreeMenuItem : ViewModelBase
    {
        public TreeMenuItem()
        {
            SubItems = new ObservableCollection<TreeMenuItem>();
        }

        private MenuInfo menuInfo = new MenuInfo();

        private bool? isCheck = false;
        public bool? IsCheck
        {
            get { return isCheck; }
            set
            {
                isCheck = value;
                OnPropertyChanged();
            }
        }


        public int MenuId
        {
            get { return menuInfo.MenuId; }
            set { menuInfo.MenuId = value;
                OnPropertyChanged();
            }
        }

        public string MenuName
        {
            get { return menuInfo.MenuName; }
            set { menuInfo.MenuName = value; 
               OnPropertyChanged();
            }
        }

        private TreeMenuItem parentNode = null;
        public TreeMenuItem ParentNode
        {
            get { return parentNode; }
            set { parentNode = value;  
            }
        }

        private ObservableCollection<TreeMenuItem> subItems = null;
        public ObservableCollection<TreeMenuItem> SubItems
        {
            get { return subItems; }
            set
            {
                subItems = value;
                SetParentValue();
            }
        }

        private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            isCheck = value;
            if (updateChildren && isCheck.HasValue && SubItems.Count > 0)
            {
                foreach (var c in this.SubItems)
                {
                    c.SetIsChecked(isCheck, true, false);
                }
            }
            if (updateParent && ParentNode != null)
            {
                ParentNode.VerifyCheckState();
            }
            OnPropertyChanged("IsCheck");
        }

        private void VerifyCheckState()
        {
            bool? state = false;
            for (int i = 0; i < this.SubItems.Count; ++i)
            {
                bool? current = this.SubItems[i].IsCheck;
                if (current.Value == true)
                {
                    state = true;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

        private void SetParentValue()
        {
            if (this.SubItems.Count > 0)
            {
                foreach (var c in this.SubItems)
                {
                    c.ParentNode = this;
                }
            }
        }

        public ICommand CheckItemCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    this.SetIsChecked(this.IsCheck, true, true);
                });
            }
        }
    }
}
