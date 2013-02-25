using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SlvHanbai.Web.Class.DB;
using SlvHanbai.Web.Class.Utility;

namespace SlvHanbai.Web.Class
{
    public class ExSession
    {
        private const string CLASS_NM = "ExSession";

        public const string DATA_CLASS = "DATA_CLASS";
        public const string DB_CONNECTION_STR = "DB_CONNECTION_STR";
        public const string DB_CONNECTION_PROVIDER = "DB_CONNECTION_PROVIDER";
        public const string SESSION_RANDOM_STR = "SESSION_RANDOM_STR";
        public const string USER_ID = "USER_ID";
        public const string USER_NM = "USER_NM";
        public const string DEFAULT_PERSON_ID = "DEFAULT_PERSON_ID";
        public const string PERSON_ID = "PERSON_ID";
        public const string IP_ADRESS = "IP_ADRESS";
        public const string COMPANY_ID = "COMPANY_ID";
        public const string GROUP_ID = "GROUP_ID";
        public const string GROUP_DISPLAY_NAME = "GROUP_DISPLAY_NAME";
        public const string EVIDENCE_SAVE_FLG = "EVIDENCE_SAVE_FLG";
        public const string ACCOUNT_BEGIN_PERIOD = "ACCOUNT_BEGIN_PERIOD";
        public const string ACCOUNT_END_PERIOD = "ACCOUNT_END_PERIOD";
        public const string ID_FIGURE_SLIP_NO = "ID_FIGURE_SLIP_NO";
        public const string ID_FIGURE_CUSTOMER = "ID_FIGURE_CUSTOMER";
        public const string ID_FIGURE_PURCHASE = "ID_FIGURE_PURCHASE";
        public const string ID_FIGURE_GOODS = "ID_FIGURE_GOODS";
        public const string ESTIMATE_APPROVAL_FLG = "ESTIMATE_APPROVAL_FLG";
        public const string RECEIPT_ACCOUNT_INVOICE_PRINT_FLG = "RECEIPT_ACCOUNT_INVOICE_PRINT_FLG";
        public const string REPORT_SAVE_SIZE_USER = "REPORT_SAVE_SIZE_USER";
        public const string REPORT_SAVE_SIZE_ALL = "REPORT_SAVE_SIZE_ALL";
        public const string REPORT_TOTAL_AUTHORITY_KBN = "REPORT_TOTAL_AUTHORITY_KBN";      // レポート集計権限(GetAuthority,UpdateAuthorityにて設定)

        public static List<SessionHold> sessionInf = new List<SessionHold>();

        // セッション保持情報の追加
        public static bool AddSessionInf(int userId, string randomString, ExMySQLData db, ref string message)
        {
            try
            {
                DelSessionInf(userId);

                for (int i = 0; i <= sessionInf.Count - 1; i++)
                {
                    if (sessionInf[i].userId == userId)
                    {
                        return false;
                    }
                }

                sessionInf.Add(new SessionHold(userId, randomString, db));

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }

        }

        // セッション保持情報の削除
        public static bool DelSessionInf(int userId)
        {
            for (int i = 0; i <= sessionInf.Count - 1; i++)
            {
                if (sessionInf[i].userId == userId)
                {
                    sessionInf.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        // セッション保持DB情報の取得
        public static ExMySQLData GetSessionDb(int userId, string randomString)
        {
            for (int i = 0; i <= sessionInf.Count - 1; i++)
            {
                // 別の端末で同一ユーザー名でログインしていないかチェック
                if (sessionInf[i].userId == userId && sessionInf[i].randomString == randomString)
                {
                    return sessionInf[i].db;
                }

                if (CommonUtl.gDemoKbn == 1)
                {
                    return new ExMySQLData(ExCast.zCStr(HttpContext.Current.Session[ExSession.DB_CONNECTION_STR]));
                }
            }

            return null;
        }

        public static string SessionUserUniqueCheck(string random, string session_random, int session_user_id)
        {
            string message = "";
            try
            {
                if (session_random == "")
                {
                    // セッションランダム未設定
                    //return "認証に失敗しました。(エラーコード : 001001)";
                    return "セッションタイムアウトが発生しました。再度ログインして下さい。";
                }

                if (random == "")
                {
                    // ランダム未送信
                    return "認証に失敗しました。(エラーコード : 001001)";
                }

                if (CommonUtl.gDemoKbn == 1)
                {
                    return "";
                }

                if (session_random != random)
                {
                    // ランダム不一致
                    return "認証に失敗しました。(エラーコード : 001002)";
                }

                if (ExSession.CheckSessionInf(session_user_id, session_random, ref message) == false)
                {
                    if (message != "")
                    {
                        return "セッション情報の設定に失敗しました。" + Environment.NewLine +
                               "システム管理者へ報告して下さい。" + Environment.NewLine +
                               message;
                    }
                    else
                    {
                        return "別の端末にて同じログインユーザーIDで" + Environment.NewLine +
                               "ログインされている可能性があります。" + Environment.NewLine +
                               "データの参照・更新が行えません。" + Environment.NewLine +
                               "システム管理者へ報告して下さい。" + Environment.NewLine;
                    }
                }

                if (ExSession.ExistsSessionInf(session_user_id, ref message) == false)
                {
                    if (message != "")
                    {
                        return "セッション情報の設定に失敗しました。" + Environment.NewLine +
                               "システム管理者へ報告して下さい。" + Environment.NewLine +
                               message;
                    }
                    else
                    {
                        return "セッション情報が破棄されています。" + Environment.NewLine +
                               "再度ログイン処理を行ってください。" + Environment.NewLine;
                    }
                }

                return "";

            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".SessionUniqueCheck", ex);
                return "セッション情報の設定に失敗しました。" + Environment.NewLine +
                       "システム管理者へ報告して下さい。" + Environment.NewLine +
                       ex.Message;
            }
        }

        // セッション保持情報のチェック
        public static bool CheckSessionInf(int userId, string randomString, ref string message)
        {
            try
            {
                for (int i = 0; i <= sessionInf.Count - 1; i++)
                {
                    if (sessionInf[i].userId == userId && sessionInf[i].randomString != randomString)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        // セッション保持情報の存在チェック
        public static bool ExistsSessionInf(int userId, ref string message)
        {
            try
            {
                for (int i = 0; i <= sessionInf.Count - 1; i++)
                {
                    if (sessionInf[i].userId == userId)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        // セッション保持情報の存在チェック
        public static bool ExistsSessionInf(int userId, string random, ref string message)
        {
            try
            {
                for (int i = 0; i <= sessionInf.Count - 1; i++)
                {
                    if (sessionInf[i].userId == userId && sessionInf[i].randomString == random)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }

    public class SessionHold
    {
        private int _companyId = 0;
        public int companyId { set { this._companyId = value; } get { return this._companyId; } }

        private int _userId = 0;
        public int userId { set { this._userId = value; } get { return this._userId; } }

        private string _randomString = "";
        public string randomString { set { this._randomString = value; } get { return this._randomString; } }

        private ExMySQLData _db = null;
        public ExMySQLData db { set { this._db = value; } get { return this._db; } }

        public SessionHold(int userId, string randomString, ExMySQLData db)
        {
            this.userId = userId;
            this.randomString = randomString;
            this.db = db;
        }
    }

}