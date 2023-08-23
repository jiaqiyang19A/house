using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
        public class UserListViewModel : ListViewModelBase
        {
                RoleBLL roleBLL = new RoleBLL();
                UserBLL userBLL = new UserBLL();
                public UserListViewModel()
                {
                        this.UserList = GetUserList();
                        InitToolbars();
                        this.FindUserListCmd = new RelayCommand(o =>
                          {
                                  ReloadUserList();
                          });
                        this.CheckAllCmd = GetCheckAllCmd(this.UserList);
                        this.CheckItemCmd = GetCheckItemCmd(this.UserList);
                }

                private string keywords;
 
                public string KeyWords
                {
                        get { return keywords; }
                        set
                        {
                                keywords = value;
                                OnPropertyChanged();
                        }
                }

                private bool isFrozen;
                public bool IsFrozen
                {
                        get { return isFrozen; }
                        set
                        {
                                isFrozen = value;
                                OnPropertyChanged();
                        }
                }

                private ObservableCollection<UserCheckInfo> userList;

                public ObservableCollection<UserCheckInfo> UserList
                {
                        get { return userList; }
                        set
                        {
                                userList = value;
                                OnPropertyChanged();
                        }
                }


                private ObservableCollection<UserCheckInfo> GetUserList()
                {
                        ObservableCollection<UserCheckInfo> uList = new ObservableCollection<UserCheckInfo>();
                        List<UserInfo> reList = userBLL.GetUserList(this.KeyWords, IsFrozen, IsShowDel);
                        reList.ForEach(u => uList.Add(new UserCheckInfo()
                        {
                                UserInfo = u,
                                IsCheck = false
                        }));
                        return uList;
                }

       
                private void InitToolbars()
                {
                        this.ToolBarInfo = new UC.ListToolBarViewModel()
                        {
                                AddCommand = new RelayCommand(o => ShowUserInfoPage(1, 0), b => !IsShowDel),
                                EditCommand = new RelayCommand(o => ShowUserInfoPage(2), b => IsEditable(this.UserList)),
                                DeleteCommand = new RelayCommand(o => DeleteUsers(1, false), b => IsEditable(this.UserList)),
                                RecoverCommand = new RelayCommand(o => DeleteUsers(0, false), b => IsDeletedable(this.UserList)),
                                RemoveCommand = new RelayCommand(o => DeleteUsers(2, false), b => IsDeletedable(this.UserList)),
                                CloseCommand = this.CloseTabPage,
                                InfoCommand = new RelayCommand(o => ShowUserInfoPage(4), b => this.UserList.Count > 0)
                        };
                }

                private void ShowUserInfoPage(int actType, int userId)
                {
                        UserInfoViewModel userVM = new UserInfoViewModel(actType, userId);
                        userVM.ReloadListEvent += ReloadUserList;
                        ShowDialog("userInfo", userVM);
                }

                private void ShowUserInfoPage(int actType)
                {
                        if (this.CurrentItem != null)
                        {
                                UserCheckInfo user = this.CurrentItem as UserCheckInfo;
                                ShowUserInfoPage(actType, user.UserInfo.UserId);
                        }
                }

                private void ReloadUserList()
                {
                        this.UserList = GetUserList();
                        this.CheckAllCmd = GetCheckAllCmd(this.UserList);
                        //刷新工具栏
                        this.ToolBarInfo.OnCanExecuteChanged();
                }

                private void SingleDelUser(object oId, int isDeleted)
                {
                        if (oId != null)
                        {
                                int userId = (int)oId;
                                UserCheckInfo info = this.UserList.Where(r => r.UserInfo.UserId == userId).FirstOrDefault();
                                if (this.SelectedItem == null)
                                        this.SelectedItem = new UserCheckInfo();
                                this.SelectedItem = info;
                                DeleteUsers(isDeleted, true);
                        }
                }

                private void DeleteUsers(int isDeleted, bool isSingle)
                {
                        string delType = GetDelTypeName(isDeleted);
                        string msgTile = $"用户{delType}";
                        List<int> delIds = new List<int>();

                        if (isSingle && this.SelectedItem != null)
                        {
                                UserCheckInfo selInfo = this.SelectedItem as UserCheckInfo;
                                delIds.Add(selInfo.UserInfo.UserId);
                        }
                        else
                        {
                                delIds = this.UserList.Where(r => r.IsCheck == true).Select(r => r.UserInfo.UserId).ToList();
                        }
                        if (delIds.Count > 0)
                        {
                                if (isDeleted == 1)
                                {
                                        foreach (int userId in delIds)
                                        {
                                               //获取用户角色编号集合
                                                List<int> roleIds = userBLL.GetUserRoleIds(userId);
                                                if (roleBLL.IsRolesAdmin(roleIds))
                                                {
                                                        ShowError("不能删除管理员账号", msgTile);
                                                        return;
                                                }
                                        }
                                }
                                if (ShowQuestion($"您确定要{delType}选择的用户信息吗？", msgTile) == MsgBoxWindow.CustomMessageBoxResult.OK)
                                {
                                        bool isDel = false;
                                        switch (isDeleted)
                                        {
                                                case 1:
                                                        isDel = userBLL.LogicDelUserList(delIds);
                                                        break;
                                                case 0:
                                                        isDel = userBLL.RecoverUserList(delIds);
                                                        break;
                                                case 2:
                                                        isDel = userBLL.RemoveUserList(delIds);
                                                        break;
                                        }
                                        string sucMsg = isDel ? "成功" : "失败";
                                        string msg = $"选择的用户信息{delType} {sucMsg}!";
                                        if (isDel)
                                        {
                                                ShowMessage(msg, msgTile);
                                                ReloadUserList();
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

                #region 命令

                public ICommand EditUserCmd
                {
                        get
                        {
                                return new RelayCommand(o =>
                                {
                                        ShowUserInfoPage(2);
                                });
                        }
                }


                public ICommand DelUserCmd
                {
                        get
                        {
                                return new RelayCommand(oId =>
                                {
                                        SingleDelUser(oId, 1);
                                });
                        }
                }

                public ICommand RecoverUserCmd
                {
                        get
                        {
                                return new RelayCommand(oId =>
                                {
                                        SingleDelUser(oId, 0);
                                });
                        }
                }

                public ICommand RemoveUserCmd
                {
                        get
                        {
                                return new RelayCommand(oId =>
                                {
                                        SingleDelUser(oId, 2);
                                });
                        }
                }

                private ICommand findUserListCmd;
                public ICommand FindUserListCmd
                {
                        get { return findUserListCmd; }
                        set
                        {
                                findUserListCmd = value;
                                OnPropertyChanged();
                        }
                }
                #endregion
        }
}
