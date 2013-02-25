using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;

namespace SlvHanbai.Web.Class.Data
{
    public class DataPlanDay
    {
        private const string CLASS_NM = "DataPlanDay";

        /// <summary>
        /// 回収(支払)予定日取得
        /// </summary>
        public static void GetPlanDay(int collect_cycle_id,
                                             int collect_day, 
                                             string yyyymmdd,
                                             ref string ret_collect_plan_day,
                                             ref int ret_collect_day)
        {
            int _collect_day = 0;
            string ret = "";
            try
            {
                if (collect_day == 0)
                {
                    ret_collect_day = ExCast.zCInt(yyyymmdd.Substring(8, 2));
                }
                else
                {
                    ret_collect_day = collect_day;
                }

                string _yyyymm = "";
                switch (collect_cycle_id)
                {
                    case 1:         // 当月
                        _yyyymm = DateTime.Now.ToString("yyyy/MM");
                        break;
                    case 2:         // 翌月
                        _yyyymm = DateTime.Now.AddMonths(1).ToString("yyyy/MM");
                        break;
                    case 3:         // 翌々月
                        _yyyymm = DateTime.Now.AddMonths(2).ToString("yyyy/MM");
                        break;
                    case 4:         // 3ヶ月後
                        _yyyymm = DateTime.Now.AddMonths(3).ToString("yyyy/MM");
                        break;
                    case 5:         // 4ヶ月後
                        _yyyymm = DateTime.Now.AddMonths(4).ToString("yyyy/MM");
                        break;
                    case 6:         // 5ヶ月後
                        _yyyymm = DateTime.Now.AddMonths(5).ToString("yyyy/MM");
                        break;
                    case 7:         // 6ヶ月後
                        _yyyymm = DateTime.Now.AddMonths(6).ToString("yyyy/MM");
                        break;
                    default:        // 翌月
                        _yyyymm = DateTime.Now.AddMonths(1).ToString("yyyy/MM");
                        break;
                }

                ret_collect_plan_day = _yyyymm + "/" + ret_collect_day.ToString("00");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
   }
}