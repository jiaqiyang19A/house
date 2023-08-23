using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.CRM
{
        public  class CustomerFollowUpLogCheckInfo:InfoCheckViewModelBase
        {
                private ViewCustomerFollowUpLogInfo followUpLogInfo;

                public ViewCustomerFollowUpLogInfo FollowUpLogInfo
                {
                        get { return followUpLogInfo; }
                        set { followUpLogInfo = value;
                                OnPropertyChanged();
                        }
                }

        }
}
