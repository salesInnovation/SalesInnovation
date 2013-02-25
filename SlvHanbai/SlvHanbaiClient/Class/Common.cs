using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;  // ObservableCollectionを利用するために必要   
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.svcSysLogin;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.svcAuthority;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View;
using SlvHanbaiClient.View.Dlg.Report;

namespace SlvHanbaiClient.Class
{
    public class Common
    {
        public static bool gblnLogin = false;
        public static string gstrLoginUserID = "";
        public static string gstrLoginPassword = "";
        public static int gintUserID = 0;
        public static string gstrUserNm = "";
        public static int gintTimeOut = 2;
        public static MeiNameList gNameList;
        public static ObservableCollection<EntityAuthority> gAuthorityList;
        //public static string gstrMainUrl = "http://sales.system-innovation.net";
        //public static string gstrMainUrl = "http://vw01.willnet.ad.jp/SlvHanbai";
        //public static string gstrMainUrl = "http://localhost/SlvHanbai";
        //public static string gstrMainUrl = "https://system-innovation.biz/SlvHanbai";
        public static string gstrMainDomain = "https://system-innovation.biz";
        public static string gstrMainUrl = gstrMainDomain + "/SlvHanbai_demo";
        
        //public static string gstrMainUrl = "https://localhost/SlvHanbai";
        public static string gstrReportDownloadUrl = gstrMainUrl + "/Page/FrmReportDowmLoad.aspx";
        public static string gstrReportDeleteUrl = gstrMainUrl + "/Page/FrmReportDelete.aspx";
        public static string gstrReportViewUrl = gstrMainUrl + "/Page/FrmReportView.aspx";
        public static string gstrFileUploadUrl = gstrMainUrl + "/GenericHandler/FileUpload.ashx";
        public static string gstrManualUrl = gstrMainDomain + "/SlvHanbaiWebOthers/Page/Support/Manual/Manual_ALL.pdf";
        //public static string gstrFileUploadUrl = gstrMainUrl + "/Page/FrmFileUpload.aspx";
        public static bool gblnAppStart = false;        // デザイナでWebサービスが呼ばれないよう
        public static bool gblnDesynchronizeLock = false;           // 非同期処理用排他制御

        // ボタン押下排他制御
        public static bool gblnBtnProcLock = false;
        public static bool gblnBtnDesynchronizeLock = false;        // ボタン押下時非同期入力チェック処理用

        // バージョン管理
        public static string gstrClinetVer = "1.0.1";
        public static string gstrSystemVer = "";

        // ログイン時取得情報
        public static int gintCompanyId = 0;
        public static string gstrCompanyNm = "";
        public static int gintGroupId = 0;
        public static string gstrGroupNm = "";
        public static int gintEstimateApprovalFlg = 1;
        public static int gintReceiptAccountInvoicePringFlg = 1;
        public static int gintDefaultPersonId = 0;
        public static string gstrDefaultPersonNm = "";
        public static string gstrGroupDisplayNm = "";
        public static int gintEvidenceFlg = 0;
        public static int gintidFigureSlipNo = 10;
        public static int gintidFigureCustomer = 10;
        public static int gintidFigurePurchase = 10;
        public static int gintidFigureCommodity = 10;
        public static string gstrSessionString = "";
        public static int gintDemoFlg = 0;

        // 共通処理中ダイアログ用
        public static string gstrProgressDialogTitle = "";
        public static string gstrProgressDialogMsg = "";

        public static string gstrMsgSessionError = "";

        // アプリケーション終了フラグ
        public static bool gblnAppEnd = false;

        // 再ログイン用
        private static ExUserControl utlDummy = new ExUserControl();
        private static ExWebService webService = new ExWebService();
        public static UA_Main _main;
        public static ExChildWindow winParemt;

        public static bool gblnStartSettingDlg = false;

        private static Dlg_DataForm _dataForm = null;
        public static Dlg_DataForm dataForm
        {
            set
            {
                Common._dataForm = value;
            }
            get
            {
                if (Common._dataForm == null)
                {
                    Common._dataForm = new Dlg_DataForm();
                }
                return Common._dataForm;
            }
        }

