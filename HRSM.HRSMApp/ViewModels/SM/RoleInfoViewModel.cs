using HRSM.BLL;
using HRSM.Models.DModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HRSM.HRSMApp.ViewModels.SM
{
    public class RoleInfoViewModel : InfoViewModelBase
    {
        RoleInfo roleInfo = new RoleInfo();
        RoleBLL roleBLL = new RoleBLL();
        string oldName = "";
        public RoleInfoViewModel()
        {

        }

        public RoleInfoViewModel(int roleId, int actType)
        {
            this.RoleId = roleId;
            this.ActType = actType;//1  add  2  edit  3 addchild  4 info
            this.CloseWindowCmd = GetCloseWindowCmd("roleInfo");
            switch (ActType)
            {
                case 1:
                    this.ConfirmBtnContent = "新增";
                    break;
                case 2:
                    this.roleInfo = roleBLL.GetRoleInfo(this.RoleId);
                    this.oldName = this.roleInfo.RoleName;
                    this.ConfirmBtnContent = "修改";
                    this.IsConfirmBtnEnabled = true;
                    break;
                case 4:
                    this.roleInfo = roleBLL.GetRoleInfo(this.RoleId);
                    this.ConfirmBtnVisible = Visibility.Hidden;
                    break;
            }
        }

        public int RoleId
        {
            get { return roleInfo.RoleId; }
            set { roleInfo.RoleId = value; }
        }


        public string RoleName
        {
            get { return roleInfo.RoleName; }
            set
            {
                roleInfo.RoleName = value;
                OnPropertyChanged();
            }
        }

    
        public string Remark
        {
            get { return roleInfo.Remark; }
            set
            {
                roleInfo.Remark = value;
                OnPropertyChanged();
            }
        }


        public ICommand CheckInfoCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (string.IsNullOrEmpty(this.RoleName))
                    {
                        ShowError("角色名称不能为空！");
                        this.IsConfirmBtnEnabled = false;
                        return;
                    }
                    else if (this.RoleId == 0 || (this.RoleId > 0 && this.oldName != this.RoleName))
                    {
                        if (roleBLL.ExistsRole(this.RoleName))
                        {
                            ShowError("该角色名称已存在！");
                            this.IsConfirmBtnEnabled = false;
                            return;
                        }
                        else
                            this.IsConfirmBtnEnabled = true;
                    }

                });
            }
        }


        public ICommand ConfirmCmd
        {
            get
            {
                return new RelayCommand(o =>
                {
                    bool bl = false;//执行结果
                   if (this.ActType == 2)
                        bl = roleBLL.UpdateRoleInfo(roleInfo);
                    else
                        bl = roleBLL.AddRoleInfo(roleInfo);
                    string actName = ActType == 1 ? "新增" : "修改";
                    string msgTitle = $"{actName}角色信息";
                    string sucType = bl ? "成功" : "失败";
                    string msg = $"角色：{this.RoleName} {actName} {sucType}!";
                    if (bl)
                    {
                        ShowMessage(msg, msgTitle);
                        InvokeReload();
                    }
                    else
                    {
                        ShowError(msg, msgTitle);
                        return;
                    }
                });
            }
        }


    }
}
