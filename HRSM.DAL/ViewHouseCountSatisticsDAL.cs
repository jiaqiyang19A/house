using HRSM.DAL.Base;
using HRSM.Models.VModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSM.DAL
{
        public class ViewHouseCountSatisticsDAL:BQuery<ViewHouseCountSatisticsInfo>
        {

                public ViewHouseCountSatisticsInfo GetHouseCountStatistics()
                {
                        return GetModel("", "");
                }
        }
}
