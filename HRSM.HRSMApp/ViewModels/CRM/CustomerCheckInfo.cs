using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.HRSMApp.ViewModels.CRM
{
        public class CustomerCheckInfo:InfoCheckViewModelBase
        {

                private CustomerInfo custInfo = new CustomerInfo();
                public CustomerInfo CustInfo
                {
                        get { return custInfo; }
                        set
                        {
                                custInfo = value;
                                OnPropertyChanged();
                        }
                }
        }
}
