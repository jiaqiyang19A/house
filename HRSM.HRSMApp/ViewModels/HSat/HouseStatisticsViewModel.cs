using Common;
using HRSM.BLL;
using HRSM.Models.VModels;
using LiveCharts;
using LiveCharts.Wpf;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.HSat
{
        public class HouseStatisticsViewModel:ViewModelBase
        {
                HouseBLL houseBLL = new HouseBLL();
                public HouseStatisticsViewModel()
                {
                        houseStat = houseBLL.GetHouseCountStatistics();
                }
                private ViewHouseCountSatisticsInfo houseStat = new ViewHouseCountSatisticsInfo();
 
                public SeriesCollection HouseSeriesList
                {
                        get
                        {
                                return new SeriesCollection()
                                {
                                        new ColumnSeries
                                        {
                                                Title="总量",
                                                Values=new ChartValues<int>{PublishedCount,TRentCount,TSaleCount},
                                                DataLabels=true
                                        },
                                       new ColumnSeries
                                        {
                                                Title="已租售",
                                                Values=new ChartValues<int>{HasRentCount+HasSaleCount,HasRentCount,HasSaleCount},
                                                DataLabels=true
                                        },
                                        new ColumnSeries
                                        {
                                                Title="未租售",
                                                Values=new ChartValues<int>{UnRentCount+UnSaleCount,UnRentCount,UnSaleCount},
                                                DataLabels=true
                                        }
                                };
                        }
                }

            
         
                public List<string> Labels
                {
                        get
                        {
                                return new List<string>() { "总量", "出租", "出售" };
                        }
                }
   
                public int TotalCount
                {
                        get { return houseStat.TotalCount; }
                }
      
                public int TRentCount
                {
                        get { return houseStat.TRentCount; }
                }

                public int TSaleCount
                {
                        get { return houseStat.TSaleCount; }
                }
      
                public int PublishedCount
                {
                        get { return houseStat.PublishedCount; }
                }
  
                public int UnPublishedCount
                {
                        get { return houseStat.UnPublishedCount; }
                }
      
                public int HasRentCount
                {
                        get { return houseStat.HasRentCount; }
                }
  
                public int HasSaleCount
                {
                        get { return houseStat.HasSaleCount; }
                }
   
                public int UnRentCount
                {
                        get { return houseStat.UnRentCount; }
                }
      
                public int UnSaleCount
                {
                        get { return houseStat.UnSaleCount; }
                }

                public ICommand ExportDataCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
                                        sfd.Filter = "Excel Files (*.xlsx)|*.xlsx| Excel Files 2003 (*.xls)|*.xls";
                                        var dr = sfd.ShowDialog();
                                        if (dr == true)
                                        {
                                                string fileName = sfd.FileName;
                                                string extName = System.IO.Path.GetExtension(fileName);
                                                IWorkbook workbook = ExcelHelper.CreateWorkBook(extName);
                                                ISheet sheet = ExcelHelper.CreateSheet(workbook, "房屋数量统计");
                                                int count = 0;
                                                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
                                                {
                                                        IRow row = sheet.CreateRow(count);
                                                        ICell cell0 = row.CreateCell(0);
                                                        string strTitle = "房屋数量统计";
                                                        cell0.SetCellValue(strTitle);
                                                        ICellStyle style = workbook.CreateCellStyle();
                                                        style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                                                        IFont font = workbook.CreateFont();
                                                        font.FontHeight = 20 * 20;
                                                        style.SetFont(font);
                                                        cell0.CellStyle = style;
                                                        //合并单元格
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 5));
                                                        ++count;

                                                        IRow sRow = sheet.CreateRow(count);
                                                        sRow.CreateCell(0).SetCellValue($"房屋总计({TotalCount})");
                                                        //合并单元格
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 1));

                                                        sRow.CreateCell(2).SetCellValue($"出租({TRentCount})");
                                                        //合并单元格
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 2, 3));

                                                        sRow.CreateCell(4).SetCellValue($"出售({TSaleCount})");
                                                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 4, 5));

                                                        ++count;
                                                        IRow tRow = sheet.CreateRow(count);
                                                        tRow.CreateCell(0).SetCellValue("已发布");
                                                        tRow.CreateCell(1).SetCellValue("未发布");
                                                        tRow.CreateCell(2).SetCellValue("已出租");
                                                        tRow.CreateCell(3).SetCellValue("未出租");
                                                        tRow.CreateCell(4).SetCellValue("已出售");
                                                        tRow.CreateCell(5).SetCellValue("未出售");
                                                        ++count;

                                                        IRow rowData = sheet.CreateRow(count);
                                                        rowData.CreateCell(0).SetCellValue(PublishedCount.ToString());
                                                        rowData.CreateCell(1).SetCellValue(UnPublishedCount.ToString());
                                                        rowData.CreateCell(2).SetCellValue(HasRentCount.ToString());
                                                        rowData.CreateCell(3).SetCellValue(UnRentCount.ToString());
                                                        rowData.CreateCell(4).SetCellValue(HasSaleCount.ToString());
                                                        rowData.CreateCell(5).SetCellValue(UnSaleCount.ToString());
                                                        workbook.Write(fs); //写入到excel
                                                        ShowMessage("导出成功！");
                                                }

                                        }

                                });
                        }
                }

        }
}
