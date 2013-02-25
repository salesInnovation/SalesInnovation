using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ServiceModel.DomainServices.Client;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcEstimate;
using SlvHanbaiClient.svcSales;
using SlvHanbaiClient.svcInvoiceClose;
using SlvHanbaiClient.svcReceipt;
using SlvHanbaiClient.svcInvoiceBalance;
using SlvHanbaiClient.svcSalesCreditBalance;
using SlvHanbaiClient.svcPurchaseOrder;
using SlvHanbaiClient.svcPurchase;
using SlvHanbaiClient.svcPaymentClose;
using SlvHanbaiClient.svcPaymentCash;
using SlvHanbaiClient.svcPaymentCreditBalance;
using SlvHanbaiClient.svcPaymentBalance;
using SlvHanbaiClient.svcInOutDelivery;
using SlvHanbaiClient.svcStockInventory;
using SlvHanbaiClient.svcSysName;
using SlvHanbaiClient.svcSysLogin;
using SlvHanbaiClient.svcPgEvidence;
using SlvHanbaiClient.svcCompany;
using SlvHanbaiClient.svcCompanyGroup;
using SlvHanbaiClient.svcUser;
using SlvHanbaiClient.svcPerson;
using SlvHanbaiClient.svcCustomer;
using SlvHanbaiClient.svcCommodity;
using SlvHanbaiClient.svcCondition;
using SlvHanbaiClient.svcPgLock;
using SlvHanbaiClient.svcCopying;
using SlvHanbaiClient.svcReport;
using SlvHanbaiClient.svcClass;
using SlvHanbaiClient.svcSupplier;
using SlvHanbaiClient.svcInquiry;
using SlvHanbaiClient.svcDuties;
using SlvHanbaiClient.svcSystemInf;
using SlvHanbaiClient.svcAuthority;
using SlvHanbaiClient.svcPurchaseMst;
using SlvHanbaiClient.View;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.Dlg.Report;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;

namespace SlvHanbaiClient.Class.WebService
{
    public class ExWebService
    {

        #region Filed Enum Const

        private const String CLASS_NM = "ExWebService";

        private string message = "";

        public Dlg_ReportView reportView = null;

        // 呼出元
        public ExUserControl objPerent = null;
        public ExChildWindow objWindow = null;
        public UA_Main objMenu = null;

        // 処理中ダイアログ
        protected Dlg_Progress win = null;

        // 処理中ダイアログ表示フラグ
        public enum geDialogDisplayFlg { Yes = 0, No }
        protected geDialogDisplayFlg DialogDisplayFlg = geDialogDisplayFlg.Yes;

        // 処理中ダイアログクローズフラグ
        public enum geDialogCloseFlg { Yes = 0, No }
        protected geDialogCloseFlg DialogCloseFlg = geDialogCloseFlg.Yes;

        #region Webサービス呼出区分

        public enum geWebServiceCallKbn
        {
            Login = 0, 
            Logoff,

            #region 伝票入力系

            GetOrderList, 
            GetOrderListH, 
            GetOrderListD,
            UpdateOrder,
            UpdateOrderPrint,
            GetEstimateList,
            GetEstimateListH,
            GetEstimateListD,
            UpdateEstimate,
            UpdateEstimatePrint,
            GetSalesList,
            GetSalesListH,
            GetSalesListD,
            UpdateSales,
            UpdateSalesPrint,
            GetInvoiceTotal,
            GetInvoiceList,
            UpdateInvoice,
            UpdateInvoicePrint,
            GetReceiptList,
            GetReceiptListH,
            GetReceiptListD,
            GetPlanList,
            GetInvocieReceipt,
            UpdateReceipt,

            GetPurchaseOrderList,
            GetPurchaseOrderListH,
            GetPurchaseOrderListD,
            UpdatePurchaseOrder,
            UpdatePurchaseOrderPrint,
            GetPurchaseList,
            GetPurchaseListH,
            GetPurchaseListD,
            UpdatePurchase,
            GetPaymentTotal,
            GetPaymentList,
            UpdatePayment,
            UpdatePaymentPrint,
            GetPaymentCashList,
            GetPaymentCashListH,
            GetPaymentCashListD,
            GetPaymentCashOut,
            UpdatePaymentCash,

            GetInOutDeliveryList,
            GetInOutDeliveryListH,
            GetInOutDeliveryListD,
            UpdateInOutDelivery,
            GetStockInventoryList,
            UpdateStockInventory,
                        
            #endregion

            #region マスタ系

            GetCompany,
            UpdateCompany,
            GetCompanyGroup,
            UpdateCompanyGroup,
            GetUser,
            UpdateUser,
            GetPerson,
            UpdatePerson,
            GetCustomer,
            UpdateCustomer,
            GetSupplier,
            UpdateSupplier,
            GetCommodity,
            UpdateCommodity,
            GetSetCommodity,
            UpdateSetCommodity,
            GetCondition,
            UpdateCondition,
            GetClass,
            UpdateClass,
            GetAuthority,
            UpdateAuthority,
            GetInvoiceBalanceList,
            UpdateInvoiceBalance,
            GetSalesCreditBalanceList,
            UpdateSalesCreditBalance,
            GetPaymentCreditBalanceList,
            UpdatePaymentCreditBalance,
            GetPaymentBalanceList,
            UpdatePaymentBalance,
            GetPurchaseMst,
            UpdatePurchaseMst,
            
            #endregion

            GetNameList,
            GetReportSetting,
            UpdateReportSetting,
            GetInquiry,
            UpdateInquiry,
            GetDuties,
            GetSystemInf,
            UpdateDuties,
            AddEvidence,
            CopyCheck,
            ReportOut,
            LockPg
        }

        #endregion

        public geWebServiceCallKbn gWebServiceCallKbn;

        #region 取得データリスト

        private EntitySysLogin objLogin;　　                                                          // ログイン情報

        #region 伝票入力系

        private ObservableCollection<EntityOrder> objOrderList;　　                                   // 受注リスト
        private EntityOrderH objOrderH;　　                                                           // 受注ヘッダ
        private ObservableCollection<EntityOrderD> objOrderListD;　　                                 // 受注明細リスト
        private ObservableCollection<EntityEstimate> objEstimateList;　　                             // 見積リスト
        private EntityEstimateH objEstimateH;　　                                                     // 見積ヘッダ
        private ObservableCollection<EntityEstimateD> objEstimateListD;　　                           // 見積明細リスト
        private ObservableCollection<EntitySales> objSalesList;　　                                   // 売上リスト
        private EntitySalesH objSalesH;　　                                                           // 売上ヘッダ
        private ObservableCollection<EntitySalesD> objSalesListD;　　                                 // 売上明細リスト
        private ObservableCollection<EntityInvoiceClose> objInvoiceCloseList;　　                     // 請求締リスト
        private ObservableCollection<EntityInvoiceClose> objInvoiceList;　　                          // 請求リスト
        private ObservableCollection<EntityReceipt> objReceiptList;　　                               // 入金リスト
        private EntityReceiptH objReceiptH;　　                                                       // 入金ヘッダ
        private ObservableCollection<EntityReceiptD> objReceiptListD;　　                             // 入金明細リスト
        private EntityInvoiceReceipt objInvocieReceipt;　　                                           // 請求入金

        private ObservableCollection<EntityPurchaseOrder> objPurchaseOrderList;　　                   // 発注リスト
        private EntityPurchaseOrderH objPurchaseOrderH;　　                                           // 発注ヘッダ
        private ObservableCollection<EntityPurchaseOrderD> objPurchaseOrderListD;　　                 // 発注明細リスト
        private ObservableCollection<EntityPurchase> objPurchaseList;　　                             // 仕入リスト
        private EntityPurchaseH objPurchaseH;　　                                                     // 仕入ヘッダ
        private ObservableCollection<EntityPurchaseD> objPurchaseListD;　　                           // 仕入明細リスト
        private ObservableCollection<EntityPaymentClose> objPaymentCloseList;　　                     // 支払締リスト
        private ObservableCollection<EntityPaymentClose> objPaymentList;　　                          // 支払リスト
        private ObservableCollection<EntityPaymentCash> objPaymentCashList;　　                       // 出金リスト
        private EntityPaymentCashH objPaymentCashH;　　                                               // 出金ヘッダ
        private ObservableCollection<EntityPaymentCashD> objPaymentCashListD;　　                     // 出金明細リスト
        private EntityPaymentCashOut objPaymentCashOut;　　                                           // 支払出金

        private ObservableCollection<EntityInOutDelivery> objInOutDeliveryList;　　                   // 入出庫リスト
        private EntityInOutDeliveryH objInOutDeliveryH;　　                                           // 入出庫ヘッダ
        private ObservableCollection<EntityInOutDeliveryD> objInOutDeliveryListD;　　                 // 入出庫明細リスト
        private ObservableCollection<EntityStockInventory> objStockInventoryList;                     // 棚卸リスト
        
        #endregion

        #region マスタ系

        private ObservableCollection<EntityInvoiceBalance> objInvoiceBalanceList;                     // 請求残高リスト
        private ObservableCollection<EntitySalesCreditBalance> objSalesCreditBalanceList;　　         // 売掛残高リスト
        private ObservableCollection<EntityPaymentBalance> objPaymentBalanceList;                     // 支払残高リスト
        private ObservableCollection<EntityPaymentCreditBalance> objPaymentCreditBalanceList;　　     // 買掛残高リスト
        private EntityCompany objCompany;　　                                                         // 会社マスタ
        private EntityCompanyGroup objCompanyGroup;　　                                               // 会社グループマスタ
        private EntityUser objUser;　　                                                               // ユーザマスタ
        private EntityPerson objPerson;　　                                                           // 担当マスタ
        private EntityCustomer objCustomer;　　                                                       // 得意先マスタ
        private EntityCommodity objCommodity;　　                                                     // 商品マスタ
        private ObservableCollection<EntityCondition> objCondition;　　                               // 締区分マスタ
        private ObservableCollection<EntityClass> objClass;　　                                       // 分類マスタ
        private EntitySupplier objSupplier;　　                                                       // 納入先マスタ
        private ObservableCollection<EntityAuthority> objAuthority;　　                               // 権限マスタ
        private EntityPurchaseMst objPurchaseMst;　　                                                 // 仕入先マスタ

        #endregion

        private ObservableCollection<EntityName> objNameList;　　                                     // 名称マスタリスト
        private ObservableCollection<EntityInquiry> objInquiryList;　　                               // 問い合わせリスト
        private ObservableCollection<svcDuties.EntityDuties> objDutiesList;　　                       // 業務連絡リスト
        private ObservableCollection<svcSystemInf.EntityDuties> objSystemInfList;               　    // システムからの連絡リスト
        private EntityCopying objCopying;　　                                                         // 複写情報
        private EntityReport objReport;　　                                                           // レポート出力情報
        private EntityReportSetting objReportSetting;　　                                             // レポート設定情報
        private object[] _prmList;

        #endregion

        #endregion

        #region Method

        #region Web Service Call

        // ダイアログ表示
        public void ProcessingDlgShow()
        {
            win = new Dlg_Progress();
            win.Show();
        }

        // ダイアログ閉じる
        public void ProcessingDlgClose()
        {
            try
            {
                Common.gblnDesynchronizeLock = false;

                if (win != null)
                {
                    win.Close();
                    win = null;
                }
            }
            catch
            { 
                // ダイアログが開いてなくてエラーが発生しても握りつぶす
            }

            try
            {
                // システム終了時のエラー発生時
                if (reportView != null)
                {
                    reportView.Close();
                } 
            }
            catch
            { 
            }

            try
            {
                // システム終了時のエラー発生時
                if (Common.gblnAppEnd == true && Application.Current.IsRunningOutOfBrowser)
                {
                    Application.Current.MainWindow.Close();
                } 

            }
            catch
            { 
            }            
        }

