using System;
using System.Collections.Generic;
using System.Linq;
using SlvHanbaiClient.Class.WebService;

namespace SlvHanbaiClient.Class.Data
{
    public class DataPgEvidence
    {
        public class PGName
        {
            public const string System = "System";

            public class Menu
            {
                public const string MainMenu = "MainMenu";        // メインメニュー
            }

            public class SystemContact
            {
                public const string SystemList = "SystemList";                              // システムからの連絡参照
            }

            #region 売上入力系

            public class Order
            {
                public const string OrderInp = "OrderInp";                                  // 受注入力
                public const string OrderList = "OrderList";                                // 受注一覧
                public const string OrderListPrint = "OrderListPrint";                      // 受注一覧印刷
                public const string OrderPrint = "OrderPrint";                              // 注文請書印刷
                public const string OrderDPrint = "OrderDPrint";                            // 受注明細印刷
            }

            public class Estimate
            {
                public const string EstimateInp = "EstimateInp";                            // 見積入力
                public const string EstimateList = "EstimateList";                          // 見積一覧
                public const string EstimateListPrint = "EstimateListPrint";                // 見積一覧印刷
                public const string EstimatePrint = "EstimatePrint";                        // 見積書印刷
                public const string EstimateDPrint = "EstimateDPrint";                      // 見積明細印刷
            }

            public class Sales
            {
                public const string SalesInp = "SalesInp";                                  // 売上入力
                public const string SalesList = "SalesList";                                // 売上一覧
                public const string SalesListPrint = "SalesListPrint";                      // 売上一覧印刷
                public const string SalesPrint = "SalesPrint";                              // 納品書印刷
                public const string SalesDPrint = "SalesDPrint";                            // 売上明細印刷
                public const string SalesDayPrint = "SalesDayPrint";                        // 売上日報印刷
                public const string SalesMonthPrint = "SalesMonthPrint";                    // 売上月報印刷
                public const string SalesChangePrint = "SalesChangePrint";                  // 売上推移表印刷
            }

            public class Receipt
            {
                public const string ReceiptInp = "ReceiptInp";                              // 入金入力
                public const string ReceiptList = "ReceiptList";                            // 入金一覧
                public const string ReceiptDPrint = "ReceiptDPrint";                        // 入金明細印刷
                public const string ReceiptDayPrint = "ReceiptDayPrint";                    // 入金日報印刷
                public const string ReceiptMonthPrint = "ReceiptMonthPrint";                // 入金月報印刷
            }

            public class Invoice
            {
                public const string InvoiceClose = "InvoiceClose";                          // 請求締
                public const string InvoicePrint = "InvoicePrint";                          // 請求書発行
            }

            public class Plan
            {
                public const string CollectPlanPrint = "CollectPlanPrint";                  // 回収予定表印刷
                public const string PaymentPlanPrint = "PaymentPlanPrint";                  // 支払予定表印刷
            }

            public class SalesManagement
            {
                public const string SalesBalance = "SalesBalance";                          // 売掛残高
                public const string SalesBalancePrint = "SalesBalancePrint";                // 売掛残高一覧表
                public const string InvoiceBalance = "InvoiceBalance";                      // 請求残高
                public const string InvoiceBalancePrint = "InvoiceBalancePrint";            // 請求残高一覧表
                public const string CustomerLedger = "CustomerLedger";                      // 得意先元帳
            }

            #endregion

            #region 仕入入力系

            public class PurchaseOrder
            {
                public const string PurchaseOrderInp = "PurchaseOrderInp";                  // 発注入力
                public const string PurchaseOrderList = "PurchaseOrderList";                // 発注一覧
                public const string PurchaseOrderListPrint = "PurchaseOrderListPrint";      // 発注一覧印刷
                public const string PurchaseOrderPrint = "PurchaseOrderPrint";              // 発文印刷
                public const string PurchaseOrderDPrint = "PurchaseOrderDPrint";            // 発注明細印刷
            }

            public class Purchase
            {
                public const string PurchaseInp = "PurchaseInp";                            // 仕入入力
                public const string PurchaseList = "PurchaseList";                          // 仕入一覧
                public const string PurchaseListPrint = "PurchaseListPrint";                // 仕入一覧印刷
                //public const string PurchasePrint = "PurchasePrint";                        // 仕入請書印刷
                public const string PurchaseDPrint = "PurchaseDPrint";                      // 仕入明細印刷
                public const string PurchaseDayPrint = "PurchaseDayPrint";                  // 仕入日報印刷
                public const string PurchaseMonthPrint = "PurchaseMonthPrint";              // 仕入月報印刷
                public const string PurchaseChangePrint = "PurchaseChangePrint";            // 仕入推移表印刷
            }

