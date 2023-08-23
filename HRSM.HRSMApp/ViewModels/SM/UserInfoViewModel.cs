using HRSM.BLL;
using HRSM.Models.DModels;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class UserInfoViewModel:InfoViewModelBase
    {
        UserBLL userBLL = new UserBLL();
        RoleBLL roleBLL = new RoleBLL();
        private UserInfo userInfo = new UserInfo();
        private UserRoleIdsInfo urInfo = new UserRoleIdsInfo();
        public UserInfoViewModel()
        {

        }
        public  UserInfoViewModel(int actType,int userId)
        {
            this.ActType = actType;
            this.UserId = userId;
          this.CloseWindowCmd = GetCloseWindowCmd("userInfo");
            this.RoleList = GetRoleList();
            this.IsConfirmBtnEnabled = true;
            switch (this.ActType)
            {
                case 1:
                    this.ConfirmBtnContent = "添加";
                    break;
                case 2:
                    LoadUserRoleList();//加载用户已设置的角色信息
                    this.ConfirmBtnContent = "修改";
                    break;
                case 4:
                    LoadUserRoleList();
                    this.ConfirmBtnVisible = Visibility.Hidden;
                    break;
            }
        }

      

        public int UserId { get; set; }


        public string UserName
        {
            get { return userInfo.UserName; }
            set { userInfo.UserName = value;
                OnPropertyChanged();
            }
        }

        public string UserPwd
        {
            get { return userInfo.UserPwd; }
            set
            {
                userInfo.UserPwd = value;
                OnPropertyChanged();
            }
        }


        public string UserFName
        {
            get { return userInfo.UserFName; }
            set
            {
                userInfo.UserFName = value;
                OnPropertyChanged();
            }
        }


        public string UserPhone
        {
            get { return userInfo.UserPhone; }
            set
            {
                userInfo.UserPhone = value;
                OnPropertyChanged();
            }
        }


        public List<int> RoleIds
        {
            get
            {
                if (urInfo.RoleIds != null)
                    return urInfo.RoleIds;
                else
                    return new List<int>();
            }
            set
            {
                urInfo.RoleIds = value;
            }
        }

        public bool IsNormal
        {
            get { return userInfo.UserState; }
            set
            {
                userInfo.UserState = value;
                OnPropertyChanged();
            }
        }

      
        public bool IsFrozen
        {
            get { return userInfo.UserState ? false : true; }
        }


        private ObservableCollection<RoleCheckInfo> roleList=new ObservableCollection<RoleCheckInfo>();
        public ObservableCollection<RoleCheckInfo> RoleList
        {
            get { return roleList; }
            set
            {
                roleList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RoleCheckInfo> GetRoleList()
        {
            ObservableCollection<RoleCheckInfo> list = new ObservableCollection<RoleCheckInfo>();
            List<RoleInfo> roles = roleBLL.GetAllRoles();
            roles.ForEach(r => list.Add(new RoleCheckInfo()
            {
                IsCheck = false,
                RoleInfo = r
            }));
            return list;
        }
        private void CheckRoleList(bool bl)
        {
            foreach(var role in RoleList)
            {
                role.IsCheck = bl;
            }
        }

        private void LoadUserRoleList()
        {
            this.urInfo = userBLL.GetUserRoleIdsInfo(this.UserId);
            this.userInfo = new UserInfo()
            {
                UserId = this.UserId,
                UserName = urInfo.UserName,
                UserPwd = urInfo.UserPwd,
                UserFName = urInfo.UserFName,
                UserPhone = urInfo.UserPhone,
                UserState = urInfo.UserState
            };
      
            if(this.RoleIds!=null&&this.RoleIds.Count>0)
            {
                CheckRoleList(false);
                foreach (var role in RoleList)
                {
                    if(this.RoleIds.Contains(role.RoleInfo.RoleId))
                    {
                        role.IsCheck = true;
                    }
                }
            }
        }

        #region 命令
        public ICommand ConfirmCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    string actName = ActType == 1 ? "添加" : "修改";
                    string msgTitle = $"用户{actName}";
                      if(string.IsNullOrEmpty(this.UserName))
                    {
                        ShowError("请输入用户名！",msgTitle);
                        return;
                    }
                      else if(string.IsNullOrEmpty(this.UserPwd)&&ActType==1)
                    {
                        ShowError("请输入密码！", msgTitle);
                        return;
                    }
                    else if(this.UserId==0||(this.UserId>0&&this.UserName!=urInfo.UserName))
                    {
                        if(userBLL.ExistsUser(UserName))
                        {
                            ShowError("该用户已存在！");
                            return;
                        }
                    }
                    bool bl = false;
                    List<UserRoleInfo> roleNew = new List<UserRoleInfo>();
                    roleNew = RoleList.Where(r => r.IsCheck == true).Select(r => new UserRoleInfo()
                    {
                        RoleId = r.RoleInfo.RoleId,
                        UserId = this.UserId
                    }).ToList();
                    if (ActType == 2 && this.RoleIds != null)
                    {
                        List<UserRoleInfo> rolesAdd = roleNew.Where(r => !this.RoleIds.Contains(r.RoleId)).ToList();
                        bl = userBLL.UpdateUserInfo(this.userInfo, roleNew, rolesAdd);
                    }
                    else
                        bl = userBLL.AddUserInfo(this.userInfo, roleNew);
                    string sucType = bl ? "成功" : "失败";
                    string msgInfo = $"用户：{this.UserName} {actName}{sucType}!";
                    if (bl)
                    {
                        ShowMessage(msgInfo, msgTitle);
                        InvokeReload();
                    }
                    else
                    {
                        ShowError(msgInfo, msgTitle);
                        return;
                    }
                });
            }
        }


        
        #endregion


    }
}