        private static Dlg_Report _report = null;
        public static Dlg_Report report
        {
            set
            {
                Common._report = value;
            }
            get 
            {
                if (Common._report == null)
                {
                    Common._report = new Dlg_Report();
                }
                return Common._report; 
            }
        }

        private static Dlg_ReportView _reportView = null;
        public static Dlg_ReportView reportView
        {
            set
            {
                Common._reportView = value;
            }
            get
            {
                if (Common._reportView == null)
                {
                    Common._reportView = new Dlg_ReportView();
                }
                return Common._reportView;
            }
        }

        #region 入力系検索ダイアログ

        private static Dlg_InpSearch _InpSearchEstimate = null;
        public static Dlg_InpSearch InpSearchEstimate
        {
            set
            {
                Common._InpSearchEstimate = value;
            }
            get
            {
                if (Common._InpSearchEstimate == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListEstimat;
                    Common._InpSearchEstimate = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;

                }
                return Common._InpSearchEstimate;
            }
        }

        private static Dlg_InpSearch _InpSearchOrder = null;
        public static Dlg_InpSearch InpSearchOrder
        {
            set
            {
                Common._InpSearchOrder = value;
            }
            get
            {
                if (Common._InpSearchOrder == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListOrder;
                    Common._InpSearchOrder = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchOrder;
            }
        }

        private static Dlg_InpSearch _InpSearchSales = null;
        public static Dlg_InpSearch InpSearchSales
        {
            set
            {
                Common._InpSearchSales = value;
            }
            get
            {
                if (Common._InpSearchSales == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListSales;
                    Common._InpSearchSales = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchSales;
            }
        }

        private static Dlg_InpSearch _InpSearchReceipt = null;
        public static Dlg_InpSearch InpSearchReceipt
        {
            set
            {
                Common._InpSearchReceipt = value;
            }
            get
            {
                if (Common._InpSearchReceipt == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListReceipt;
                    Common._InpSearchReceipt = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchReceipt;
            }
        }

        private static Dlg_InpSearch _InpSearchPlan = null;
        public static Dlg_InpSearch InpSearchPlan
        {
            set
            {
                Common._InpSearchPlan = value;
            }
            get
            {
                if (Common._InpSearchPlan == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpDetailReport;
                    Common.gWinType = Common.geWinType.ListCollectPlan;
                    Common._InpSearchPlan = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchPlan;
            }
        }

        private static Dlg_InpSearch _InpSearchInvoice = null;
        public static Dlg_InpSearch InpSearchInvoice
        {
            set
            {
                Common._InpSearchInvoice = value;
            }
            get
            {
                if (Common._InpSearchInvoice == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListInvoice;
                    Common._InpSearchInvoice = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchInvoice;
            }
        }

        private static Dlg_InpSearch _InpSearchPurchaseOrder = null;
        public static Dlg_InpSearch InpSearchPurchaseOrder
        {
            set
            {
                Common._InpSearchPurchaseOrder = value;
            }
            get
            {
                if (Common._InpSearchPurchaseOrder == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListPurchaseOrder;
                    Common._InpSearchPurchaseOrder = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchPurchaseOrder;
            }
        }

        private static Dlg_InpSearch _InpSearchPurchase = null;
        public static Dlg_InpSearch InpSearchPurchase
        {
            set
            {
                Common._InpSearchPurchase = value;
            }
            get
            {
                if (Common._InpSearchPurchase == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListPurchase;
                    Common._InpSearchPurchase = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchPurchase;
            }
        }

        private static Dlg_InpSearch _InpSearchPaymentCash = null;
        public static Dlg_InpSearch InpSearchPaymentCash
        {
            set
            {
                Common._InpSearchPaymentCash = value;
            }
            get
            {
                if (Common._InpSearchPaymentCash == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListPaymentCash;
                    Common._InpSearchPaymentCash = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchPaymentCash;
            }
        }

        private static Dlg_InpSearch _InpSearchPayment = null;
        public static Dlg_InpSearch InpSearchPayment
        {
            set
            {
                Common._InpSearchPayment = value;
            }
            get
            {
                if (Common._InpSearchPayment == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListPayment;
                    Common._InpSearchPayment = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchPayment;
            }
        }

        private static Dlg_InpSearch _InpSearchInOutDelivery = null;
        public static Dlg_InpSearch InpSearchInOutDelivery
        {
            set
            {
                Common._InpSearchInOutDelivery = value;
            }
            get
            {
                if (Common._InpSearchInOutDelivery == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpList;
                    Common.gWinType = Common.geWinType.ListInOutDelivery;
                    Common._InpSearchInOutDelivery = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchInOutDelivery;
            }
        }

        private static Dlg_InpSearch _InpSearchInventory = null;
        public static Dlg_InpSearch InpSearchInventory
        {
            set
            {
                Common._InpSearchInventory = value;
            }
            get
            {
                if (Common._InpSearchInventory == null)
                {
                    Common.geWinGroupType _WinGroupTypeBefore = Common.gWinGroupType;
                    Common.geWinType _WinTypeBefore = Common.gWinType;
                    Common.gWinGroupType = Common.geWinGroupType.InpListReport;
                    Common.gWinType = Common.geWinType.ListInventory;
                    Common._InpSearchInventory = new Dlg_InpSearch();
                    Common.gWinGroupType = _WinGroupTypeBefore;
                    Common.gWinType = _WinTypeBefore;
                }
                return Common._InpSearchInventory;
            }
        }

        #endregion

        // Page判別情報保持
        public static gePageType gPageType;
        public static gePageGroupType gPageGroupType;
        public enum gePageType
        {
            None = 0,
            Install,            // インストール
            Login,              // ログイン
            Menu,               // メニュー
            InpEstimate,        // 見積入力
            InpOrder,           // 受注入力
            InpSales,           // 売上入力
            InpInvoiceClose,    // 請求締処理
            InpReceipt,         // 入金入力
            InpPurchaseOrder,   // 発注入力
            InpPurchase,        // 仕入入力
            InpPaymentClose,    // 支払締処理
            InpPaymentCash,     // 出金入力
            InpInOutDelivery,   // 入出庫入力
            InpStockInventory   // 棚卸入力
        };

        public enum gePageGroupType
        {
            None = 0,
            StartUp,        // 起動
            Menu,           // メニュー
            Inp             // 伝票入力
        };

        // DataForm判別情報保持
        public static geDataFormType gDataFormType;
        public enum geDataFormType
        {
            None = 0,
            OrderDetail,                // 受注明細
            EstimateDetail,             // 見積明細
            SalesDetail,                // 売上明細
            ReceiptDetail,              // 入金明細
            PurchaseOrderDetail,        // 発注明細
            PurchaseDetail,             // 仕入明細
            PaymentCashDetail,          // 出金明細
            InOutDeliveryDetail         // 入出庫明細
        };
        
        // ChildWindow判別情報保持
        public static geWinType gWinType;
        public static geWinGroupType gWinGroupType;
        public static geWinMsterType gWinMsterType;
        public DateTime date;

        public enum geWinType
        {
            None = 0,
            ListEstimat,                    // 見積一覧
            ListOrder,                      // 受注一覧
            ListSales,                      // 売上一覧
            ListSalesDay,                   // 売上日報
            ListSalesMonth,                 // 売上月報
            ListSalesChange,                // 売上推移表
            ListInvoice,                    // 請求一覧
            ListReceipt,                    // 入金一覧
            ListReceiptDay,                 // 入金日報
            ListReceiptMonth,               // 入金月報
            ListCollectPlan,                // 回収予定
            ListSalesCreditBalance,         // 売掛残高一覧
            ListInvoiceBalance,             // 請求残高一覧
            ListPurchaseOrder,              // 発注一覧
            ListPurchase,                   // 仕入一覧
            ListPurchaseDay,                // 仕入日報
            ListPurchaseMonth,              // 仕入月報
            ListPurchaseChange,             // 仕入推移表
            ListPayment,                    // 支払一覧
            ListPaymentCash,                // 出金一覧
            ListPaymentCashDay,             // 出金日報
            ListPaymentCashMonth,           // 出金月報
            ListPaymentPlan,                // 支払予定
            ListPaymentCreditBalance,       // 買掛残高一覧
            ListPaymentBalance,             // 支払残高一覧
            ListInOutDelivery,              // 入出庫一覧
            ListInventory                   // 在庫一覧
        };

        public enum geWinGroupType
        {
            None = 0,
            Report,             // レポート出力
            ReportSetting,      // レポート出力設定
            InpMaster,          // マスタ登録
            InpMasterDetail,    // マスタ登録(明細式)
            InpList,            // 伝票一覧参照
            InpListReport,      // 伝票一覧参照(印刷)
            InpDetailReport,    // 伝票一覧参照(明細表)
            InpListUpd,         // 伝票一覧更新
            NameList,           // 名称一覧参照
            MstList             // マスタ一覧参照
        };
        public enum geWinMsterType
        {
            None = 0,
            Company,        // 基本情報
            CompanyGroup,   // 会社グループ
            User,           // ユーザ
            Person,         // 担当
            Customer,       // 得意先
            Supplier,       // 納入先
            Commodity,      // 商品
            SetCommodity,   // セット商品
            Condition,      // 締区分
            Class,          // 分類
            Authority,      // 権限
            Purchase        // 仕入先
        };

        public enum geTaxChange
        {
            None = 0,
            OUT_TAX_SUM,        // 外税/伝票計
            OUT_TAX_INVOICE,    // 外税/請求時
            IN_TAX_SUM,         // 内税/伝票計
            IN_TAX_INVOICE,     // 内税/請求時
            NO_TAX              // 非課税
        };

        // 更新フラグ
        public enum geUpdateType { Update = 0, Insert , Delete }

        // ログイン
        public static void ReLogin(ExWebService.geDialogDisplayFlg displayFlg, ExWebService.geDialogCloseFlg closeFlg)
        {
            try
            {
                Common.gstrMsgSessionError = "";

                utlDummy.evtDataSelect -= _evtDataSelect;
                utlDummy.evtDataSelect += _evtDataSelect;

                object[] prm = new object[3];
                prm[0] = Common.gstrLoginUserID;
                prm[1] = Common.gstrLoginPassword;
                prm[2] = 0;
                webService.objPerent = utlDummy;
                webService.CallWebService(ExWebService.geWebServiceCallKbn.Login,
                                          displayFlg,
                                          closeFlg,
                                          prm);
            }
            catch
            {
            }
        }

        private static void _evtDataSelect(int intKbn, object objList)
        {
            Common.gstrMsgSessionError = "";

            utlDummy.evtDataSelect -= _evtDataSelect;

            EntitySysLogin entity = null;
            try
            {
                entity = (EntitySysLogin)objList;
            }
            catch
            {
                return;
            }

            switch (entity._login_flg)
            {
                case 0:     // 正常ログイン
                    // システム情報設定
                    Common.gintCompanyId = entity._company_id;
                    Common.gstrCompanyNm = entity._company_nm;
                    Common.gintGroupId = entity._group_id;
                    Common.gstrGroupNm = entity._group_nm;
                    Common.gintDefaultPersonId = entity._defult_person_id;
                    Common.gstrDefaultPersonNm = entity._defult_person_nm;
                    Common.gstrGroupDisplayNm = entity._group_display_name;
                    Common.gintEvidenceFlg = entity._evidence_flg;
                    Common.gintidFigureSlipNo = entity._idFigureSlipNo;
                    Common.gintidFigureCustomer = entity._idFigureCustomer;
                    Common.gintidFigurePurchase = entity._idFigurePurchase;
                    Common.gintidFigureCommodity = entity._idFigureGoods;
                    Common.gintEstimateApprovalFlg = entity._estimate_approval_flg;
                    Common.gintReceiptAccountInvoicePringFlg = entity._receipt_account_invoice_print_flg;
                    Common.gstrSessionString = entity._session_string;
                    Common.gintDemoFlg = entity._demo_flg;
                    Common.gstrSystemVer = entity._sys_ver;
                    Common.gblnLogin = true;

                    if (winParemt != null) winParemt.DataSelect((int)ExWebService.geWebServiceCallKbn.Login, null);

                    break;
                case 1:     // 同一ユーザーログイン
                    if (winParemt != null) winParemt.DataSelect((int)ExWebService.geWebServiceCallKbn.Login, null);

                    break;
                default:    // ログイン失敗
                    if (winParemt != null) winParemt.DataSelect((int)ExWebService.geWebServiceCallKbn.Login, "error");

                    break;
            }

            if (_main != null)
            {
                _main.SetHeaderInf();
            }

            winParemt = null;
        }
    }
}
