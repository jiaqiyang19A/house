using HRSM.HRSMApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HRSM.HRSMApp.CRM
{
        /// <summary>
        /// CustomerList.xaml 的交互逻辑
        /// </summary>
        public partial class CustomerList : UserControl
        {
                public CustomerList()
                {
                        InitializeComponent();
                        this.Register<CustomerRequestList>("custReqList");//意向客户需求列表页
                        this.Register<CustomerInfoView>("customerInfoView");//客户信息页面
                }
        }
}
