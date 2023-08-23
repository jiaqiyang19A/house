using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.UC
{
        public class ListToolBarViewModel : ViewModelBase
        {
                /// <summary>
                /// 新增项的显示
                /// </summary>
                private Visibility showAdd = Visibility.Visible;
                public Visibility ShowAdd
                {
                        get { return showAdd; }
                        set
                        {
                                showAdd = value;
                                OnPropertyChanged();
                        }
                }

                private Visibility showEdit = Visibility.Visible;
                //显示修改按钮
                public Visibility ShowEdit
                {
                        get { return showEdit; }
                        set
                        {
                                showEdit = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showDelete = Visibility.Visible;
                //显示删除按钮
                public Visibility ShowDelete
                {
                        get { return showDelete; }
                        set
                        {
                                showDelete = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showRecover = Visibility.Visible;
                //显示恢复按钮
                public Visibility ShowRecover
                {
                        get { return showRecover; }
                        set
                        {
                                showRecover = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showRemove = Visibility.Visible;
                //显示移除按钮
                public Visibility ShowRemove
                {
                        get { return showRemove; }
                        set
                        {
                                showRemove = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showClose = Visibility.Visible;
                //显示关闭按钮
                public Visibility ShowClose
                {
                        get { return showClose; }
                        set
                        {
                                showClose = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showInfo = Visibility.Visible;
                //显示详情按钮
                public Visibility ShowInfo
                {
                        get { return showInfo; }
                        set
                        {
                                showInfo = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 查询项的显示
                /// </summary>
                private Visibility showFind = Visibility.Collapsed;
                public Visibility ShowFind
                {
                        get { return showFind; }
                        set
                        {
                                showFind = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showAllRequestList = Visibility.Collapsed;
                //显示所有客户需求按钮
                public Visibility ShowAllRequestList
                {
                        get { return showAllRequestList; }
                        set
                        {
                                showAllRequestList = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showCustRequestList = Visibility.Collapsed;
                //显示客户意向需求按钮
                public Visibility ShowCustRequestList
                {
                        get { return showCustRequestList; }
                        set
                        {
                                showCustRequestList = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showAllCustFULog = Visibility.Collapsed;
                //显示所有客户日志按钮
                public Visibility ShowAllCustFULog
                {
                        get { return showAllCustFULog; }
                        set
                        {
                                showAllCustFULog = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showCustFULog = Visibility.Collapsed;
                //显示客户跟进日志按钮
                public Visibility ShowCustFULog
                {
                        get { return showCustFULog; }
                        set
                        {
                                showCustFULog = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showRight = Visibility.Collapsed;
                //显示权限分配按钮
                public Visibility ShowRight
                {
                        get { return showRight; }
                        set
                        {
                                showRight = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showImport = Visibility.Collapsed;
                //显示导入按钮
                public Visibility ShowImport
                {
                        get { return showImport; }
                        set
                        {
                                showImport = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showPublish = Visibility.Collapsed;
                //显示发布按钮
                public Visibility ShowPublish
                {
                        get { return showPublish; }
                        set
                        {
                                showPublish = value;
                                OnPropertyChanged();
                        }
                }
                private Visibility showUnPublish = Visibility.Collapsed;
                //显示取消发布
                public Visibility ShowUnPublish
                {
                        get { return showUnPublish; }
                        set
                        {
                                showUnPublish = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 新增命令
                /// </summary>
                private RelayCommand addCommand;
                public RelayCommand AddCommand
                {
                        get { return addCommand; }
                        set
                        {
                                addCommand = value;
                                OnPropertyChanged();
                        }
                }

                //修改命令
                private RelayCommand editCommand;
                public RelayCommand EditCommand
                {
                        get { return editCommand; }
                        set
                        {
                                editCommand = value;
                                OnPropertyChanged();
                        }
                }
                //删除命令
                private RelayCommand deleteCommand;
                public RelayCommand DeleteCommand
                {
                        get { return deleteCommand; }
                        set
                        {
                                deleteCommand = value;
                                OnPropertyChanged();
                        }
                }
                //恢复命令
                private RelayCommand recoverCommand;
                public RelayCommand RecoverCommand
                {
                        get { return recoverCommand; }
                        set
                        {
                                recoverCommand = value;
                                OnPropertyChanged();
                        }
                }
                //移除命令
                private RelayCommand removeCommand;
                public RelayCommand RemoveCommand
                {
                        get { return removeCommand; }
                        set
                        {
                                removeCommand = value;
                                OnPropertyChanged();
                        }
                }
                //查询命令
                private RelayCommand findCommand;
                public RelayCommand FindCommand
                {
                        get { return findCommand; }
                        set
                        {
                                findCommand = value;
                                OnPropertyChanged();

                        }
                }
                //查看命令
                private RelayCommand infoCommand;
                public RelayCommand InfoCommand
                {
                        get { return infoCommand; }
                        set
                        {
                                infoCommand = value;
                                OnPropertyChanged();
                        }
                }
                //权限分配命令
                private RelayCommand rightCommand;
                public RelayCommand RightCommand
                {
                        get { return rightCommand; }
                        set
                        {
                                rightCommand = value;
                                OnPropertyChanged();
                        }
                }
                //所有客户需求命令
                private RelayCommand allRequestListCommand;
                public RelayCommand AllRequestListCommand
                {
                        get { return allRequestListCommand; }
                        set
                        {
                                allRequestListCommand = value;
                                OnPropertyChanged();
                        }
                }
                //客户意向需求命令
                private RelayCommand custRequestListCommand;
                public RelayCommand CustRequestListCommand
                {
                        get { return custRequestListCommand; }
                        set
                        {
                                custRequestListCommand = value;
                                OnPropertyChanged();
                        }
                }
                //所有客户日志命令
                private RelayCommand custAllFULogCommand;
                public RelayCommand CustAllFULogCommand
                {
                        get { return custAllFULogCommand; }
                        set
                        {
                                custAllFULogCommand = value;
                                OnPropertyChanged();
                        }
                }
                //客户跟进日志命令
                private RelayCommand custFULogCommand;
                public RelayCommand CustFULogCommand
                {
                        get { return custFULogCommand; }
                        set
                        {
                                custFULogCommand = value;
                                OnPropertyChanged();
                        }
                }

                //导入数据命令
                private RelayCommand importCommand;
                public RelayCommand ImportCommand
                {
                        get { return importCommand; }
                        set
                        {
                                importCommand = value;
                                OnPropertyChanged();
                        }
                }
                /// <summary>
                /// 发布命令
                /// </summary>
                private RelayCommand publishCommand;
                public RelayCommand PublishCommand
                {
                        get { return publishCommand; }
                        set
                        {
                                publishCommand = value;
                                OnPropertyChanged();
                        }
                }
                /// <summary>
                /// 取消发布
                /// </summary>
                private RelayCommand unPublishCommand;
                public RelayCommand UnPublishCommand
                {
                        get { return unPublishCommand; }
                        set
                        {
                                unPublishCommand = value;
                                OnPropertyChanged();
                        }
                }

                //关闭命令
                private RelayCommand closeCommand;
                public RelayCommand CloseCommand
                {
                        get { return closeCommand; }
                        set
                        {
                                closeCommand = value;
                                OnPropertyChanged();
                        }
                }

                /// <summary>
                /// 刷新命令的可执行状态
                /// </summary>
                public void OnCanExecuteChanged()
                {
                        AddCommand?.OnCanExecuteChanged();
                        EditCommand?.OnCanExecuteChanged();
                        DeleteCommand?.OnCanExecuteChanged();
                        RecoverCommand?.OnCanExecuteChanged();
                        RemoveCommand?.OnCanExecuteChanged();
                        InfoCommand?.OnCanExecuteChanged();
                        RightCommand?.OnCanExecuteChanged();
                        UnPublishCommand?.OnCanExecuteChanged();
                        PublishCommand?.OnCanExecuteChanged();
                        ImportCommand?.OnCanExecuteChanged();
                        CustAllFULogCommand?.OnCanExecuteChanged();
                        CustFULogCommand?.OnCanExecuteChanged();
                        CustRequestListCommand?.OnCanExecuteChanged();
                        AllRequestListCommand?.OnCanExecuteChanged();
                        FindCommand?.OnCanExecuteChanged();
                }
        }
}
