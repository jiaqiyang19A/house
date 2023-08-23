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

namespace HRSM.HRSMApp.SM
{
    /// <summary>
    /// RoleList.xaml 的交互逻辑
    /// </summary>
    public partial class RoleList : UserControl
    {
        public RoleList()
        {
            InitializeComponent();
            this.Register<RoleInfoWindow>("roleInfo");
            this.Register<RightSet>("rightSet");
        }
    }
}