            public class Payment
            {
                public const string PaymentClose = "PaymentClose";                          // 支払締
                public const string PaymentPrint = "PaymentPrint";                          // 支払書発行
            }

            public class PaymentCash
            {
                public const string PaymentCashInp = "PaymentCashInp";                      // 出金入力
                public const string PaymentCashList = "PaymentCashList";                    // 出金一覧
                public const string PaymentCashDPrint = "PaymentCashDPrint";                // 出金明細印刷
                public const string PaymentCashDayPrint = "PaymentCashDayPrint";            // 出金日報印刷
                public const string PaymentCashMonthPrint = "PaymentCashMonthPrint";        // 出金月報印刷
            }

            public class PaymentManagement
            {
                public const string PaymentCreditBalance = "PaymentCreditBalance";                  // 売掛残高
                public const string PaymentCreditBalancePrint = "PaymentCreditBalancePrint";        // 売掛残高一覧表
                public const string PaymentBalance = "PaymentBalance";                              // 支払残高
                public const string PaymentBalancePrint = "PaymentBalancePrint";                    // 支払残高一覧表
                public const string PurchaseLedger = "CustomerLedger";                              // 仕入先元帳
            }

            #endregion

            #region 在庫入力系

            public class InOutDeliver
            {
                public const string InOutDeliverInp = "InOutDeliverInp";                    // 入出庫入力
                public const string InOutDeliverList = "InOutDeliverList";                  // 入出庫一覧
                public const string InOutDeliverListPrint = "InOutDeliverListPrint";        // 入出庫一覧印刷
                public const string InOutDeliverPrint = "InOutDeliverPrint";                // 入出庫書印刷
                public const string InOutDeliverDPrint = "InOutDeliverDPrint";              // 入出庫明細印刷
            }

            public class StockInventory
            {
                public const string StockInventoryInp = "StockInventoryInp";                // 棚卸入力
            }

            public class Inventory
            {
                public const string InventoryListPrint = "InventoryListPrint";              // 在庫一覧印刷
            }

            #endregion

            public class Mst
            {
                public const string Company = "CompanyMst";                                 // 会社マスタ
                public const string CompanyGroup = "CompanyGroupMst";                       // 会社グループマスタ
                public const string User = "UserMst";                                       // ユーザーマスタ  
                public const string Person = "PersonMst";                                   // 担当マスタ
                public const string Customer = "CustomerMst";                               // 得意先マスタ
                public const string Supplier = "SupplierMst";                               // 納入先マスタ
                public const string Commodity = "CommodityMst";                             // 商品マスタ
                public const string Condition = "ConditionMst";                             // 締区分マスタ                
                public const string Class = "ClassMst";                                     // 分類マスタ  
                public const string Authority = "AuthorityMst";                             // 権限マスタ  
                public const string ReportSetting = "ReportSettingMst";                     // レポート設定マスタ  
                public const string Purchase = "PurchaseMst";                               // 仕入先マスタ
            }

            public class Inquiry
            {
                public const string InquiryInp = "InquiryInp";                              // 問い合わせ
                public const string InquiryList = "InquiryList";                            // 問い合わせ一覧
            }

            public class Duties
            {
                public const string DutiesInp = "DutiesInp";                                // 業務連絡
            }

            public class Report
            {
            }

        }

        public static string GetPgName(string pgId)
        {
            switch (pgId)
            {
                case DataPgEvidence.PGName.System:
                    return "販売管理システム";
                case DataPgEvidence.PGName.Order.OrderInp:
                    return "受注入力";
                case DataPgEvidence.PGName.Order.OrderList:
                    return "受注一覧";
                case DataPgEvidence.PGName.Order.OrderPrint:
                    return "受注書印刷";
                case DataPgEvidence.PGName.Order.OrderDPrint:
                    return "受注明細印刷";
                case DataPgEvidence.PGName.Mst.Customer:
                    return "得意先マスタ登録";
                case DataPgEvidence.PGName.Mst.Supplier:
                    return "納入先マスタ登録";
                case DataPgEvidence.PGName.Mst.Person:
                    return "担当マスタ登録";
                case DataPgEvidence.PGName.Mst.Condition:
                    return "締区分マスタ登録";
                case DataPgEvidence.PGName.Mst.Class:
                    return "分類マスタ登録";
                default:
                    return "";
            }
        }

