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
using SlvHanbai.Web.Class.Data;
using SlvHanbai.Web.Class.Utility;
using SlvHanbai.Web.Class.Entity;
using SlvHanbai.Web.Class.Reports;

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcReport
    {
        private const string CLASS_NM = "svcReport";
        private readonly string PG_NM = DataPgEvidence.PGName.Mst.ReportSetting;

        [OperationContract]
        [WebMethod(EnableSession = true)]
        public EntityReport ReportOut(string random, string rptKbn, string pgId, string parameters)
        {
            EntityReport entity;
            EntityReportSetting entitySetting;

            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            int reportSizeUser = 0;
            int idFigureCommodity = 0;
            int idFigureCustomer = 0;
            int idFigurePurchase = 0;
            int idFigureSlipNo = 0;
            int rpTotalAuthorityKbn = 0;

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                reportSizeUser = ExCast.zCInt(HttpContext.Current.Session[ExSession.REPORT_SAVE_SIZE_USER]);
                idFigureCommodity = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_GOODS]);
                idFigureCustomer = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_CUSTOMER]);
                idFigurePurchase = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_PURCHASE]);
                idFigureSlipNo = ExCast.zCInt(HttpContext.Current.Session[ExSession.ID_FIGURE_SLIP_NO]);
                rpTotalAuthorityKbn = ExCast.zCInt(HttpContext.Current.Session[ExSession.REPORT_TOTAL_AUTHORITY_KBN]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    entity = new EntityReport();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".ReportOut(認証処理)", ex);
                entity = new EntityReport();
                entity.MESSAGE = CLASS_NM + ".ReportOut : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            #endregion

            ExMySQLData db = ExSession.GetSessionDb(ExCast.zCInt(userId), sessionString);

            try
            {
                #region Get Report Setting

                entitySetting = GetReportSetting(random, pgId);

                if (entitySetting == null)
                {
                    entitySetting = new EntityReportSetting();
                    entitySetting.user_id = ExCast.zCInt(userId);
                    entitySetting.pg_id = pgId;
                    entitySetting.group_id_from = ExCast.zCInt(groupId).ToString();
                    entitySetting.group_id_to = ExCast.zCInt(groupId).ToString();
                }
                else if (entitySetting != null)
                {
                    if (!string.IsNullOrEmpty(entitySetting.MESSAGE))
                    {
                        entitySetting = new EntityReportSetting();
                        entitySetting.user_id = ExCast.zCInt(userId);
                        entitySetting.pg_id = pgId;
                        entitySetting.group_id_from = ExCast.zCInt(groupId).ToString();
                        entitySetting.group_id_to = ExCast.zCInt(groupId).ToString();
                    }
                    else
                    {
                        if (ExCast.zCInt(entitySetting.group_id_from)== 0 && ExCast.zCInt(entitySetting.group_id_to) == 0)
                        {
                            entitySetting.group_id_from = ExCast.zCInt(groupId).ToString();
                            entitySetting.group_id_to = ExCast.zCInt(groupId).ToString();
                        }
                    }
                }

                if (rpTotalAuthorityKbn < 2)
                {
                    entitySetting.group_id_from = ExCast.zCInt(groupId).ToString();
                    entitySetting.group_id_to = ExCast.zCInt(groupId).ToString();
                }

                #endregion

                entity = new EntityReport();

                DataReport.geReportKbn kbn = (DataReport.geReportKbn)ExCast.zCInt(rptKbn);
                ExReportManeger rptMgr = new ExReportManeger();
                rptMgr.idFigureCommodity = idFigureCommodity;
                rptMgr.idFigureCustomer = idFigureCustomer;
                rptMgr.idFigurePurchase = idFigurePurchase;
                rptMgr.idFigureSlipNo = idFigureSlipNo;
                rptMgr.entitySetting = entitySetting;
                rptMgr.rptKbn = kbn;

                #region Report FilePath Setting

                bool _ret = rptMgr.GetReportFilePath(pgId, companyId, userId);
                if (_ret == false)
                {
                    entity.MESSAGE = CommonUtl.gstrErrMsg;
                    return entity;
                }
                entity.downLoadFilePath = rptMgr.reportFilePath;
                entity.downLoadFileName = rptMgr.reportFileName;
                entity.downLoadFileSize = rptMgr.reportFileSize.ToString();
                entity.downLoadUrl = CommonUtl.gstrMainUrl + rptMgr.reportDir;

                #endregion

                DataSet ds = db.GetDataSet(rptMgr.ReportSQL(pgId, companyId, groupId, parameters), rptMgr.GetPGIDXsd(pgId));

                #region Export xsd

                System.IO.StreamWriter xmlSW = null;
                try
                {
                    xmlSW = new System.IO.StreamWriter(CommonUtl.gstrReportTemp + rptMgr.GetPGIDXsd(pgId) + ".xsd");
                    ds.WriteXmlSchema(xmlSW);
                    xmlSW.Close();
                }
                catch (Exception ex)
                {
                    CommonUtl.ExLogger.Error(CLASS_NM + ".WriteXmlSchema", ex);
                }
                finally
                {
                    if (xmlSW != null)
                    {
                        xmlSW.Dispose();
                        xmlSW = null;
                    }
                }

                #endregion

                if (ds.Tables[0].Rows.Count == 0)
                {
                    entity.MESSAGE = "データが存在しません。";
                    entity.ret = false;
                    return entity;
                }

                string _fileType = "";
                string _DownloadType = "";
                switch (kbn)
                {
                    case DataReport.geReportKbn.OutPut:
                        _fileType = "PDF";
                        _DownloadType = "出力";

                        if (rptMgr.ReportToPdf(ds, pgId) == true)
                        {
                            entity.ret = true;
                        }
                        else
                        {
                            entity.MESSAGE = CommonUtl.gstrErrMsg;
                            entity.ret = false;
                        }
                        break;
                    case DataReport.geReportKbn.Download:
                        _fileType = "PDF";
                        _DownloadType = "ダウンロード";

                        if (rptMgr.ReportToPdf(ds, pgId) == true)
                        {
                            entity.ret = true;
                        }
                        else
                        {
                            entity.MESSAGE = CommonUtl.gstrErrMsg;
                            entity.ret = false;
                        }
                        break;
                    case DataReport.geReportKbn.Csv:
                        _fileType = "CSV";
                        _DownloadType = "ダウンロード";

                        if (rptMgr.DataTableToCsv(ds.Tables[0]) == true)
                        {
                            entity.ret = true;
                        }
                        else
                        {
                            entity.MESSAGE = CommonUtl.gstrErrMsg;
                            entity.ret = false;
                        }
                        break;
                    default:
                        break;
                }

                //entity.downLoadFilePath = @"d:\HostingSpaces\Users\EW20121725\Sales.system-innovation.net\wwwroot\temp\顧客マスタ一覧.csv";
                //System.IO.FileInfo fi = new System.IO.FileInfo(entity.downLoadFilePath);
                //entity.downLoadFileSize = fi.Length.ToString();
                if (reportSizeUser < ExCast.zCDbl(entity.downLoadFileSize) / 1000000)
                {
                    entity.MESSAGE = _fileType + "ファイルのサイズが" + reportSizeUser.ToString() + "Mバイトを超える為、" + _DownloadType + "できません。";
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".ReportOut", ex);
                entity = new EntityReport();
                entity.MESSAGE = CLASS_NM + ".ReportOut : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
                return entity;
            }

            return entity;
        }

        #region データ取得

        /// <summary> 
        /// データ取得
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public EntityReportSetting GetReportSetting(string random, string Id)
        {

            EntityReportSetting entity;

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
                    entity = new EntityReportSetting();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReportSetting(認証処理)", ex);
                entity = new EntityReportSetting();
                entity.MESSAGE = CLASS_NM + ".GetReportSetting : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
                return entity;
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
                sb.Append("      ,SG1.NAME AS GROUP_NAME_FROM " + Environment.NewLine);
                sb.Append("      ,SG2.NAME AS GROUP_NAME_TO " + Environment.NewLine);
                sb.Append("  FROM M_REPORT_SETTING AS MT" + Environment.NewLine);

                #region Join

                // ユーザ
                sb.Append("  LEFT JOIN SYS_M_USER AS UR" + Environment.NewLine);
                sb.Append("    ON UR.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND UR.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND UR.ID = MT.USER_ID" + Environment.NewLine);

                // グループFrom
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS SG1" + Environment.NewLine);
                sb.Append("    ON SG1.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SG1.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SG1.COMPANY_ID = UR.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SG1.ID = MT.GROUP_ID_FROM" + Environment.NewLine);

                // グループTo
                sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS SG2" + Environment.NewLine);
                sb.Append("    ON SG2.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND SG2.DISPLAY_FLG = 1 " + Environment.NewLine);
                sb.Append("   AND SG2.COMPANY_ID = UR.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND SG2.ID = MT.GROUP_ID_TO" + Environment.NewLine);

                #endregion

                sb.Append(" WHERE MT.DELETE_FLG = 0 " + Environment.NewLine);
                sb.Append("   AND MT.USER_ID = " + userId + Environment.NewLine);
                sb.Append("   AND MT.PG_ID = " + ExEscape.zRepStr(Id) + Environment.NewLine);

                #endregion

                dt = db.GetDataTable(sb.ToString());

                if (dt.DefaultView.Count > 0)
                {
                    entity = new EntityReportSetting();

                    // 排他制御
                    //DataPgLock.geLovkFlg lockFlg;
                    //string strErr = DataPgLock.SetLockPg(companyId, userId, PG_NM, Id.ToString(), ipAdress, db, out lockFlg);
                    //if (strErr != "")
                    //{
                    //    entity.MESSAGE = CLASS_NM + ".GetReportSetting : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    //}

                    #region Set Entity

                    entity.user_id = ExCast.zCInt(dt.DefaultView[0]["USER_ID"]);
                    entity.pg_id = Id;
                    entity.size = ExCast.zCInt(dt.DefaultView[0]["SIZE"]);
                    entity.orientation = ExCast.zCInt(dt.DefaultView[0]["ORIENTATION"]);

                    entity.left_margin = ExCast.zCDbl(dt.DefaultView[0]["LEFT_MARGIN"]);
                    entity.right_margin = ExCast.zCDbl(dt.DefaultView[0]["RIGHT_MARGIN"]);
                    entity.top_margin = ExCast.zCDbl(dt.DefaultView[0]["TOP_MARGIN"]);
                    entity.bottom_margin = ExCast.zCDbl(dt.DefaultView[0]["BOTTOM_MARGIN"]);

                    if (ExCast.zCInt(dt.DefaultView[0]["GROUP_ID_FROM"]) != 0)
                    {
                        entity.group_id_from = string.Format("{0:000}", ExCast.zCInt(dt.DefaultView[0]["GROUP_ID_FROM"]));
                    }
                    entity.group_nm_from = ExCast.zCStr(dt.DefaultView[0]["GROUP_NAME_FROM"]);

                    if (ExCast.zCInt(dt.DefaultView[0]["GROUP_ID_TO"]) != 0)
                    {
                        entity.group_id_to = string.Format("{0:000}", ExCast.zCInt(dt.DefaultView[0]["GROUP_ID_TO"]));
                    }
                    entity.group_nm_to = ExCast.zCStr(dt.DefaultView[0]["GROUP_NAME_TO"]);

                    entity.group_total = ExCast.zCInt(dt.DefaultView[0]["GROUP_TOTAL"]);
                    entity.total_kbn = ExCast.zCInt(dt.DefaultView[0]["TOTAL_KBN"]);

                    #endregion

                }
                else
                {
                    entity = null;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReportSetting", ex);
                entity = new EntityReportSetting();
                entity.MESSAGE = CLASS_NM + ".GetReportSetting : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString();
            }
            finally
            {
                db = null;
            }

            svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                           companyId,
                           userId,
                           ipAdress,
                           sessionString,
                           PG_NM,
                           DataPgEvidence.geOperationType.Select,
                           "ID:" + Id.ToString());

            return entity;

        }

        #endregion

        #region データ更新

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="random">セッションランダム文字列</param>
        /// <param name="type">0:Update 1:Insert 2:Delete</param>
        /// <param name="Id">Id</param>
        /// <param name="entity">更新データ</param>
        /// <returns></returns>
        [OperationContract()]
        [WebMethod(EnableSession = true)]
        public string UpdateReportSetting(string random, int type, string Id, EntityReportSetting entity)
        {
            #region 認証処理

            string companyId = "";
            string groupId = "";
            string userId = "";
            string ipAdress = "";
            string sessionString = "";
            string personId = "";

            try
            {
                companyId = ExCast.zCStr(HttpContext.Current.Session[ExSession.COMPANY_ID]);
                groupId = ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_ID]);
                userId = ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_ID]);
                ipAdress = ExCast.zCStr(HttpContext.Current.Session[ExSession.IP_ADRESS]);
                sessionString = ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]);
                personId = ExCast.zCStr(HttpContext.Current.Session[ExSession.PERSON_ID]);

                string _message = ExSession.SessionUserUniqueCheck(random, ExCast.zCStr(HttpContext.Current.Session[ExSession.SESSION_RANDOM_STR]), ExCast.zCInt(HttpContext.Current.Session[ExSession.USER_ID]));
                if (_message != "")
                {
                    return _message;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(認証処理)", ex);
                return CLASS_NM + ".UpdateReportSetting : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString();
            }

            #endregion

            #region Field

            StringBuilder sb = new StringBuilder();
            DataTable dt;
            ExMySQLData db = null;
            string _Id = Id;

            #endregion

            #region Databese Open

            try
            {
                db = new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                db.DbOpen();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(DbOpen)", ex);
                return CLASS_NM + ".UpdateReportSetting(DbOpen) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region BeginTransaction

            try
            {
                db.ExBeginTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(BeginTransaction)", ex);
                return CLASS_NM + ".UpdateReportSetting(BeginTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Insert

            if (type <= 1)
            {
                try
                {
                    #region Delete SQL

                    sb.Length = 0;
                    sb.Append("DELETE FROM M_REPORT_SETTING " + Environment.NewLine);
                    sb.Append(" WHERE USER_ID = " + userId + Environment.NewLine);
                    sb.Append("   AND PG_ID = " + ExEscape.zRepStr(Id) + Environment.NewLine);

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                    #region Insert SQL

                    sb.Length = 0;
                    sb.Append("INSERT INTO M_REPORT_SETTING " + Environment.NewLine);
                    sb.Append("       ( USER_ID" + Environment.NewLine);
                    sb.Append("       , PG_ID" + Environment.NewLine);
                    sb.Append("       , SIZE" + Environment.NewLine);
                    sb.Append("       , ORIENTATION" + Environment.NewLine);
                    sb.Append("       , LEFT_MARGIN" + Environment.NewLine);
                    sb.Append("       , RIGHT_MARGIN" + Environment.NewLine);
                    sb.Append("       , TOP_MARGIN" + Environment.NewLine);
                    sb.Append("       , BOTTOM_MARGIN" + Environment.NewLine);
                    sb.Append("       , GROUP_ID_FROM" + Environment.NewLine);
                    sb.Append("       , GROUP_ID_TO" + Environment.NewLine);
                    sb.Append("       , GROUP_TOTAL" + Environment.NewLine);
                    sb.Append("       , TOTAL_KBN" + Environment.NewLine);
                    sb.Append("       , UPDATE_FLG" + Environment.NewLine);
                    sb.Append("       , DELETE_FLG" + Environment.NewLine);
                    sb.Append("       , CREATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , CREATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , CREATE_DATE" + Environment.NewLine);
                    sb.Append("       , CREATE_TIME" + Environment.NewLine);
                    sb.Append("       , UPDATE_PG_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_ADRESS" + Environment.NewLine);
                    sb.Append("       , UPDATE_USER_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_PERSON_ID" + Environment.NewLine);
                    sb.Append("       , UPDATE_DATE" + Environment.NewLine);
                    sb.Append("       , UPDATE_TIME" + Environment.NewLine);
                    sb.Append(")" + Environment.NewLine);
                    sb.Append("SELECT  " + userId + Environment.NewLine);                       // USER_ID
                    sb.Append("       ," + ExEscape.zRepStr(_Id) + Environment.NewLine);        // PG_ID
                    sb.Append("       ," + entity.size + Environment.NewLine);                  // SIZE
                    sb.Append("       ," + entity.orientation + Environment.NewLine);           // ORIENTATION
                    sb.Append("       ," + entity.left_margin + Environment.NewLine);           // LEFT_MARGIN
                    sb.Append("       ," + entity.right_margin + Environment.NewLine);          // RIGHT_MARGIN
                    sb.Append("       ," + entity.top_margin + Environment.NewLine);            // TOP_MARGIN
                    sb.Append("       ," + entity.bottom_margin + Environment.NewLine);         // BOTTOM_MARGIN
                    sb.Append("       ," + entity.group_id_from + Environment.NewLine);         // GROUP_ID_FROM
                    sb.Append("       ," + entity.group_id_to + Environment.NewLine);           // GROUP_ID_TO
                    sb.Append("       ," + entity.group_total + Environment.NewLine);           // GROUP_TOTAL
                    sb.Append("       ," + entity.total_kbn + Environment.NewLine);             // TOTAL_KBN
                    sb.Append(CommonUtl.GetInsSQLCommonColums(CommonUtl.UpdKbn.Ins,
                                                                PG_NM,
                                                                "M_REPORT_SETTING",
                                                                ExCast.zCInt(personId),
                                                                _Id,
                                                                ipAdress,
                                                                userId));

                    #endregion

                    db.ExecuteSQL(sb.ToString(), false);

                }
                catch (Exception ex)
                {
                    db.ExRollbackTransaction();
                    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(Insert)", ex);
                    return CLASS_NM + ".UpdateReportSetting(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                }

            }

            #endregion

            #region Update

            if (type == 0)
            {
                //try
                //{
                //    #region SQL

                //    string _invoice_id = entity.invoice_id;
                //    if (ExCast.IsNumeric(_invoice_id)) _invoice_id = ExCast.zCDbl(_invoice_id).ToString();

                //    sb.Length = 0;
                //    sb.Append("UPDATE M_REPORT_SETTING " + Environment.NewLine);
                //    sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
                //                                               ExCast.zCInt(personId),
                //                                               ipAdress,
                //                                               userId,
                //                                               0));
                //    sb.Append("      ,NAME = " + ExEscape.zRepStr(entity.name) + Environment.NewLine);
                //    sb.Append("      ,KANA = " + ExEscape.zRepStr(entity.kana) + Environment.NewLine);
                //    sb.Append("      ,ABOUT_NAME = " + ExEscape.zRepStr(entity.about_name) + Environment.NewLine);
                //    sb.Append("      ,ZIP_CODE = " + ExCast.zNullToZero(entity.zip_code_from + entity.zip_code_to) + Environment.NewLine);
                //    sb.Append("      ,PREFECTURE_ID = " + entity.prefecture_id + Environment.NewLine);
                //    sb.Append("      ,CITY_ID = " + entity.city_id + Environment.NewLine);
                //    sb.Append("      ,TOWN_ID = " + entity.town_id + Environment.NewLine);
                //    sb.Append("      ,ADRESS_CITY = " + ExEscape.zRepStr(entity.adress_city) + Environment.NewLine);
                //    sb.Append("      ,ADRESS_TOWN = " + ExEscape.zRepStr(entity.adress_town) + Environment.NewLine);
                //    sb.Append("      ,ADRESS1 = " + ExEscape.zRepStr(entity.adress1) + Environment.NewLine);
                //    sb.Append("      ,ADRESS2 = " + ExEscape.zRepStr(entity.adress2) + Environment.NewLine);
                //    sb.Append("      ,STATION_NAME = " + ExEscape.zRepStr(entity.station_name) + Environment.NewLine);
                //    sb.Append("      ,POST_NAME = " + ExEscape.zRepStr(entity.post_name) + Environment.NewLine);
                //    sb.Append("      ,PERSON_NAME = " + ExEscape.zRepStr(entity.person_name) + Environment.NewLine);
                //    sb.Append("      ,TITLE_ID = " + entity.title_id + Environment.NewLine);
                //    sb.Append("      ,TITLE_NAME = " + ExEscape.zRepStr(entity.title_name) + Environment.NewLine);
                //    sb.Append("      ,TEL = " + ExEscape.zRepStr(entity.tel) + Environment.NewLine);
                //    sb.Append("      ,FAX = " + ExEscape.zRepStr(entity.fax) + Environment.NewLine);
                //    sb.Append("      ,MAIL_ADRESS = " + ExEscape.zRepStr(entity.mail_adress) + Environment.NewLine);
                //    sb.Append("      ,MOBILE_TEL = " + ExEscape.zRepStr(entity.mobile_tel) + Environment.NewLine);
                //    sb.Append("      ,MOBILE_ADRESS = " + ExEscape.zRepStr(entity.mobile_adress) + Environment.NewLine);
                //    sb.Append("      ,URL = " + ExEscape.zRepStr(entity.url) + Environment.NewLine);
                //    sb.Append("      ,INVOICE_ID = " + ExEscape.zRepStr(_invoice_id) + Environment.NewLine);
                //    sb.Append("      ,BUSINESS_DIVISION_ID = " + entity.business_division_id + Environment.NewLine);
                //    sb.Append("      ,UNIT_KIND_ID = " + entity.unit_kind_id + Environment.NewLine);
                //    sb.Append("      ,CREDIT_RATE = " + entity.credit_rate + Environment.NewLine);
                //    sb.Append("      ,TAX_CHANGE_ID = " + entity.tax_change_id + Environment.NewLine);
                //    sb.Append("      ,SUMMING_UP_GROUP_ID = " + ExEscape.zRepStr(entity.summing_up_group_id) + Environment.NewLine);
                //    sb.Append("      ,PRICE_FRACTION_PROC_ID = " + entity.price_fraction_proc_id + Environment.NewLine);
                //    sb.Append("      ,TAX_FRACTION_PROC_ID = " + entity.tax_fraction_proc_id + Environment.NewLine);
                //    sb.Append("      ,CREDIT_LIMIT_PRICE = " + entity.credit_limit_price + Environment.NewLine);
                //    sb.Append("      ,SALES_CREDIT_PRICE = " + entity.sales_credit_price + Environment.NewLine);
                //    sb.Append("      ,RECEIPT_DIVISION_ID = " + ExEscape.zRepStr(entity.receipt_division_id) + Environment.NewLine);
                //    sb.Append("      ,COLLECT_CYCLE_ID = " + entity.collect_cycle_id + Environment.NewLine);
                //    sb.Append("      ,COLLECT_DAY = " + entity.collect_day + Environment.NewLine);
                //    sb.Append("      ,BILL_SITE = " + entity.bill_site + Environment.NewLine);
                //    sb.Append("      ,GROUP1_ID = " + ExEscape.zRepStr(entity.group1_id) + Environment.NewLine);
                //    sb.Append("      ,GROUP2_ID = " + ExEscape.zRepStr(entity.group2_id) + Environment.NewLine);
                //    sb.Append("      ,GROUP3_ID = " + ExEscape.zRepStr(entity.group3_id) + Environment.NewLine);
                //    sb.Append("      ,MEMO = " + ExEscape.zRepStr(entity.memo) + Environment.NewLine);
                //    sb.Append("      ,DISPLAY_FLG = " + entity.display_division_id + Environment.NewLine);
                //    sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);                            // COMPANY_ID
                //    sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);     // ID

                //    #endregion

                //    db.ExecuteSQL(sb.ToString(), false);

                //}
                //catch (Exception ex)
                //{
                //    db.ExRollbackTransaction();
                //    CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(Update)", ex);
                //    return CLASS_NM + ".UpdateReportSetting(Insert) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
                //}
            }

            #endregion

            #region Delete

            //if (type == 2)
            //{
            //    #region Exist Data

            //    try
            //    {
            //        bool _ret = false;
            //        _ret = DataExists.IsExistData(db, companyId, "", "M_REPORT_SETTING", "INVOICE_ID", ExCast.zNumZeroNothingFormat(Id), "ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
            //        if (_ret == true)
            //        {
            //            return "得意先ID : " + Id + " は得意先マスタの請求IDにが使用されている為、削除できません。";
            //        }

            //        _ret = DataExists.IsExistData(db, companyId, "", "T_ORDER_H", "ReportSetting_ID", ExCast.zNumZeroNothingFormat(Id), CommonUtl.geStrOrNumKbn.String);
            //        if (_ret == true)
            //        {
            //            return "得意先ID : " + Id + " は受注データの得意先に使用されている為、削除できません。";
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        db.ExRollbackTransaction();
            //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(Exist Data)", ex);
            //        return CLASS_NM + ".UpdateReportSetting(Exist Data) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //    }

            //    #endregion

            //    #region Update

            //    try
            //    {
            //        sb.Length = 0;
            //        sb.Append("UPDATE M_REPORT_SETTING " + Environment.NewLine);
            //        sb.Append(CommonUtl.GetUpdSQLCommonColums(PG_NM,
            //                                                   ExCast.zCInt(personId),
            //                                                   ipAdress,
            //                                                   userId,
            //                                                   1));
            //        sb.Append(" WHERE COMPANY_ID = " + companyId + Environment.NewLine);    // COMPANY_ID
            //        sb.Append("   AND ID = " + ExEscape.zRepStr(ExCast.zNumZeroNothingFormat(Id)) + Environment.NewLine);            // ID

            //        db.ExecuteSQL(sb.ToString(), false);

            //    }
            //    catch (Exception ex)
            //    {
            //        db.ExRollbackTransaction();
            //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(Delete)", ex);
            //        return CLASS_NM + ".UpdateReportSetting(Delete) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //    }

            //    #endregion
            //}

            #endregion

            #region PG排他制御

            //if (type == 0 || type == 2)
            //{
            //    try
            //    {
            //        DataPgLock.DelLockPg(companyId, userId, PG_NM, "", ipAdress, false, db);
            //    }
            //    catch (Exception ex)
            //    {
            //        db.ExRollbackTransaction();
            //        CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(DelLockPg)", ex);
            //        return CLASS_NM + ".UpdateReportSetting(DelLockPg) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            //    }
            //}

            #endregion

            #region CommitTransaction

            try
            {
                db.ExCommitTransaction();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(CommitTransaction)", ex);
                return CLASS_NM + ".UpdateReportSetting(CommitTransaction) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Database Close

            try
            {
                db.DbClose();
            }
            catch (Exception ex)
            {
                db.ExRollbackTransaction();
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(DbClose)", ex);
                return CLASS_NM + ".UpdateReportSetting(DbClose) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }
            finally
            {
                db = null;
            }

            #endregion

            #region Add Evidence

            try
            {
                switch (type)
                {
                    case 0:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Update,
                                                   "ID:" + _Id.ToString());
                        break;
                    case 1:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Insert,
                                                   "ID:" + _Id.ToString());
                        break;
                    case 2:
                        svcPgEvidence.gAddEvidence(ExCast.zCInt(HttpContext.Current.Session[ExSession.EVIDENCE_SAVE_FLG]),
                                                   companyId,
                                                   userId,
                                                   ipAdress,
                                                   sessionString,
                                                   PG_NM,
                                                   DataPgEvidence.geOperationType.Delete,
                                                   "ID:" + Id.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".UpdateReportSetting(Add Evidence)", ex);
                return CLASS_NM + ".UpdateReportSetting(Add Evidence) : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message;
            }

            #endregion

            #region Return

            return "";
            //if (type == 1 && (Id == "0" || Id == ""))
            //{
            //    return "Auto Insert success : " + "ID : " + _Id.ToString() + "で登録しました。";
            //}
            //else
            //{
            //    return "";
            //}

            #endregion
        }

        #endregion

    }
}