        // WebサービスCall(データ参照時)
        public void CallWebService(geWebServiceCallKbn callKbn, 
                                   geDialogDisplayFlg dialogDisplayFlg,
                                   geDialogCloseFlg dialogCloseFlg,
                                   object[] prmList)
        {
            try
            {
                if (Common.gblnAppStart == false)
                {
                    return;
                }

                DialogCloseFlg = dialogCloseFlg;
                DialogDisplayFlg = dialogDisplayFlg;
                gWebServiceCallKbn = callKbn;
                _prmList = prmList;

                // Web Service Call
                switch (gWebServiceCallKbn)
                {
                    #region ログイン

                    case geWebServiceCallKbn.Login:
                        Login(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]), ExCast.zCInt(_prmList[2]));
                        break;
                    case geWebServiceCallKbn.Logoff:
                        Logoff();
                        break;

                    #endregion

                    #region 伝票入力

                    #region 見積

                    case geWebServiceCallKbn.GetEstimateList:
                        GetEstimateList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetEstimateListH:
                        GetEstimateH(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetEstimateListD:
                        GetEstimateListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateEstimate:
                        UpdateEstimate(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityEstimateH)_prmList[2], (ObservableCollection<EntityEstimateD>)_prmList[3]);
                        break;
                    case geWebServiceCallKbn.UpdateEstimatePrint:
                        UpdateEstimatePrint(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;

                    #endregion

                    #region 受注

                    case geWebServiceCallKbn.GetOrderList:
                        GetOrderList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetOrderListH:
                        GetOrderH(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetOrderListD:
                        GetOrderListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateOrder:
                        UpdateOrder(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityOrderH)_prmList[2], (ObservableCollection<EntityOrderD>)_prmList[3]);
                        break;
                    case geWebServiceCallKbn.UpdateOrderPrint:
                        UpdateOrderPrint(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;

                    #endregion

                    #region 売上

                    case geWebServiceCallKbn.GetSalesList:
                        GetSalesList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetSalesListH:
                        GetSalesH(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetSalesListD:
                        GetSalesListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateSales:
                        UpdateSales(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntitySalesH)_prmList[2], (ObservableCollection<EntitySalesD>)_prmList[3], (EntitySalesH)_prmList[4], (ObservableCollection<EntitySalesD>)_prmList[5]);
                        break;
                    case geWebServiceCallKbn.UpdateSalesPrint:
                        UpdateSalesPrint(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;

                    #endregion

                    #region 請求

                    case geWebServiceCallKbn.GetInvoiceTotal:
                        GetInvoiceTotal((EntityInvoiceClosePrm)_prmList[0]);
                        break;
                    case geWebServiceCallKbn.GetInvoiceList:
                        GetInvoiceList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdateInvoice:
                        UpdateInvoice(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityInvoiceClose>)_prmList[1]);
                        break;
                    case geWebServiceCallKbn.UpdateInvoicePrint:
                        UpdateInvoicePrint(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityInvoiceClose>)_prmList[1]);
                        break;

                    #endregion

                    #region 入金

                    case geWebServiceCallKbn.GetReceiptList:
                        GetReceiptList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetReceiptListH:
                        GetReceiptH(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.GetReceiptListD:
                        GetReceiptListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.GetInvocieReceipt:
                        GetInvoiceReceipt(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdateReceipt:
                        UpdateReceipt(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityReceiptH)_prmList[2], (ObservableCollection<EntityReceiptD>)_prmList[3], (EntityReceiptH)_prmList[4]);
                        break;

                    #endregion

                    #region 請求残高

                    case geWebServiceCallKbn.GetInvoiceBalanceList:
                        GetInvoiceBalanceList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdateInvoiceBalance:
                        UpdateInvoiceBalance(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityInvoiceBalance>)_prmList[1]);
                        break;

                    #endregion

                    #region 売掛残高

                    case geWebServiceCallKbn.GetSalesCreditBalanceList:
                        GetSalesCreditBalanceList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdateSalesCreditBalance:
                        UpdateSalesCreditBalance(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntitySalesCreditBalance>)_prmList[1]);
                        break;

                    #endregion

                    #region 発注

                    case geWebServiceCallKbn.GetPurchaseOrderList:
                        GetPurchaseOrderList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetPurchaseOrderListH:
                        GetPurchaseOrderH(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetPurchaseOrderListD:
                        GetPurchaseOrderListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdatePurchaseOrder:
                        UpdatePurchaseOrder(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityPurchaseOrderH)_prmList[2], (ObservableCollection<EntityPurchaseOrderD>)_prmList[3]);
                        break;
                    case geWebServiceCallKbn.UpdatePurchaseOrderPrint:
                        UpdatePurchaseOrderPrint(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;

                    #endregion

                    #region 仕入

                    case geWebServiceCallKbn.GetPurchaseList:
                        GetPurchaseList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetPurchaseListH:
                        GetPurchaseH(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetPurchaseListD:
                        GetPurchaseListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdatePurchase:
                        UpdatePurchase(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityPurchaseH)_prmList[2], (ObservableCollection<EntityPurchaseD>)_prmList[3], (EntityPurchaseH)_prmList[4], (ObservableCollection<EntityPurchaseD>)_prmList[5]);
                        break;
                    //case geWebServiceCallKbn.UpdateSalesPrint:
                    //    UpdateSalesPrint(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]));
                    //    break;

                    #endregion

                    #region 支払

                    case geWebServiceCallKbn.GetPaymentTotal:
                        GetPaymentTotal((EntityPaymentClosePrm)_prmList[0]);
                        break;
                    case geWebServiceCallKbn.GetPaymentList:
                        GetPaymentList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdatePayment:
                        UpdatePayment(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityPaymentClose>)_prmList[1]);
                        break;
                    case geWebServiceCallKbn.UpdatePaymentPrint:
                        UpdatePaymentPrint(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityPaymentClose>)_prmList[1]);
                        break;

                    #endregion

                    #region 出金

                    case geWebServiceCallKbn.GetPaymentCashList:
                        GetPaymentCashList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetPaymentCashListH:
                        GetPaymentCashH(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.GetPaymentCashListD:
                        GetPaymentCashListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.GetPaymentCashOut:
                        GetPaymentCashOut(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdatePaymentCash:
                        UpdatePaymentCash(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityPaymentCashH)_prmList[2], (ObservableCollection<EntityPaymentCashD>)_prmList[3], (EntityPaymentCashH)_prmList[4]);
                        break;

                    #endregion

                    #region 支払残高

                    case geWebServiceCallKbn.GetPaymentBalanceList:
                        GetPaymentBalanceList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdatePaymentBalance:
                        UpdatePaymentBalance(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityPaymentBalance>)_prmList[1]);
                        break;

                    #endregion

                    #region 買掛残高

                    case geWebServiceCallKbn.GetPaymentCreditBalanceList:
                        GetPaymentCreditBalanceList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdatePaymentCreditBalance:
                        UpdatePaymentCreditBalance(ExCast.zCInt(_prmList[0]), (ObservableCollection<EntityPaymentCreditBalance>)_prmList[1]);
                        break;

                    #endregion

                    #region 入出庫