        public enum geOperationType
        {
            Start = 0,
            End,
            Select,
            Update,
            Insert,
            Delete,
            PDF,
            Excel,
            CSV
        }

        public static string GetOperationTypeName(DataPgEvidence.geOperationType type)
        {
            switch ((int)type)
            {
                case 0:
                    return "開始";
                case 1:
                    return "終了";
                case 2:
                    return "選択";
                case 3:
                    return "更新";
                case 4:
                    return "追加";
                case 5:
                    return "削除";
                case 6:
                    return "PDF出力";
                case 7:
                    return "Excel出力";
                case 8:
                    return "CSV出力";
                default:
                    return "";
            }
        }

        // PG証跡追加
        public static void gAddEvidence(string pgId, int operationType, string memo)
        {
            object[] prm = new object[3];
            prm[0] = pgId;
            prm[1] = operationType.ToString();
            prm[2] = memo;
            ExWebService webService = new ExWebService();
            webService.CallWebService(ExWebService.geWebServiceCallKbn.AddEvidence,
                                      ExWebService.geDialogDisplayFlg.No,
                                      ExWebService.geDialogCloseFlg.No,
                                      prm);
        }

        // PG証跡追加
        public static void gAddEvidenceWithDialog(string pgId, int operationType, string memo)
        {
            object[] prm = new object[3];
            prm[0] = pgId;
            prm[1] = operationType.ToString();
            prm[2] = memo;
            ExWebService webService = new ExWebService();
            webService.CallWebService(ExWebService.geWebServiceCallKbn.AddEvidence,
                                      ExWebService.geDialogDisplayFlg.Yes,
                                      ExWebService.geDialogCloseFlg.Yes,
                                      prm);
        }

        public static void SaveLoadOrUnLoadEvidence(string pgId,
                                                    DataPgEvidence.geOperationType operationType)
        {
            DataPgEvidence.gAddEvidence(pgId, (int)operationType, "");
        }

        public static void SaveLoadOrUnLoadEvidence(Common.gePageGroupType pageGroupType,
                                                    Common.gePageType pageType,
                                                    DataPgEvidence.geOperationType operationType)
        {
            switch (pageGroupType)
            {
                case Common.gePageGroupType.StartUp:
                case Common.gePageGroupType.Menu:
                    break;
                case Common.gePageGroupType.Inp:
                    switch (pageType)
                    {
                        case Common.gePageType.InpOrder:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Order.OrderInp, (int)operationType, "");
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        public static void SaveLoadOrUnLoadEvidence(Common.geWinGroupType winGroupType,
                                                    Common.geWinType winType,
                                                    DataPgEvidence.geOperationType operationType)
        {
            switch (winGroupType)
            {
                case Common.geWinGroupType.NameList:
                    break;
                case Common.geWinGroupType.InpList:
                    switch (winType)
                    {
                        case Common.geWinType.ListOrder:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Order.OrderList, (int)operationType, "");
                            break;
                        default:
                            break;
                    }
                    break;
                case Common.geWinGroupType.MstList:
                    break;
                default:
                    break;
            }
        }

        public static void SaveLoadOrUnLoadEvidence(Common.geWinGroupType winGroupType,
                                                    Common.geWinMsterType winMstType,
                                                    DataPgEvidence.geOperationType operationType)
        {
            switch (winGroupType)
            {
                case Common.geWinGroupType.InpMaster:
                case Common.geWinGroupType.InpMasterDetail:
                    switch (winMstType)
                    {
                        case  Common.geWinMsterType.Customer:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Mst.Customer, (int)operationType, "");
                            break;
                        case Common.geWinMsterType.Person:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Mst.Person, (int)operationType, "");
                            break;
                        case Common.geWinMsterType.Commodity:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Mst.Commodity, (int)operationType, "");
                            break;
                        case Common.geWinMsterType.Condition:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Mst.Condition, (int)operationType, "");
                            break;
                        case Common.geWinMsterType.Class:
                            DataPgEvidence.gAddEvidence(DataPgEvidence.PGName.Mst.Class, (int)operationType, "");
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

    }


}