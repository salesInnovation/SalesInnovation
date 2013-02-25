using System;
using System.Collections.Generic;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Activation;
using System.Web;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Entity;

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcMstData
    {
        private const string CLASS_NM = "svcMstData";

        #region データ取得

        // 納入先マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetSupplier(string random, string CustomerID, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]); 

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSupplier(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                sb = new StringBuilder();

                #region SQL

                sb.Length = 0;
                sb.Append("SELECT SR.* " + Environment.NewLine);
                sb.Append("  FROM M_SUPPLIER AS SR" + Environment.NewLine);
                sb.Append(" WHERE SR.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SR.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SR.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND SR.CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerID)) + Environment.NewLine);
                sb.Append("   AND SR.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                sb.Append(" ORDER BY SR.CUSTOMER_ID ");
                sb.Append("         ,SR.ID ");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                ExCast.zCStr(dt.DefaultView[0]["CUSTOMER_ID"]),
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"]));
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSupplier", ex);
            }
            finally
            {
                db = null;
            }
            return objList;
        }

        // 得意先マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetCustomer(string random, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int idFigureCustomer = 0;

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                idFigureCustomer = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCustomer(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                string _id = ID;
                if (ExCast.IsNumeric(_id))
                {
                    _id = ExCast.zCDbl(_id).ToString();
                }

                sb.Append("SELECT  CM.COMPANY_ID" + Environment.NewLine);
                sb.Append("      , CM.ID" + Environment.NewLine);
                sb.Append("      , CM.NAME" + Environment.NewLine);
                sb.Append("      , CM.KANA" + Environment.NewLine);
                sb.Append("      , CM.ABOUT_NAME" + Environment.NewLine);
                sb.Append("      , CM.ZIP_CODE" + Environment.NewLine);
                sb.Append("      , CM.PREFECTURE_ID" + Environment.NewLine);
                sb.Append("      , CM.CITY_ID" + Environment.NewLine);
                sb.Append("      , CM.TOWN_ID" + Environment.NewLine);
                sb.Append("      , CM.ADRESS_CITY" + Environment.NewLine);
                sb.Append("      , CM.ADRESS_TOWN" + Environment.NewLine);
                sb.Append("      , CM.ADRESS1" + Environment.NewLine);
                sb.Append("      , CM.ADRESS2" + Environment.NewLine);
                sb.Append("      , CM.STATION_NAME" + Environment.NewLine);
                sb.Append("      , CM.POST_NAME" + Environment.NewLine);
                sb.Append("      , CM.PERSON_NAME" + Environment.NewLine);
                sb.Append("      , CM.TITLE_ID" + Environment.NewLine);
                sb.Append("      , CM.TITLE_NAME" + Environment.NewLine);
                sb.Append("      , CM.TEL" + Environment.NewLine);
                sb.Append("      , CM.FAX" + Environment.NewLine);
                sb.Append("      , CM.MAIL_ADRESS" + Environment.NewLine);
                sb.Append("      , CM.MOBILE_TEL" + Environment.NewLine);
                sb.Append("      , CM.MOBILE_ADRESS" + Environment.NewLine);
                sb.Append("      , CM.URL" + Environment.NewLine);
                sb.Append("      , CM.INVOICE_ID" + Environment.NewLine);
                sb.Append("      , SK.NAME AS INVOICE_NAME" + Environment.NewLine);
                sb.Append("      , CM.BUSINESS_DIVISION_ID" + Environment.NewLine);
                sb.Append("      , CM.UNIT_KIND_ID" + Environment.NewLine);
                sb.Append("      , CM.CREDIT_RATE" + Environment.NewLine);
                sb.Append("      , CM.TAX_CHANGE_ID" + Environment.NewLine);
                sb.Append("      , CM.SUMMING_UP_GROUP_ID" + Environment.NewLine);
                sb.Append("      , CM.PRICE_FRACTION_PROC_ID" + Environment.NewLine);
                sb.Append("      , CM.TAX_FRACTION_PROC_ID" + Environment.NewLine);
                sb.Append("      , CM.CREDIT_LIMIT_PRICE" + Environment.NewLine);
                sb.Append("      , CM.SALES_CREDIT_PRICE" + Environment.NewLine);
                sb.Append("      , CM.RECEIPT_DIVISION_ID" + Environment.NewLine);
                sb.Append("      , CM.COLLECT_CYCLE_ID" + Environment.NewLine);
                sb.Append("      , CM.COLLECT_DAY" + Environment.NewLine);
                sb.Append("      , CM.BILL_SITE" + Environment.NewLine);
                sb.Append("      , CM.GROUP1_ID" + Environment.NewLine);
                sb.Append("      , CM.GROUP2_ID" + Environment.NewLine);
                sb.Append("      , CM.GROUP3_ID" + Environment.NewLine);
                sb.Append("      , CM.MEMO" + Environment.NewLine);
                sb.Append("      , SK.CREDIT_LIMIT_PRICE AS INVOICE_CREDIT_LIMIT_PRICE" + Environment.NewLine);
                sb.Append("      , SK.SALES_CREDIT_PRICE AS INVOICE_SALES_CREDIT_PRICE" + Environment.NewLine);
                sb.Append("  FROM M_CUSTOMER AS CM" + Environment.NewLine);

                #region Join

                // 請求先
                sb.Append("  LEFT JOIN M_CUSTOMER AS SK" + Environment.NewLine);
                sb.Append("    ON SK.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SK.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SK.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND SK.ID = CM.INVOICE_ID" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE CM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CM.COMPANY_ID = " + ExCast.zNumZeroNothingFormat(companyId) + Environment.NewLine);
                sb.Append("   AND CM.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_id)) + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                    objList.ATTRIBUTE1 = ExCast.zCStr(dt.DefaultView[0]["BUSINESS_DIVISION_ID"]);
                    objList.ATTRIBUTE2 = ExCast.zCStr(dt.DefaultView[0]["TAX_CHANGE_ID"]);
                    objList.ATTRIBUTE3 = ExCast.zCStr(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    objList.ATTRIBUTE4 = ExCast.zCStr(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);
                    objList.ATTRIBUTE5 = ExCast.zCStr(dt.DefaultView[0]["UNIT_KIND_ID"]);
                    objList.ATTRIBUTE6 = ExCast.zCStr(dt.DefaultView[0]["CREDIT_LIMIT_PRICE"]);
                    objList.ATTRIBUTE7 = ExCast.zCStr(dt.DefaultView[0]["SALES_CREDIT_PRICE"]);
                    objList.ATTRIBUTE8 = ExCast.zFormatForID(dt.DefaultView[0]["INVOICE_ID"], idFigureCustomer);
                    objList.ATTRIBUTE9 = ExCast.zCStr(dt.DefaultView[0]["INVOICE_NAME"]);

                    objList.ATTRIBUTE10 = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_ID"]);
                    objList.ATTRIBUTE11 = ExCast.zCStr(dt.DefaultView[0]["RECEIPT_DIVISION_ID"]);
                    objList.ATTRIBUTE12 = ExCast.zCStr(dt.DefaultView[0]["COLLECT_CYCLE_ID"]);
                    objList.ATTRIBUTE13 = ExCast.zCStr(dt.DefaultView[0]["COLLECT_DAY"]);
                    objList.ATTRIBUTE14 = ExCast.zCStr(dt.DefaultView[0]["BILL_SITE"]);
                    objList.ATTRIBUTE15 = ExCast.zCStr(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    objList.ATTRIBUTE16 = ExCast.zCStr(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);

                    objList.ATTRIBUTE17 = ExCast.zCStr(dt.DefaultView[0]["INVOICE_CREDIT_LIMIT_PRICE"]);
                    objList.ATTRIBUTE18 = ExCast.zCStr(dt.DefaultView[0]["INVOICE_SALES_CREDIT_PRICE"]);

                    objList.ATTRIBUTE19 = ExCast.zCStr(dt.DefaultView[0]["CREDIT_RATE"]);

                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCustomer", ex);
            }
            return objList;
        }

        // 担当マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetPerson(string random, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                
                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPerson(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT PS.* " + Environment.NewLine);
                sb.Append("  FROM M_PERSON AS PS" + Environment.NewLine);
                sb.Append(" WHERE PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND PS.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPerson", ex);
            }
            return objList;
        }

        // 商品マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetCommodity(string random, string ID, string dataGirdSelectedIndex)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]); 

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCommodity(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT GS.* " + Environment.NewLine);
                sb.Append("  FROM M_COMMODITY AS GS" + Environment.NewLine);
                sb.Append(" WHERE GS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND GS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND GS.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND GS.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                    objList.ATTRIBUTE1 = ExCast.zCStr(dt.DefaultView[0]["UNIT_ID"]);                                // 単位ID
                    objList.ATTRIBUTE2 = ExCast.zCStr(dt.DefaultView[0]["ENTER_NUMBER"]);                           // 入数
                    objList.ATTRIBUTE3 = ExCast.zCStr(dt.DefaultView[0]["NUMBER_DECIMAL_DIGIT"]);                   // 数量小数桁
                    objList.ATTRIBUTE4 = ExCast.zCStr(dt.DefaultView[0]["UNIT_DECIMAL_DIGIT"]);                     // 単価小数桁
                    objList.ATTRIBUTE5 = ExCast.zCStr(dt.DefaultView[0]["TAXATION_DIVISION_ID"]);                   // 課税区分ID
                    objList.ATTRIBUTE6 = ExCast.zCStr(dt.DefaultView[0]["INVENTORY_MANAGEMENT_DIVISION_ID"]);       // 在庫管理区分ID
                    objList.ATTRIBUTE7 = ExCast.zCStr(dt.DefaultView[0]["JUST_INVENTORY_NUMBER"]);                  // 適正在庫数
                    objList.ATTRIBUTE8 = ExCast.zCStr(dt.DefaultView[0]["INVENTORY_NUMBER"]);                       // 現在庫数
                    objList.ATTRIBUTE9 = ExCast.zCStr(dt.DefaultView[0]["RETAIL_PRICE_SKIP_TAX"]);                  // 上代税抜
                    objList.ATTRIBUTE10 = ExCast.zCStr(dt.DefaultView[0]["RETAIL_PRICE_BEFORE_TAX"]);               // 上代税込
                    objList.ATTRIBUTE11 = ExCast.zCStr(dt.DefaultView[0]["SALES_UNIT_PRICE_SKIP_TAX"]);             // 売上単価税抜
                    objList.ATTRIBUTE12 = ExCast.zCStr(dt.DefaultView[0]["SALES_UNIT_PRICE_BEFORE_TAX"]);           // 売上単価税込
                    objList.ATTRIBUTE13 = ExCast.zCStr(dt.DefaultView[0]["SALES_COST_PRICE_SKIP_TAX"]);             // 売上原価税抜
                    objList.ATTRIBUTE14 = ExCast.zCStr(dt.DefaultView[0]["SALES_COST_PRICE_BEFORE_TAX"]);           // 売上原価税込
                    objList.ATTRIBUTE15 = ExCast.zCStr(dt.DefaultView[0]["PURCHASE_UNIT_PRICE_SKIP_TAX"]);          // 仕入単価税抜
                    objList.ATTRIBUTE16 = ExCast.zCStr(dt.DefaultView[0]["PURCHASE_UNIT_PRICE_BEFORE_TAX"]);        // 仕入単価税込
                    objList.ATTRIBUTE20 = dataGirdSelectedIndex;
                }
                else
                {
                    objList = new EntityMstData();
                    objList.ATTRIBUTE20 = dataGirdSelectedIndex;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCommodity", ex);
            }
            return objList;
        }

        // 会社グループマスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetCompanyGroup(string random, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompanyGroup(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT CG.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
                sb.Append(" WHERE CG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CG.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CG.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND CG.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);

                #endregion

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                }
                else
                {
                    //if (ID == "0" || ID == "00" || ID == "000")
                    //{
                    //    objList = new EntityMstData("000",
                    //                                "000",
                    //                                "全グループ"
                    //                                );

                    //}
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompanyGroup", ex);
            }
            return objList;
        }

        // 郵便番号マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetZip(string random, string zipFrom, string zipTo)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetZip(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT ZP.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_ZIP AS ZP" + Environment.NewLine);
                sb.Append(" WHERE ZP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND ZP.DISPLAY_FLG = 1 " + Environment.NewLine);
                if (zipFrom != "" && zipTo != "")
                {
                    sb.Append("   AND ZP.ZIP = " + ExEscape.zRepStr(ExCast.zCInt(zipFrom).ToString() + zipTo) + Environment.NewLine);
                }
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    string zip = string.Format("{0:0000000}", ExCast.zCInt(dt.DefaultView[0]["ZIP"]));
                    string zip1 = zip.Substring(0, 3);
                    string zip2 = zip.Substring(3, 4);

                    objList = new EntityMstData(zip,
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["PREDECTURE_NAME"]) +
                                                ExCast.zCStr(dt.DefaultView[0]["CITY_NAME"]) +
                                                ExCast.zCStr(dt.DefaultView[0]["TOWN_NAME"])
                                                );
                    objList.ATTRIBUTE1 = zip1;                                                      // 郵便番号前3桁
                    objList.ATTRIBUTE2 = zip2;                                                      // 郵便番号後4桁
                    objList.ATTRIBUTE3 = ExCast.zCStr(dt.DefaultView[0]["PREDECTURE_NAME"]);        // 都道府県名
                    objList.ATTRIBUTE4 = ExCast.zCStr(dt.DefaultView[0]["CITY_NAME"]);              // 市区郡名
                    objList.ATTRIBUTE5 = ExCast.zCStr(dt.DefaultView[0]["TOWN_NAME"]);              // 町村名

                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetZip", ex);
            }
            return objList;
        }

        // 条件（支払・回収）マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetCondition(string random, int kbn, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCondition(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                entity.ATTRIBUTE1 = kbn.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                string _id = string.Format("{0:00}", ExCast.zCInt(ID));

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM M_CONDITION AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.CONDITION_DIVISION_ID = " + kbn.ToString() + Environment.NewLine);
                sb.Append("   AND MT.ID = " + ExEscape.zRepStr(_id) + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    string _id2 = string.Format("{0:00}", ExCast.zCInt(dt.DefaultView[0]["ID"]));

                    objList = new EntityMstData(_id2,
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                    objList.ATTRIBUTE1 = kbn.ToString();
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCondition", ex);
            }
            return objList;
        }

        // 入金区分マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetReceitpDivision(string random, string ID, string dataGirdSelectedIndex)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceitpDivision(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_RECEIPT_DIVISION AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["DESCRIPTION"])
                                                );
                    objList.ATTRIBUTE20 = dataGirdSelectedIndex;
                }
                else
                {
                    objList = new EntityMstData();
                    objList.ATTRIBUTE20 = dataGirdSelectedIndex;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceitpDivision", ex);
            }
            return objList;
        }

        // 分類マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetGroup(string random, int kbn, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetGroup(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM M_CLASS AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.CLASS_DIVISION_ID = " + kbn.ToString() + Environment.NewLine);
                sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                sb.Append(" LIMIT 0, 1");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                "",
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetGroup", ex);
            }
            return objList;
        }

        // 仕入先マスタ取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityMstData GetPurchase(string random, string ID)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstData entity = new EntityMstData();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchase(認証処理)", ex);
                EntityMstData entity = new EntityMstData();
                entity.MESSAGE = CLASS_NM + ".GetPurchase : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            EntityMstData objList = null;
            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                string _id = ID;
                if (ExCast.IsNumeric(_id))
                {
                    _id = ExCast.zCDbl(_id).ToString();
                }

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM M_PURCHASE AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(_id)) + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    objList = new EntityMstData(ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                ExCast.zCStr(dt.DefaultView[0]["ID"]),
                                                ExCast.zCStr(dt.DefaultView[0]["NAME"])
                                                );
                    objList.ATTRIBUTE1 = ExCast.zCStr(dt.DefaultView[0]["BUSINESS_DIVISION_ID"]);
                    objList.ATTRIBUTE2 = ExCast.zCStr(dt.DefaultView[0]["TAX_CHANGE_ID"]);
                    objList.ATTRIBUTE3 = ExCast.zCStr(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    objList.ATTRIBUTE4 = ExCast.zCStr(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);
                    objList.ATTRIBUTE5 = ExCast.zCStr(dt.DefaultView[0]["UNIT_KIND_ID"]);
                    objList.ATTRIBUTE7 = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_CREDIT_PRICE"]);

                    objList.ATTRIBUTE10 = ExCast.zCStr(dt.DefaultView[0]["SUMMING_UP_GROUP_ID"]);
                    objList.ATTRIBUTE11 = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_DIVISION_ID"]);
                    objList.ATTRIBUTE12 = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_CYCLE_ID"]);
                    objList.ATTRIBUTE13 = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_DAY"]);
                    objList.ATTRIBUTE14 = ExCast.zCStr(dt.DefaultView[0]["BILL_SITE"]);
                    objList.ATTRIBUTE15 = ExCast.zCStr(dt.DefaultView[0]["PRICE_FRACTION_PROC_ID"]);
                    objList.ATTRIBUTE16 = ExCast.zCStr(dt.DefaultView[0]["TAX_FRACTION_PROC_ID"]);

                    //objList.ATTRIBUTE17 = ExCast.zCStr(dt.DefaultView[0]["INVOICE_CREDIT_LIMIT_PRICE"]);
                    objList.ATTRIBUTE18 = ExCast.zCStr(dt.DefaultView[0]["PAYMENT_CREDIT_PRICE"]);

                    objList.ATTRIBUTE19 = ExCast.zCStr(dt.DefaultView[0]["CREDIT_RATE"]);
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchase", ex);
            }
            return objList;
        }

        #endregion

        #region 一覧データ取得

        // 納入先マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetSupplierList(string random, string CustomerID, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]); 

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSupplierList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));

                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT SR.* " + Environment.NewLine);
                sb.Append("  FROM M_SUPPLIER AS SR" + Environment.NewLine);
                sb.Append(" WHERE SR.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SR.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SR.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND SR.CUSTOMER_ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(CustomerID)) + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND SR.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND SR.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                if (Kana != "")
                {
                    sb.Append("   AND SR.KANA LIKE '" + ExEscape.zRepStrNoQuota(Kana) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY SR.ID2 ");
                sb.Append("         ,SR.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["CUSTOMER_ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSupplierList", ex);
            }
            finally
            {
                db = null;
            }
            return objList;
        }

        // 得意先マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetCustomerList(string random, string ID, string Name, string Kana, string GroupID)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]); 

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCustomerList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT CM.* " + Environment.NewLine);
                sb.Append("  FROM M_CUSTOMER AS CM" + Environment.NewLine);
                sb.Append(" WHERE CM.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CM.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND CM.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND CM.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(GroupID)) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND CM.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                if (Kana != "")
                {
                    sb.Append("   AND CM.KANA LIKE '" + ExEscape.zRepStrNoQuota(Kana) + "%'" + Environment.NewLine);
                }
                if (GroupID != "")
                {
                    sb.Append("   AND CM.GROUP1_ID = " + ExEscape.zRepStr(string.Format("{0:000}", ExCast.zCDbl(GroupID))) + Environment.NewLine);
                }
                sb.Append(" ORDER BY CM.ID2" + Environment.NewLine);
                sb.Append("         ,CM.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCustomerList", ex);
            }
            return objList;
        }

        // 担当マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetPersonList(string random, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]); 

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPersonList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT PS.* " + Environment.NewLine);
                sb.Append("  FROM M_PERSON AS PS" + Environment.NewLine);
                sb.Append(" WHERE PS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND PS.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND PS.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY PS.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPersonList", ex);
            }
            return objList;
        }

        // 商品マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetCommodityList(string random, string ID, string Name, string Kana, string GroupID)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]); 

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCommodityList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT GS.* " + Environment.NewLine);
                sb.Append("  FROM M_COMMODITY AS GS" + Environment.NewLine);
                sb.Append(" WHERE GS.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND GS.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND GS.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND GS.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND GS.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                if (Kana != "")
                {
                    sb.Append("   AND GS.KANA LIKE '" + ExEscape.zRepStrNoQuota(Kana) + "%'" + Environment.NewLine);
                }
                if (GroupID != "")
                {
                    sb.Append("   AND GS.GROUP1_ID = " + ExEscape.zRepStr(string.Format("{0:000}", ExCast.zCDbl(GroupID))) + Environment.NewLine);
                }
                sb.Append(" ORDER BY GS.ID2" + Environment.NewLine);
                sb.Append("         ,GS.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCommodityList", ex);
            }
            return objList;
        }

        // 会社グループマスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetCompanyGroupList(string random, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompanyGroupList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT CG.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
                sb.Append(" WHERE CG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CG.DISPLAY_FLG = 1 " + Environment.NewLine); 
                sb.Append("   AND CG.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND CG.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND CG.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                if (Kana != "")
                {
                    sb.Append("   AND CG.KANA LIKE '" + ExEscape.zRepStrNoQuota(Kana) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY CG.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                //objList.Add(new EntityMstList("0",
                //                              "0",
                //                              "全グループ"
                //                              ));

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetCompanyGroupList", ex);
            }
            return objList;
        }

        // 郵便番号マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetZipList(string random, string zipFrom, string zipTo)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetZipList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT ZP.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_ZIP AS ZP" + Environment.NewLine);
                sb.Append(" WHERE ZP.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND ZP.DISPLAY_FLG = 1 " + Environment.NewLine);
                if (zipFrom != "" && zipTo != "")
                {
                    sb.Append("   AND ZP.ZIP = " + ExEscape.zRepStr(ExCast.zCInt(zipFrom).ToString() + zipTo) + Environment.NewLine);
                }
                else if (zipFrom != "")
                {
                    sb.Append("   AND ZP.ZIP LIKE '" + ExEscape.zRepStrNoQuota(ExCast.zCInt(zipFrom)) + "%'" + Environment.NewLine);
                }
                else if (zipTo != "")
                {
                    sb.Append("   AND ZP.ZIP LIKE '%" + ExEscape.zRepStrNoQuota(zipTo) + "'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY ZIP" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityMstList mst = new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ZIP"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["ZIP"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["PREDECTURE_NAME"]) +
                                                              ExCast.zCStr(dt.DefaultView[i]["CITY_NAME"]) +
                                                              ExCast.zCStr(dt.DefaultView[i]["TOWN_NAME"]));

                        mst.ATTRIBUTE1 = ExCast.zCStr(dt.DefaultView[i]["PREDECTURE_NAME"]);
                        mst.ATTRIBUTE2 = ExCast.zCStr(dt.DefaultView[i]["CITY_NAME"]);
                        mst.ATTRIBUTE3 = ExCast.zCStr(dt.DefaultView[i]["TOWN_NAME"]);

                        objList.Add(mst);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetZipList", ex);
            }
            return objList;
        }

        // 条件（支払・回収）マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetConditionList(string random, int kbn, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    entity.ATTRIBUTE1 = kbn.ToString();
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetConditionList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL


                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM M_CONDITION AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.CONDITION_DIVISION_ID = " + kbn.ToString() + Environment.NewLine);
                if (ID != "")
                {
                    string _id = string.Format("{0:00}", ExCast.zCInt(ID));
                   sb.Append("   AND MT.ID = " + ExEscape.zRepStr(_id) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY MT.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityMstList mst = new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                              );
                        mst.ATTRIBUTE1 = kbn.ToString();

                        objList.Add(mst);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetConditionList", ex);
            }
            return objList;
        }

        // 入金区分マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetReceitpDivisionList(string random, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceitpDivisionList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL


                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_RECEIPT_DIVISION AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY MT.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityMstList mst = new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["DESCRIPTION"])
                                                              );
                        objList.Add(mst);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReceitpDivisionList", ex);
            }
            return objList;
        }

        // 分類マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetGroupList(string random, int kbn, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetGroupList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL


                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM M_CLASS AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append("   AND MT.CLASS_DIVISION_ID = " + kbn.ToString() + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY MT.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityMstList mst = new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                              );
                        objList.Add(mst);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetGroupList", ex);
            }
            return objList;
        }

        // 仕入先マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetPurchaseList(string random, string ID, string Name, string Kana, string GroupID)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = CLASS_NM + ".GetPurchaseList : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM M_PURCHASE AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                if (Kana != "")
                {
                    sb.Append("   AND MT.KANA LIKE '" + ExEscape.zRepStrNoQuota(Kana) + "%'" + Environment.NewLine);
                }
                if (GroupID != "")
                {
                    sb.Append("   AND MT.GROUP1_ID = " + ExEscape.zRepStr(string.Format("{0:000}", ExCast.zCDbl(GroupID))) + Environment.NewLine);
                }
                sb.Append(" ORDER BY MT.ID2" + Environment.NewLine);
                sb.Append("         ,MT.ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["ID"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPurchaseList", ex);
            }
            return objList;
        }

        // ユーザーマスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetUserList(string random, string ID, string Name, string Kana)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetGroupList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;

            try
            {
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("  FROM SYS_M_USER AS MT" + Environment.NewLine);
                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.LOGIN_ID = " + ExEscape.zRepStr(ID) + Environment.NewLine);
                }
                if (Name != "")
                {
                    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                }
                sb.Append(" ORDER BY MT.LOGIN_ID" + Environment.NewLine);
                sb.Append(" LIMIT 0, 1000");

                #endregion

                dt = CommonUtl.gMySqlDt.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        EntityMstList mst = new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["LOGIN_ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["LOGIN_ID"]),
                                                              ExCast.zCStr(dt.DefaultView[i]["NAME"])
                                                              );
                        mst.ATTRIBUTE1 = ExCast.zCStr(dt.DefaultView[i]["ID"]);
                        objList.Add(mst);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetUserList", ex);
            }
            return objList;
        }

        // 商品在庫マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetInventoryList(string random, string ID, string Name)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInventoryList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("      ,CG.NAME " + Environment.NewLine);
                sb.Append("  FROM M_COMMODITY_INVENTORY AS MT" + Environment.NewLine);

                // 会社グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
                sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CG.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CG.ID = MT.GROUP_ID" + Environment.NewLine);

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                }
                //if (Name != "")
                //{
                //    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                //}
                sb.Append(" ORDER BY CG.ID" + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["NAME"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"]),
                                                      String.Format("{0,12:#,##0}", ExCast.zCDbl(dt.DefaultView[i]["INVENTORY_NUMBER"]))
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetInventoryList", ex);
            }
            return objList;
        }

        // 売掛残高マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetSalesBalanceList(string random, string ID, string Name)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSalesBalanceList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("      ,CG.NAME " + Environment.NewLine);
                sb.Append("  FROM M_SALES_CREDIT_BALANCE AS MT" + Environment.NewLine);

                // 会社グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
                sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CG.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CG.ID = MT.GROUP_ID" + Environment.NewLine);

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                }
                //if (Name != "")
                //{
                //    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                //}
                sb.Append(" ORDER BY CG.ID" + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["NAME"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"]),
                                                      String.Format("{0,12:#,##0}", ExCast.zCDbl(dt.DefaultView[i]["SALES_CREDIT_PRICE"]))
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSalesBalanceList", ex);
            }
            return objList;
        }

        // 買掛残高マスタ一覧取得
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public List<EntityMstList> GetPaymentBalanceList(string random, string ID, string Name)
        {
            List<EntityMstList> objList = new List<EntityMstList>();

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    EntityMstList entity = new EntityMstList();
                    entity.MESSAGE = _message;
                    objList.Add(entity);
                    return objList;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetSalesBalanceList(認証処理)", ex);
                EntityMstList entity = new EntityMstList();
                entity.MESSAGE = "認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                objList.Add(entity);
                return objList;
            }

            #endregion

            StringBuilder sb;
            DataTable dt;
            ExMySQLData db;

            try
            {
                db = ExSession.GetSessionDb(ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]),
                                            ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]));
                sb = new StringBuilder();

                #region SQL

                sb.Append("SELECT MT.* " + Environment.NewLine);
                sb.Append("      ,CG.NAME " + Environment.NewLine);
                sb.Append("  FROM M_PAYMENT_CREDIT_BALANCE AS MT" + Environment.NewLine);

                // 会社グループ
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
                sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND CG.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CG.ID = MT.GROUP_ID" + Environment.NewLine);

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND MT.COMPANY_ID = " + companyId + Environment.NewLine);
                if (ID != "")
                {
                    sb.Append("   AND MT.ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(ID)) + Environment.NewLine);
                }
                //if (Name != "")
                //{
                //    sb.Append("   AND MT.NAME LIKE '" + ExEscape.zRepStrNoQuota(Name) + "%'" + Environment.NewLine);
                //}
                sb.Append(" ORDER BY CG.ID" + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    for (int i = 0; i <= dt.DefaultView.Count - 1; i++)
                    {
                        objList.Add(new EntityMstList(ExCast.zCStr(dt.DefaultView[i]["NAME"]),
                                                      ExCast.zCStr(dt.DefaultView[i]["NAME"]),
                                                      String.Format("{0,12:#,##0}", ExCast.zCDbl(dt.DefaultView[i]["PAYMENT_CREDIT_PRICE"]))
                                                      ));
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetPaymentBalanceList", ex);
            }
            return objList;
        }

        #endregion

    }
}
