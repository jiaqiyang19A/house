using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.CRM
{
        public  class CustomerRequestCheckInfo:InfoCheckViewModelBase
        {
                private ViewCustomerRequestInfo custRequestInfo;

                public ViewCustomerRequestInfo CustRequestInfo
                {
                        get { return custRequestInfo; }
                        set { custRequestInfo = value;
                                OnPropertyChanged();
                        }
                }

        }
}
