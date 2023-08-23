using HRSM.HRSMApp.ViewModels.UC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels
{
    /// <summary>
    /// 列表页的ViewModel基类
    /// </summary>
        public class ListViewModelBase : ViewModelBase
        {
                /// <summary>
                /// 已删除筛选框勾选状态属性
                /// </summary>
                private bool isShowDel = false;
                public bool IsShowDel
                {
                        get { return isShowDel; }
                        set
                        {
                                isShowDel = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 全选状态
                /// </summary>
                private bool isCheckAll = false;
                public bool IsCheckAll
                {
                        get { return isCheckAll; }
                        set
                        {
                                isCheckAll = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 工具栏DataContext属性
                /// </summary>
                private ListToolBarViewModel toolBarInfo;
                public ListToolBarViewModel ToolBarInfo
                {
                        get { return toolBarInfo; }
                        set
                        {
                                toolBarInfo = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 选择的项
                /// </summary>
                private object selectedItem;

                public object SelectedItem
                {
                        get { return selectedItem; }
                        set { selectedItem = value; }
                }

                private object currentItem;
                /// <summary>
                /// 当前项
                /// </summary>
                public object CurrentItem
                {
                        get { return currentItem; }
                        set
                        {
                                currentItem = value;
                                OnPropertyChanged();
                        }
                }
                /// <summary>
                /// 全选命令
                /// </summary>
                private RelayCommand checkAllCmd;
                public RelayCommand CheckAllCmd
                {
                        get
                        {
                                return checkAllCmd;
                        }
                        set
                        {
                                checkAllCmd = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 单选命令
                /// </summary>
                private RelayCommand checkItemCmd;
                public RelayCommand CheckItemCmd
                {
                        get
                        {
                                return checkItemCmd;
                        }
                        set
                        {
                                checkItemCmd = value;
                                OnPropertyChanged();
                        }
                }


                /// <summary>
                /// 获取信息页面类别名称
                /// </summary>
                /// <param name="actType"></param>
                /// <returns></returns>
                public string GetInfoTypeName(int actType)
                {
                        string typeName = "";
                        switch (actType)
                        {
                                case 1: typeName = "新增"; break;
                                case 2: typeName = "修改"; break;
                                case 3: typeName = "添加子项"; break;
                                case 4: typeName = "详情"; break;
                                default:
                                        break;
                        }
                        return typeName;
                }

                /// <summary>
                /// 获取删除类别名称
                /// </summary>
                /// <param name="isDeleted"></param>
                /// <returns></returns>
                public string GetDelTypeName(int isDeleted)
                {
                        string typeName = "";
                        switch (isDeleted)
                        {
                                case 1: typeName = "删除"; break;
                                case 0: typeName = "恢复"; break;
                                case 2: typeName = "移除"; break;
                                default:
                                        break;
                        }
                        return typeName;
                }

                /// <summary>
                /// 添加子页到TabControl中
                /// </summary>
                /// <param name="uc"></param>
                /// <param name="header"></param>
                /// <param name="key"></param>
                /// <param name="VM"></param>
                public void AddUCTabItem(UserControl uc, string header, string key, object VM)
                {
                        CommonUtility.AddUCTabItem(uc, header, key, VM);
                }

                /// <summary>
                /// 全选或全不选处理
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="list"></param>
                public RelayCommand GetCheckAllCmd<T>(ObservableCollection<T> list) where T : InfoCheckViewModelBase
                {
                        return new RelayCommand(o =>
                        {
                                foreach (var info in list)
                                {
                                        info.IsCheck = this.IsCheckAll;
                                }
                        });
                }

                /// <summary>
                /// 单选命令
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="list"></param>
                /// <returns></returns>
                public RelayCommand GetCheckItemCmd<T>(ObservableCollection<T> list) where T : InfoCheckViewModelBase
                {
                        return new RelayCommand(o =>
                        {
                            //if (o != null)
                            //{
                            //       // var info = o as T;
                            //       // info.IsCheck = info.IsCheck;

                            //}
                            if (list.Where(r => r.IsCheck == true).Count() == 0)
                            {
                                IsCheckAll = false;
                            }
                            else
                                IsCheckAll = true;
                            DateTime dt = new DateTime(2020, 1, 1);
                        });
                }

                /// <summary>
                /// 修改、删除  工具项可用性
                /// </summary>
                /// <returns></returns>
                public bool IsEditable<T>(ObservableCollection<T> list)
                {
                        return (list.Count > 0 && !IsShowDel);
                }

                /// <summary>
                /// 恢复、移除工具项的可用性
                /// </summary>
                /// <returns></returns>
                public bool IsDeletedable<T>(ObservableCollection<T> list)
                {
                        return (list.Count > 0 && IsShowDel);
                }
        }
}
