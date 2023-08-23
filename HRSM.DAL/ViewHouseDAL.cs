using Common;
using HRSM.DAL.Base;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
    public class ViewHouseDAL:BQuery<ViewHouseInfo>
    {

         public List<ViewHouseInfo> GetShowHouseList(string houseName,string rsType,string direction,string layout)
        {
            string cols = "HouseId,HouseName,RentSale,HouseDirection,HouseLayout,OwnerName,OwnerId,HousePic,HouseState";
            string strWhere = "IsDeleted=0 and IsPublish=1";
            List<SqlParameter> listParas = new List<SqlParameter>();
            if(!string.IsNullOrEmpty(houseName))
            {
                strWhere += " and HouseName like @houseName";
                listParas.Add(new SqlParameter("@houseName", $"%{houseName}%"));
            }
            if (!string.IsNullOrEmpty(rsType))
            {
                strWhere += " and RentSale =  @rsType";
                listParas.Add(new SqlParameter("@rsType", rsType));
            }
            if (!string.IsNullOrEmpty(direction))
            {
                strWhere += " and HouseDirection =  @direction";
                listParas.Add(new SqlParameter("@direction", direction));
            }
            if (!string.IsNullOrEmpty(layout))
            {
                strWhere += " and HouseLayout like @layout";
                listParas.Add(new SqlParameter("@layout", $"%{layout}%"));
            }
            return GetModelList(strWhere, cols, "", listParas.ToArray());
        }


        public ViewHouseInfo GetHouseInfo(int houseId)
        {
            string cols = PropertyHelper.GetColNames<ViewHouseInfo>("IsDeleted");
            return GetModel($"HouseId={houseId}", cols);
        }


                public List<ViewHouseInfo> GetHouseList(string keywords, string rentSaleName, string houseDirection, string houseLayout, string houseState, int isPublish, int isDeleted)
                {
                        string cols = "HouseId,HouseName,Building,HouseAddress,RentSale,HouseDirection,HouseLayout,OwnerId,OwnerName,HouseState,IsPublish";
                        string strWhere = $"IsDeleted={isDeleted}";
                        List<SqlParameter> listParas = new List<SqlParameter>();
                        if (!string.IsNullOrEmpty(keywords))
                        {
                                strWhere += " and (HouseName like @keywords or Building like @keywords or HouseAddress like @keywords or OwnerName like @keywords)";
                                listParas.Add(new SqlParameter("@keywords", $"%{keywords}%"));
                        }
                        if (!string.IsNullOrEmpty(rentSaleName))
                        {
                                strWhere += " and RentSale=@rentSale";
                                listParas.Add(new SqlParameter("@rentSale", rentSaleName));
                        }
                        if (!string.IsNullOrEmpty(houseDirection))
                        {
                                strWhere += " and HouseDirection=@houseDirection";
                                listParas.Add(new SqlParameter("@houseDirection", houseDirection));
                        }
                        if (!string.IsNullOrEmpty(houseLayout))
                        {
                                strWhere += " and HouseLayout=@houseLayout";
                                listParas.Add(new SqlParameter("@houseLayout", houseLayout));
                        }
                        if (!string.IsNullOrEmpty(houseState))
                        {
                                strWhere += " and HouseState=@houseState";
                                listParas.Add(new SqlParameter("@houseState", houseState));
                        }
                        if (isPublish != -1)//-1  1  0
                                strWhere += $" and IsPublish={isPublish}";
                        return GetModelList(strWhere, cols, "",listParas.ToArray());
                }
        }
}