                    case geWebServiceCallKbn.GetInOutDeliveryList:
                        GetInOutDeliveryList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetInOutDeliveryListH:
                        GetInOutDeliveryH(ExCast.zCLng(_prmList[0]), ExCast.zCLng(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.GetInOutDeliveryListD:
                        GetInOutDeliveryListD(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateInOutDelivery:
                        UpdateInOutDelivery(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityInOutDeliveryH)_prmList[2], (ObservableCollection<EntityInOutDeliveryD>)_prmList[3], (EntityInOutDeliveryH)_prmList[4], (ObservableCollection<EntityInOutDeliveryD>)_prmList[5]);
                        break;

                    #endregion

                    #region 棚卸

                    case geWebServiceCallKbn.GetStockInventoryList:
                        GetStockInventoryList(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdateStockInventory:
                        UpdateStockInventory(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]), (ObservableCollection<EntityStockInventory>)_prmList[2]);
                        break;

                    #endregion


                    #endregion

                    #region マスタ

                    #region 名称

                    case geWebServiceCallKbn.GetNameList:
                        GetNameList();
                        break;

                    #endregion

                    #region 会社

                    case geWebServiceCallKbn.GetCompany:
                        GetCompany();
                        break;
                    case geWebServiceCallKbn.UpdateCompany:
                        UpdateCompany(ExCast.zCInt(_prmList[0]), (EntityCompany)_prmList[1]);
                        break;

                    #endregion

                    #region 会社グループ

                    case geWebServiceCallKbn.GetCompanyGroup:
                        GetCompanyGroup(ExCast.zCInt(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateCompanyGroup:
                        UpdateCompanyGroup(ExCast.zCInt(_prmList[0]), ExCast.zCInt(_prmList[1]), (EntityCompanyGroup)_prmList[2]);
                        break;

                    #endregion

                    #region ユーザ

                    case geWebServiceCallKbn.GetUser:
                        GetUser(ExCast.zCInt(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateUser:
                        UpdateUser(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityUser)_prmList[2]);
                        break;

                    #endregion

                    #region 担当

                    case geWebServiceCallKbn.GetPerson:
                        GetPerson(ExCast.zCInt(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdatePerson:
                        UpdatePerson(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityPerson)_prmList[2]);
                        break;

                    #endregion

                    #region 得意先

                    case geWebServiceCallKbn.GetCustomer:
                        GetCustomer(ExCast.zCStr(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateCustomer:
                        UpdateCustomer(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]), (EntityCustomer)_prmList[2]);
                        break;

                    #endregion

                    #region 商品

                    case geWebServiceCallKbn.GetCommodity:
                        GetCommodity(ExCast.zCStr(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateCommodity:
                        UpdateCommodity(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]), (EntityCommodity)_prmList[2]);
                        break;

                    #endregion

                    #region 締区分

                    case geWebServiceCallKbn.GetCondition:
                        GetCondition();
                        break;
                    case geWebServiceCallKbn.UpdateCondition:
                        UpdateCondition((ObservableCollection<EntityCondition>)_prmList[0]);
                        break;

                    #endregion

                    #region 分類

                    case geWebServiceCallKbn.GetClass:
                        GetClass(ExCast.zCInt(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateClass:
                        UpdateClass((ObservableCollection<EntityClass>)_prmList[0], ExCast.zCInt(_prmList[1]));
                        break;

                    #endregion

                    #region 納入先

                    case geWebServiceCallKbn.GetSupplier:
                        GetSupplier(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.UpdateSupplier:
                        UpdateSupplier(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]), ExCast.zCStr(_prmList[2]), (EntitySupplier)_prmList[3]);
                        break;

                    #endregion

                    #region 権限

                    case geWebServiceCallKbn.GetAuthority:
                        GetAuthority(ExCast.zCInt(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateAuthority:
                        UpdateAuthority((ObservableCollection<EntityAuthority>)_prmList[0], ExCast.zCInt(_prmList[1]));
                        break;

                    #endregion

                    #region レポート設定

                    case geWebServiceCallKbn.GetReportSetting:
                        GetReportSetting(ExCast.zCStr(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateReportSetting:
                        UpdateReportSetting(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]), (EntityReportSetting)_prmList[2]);
                        break;

                    #endregion

                    #region 仕入先

                    case geWebServiceCallKbn.GetPurchaseMst:
                        GetPurchaseMst(ExCast.zCStr(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdatePurchaseMst:
                        UpdatePurchaseMst(ExCast.zCInt(_prmList[0]), ExCast.zCStr(_prmList[1]), (EntityPurchaseMst)_prmList[2]);
                        break;

                    #endregion

                    #endregion

                    #region 証跡

                    case geWebServiceCallKbn.AddEvidence:
                        AddEvidence(ExCast.zCStr(_prmList[0]), ExCast.zCInt(_prmList[1]), ExCast.zCStr(_prmList[2]));
                        break;

                    #endregion

                    #region 排他制御

                    case geWebServiceCallKbn.LockPg:
                        LockPg(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]), ExCast.zCInt(_prmList[2]));
                        break;

                    #endregion

                    #region サポート

                    #region 問い合わせ

                    case geWebServiceCallKbn.GetInquiry:
                        GetInquiry(ExCast.zCLng(_prmList[0]));
                        break;
                    case geWebServiceCallKbn.UpdateInquiry:
                        UpdateInquiry(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (EntityInquiry)_prmList[2]);
                        break;

                    #endregion

                    #endregion

                    #region 業務連絡

                    case geWebServiceCallKbn.GetDuties:
                        if (_prmList.Length == 1)
                        {
                            GetDuties(ExCast.zCLng(_prmList[0]), 0);
                        }
                        else
                        {
                            GetDuties(ExCast.zCLng(_prmList[0]), ExCast.zCInt(_prmList[1]));
                        }
                        break;
                    case geWebServiceCallKbn.UpdateDuties:
                        UpdateDuties(ExCast.zCInt(_prmList[0]), ExCast.zCLng(_prmList[1]), (svcDuties.EntityDuties)_prmList[2]);
                        break;

                    #endregion

                    #region システムからの連絡

                    case geWebServiceCallKbn.GetSystemInf:
                        if (_prmList.Length == 1)
                        {
                            GetSystemInf(ExCast.zCLng(_prmList[0]), 0);
                        }
                        else
                        {
                            GetSystemInf(ExCast.zCLng(_prmList[0]), ExCast.zCInt(_prmList[1]));
                        }
                        break;

                    #endregion

                    case geWebServiceCallKbn.CopyCheck:
                        CopyCheck(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]));
                        break;
                    case geWebServiceCallKbn.ReportOut:
                        ReportOut(ExCast.zCStr(_prmList[0]), ExCast.zCStr(_prmList[1]), ExCast.zCStr(_prmList[2]));
                        break;
                    default:
                        return;
                }

                // 処理中ダイアログ表示
                if (DialogDisplayFlg == geDialogDisplayFlg.Yes)
                {
                    if (win == null) { win = new Dlg_Progress(); }
                    win.Show();
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".CallWebService" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #region ログイン

        private void Login(string loginId, string password, int confirmFlg)
        {
            try
            {
                objLogin = null;   // 初期化
                svcSysLoginClient svc = new svcSysLoginClient();
                svc.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(this.LoginCompleted);
                svc.LoginAsync(loginId, password, confirmFlg);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".Login" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void LoginCompleted(Object sender, LoginCompletedEventArgs e)
        {
            try
            {
                objLogin = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
                objPerent.DataSelect((int)geWebServiceCallKbn.Login, (object)objLogin);

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".LoginCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region ログオフ

        private void Logoff()
        {
            try
            {
                objLogin = null;   // 初期化
                svcSysLoginClient svc = new svcSysLoginClient();
                svc.LogoffCompleted += new EventHandler<LogoffCompletedEventArgs>(this.LogoffCompleted);
                svc.LogoffAsync(Common.gstrSessionString);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".Logoff" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void LogoffCompleted(Object sender, LogoffCompletedEventArgs e)
        {
            try
            {
                bool ret = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
                objMenu.DataInsert((int)geWebServiceCallKbn.Logoff);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".LogoffCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 伝票入力系

        #region 受注

        #region 受注リスト取得

        private void GetOrderList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objOrderList = null;   // 初期化
                svcOrderClient svc = new svcOrderClient();
                svc.GetOrderListCompleted += new EventHandler<GetOrderListCompletedEventArgs>(this.GetOrderListCompleted);
                svc.GetOrderListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetOrderListCompleted(Object sender, GetOrderListCompletedEventArgs e)
        {
            try
            {
                objOrderList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objOrderList != null)
                {
                    if (objOrderList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderList, null);
                        return;
                    }

                    if (objOrderList[0].message != null)
                    {
                        if (objOrderList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objOrderList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderList, (object)objOrderList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderList, (object)objOrderList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 受注ヘッダ取得

        private void GetOrderH(long OrderNoFrom, long OrderNoTo)
        {
            try
            {
                objOrderH = null;   // 初期化
                svcOrderClient svc = new svcOrderClient();
                svc.GetOrderHCompleted += new EventHandler<GetOrderHCompletedEventArgs>(this.GetOrderHCompleted);
                svc.GetOrderHAsync(Common.gstrSessionString, OrderNoFrom, OrderNoTo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetOrderHCompleted(Object sender, GetOrderHCompletedEventArgs e)
        {
            try
            {
                objOrderH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objOrderH != null)
                {
                    if (objOrderH._message != "" && objOrderH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objOrderH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderListH, (object)objOrderH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderListH, (object)objOrderH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 受注明細リスト取得

        private void GetOrderListD(long OrderNo)
        {
            try
            {
                objOrderListD = null;   // 初期化
                svcOrderClient svc = new svcOrderClient();
                svc.GetOrderListDCompleted += new EventHandler<GetOrderListDCompletedEventArgs>(this.GetOrderListDCompleted);
                svc.GetOrderListDAsync(Common.gstrSessionString, OrderNo, OrderNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetOrderListDCompleted(Object sender, GetOrderListDCompletedEventArgs e)
        {
            try
            {
                objOrderListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objOrderListD != null)
                {
                    if (objOrderListD[0]._message != "" && objOrderListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objOrderListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderListD, (object)objOrderListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetOrderListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 受注更新

        private void UpdateOrder(int type, long OrderNo, EntityOrderH entityH, ObservableCollection<EntityOrderD> entityD)
        {
            try
            {
                svcOrderClient svc = new svcOrderClient();
                svc.UpdateOrderCompleted += new EventHandler<UpdateOrderCompletedEventArgs>(this.UpdateOrderCompleted);
                svc.UpdateOrderAsync(Common.gstrSessionString, type, OrderNo, entityH, entityD);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateOrder" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateOrderCompleted(Object sender, UpdateOrderCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateOrder, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateOrder, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateOrder, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetOrderListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 注文請書発行更新

        private void UpdateOrderPrint(int type, string _no)
        {
            try
            {
                svcOrderClient svc = new svcOrderClient();
                svc.UpdateOrderPrintAsync(Common.gstrSessionString, type, _no);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateOrderPrint" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #endregion

        #region 見積

        #region 見積リスト取得

        private void GetEstimateList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objEstimateList = null;   // 初期化
                svcEstimateClient svc = new svcEstimateClient();
                svc.GetEstimateListCompleted += new EventHandler<GetEstimateListCompletedEventArgs>(this.GetEstimateListCompleted);
                svc.GetEstimateListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetEstimateListCompleted(Object sender, GetEstimateListCompletedEventArgs e)
        {
            try
            {
                objEstimateList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objEstimateList != null)
                {
                    if (objEstimateList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateList, null);
                        return;
                    }

                    if (objEstimateList[0].message != null)
                    {
                        if (objEstimateList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objEstimateList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateList, (object)objEstimateList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateList, (object)objEstimateList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 見積ヘッダ取得

        private void GetEstimateH(long EstimateNoFrom, long EstimateNoTo)
        {
            try
            {
                objEstimateH = null;   // 初期化
                svcEstimateClient svc = new svcEstimateClient();
                svc.GetEstimateHCompleted += new EventHandler<GetEstimateHCompletedEventArgs>(this.GetEstimateHCompleted);
                svc.GetEstimateHAsync(Common.gstrSessionString, EstimateNoFrom, EstimateNoTo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetEstimateHCompleted(Object sender, GetEstimateHCompletedEventArgs e)
        {
            try
            {
                objEstimateH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objEstimateH != null)
                {
                    if (objEstimateH._message != "" && objEstimateH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objEstimateH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateListH, (object)objEstimateH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateListH, (object)objEstimateH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 見積明細リスト取得

        private void GetEstimateListD(long EstimateNo)
        {
            try
            {
                objEstimateListD = null;   // 初期化
                svcEstimateClient svc = new svcEstimateClient();
                svc.GetEstimateListDCompleted += new EventHandler<GetEstimateListDCompletedEventArgs>(this.GetEstimateListDCompleted);
                svc.GetEstimateListDAsync(Common.gstrSessionString, EstimateNo, EstimateNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetEstimateListDCompleted(Object sender, GetEstimateListDCompletedEventArgs e)
        {
            try
            {
                objEstimateListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objEstimateListD != null)
                {
                    if (objEstimateListD[0]._message != "" && objEstimateListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objEstimateListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateListD, (object)objEstimateListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetEstimateListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 見積更新

        private void UpdateEstimate(int type, long EstimateNo, EntityEstimateH entityH, ObservableCollection<EntityEstimateD> entityD)
        {
            try
            {
                svcEstimateClient svc = new svcEstimateClient();
                svc.UpdateEstimateCompleted += new EventHandler<UpdateEstimateCompletedEventArgs>(this.UpdateEstimateCompleted);
                svc.UpdateEstimateAsync(Common.gstrSessionString, type, EstimateNo, entityH, entityD);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateEstimate" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateEstimateCompleted(Object sender, UpdateEstimateCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateEstimate, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateEstimate, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateEstimate, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetEstimateListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 見積書発行更新

        private void UpdateEstimatePrint(int type, string _no)
        {
            try
            {
                svcEstimateClient svc = new svcEstimateClient();
                svc.UpdateEstimatePrintAsync(Common.gstrSessionString, type, _no);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateEstimatePrint" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #endregion

        #region 売上

        #region 売上リスト取得

        private void GetSalesList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objSalesList = null;   // 初期化
                svcSalesClient svc = new svcSalesClient();
                svc.GetSalesListCompleted += new EventHandler<GetSalesListCompletedEventArgs>(this.GetSalesListCompleted);
                svc.GetSalesListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSalesListCompleted(Object sender, GetSalesListCompletedEventArgs e)
        {
            try
            {
                objSalesList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objSalesList != null)
                {
                    if (objSalesList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesList, null);
                        return;
                    }

                    if (objSalesList[0].message != null)
                    {
                        if (objSalesList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objSalesList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesList, (object)objSalesList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesList, (object)objSalesList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 売上ヘッダ取得

        private void GetSalesH(long SalesNoFrom, long SalesNoTo)
        {
            try
            {
                objSalesH = null;   // 初期化
                svcSalesClient svc = new svcSalesClient();
                svc.GetSalesHCompleted += new EventHandler<GetSalesHCompletedEventArgs>(this.GetSalesHCompleted);
                svc.GetSalesHAsync(Common.gstrSessionString, SalesNoFrom, SalesNoTo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSalesHCompleted(Object sender, GetSalesHCompletedEventArgs e)
        {
            try
            {
                objSalesH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objSalesH != null)
                {
                    if (objSalesH._message != "" && objSalesH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objSalesH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesListH, (object)objSalesH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesListH, (object)objSalesH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 売上明細リスト取得

        private void GetSalesListD(long SalesNo)
        {
            try
            {
                objSalesListD = null;   // 初期化
                svcSalesClient svc = new svcSalesClient();
                svc.GetSalesListDCompleted += new EventHandler<GetSalesListDCompletedEventArgs>(this.GetSalesListDCompleted);
                svc.GetSalesListDAsync(Common.gstrSessionString, SalesNo, SalesNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSalesListDCompleted(Object sender, GetSalesListDCompletedEventArgs e)
        {
            try
            {
                objSalesListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objSalesListD != null)
                {
                    if (objSalesListD[0]._message != "" && objSalesListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objSalesListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesListD, (object)objSalesListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 売上更新

        private void UpdateSales(int type, long SalesNo, EntitySalesH entityH, ObservableCollection<EntitySalesD> entityD, EntitySalesH before_entityH, ObservableCollection<EntitySalesD> before_entityD)
        {
            try
            {
                svcSalesClient svc = new svcSalesClient();
                svc.UpdateSalesCompleted += new EventHandler<UpdateSalesCompletedEventArgs>(this.UpdateSalesCompleted);
                svc.UpdateSalesAsync(Common.gstrSessionString, type, SalesNo, entityH, entityD, before_entityH, before_entityD);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateSales" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateSalesCompleted(Object sender, UpdateSalesCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSales, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSales, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSales, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 納品書発行更新

        private void UpdateSalesPrint(int type, string _no)
        {
            try
            {
                svcSalesClient svc = new svcSalesClient();
                svc.UpdateSalesPrintAsync(Common.gstrSessionString, type, _no);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateSalesPrint" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #endregion

        #region 請求

        #region 請求締集計

        private void GetInvoiceTotal(EntityInvoiceClosePrm entityPrm)
        {
            try
            {
                objClass = null;   // 初期化
                svcInvoiceCloseClient svc = new svcInvoiceCloseClient();
                svc.GetInvoiceTotalCompleted += new EventHandler<GetInvoiceTotalCompletedEventArgs>(this.GetInvoiceTotalCompleted);
                svc.GetInvoiceTotalAsync(Common.gstrSessionString, entityPrm);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceTotal" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInvoiceTotalCompleted(Object sender, GetInvoiceTotalCompletedEventArgs e)
        {
            try
            {
                objInvoiceCloseList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInvoiceCloseList != null)
                {
                    if (objInvoiceCloseList.Count == 0)
                    {
                        ExMessageBox.Show("締対象データが存在しません。");
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceTotal, null);
                        return;
                    }
                }

                if (objInvoiceCloseList != null)
                {
                    if (objInvoiceCloseList[0].message != "" && objInvoiceCloseList[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objInvoiceCloseList[0].message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceTotal, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceTotal, (object)objInvoiceCloseList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceTotal, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceTotalCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 請求リスト取得

        private void GetInvoiceList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objInvoiceList = null;   // 初期化
                svcInvoiceCloseClient svc = new svcInvoiceCloseClient();
                svc.GetInvoiceListCompleted += new EventHandler<GetInvoiceListCompletedEventArgs>(this.GetInvoiceListCompleted);
                svc.GetInvoiceListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInvoiceListCompleted(Object sender, GetInvoiceListCompletedEventArgs e)
        {
            try
            {
                objInvoiceList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInvoiceList != null)
                {
                    if (objInvoiceList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceList, null);
                        return;
                    }

                    if (objInvoiceList[0].message != null)
                    {
                        if (objInvoiceList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objInvoiceList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceList, (object)objInvoiceList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceList, (object)objInvoiceList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 請求締処理

        private void UpdateInvoice(int type, ObservableCollection<EntityInvoiceClose> entity)
        {
            try
            {
                svcInvoiceCloseClient svc = new svcInvoiceCloseClient();
                svc.UpdateInvoiceCompleted += new EventHandler<UpdateInvoiceCompletedEventArgs>(this.UpdateInvoiceCompleted);
                svc.UpdateInvoiceAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInvoice" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateInvoiceCompleted(Object sender, UpdateInvoiceCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    ExMessageBox.Show(message);

                    if (message.IndexOf("請求締切を実行しました。") != -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoice, "");
                    }
                    else if (message.IndexOf("削除しました。") != -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoice, "");
                    }
                    else
                    {
                        // 失敗
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoice, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoice, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInvoiceCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 請求発行更新

        private void UpdateInvoicePrint(int type, ObservableCollection<EntityInvoiceClose> entity)
        {
            try
            {
                svcInvoiceCloseClient svc = new svcInvoiceCloseClient();
                svc.UpdateInvoicePrintAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInvoicePrint" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #endregion

        #region 入金

        #region 入金リスト取得

        private void GetReceiptList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objReceiptList = null;   // 初期化
                svcReceiptClient svc = new svcReceiptClient();
                svc.GetReceiptListCompleted += new EventHandler<GetReceiptListCompletedEventArgs>(this.GetReceiptListCompleted);
                svc.GetReceiptListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetReceiptListCompleted(Object sender, GetReceiptListCompletedEventArgs e)
        {
            try
            {
                objReceiptList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objReceiptList != null)
                {
                    if (objReceiptList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptList, null);
                        return;
                    }

                    if (objReceiptList[0].message != null)
                    {
                        if (objReceiptList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objReceiptList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptList, (object)objReceiptList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptList, (object)objReceiptList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 入金ヘッダ取得

        private void GetReceiptH(long ReceiptNo)
        {
            try
            {
                objReceiptH = null;   // 初期化
                svcReceiptClient svc = new svcReceiptClient();
                svc.GetReceiptHCompleted += new EventHandler<GetReceiptHCompletedEventArgs>(this.GetReceiptHCompleted);
                svc.GetReceiptHAsync(Common.gstrSessionString, ReceiptNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetReceiptHCompleted(Object sender, GetReceiptHCompletedEventArgs e)
        {
            try
            {
                objReceiptH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objReceiptH != null)
                {
                    if (objReceiptH._message != "" && objReceiptH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objReceiptH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptListH, (object)objReceiptH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptListH, (object)objReceiptH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 入金明細リスト取得

        private void GetReceiptListD(long ReceiptNo)
        {
            try
            {
                objReceiptListD = null;   // 初期化
                svcReceiptClient svc = new svcReceiptClient();
                svc.GetReceiptListDCompleted += new EventHandler<GetReceiptListDCompletedEventArgs>(this.GetReceiptListDCompleted);
                svc.GetReceiptListDAsync(Common.gstrSessionString, ReceiptNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetReceiptListDCompleted(Object sender, GetReceiptListDCompletedEventArgs e)
        {
            try
            {
                objReceiptListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objReceiptListD != null)
                {
                    if (objReceiptListD[0]._message != "" && objReceiptListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objReceiptListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptListD, (object)objReceiptListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetReceiptListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 請求入金取得

        private void GetInvoiceReceipt(long invoiceNo, long receiptNo)
        {
            try
            {
                objInvocieReceipt = null;   // 初期化
                svcInvoiceCloseClient svc = new svcInvoiceCloseClient();
                svc.GetPaymentCashInCompleted += new EventHandler<GetPaymentCashInCompletedEventArgs>(this.GetPaymentCashInCompleted);
                svc.GetPaymentCashInAsync(Common.gstrSessionString, invoiceNo, receiptNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashIn" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentCashInCompleted(Object sender, GetPaymentCashInCompletedEventArgs e)
        {
            try
            {
                objInvocieReceipt = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInvocieReceipt != null)
                {
                    if (objInvocieReceipt.message != "" && objInvocieReceipt.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objInvocieReceipt.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvocieReceipt, (object)objInvocieReceipt);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvocieReceipt, (object)objInvocieReceipt);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInvocieReceipt, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvocieReceiptCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 入金更新

        private void UpdateReceipt(int type, long ReceiptNo, EntityReceiptH entityH, ObservableCollection<EntityReceiptD> entityD, EntityReceiptH before_entityH)
        {
            try
            {
                svcReceiptClient svc = new svcReceiptClient();
                svc.UpdateReceiptCompleted += new EventHandler<UpdateReceiptCompletedEventArgs>(this.UpdateReceiptCompleted);
                svc.UpdateReceiptAsync(Common.gstrSessionString, type, ReceiptNo, entityH, entityD, before_entityH);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateReceipt" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateReceiptCompleted(Object sender, UpdateReceiptCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateReceipt, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateReceipt, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateReceipt, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReceiptListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 請求残高

        #region 請求残高リスト取得

        private void GetInvoiceBalanceList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objInvoiceBalanceList = null;   // 初期化
                svcInvoiceBalanceClient svc = new svcInvoiceBalanceClient();
                svc.GetInvoiceBalanceListCompleted += new EventHandler<GetInvoiceBalanceListCompletedEventArgs>(this.GetInvoiceBalanceListCompleted);
                svc.GetInvoiceBalanceListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceBalanceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInvoiceBalanceListCompleted(Object sender, GetInvoiceBalanceListCompletedEventArgs e)
        {
            try
            {
                objInvoiceBalanceList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInvoiceBalanceList != null)
                {
                    if (objInvoiceBalanceList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceBalanceList, null);
                        return;
                    }

                    if (objInvoiceBalanceList[0].message != null)
                    {
                        if (objInvoiceBalanceList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objInvoiceBalanceList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceBalanceList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceBalanceList, (object)objInvoiceBalanceList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceBalanceList, (object)objInvoiceBalanceList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInvoiceBalanceList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 請求残高更新

        private void UpdateInvoiceBalance(int type, ObservableCollection<EntityInvoiceBalance> entity)
        {
            try
            {
                svcInvoiceBalanceClient svc = new svcInvoiceBalanceClient();
                svc.UpdateInvoiceBalanceCompleted += new EventHandler<UpdateInvoiceBalanceCompletedEventArgs>(this.UpdateInvoiceBalanceCompleted);
                svc.UpdateInvoiceBalanceAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInvoiceBalance" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateInvoiceBalanceCompleted(Object sender, UpdateInvoiceBalanceCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoiceBalance, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoiceBalance, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInvoiceBalance, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInvoiceBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 売掛残高

        #region 売掛残高リスト取得

        private void GetSalesCreditBalanceList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objSalesCreditBalanceList = null;   // 初期化
                svcSalesCreditBalanceClient svc = new svcSalesCreditBalanceClient();
                svc.GetSalesCreditBalanaceListCompleted += new EventHandler<GetSalesCreditBalanaceListCompletedEventArgs>(this.GetSalesCreditBalanceListCompleted);
                svc.GetSalesCreditBalanaceListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesCreditBalanceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSalesCreditBalanceListCompleted(Object sender, GetSalesCreditBalanaceListCompletedEventArgs e)
        {
            try
            {
                objSalesCreditBalanceList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objSalesCreditBalanceList != null)
                {
                    if (objSalesCreditBalanceList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesCreditBalanceList, null);
                        return;
                    }

                    if (objSalesCreditBalanceList[0].message != null)
                    {
                        if (objSalesCreditBalanceList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objSalesCreditBalanceList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesCreditBalanceList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesCreditBalanceList, (object)objSalesCreditBalanceList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesCreditBalanceList, (object)objSalesCreditBalanceList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetSalesCreditBalanceList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesCreditBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 売掛残高更新

        private void UpdateSalesCreditBalance(int type, ObservableCollection<EntitySalesCreditBalance> entity)
        {
            try
            {
                svcSalesCreditBalanceClient svc = new svcSalesCreditBalanceClient();
                svc.UpdateSalesCreditBalanceCompleted += new EventHandler<UpdateSalesCreditBalanceCompletedEventArgs>(this.UpdateSalesCreditBalanceCompleted);
                svc.UpdateSalesCreditBalanceAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateSalesCreditBalance" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateSalesCreditBalanceCompleted(Object sender, UpdateSalesCreditBalanceCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSalesCreditBalance, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSalesCreditBalance, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSalesCreditBalance, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSalesCreditBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 発注

        #region 発注リスト取得

        private void GetPurchaseOrderList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objPurchaseOrderList = null;   // 初期化
                svcPurchaseOrderClient svc = new svcPurchaseOrderClient();
                svc.GetPurchaseOrderListCompleted += new EventHandler<GetPurchaseOrderListCompletedEventArgs>(this.GetPurchaseOrderListCompleted);
                svc.GetPurchaseOrderListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseOrderListCompleted(Object sender, GetPurchaseOrderListCompletedEventArgs e)
        {
            try
            {
                objPurchaseOrderList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseOrderList != null)
                {
                    if (objPurchaseOrderList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderList, null);
                        return;
                    }

                    if (objPurchaseOrderList[0].message != null)
                    {
                        if (objPurchaseOrderList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objPurchaseOrderList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderList, (object)objPurchaseOrderList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderList, (object)objPurchaseOrderList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 発注ヘッダ取得

        private void GetPurchaseOrderH(long OrderNoFrom, long OrderNoTo)
        {
            try
            {
                objPurchaseOrderH = null;   // 初期化
                svcPurchaseOrderClient svc = new svcPurchaseOrderClient();
                svc.GetPurchaseOrderHCompleted += new EventHandler<GetPurchaseOrderHCompletedEventArgs>(this.GetPurchaseOrderHCompleted);
                svc.GetPurchaseOrderHAsync(Common.gstrSessionString, OrderNoFrom, OrderNoTo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseOrderHCompleted(Object sender, GetPurchaseOrderHCompletedEventArgs e)
        {
            try
            {
                objPurchaseOrderH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseOrderH != null)
                {
                    if (objPurchaseOrderH._message != "" && objPurchaseOrderH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPurchaseOrderH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderListH, (object)objPurchaseOrderH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderListH, (object)objPurchaseOrderH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 発注明細リスト取得

        private void GetPurchaseOrderListD(long OrderNo)
        {
            try
            {
                objPurchaseOrderListD = null;   // 初期化
                svcPurchaseOrderClient svc = new svcPurchaseOrderClient();
                svc.GetPurchaseOrderListDCompleted += new EventHandler<GetPurchaseOrderListDCompletedEventArgs>(this.GetPurchaseOrderListDCompleted);
                svc.GetPurchaseOrderListDAsync(Common.gstrSessionString, OrderNo, OrderNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseOrderListDCompleted(Object sender, GetPurchaseOrderListDCompletedEventArgs e)
        {
            try
            {
                objPurchaseOrderListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseOrderListD != null)
                {
                    if (objPurchaseOrderListD[0]._message != "" && objPurchaseOrderListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPurchaseOrderListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderListD, (object)objPurchaseOrderListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseOrderListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 発注更新

        private void UpdatePurchaseOrder(int type, long OrderNo, EntityPurchaseOrderH entityH, ObservableCollection<EntityPurchaseOrderD> entityD)
        {
            try
            {
                svcPurchaseOrderClient svc = new svcPurchaseOrderClient();
                svc.UpdatePurchaseOrderCompleted += new EventHandler<UpdatePurchaseOrderCompletedEventArgs>(this.UpdatePurchaseOrderCompleted);
                svc.UpdatePurchaseOrderAsync(Common.gstrSessionString, type, OrderNo, entityH, entityD);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePurchaseOrder" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePurchaseOrderCompleted(Object sender, UpdatePurchaseOrderCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchaseOrder, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchaseOrder, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchaseOrder, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseOrderListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 発注書発行更新

        private void UpdatePurchaseOrderPrint(int type, string _no)
        {
            try
            {
                svcPurchaseOrderClient svc = new svcPurchaseOrderClient();
                svc.UpdatePurchaseOrderPrintAsync(Common.gstrSessionString, type, _no);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePurchaseOrderPrint" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #endregion

        #region 仕入

        #region 仕入リスト取得

        private void GetPurchaseList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objPurchaseList = null;   // 初期化
                svcPurchaseClient svc = new svcPurchaseClient();
                svc.GetPurchaseListCompleted += new EventHandler<GetPurchaseListCompletedEventArgs>(this.GetPurchaseListCompleted);
                svc.GetPurchaseListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseListCompleted(Object sender, GetPurchaseListCompletedEventArgs e)
        {
            try
            {
                objPurchaseList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseList != null)
                {
                    if (objPurchaseList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseList, null);
                        return;
                    }

                    if (objPurchaseList[0].message != null)
                    {
                        if (objPurchaseList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objPurchaseList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseList, (object)objPurchaseList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseList, (object)objPurchaseList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 仕入ヘッダ取得

        private void GetPurchaseH(long PurchaseNoFrom, long PurchaseNoTo)
        {
            try
            {
                objPurchaseH = null;   // 初期化
                svcPurchaseClient svc = new svcPurchaseClient();
                svc.GetPurchaseHCompleted += new EventHandler<GetPurchaseHCompletedEventArgs>(this.GetPurchaseHCompleted);
                svc.GetPurchaseHAsync(Common.gstrSessionString, PurchaseNoFrom, PurchaseNoTo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseHCompleted(Object sender, GetPurchaseHCompletedEventArgs e)
        {
            try
            {
                objPurchaseH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseH != null)
                {
                    if (objPurchaseH._message != "" && objPurchaseH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPurchaseH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseListH, (object)objPurchaseH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseListH, (object)objPurchaseH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 仕入明細リスト取得

        private void GetPurchaseListD(long PurchaseNo)
        {
            try
            {
                objPurchaseListD = null;   // 初期化
                svcPurchaseClient svc = new svcPurchaseClient();
                svc.GetPurchaseListDCompleted += new EventHandler<GetPurchaseListDCompletedEventArgs>(this.GetPurchaseListDCompleted);
                svc.GetPurchaseListDAsync(Common.gstrSessionString, PurchaseNo, PurchaseNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseListDCompleted(Object sender, GetPurchaseListDCompletedEventArgs e)
        {
            try
            {
                objPurchaseListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseListD != null)
                {
                    if (objPurchaseListD[0]._message != "" && objPurchaseListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPurchaseListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseListD, (object)objPurchaseListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 仕入更新

        private void UpdatePurchase(int type, long PurchaseNo, EntityPurchaseH entityH, ObservableCollection<EntityPurchaseD> entityD, EntityPurchaseH before_entityH, ObservableCollection<EntityPurchaseD> before_entityD)
        {
            try
            {
                svcPurchaseClient svc = new svcPurchaseClient();
                svc.UpdatePurchaseCompleted += new EventHandler<UpdatePurchaseCompletedEventArgs>(this.UpdatePurchaseCompleted);
                svc.UpdatePurchaseAsync(Common.gstrSessionString, type, PurchaseNo, entityH, entityD, before_entityH, before_entityD);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePurchase" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePurchaseCompleted(Object sender, UpdatePurchaseCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchase, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchase, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchase, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 支払

        #region 支払締集計

        private void GetPaymentTotal(EntityPaymentClosePrm entityPrm)
        {
            try
            {
                objClass = null;   // 初期化
                svcPaymentCloseClient svc = new svcPaymentCloseClient();
                svc.GetPaymentTotalCompleted += new EventHandler<GetPaymentTotalCompletedEventArgs>(this.GetPaymentTotalCompleted);
                svc.GetPaymentTotalAsync(Common.gstrSessionString, entityPrm);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentTotal" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentTotalCompleted(Object sender, GetPaymentTotalCompletedEventArgs e)
        {
            try
            {
                objPaymentCloseList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentCloseList != null)
                {
                    if (objPaymentCloseList.Count == 0)
                    {
                        ExMessageBox.Show("締対象データが存在しません。");
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentTotal, null);
                        return;
                    }
                }

                if (objPaymentCloseList != null)
                {
                    if (objPaymentCloseList[0].message != "" && objPaymentCloseList[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPaymentCloseList[0].message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentTotal, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentTotal, (object)objPaymentCloseList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentTotal, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentTotalCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 支払リスト取得

        private void GetPaymentList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objPaymentList = null;   // 初期化
                svcPaymentCloseClient svc = new svcPaymentCloseClient();
                svc.GetPaymentListCompleted += new EventHandler<GetPaymentListCompletedEventArgs>(this.GetPaymentListCompleted);
                svc.GetPaymentListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentListCompleted(Object sender, GetPaymentListCompletedEventArgs e)
        {
            try
            {
                objPaymentList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentList != null)
                {
                    if (objPaymentList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentList, null);
                        return;
                    }

                    if (objPaymentList[0].message != null)
                    {
                        if (objPaymentList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objPaymentList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentList, (object)objPaymentList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentList, (object)objPaymentList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 支払締処理

        private void UpdatePayment(int type, ObservableCollection<EntityPaymentClose> entity)
        {
            try
            {
                svcPaymentCloseClient svc = new svcPaymentCloseClient();
                svc.UpdatePaymentCompleted += new EventHandler<UpdatePaymentCompletedEventArgs>(this.UpdatePaymentCompleted);
                svc.UpdatePaymentAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePayment" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePaymentCompleted(Object sender, UpdatePaymentCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    ExMessageBox.Show(message);

                    if (message.IndexOf("請求締切を実行しました。") != -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePayment, "");
                    }
                    else if (message.IndexOf("削除しました。") != -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePayment, "");
                    }
                    else
                    {
                        // 失敗
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePayment, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePayment, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePaymentCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 支払発行更新

        private void UpdatePaymentPrint(int type, ObservableCollection<EntityPaymentClose> entity)
        {
            try
            {
                svcPaymentCloseClient svc = new svcPaymentCloseClient();
                svc.UpdatePaymentPrintAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePaymentPrint" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #endregion

        #region 出金

        #region 出金リスト取得

        private void GetPaymentCashList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objPaymentCashList = null;   // 初期化
                svcPaymentCashClient svc = new svcPaymentCashClient();
                svc.GetPaymentCashListCompleted += new EventHandler<GetPaymentCashListCompletedEventArgs>(this.GetPaymentCashListCompleted);
                svc.GetPaymentCashListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentCashListCompleted(Object sender, GetPaymentCashListCompletedEventArgs e)
        {
            try
            {
                objPaymentCashList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentCashList != null)
                {
                    if (objPaymentCashList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashList, null);
                        return;
                    }

                    if (objPaymentCashList[0].message != null)
                    {
                        if (objPaymentCashList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objPaymentCashList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashList, (object)objPaymentCashList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashList, (object)objPaymentCashList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 出金ヘッダ取得

        private void GetPaymentCashH(long PaymentCashNo)
        {
            try
            {
                objPaymentCashH = null;   // 初期化
                svcPaymentCashClient svc = new svcPaymentCashClient();
                svc.GetPaymentCashHCompleted += new EventHandler<GetPaymentCashHCompletedEventArgs>(this.GetPaymentCashHCompleted);
                svc.GetPaymentCashHAsync(Common.gstrSessionString, PaymentCashNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentCashHCompleted(Object sender, GetPaymentCashHCompletedEventArgs e)
        {
            try
            {
                objPaymentCashH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentCashH != null)
                {
                    if (objPaymentCashH._message != "" && objPaymentCashH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPaymentCashH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashListH, (object)objPaymentCashH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashListH, (object)objPaymentCashH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 出金明細リスト取得

        private void GetPaymentCashListD(long PaymentCashNo)
        {
            try
            {
                objPaymentCashListD = null;   // 初期化
                svcPaymentCashClient svc = new svcPaymentCashClient();
                svc.GetPaymentCashDCompleted += new EventHandler<GetPaymentCashDCompletedEventArgs>(this.GetPaymentCashListDCompleted);
                svc.GetPaymentCashDAsync(Common.gstrSessionString, PaymentCashNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentCashListDCompleted(Object sender, GetPaymentCashDCompletedEventArgs e)
        {
            try
            {
                objPaymentCashListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentCashListD != null)
                {
                    if (objPaymentCashListD[0]._message != "" && objPaymentCashListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPaymentCashListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashListD, (object)objPaymentCashListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 支払出金取得

        private void GetPaymentCashOut(long paymentNo, long paymentCashNo)
        {
            try
            {
                objPaymentCashOut = null;   // 初期化
                svcPaymentCloseClient svc = new svcPaymentCloseClient();
                svc.GetPaymentCashOutCompleted += new EventHandler<GetPaymentCashOutCompletedEventArgs>(this.GetPaymentCashOutCompleted);
                svc.GetPaymentCashOutAsync(Common.gstrSessionString, paymentNo, paymentCashNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashOut" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentCashOutCompleted(Object sender, GetPaymentCashOutCompletedEventArgs e)
        {
            try
            {
                objPaymentCashOut = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentCashOut != null)
                {
                    if (objPaymentCashOut.message != "" && objPaymentCashOut.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPaymentCashOut.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashOut, (object)objPaymentCashOut);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashOut, (object)objPaymentCashOut);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCashOut, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashOutCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 出金更新

        private void UpdatePaymentCash(int type, long PaymentCashNo, EntityPaymentCashH entityH, ObservableCollection<EntityPaymentCashD> entityD, EntityPaymentCashH before_entityH)
        {
            try
            {
                svcPaymentCashClient svc = new svcPaymentCashClient();
                svc.UpdatePaymentCashCompleted += new EventHandler<UpdatePaymentCashCompletedEventArgs>(this.UpdatePaymentCashCompleted);
                svc.UpdatePaymentCashAsync(Common.gstrSessionString, type, PaymentCashNo, entityH, entityD, before_entityH);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePaymentCash" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePaymentCashCompleted(Object sender, UpdatePaymentCashCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentCash, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentCash, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentCash, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCashListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 支払残高

        #region 支払残高リスト取得

        private void GetPaymentBalanceList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objPaymentBalanceList = null;   // 初期化
                svcPaymentBalanceClient svc = new svcPaymentBalanceClient();
                svc.GetPaymentBalanceListCompleted += new EventHandler<GetPaymentBalanceListCompletedEventArgs>(this.GetPaymentBalanceListCompleted);
                svc.GetPaymentBalanceListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentBalanceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentBalanceListCompleted(Object sender, GetPaymentBalanceListCompletedEventArgs e)
        {
            try
            {
                objPaymentBalanceList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentBalanceList != null)
                {
                    if (objPaymentBalanceList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentBalanceList, null);
                        return;
                    }

                    if (objPaymentBalanceList[0].message != null)
                    {
                        if (objPaymentBalanceList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objPaymentBalanceList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentBalanceList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentBalanceList, (object)objPaymentBalanceList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentBalanceList, (object)objPaymentBalanceList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentBalanceList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 支払残高更新

        private void UpdatePaymentBalance(int type, ObservableCollection<EntityPaymentBalance> entity)
        {
            try
            {
                svcPaymentBalanceClient svc = new svcPaymentBalanceClient();
                svc.UpdatePaymentBalanceCompleted += new EventHandler<UpdatePaymentBalanceCompletedEventArgs>(this.UpdatePaymentBalanceCompleted);
                svc.UpdatePaymentBalanceAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePaymentBalance" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePaymentBalanceCompleted(Object sender, UpdatePaymentBalanceCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentBalance, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentBalance, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentBalance, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 買掛残高

        #region 買掛残高リスト取得

        private void GetPaymentCreditBalanceList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objPaymentCreditBalanceList = null;   // 初期化
                svcPaymentCreditBalanceClient svc = new svcPaymentCreditBalanceClient();
                svc.GetPaymentCreditBalanceListCompleted += new EventHandler<GetPaymentCreditBalanceListCompletedEventArgs>(this.GetPaymentCreditBalanceListCompleted);
                svc.GetPaymentCreditBalanceListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCreditBalanceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentCreditBalanceListCompleted(Object sender, GetPaymentCreditBalanceListCompletedEventArgs e)
        {
            try
            {
                objPaymentCreditBalanceList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPaymentCreditBalanceList != null)
                {
                    if (objPaymentCreditBalanceList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCreditBalanceList, null);
                        return;
                    }

                    if (objPaymentCreditBalanceList[0].message != null)
                    {
                        if (objPaymentCreditBalanceList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objPaymentCreditBalanceList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCreditBalanceList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCreditBalanceList, (object)objPaymentCreditBalanceList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCreditBalanceList, (object)objPaymentCreditBalanceList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPaymentCreditBalanceList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCreditBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 買掛残高更新

        private void UpdatePaymentCreditBalance(int type, ObservableCollection<EntityPaymentCreditBalance> entity)
        {
            try
            {
                svcPaymentCreditBalanceClient svc = new svcPaymentCreditBalanceClient();
                svc.UpdatePaymentCreditBalanceCompleted += new EventHandler<UpdatePaymentCreditBalanceCompletedEventArgs>(this.UpdatePaymentCreditBalanceCompleted);
                svc.UpdatePaymentCreditBalanceAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePaymentCreditBalance" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePaymentCreditBalanceCompleted(Object sender, UpdatePaymentCreditBalanceCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentCreditBalance, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentCreditBalance, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePaymentCreditBalance, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPaymentCreditBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 入出庫

        #region 入出庫リスト取得

        private void GetInOutDeliveryList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objInOutDeliveryList = null;   // 初期化
                svcInOutDeliveryClient svc = new svcInOutDeliveryClient();
                svc.GetInOutDeliveryListCompleted += new EventHandler<GetInOutDeliveryListCompletedEventArgs>(this.GetInOutDeliveryListCompleted);
                svc.GetInOutDeliveryListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInOutDeliveryListCompleted(Object sender, GetInOutDeliveryListCompletedEventArgs e)
        {
            try
            {
                objInOutDeliveryList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInOutDeliveryList != null)
                {
                    if (objInOutDeliveryList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryList, null);
                        return;
                    }

                    if (objInOutDeliveryList[0].message != null)
                    {
                        if (objInOutDeliveryList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objInOutDeliveryList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryList, (object)objInOutDeliveryList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryList, (object)objInOutDeliveryList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 入出庫ヘッダ取得

        private void GetInOutDeliveryH(long InOutDeliveryNoFrom, long InOutDeliveryNoTo)
        {
            try
            {
                objInOutDeliveryH = null;   // 初期化
                svcInOutDeliveryClient svc = new svcInOutDeliveryClient();
                svc.GetInOutDeliveryHCompleted += new EventHandler<GetInOutDeliveryHCompletedEventArgs>(this.GetInOutDeliveryHCompleted);
                svc.GetInOutDeliveryHAsync(Common.gstrSessionString, InOutDeliveryNoFrom, InOutDeliveryNoTo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryH" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInOutDeliveryHCompleted(Object sender, GetInOutDeliveryHCompletedEventArgs e)
        {
            try
            {
                objInOutDeliveryH = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInOutDeliveryH != null)
                {
                    if (objInOutDeliveryH._message != "" && objInOutDeliveryH._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objInOutDeliveryH._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryListH, (object)objInOutDeliveryH);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryListH, (object)objInOutDeliveryH);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryListH, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryHCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 入出庫明細リスト取得

        private void GetInOutDeliveryListD(long InOutDeliveryNo)
        {
            try
            {
                objInOutDeliveryListD = null;   // 初期化
                svcInOutDeliveryClient svc = new svcInOutDeliveryClient();
                svc.GetInOutDeliveryListDCompleted += new EventHandler<GetInOutDeliveryListDCompletedEventArgs>(this.GetInOutDeliveryListDCompleted);
                svc.GetInOutDeliveryListDAsync(Common.gstrSessionString, InOutDeliveryNo, InOutDeliveryNo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryListD" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInOutDeliveryListDCompleted(Object sender, GetInOutDeliveryListDCompletedEventArgs e)
        {
            try
            {
                objInOutDeliveryListD = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInOutDeliveryListD != null)
                {
                    if (objInOutDeliveryListD[0]._message != "" && objInOutDeliveryListD[0]._message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objInOutDeliveryListD[0]._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryListD, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryListD, (object)objInOutDeliveryListD);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetInOutDeliveryListD, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryListDCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 入出庫更新

        private void UpdateInOutDelivery(int type, long InOutDeliveryNo, EntityInOutDeliveryH entityH, ObservableCollection<EntityInOutDeliveryD> entityD, EntityInOutDeliveryH before_entityH, ObservableCollection<EntityInOutDeliveryD> before_entityD)
        {
            try
            {
                svcInOutDeliveryClient svc = new svcInOutDeliveryClient();
                svc.UpdateInOutDeliveryCompleted += new EventHandler<UpdateInOutDeliveryCompletedEventArgs>(this.UpdateInOutDeliveryCompleted);
                svc.UpdateInOutDeliveryAsync(Common.gstrSessionString, type, InOutDeliveryNo, entityH, entityD, before_entityH, before_entityD);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInOutDelivery" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateInOutDeliveryCompleted(Object sender, UpdateInOutDeliveryCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInOutDelivery, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInOutDelivery, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateInOutDelivery, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInOutDeliveryListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region 棚卸

        #region 棚卸リスト取得

        private void GetStockInventoryList(string strWhereSql, string strOrderBySql)
        {
            try
            {
                objStockInventoryList = null;   // 初期化
                svcStockInventoryClient svc = new svcStockInventoryClient();
                svc.GetStockInventoryListCompleted += new EventHandler<GetStockInventoryListCompletedEventArgs>(this.GetStockInventoryListCompleted);
                svc.GetStockInventoryListAsync(Common.gstrSessionString, strWhereSql, strOrderBySql);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetStockInventoryList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetStockInventoryListCompleted(Object sender, GetStockInventoryListCompletedEventArgs e)
        {
            try
            {
                objStockInventoryList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objStockInventoryList != null)
                {
                    if (objStockInventoryList.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetStockInventoryList, null);
                        return;
                    }

                    if (objStockInventoryList[0].message != null)
                    {
                        if (objStockInventoryList[0].message != "")
                        {
                            // 認証失敗
                            ExMessageBox.Show(objStockInventoryList[0].message);
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetStockInventoryList, null);
                        }
                        else
                        {
                            // 認証成功
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetStockInventoryList, (object)objStockInventoryList);
                        }
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetStockInventoryList, (object)objStockInventoryList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetStockInventoryList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetStockInventoryListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 棚卸更新

        private void UpdateStockInventory(int type, string ymd, ObservableCollection<EntityStockInventory> entity)
        {
            try
            {
                svcStockInventoryClient svc = new svcStockInventoryClient();
                svc.UpdateStockInventoryCompleted += new EventHandler<UpdateStockInventoryCompletedEventArgs>(this.UpdateStockInventoryCompleted);
                svc.UpdateStockInventoryAsync(Common.gstrSessionString, type, ymd, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateStockInventory" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateStockInventoryCompleted(Object sender, UpdateStockInventoryCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateStockInventory, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateStockInventory, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateStockInventory, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetStockInventoryListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region マスタ系

        #region 名称マスタリスト取得

        private void GetNameList()
        {
            try
            {
                objNameList = null;   // 初期化
                svcSysNameClient svc = new svcSysNameClient();
                svc.GetNameListAllCompleted += new EventHandler<GetNameListAllCompletedEventArgs>(this.GetNameListAllCompleted);
                svc.GetNameListAllAsync(Common.gstrSessionString);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetNameList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetNameListAllCompleted(Object sender, GetNameListAllCompletedEventArgs e)
        {
            try
            {
                objNameList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objNameList != null)
                {
                    if (objNameList[0].message != "" && objNameList[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objNameList[0].message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetNameList, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetNameList, (object)objNameList);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetNameList, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetNameListAllCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 会社マスタ取得

        private void GetCompany()
        {
            try
            {
                objCompany = null;   // 初期化
                svcCompanyClient svc = new svcCompanyClient();
                svc.GetCompanyCompleted += new EventHandler<GetCompanyCompletedEventArgs>(this.GetCompanyCompleted);
                svc.GetCompanyAsync(Common.gstrSessionString);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCompany" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCompanyCompleted(Object sender, GetCompanyCompletedEventArgs e)
        {
            try
            {
                objCompany = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objCompany != null)
                {
                    if (objCompany.message != "" && objCompany.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objCompany.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCompany, (object)objCompany);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCompany, (object)objCompany);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetCompany, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCompanyCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 会社更新

        private void UpdateCompany(int type, EntityCompany entity)
        {
            try
            {
                svcCompanyClient svc = new svcCompanyClient();
                svc.UpdateCompanyCompleted += new EventHandler<UpdateCompanyCompletedEventArgs>(this.UpdateCompanyCompleted);
                svc.UpdateCompanyAsync(Common.gstrSessionString, type, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCompany" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateCompanyCompleted(Object sender, UpdateCompanyCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCompany, "");
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCompany, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCompany, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCompanyCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 会社グループマスタ取得

        private void GetCompanyGroup(int id)
        {
            try
            {
                objCompanyGroup = null;   // 初期化
                svcCompanyGroupClient svc = new svcCompanyGroupClient();
                svc.GetCompanyGroupCompleted += new EventHandler<GetCompanyGroupCompletedEventArgs>(this.GetCompanyGroupCompleted);
                svc.GetCompanyGroupAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCompanyGroup" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCompanyGroupCompleted(Object sender, GetCompanyGroupCompletedEventArgs e)
        {
            try
            {
                objCompanyGroup = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objCompanyGroup != null)
                {
                    if (objCompanyGroup.message != "" && objCompanyGroup.message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objCompanyGroup.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCompanyGroup, (object)objCompanyGroup);
                    }
                    else
                    {
                        // 成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCompanyGroup, (object)objCompanyGroup);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetCompanyGroup, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCompanyGroupCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 会社グループマスタ更新

        private void UpdateCompanyGroup(int type, int Id, EntityCompanyGroup entity)
        {
            try
            {
                svcCompanyGroupClient svc = new svcCompanyGroupClient();
                svc.UpdateCompanyGroupCompleted += new EventHandler<UpdateCompanyGroupCompletedEventArgs>(this.UpdateCompanyGroupCompleted);
                svc.UpdateCompanyGroupAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCompanyGroup" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateCompanyGroupCompleted(Object sender, UpdateCompanyGroupCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCompanyGroup, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCompanyGroup, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCompanyGroup, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCompanyGroupCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region ユーザマスタ取得

        private void GetUser(int id)
        {
            try
            {
                objUser = null;   // 初期化
                svcUserClient svc = new svcUserClient();
                svc.GetUserCompleted += new EventHandler<GetUserCompletedEventArgs>(this.GetUserCompleted);
                svc.GetUserAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetUser" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetUserCompleted(Object sender, GetUserCompletedEventArgs e)
        {
            try
            {
                objUser = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objUser != null)
                {
                    if (objUser.message != "" && objUser.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objUser.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetUser, (object)objUser);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetUser, (object)objUser);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetUser, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetUserCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region ユーザ更新

        private void UpdateUser(int type, long Id, EntityUser entity)
        {
            try
            {
                svcUserClient svc = new svcUserClient();
                svc.UpdateUserCompleted += new EventHandler<UpdateUserCompletedEventArgs>(this.UpdateUserCompleted);
                svc.UpdateUserAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateUser" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateUserCompleted(Object sender, UpdateUserCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateUser, "");
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateUser, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateUser, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateUserCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 担当マスタ取得

        private void GetPerson(int id)
        {
            try
            {
                objPerson = null;   // 初期化
                svcPersonClient svc = new svcPersonClient();
                svc.GetPersonCompleted += new EventHandler<GetPersonCompletedEventArgs>(this.GetPersonCompleted);
                svc.GetPersonAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPerson" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPersonCompleted(Object sender, GetPersonCompletedEventArgs e)
        {
            try
            {
                objPerson = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPerson != null)
                {
                    if (objPerson.message != "" && objPerson.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objPerson.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPerson, (object)objPerson);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPerson, (object)objPerson);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPerson, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPersonCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 担当更新

        private void UpdatePerson(int type, long Id, EntityPerson entity)
        {
            try
            {
                svcPersonClient svc = new svcPersonClient();
                svc.UpdatePersonCompleted += new EventHandler<UpdatePersonCompletedEventArgs>(this.UpdatePersonCompleted);
                svc.UpdatePersonAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePerson" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePersonCompleted(Object sender, UpdatePersonCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePerson, "");
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePerson, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePerson, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePersonCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 得意先マスタ取得

        private void GetCustomer(string id)
        {
            try
            {
                objCustomer = null;   // 初期化
                svcCustomerClient svc = new svcCustomerClient();
                svc.GetCustomerCompleted += new EventHandler<GetCustomerCompletedEventArgs>(this.GetCustomerCompleted);
                svc.GetCustomerAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCustomer" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCustomerCompleted(Object sender, GetCustomerCompletedEventArgs e)
        {
            try
            {
                objCustomer = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objCustomer != null)
                {
                    if (objCustomer.message != "" && objCustomer.message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objCustomer.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCustomer, (object)objCustomer);
                    }
                    else
                    {
                        // 成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCustomer, (object)objCustomer);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetCustomer, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCustomerCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 得意先更新

        private void UpdateCustomer(int type, string Id, EntityCustomer entity)
        {
            try
            {
                svcCustomerClient svc = new svcCustomerClient();
                svc.UpdateCustomerCompleted += new EventHandler<UpdateCustomerCompletedEventArgs>(this.UpdateCustomerCompleted);
                svc.UpdateCustomerAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCustomer" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateCustomerCompleted(Object sender, UpdateCustomerCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCustomer, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCustomer, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCustomer, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCustomerCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 商品マスタ取得

        private void GetCommodity(string id)
        {
            try
            {
                objCommodity = null;   // 初期化
                svcCommodityClient svc = new svcCommodityClient();
                svc.GetCommodityCompleted += new EventHandler<GetCommodityCompletedEventArgs>(this.GetCommodityCompleted);
                svc.GetCommodityAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCommodity" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCommodityCompleted(Object sender, GetCommodityCompletedEventArgs e)
        {
            try
            {
                objCommodity = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objCommodity != null)
                {
                    if (objCommodity.message != "" && objCommodity.message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objCommodity.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCommodity, (object)objCommodity);
                    }
                    else
                    {
                        // 成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCommodity, (object)objCommodity);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetCommodity, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCommodityCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 商品マスタ更新

        private void UpdateCommodity(int type, string Id, EntityCommodity entity)
        {
            try
            {
                svcCommodityClient svc = new svcCommodityClient();
                svc.UpdateCommodityCompleted += new EventHandler<UpdateCommodityCompletedEventArgs>(this.UpdateCommodityCompleted);
                svc.UpdateCommodityAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCommodity" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateCommodityCompleted(Object sender, UpdateCommodityCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCommodity, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCommodity, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCommodity, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCommodityCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 締区分マスタ取得

        private void GetCondition()
        {
            try
            {
                objCondition = null;   // 初期化
                svcConditionClient svc = new svcConditionClient();
                svc.GetConditionCompleted += new EventHandler<GetConditionCompletedEventArgs>(this.GetConditionCompleted);
                svc.GetConditionAsync(Common.gstrSessionString);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetCondition" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetConditionCompleted(Object sender, GetConditionCompletedEventArgs e)
        {
            try
            {
                objCondition = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objCondition != null)
                {
                    if (objCondition.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCondition, null);
                        return;
                    }
                }

                if (objCondition != null)
                {
                    if (objCondition[0].message != "" && objCondition[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objCondition[0].message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCondition, (object)objCondition);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetCondition, (object)objCondition);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetCondition, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetConditionCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 締区分マスタ更新

        private void UpdateCondition(ObservableCollection<EntityCondition> entity)
        {
            try
            {
                svcConditionClient svc = new svcConditionClient();
                svc.UpdateConditionCompleted += new EventHandler<UpdateConditionCompletedEventArgs>(this.UpdateConditionCompleted);
                svc.UpdateConditionAsync(Common.gstrSessionString, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateCondition" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateConditionCompleted(Object sender, UpdateConditionCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    // 失敗
                    ExMessageBox.Show(message);
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCondition, message);
                }
                else
                {
                    ExMessageBox.Show("登録しました。");

                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateCondition, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateConditionCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 分類マスタ取得

        private void GetClass(int classKbn)
        {
            try
            {
                objClass = null;   // 初期化
                svcClassClient svc = new svcClassClient();
                svc.GetClassCompleted += new EventHandler<GetClassCompletedEventArgs>(this.GetClassCompleted);
                svc.GetClassAsync(Common.gstrSessionString, classKbn);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetClass" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetClassCompleted(Object sender, GetClassCompletedEventArgs e)
        {
            try
            {
                objClass = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objClass != null)
                {
                    if (objClass.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetClass, null);
                        return;
                    }
                }

                if (objClass != null)
                {
                    if (objClass[0].message != "" && objClass[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objClass[0].message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetClass, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetClass, (object)objClass);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetClass, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetClassCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 分類マスタ更新

        private void UpdateClass(ObservableCollection<EntityClass> entity, int classKbn)
        {
            try
            {
                svcClassClient svc = new svcClassClient();
                svc.UpdateClassCompleted += new EventHandler<UpdateClassCompletedEventArgs>(this.UpdateClassCompleted);
                svc.UpdateClassAsync(Common.gstrSessionString, entity, classKbn);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateClass" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateClassCompleted(Object sender, UpdateClassCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    // 失敗
                    ExMessageBox.Show(message);
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateClass, message);
                }
                else
                {
                    ExMessageBox.Show("登録しました。");

                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateClass, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateClassCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 納入先マスタ取得

        private void GetSupplier(string CustomerId, string id)
        {
            try
            {
                objSupplier = null;   // 初期化
                svcSupplierClient svc = new svcSupplierClient();
                svc.GetSupplierCompleted += new EventHandler<GetSupplierCompletedEventArgs>(this.GetSupplierCompleted);
                svc.GetSupplierAsync(Common.gstrSessionString, CustomerId, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSupplier" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSupplierCompleted(Object sender, GetSupplierCompletedEventArgs e)
        {
            try
            {
                objSupplier = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objSupplier != null)
                {
                    if (objSupplier.message != "" && objSupplier.message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objSupplier.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSupplier, (object)objSupplier);
                    }
                    else
                    {
                        // 成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSupplier, (object)objSupplier);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetSupplier, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSupplierCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 納入先更新

        private void UpdateSupplier(int type, string CustomerId, string Id, EntitySupplier entity)
        {
            try
            {
                svcSupplierClient svc = new svcSupplierClient();
                svc.UpdateSupplierCompleted += new EventHandler<UpdateSupplierCompletedEventArgs>(this.UpdateSupplierCompleted);
                svc.UpdateSupplierAsync(Common.gstrSessionString, type, CustomerId, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateSupplier" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateSupplierCompleted(Object sender, UpdateSupplierCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSupplier, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSupplier, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateSupplier, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateSupplierCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 仕入先マスタ取得

        private void GetPurchaseMst(string id)
        {
            try
            {
                objPurchaseMst = null;   // 初期化
                svcPurchaseMstClient svc = new svcPurchaseMstClient();
                svc.GetPurchaseMstCompleted += new EventHandler<GetPurchaseMstCompletedEventArgs>(this.GetPurchaseMstCompleted);
                svc.GetPurchaseMstAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseMst" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseMstCompleted(Object sender, GetPurchaseMstCompletedEventArgs e)
        {
            try
            {
                objPurchaseMst = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objPurchaseMst != null)
                {
                    if (objPurchaseMst.message != "" && objPurchaseMst.message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objPurchaseMst.message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseMst, (object)objPurchaseMst);
                    }
                    else
                    {
                        // 成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseMst, (object)objPurchaseMst);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetPurchaseMst, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseMstCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 仕入先更新

        private void UpdatePurchaseMst(int type, string Id, EntityPurchaseMst entity)
        {
            try
            {
                svcPurchaseMstClient svc = new svcPurchaseMstClient();
                svc.UpdatePurchaseMstCompleted += new EventHandler<UpdatePurchaseMstCompletedEventArgs>(this.UpdatePurchaseMstCompleted);
                svc.UpdatePurchaseMstAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePurchaseMst" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdatePurchaseMstCompleted(Object sender, UpdatePurchaseMstCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchaseMst, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchaseMst, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdatePurchaseMst, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdatePurchaseMstCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 権限マスタ取得

        private void GetAuthority(int user_id)
        {
            try
            {
                objAuthority = null;   // 初期化
                svcAuthorityClient svc = new svcAuthorityClient();
                svc.GetAuthorityCompleted += new EventHandler<GetAuthorityCompletedEventArgs>(this.GetAuthorityCompleted);
                svc.GetAuthorityAsync(Common.gstrSessionString, user_id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetAuthority" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetAuthorityCompleted(Object sender, GetAuthorityCompletedEventArgs e)
        {
            try
            {
                objAuthority = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objAuthority != null)
                {
                    if (objAuthority.Count == 0)
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetAuthority, null);
                        return;
                    }
                }

                if (objAuthority != null)
                {
                    if (objAuthority[0].message != "" && objAuthority[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objAuthority[0].message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetAuthority, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetAuthority, (object)objAuthority);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetAuthority, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetAuthorityCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 権限マスタ更新

        private void UpdateAuthority(ObservableCollection<EntityAuthority> entity, int user_id)
        {
            try
            {
                svcAuthorityClient svc = new svcAuthorityClient();
                svc.UpdateAuthorityCompleted += new EventHandler<UpdateAuthorityCompletedEventArgs>(this.UpdateAuthorityCompleted);
                svc.UpdateAuthorityAsync(Common.gstrSessionString, entity, user_id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateAuthority" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateAuthorityCompleted(Object sender, UpdateAuthorityCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    // 失敗
                    ExMessageBox.Show(message);
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateAuthority, message);
                }
                else
                {
                    ExMessageBox.Show("登録しました。");

                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateAuthority, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateAuthorityCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region レポート設定取得

        private void GetReportSetting(string id)
        {
            try
            {
                objReportSetting = null;   // 初期化
                svcReportClient svc = new svcReportClient();
                svc.GetReportSettingCompleted += new EventHandler<GetReportSettingCompletedEventArgs>(this.GetReportSettingCompleted);
                svc.GetReportSettingAsync(Common.gstrSessionString, id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReportSetting" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetReportSettingCompleted(Object sender, GetReportSettingCompletedEventArgs e)
        {
            try
            {
                objReportSetting = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objReportSetting != null)
                {
                    if (objReportSetting._message != "" && objReportSetting._message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objReportSetting._message);
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReportSetting, (object)objReportSetting);
                    }
                    else
                    {
                        // 成功
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetReportSetting, (object)objReportSetting);
                    }
                }
                else
                {
                    objPerent.DataSelect((int)geWebServiceCallKbn.GetReportSetting, null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetReportSettingCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region レポート設定更新

        private void UpdateReportSetting(int type, string Id, EntityReportSetting entity)
        {
            try
            {
                svcReportClient svc = new svcReportClient();
                svc.UpdateReportSettingCompleted += new EventHandler<UpdateReportSettingCompletedEventArgs>(this.UpdateReportSettingCompleted);
                svc.UpdateReportSettingAsync(Common.gstrSessionString, type, Id, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateReportSetting" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateReportSettingCompleted(Object sender, UpdateReportSettingCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateReportSetting, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateReportSetting, message);
                    }
                }
                else
                {
                    // 成功
                    objPerent.DataUpdate((int)geWebServiceCallKbn.UpdateReportSetting, "");
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateReportSettingCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region サポート

        #region 問い合わせ

        #region 問い合わせリスト取得

        private void GetInquiry(long no)
        {
            try
            {
                objInquiryList = null;   // 初期化
                svcInquiryClient svc = new svcInquiryClient();
                svc.GetInquiryCompleted += new EventHandler<GetInquiryCompletedEventArgs>(this.GetInquiryCompleted);
                svc.GetInquiryAsync(Common.gstrSessionString, no);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInquiry" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInquiryCompleted(Object sender, GetInquiryCompletedEventArgs e)
        {
            try
            {
                objInquiryList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objInquiryList != null)
                {
                    if (objInquiryList.Count == 0)
                    {
                        objWindow.DataSelect((int)geWebServiceCallKbn.GetInquiry, null);
                        return;
                    }
                }

                if (objInquiryList != null)
                {
                    if (objInquiryList[0].message != "" && objInquiryList[0].message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objInquiryList[0].message);
                        objWindow.DataSelect((int)geWebServiceCallKbn.GetInquiry, null);
                    }
                    else
                    {
                        // 認証成功
                        objWindow.DataSelect((int)geWebServiceCallKbn.GetInquiry, (object)objInquiryList);
                    }
                }
                else
                {
                    objWindow.DataSelect((int)geWebServiceCallKbn.GetInquiry, null);
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetInquiryCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 問い合わせ更新

        private void UpdateInquiry(int type, long no, EntityInquiry entity)
        {
            try
            {
                svcInquiryClient svc = new svcInquiryClient();
                svc.UpdateInquiryCompleted += new EventHandler<UpdateInquiryCompletedEventArgs>(this.UpdateInquiryCompleted);
                svc.UpdateInquiryAsync(Common.gstrSessionString, type, no, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInquiry" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateInquiryCompleted(Object sender, UpdateInquiryCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    // 失敗
                    ExMessageBox.Show(message);
                    objWindow.DataUpdate((int)geWebServiceCallKbn.UpdateInquiry, message);
                }
                else
                {
                    ExMessageBox.Show("登録しました。");

                    // 成功
                    objWindow.DataUpdate((int)geWebServiceCallKbn.UpdateInquiry, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateInquiryCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion
        
        #endregion

        #endregion

        #region 業務連絡

        #region 業務連絡リスト取得

        private void GetDuties(long no, int kbn)
        {
            try
            {
                objDutiesList = null;   // 初期化
                svcDutiesClient svc = new svcDutiesClient();
                svc.GetDutiesCompleted += new EventHandler<GetDutiesCompletedEventArgs>(this.GetDutiesCompleted);
                svc.GetDutiesAsync(Common.gstrSessionString, no, kbn);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetDuties" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetDutiesCompleted(Object sender, GetDutiesCompletedEventArgs e)
        {
            try
            {
                objDutiesList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objDutiesList != null)
                {
                    if (objDutiesList.Count == 0)
                    {
                        if (objWindow != null)
                        {
                            objWindow.DataSelect((int)geWebServiceCallKbn.GetDuties, null);
                        }
                        else
                        {
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetDuties, null);
                        }
                        return;
                    }
                }

                if (objDutiesList != null)
                {
                    if (objDutiesList[0].message != "" && objDutiesList[0].message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(message);

                        if (objWindow != null)
                        {
                            objWindow.DataSelect((int)geWebServiceCallKbn.GetDuties, null);
                        }
                        else
                        {
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetDuties, null);
                        }
                    }
                    else
                    {
                        // 認証成功
                        if (objWindow != null)
                        {
                            objWindow.DataSelect((int)geWebServiceCallKbn.GetDuties, (object)objDutiesList);
                        }
                        else
                        {
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetDuties, (object)objDutiesList);
                        }
                    }
                }
                else
                {
                    if (objWindow != null)
                    {
                        objWindow.DataSelect((int)geWebServiceCallKbn.GetDuties, null);
                    }
                    else
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetDuties, null);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetDutiesCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region 業務連絡更新

        private void UpdateDuties(int type, long no, svcDuties.EntityDuties entity)
        {
            try
            {
                svcDutiesClient svc = new svcDutiesClient();
                svc.UpdateDutiesCompleted += new EventHandler<UpdateDutiesCompletedEventArgs>(this.UpdateDutiesCompleted);
                svc.UpdateDutiesAsync(Common.gstrSessionString, type, no, entity);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateDuties" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void UpdateDutiesCompleted(Object sender, UpdateDutiesCompletedEventArgs e)
        {
            try
            {
                string message = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (message != "" && message != null)
                {
                    if (message.IndexOf("Auto Insert success : ") > -1)
                    {
                        // 成功
                        objWindow.DataUpdate((int)geWebServiceCallKbn.UpdateDuties, "");
                        ExMessageBox.Show(message.Replace("Auto Insert success : ", ""));
                    }
                    else
                    {
                        // 失敗
                        ExMessageBox.Show(message);
                        objWindow.DataUpdate((int)geWebServiceCallKbn.UpdateDuties, message);
                    }

                }
                else
                {
                    ExMessageBox.Show("登録しました。");

                    // 成功
                    objWindow.DataUpdate((int)geWebServiceCallKbn.UpdateDuties, "");
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".UpdateDutiesCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region システムからの連絡

        #region システムからの連絡リスト取得

        private void GetSystemInf(long no, int kbn)
        {
            try
            {
                objSystemInfList = null;   // 初期化
                svcSystemInfClient svc = new svcSystemInfClient();
                svc.GetSystemInfCompleted += new EventHandler<GetSystemInfCompletedEventArgs>(this.GetSystemInfCompleted);
                svc.GetSystemInfAsync(Common.gstrSessionString, no, kbn);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSystemInf" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSystemInfCompleted(Object sender, GetSystemInfCompletedEventArgs e)
        {
            try
            {
                objSystemInfList = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objSystemInfList != null)
                {
                    if (objSystemInfList.Count == 0)
                    {
                        if (objWindow != null)
                        {
                            objWindow.DataSelect((int)geWebServiceCallKbn.GetSystemInf, null);
                        }
                        else
                        {
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSystemInf, null);
                        }
                        return;
                    }
                }

                if (objSystemInfList != null)
                {
                    if (objSystemInfList[0].message != "" && objSystemInfList[0].message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(message);

                        if (objWindow != null)
                        {
                            objWindow.DataSelect((int)geWebServiceCallKbn.GetSystemInf, null);
                        }
                        else
                        {
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSystemInf, null);
                        }
                    }
                    else
                    {
                        // 認証成功
                        if (objWindow != null)
                        {
                            objWindow.DataSelect((int)geWebServiceCallKbn.GetSystemInf, (object)objSystemInfList);
                        }
                        else
                        {
                            objPerent.DataSelect((int)geWebServiceCallKbn.GetSystemInf, (object)objSystemInfList);
                        }
                    }
                }
                else
                {
                    if (objWindow != null)
                    {
                        objWindow.DataSelect((int)geWebServiceCallKbn.GetSystemInf, null);
                    }
                    else
                    {
                        objPerent.DataSelect((int)geWebServiceCallKbn.GetSystemInf, null);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".GetSystemInfCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion

        #region PG証跡追加

        private void AddEvidence(string pgId, int operationType, string memo)
        {
            try
            {
                svcPgEvidenceClient svc = new svcPgEvidenceClient();
                svc.AddEvidenceAsync(Common.gstrSessionString, pgId, operationType, memo);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".AddEvidence" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #region PG排他制御

        private void LockPg(string pgId, string lockId, int type)
        {
            try
            {
                svcPgLockClient svc = new svcPgLockClient();
                svc.LockPgAsync(Common.gstrSessionString, pgId, lockId, type);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".LockPg" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #region 複写処理

        private void CopyCheck(string tblName, string Id)
        {
            try
            {
                objCopying = null;   // 初期化
                svcCopyingClient svc = new svcCopyingClient();
                svc.CopyCheckCompleted += new EventHandler<CopyCheckCompletedEventArgs>(this.CopyCheckCompleted);
                svc.CopyCheckAsync(Common.gstrSessionString, tblName, Id);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".CopyCheck" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void CopyCheckCompleted(Object sender, CopyCheckCompletedEventArgs e)
        {
            try
            {
                objCopying = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objCopying != null)
                {
                    if (objCopying._message != "" && objCopying._message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objCopying._message);
                        objPerent.CopyCheck((object)objCopying);
                    }
                    else
                    {
                        // 成功
                        objPerent.CopyCheck((object)objCopying);
                    }
                }
                else
                {
                    objPerent.CopyCheck(null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".CopyCheckCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #region レポート出力

        private void ReportOut(string rptKbn, string pgId, string parameters)
        {
            try
            {
                objReport = null;   // 初期化
                svcReportClient svc = new svcReportClient();
                svc.ReportOutCompleted += new EventHandler<ReportOutCompletedEventArgs>(this.ReportOutCompleted);
                svc.ReportOutAsync(Common.gstrSessionString, rptKbn, pgId, parameters);
            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".ReportOut" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void ReportOutCompleted(Object sender, ReportOutCompletedEventArgs e)
        {
            try
            {
                objReport = e.Result;
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                if (objReport != null)
                {
                    if (objReport._message != "" && objReport._message != null)
                    {
                        // 失敗
                        ExMessageBox.Show(objReport._message);
                        objPerent.ReportOut((object)objReport);
                    }
                    else
                    {
                        // 成功
                        objPerent.ReportOut((object)objReport);
                    }
                }
                else
                {
                    objPerent.ReportOut(null);
                }

            }
            catch (Exception ex)
            {
                this.ProcessingDlgClose();
                ExMessageBox.Show(CLASS_NM + ".ReportOutCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
            finally
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }
            }
        }

        #endregion

        #endregion
    }
}
