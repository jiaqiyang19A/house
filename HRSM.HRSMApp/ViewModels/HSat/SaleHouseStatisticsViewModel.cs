
using Common;
using HRSM.BLL;
using HRSM.Models.VModels;
using LiveCharts;
using LiveCharts.Wpf;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace HRSM.HRSMApp.ViewModels.HSat
{
       public  class SaleHouseStatisticsViewModel:ViewModelBase
        {
                HouseBLL houseBLL = new HouseBLL();
                public SaleHouseStatisticsViewModel()
                {
                        this.SaleList = houseBLL.GetSaleHouseStatisticsData();//统计销售量列表
                        this.TotalCount = this.SaleList.Sum(h => h.TotalCount);
                        this.TotalRent = this.SaleList.Sum(h => h.RentCount);
                        this.TotalSale = this.SaleList.Sum(h => h.SaleCount);
                }

                private List<ViewSaleHouseStatisticsInfo> saleList=new List<ViewSaleHouseStatisticsInfo>();
         
                public List<ViewSaleHouseStatisticsInfo> SaleList
                {
                        get { return saleList; }
                        set { saleList = value;
                                OnPropertyChanged();
                        }
                }

                private int totalCount;
         
                public int TotalCount
                {
                        get { return totalCount; }
                        set { totalCount = value;
                                OnPropertyChanged();
                        }
                }

                private int totalRent;
          
                public int TotalRent
                {
                        get { return totalRent; }
                        set
                        {
                                totalRent = value;
                                OnPropertyChanged();
                        }
                }

                private int totalSale;
        
                public int TotalSale
                {
                        get { return totalSale; }
                        set
                        {
                                totalSale = value;
                                OnPropertyChanged();
                        }
                }

            
                public List<string> Labels
                {
                        get
                        {
                                return this.SaleList.Select(s => s.UserFName).ToList();
                        }
                }
           
                private Func<ChartPoint, string> PointLabel => cp => string.Format("{0}:{1} ({2:P})", cp.SeriesView.Title, cp.Y, cp.Participation);
   
                public SeriesCollection SeriesList
                {
                        get
                        {
                                ChartValues<int> totalValues = new ChartValues<int>();
                                this.SaleList.ForEach(s => totalValues.Add(s.TotalCount));
                                ChartValues<int> rentValues = new ChartValues<int>();
                                this.SaleList.ForEach(s => rentValues.Add(s.RentCount));
                                ChartValues<int> saleValues = new ChartValues<int>();
                                this.SaleList.ForEach(s => saleValues.Add(s.SaleCount));
                                return new SeriesCollection()
                                {
                                       CreateLineSeries("总量",totalValues,DefaultGeometries.Square),
                                       CreateLineSeries("出租",rentValues,DefaultGeometries.Circle),
                                       CreateLineSeries("出售",saleValues,DefaultGeometries.Diamond),
                                };
                        }
                }


                public SeriesCollection PieSeriesList
                {

                        get
                        {
                                return new SeriesCollection()
                                {
                                      CreatePieSeries("出租",TotalRent),
                                      CreatePieSeries("出售",TotalSale)
                                };
                        }
                }


                public ICommand ExportDataCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        ObservableCollection<DataGridColumn> cols = o as ObservableCollection<DataGridColumn>;
                                        Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                                        sfd.Filter = "Excel Files (*.xlsx)|*.xlsx| Excel Files 2003 (*.xls)|*.xls";
                                        var dr = sfd.ShowDialog();
                                        if (dr == true)
                                        {
                                                string fileName = sfd.FileName;
                                                string extName = System.IO.Path.GetExtension(fileName);
                                                IWorkbook workbook = ExcelHelper.CreateWorkBook(extName);
                                                ISheet sheet = ExcelHelper.CreateSheet(workbook, "业务员总销售量统计");
                                                int count = 0;
                                                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                                                {

                                                        IRow row = sheet.CreateRow(count);
                                                        ICell cell0 = row.CreateCell(0);

                                                        string strTitle = "";
                                                        strTitle = "业务员总销售量统计";


                                                        cell0.SetCellValue(strTitle);
                                                        ICellStyle style = workbook.CreateCellStyle();
                                                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                                                        IFont font = workbook.CreateFont();
                                                        font.FontHeight = 20 * 20;
                                                        style.SetFont(font);
                                                        cell0.CellStyle = style;
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));
                                                        ++count;

                                                        IRow tRow = sheet.CreateRow(count);
                                                        for (int j = 0; j < cols.Count; j++)
                                                        {
                                                                tRow.CreateCell(j).SetCellValue(cols[j].Header.ToString());
                                                        }
                                                        ++count;
                                                        int cellCount = tRow.LastCellNum;
                                                        Type type = typeof(ViewSaleHouseStatisticsInfo);
                                                        PropertyInfo[] props = type.GetProperties();
                                                        for (int i = 0; i < this.SaleList.Count; ++i)
                                                        {
                                                                IRow rowData = sheet.CreateRow(count);
                                                                for (int j = tRow.FirstCellNum; j < cellCount; ++j)
                                                                {
                                                                        DataGridBoundColumn col = cols[j] as DataGridBoundColumn;
                                                                        string proName = (col.Binding as Binding).Path.Path;
                                                                        var p = type.GetProperty(proName);
                                                                        object val = p.GetValue(this.SaleList[i]);
                                                                        if (val == null)
                                                                                val = "";
                                                                        rowData.CreateCell(j).SetCellValue(val.ToString());
                                                                }
                                                                ++count;
                                                        }
                                                        ++count;
                                                        IRow tRow1 = sheet.CreateRow(count);

                                                        //合并单元格
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(count, count, 0, 4));
                                                        tRow1.CreateCell(0).SetCellValue($"销售总量：{TotalCount} 套");
                                                        workbook.Write(fs); //写入到excel

                                                }
                                                ShowMessage("导出成功！");
                                        }
                                });
                        }
                }


                private LineSeries CreateLineSeries(string title, ChartValues<int> values, Geometry pointGeometry)
                {
                        return new LineSeries
                        {
                                Title = title,
                                Values = values,
                                DataLabels = true,
                                PointGeometry = pointGeometry,
                                LineSmoothness = 0
                        };
                }


              private PieSeries  CreatePieSeries(string title, int val)
                {
                        return new PieSeries()
                        {
                                Title = title,
                                Values = new ChartValues<int> { val },
                                StrokeThickness = 0,
                                DataLabels = true,
                                LabelPoint=PointLabel,
                                LabelPosition = PieLabelPosition.InsideSlice,
                                Foreground = new SolidColorBrush(Colors.Orange)
                        };
                }


        }
}
