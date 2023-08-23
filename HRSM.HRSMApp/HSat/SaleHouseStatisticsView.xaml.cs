using LiveCharts;
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

namespace HRSM.HRSMApp.HSat
{
    /// <summary>
    /// SaleHouseStatisticsView.xaml 的交互逻辑
    /// </summary>
    public partial class SaleHouseStatisticsView : UserControl
    {
        public SaleHouseStatisticsView()
        {
            InitializeComponent();
            //PointLabel = chartPoint => string.Format("{0}:{1} ({2:P})", chartPoint.SeriesView.Title, chartPoint.Y, chartPoint.Participation);
            //DataContext = this;
        }
       // public Func<ChartPoint, string> PointLabel { get; set; }
    }
}
