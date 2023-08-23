using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels
{
    public  class InfoCheckViewModelBase:ViewModelBase
    {
        /// <summary>
        /// 项勾选状态
        /// </summary>
        private bool isCheck;

        public bool IsCheck
        {
            get { return isCheck; }
            set { isCheck = value;
                OnPropertyChanged();
            }
        }

    }
}
