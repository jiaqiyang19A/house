using HRSM.DAL.Base;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
    public class ViewUserRoleDAL:BQuery<ViewUserRoleInfo>
    {

       public List<ViewUserRoleInfo>  GetUserRoleList(int userId)
        {
            string cols = "UserId,UserName,RoleId,IsAdmin,UserState,UserFName,UserPhone,UserPwd";
            return GetModelList( $"UserId={userId} and UserDeleted=0", cols,"");
        }


       public List<int> GetUserRoleIds(int userId)
        {
           List<ViewUserRoleInfo> urList=  GetModelList($"UserId={userId} and UserDeleted=0", "RoleId", "");
            if (urList != null && urList.Count > 0)
                return urList.Select(ur => ur.RoleId).ToList();
            else
                return null;
        }
    }
}
