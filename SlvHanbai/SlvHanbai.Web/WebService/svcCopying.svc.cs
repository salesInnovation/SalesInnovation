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

namespace SlvHanbai.Web.WebService
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class svcCopying
    {
        private const string CLASS_NM = "svcCopying";

        [OperationContract]
        [WebMethod(EnableSession = true)]
        public EntityCopying CopyCheck(string random, string tblName, string Id)
        {
            EntityCopying entity;

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
                    entity = new EntityCopying();
                    entity.MESSAGE = _message;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".CopyCheck(認証処理)", ex);
                entity = new EntityCopying();
                entity.MESSAGE = CLASS_NM + ".CopyCheck : 認証処理に失敗しました。" + Environment.NewLine + ex.Message.ToString(); ;
                return entity;
            }

            #endregion

            #region Field

            ExMySQLData db = ExSession.GetSessionDb(ExCast.zCInt(userId), sessionString);
            DataPgLock.geLovkFlg lockFlg;

            entity = new EntityCopying();
            entity.is_exists_data = false;
            entity.is_lock_success = false;
            entity.ret = false;

            string get_col1Value = "";
            string get_col2Value = "";
            string get_col3Value = "";
            string get_col4Value = "";
            string get_col5Value = "";
            string lock_check_pg_id = "";
            string lock_check_id = "";

            #endregion

            try
            {
                // 複写時存在チェック
                switch (tblName)
                {
                    #region マスタ系

                    case "SYS_M_COMPANY_GROUP":
                        lock_check_id = Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Mst.CompanyGroup;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, "", tblName, "ID", Id, CommonUtl.geStrOrNumKbn.String);
                        break;

                    case "M_CUSTOMER":
                        lock_check_id = Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Mst.Customer;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, "", tblName, "ID", Id, CommonUtl.geStrOrNumKbn.String);
                        break;

                    case "M_PERSON":
                        lock_check_id = Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Mst.Person;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, "", tblName, "ID", Id, CommonUtl.geStrOrNumKbn.Number);
                        break;

                    case "M_COMMODITY":
                        lock_check_id = Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Mst.Commodity;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, "", tblName, "ID", Id, CommonUtl.geStrOrNumKbn.String);
                        break;

                    #endregion

                    #region 伝票入力系

                    #region 売上入力系

                    case "T_ESTIMATE_H":
                        lock_check_id = groupId + "-" + Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Estimate.EstimateInp;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, groupId, tblName, "NO", Id, CommonUtl.geStrOrNumKbn.Number);

                        if (entity.is_exists_data)
                        {
                            // 受注計上済チェック
                            if (DataExists.IsExistData(db, companyId, groupId, "T_ORDER_H", "ESTIMATENO", Id, CommonUtl.geStrOrNumKbn.Number))
                            {
                                entity.MESSAGE = "複写先ID : " + Id + " は受注計上済の為、複写できません。";
                                return entity;
                            } 

                            // 売上計上済チェック
                            if (DataExists.IsExistData(db, companyId, groupId, "T_SALES_H", "ESTIMATENO", Id, CommonUtl.geStrOrNumKbn.Number))
                            {
                                entity.MESSAGE = "複写先ID : " + Id + " は売上計上済の為、複写できません。";
                                return entity;
                            }
                        }

                        break;

                    case "T_ORDER_H":
                        lock_check_id = groupId + "-" + Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Order.OrderInp;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, groupId, tblName, "NO", Id, CommonUtl.geStrOrNumKbn.Number);

                        if (entity.is_exists_data)
                        {
                            // 売上計上済チェック
                            if (DataExists.IsExistData(db, companyId, groupId, "T_SALES_H", "ORDER_NO", Id, CommonUtl.geStrOrNumKbn.Number))
                            {
                                entity.MESSAGE = "複写先ID : " + Id + " は売上計上済の為、複写できません。";
                                return entity;
                            }
                        }

                        break;

                    case "T_SALES_H":
                        lock_check_id = groupId + "-" + Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Sales.SalesInp;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, groupId, tblName, "NO", Id, CommonUtl.geStrOrNumKbn.Number);

                        if (entity.is_exists_data)
                        {
                            DataExists.GetData(db,
                                               companyId,
                                               groupId,
                                               tblName,
                                               "NO",
                                               Id,
                                               CommonUtl.geStrOrNumKbn.Number,
                                               "TBL.INVOICE_ID",
                                               "TBL.INVOICE_NO",
                                               "TBL.RECEIPT_NO",
                                               "TBL.BUSINESS_DIVISION_ID",
                                               "date_format(TBL.SALES_YMD , " + ExEscape.SQL_YMD + ") AS SALES_YMD",
                                               "INVOICE_ID",
                                               "INVOICE_NO",
                                               "RECEIPT_NO",
                                               "BUSINESS_DIVISION_ID",
                                               "SALES_YMD",
                                               ref get_col1Value,
                                               ref get_col2Value,
                                               ref get_col3Value,
                                               ref get_col4Value,
                                               ref get_col5Value
                                               );

                            // 掛売上
                            if (ExCast.zCInt(get_col4Value) == 1)
                            {
                                // 請求締切済チェック
                                if (DataClose.IsInvoiceClose(companyId, db, ExCast.zNumZeroNothingFormat(get_col1Value), ExCast.zDateNullToDefault(get_col5Value)))
                                {
                                    entity.is_lock_success = false;
                                    entity.ret = false;
                                    entity.MESSAGE = "複写先ID : " + Id + " は請求締切済の為、複写できません。";
                                    return entity;
                                }
                            }

                            // 請求済チェック
                            // 都度請求分も請求番号が設定されている為、不許可とする
                            if (ExCast.zCLng(get_col2Value) != 0)
                            {
                                entity.MESSAGE = "複写先ID : " + Id + " は請求済の為、複写できません。";
                                return entity;
                            }

                            // 入金済チェック
                            if (ExCast.zCLng(get_col2Value) != 0)
                            {
                                if (DataExists.IsExistData(db, companyId, groupId, "T_RECEIPT_H", "INVOICE_NO", ExCast.zCLng(get_col2Value).ToString(), CommonUtl.geStrOrNumKbn.Number))
                                {
                                    entity.MESSAGE = "複写先ID : " + Id + " は入金計上済の為、複写できません。";
                                    return entity;
                                }
                            }
                        }
                        break;

                    case "T_RECEIPT_H":
                        lock_check_id = groupId + "-" + Id;
                        lock_check_pg_id = DataPgEvidence.PGName.Receipt.ReceiptInp;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, groupId, tblName, "NO", Id, CommonUtl.geStrOrNumKbn.Number);
                        break;

                    #endregion

                    #region 在庫入力系

                    case "T_IN_OUT_DELIVERY_H":
                        lock_check_id = groupId + "-" + Id;
                        lock_check_pg_id = DataPgEvidence.PGName.InOutDeliver.InOutDeliverInp;
                        entity.is_exists_data = DataExists.IsExistData(db, companyId, groupId, tblName, "NO", Id, CommonUtl.geStrOrNumKbn.Number);

                        if (entity.is_exists_data)
                        {
                            // 売上計上分チェック
                            if (DataExists.IsExistDataDouble(db, companyId, groupId, "T_IN_OUT_DELIVERY_H", "NO", Id, "IN_OUT_DELIVERY_PROC_KBN", "2", CommonUtl.geStrOrNumKbn.Number))
                            {
                                entity.MESSAGE = "複写先ID : " + Id + " は売上計上分の為、複写できません。";
                                return entity;
                            }

                            // 仕入計上分チェック
                            if (DataExists.IsExistDataDouble(db, companyId, groupId, "T_IN_OUT_DELIVERY_H", "NO", Id, "IN_OUT_DELIVERY_PROC_KBN", "3", CommonUtl.geStrOrNumKbn.Number))
                            {
                                entity.MESSAGE = "複写先ID : " + Id + " は仕入計上分の為、複写できません。";
                                return entity;
                            }
                        }

                        break;

                    #endregion

                    #endregion

                    default:
                        entity.is_exists_data = false;
                        break;
                }

                // 排他制御
                string strErr = DataPgLock.SetLockPg(companyId, userId, lock_check_pg_id, lock_check_id, ipAdress, db, out lockFlg);
                if (strErr != "")
                {
                    entity.MESSAGE = CLASS_NM + ".CopyCheck : 排他制御(ロック情報取得)に失敗しました。" + Environment.NewLine + strErr;
                    entity.is_lock_success = false;
                    entity.ret = false;
                }
                else
                {
                    if (lockFlg == DataPgLock.geLovkFlg.Lock)
                    {
                        entity.is_lock_success = false;
                        entity.ret = false;
                        entity.MESSAGE = "複写先ID : " + Id + " は他ユーザーにて現在更新中の為、複写できません。";
                    }
                    else
                    {
                        entity.is_lock_success = true;
                        entity.ret = true;
                    }
                }

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".CopyCheck", ex);
                entity = new EntityCopying();
                entity.MESSAGE = CLASS_NM + ".CopyCheck : 予期せぬエラーが発生しました。" + Environment.NewLine + ex.Message.ToString(); ;
            }

            return entity;
        }
    }
}
