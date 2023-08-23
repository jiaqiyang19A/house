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

namespace HRSM.HRSMApp.ViewModels.HSat
{
        public class SaleTimeStatisticsViewModel:ViewModelBase
        {
                HouseBLL houseBLL = new HouseBLL();
                public SaleTimeStatisticsViewModel()
                {
                        ReloadSaleList();
                }

               private void ReloadSaleList()
                {
                        this.SaleList = GetSaleList();
                        this.PieSeriesList = CreatePieSeriesList();
                }

                private string saleName;

                public string SaleName
                {
                        get { return saleName; }
                        set { saleName = value;
                                OnPropertyChanged();
                        }
                }

                private string stTime;
        
                public string StTime
                {
                        get { return stTime; }
                        set
                        {
                                stTime = value;
                                OnPropertyChanged();
                        }
                }

                private string etTime;
      
                public string EtTime
                {
                        get { return etTime; }
                        set
                        {
                                etTime = value;
                                OnPropertyChanged();
                        }
                }

              

                private List<ViewSaleHouseStatisticsInfo> saleList = new List<ViewSaleHouseStatisticsInfo>();
                
                public List<ViewSaleHouseStatisticsInfo> SaleList
                {
                        get { return saleList; }
                        set
                        {
                                saleList = value;
                                OnPropertyChanged();
                        }
                }

                 private SeriesCollection pieSeriesList;
 
                public SeriesCollection PieSeriesList
                {
                        get { return pieSeriesList; }
                        set { pieSeriesList = value;
                                OnPropertyChanged();
                        }
                }

        
                private Func<ChartPoint, string> PointLabel => cp => string.Format("{0}:{1} ({2:P})", cp.SeriesView.Title, cp.Y, cp.Participation);

      
                public ICommand FindSaleListCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        ReloadSaleList();
                                });
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
                                                ISheet sheet = ExcelHelper.CreateSheet(workbook, "业务员销售数据");
                                                int count = 0;
                                                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                                                {

                                                        IRow row = sheet.CreateRow(count);
                                                        ICell cell0 = row.CreateCell(0);

                                                        string strTitle = "";
                                                        if (this.StTime == null && this.EtTime == null)
                                                                strTitle = "所有业务员销售量统计数据";
                                                        else
                                                        {
                                                                if (!string.IsNullOrEmpty(this.StTime))
                                                                        strTitle += "统计时间：" + this.StTime;
                                                                if (!string.IsNullOrEmpty(this.EtTime))
                                                                        strTitle += " 至 " + this.EtTime;
                                                                strTitle += "  业务员销售量统计数据";
                                                        }

                                                        cell0.SetCellValue(strTitle);
                                                        ICellStyle style = workbook.CreateCellStyle();
                                                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                                                        IFont font = workbook.CreateFont();
                                                        font.FontHeight = 20 * 20;
                                                        style.SetFont(font);
                                                        cell0.CellStyle = style;
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 4));
                                                        ++count;
                                                        IRow sRow = sheet.CreateRow(count);
                                                        sRow.CreateCell(0).SetCellValue("销售员信息");
                                                        //合并单元格
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 1));
                                                        sRow.CreateCell(2).SetCellValue("租售");
                                                        //合并单元格
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 2, 3));
                                                        sRow.CreateCell(4).SetCellValue("总计");
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
                                                        workbook.Write(fs); 

                                                }
                                                ShowMessage("导出成功！");
                                        }
                                });
                        }
                }



                private SeriesCollection CreatePieSeriesList()
                {
                        SeriesCollection seriesList = new SeriesCollection();
                        foreach (ViewSaleHouseStatisticsInfo info in this.SaleList)
                        {
                                seriesList.Add(new PieSeries()
                                {
                                        Title = info.UserFName,
                                        Values = new ChartValues<int> { info.TotalCount },
                                        LabelPoint = PointLabel,
                                        DataLabels = true
                                });
                        }
                        return seriesList;
                }
       
                private List<ViewSaleHouseStatisticsInfo> GetSaleList()
                {
                        DateTime? dEtTime = null,dStTime=null;
                        if (!string.IsNullOrEmpty(this.EtTime))
                        {
                                DateTime ddetTime;
                                bool bl= DateTime.TryParse(this.EtTime, out ddetTime);
                                if (bl)
                                        dEtTime = ddetTime;
                        }
                             
                        if (!string.IsNullOrEmpty(this.StTime))
                        {
                                 DateTime ddstTime;
                                 bool bl= DateTime.TryParse(this.StTime,out ddstTime);
                                if (bl)
                                        dStTime = ddstTime;
                        }
                              
                        return houseBLL.GetSaleTimeHouseStatisticsData(this.SaleName, dStTime, dEtTime)
;                }

        }
}
