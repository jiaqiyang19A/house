using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
        public class RoleListViewModel : ListViewModelBase
        {
                RoleBLL roleBLL = new RoleBLL();
                public RoleListViewModel()
                {
                        this.RoleList = GetRoleList();
                        InitToolBarInfo();
                        FindRoleListCmd = new RelayCommand(o =>
                        {
                                ReloadRoleList();
                        });
                        this.CheckAllCmd = GetCheckAllCmd(this.RoleList);
                        this.CheckItemCmd = GetCheckItemCmd(this.RoleList);
                }


                private ICommand findRoleListCmd;
                public ICommand FindRoleListCmd
                {
                        get { return findRoleListCmd; }
                        set
                        {
                                findRoleListCmd = value;
                                OnPropertyChanged();
                        }
                }

                public ICommand EditRoleCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        ShowRoleInfoPage(2);
                                });
                        }
                }


                public ICommand DelRoleCmd
                {
                        get
                        {
                                return new RelayCommand(oId =>
                                {
                                        SingleDelete(oId, 1);
                                });
                        }
                }

                public ICommand RecoverRoleCmd
                {
                        get
                        {
                                return new RelayCommand(oId =>
                                {
                                        SingleDelete(oId, 0);
                                });
                        }
                }

                public ICommand RemoveRoleCmd
                {
                        get
                        {
                                return new RelayCommand(oId =>
                                {
                                        SingleDelete(oId, 2);
                                });
                        }
                }


                private ObservableCollection<RoleCheckInfo> roleList = new ObservableCollection<RoleCheckInfo>();
                public ObservableCollection<RoleCheckInfo> RoleList
                {
                        get { return roleList; }
                        set
                        {
                                roleList = value;
                                OnPropertyChanged();
                        }
                }

          
                private void InitToolBarInfo()
                {
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                ShowRight = Visibility.Visible,
                                AddCommand = new RelayCommand(o => ShowRoleInfoPage(0, 1), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowRoleInfoPage(2), b => IsEditable(this.RoleList)),
                                DeleteCommand = new RelayCommand(o => DeleteRoles(1, false), b => IsEditable(this.RoleList)),
                                RecoverCommand = new RelayCommand(o => DeleteRoles(0, false), b => IsDeletedable(this.RoleList)),
                                RemoveCommand = new RelayCommand(o => DeleteRoles(2, false), b => IsDeletedable(this.RoleList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowRoleInfoPage(4), b => this.RoleList.Count > 0),
                                RightCommand = new RelayCommand(o => ShowRightPage(o), b => IsEditable(this.RoleList))
                        };
                }

                private ObservableCollection<RoleCheckInfo> GetRoleList()
                {
                        ObservableCollection<RoleCheckInfo> list = new ObservableCollection<RoleCheckInfo>();
                        List<RoleInfo> rList = roleBLL.GetRoleList(this.IsShowDel);
                        rList.ForEach(r => list.Add(new RoleCheckInfo()
                        {
                                IsCheck = false,
                                RoleInfo = r
                        }));
                        return list;
                }

                private void ReloadRoleList()
                {
                        this.RoleList = GetRoleList();
                        this.CheckAllCmd = GetCheckAllCmd(this.RoleList);
                        ToolBarInfo.OnCanExecuteChanged();
                }

  
                public void ShowRoleInfoPage(int actType)
                {
                        if (this.CurrentItem != null)
                        {
                                RoleCheckInfo roleInfo = this.CurrentItem as RoleCheckInfo;
                                ShowRoleInfoPage(roleInfo.RoleInfo.RoleId, actType);
                        }
                }

                private void ShowRoleInfoPage(int roleId, int actType)
                {
                        RoleInfoViewModel roleVM = new RoleInfoViewModel(roleId, actType);
                        roleVM.ReloadListEvent += ReloadRoleList;
                        ShowDialog("roleInfo", roleVM);
                }
     
                private void ShowRightPage(object uc)
                {
                        int roleId = 0;
                        if (this.CurrentItem != null)
                        {
                                RoleCheckInfo roleInfo = this.CurrentItem as RoleCheckInfo;
                                roleId = roleInfo.RoleInfo.RoleId;
                        }
                        RightSetViewModel rightVM = new RightSetViewModel(roleId);
                        AddUCTabItem((UserControl)uc, "权限设置", "rightSet", rightVM);
                }


                private void SingleDelete(object oId, int isDeleted)
                {
                        if (oId != null)
                        {
                                int roleId = (int)oId;
                                RoleCheckInfo roleInfo = this.RoleList.Where(r => r.RoleInfo.RoleId == roleId).FirstOrDefault();
                                if (this.SelectedItem == null)
                                        this.SelectedItem = new RoleCheckInfo();
                                this.SelectedItem = roleInfo;
                                DeleteRoles(isDeleted, true);
                        }
                }
                private void DeleteRoles(int isDeleted, bool isSingle)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"角色{delType}";
                        List<int> delIds = new List<int>();

                        if (isSingle && this.SelectedItem != null)
                        {
                                RoleCheckInfo selRole = this.SelectedItem as RoleCheckInfo;
                                delIds.Add(selRole.RoleInfo.RoleId);
                        }

                        else
                        {
                                delIds = this.RoleList.Where(r => r.IsCheck == true).Select(r => r.RoleInfo.RoleId).ToList();
                        }
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        if (roleBLL.IsRolesAdmin(delIds))
                                        {
                                                ShowError("不能删除管理员角色", msgTile);
                                                return;
                                        }

                                }
                                if (ShowQuestion($"您确定要{delType}选择的角色信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = roleBLL.LogicDelRoles(delIds);
                                                        break;
                                                case 0:
                                                        isDel = roleBLL.RecoverRoles(delIds);
                                                        break;
                                                case 2:
                                                        isDel = roleBLL.RemoveRoles(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的角色信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                                //刷新列表数据
                                                ReloadRoleList();
                                        }
                                        else
                                        {
                                                ShowError(msg, msgTile);
                                                return;
                                        }
                                }
                        }
                        else
                        {
                                ShowError($"没有要{delType}的信息！", msgTile);
                                return;
                        }
                }


        }
}
