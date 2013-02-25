using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlvHanbai.Web.Class
{
    public class ExCertification
    {
        public static string Certification(string sessionRandom, string sendRandom)
        {
            if (sessionRandom == "")
            { 
                // セッションランダム未設定
                return "認証に失敗しました。(エラーコード : 001001)";
            }

            if (sendRandom == "")
            { 
                // ランダム未送信
                return "認証に失敗しました。(エラーコード : 001002)";
            }

            if (sessionRandom != sendRandom)
            {
                // ランダム不一致
                return "認証に失敗しました。(エラーコード : 001003)";
            }

            return "";
        }
    }
}