using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using SlvHanbai.Web.Reports;
using SlvHanbai.Web.Class;
using SlvHanbai.Web.Class.Data;
using SlvHanbai.Web.Class.Entity;
using SlvHanbai.Web.Class.Utility;

namespace SlvHanbai.Web.Class.Reports
{
    public class ExReportManeger
    {
        private const string CLASS_NM = "ExReportManeger";

        #region Field

        public DataReport.geReportKbn rptKbn;

        public PaperOrientation paperOrientation;
        public PaperSize paperSize;
        public string pdfFilePath;
        public string pdfFileName;
        public string reportFilePath;
        public string reportFileName;
        public string reportDir;
        public long reportFileSize;
        public int idFigureSlipNo = 0;
        public int idFigureCommodity = 0;
        public int idFigureCustomer = 0;
        public int idFigurePurchase = 0;
        public EntityReportSetting entitySetting;

        #endregion

        #region Report Method

        public bool GetReportFilePath(string pgId, string companyId, string userId)
        {
            this.reportFilePath = "";
            this.reportFileName = "";
            this.reportFileSize = 0;
            this.reportDir = "";

            try
            {
                string _now = DateTime.Now.ToString("yyyyMMddHHmmss").Replace("i", "");
                string _random = ExRandomString.GetRandomString();
                string _dir = @HttpContext.Current.Server.MapPath(@"..\") + @"temp\" + _now + "-" + companyId + "-" + userId + "-" + _random;

                DelDirectory(companyId, userId);
                if (!System.IO.Directory.Exists(_dir))
                {
                    System.IO.Directory.CreateDirectory(_dir);
                }

                string _fileName = "";
                switch (pgId)
                {

                    #region マスタ一覧表

                    case DataPgEvidence.PGName.Mst.Customer:
                        _fileName = "得意先マスタ一覧" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Mst.Supplier:
                        _fileName = "納入先マスタ一覧" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Mst.Purchase:
                        _fileName = "仕入先マスタ一覧" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Mst.Person:
                        _fileName = "担当マスタ一覧" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Mst.Commodity:
                        _fileName = "商品マスタ一覧" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Mst.Condition:
                        _fileName = "締区分マスタ一覧" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Mst.Class:
                        _fileName = "分類マスタ一覧" + this.GetExtension();
                        break;

                    #endregion

                    #region 売上入力系帳票

                    case DataPgEvidence.PGName.Estimate.EstimatePrint:
                        _fileName = "見積書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Order.OrderPrint:
                        _fileName = "注文請書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Sales.SalesPrint:
                        _fileName = "納品書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Invoice.InvoicePrint:
                        _fileName = "請求書" + this.GetExtension();
                        break;

                    #endregion

                    #region 売上系レポート

                    case DataPgEvidence.PGName.Estimate.EstimateDPrint:
                        _fileName = "見積明細書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Order.OrderDPrint:
                        _fileName = "受注明細書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Sales.SalesDPrint:
                        _fileName = "売上明細書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Sales.SalesDayPrint:
                        _fileName = "売上日報" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Sales.SalesMonthPrint:
                        _fileName = "売上月報" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Sales.SalesChangePrint:
                        _fileName = "売上推移表" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Receipt.ReceiptDPrint:
                        _fileName = "入金明細書" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Receipt.ReceiptDayPrint:
                        _fileName = "入金日報" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Receipt.ReceiptMonthPrint:
                        _fileName = "入金月報" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.Plan.CollectPlanPrint:
                        _fileName = "回収予定表" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.SalesManagement.SalesBalancePrint:
                        _fileName = "売掛残高一覧表" + this.GetExtension();
                        break;
                    case DataPgEvidence.PGName.SalesManagement.InvoiceBalancePrint:
                        _fileName = "請求残高一覧表" + this.GetExtension();
                        break;

                    #endregion

                    #region 仕入入力系帳票

                    case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderPrint:
                        _fileName = "発注書" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Payment.PaymentPrint:
                        _fileName = "支払書" + this.GetExtension();
                        break;

                    #endregion

                    #region 仕入系レポート

                    case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderDPrint:
                        _fileName = "発注明細書" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Purchase.PurchaseDPrint:
                        _fileName = "仕入明細書" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Purchase.PurchaseDayPrint:
                        _fileName = "仕入日報" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Purchase.PurchaseMonthPrint:
                        _fileName = "仕入月報" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Purchase.PurchaseChangePrint:
                        _fileName = "仕入推移表" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.PaymentCash.PaymentCashDPrint:
                        _fileName = "出金明細書" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.PaymentCash.PaymentCashDayPrint:
                        _fileName = "出金日報" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.PaymentCash.PaymentCashMonthPrint:
                        _fileName = "出金月報" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Plan.PaymentPlanPrint:
                        _fileName = "支払予定表" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.PaymentManagement.PaymentCreditBalancePrint:
                        _fileName = "買掛残高一覧表" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.PaymentManagement.PaymentBalancePrint:
                        _fileName = "支払残高一覧表" + this.GetExtension();
                        break;

                    #endregion

                    #region 在庫系レポート

                    case DataPgEvidence.PGName.InOutDeliver.InOutDeliverDPrint:
                        _fileName = "入出庫一覧表" + this.GetExtension();
                        break;

                    case DataPgEvidence.PGName.Inventory.InventoryListPrint:
                        _fileName = "在庫一覧表" + this.GetExtension();
                        break;


                    #endregion

                    default:
                        break;

                }

                if (_fileName == "")
                {
                    CommonUtl.gstrErrMsg = CLASS_NM + ".GetReportFilePath : ファイル名の設定に失敗しました。";
                    return false;
                } 
                
                this.reportFilePath = _dir + @"\" + _fileName;
                this.reportDir = @"/temp/" + _now + "-" + companyId + "-" + userId + "-" + _random + "/" + _fileName;
                this.reportFileName = _fileName;

                return true;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".GetReportFilePath", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".GetReportFilePath" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        private string GetExtension()
        {
            switch (this.rptKbn)
            {
                case DataReport.geReportKbn.Download:
                case DataReport.geReportKbn.OutPut:
                    return ".pdf";
                case DataReport.geReportKbn.Csv:
                    return ".csv";
                default:
                    return "";
            }
        }

        private bool DelDirectory(string companyId, string userId)
        {
            try
            {
                string _dir = @HttpContext.Current.Server.MapPath("../") + "temp";
                if (!System.IO.Directory.Exists(_dir)) return true;

                string[] subFolders = System.IO.Directory.GetDirectories(_dir, "*", System.IO.SearchOption.AllDirectories);
                string _now = DateTime.Now.ToString("yyyyMMddHHmmss").Replace("i", "");
                string _dir_companyId = "";
                string _dir_userId = "";

                // 会社IDとユーザIDが一致し、現在日時より前のディレクトリを削除する
                for (int i = 0; i < subFolders.Length; i++)
                {
                    // temp以下を取得する
                    string _temp = subFolders[i].Substring(subFolders[i].IndexOf(@"\temp"));

                    string _datetime = _temp.Substring(6, 14);

                    int _cnt2 = 0;
                    int _pos = 0;
                    for (int _i = 0; _i < _temp.Length - 1; _i++)
                    {
                        string _str = _temp.Substring(_i, 1);
                        if (_str.Equals("-"))
                        {
                            switch (_cnt2)
                            {
                                case 0:
                                    _pos = _i;
                                    break;
                                case 1:
                                    _dir_companyId = _temp.Substring(_pos + 1, _i - _pos - 1);
                                    _pos = _i;
                                    break;
                                case 2:
                                    _dir_userId = _temp.Substring(_pos + 1, _i - _pos - 1);
                                    break;
                                default:
                                    break;
                            }
                            _cnt2++;
                        }
                        if (_cnt2 == 3) break;
                    }

                    if (_dir_companyId == companyId && _dir_userId == userId && ExCast.zCDbl(_now) > ExCast.zCDbl(_datetime))
                    {
                        System.IO.Directory.Delete(subFolders[i], true);
                    }
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".DelDirectory", ex);
                return false;
            }

            return true;
        }

        public string GetPGIDXsd(string pgId)
        {
            string ret = "";

            switch (pgId)
            {
                case DataPgEvidence.PGName.Sales.SalesMonthPrint:
                    ret = DataPgEvidence.PGName.Sales.SalesDayPrint;
                    break;
                case DataPgEvidence.PGName.Receipt.ReceiptMonthPrint:
                    ret = DataPgEvidence.PGName.Receipt.ReceiptDayPrint;
                    break;
                case DataPgEvidence.PGName.Purchase.PurchaseMonthPrint:
                    ret = DataPgEvidence.PGName.Purchase.PurchaseDayPrint;
                    break;
                case DataPgEvidence.PGName.PaymentCash.PaymentCashMonthPrint:
                    ret = DataPgEvidence.PGName.PaymentCash.PaymentCashDayPrint;
                    break;
                default:
                    ret = pgId;
                    break;
            }

            return ret;
        }
        
        public bool ReportToPdf(DataSet ds, string pgId)
        {

            #region Report Setting

            ReportClass rpt = null;
            DataSet _ds = null;
            try
            {
                switch (pgId)
                {
                    #region マスタ一覧表

                    case DataPgEvidence.PGName.Mst.Customer:
                        rpt = new rptCustomerMst();
                        rpt.SetDataSource(ds);
                        break;
                    case DataPgEvidence.PGName.Mst.Purchase:
                        rpt = new rptCustomerMst();
                        rpt.SetDataSource(ds);
                        break;
                    case DataPgEvidence.PGName.Mst.Person:
                        rpt = new rptPersonMst();
                        rpt.SetDataSource(ds);
                        break;
                    case DataPgEvidence.PGName.Mst.Commodity:
                        rpt = new rptCommodityMst();
                        rpt.SetDataSource(ds);
                        break;
                    case DataPgEvidence.PGName.Mst.Condition:
                        rpt = new rptConditionMst();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 売上入力系帳票

                    case DataPgEvidence.PGName.Estimate.EstimatePrint:
                        rpt = new rptEstimatePrint();
                        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        _ds = SetDefualtRecord(ds);
                        rpt.SetDataSource(_ds);
                        break;
                    case DataPgEvidence.PGName.Order.OrderPrint:
                        rpt = new rptOrderPrint();
                        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        _ds = SetDefualtRecord(ds);
                        rpt.SetDataSource(_ds);
                        break;
                    case DataPgEvidence.PGName.Sales.SalesPrint:
                        rpt = new rptSalesPrint();
                        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        _ds = SetDefualtRecord(ds);
                        rpt.SetDataSource(_ds);
                        break;
                    case DataPgEvidence.PGName.Invoice.InvoicePrint:
                        rpt = new rptInvoiePrint();
                        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        SetRecordNoRecord(ref ds, 0, 3);
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 売上系レポート

                    #region 見積明細書

                    case DataPgEvidence.PGName.Estimate.EstimateDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptEstimateDPrint();
                            }
                            else
                            {
                                rpt = new rptEstimateDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptEstimateDPrint();
                        }

                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 受注明細書

                    case DataPgEvidence.PGName.Order.OrderDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptOrderDPrint();
                            }
                            else
                            {
                                rpt = new rptOrderDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptOrderDPrint();
                        }

                        rpt.SetDataSource(ds);
                        //rpt.SetParameterValue(0, "where app");
                        break;

                    #endregion

                    #region 売上明細書

                    case DataPgEvidence.PGName.Sales.SalesDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptSalesDPrint();
                            }
                            else
                            {
                                rpt = new rptSalesDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptSalesDPrint();
                        }

                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 売上日(月)報

                    case DataPgEvidence.PGName.Sales.SalesDayPrint:
                    case DataPgEvidence.PGName.Sales.SalesMonthPrint:
                        rpt = new rptSalesDayPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 売上推移表

                    case DataPgEvidence.PGName.Sales.SalesChangePrint:
                        rpt = new rptSalesChangePrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 入金明細書

                    case DataPgEvidence.PGName.Receipt.ReceiptDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptReceiptDPrint();
                            }
                            else
                            {
                                rpt = new rptReceiptDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptReceiptDPrint();
                        }

                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 入金日(月)報

                    case DataPgEvidence.PGName.Receipt.ReceiptDayPrint:
                    case DataPgEvidence.PGName.Receipt.ReceiptMonthPrint:
                        rpt = new rptReceiptDayPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 回収予定表

                    case DataPgEvidence.PGName.Plan.CollectPlanPrint:
                        rpt = new rptCollectPlanPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 売掛残高一覧表

                    case DataPgEvidence.PGName.SalesManagement.SalesBalancePrint:
                        rpt = new rptSalesBalancePrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 請求残高一覧表

                    case DataPgEvidence.PGName.SalesManagement.InvoiceBalancePrint:
                        rpt = new rptInvoiceBalancePrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #endregion

                    #region 仕入入力系帳票

                    case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderPrint:
                        rpt = new rptPurchaseOrderPrint();
                        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        _ds = SetDefualtRecord(ds);
                        rpt.SetDataSource(_ds);
                        break;

                    case DataPgEvidence.PGName.Payment.PaymentPrint:
                        rpt = new rptPaymentPrint();
                        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                        SetRecordNoRecord(ref ds, 0, 3);
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 仕入系レポート

                    #region 発注明細書

                    case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptPurchaseOrderDPrint();
                            }
                            else
                            {
                                rpt = new rptPurchaseOrderDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptPurchaseOrderDPrint();
                        }

                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 仕入明細書

                    case DataPgEvidence.PGName.Purchase.PurchaseDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptPurchaseDPrint();
                            }
                            else
                            {
                                rpt = new rptPurchaseDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptPurchaseDPrint();
                        }

                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 仕入日(月)報

                    case DataPgEvidence.PGName.Purchase.PurchaseDayPrint:
                    case DataPgEvidence.PGName.Purchase.PurchaseMonthPrint:
                        rpt = new rptPurchaseDayPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 仕入推移表

                    case DataPgEvidence.PGName.Purchase.PurchaseChangePrint:
                        rpt = new rptPurchaseChangePrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 出金明細書

                    case DataPgEvidence.PGName.PaymentCash.PaymentCashDPrint:
                        if (this.entitySetting != null)
                        {
                            if (this.entitySetting.total_kbn == 0)
                            {
                                rpt = new rptPaymentCashDPrint();
                            }
                            else
                            {
                                rpt = new rptPaymentCashDPrintTotal();
                            }
                        }
                        else
                        {
                            rpt = new rptPaymentCashDPrint();
                        }

                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 出金日(月)報

                    case DataPgEvidence.PGName.PaymentCash.PaymentCashDayPrint:
                    case DataPgEvidence.PGName.PaymentCash.PaymentCashMonthPrint:
                        rpt = new rptPaymentCashDayPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 支払予定表

                    case DataPgEvidence.PGName.Plan.PaymentPlanPrint:
                        rpt = new rptPaymentPlanPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 買掛残高一覧表

                    case DataPgEvidence.PGName.PaymentManagement.PaymentCreditBalancePrint:
                        rpt = new rptPaymentCreditBalancePrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 支払残高一覧表

                    case DataPgEvidence.PGName.PaymentManagement.PaymentBalancePrint:
                        rpt = new rptPaymentBalancePrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #endregion

                    #region 在庫系レポート

                    #region 入出庫一覧表

                    case DataPgEvidence.PGName.InOutDeliver.InOutDeliverDPrint:
                        rpt = new rptInOutDeliverDPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #region 在庫一覧表

                    case DataPgEvidence.PGName.Inventory.InventoryListPrint:
                        rpt = new rptInventoryListPrint();
                        rpt.SetDataSource(ds);
                        break;

                    #endregion

                    #endregion

                    default:
                        return false;
                }

                if (this.entitySetting != null)
                {
                    // 余白設定
                    int _leftMargin = rpt.PrintOptions.PageMargins.leftMargin;
                    int _rightMargin = rpt.PrintOptions.PageMargins.rightMargin;
                    int _topMargin = rpt.PrintOptions.PageMargins.topMargin;
                    int _bottomMargin = rpt.PrintOptions.PageMargins.bottomMargin;
                    CrystalDecisions.Shared.PageMargins margins;
                    margins.leftMargin = _leftMargin + ExCast.zCInt(this.entitySetting.left_margin * 25.40);
                    margins.rightMargin = _rightMargin + ExCast.zCInt(this.entitySetting.right_margin * 25.40);
                    margins.topMargin = _topMargin + ExCast.zCInt(this.entitySetting.top_margin * 25.40);
                    margins.bottomMargin = _bottomMargin + ExCast.zCInt(this.entitySetting.bottom_margin * 25.40);
                    rpt.PrintOptions.ApplyPageMargins(margins);

                    switch (this.entitySetting.orientation)
                    { 
                        case 1: // 縦
                            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
                            break;
                        case 2: // 横
                            rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
                            break;
                    }

                    switch (this.entitySetting.size)
                    {
                        case 1: // A3
                            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA3;
                            break;
                        case 2: // A4
                            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
                            break;
                        case 3: // A5
                            rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".ReportToPdf(Report Setting)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".ReportToPdf(Report Setting)" + Environment.NewLine + ex.Message;
                return false;
            }

            #endregion

            #region Export

            ExportOptions exportOpts = null;
            DiskFileDestinationOptions diskOpts = null;
            try
            {
                // Export Option 設定
                exportOpts = new ExportOptions();
                diskOpts = new DiskFileDestinationOptions();
                diskOpts.DiskFileName = this.reportFilePath;

                exportOpts = rpt.ExportOptions;
                exportOpts.DestinationOptions = diskOpts;
                exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                rpt.Export();
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".ReportToPdf(Export)", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".ReportToPdf(Export)" + Environment.NewLine + ex.Message;
                return false;
            }
            finally
            {
                if (exportOpts != null) exportOpts = null;
                if (diskOpts != null) diskOpts = null;
            }

            #endregion

            return true;
        }

        public bool DataTableToCsv(DataTable dt, string path)
        {
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            System.IO.StreamWriter sr = null;
            System.IO.FileInfo fi = null;
            try
            {
                sr = new System.IO.StreamWriter(path, false, enc);

                int colCount = dt.Columns.Count;
                int lastColIndex = colCount - 1;

                #region ヘッダ出力

                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む必要があるか調べる
                    if (field.IndexOf('"') > -1 ||
                        field.IndexOf(',') > -1 ||
                        field.IndexOf('\r') > -1 ||
                        field.IndexOf('\n') > -1 ||
                        field.StartsWith(" ") || field.StartsWith("\t") ||
                        field.EndsWith(" ") || field.EndsWith("\t"))
                    {
                        if (field.IndexOf('"') > -1)
                        {
                            //"を""とする
                            field = field.Replace("\"", "\"\"");
                        }
                        field = "\"" + field + "\"";
                    }
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");

                #endregion

                #region 明細出力

                //レコードを書き込む
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < colCount; i++)
                    {
                        //フィールドの取得
                        string field = row[i].ToString();
                        //"で囲む必要があるか調べる
                        if (field.IndexOf('"') > -1 ||
                            field.IndexOf(',') > -1 ||
                            field.IndexOf('\r') > -1 ||
                            field.IndexOf('\n') > -1 ||
                            field.StartsWith(" ") || field.StartsWith("\t") ||
                            field.EndsWith(" ") || field.EndsWith("\t"))
                        {
                            if (field.IndexOf('"') > -1)
                            {
                                //"を""とする
                                field = field.Replace("\"", "\"\"");
                            }
                            field = "\"" + field + "\"";
                        }

                        //フィールドを書き込む
                        sr.Write(field);
                        //カンマを書き込む
                        if (lastColIndex > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する
                    sr.Write("\r\n");
                }

                #endregion

                //閉じる
                sr.Close();

                fi = new System.IO.FileInfo(path);
                //ファイルのサイズを取得
                this.reportFileSize = fi.Length;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".DataTableToCsv", ex);
                return false;
            }
            finally
            {
                if (sr != null) sr.Dispose(); sr = null;
                if (fi != null) fi = null;
            }

            return true;
        }

        public bool DataTableToCsv(DataTable dt)
        {
            System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");
            System.IO.StreamWriter sr = null;
            System.IO.FileInfo fi = null;
            try
            {
                sr = new System.IO.StreamWriter(this.reportFilePath, false, enc);

                int colCount = dt.Columns.Count;
                int lastColIndex = colCount - 1;

                #region ヘッダ出力

                for (int i = 0; i < colCount; i++)
                {
                    //ヘッダの取得
                    string field = dt.Columns[i].Caption;
                    //"で囲む必要があるか調べる
                    if (field.IndexOf('"') > -1 ||
                        field.IndexOf(',') > -1 ||
                        field.IndexOf('\r') > -1 ||
                        field.IndexOf('\n') > -1 ||
                        field.StartsWith(" ") || field.StartsWith("\t") ||
                        field.EndsWith(" ") || field.EndsWith("\t"))
                    {
                        if (field.IndexOf('"') > -1)
                        {
                            //"を""とする
                            field = field.Replace("\"", "\"\"");
                        }
                        field = "\"" + field + "\"";
                    }
                    //フィールドを書き込む
                    sr.Write(field);
                    //カンマを書き込む
                    if (lastColIndex > i)
                    {
                        sr.Write(',');
                    }
                }
                //改行する
                sr.Write("\r\n");

                #endregion

                #region 明細出力

                //レコードを書き込む
                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < colCount; i++)
                    {
                        //フィールドの取得
                        string field = row[i].ToString();
                        //"で囲む必要があるか調べる
                        if (field.IndexOf('"') > -1 ||
                            field.IndexOf(',') > -1 ||
                            field.IndexOf('\r') > -1 ||
                            field.IndexOf('\n') > -1 ||
                            field.StartsWith(" ") || field.StartsWith("\t") ||
                            field.EndsWith(" ") || field.EndsWith("\t"))
                        {
                            if (field.IndexOf('"') > -1)
                            {
                                //"を""とする
                                field = field.Replace("\"", "\"\"");
                            }
                            field = "\"" + field + "\"";
                        }

                        //フィールドを書き込む
                        sr.Write(field);
                        //カンマを書き込む
                        if (lastColIndex > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する
                    sr.Write("\r\n");
                }

                #endregion

                //閉じる
                sr.Close();

                fi = new System.IO.FileInfo(this.reportFilePath);
                //ファイルのサイズを取得
                this.reportFileSize = fi.Length;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".DataTableToCsv", ex);
                return false;
            }
            finally
            {
                if (sr != null) sr.Dispose(); sr = null;
                if (fi != null) fi = null;
            }

            return true;
        }

        #endregion

        #region Report SQL

        public string ReportSQL(string pgId, string companyId, string groupId, string parameters)
        {
            string[] _parameters = parameters.Split(',');
            string sqlWhere = "";
            string sqlOrderBy = "";
            string IdFrom = "";
            string IdTo = "";
            string updateDate = "";
            string orderIndex = "";

            switch (pgId)
            {
                case DataPgEvidence.PGName.Mst.Customer:
                case DataPgEvidence.PGName.Mst.Supplier:
                case DataPgEvidence.PGName.Mst.Purchase:
                case DataPgEvidence.PGName.Mst.Person:
                case DataPgEvidence.PGName.Mst.Commodity:
                case DataPgEvidence.PGName.Mst.Condition:
                case DataPgEvidence.PGName.Mst.Class:
                    IdFrom = _parameters[0];
                    IdTo = _parameters[1];
                    updateDate = _parameters[2];
                    orderIndex = _parameters[3];
                    break;
                default:
                    sqlWhere = _parameters[0];
                    sqlOrderBy = _parameters[1];
                    break;
            }

            switch (pgId)
            {
                #region マスタ一覧表

                case DataPgEvidence.PGName.Mst.Customer:
                    return GetCustomerReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);
                case DataPgEvidence.PGName.Mst.Supplier:
                    return GetSupplierReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);
                case DataPgEvidence.PGName.Mst.Purchase:
                    return GetPurchaseReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);
                case DataPgEvidence.PGName.Mst.Person:
                    return GetPersonReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);
                case DataPgEvidence.PGName.Mst.Commodity:
                    return GetCommodityReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);
                case DataPgEvidence.PGName.Mst.Condition:
                    return GetConditionReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);
                case DataPgEvidence.PGName.Mst.Class:
                    return GetClassReportSQL(companyId, IdFrom, IdTo, updateDate, orderIndex);

                #endregion

                #region 売上系

                case DataPgEvidence.PGName.Order.OrderPrint:
                case DataPgEvidence.PGName.Order.OrderDPrint:
                    return GetOrderListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Estimate.EstimatePrint:
                case DataPgEvidence.PGName.Estimate.EstimateDPrint:
                    return GetEstimateListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Sales.SalesPrint:
                case DataPgEvidence.PGName.Sales.SalesDPrint:
                    return GetSalesListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Sales.SalesDayPrint:
                case DataPgEvidence.PGName.Sales.SalesMonthPrint:
                    return GetSalesDDMMTotalReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Sales.SalesChangePrint:
                    return GetSalesChangeReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Invoice.InvoicePrint:
                    return GetInvoiceListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Receipt.ReceiptDPrint:
                    return GetReceiptListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Receipt.ReceiptDayPrint:
                case DataPgEvidence.PGName.Receipt.ReceiptMonthPrint:
                    return GetReceiptDDMMTotalReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Plan.CollectPlanPrint:
                    return GetCollectPlanReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.SalesManagement.SalesBalancePrint:
                    return GetSalesCreditBalanaceListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.SalesManagement.InvoiceBalancePrint:
                    return GetInvoiceBalanceListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);

                #endregion

                #region 仕入系

                case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderPrint:
                case DataPgEvidence.PGName.PurchaseOrder.PurchaseOrderDPrint:
                    return GetPurchaseOrderListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Purchase.PurchaseDPrint:
                    return GetPurchaseListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Purchase.PurchaseDayPrint:
                case DataPgEvidence.PGName.Purchase.PurchaseMonthPrint:
                    return GetPurchaseDDMMTotalReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Purchase.PurchaseChangePrint:
                    return GetPurchaseChangeReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.PaymentCash.PaymentCashDPrint:
                    return GetPaymentCashListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.PaymentCash.PaymentCashDayPrint:
                case DataPgEvidence.PGName.PaymentCash.PaymentCashMonthPrint:
                    return GetPaymentCashDDMMTotalReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Payment.PaymentPrint:
                    return GetPaymentListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.Plan.PaymentPlanPrint:
                    return GetPaymentPlanReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.PaymentManagement.PaymentCreditBalancePrint:
                    return GetPaymentCreditBalanaceListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);
                case DataPgEvidence.PGName.PaymentManagement.PaymentBalancePrint:
                    return GetPaymentBalanceListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);

                #endregion

                #region 在庫系

                case DataPgEvidence.PGName.InOutDeliver.InOutDeliverDPrint:
                    return GetInOutDeliveryListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);

                case DataPgEvidence.PGName.Inventory.InventoryListPrint:
                    return GetStockInventoryListReportSQL(companyId, groupId, sqlWhere, sqlOrderBy);

                #endregion

            }
            return "";
        }

        #region Master

        private string GetCustomerReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Length = 0;
            sb.Append("SELECT MT.ID AS 得意先ID" + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称" + Environment.NewLine);
            sb.Append("      ,MT.KANA AS カナ" + Environment.NewLine);
            sb.Append("      ,MT.ABOUT_NAME AS 略称" + Environment.NewLine);
            sb.Append("      ,MT.ZIP_CODE AS 郵便番号" + Environment.NewLine);
            //sb.Append("      ,MT.PREFECTURE_ID" + Environment.NewLine);
            //sb.Append("      ,MT.CITY_ID" + Environment.NewLine);
            //sb.Append("      ,MT.TOWN_ID" + Environment.NewLine);
            //sb.Append("      ,MT.ADRESS_CITY" + Environment.NewLine);
            //sb.Append("      ,MT.ADRESS_TOWN" + Environment.NewLine);
            sb.Append("      ,MT.ADRESS1 AS 住所１" + Environment.NewLine);
            sb.Append("      ,MT.ADRESS2 AS 住所２" + Environment.NewLine);
            sb.Append("      ,MT.STATION_NAME AS 部署名" + Environment.NewLine);
            sb.Append("      ,MT.POST_NAME AS 役職名" + Environment.NewLine);
            sb.Append("      ,MT.PERSON_NAME AS 担当名" + Environment.NewLine);
            sb.Append("      ,MT.TITLE_ID AS 敬称ID" + Environment.NewLine);
            sb.Append("      ,MT.TITLE_NAME AS 敬称名" + Environment.NewLine);
            sb.Append("      ,MT.TEL" + Environment.NewLine);
            sb.Append("      ,MT.FAX" + Environment.NewLine);
            sb.Append("      ,MT.MAIL_ADRESS AS ﾒｰﾙｱﾄﾞﾚｽ" + Environment.NewLine);
            //sb.Append("      ,MT.MOBILE_TEL" + Environment.NewLine);
            //sb.Append("      ,MT.MOBILE_ADRESS" + Environment.NewLine);
            //sb.Append("      ,MT.URL" + Environment.NewLine);
            sb.Append("      ,MT.BUSINESS_DIVISION_ID AS 取引区分ID" + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS 取引区分名" + Environment.NewLine);
            sb.Append("      ,MT.UNIT_KIND_ID AS 単価種類ID" + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS 単価種類名" + Environment.NewLine);
            sb.Append("      ,MT.CREDIT_RATE AS 掛率" + Environment.NewLine);
            sb.Append("      ,MT.INVOICE_ID AS 請求先ID" + Environment.NewLine);
            sb.Append("      ,CM.NAME AS 請求先名" + Environment.NewLine);
            sb.Append("      ,MT.TAX_CHANGE_ID AS 税転換ID" + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS 税転換名" + Environment.NewLine);
            sb.Append("      ,MT.SUMMING_UP_GROUP_ID AS 締区分ID" + Environment.NewLine);
            sb.Append("      ,CN.NAME AS 締区分名" + Environment.NewLine);
            sb.Append("      ,MT.RECEIPT_DIVISION_ID AS 入金区分ID" + Environment.NewLine);
            sb.Append("      ,RD.DESCRIPTION AS 入金区分名" + Environment.NewLine);
            sb.Append("      ,MT.COLLECT_CYCLE_ID AS 回収サイクルID" + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS 回収サイクル名" + Environment.NewLine);
            sb.Append("      ,MT.COLLECT_DAY AS 回収日" + Environment.NewLine);
            sb.Append("      ,MT.BILL_SITE AS 手形サイト" + Environment.NewLine);
            sb.Append("      ,MT.PRICE_FRACTION_PROC_ID AS 金額端数処理ID" + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS 金額端数処理名" + Environment.NewLine);
            sb.Append("      ,MT.TAX_FRACTION_PROC_ID AS 税端数処理ID" + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS 税端数処理名" + Environment.NewLine);
            sb.Append("      ,MT.CREDIT_LIMIT_PRICE AS 与信限度額" + Environment.NewLine);
            sb.Append("      ,MT.SALES_CREDIT_PRICE AS 売掛残高" + Environment.NewLine);
            sb.Append("      ,MT.GROUP1_ID AS 得意先分類ID" + Environment.NewLine);
            sb.Append("      ,CL1.NAME AS 得意先分類名" + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID" + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名" + Environment.NewLine);
            sb.Append("      ,MT.MEMO AS 備考" + Environment.NewLine);
            sb.Append("  FROM M_CUSTOMER AS MT" + Environment.NewLine);

            #region Join

            // 請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CM" + Environment.NewLine);
            sb.Append("    ON CM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CM.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CM.ID = MT.INVOICE_ID" + Environment.NewLine);

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = MT.BUSINESS_DIVISION_ID" + Environment.NewLine);

            // 単価種類
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_PRICE_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM2.ID = MT.UNIT_KIND_ID" + Environment.NewLine);

            // 税転換(課税区分)
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_ID + Environment.NewLine);
            sb.Append("   AND NM3.ID = MT.TAX_CHANGE_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = MT.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            // 金額端数処理
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.FRACTION_PROC_ID + Environment.NewLine);
            sb.Append("   AND NM4.ID = MT.PRICE_FRACTION_PROC_ID" + Environment.NewLine);

            // 税端数処理
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.FRACTION_PROC_ID + Environment.NewLine);
            sb.Append("   AND NM5.ID = MT.TAX_FRACTION_PROC_ID" + Environment.NewLine);

            // 回収(入金)区分
            sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
            sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND RD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND RD.ID = MT.RECEIPT_DIVISION_ID" + Environment.NewLine);

            // 回収サイクル
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = MT.COLLECT_CYCLE_ID" + Environment.NewLine);

            // 得意先分類1
            sb.Append("  LEFT JOIN M_CLASS AS CL1" + Environment.NewLine);
            sb.Append("    ON CL1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL1.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL1.CLASS_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CL1.ID = MT.GROUP1_ID" + Environment.NewLine);

            // 得意先分類2
            sb.Append("  LEFT JOIN M_CLASS AS CL2" + Environment.NewLine);
            sb.Append("    ON CL2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL2.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL2.CLASS_DIVISION_ID = 2" + Environment.NewLine);
            sb.Append("   AND CL2.ID = MT.GROUP2_ID" + Environment.NewLine);

            // 得意先分類3
            sb.Append("  LEFT JOIN M_CLASS AS CL3" + Environment.NewLine);
            sb.Append("    ON CL3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL3.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL3.CLASS_DIVISION_ID = 3" + Environment.NewLine);
            sb.Append("   AND CL3.ID = MT.GROUP1_ID" + Environment.NewLine);

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            if (IdFrom != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(ExCast.zCDbl(IdFrom)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(IdFrom) + Environment.NewLine);
                }
            }
            if (IdTo != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(ExCast.zCDbl(IdTo)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(IdTo) + Environment.NewLine);
                }
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            { 
                case 0:
                    sb.Append(" ORDER BY MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 1:
                    sb.Append(" ORDER BY MT.GROUP1_ID " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 2:
                    sb.Append(" ORDER BY MT.KANA " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.GROUP1_ID " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        private string GetSupplierReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Length = 0;
            sb.Append("SELECT MT.CUSTOMER_ID AS 得意先ID" + Environment.NewLine);
            sb.Append("      ,CM.NAME AS 得意先名" + Environment.NewLine);
            sb.Append("      ,MT.ID AS 納入先ID" + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称" + Environment.NewLine);
            sb.Append("      ,MT.KANA AS カナ" + Environment.NewLine);
            sb.Append("      ,MT.ABOUT_NAME AS 略称" + Environment.NewLine);
            sb.Append("      ,MT.ZIP_CODE AS 郵便番号" + Environment.NewLine);
            //sb.Append("      ,MT.PREFECTURE_ID" + Environment.NewLine);
            //sb.Append("      ,MT.CITY_ID" + Environment.NewLine);
            //sb.Append("      ,MT.TOWN_ID" + Environment.NewLine);
            //sb.Append("      ,MT.ADRESS_CITY" + Environment.NewLine);
            //sb.Append("      ,MT.ADRESS_TOWN" + Environment.NewLine);
            sb.Append("      ,MT.ADRESS1 AS 住所１" + Environment.NewLine);
            sb.Append("      ,MT.ADRESS2 AS 住所２" + Environment.NewLine);
            sb.Append("      ,MT.STATION_NAME AS 部署名" + Environment.NewLine);
            sb.Append("      ,MT.POST_NAME AS 役職名" + Environment.NewLine);
            sb.Append("      ,MT.PERSON_NAME AS 担当名" + Environment.NewLine);
            sb.Append("      ,MT.TITLE_ID AS 敬称ID" + Environment.NewLine);
            sb.Append("      ,MT.TITLE_NAME AS 敬称名" + Environment.NewLine);
            sb.Append("      ,MT.TEL" + Environment.NewLine);
            sb.Append("      ,MT.FAX" + Environment.NewLine);
            sb.Append("      ,MT.MAIL_ADRESS AS ﾒｰﾙｱﾄﾞﾚｽ" + Environment.NewLine);
            //sb.Append("      ,MT.MOBILE_TEL" + Environment.NewLine);
            //sb.Append("      ,MT.MOBILE_ADRESS" + Environment.NewLine);
            //sb.Append("      ,MT.URL" + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID" + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名" + Environment.NewLine);
            sb.Append("      ,MT.MEMO AS 備考" + Environment.NewLine);
            sb.Append("  FROM M_SUPPLIER AS MT" + Environment.NewLine);

            #region Join

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CM" + Environment.NewLine);
            sb.Append("    ON CM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CM.COMPANY_ID = MT.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND CM.ID = MT.CUSTOMER_ID" + Environment.NewLine);

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            if (IdFrom != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND CM.ID >= " + ExEscape.zRepStr(ExCast.zCDbl(IdFrom)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND CM.ID >= " + ExEscape.zRepStr(IdFrom) + Environment.NewLine);
                }
            }
            if (IdTo != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND CM.ID <= " + ExEscape.zRepStr(ExCast.zCDbl(IdTo)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND CM.ID <= " + ExEscape.zRepStr(IdTo) + Environment.NewLine);
                }
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            {
                case 0:
                    sb.Append(" ORDER BY IFNULL(CM.ID2, 999999999999999) " + Environment.NewLine);
                    sb.Append("         ,IFNULL(CM.ID, 999999999999999) " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 1:
                    sb.Append(" ORDER BY MT.KANA " + Environment.NewLine);
                    sb.Append("         ,IFNULL(CM.ID2, 999999999999999) " + Environment.NewLine);
                    sb.Append("         ,IFNULL(CM.ID, 999999999999999) " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.GROUP1_ID " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        private string GetPurchaseReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Length = 0;
            sb.Append("SELECT MT.ID AS 仕入先ID" + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称" + Environment.NewLine);
            sb.Append("      ,MT.KANA AS カナ" + Environment.NewLine);
            sb.Append("      ,MT.ABOUT_NAME AS 略称" + Environment.NewLine);
            sb.Append("      ,MT.ZIP_CODE AS 郵便番号" + Environment.NewLine);
            //sb.Append("      ,MT.PREFECTURE_ID" + Environment.NewLine);
            //sb.Append("      ,MT.CITY_ID" + Environment.NewLine);
            //sb.Append("      ,MT.TOWN_ID" + Environment.NewLine);
            //sb.Append("      ,MT.ADRESS_CITY" + Environment.NewLine);
            //sb.Append("      ,MT.ADRESS_TOWN" + Environment.NewLine);
            sb.Append("      ,MT.ADRESS1 AS 住所１" + Environment.NewLine);
            sb.Append("      ,MT.ADRESS2 AS 住所２" + Environment.NewLine);
            sb.Append("      ,MT.STATION_NAME AS 部署名" + Environment.NewLine);
            sb.Append("      ,MT.POST_NAME AS 役職名" + Environment.NewLine);
            sb.Append("      ,MT.PERSON_NAME AS 担当名" + Environment.NewLine);
            sb.Append("      ,MT.TITLE_ID AS 敬称ID" + Environment.NewLine);
            sb.Append("      ,MT.TITLE_NAME AS 敬称名" + Environment.NewLine);
            sb.Append("      ,MT.TEL" + Environment.NewLine);
            sb.Append("      ,MT.FAX" + Environment.NewLine);
            sb.Append("      ,MT.MAIL_ADRESS AS ﾒｰﾙｱﾄﾞﾚｽ" + Environment.NewLine);
            //sb.Append("      ,MT.MOBILE_TEL" + Environment.NewLine);
            //sb.Append("      ,MT.MOBILE_ADRESS" + Environment.NewLine);
            //sb.Append("      ,MT.URL" + Environment.NewLine);
            sb.Append("      ,MT.BUSINESS_DIVISION_ID AS 取引区分ID" + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS 取引区分名" + Environment.NewLine);
            sb.Append("      ,MT.UNIT_KIND_ID AS 単価種類ID" + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS 単価種類名" + Environment.NewLine);
            sb.Append("      ,MT.CREDIT_RATE AS 掛率" + Environment.NewLine);
            //sb.Append("      ,MT.INVOICE_ID AS 請求先ID" + Environment.NewLine);
            //sb.Append("      ,CM.NAME AS 請求先名" + Environment.NewLine);
            sb.Append("      ,MT.TAX_CHANGE_ID AS 税転換ID" + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS 税転換名" + Environment.NewLine);
            sb.Append("      ,MT.SUMMING_UP_GROUP_ID AS 締区分ID" + Environment.NewLine);
            sb.Append("      ,CN.NAME AS 締区分名" + Environment.NewLine);
            sb.Append("      ,MT.PAYMENT_DIVISION_ID AS 支払区分ID" + Environment.NewLine);
            sb.Append("      ,RD.DESCRIPTION AS 支払区分名" + Environment.NewLine);
            sb.Append("      ,MT.PAYMENT_CYCLE_ID AS 支払サイクルID" + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS 支払サイクル名" + Environment.NewLine);
            sb.Append("      ,MT.PAYMENT_DAY AS 支払日" + Environment.NewLine);
            sb.Append("      ,MT.BILL_SITE AS 手形サイト" + Environment.NewLine);
            sb.Append("      ,MT.PRICE_FRACTION_PROC_ID AS 金額端数処理ID" + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS 金額端数処理名" + Environment.NewLine);
            sb.Append("      ,MT.TAX_FRACTION_PROC_ID AS 税端数処理ID" + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS 税端数処理名" + Environment.NewLine);
            //sb.Append("      ,MT.CREDIT_LIMIT_PRICE AS 与信限度額" + Environment.NewLine);
            sb.Append("      ,MT.PAYMENT_CREDIT_PRICE AS 買掛残高" + Environment.NewLine);
            sb.Append("      ,MT.GROUP1_ID AS 仕入先分類ID" + Environment.NewLine);
            sb.Append("      ,CL1.NAME AS 仕入先分類名" + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID" + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名" + Environment.NewLine);
            sb.Append("      ,MT.MEMO AS 備考" + Environment.NewLine);
            sb.Append("  FROM M_PURCHASE AS MT" + Environment.NewLine);

            #region Join

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_PU_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = MT.BUSINESS_DIVISION_ID" + Environment.NewLine);

            // 単価種類
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_PRICE_DIVISION_PU_ID + Environment.NewLine);
            sb.Append("   AND NM2.ID = MT.UNIT_KIND_ID" + Environment.NewLine);

            // 税転換(課税区分)
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_PU_ID + Environment.NewLine);
            sb.Append("   AND NM3.ID = MT.TAX_CHANGE_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = MT.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            // 金額端数処理
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.FRACTION_PROC_ID + Environment.NewLine);
            sb.Append("   AND NM4.ID = MT.PRICE_FRACTION_PROC_ID" + Environment.NewLine);

            // 税端数処理
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.FRACTION_PROC_ID + Environment.NewLine);
            sb.Append("   AND NM5.ID = MT.TAX_FRACTION_PROC_ID" + Environment.NewLine);

            // 支払区分
            sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
            sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND RD.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND RD.ID = MT.PAYMENT_DIVISION_ID" + Environment.NewLine);

            // 支払サイクル
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = MT.PAYMENT_CYCLE_ID" + Environment.NewLine);

            // 仕入先分類1
            sb.Append("  LEFT JOIN M_CLASS AS CL1" + Environment.NewLine);
            sb.Append("    ON CL1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL1.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL1.CLASS_DIVISION_ID = 7" + Environment.NewLine);
            sb.Append("   AND CL1.ID = MT.GROUP1_ID" + Environment.NewLine);

            // 仕入先分類2
            sb.Append("  LEFT JOIN M_CLASS AS CL2" + Environment.NewLine);
            sb.Append("    ON CL2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL2.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL2.CLASS_DIVISION_ID = 8" + Environment.NewLine);
            sb.Append("   AND CL2.ID = MT.GROUP2_ID" + Environment.NewLine);

            // 仕入先分類3
            sb.Append("  LEFT JOIN M_CLASS AS CL3" + Environment.NewLine);
            sb.Append("    ON CL3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL3.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL3.CLASS_DIVISION_ID = 9" + Environment.NewLine);
            sb.Append("   AND CL3.ID = MT.GROUP1_ID" + Environment.NewLine);

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            if (IdFrom != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(ExCast.zCDbl(IdFrom)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(IdFrom) + Environment.NewLine);
                }
            }
            if (IdTo != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(ExCast.zCDbl(IdTo)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(IdTo) + Environment.NewLine);
                }
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            {
                case 0:
                    sb.Append(" ORDER BY MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 1:
                    sb.Append(" ORDER BY MT.GROUP1_ID " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 2:
                    sb.Append(" ORDER BY MT.KANA " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.GROUP1_ID " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        private string GetPersonReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Length = 0;

            sb.Append("SELECT MT.ID AS 担当ID" + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称" + Environment.NewLine);
            sb.Append("      ,MT.GROUP_ID AS グループID" + Environment.NewLine);
            sb.Append("      ,SG.NAME AS グループ名" + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID" + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名" + Environment.NewLine);
            sb.Append("      ,MT.MEMO AS 備考" + Environment.NewLine);
            sb.Append("  FROM M_PERSON AS MT" + Environment.NewLine);

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND MT.DISPLAY_FLG = NM.ID" + Environment.NewLine);

            // グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS SG" + Environment.NewLine);
            sb.Append("    ON SG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SG.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND SG.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND SG.ID = MT.GROUP_ID" + Environment.NewLine);

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            if (IdFrom != "")
            {
                sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(IdFrom) + Environment.NewLine);
            }
            if (IdTo != "")
            {
                sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(IdTo) + Environment.NewLine);
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            {
                case 0:
                    sb.Append(" ORDER BY MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.NAME " + Environment.NewLine);
                    break;
                case 1:
                    sb.Append(" ORDER BY MT.GROUP_ID " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.NAME " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        private string GetCommodityReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Length = 0;

            sb.Append("SELECT MT.ID AS 商品ID" + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称" + Environment.NewLine);
            sb.Append("      ,MT.KANA AS カナ" + Environment.NewLine);
            sb.Append("      ,MT.UNIT_ID AS 単位ID" + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS 単位名" + Environment.NewLine);
            sb.Append("      ,MT.ENTER_NUMBER AS 入数 " + Environment.NewLine);
            sb.Append("      ,MT.NUMBER_DECIMAL_DIGIT AS 数量小数桁 " + Environment.NewLine);
            sb.Append("      ,MT.UNIT_DECIMAL_DIGIT AS 単位小数桁 " + Environment.NewLine);
            sb.Append("      ,MT.TAXATION_DIVISION_ID AS 課税区分ID " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS 課税区分名 " + Environment.NewLine);
            sb.Append("      ,MT.INVENTORY_MANAGEMENT_DIVISION_ID AS 在庫管理区分ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS 在庫管理区分名 " + Environment.NewLine);
            sb.Append("      ,MT.PURCHASE_LOT AS 発注ロット " + Environment.NewLine);
            sb.Append("      ,MT.JUST_INVENTORY_NUMBER AS 適正在庫数 " + Environment.NewLine);
            sb.Append("      ,MT.INVENTORY_NUMBER AS 現在庫数 " + Environment.NewLine);
            //sb.Append("      ,MT.INVENTORY_EVALUATION_ID AS 在庫評価ID " + Environment.NewLine);
            //sb.Append("      ,NM4.DESCRIPTION AS 在庫評価名 " + Environment.NewLine);
            sb.Append("      ,MT.MAIN_PURCHASE_ID AS 主仕入先ID" + Environment.NewLine);
            sb.Append("      ,PU.NAME AS 主仕入先名" + Environment.NewLine);
            sb.Append("      ,MT.LEAD_TIME AS リードタイム" + Environment.NewLine);
            sb.Append("      ,MT.RETAIL_PRICE_SKIP_TAX AS 上代税抜 " + Environment.NewLine);
            sb.Append("      ,MT.RETAIL_PRICE_BEFORE_TAX AS 上代税込 " + Environment.NewLine);
            sb.Append("      ,MT.SALES_UNIT_PRICE_SKIP_TAX AS 売上単価税抜 " + Environment.NewLine);
            sb.Append("      ,MT.SALES_UNIT_PRICE_BEFORE_TAX AS 売上単価税込 " + Environment.NewLine);
            sb.Append("      ,MT.SALES_COST_PRICE_SKIP_TAX AS 売上原価税抜 " + Environment.NewLine);
            sb.Append("      ,MT.SALES_COST_PRICE_BEFORE_TAX AS 売上原価税込 " + Environment.NewLine);
            sb.Append("      ,MT.PURCHASE_UNIT_PRICE_SKIP_TAX AS 仕入単価税抜 " + Environment.NewLine);
            sb.Append("      ,MT.PURCHASE_UNIT_PRICE_BEFORE_TAX AS 仕入単価税込 " + Environment.NewLine);
            sb.Append("      ,MT.GROUP1_ID AS 商品分類ID " + Environment.NewLine);
            sb.Append("      ,CL1.NAME AS 商品分類名 " + Environment.NewLine);
            //sb.Append("      ,MT.GROUP2_ID " + Environment.NewLine);
            //sb.Append("      ,CL2.NAME AS GROUP2_NAME " + Environment.NewLine);
            //sb.Append("      ,MT.GROUP3_ID " + Environment.NewLine);
            //sb.Append("      ,CL3.NAME AS GROUP3_NAME " + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID " + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名 " + Environment.NewLine);
            sb.Append("      ,MT.MEMO AS 備考" + Environment.NewLine);
            sb.Append("  FROM M_COMMODITY AS MT" + Environment.NewLine);

            #region Join

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
            sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PU.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND PU.ID = MT.MAIN_PURCHASE_ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = MT.UNIT_ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM2.ID = MT.TAXATION_DIVISION_ID" + Environment.NewLine);

            // 在庫管理区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INVENTORY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM3.ID = MT.INVENTORY_MANAGEMENT_DIVISION_ID" + Environment.NewLine);

            // 在庫評価
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.INVENTORY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM4.ID = MT.INVENTORY_EVALUATION_ID" + Environment.NewLine);

            // 商品分類1
            sb.Append("  LEFT JOIN M_CLASS AS CL1" + Environment.NewLine);
            sb.Append("    ON CL1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL1.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL1.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.CommodityGrouop1 + Environment.NewLine);
            sb.Append("   AND CL1.ID = MT.GROUP1_ID" + Environment.NewLine);

            // 商品分類2
            sb.Append("  LEFT JOIN M_CLASS AS CL2" + Environment.NewLine);
            sb.Append("    ON CL2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL2.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL2.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.CommodityGrouop2 + Environment.NewLine);
            sb.Append("   AND CL2.ID = MT.GROUP2_ID" + Environment.NewLine);

            // 商品分類3
            sb.Append("  LEFT JOIN M_CLASS AS CL3" + Environment.NewLine);
            sb.Append("    ON CL3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CL3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CL3.COMPANY_ID = MT.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CL3.CLASS_DIVISION_ID = " + (int)CommonUtl.geMGroupKbn.CommodityGrouop3 + Environment.NewLine);
            sb.Append("   AND CL3.ID = MT.GROUP3_ID" + Environment.NewLine);

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            if (IdFrom != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(ExCast.zCDbl(IdFrom)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(IdFrom) + Environment.NewLine);
                }
            }
            if (IdTo != "")
            {
                if (ExCast.IsNumeric(IdFrom))
                {
                    sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(ExCast.zCDbl(IdTo)) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(IdTo) + Environment.NewLine);
                }
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            {
                case 0:
                    sb.Append(" ORDER BY MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 1:
                    sb.Append(" ORDER BY MT.GROUP1_ID " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    sb.Append("         ,MT.KANA " + Environment.NewLine);
                    break;
                case 2:
                    sb.Append(" ORDER BY MT.KANA " + Environment.NewLine);
                    sb.Append("         ,MT.ID2 " + Environment.NewLine);
                    sb.Append("         ,MT.ID " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        private string GetConditionReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            sb.Length = 0;

            sb.Append("SELECT MT.ID AS 締区分ID " + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称 " + Environment.NewLine);
            //sb.Append("      ,MT.MEMO AS 備考 " + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID" + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名" + Environment.NewLine);
            sb.Append("  FROM M_CONDITION AS MT" + Environment.NewLine);

            #region Join

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            if (IdFrom != "")
            {
                sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(string.Format("{0:00}", ExCast.zCDbl(IdFrom))) + Environment.NewLine);
            }
            if (IdTo != "")
            {
                sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(string.Format("{0:00}", ExCast.zCDbl(IdTo))) + Environment.NewLine);
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            {
                case 0:
                    sb.Append(" ORDER BY MT.ID " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        private string GetClassReportSQL(string companyId, string IdFrom, string IdTo, string updateDate, string orderIndex)
        {
            StringBuilder sb = new StringBuilder();

            string[] _prmFrom = IdFrom.Replace("<<@escape_comma@>>",",").Split(',');
            string[] _prmTo = IdTo.Replace("<<@escape_comma@>>", ",").Split(',');

            string _IdFrom = "";
            string _IdTo = "";
            int _Kbn = ExCast.zCInt(_prmFrom[0]);

            if (_prmFrom.Length > 1) _IdFrom = _prmFrom[1];
            if (_prmTo.Length > 1) _IdTo = _prmTo[1];

            sb.Length = 0;

            sb.Append("SELECT MT.ID AS 分類ID " + Environment.NewLine);
            sb.Append("      ,MT.NAME AS 名称 " + Environment.NewLine);
            //sb.Append("      ,MT.MEMO AS 備考 " + Environment.NewLine);
            sb.Append("      ,MT.DISPLAY_FLG AS 表示区分ID" + Environment.NewLine);
            sb.Append("      ,NM.DESCRIPTION AS 表示区分名" + Environment.NewLine);
            sb.Append("  FROM M_CLASS AS MT" + Environment.NewLine);

            #region Join

            // 表示区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM" + Environment.NewLine);
            sb.Append("    ON NM.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DISPLAY_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM.ID = MT.DISPLAY_FLG" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE MT.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND MT.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND MT.CLASS_DIVISION_ID = " + _Kbn + Environment.NewLine);
            if (_IdFrom != "")
            {
                sb.Append("   AND MT.ID >= " + ExEscape.zRepStr(_IdFrom) + Environment.NewLine);
            }
            if (_IdTo != "")
            {
                sb.Append("   AND MT.ID <= " + ExEscape.zRepStr(_IdTo) + Environment.NewLine);
            }
            if (updateDate != "")
            {
                sb.Append("   AND MT.UPDATE_DATE >= " + ExEscape.zRepStr(updateDate) + Environment.NewLine);
            }

            switch (ExCast.zCInt(orderIndex))
            {
                case 0:
                    sb.Append(" ORDER BY MT.ID " + Environment.NewLine);
                    break;
            }

            //sb.Append(" LIMIT 0, 1000");

            return sb.ToString();

        }

        #endregion

        #region Sales

        public string GetEstimateListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,CONCAT('御見積合計金額 ','\',' '" + ", FORMAT(T.SUM_NO_TAX_PRICE + T.SUM_TAX, '#,##0'), '－') AS SUM_PRICE_TITLE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm("見積", "明細表"));
            sb.Append(GetTotalKbn());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,T.STATE " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS STATE_NM " + Environment.NewLine);
            sb.Append("      ,replace(date_format(T.ESTIMATE_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS ESTIMATE_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.CUSTOMER_ID END AS CUSTOMER_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS CUSTOMER_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CS.TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS CUSTOMER_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS CUSTOMER_PERSON_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NM " + Environment.NewLine);
            sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS BISNESS_DIVISON_NM " + Environment.NewLine);
            sb.Append("      ,T.SUPPLIER_ID " + Environment.NewLine);
            sb.Append("      ,SU.NAME AS SUPPLIER_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') = '' THEN '' " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), ' 頃') END AS SUPPLY_YMD " + Environment.NewLine);
            sb.Append("      ,CASE WHEN replace(date_format(T.TIME_LIMIT_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') = '' THEN '' " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(date_format(T.TIME_LIMIT_YMD , " + ExEscape.SQL_YMD + "), ' 頃迄') END AS TIME_LIMIT_YMD " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);
            sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_SALES_COST " + Environment.NewLine);
            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_PROFITS " + Environment.NewLine);
            sb.Append("      ,T.PROFITS_PERCENT " + Environment.NewLine);
            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.SALES_COST " + Environment.NewLine);
            sb.Append("      ,OD.TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS_PERCENT AS D_PROFITS_PERCENT" + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_ESTIMATE_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_ESTIMATE_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.ESTIMATE_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 税転換
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_ID + Environment.NewLine);
            sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

            // 納入先
            sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
            sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
            sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM2.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            // 承認状態
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM7" + Environment.NewLine);
            sb.Append("    ON NM7.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM7.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM7.DIVISION_ID = " + (int)CommonUtl.geNameKbn.APPROVAL_STATE_ID + Environment.NewLine);
            sb.Append("   AND NM7.ID = T.STATE" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetOrderListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,CONCAT('御注文合計金額 ','\',' '" + ", FORMAT(T.SUM_NO_TAX_PRICE + T.SUM_TAX, '#,##0'), '－') AS SUM_PRICE_TITLE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
            sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm("受注", "明細表"));
            sb.Append(GetTotalKbn());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.ESTIMATENO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS ESTIMATENO" + Environment.NewLine);
            sb.Append("      ,replace(date_format(T.ORDER_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS ORDER_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.CUSTOMER_ID END AS CUSTOMER_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS CUSTOMER_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CS.TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS CUSTOMER_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS CUSTOMER_PERSON_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NM " + Environment.NewLine);
            sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS BISNESS_DIVISON_NM " + Environment.NewLine);
            sb.Append("      ,T.SUPPLIER_ID " + Environment.NewLine);
            sb.Append("      ,SU.NAME AS SUPPLIER_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') = '' THEN '' " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), ' まで') END AS SUPPLY_YMD " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);
            sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_SALES_COST " + Environment.NewLine);
            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_PROFITS " + Environment.NewLine);
            sb.Append("      ,T.PROFITS_PERCENT " + Environment.NewLine);
            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.SALES_COST " + Environment.NewLine);
            sb.Append("      ,OD.TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS_PERCENT AS D_PROFITS_PERCENT" + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " + 
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_ORDER_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_ORDER_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.ORDER_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 税転換
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_ID + Environment.NewLine);
            sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

            // 納入先
            sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
            sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
            sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM2.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetSalesListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _issue_ymd = "";
            int issue_index = strWhereSql.IndexOf("<issue ymd>");
            if (issue_index != -1)
            {
                _issue_ymd = strWhereSql.Substring(issue_index + 11, 10);
                strWhereSql = strWhereSql.Substring(0, issue_index);
            }

            sb.Append("SELECT lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,'' AS SUM_PRICE_TITLE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,'" + _issue_ymd + "' AS ISSUE_YMD " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm("売上", "明細表"));
            sb.Append(GetTotalKbn());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.ESTIMATENO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS ESTIMATENO" + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.ORDER_NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS ORDER_NO" + Environment.NewLine);
            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS SALES_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.CUSTOMER_ID END AS CUSTOMER_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS CUSTOMER_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CS.TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS CUSTOMER_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS CUSTOMER_PERSON_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NM " + Environment.NewLine);
            sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS BISNESS_DIVISON_NM " + Environment.NewLine);
            sb.Append("      ,T.SUPPLIER_ID " + Environment.NewLine);
            sb.Append("      ,SU.NAME AS SUPPLIER_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') = '' THEN '' " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), ' まで') END AS SUPPLY_YMD " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(CS2.ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CS2.ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE CS2.ID END AS INVOICE_ID " + Environment.NewLine);
            sb.Append("      ,CS2.NAME AS INVOICE_NM " + Environment.NewLine);
            sb.Append("      ,IFNULL(IV.NO, '未') AS INVOICE_NO " + Environment.NewLine);
            sb.Append("      ,CS2.SALES_CREDIT_PRICE " + Environment.NewLine);
            sb.Append("      ,CS2.CREDIT_LIMIT_PRICE " + Environment.NewLine);
            sb.Append("      ,CS.UNIT_KIND_ID " + Environment.NewLine);
            sb.Append("      ,NM7.DESCRIPTION AS UNIT_KIND_NM " + Environment.NewLine);
            sb.Append("      ,CS.CREDIT_RATE " + Environment.NewLine);

            sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_SALES_COST " + Environment.NewLine);
            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_PROFITS " + Environment.NewLine);
            sb.Append("      ,T.PROFITS_PERCENT " + Environment.NewLine);

            sb.Append("      ,T.DELIVERY_PRINT_FLG " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.DELIVERY_PRINT_FLG, 0) = 0 THEN '発行未' " + Environment.NewLine);
            sb.Append("            ELSE '発行済' END AS DELIVERY_PRINT_FLG_NM " + Environment.NewLine);

            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.SALES_COST " + Environment.NewLine);
            sb.Append("      ,OD.TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS_PERCENT AS D_PROFITS_PERCENT" + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_SALES_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_SALES_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.SALES_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS2" + Environment.NewLine);
            sb.Append("    ON CS2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS2.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS2.ID = CS.INVOICE_ID" + Environment.NewLine);

            // 請求データ
            sb.Append("  LEFT JOIN T_INVOICE AS IV" + Environment.NewLine);
            sb.Append("    ON IV.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND IV.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND IV.NO = T.INVOICE_NO" + Environment.NewLine);


            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 税転換
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_ID + Environment.NewLine);
            sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

            // 納入先
            sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
            sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
            sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM2.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            // 単価種類
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM7" + Environment.NewLine);
            sb.Append("    ON NM7.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM7.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM7.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_PRICE_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM7.ID = CS.UNIT_KIND_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetSalesDDMMTotalReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _group_kbn = "";
            int group_kbn = strWhereSql.IndexOf("<group kbn>");
            if (group_kbn != -1)
            {
                _group_kbn = strWhereSql.Substring(group_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, group_kbn);
            }

            sb.Append("SELECT T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (_group_kbn == "1")
            {
                sb.Append(GetTitleNm("売上", "日報"));
            }
            else
            {
                sb.Append(GetTitleNm("売上", "月報"));
            }
            sb.Append(GetTotalKbnDDMM(ExCast.zCInt(_group_kbn)));

            sb.Append("      ,SUM(T.SUM_ENTER_NUMBER) AS SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_CASE_NUMBER) AS SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_NUMBER) AS SUM_NUMBER" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_UNIT_PRICE) AS SUM_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_SALES_COST) AS SUM_SALES_COST" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_PROFITS) AS SUM_PROFITS" + Environment.NewLine);
            
            sb.Append("      ,IFNULL(SUM(OD_RETURN.UNIT_PRICE), 0) * -1 AS RETURN_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.SALES_COST), 0) * -1 AS RETURN_SALES_COST" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.TAX), 0) * -1 AS RETURN_TAX" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.NO_TAX_PRICE), 0) * -1 AS RETURN_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.PRICE), 0) * -1 AS RETURN_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.PROFITS), 0) * -1 AS RETURN_PROFITS" + Environment.NewLine);

            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.UNIT_PRICE), 0) * -1 AS NEBIKI_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.SALES_COST), 0) * -1 AS NEBIKI_SALES_COST" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.TAX), 0) * -1 AS NEBIKI_TAX" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.NO_TAX_PRICE), 0) * -1 AS NEBIKI_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.PRICE), 0) * -1 AS NEBIKI_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.PROFITS), 0) * -1 AS NEBIKI_PROFITS" + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_SALES_H AS T" + Environment.NewLine);

            #region Join

            if (this.entitySetting.total_kbn == 3)
            {
                // 商品用
                sb.Append("  LEFT JOIN T_SALES_D AS CD" + Environment.NewLine);
                sb.Append("    ON CD.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("   AND CD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                sb.Append("   AND CD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND CD.SALES_ID = T.ID" + Environment.NewLine);
            }

            sb.Append("  LEFT JOIN T_SALES_D AS OD_RETURN" + Environment.NewLine);
            sb.Append("    ON OD_RETURN.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.BREAKDOWN_ID = 6" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.SALES_ID = T.ID" + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_NEBIKI" + Environment.NewLine);
            sb.Append("    ON OD_NEBIKI.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.BREAKDOWN_ID = 2" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.SALES_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            //// 内訳
            //sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            //sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            //sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            //sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            //sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            //sb.Append(" LIMIT 0, 1000");

            #region Group by


            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 1:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.CUSTOMER_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE T.CUSTOMER_ID END" + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PERSON_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY T.PERSON_ID " + Environment.NewLine);
                        break;
                    case 3:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,CD.COMMODITY_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(CD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE CD.COMMODITY_ID END" + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        if (_group_kbn == "1")
                        {
                            // 日報時
                            sb.Append("         ,T.SALES_YMD " + Environment.NewLine);
                            sb.Append(" ORDER BY T.SALES_YMD " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("         ,date_format(T.SALES_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                            sb.Append(" ORDER BY date_format(T.SALES_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,CG.NAME " + Environment.NewLine);
                if (_group_kbn == "1")
                {
                    // 日報時
                    sb.Append("         ,T.SALES_YMD " + Environment.NewLine);
                    sb.Append(" ORDER BY T.SALES_YMD " + Environment.NewLine);
                }
                else
                {
                    // 月報時
                    sb.Append("         ,date_format(T.SALES_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                    sb.Append(" ORDER BY date_format(T.SALES_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                }
            }

            #endregion

            #endregion

            return sb.ToString();

        }

        public string GetSalesChangeReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _group_kbn = "";
            int group_kbn = strWhereSql.IndexOf("<group kbn>");
            if (group_kbn != -1)
            {
                _group_kbn = strWhereSql.Substring(group_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, group_kbn);
            }
            int total_kbn = 1;

            sb.Append("SELECT T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            sb.Append(GetTitleNm("売上", "推移表"));
            sb.Append(GetTotalKbnDDMM(3));

            if (total_kbn == 1)
            {
                sb.Append("      ,IFNULL(SUM(OD_JANUARY.PROFITS), 0) AS JANUARY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_FEBRUARY.PROFITS), 0) AS FEBRUARY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_MARCH.PROFITS), 0) AS MARCH_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_APRIL.PROFITS), 0) AS APRIL_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_MAY.PROFITS), 0) AS MAY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_JUNE.PROFITS), 0) AS JUNE_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_JULY.PROFITS), 0) AS JULY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_AUGUST.PROFITS), 0) AS AUGUST_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_SEPTEMBER.PROFITS), 0) AS SEPTEMBER_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_OCTOBER.PROFITS), 0) AS OCTOBER_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_NOVEMBER.PROFITS), 0) AS NOVEMBER_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_DECEMBER.PROFITS), 0) AS DECEMBER_SUM_PRICE" + Environment.NewLine);

                sb.Append("      ,IFNULL(SUM(OD_JANUARY.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_FEBRUARY.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_MARCH.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_APRIL.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_MAY.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_JUNE.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_JULY.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_AUGUST.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_SEPTEMBER.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_OCTOBER.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_NOVEMBER.PROFITS), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_DECEMBER.PROFITS), 0) AS TOTAL_PRICE" + Environment.NewLine);
            }
            else
            {
                sb.Append("      ,IFNULL(SUM(OD_JANUARY.NO_TAX_PRICE), 0) AS JANUARY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_FEBRUARY.NO_TAX_PRICE), 0) AS FEBRUARY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_MARCH.NO_TAX_PRICE), 0) AS MARCH_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_APRIL.NO_TAX_PRICE), 0) AS APRIL_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_MAY.NO_TAX_PRICE), 0) AS MAY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_JUNE.NO_TAX_PRICE), 0) AS JUNE_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_JULY.NO_TAX_PRICE), 0) AS JULY_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_AUGUST.NO_TAX_PRICE), 0) AS AUGUST_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_SEPTEMBER.NO_TAX_PRICE), 0) AS SEPTEMBER_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_OCTOBER.NO_TAX_PRICE), 0) AS OCTOBER_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_NOVEMBER.NO_TAX_PRICE), 0) AS NOVEMBER_SUM_PRICE" + Environment.NewLine);
                sb.Append("      ,IFNULL(SUM(OD_DECEMBER.NO_TAX_PRICE), 0) AS DECEMBER_SUM_PRICE" + Environment.NewLine);

                sb.Append("      ,IFNULL(SUM(OD_JANUARY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_FEBRUARY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_MARCH.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_APRIL.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_MAY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_JUNE.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_JULY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_AUGUST.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_SEPTEMBER.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_OCTOBER.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_NOVEMBER.NO_TAX_PRICE), 0) + " + Environment.NewLine);
                sb.Append("       IFNULL(SUM(OD_DECEMBER.NO_TAX_PRICE), 0) AS TOTAL_PRICE" + Environment.NewLine);
            }

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_SALES_H AS T" + Environment.NewLine);

            #region Join

            #region 月別集計

            if (this.entitySetting.total_kbn == 3)
            {
                // 商品用
                sb.Append("  CROSS JOIN (SELECT DISTINCT COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,COMMODITY_NAME" + Environment.NewLine);
                sb.Append("               FROM T_SALES_D AS SD" + Environment.NewLine);
                sb.Append("              WHERE SD.DELETE_FLG = 0) AS CD" + Environment.NewLine);

                //sb.Append("  LEFT JOIN T_SALES_D AS CD" + Environment.NewLine);
                //sb.Append("    ON CD.DELETE_FLG = 0" + Environment.NewLine);
                //sb.Append("   AND CD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                //sb.Append("   AND CD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
                //sb.Append("   AND CD.SALES_ID = T.ID" + Environment.NewLine);

                //sb.Append("  LEFT JOIN M_COMMODITY AS CD" + Environment.NewLine);
                //sb.Append("    ON CD.DELETE_FLG = 0 " + Environment.NewLine);
                //sb.Append("   AND CD.DISPLAY_FLG = 1 " + Environment.NewLine);
                //sb.Append("   AND CD.COMPANY_ID = " + companyId + Environment.NewLine);
            }

            #region January

            sb.Append("  LEFT JOIN T_SALES_H AS HD_JANUARY" + Environment.NewLine);
            sb.Append("    ON HD_JANUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_JANUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_JANUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_JANUARY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_JANUARY.SALES_YMD) = 1 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_JANUARY" + Environment.NewLine);
            sb.Append("    ON OD_JANUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_JANUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_JANUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_JANUARY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_JANUARY.SALES_ID = HD_JANUARY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_JANUARY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region February

            sb.Append("  LEFT JOIN T_SALES_H AS HD_FEBRUARY" + Environment.NewLine);
            sb.Append("    ON HD_FEBRUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_FEBRUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_FEBRUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_FEBRUARY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_FEBRUARY.SALES_YMD) = 2 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_FEBRUARY" + Environment.NewLine);
            sb.Append("    ON OD_FEBRUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_FEBRUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_FEBRUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_FEBRUARY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_FEBRUARY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_FEBRUARY.SALES_ID = HD_FEBRUARY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_FEBRUARY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region March

            sb.Append("  LEFT JOIN T_SALES_H AS HD_MARCH" + Environment.NewLine);
            sb.Append("    ON HD_MARCH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_MARCH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_MARCH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_MARCH.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_MARCH.SALES_YMD) = 3 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_MARCH" + Environment.NewLine);
            sb.Append("    ON OD_MARCH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_MARCH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_MARCH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_MARCH.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_MARCH.SALES_ID = HD_MARCH.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_MARCH.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region April

            sb.Append("  LEFT JOIN T_SALES_H AS HD_APRIL" + Environment.NewLine);
            sb.Append("    ON HD_APRIL.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_APRIL.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_APRIL.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_APRIL.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_APRIL.SALES_YMD) = 4 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_APRIL" + Environment.NewLine);
            sb.Append("    ON OD_APRIL.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_APRIL.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_APRIL.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_APRIL.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_APRIL.SALES_ID = HD_APRIL.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_APRIL.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region May

            sb.Append("  LEFT JOIN T_SALES_H AS HD_MAY" + Environment.NewLine);
            sb.Append("    ON HD_MAY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_MAY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_MAY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_MAY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_MAY.SALES_YMD) = 5 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_MAY" + Environment.NewLine);
            sb.Append("    ON OD_MAY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_MAY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_MAY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_MAY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_MAY.SALES_ID = HD_MAY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_MAY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region June

            sb.Append("  LEFT JOIN T_SALES_H AS HD_JUNE" + Environment.NewLine);
            sb.Append("    ON HD_JUNE.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_JUNE.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_JUNE.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_JUNE.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_JUNE.SALES_YMD) = 6 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_JUNE" + Environment.NewLine);
            sb.Append("    ON OD_JUNE.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_JUNE.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_JUNE.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_JUNE.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_JUNE.SALES_ID = HD_JUNE.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_JUNE.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region July

            sb.Append("  LEFT JOIN T_SALES_H AS HD_JULY" + Environment.NewLine);
            sb.Append("    ON HD_JULY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_JULY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_JULY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_JULY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_JULY.SALES_YMD) = 7 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_JULY" + Environment.NewLine);
            sb.Append("    ON OD_JULY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_JULY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_JULY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_JULY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_JULY.SALES_ID = HD_JULY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_JULY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region August

            sb.Append("  LEFT JOIN T_SALES_H AS HD_AUGUST" + Environment.NewLine);
            sb.Append("    ON HD_AUGUST.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_AUGUST.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_AUGUST.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_AUGUST.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_AUGUST.SALES_YMD) = 8 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_AUGUST" + Environment.NewLine);
            sb.Append("    ON OD_AUGUST.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_AUGUST.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_AUGUST.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_AUGUST.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_AUGUST.SALES_ID = HD_AUGUST.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_AUGUST.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region September

            sb.Append("  LEFT JOIN T_SALES_H AS HD_SEPTEMBER" + Environment.NewLine);
            sb.Append("    ON HD_SEPTEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_SEPTEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_SEPTEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_SEPTEMBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_SEPTEMBER.SALES_YMD) = 9 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_SEPTEMBER" + Environment.NewLine);
            sb.Append("    ON OD_SEPTEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_SEPTEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_SEPTEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_SEPTEMBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_SEPTEMBER.SALES_ID = HD_SEPTEMBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_SEPTEMBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region October

            sb.Append("  LEFT JOIN T_SALES_H AS HD_OCTOBER" + Environment.NewLine);
            sb.Append("    ON HD_OCTOBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_OCTOBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_OCTOBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_OCTOBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_OCTOBER.SALES_YMD) = 10 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_OCTOBER" + Environment.NewLine);
            sb.Append("    ON OD_OCTOBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_OCTOBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_OCTOBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_OCTOBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_OCTOBER.SALES_ID = HD_OCTOBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_OCTOBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region November

            sb.Append("  LEFT JOIN T_SALES_H AS HD_NOVEMBER" + Environment.NewLine);
            sb.Append("    ON HD_NOVEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_NOVEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_NOVEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_NOVEMBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_NOVEMBER.SALES_YMD) = 11 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_NOVEMBER" + Environment.NewLine);
            sb.Append("    ON OD_NOVEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_NOVEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_NOVEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_NOVEMBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_NOVEMBER.SALES_ID = HD_NOVEMBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_NOVEMBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region December

            sb.Append("  LEFT JOIN T_SALES_H AS HD_DECEMBER" + Environment.NewLine);
            sb.Append("    ON HD_DECEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_DECEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_DECEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_DECEMBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_DECEMBER.SALES_YMD) = 10 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_SALES_D AS OD_DECEMBER" + Environment.NewLine);
            sb.Append("    ON OD_DECEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_DECEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_DECEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純売上用
                sb.Append("   AND OD_DECEMBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_DECEMBER.SALES_ID = HD_DECEMBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_DECEMBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #endregion

            // 担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            //sb.Append(" LIMIT 0, 1000");

            #region Group by

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 1:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.CUSTOMER_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE T.CUSTOMER_ID END" + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PERSON_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY T.PERSON_ID " + Environment.NewLine);
                        break;
                    case 3:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,CD.COMMODITY_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(CD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE CD.COMMODITY_ID END" + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,date_format(T.SALES_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
                        sb.Append(" ORDER BY date_format(T.SALES_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
                        break;
                }
            }
            else
            {
                sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,CG.NAME " + Environment.NewLine);
                sb.Append("         ,date_format(T.SALES_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
                sb.Append(" ORDER BY date_format(T.SALES_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
            }

            #endregion

            #endregion

            return sb.ToString();

        }

        public string GetInvoiceListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _issue_ymd = "";
            int issue_index = strWhereSql.IndexOf("<issue ymd>");
            if (issue_index != -1)
            {
                _issue_ymd = strWhereSql.Substring(issue_index + 11, 10);
                strWhereSql = strWhereSql.Substring(0, issue_index);
            }

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,'" + _issue_ymd + "' AS ISSUE_YMD " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.INVOICE_ID END AS INVOICE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS INVOICE_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS INVOICE_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS CUSTOMER_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS CUSTOMER_PERSON_NM_TITLE_NAME " + Environment.NewLine);

            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);
            sb.Append("      ,CG.BANK_NAME AS COMPANY_BANK_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_BRANCH_NAME AS COMPANY_BANK_BRANCH_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NO AS COMPANY_BANK_ACCOUNT_NO " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NAME AS COMPANY_BANK_ACCOUNT_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KANA AS COMPANY_BANK_ACCOUNT_KANA " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KBN AS BANK_ACCOUNT_KBN " + Environment.NewLine);
            sb.Append("      ,IFNULL(NM2.DESCRIPTION, '普通') AS BANK_ACCOUNT_KBN_NM " + Environment.NewLine);

            sb.Append("      ,CONCAT('口座名義：', CG.BANK_ACCOUNT_NAME, ' カナ表記：', CG.BANK_ACCOUNT_KANA) AS ACCOUNT_INF1 " + Environment.NewLine);
            sb.Append("      ,CONCAT(CG.BANK_NAME, ' ', CG.BANK_BRANCH_NAME, ' ', IFNULL(NM2.DESCRIPTION, '普通'), ' ', CG.BANK_ACCOUNT_NO) AS ACCOUNT_INF2 " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS INVOICE_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.SUMMING_UP_GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.BEFORE_INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS BEFORE_INVOICE_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.COLLECT_CYCLE_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS COLLECT_CYCLE_NM " + Environment.NewLine);

            sb.Append("      ,T.COLLECT_DAY " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.COLLECT_PLAN_DAY , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS COLLECT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,T.BEFORE_INVOICE_PRICE " + Environment.NewLine);
            sb.Append("      ,T.RECEIPT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TRANSFER_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SALES_PRICE " + Environment.NewLine);
            sb.Append("      ,T.NO_TAX_SALES_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TAX " + Environment.NewLine);
            sb.Append("      ,T.INVOICE_PRICE " + Environment.NewLine);

            // 外税額＝税抜金額＋消費税額－金額(外税を含めない金額)
            sb.Append("      ,T.NO_TAX_SALES_PRICE + T.TAX - T.SALES_PRICE AS OUT_TAX " + Environment.NewLine);

            sb.Append("      ,T.INVOICE_PRINT_FLG " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_PRINT_FLG, 0) = 0 THEN '発行未' " + Environment.NewLine);
            sb.Append("            ELSE '発行済' END AS INVOICE_PRINT_FLG_NM " + Environment.NewLine);

            sb.Append("      ,T.INVOICE_KBN " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_KBN, 0) = 0 THEN '締処理' " + Environment.NewLine);
            sb.Append("            ELSE '都度請求' END AS INVOICE_KBN_NM " + Environment.NewLine);

            sb.Append("      ,RP_SUM.SUM_PRICE AS THIS_RECEIPT_PRICE " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN 0 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.INVOICE_PRICE, 0) THEN 1 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.INVOICE_PRICE, 0) THEN 2 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.INVOICE_PRICE, 0) THEN 3 " + Environment.NewLine);
            sb.Append("       END AS RECEIPT_RECEIVABLE_KBN" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN '消込未' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.INVOICE_PRICE, 0) THEN '一部消込' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.INVOICE_PRICE, 0) THEN '消込済' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.INVOICE_PRICE, 0) THEN '超過消込' " + Environment.NewLine);
            sb.Append("       END AS RECEIPT_RECEIVABLE_KBN_NM" + Environment.NewLine);

            sb.Append("      ,IFNULL(T.INVOICE_PRICE, 0) - IFNULL(RP_SUM.SUM_PRICE, 0) AS INVOICE_ZAN_PRICE " + Environment.NewLine);

            sb.Append("      ,T.MEMO " + Environment.NewLine);

            sb.Append("      ,CONCAT(lpad(CAST(SH.NO as char), " + this.idFigureSlipNo.ToString() + ", '0'),'-',lpad(CAST(OD.REC_NO as char), 2, '0')) AS NO_NM" + Environment.NewLine);

            sb.Append("      ,SH.NO AS H_NO " + Environment.NewLine);
            sb.Append("      ,replace(date_format(SH.SALES_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS SALES_YMD" + Environment.NewLine);
            sb.Append("      ,SH.SUM_ENTER_NUMBER AS SALES_SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH.SUM_CASE_NUMBER AS SALES_SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH.SUM_NUMBER AS SALES_SUM_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH.SUM_UNIT_PRICE AS SALES_SUM_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,SH.SUM_SALES_COST AS SALES_SUM_SALES_COST" + Environment.NewLine);
            sb.Append("      ,SH.SUM_TAX AS SALES_SUM_TAX" + Environment.NewLine);
            sb.Append("      ,SH.SUM_NO_TAX_PRICE AS SALES_SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,SH.SUM_PRICE AS SALES_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,SH.SUM_PROFITS AS SALES_SUM_PROFITS" + Environment.NewLine);
            //sb.Append("      ,SH.PROFITS_PERCENT AS SALES_PROFITS_PERCENT" + Environment.NewLine);

            sb.Append("      ,SH_SUM.SUM_ENTER_NUMBER AS SH_SALES_SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_CASE_NUMBER AS SH_SALES_SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_NUMBER AS SH_SALES_SUM_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_UNIT_PRICE AS SH_SALES_SUM_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_SALES_COST AS SH_SALES_SUM_SALES_COST" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_TAX AS SH_SALES_SUM_TAX" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_NO_TAX_PRICE AS SH_SALES_SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_PRICE AS SH_SALES_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_PROFITS AS SH_SALES_SUM_PROFITS" + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.SALES_COST " + Environment.NewLine);
            sb.Append("      ,OD.TAX AS D_TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS " + Environment.NewLine);
            sb.Append("      ,OD.PROFITS_PERCENT AS D_PROFITS_PERCENT" + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_SALES_H AS SH" + Environment.NewLine);
            sb.Append("    ON SH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND SH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH.INVOICE_NO = T.NO" + Environment.NewLine);
            sb.Append("   AND SH.DELETE_FLG = 0" + Environment.NewLine);

            sb.Append(" INNER JOIN T_SALES_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.SALES_ID = SH.ID" + Environment.NewLine);
            sb.Append("   AND OD.DELETE_FLG = 0" + Environment.NewLine);

            sb.Append(" INNER JOIN (SELECT SH.COMPANY_ID" + Environment.NewLine);
            sb.Append("                   ,SH.GROUP_ID" + Environment.NewLine);
            sb.Append("                   ,SH.INVOICE_NO" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_ENTER_NUMBER) AS SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_CASE_NUMBER) AS SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NUMBER) AS SUM_NUMBER" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_UNIT_PRICE) AS SUM_UNIT_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_SALES_COST) AS SUM_SALES_COST" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_PROFITS) AS SUM_PROFITS" + Environment.NewLine);
            sb.Append("               FROM T_SALES_H AS SH" + Environment.NewLine);
            sb.Append("              WHERE SH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("              GROUP BY SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,SH.INVOICE_NO " + Environment.NewLine);
            sb.Append("            ) AS SH_SUM" + Environment.NewLine);
            sb.Append("    ON SH_SUM.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND SH_SUM.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH_SUM.INVOICE_NO = T.NO" + Environment.NewLine);

            // 入金済額
            sb.Append("  LEFT JOIN (SELECT RP.INVOICE_NO" + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP " + Environment.NewLine);
            sb.Append("              WHERE RP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RP.COMPANY_ID = " + companyId + Environment.NewLine);

            sb.Append(GetGroupWhere(groupId, "RP"));

            sb.Append("              GROUP BY INVOICE_NO " + Environment.NewLine);
            sb.Append("            ) AS RP_SUM " + Environment.NewLine);
            sb.Append("    ON RP_SUM.INVOICE_NO = T.NO " + Environment.NewLine);


            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = T.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            // 回収サイクル
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = T.COLLECT_CYCLE_ID" + Environment.NewLine);

            // 預金種別
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.ACCOUNT_KBN + Environment.NewLine);
            sb.Append("   AND NM2.ID = CG.BANK_ACCOUNT_KBN" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM3.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM4.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM5.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("         ,T.NO " + Environment.NewLine);
            sb.Append("         ,SH.NO " + Environment.NewLine);
            sb.Append("         ,OD.REC_NO " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetReceiptListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm2("入金"));
            sb.Append(GetTotalKbn2());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), 10, '0') AS NO" + Environment.NewLine);
            sb.Append("      ,date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YMD + ") AS RECEIPT_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS2.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.INVOICE_NO " + Environment.NewLine);
            sb.Append("      ,IV.INVOICE_KBN " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(IV.INVOICE_KBN, 9999) = 9999 THEN '' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(IV.INVOICE_KBN, 9999) = 0 THEN '締処理' " + Environment.NewLine);
            sb.Append("            ELSE '都度請求' END AS INVOICE_KBN_NM " + Environment.NewLine);
            sb.Append("      ,date_format(IV.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS INVOICE_YYYYMMDD" + Environment.NewLine);
            sb.Append("      ,date_format(IV.COLLECT_PLAN_DAY , " + ExEscape.SQL_YMD + ") AS COLLECT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.INVOICE_ID END AS INVOICE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS INVOICE_NM " + Environment.NewLine);
            sb.Append("      ,CS.RECEIPT_DIVISION_ID AS H_RECEIPT_DIVISION_ID" + Environment.NewLine);
            sb.Append("      ,RD.DESCRIPTION AS H_RECEIPT_DIVISION_NM" + Environment.NewLine);
            sb.Append("      ,IV.SUMMING_UP_GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);

            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.RECEIPT_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,OD.DESCRIPTION " + Environment.NewLine);
            sb.Append("      ,replace(date_format(OD.BILL_DUE_DATE , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS BILL_DUE_DATE" + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_RECEIPT_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.RECEIPT_ID = T.ID" + Environment.NewLine);

            // 入力担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS2" + Environment.NewLine);
            sb.Append("    ON PS2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS2.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS2.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS2.ID = T.PERSON_ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.UPDATE_PERSON_ID" + Environment.NewLine);

            //請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            //請求
            sb.Append("  LEFT JOIN T_INVOICE AS IV" + Environment.NewLine);
            sb.Append("    ON IV.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND IV.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND IV.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND IV.NO = T.INVOICE_NO" + Environment.NewLine);

            // 回収(入金)区分
            sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
            sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND RD.COMPANY_ID = CS.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND RD.ID = CS.RECEIPT_DIVISION_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = IV.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetReceiptDDMMTotalReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _group_kbn = "";
            int group_kbn = strWhereSql.IndexOf("<group kbn>");
            if (group_kbn != -1)
            {
                _group_kbn = strWhereSql.Substring(group_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, group_kbn);
            }

            sb.Append("SELECT T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (_group_kbn == "1")
            {
                sb.Append(GetTitleNm2("入金", "日報"));
            }
            else
            {
                sb.Append(GetTitleNm2("入金", "月報"));
            }
            sb.Append(GetTotalKbnDDMM2(ExCast.zCInt(_group_kbn)));

            sb.Append("      ,IFNULL(SUM(OD_CASH.PRICE), 0) AS CASH_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_BANK_ACCOUNT.PRICE), 0) AS BANK_ACCOUNT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_COMMISSION.PRICE), 0) AS COMMISSION_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_BILL.PRICE), 0) AS BILL_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_ANOTHER.PRICE), 0) AS ANOTHER_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_CASH.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_BANK_ACCOUNT.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_COMMISSION.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_BILL.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_ANOTHER.PRICE), 0) AS SUM_PRICE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_RECEIPT_H AS T" + Environment.NewLine);

            #region Join

            // 現金
            sb.Append("  LEFT JOIN T_RECEIPT_D AS OD_CASH" + Environment.NewLine);
            sb.Append("    ON OD_CASH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_CASH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_CASH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_CASH.RECEIPT_DIVISION_ID = '101'" + Environment.NewLine);
            sb.Append("   AND OD_CASH.RECEIPT_ID = T.ID" + Environment.NewLine);

            // 振込
            sb.Append("  LEFT JOIN T_RECEIPT_D AS OD_BANK_ACCOUNT" + Environment.NewLine);
            sb.Append("    ON OD_BANK_ACCOUNT.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.RECEIPT_DIVISION_ID = '201'" + Environment.NewLine);      
            sb.Append("   AND OD_BANK_ACCOUNT.RECEIPT_ID = T.ID" + Environment.NewLine);

            // 手数料
            sb.Append("  LEFT JOIN T_RECEIPT_D AS OD_COMMISSION" + Environment.NewLine);
            sb.Append("    ON OD_COMMISSION.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.RECEIPT_DIVISION_ID = '301'" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.RECEIPT_ID = T.ID" + Environment.NewLine);

            // 手形
            sb.Append("  LEFT JOIN T_RECEIPT_D AS OD_BILL" + Environment.NewLine);
            sb.Append("    ON OD_BILL.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_BILL.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_BILL.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_BILL.RECEIPT_DIVISION_ID IN ('401','402')" + Environment.NewLine);
            sb.Append("   AND OD_BILL.RECEIPT_ID = T.ID" + Environment.NewLine);

            // その他
            sb.Append("  LEFT JOIN T_RECEIPT_D AS OD_ANOTHER" + Environment.NewLine);
            sb.Append("    ON OD_ANOTHER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.RECEIPT_DIVISION_ID NOT IN ('101','201','301','401','402')" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.RECEIPT_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            //sb.Append(" LIMIT 0, 1000");

            #region Group by


            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 1:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.INVOICE_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE T.INVOICE_ID END" + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PERSON_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY T.PERSON_ID " + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        if (_group_kbn == "1")
                        {
                            // 日報時
                            sb.Append("         ,T.RECEIPT_YMD " + Environment.NewLine);
                            sb.Append(" ORDER BY T.RECEIPT_YMD " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("         ,date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                            sb.Append(" ORDER BY date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,CG.NAME " + Environment.NewLine);
                if (_group_kbn == "1")
                {
                    // 日報時
                    sb.Append("         ,T.RECEIPT_YMD " + Environment.NewLine);
                    sb.Append(" ORDER BY T.RECEIPT_YMD " + Environment.NewLine);
                }
                else
                {
                    // 月報時
                    sb.Append("         ,date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                    sb.Append(" ORDER BY date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                }
            }

            #endregion

            #endregion

            return sb.ToString();

        }

        public string GetCollectPlanReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);

            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.INVOICE_ID END AS INVOICE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS INVOICE_NM " + Environment.NewLine);
            sb.Append("      ,CS.RECEIPT_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,RD.DESCRIPTION AS RECEIPT_DIVISION_NM" + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS INVOICE_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.COLLECT_PLAN_DAY , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS COLLECT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,T.INVOICE_PRICE " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);

            #region Join

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 入金区分
            sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
            sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND RD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND RD.ID = CS.RECEIPT_DIVISION_ID " + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("         ,CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("               ELSE T.INVOICE_ID END" + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetInvoiceBalanceListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.INVOICE_ID END AS INVOICE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS INVOICE_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS INVOICE_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS CUSTOMER_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS CUSTOMER_PERSON_NM_TITLE_NAME " + Environment.NewLine);

            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);
            sb.Append("      ,CG.BANK_NAME AS COMPANY_BANK_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_BRANCH_NAME AS COMPANY_BANK_BRANCH_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NO AS COMPANY_BANK_ACCOUNT_NO " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NAME AS COMPANY_BANK_ACCOUNT_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KANA AS COMPANY_BANK_ACCOUNT_KANA " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KBN AS BANK_ACCOUNT_KBN " + Environment.NewLine);
            sb.Append("      ,IFNULL(NM2.DESCRIPTION, '普通') AS BANK_ACCOUNT_KBN_NM " + Environment.NewLine);

            sb.Append("      ,CONCAT('口座名義：', CG.BANK_ACCOUNT_NAME, ' カナ表記：', CG.BANK_ACCOUNT_KANA) AS ACCOUNT_INF1 " + Environment.NewLine);
            sb.Append("      ,CONCAT(CG.BANK_NAME, ' ', CG.BANK_BRANCH_NAME, ' ', IFNULL(NM2.DESCRIPTION, '普通'), ' ', CG.BANK_ACCOUNT_NO) AS ACCOUNT_INF2 " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS INVOICE_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.SUMMING_UP_GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.BEFORE_INVOICE_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS BEFORE_INVOICE_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.COLLECT_CYCLE_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS COLLECT_CYCLE_NM " + Environment.NewLine);

            sb.Append("      ,T.COLLECT_DAY " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.COLLECT_PLAN_DAY , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS COLLECT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,T.BEFORE_INVOICE_PRICE " + Environment.NewLine);
            sb.Append("      ,T.RECEIPT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TRANSFER_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SALES_PRICE " + Environment.NewLine);
            sb.Append("      ,T.NO_TAX_SALES_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TAX " + Environment.NewLine);
            sb.Append("      ,T.INVOICE_PRICE " + Environment.NewLine);

            sb.Append("      ,T.INVOICE_PRINT_FLG " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_PRINT_FLG, 0) = 0 THEN '発行未' " + Environment.NewLine);
            sb.Append("            ELSE '発行済' END AS INVOICE_PRINT_FLG_NM " + Environment.NewLine);

            sb.Append("      ,T.INVOICE_KBN " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_KBN, 0) = 0 THEN '締処理' " + Environment.NewLine);
            sb.Append("            ELSE '都度請求' END AS INVOICE_KBN_NM " + Environment.NewLine);

            sb.Append("      ,RP_SUM.SUM_PRICE AS THIS_RECEIPT_PRICE " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN 0 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.INVOICE_PRICE, 0) THEN 1 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.INVOICE_PRICE, 0) THEN 2 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.INVOICE_PRICE, 0) THEN 3 " + Environment.NewLine);
            sb.Append("       END AS RECEIPT_RECEIVABLE_KBN" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN '消込未' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.INVOICE_PRICE, 0) THEN '一部消込' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.INVOICE_PRICE, 0) THEN '消込済' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.INVOICE_PRICE, 0) THEN '超過消込' " + Environment.NewLine);
            sb.Append("       END AS RECEIPT_RECEIVABLE_KBN_NM" + Environment.NewLine);

            sb.Append("      ,IFNULL(T.INVOICE_PRICE, 0) - IFNULL(RP_SUM.SUM_PRICE, 0) AS INVOICE_ZAN_PRICE " + Environment.NewLine);

            sb.Append("      ,T.MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_INVOICE AS T" + Environment.NewLine);

            #region Join

            // 入金済額
            sb.Append("  LEFT JOIN (SELECT RP.INVOICE_NO" + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP " + Environment.NewLine);
            sb.Append("              WHERE RP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RP.COMPANY_ID = " + companyId + Environment.NewLine);

            sb.Append(GetGroupWhere(groupId, "RP"));

            sb.Append("              GROUP BY INVOICE_NO " + Environment.NewLine);
            sb.Append("            ) AS RP_SUM " + Environment.NewLine);
            sb.Append("    ON RP_SUM.INVOICE_NO = T.NO " + Environment.NewLine);


            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 請求先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.INVOICE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = T.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            // 回収サイクル
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = T.COLLECT_CYCLE_ID" + Environment.NewLine);

            // 預金種別
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.ACCOUNT_KBN + Environment.NewLine);
            sb.Append("   AND NM2.ID = CG.BANK_ACCOUNT_KBN" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("         ,CS.ID2 " + Environment.NewLine);
            sb.Append("         ,CS.ID " + Environment.NewLine);
            sb.Append("         ,T.NO " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }
        
        public string GetSalesCreditBalanaceListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string proc_ym = "";
            int proc_ym_index = strWhereSql.IndexOf("<proc ym>");

            if (proc_ym_index != -1)
            {
                proc_ym = strWhereSql.Substring(proc_ym_index + 9, 7);
                strWhereSql = strWhereSql.Substring(0, proc_ym_index);
            }

            DateTime before_ym_ = ExCast.zConvertToDate(proc_ym + "/01");
            DateTime before_ym = before_ym_.AddDays(-1);
            DateTime this_ym_from = ExCast.zConvertToDate(proc_ym + "/01");
            DateTime this_ym_to = this_ym_from.AddMonths(1).AddDays(-1);

            sb.Append("SELECT CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,CG.ID AS GROUP_ID " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);
            sb.Append("      ," + ExEscape.zRepStr(proc_ym) + " AS YM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CM.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CM.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE CM.INVOICE_ID END AS INVOICE_ID " + Environment.NewLine);
            sb.Append("      ,CM.INVOICE_NAME " + Environment.NewLine);

            sb.Append("      ,IFNULL(SB.SALES_CREDIT_INIT_PRICE, 0) AS SALES_CREDIT_INIT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_BEFORE.SUM_TAX, 0) AS SUM_TAX" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_BEFORE.SUM_PRICE, 0) AS SUM_PRICE " + Environment.NewLine);

            sb.Append("      ,IFNULL(SB.SALES_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0)) AS BEFORE_SALES_CREDIT_BALANCE " + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS.SUM_PRICE, 0) AS THIS_RECEIPT_PRICE " + Environment.NewLine);
            sb.Append("      ,IFNULL(((IFNULL(RP_THIS.SUM_PRICE, 0)) / (IFNULL(SB.SALES_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0)))) * 100, 0) AS THIS_RECEIPT_PERCENT " + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_THIS.SUM_NO_TAX_PRICE, 0) AS THIS_SALES_PRICE " + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_THIS.SUM_TAX, 0) AS THIS_SALES_TAX " + Environment.NewLine);
            sb.Append("      ,(IFNULL(SB.SALES_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0))) - (IFNULL(RP_THIS.SUM_PRICE, 0)) + IFNULL(SH_THIS.SUM_TAX, 0) + IFNULL(SH_THIS.SUM_NO_TAX_PRICE, 0) AS THIS_SALES_CREDIT_BALANCE " + Environment.NewLine);

            sb.Append("      ,IFNULL(RP_THIS_CASH.SUM_PRICE, 0) AS THIS_CASH_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_ACCOUNT.SUM_PRICE, 0) AS THIS_ACCOUNT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_COMMISSION.SUM_PRICE, 0) AS THIS_COMMISSION_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_BILL.SUM_PRICE, 0) AS THIS_BILL_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_ANOTHER.SUM_PRICE, 0) AS THIS_ANOTHER_PRICE" + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM (SELECT DISTINCT " + Environment.NewLine);
            sb.Append("               CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("              ,CM.DELETE_FLG " + Environment.NewLine);
            sb.Append("              ,CM.INVOICE_ID " + Environment.NewLine);
            sb.Append("              ,CM2.ID2" + Environment.NewLine);
            sb.Append("              ,CM2.NAME AS INVOICE_NAME" + Environment.NewLine);
            sb.Append("          FROM M_CUSTOMER AS CM" + Environment.NewLine);
            sb.Append("         INNER JOIN M_CUSTOMER AS CM2" + Environment.NewLine);
            sb.Append("            ON CM2.COMPANY_ID = CM.COMPANY_ID" + Environment.NewLine);
            sb.Append("           AND CM2.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("           AND CM2.ID = CM.INVOICE_ID" + Environment.NewLine);
            sb.Append("         WHERE CM.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("           AND CM.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("           AND CM.DISPLAY_FLG = 1" + Environment.NewLine);
            sb.Append("        ) AS CM" + Environment.NewLine);

            #region Join

            // 初期売掛残高
            sb.Append("  LEFT JOIN M_SALES_CREDIT_BALANCE AS SB" + Environment.NewLine);
            sb.Append("    ON SB.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SB.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "SB"));    // グループ集計条件
            sb.Append("   AND SB.ID = CM.INVOICE_ID" + Environment.NewLine);


            #region 対象年月以前の掛売上

            sb.Append("  LEFT JOIN (SELECT SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,SH.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("               FROM T_SALES_H AS SH" + Environment.NewLine);
            sb.Append("              WHERE SH.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "SH", "             "));    // グループ集計条件
            sb.Append("                AND SH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND SH.BUSINESS_DIVISION_ID IN (1, 3)" + Environment.NewLine);  // 掛売上,都度請求
            sb.Append("                AND SH.SALES_YMD <= " + ExEscape.zRepStr(before_ym.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,SH.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS SH_BEFORE" + Environment.NewLine);
            sb.Append("    ON SH_BEFORE.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND SH_BEFORE.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH_BEFORE.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月以前の入金

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(before_ym.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_BEFORE" + Environment.NewLine);
            sb.Append("    ON RP_BEFORE.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_BEFORE.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_BEFORE.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の掛売上

            sb.Append("  LEFT JOIN (SELECT SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,SH.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("               FROM T_SALES_H AS SH" + Environment.NewLine);
            sb.Append("              WHERE SH.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "SH", "             "));    // グループ集計条件
            sb.Append("                AND SH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND SH.BUSINESS_DIVISION_ID IN (1, 3)" + Environment.NewLine);  // 掛売上,都度請求
            sb.Append("                AND SH.SALES_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND SH.SALES_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,SH.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS SH_THIS" + Environment.NewLine);
            sb.Append("    ON SH_THIS.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND SH_THIS.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH_THIS.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の入金

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS" + Environment.NewLine);
            sb.Append("    ON RP_THIS.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の入金(現金)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_RECEIPT_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_DIVISION_ID = '101' " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_CASH" + Environment.NewLine);
            sb.Append("    ON RP_THIS_CASH.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_CASH.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_CASH.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の入金(振込)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_RECEIPT_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_DIVISION_ID = '201' " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_ACCOUNT" + Environment.NewLine);
            sb.Append("    ON RP_THIS_ACCOUNT.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_ACCOUNT.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_ACCOUNT.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の入金(手数料)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_RECEIPT_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_DIVISION_ID = '301' " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_COMMISSION" + Environment.NewLine);
            sb.Append("    ON RP_THIS_COMMISSION.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_COMMISSION.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_COMMISSION.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の入金(手形)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_RECEIPT_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_DIVISION_ID IN ('401','402') " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_BILL" + Environment.NewLine);
            sb.Append("    ON RP_THIS_BILL.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_BILL.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_BILL.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の入金(その他)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_RECEIPT_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_RECEIPT_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.RECEIPT_DIVISION_ID NOT IN ('101','201','301','401','402') " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.RECEIPT_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.INVOICE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_ANOTHER" + Environment.NewLine);
            sb.Append("    ON RP_THIS_ANOTHER.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_ANOTHER.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_ANOTHER.INVOICE_ID = CM.INVOICE_ID" + Environment.NewLine);

            #endregion

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = CM.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = CM.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = SB.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE CM.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CM.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            sb.Append(" ORDER BY CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,CG.ID " + Environment.NewLine);
            sb.Append("         ,CM.ID2 " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        #endregion

        #region Purchase

        public string GetPurchaseOrderListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _issue_ymd = "";
            int issue_index = strWhereSql.IndexOf("<issue ymd>");
            if (issue_index != -1)
            {
                _issue_ymd = strWhereSql.Substring(issue_index + 11, 10);
                strWhereSql = strWhereSql.Substring(0, issue_index);
            }

            sb.Append("SELECT lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,CONCAT('合計金額 ','\',' '" + ", FORMAT(T.SUM_NO_TAX_PRICE + T.SUM_TAX, '#,##0'), '－') AS SUM_PRICE_TITLE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,'" + _issue_ymd + "' AS ISSUE_YMD " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm3("発注"));
            sb.Append(GetTotalKbn3());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,replace(date_format(T.PURCHASE_ORDER_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PURCHASE_ORDER_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,PU.NAME AS PURCHASE_NM " + Environment.NewLine);
            sb.Append("      ,PU.PERSON_NAME AS PURCHASE_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,PU.TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(PU.PERSON_NAME, '') = '' THEN CONCAT(PU.NAME, ' ', PU.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE PU.NAME END AS PURCHASE_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(PU.PERSON_NAME, '') = '' THEN PU.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(PU.PERSON_NAME, ' ', PU.TITLE_NAME) END AS PURCHASE_PERSON_NM_TITLE_NAME " + Environment.NewLine);

            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NM " + Environment.NewLine);
            sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS BISNESS_DIVISON_NM " + Environment.NewLine);

            sb.Append("      ,T.SEND_ID " + Environment.NewLine);
            sb.Append("      ,NM8.DESCRIPTION AS SEND_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.CUSTOMER_ID END AS CUSTOMER_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.SUPPLIER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.SUPPLIER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.SUPPLIER_ID END AS SUPPLIER_ID " + Environment.NewLine);
            sb.Append("      ,SU.NAME AS SUPPLIER_NM " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS SUPPLY_YMD" + Environment.NewLine);

            sb.Append("      ,CASE WHEN replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') = '' THEN '' " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), ' まで') END AS SUPPLY_YMD_NM " + Environment.NewLine);

            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);

            sb.Append("      ,PU.PAYMENT_CREDIT_PRICE " + Environment.NewLine);
            sb.Append("      ,PU.UNIT_KIND_ID " + Environment.NewLine);
            sb.Append("      ,NM7.DESCRIPTION AS UNIT_KIND_NM " + Environment.NewLine);
            sb.Append("      ,PU.CREDIT_RATE " + Environment.NewLine);

            sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);

            sb.Append("      ,T.PURCHASE_PRINT_FLG " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_PRINT_FLG, 0) = 0 THEN '発行未' " + Environment.NewLine);
            sb.Append("            ELSE '発行済' END AS PURCHASE_PRINT_FLG_NM " + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PURCHASE_ORDER_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_PURCHASE_ORDER_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.PURCHASE_ORDER_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
            sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PU.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 税転換
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_PU_ID + Environment.NewLine);
            sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_PU_ID + Environment.NewLine);
            sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

            // 発送区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM8" + Environment.NewLine);
            sb.Append("    ON NM8.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM8.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM8.DIVISION_ID = " + (int)CommonUtl.geNameKbn.SEND_KBN + Environment.NewLine);
            sb.Append("   AND T.SEND_ID = NM8.ID" + Environment.NewLine);

            // 納入先
            sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
            sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
            sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM2.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            // 単価種類
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM7" + Environment.NewLine);
            sb.Append("    ON NM7.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM7.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM7.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_PRICE_DIVISION_PU_ID + Environment.NewLine);
            sb.Append("   AND NM7.ID = PU.UNIT_KIND_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetPurchaseListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,'' AS SUM_PRICE_TITLE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.SUM_NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_TAX " + Environment.NewLine);
            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm3("仕入"));
            sb.Append(GetTotalKbn3());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.PURCHASE_ORDER_NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS PURCHASE_ORDER_NO" + Environment.NewLine);
            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PURCHASE_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,PU.NAME AS PURCHASE_NM " + Environment.NewLine);
            sb.Append("      ,PU.PERSON_NAME AS PURCHASE_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,PU.TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(PU.PERSON_NAME, '') = '' THEN CONCAT(PU.NAME, ' ', PU.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE PU.NAME END AS PURCHASE_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(PU.PERSON_NAME, '') = '' THEN PU.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(PU.PERSON_NAME, ' ', PU.TITLE_NAME) END AS PURCHASE_PERSON_NM_TITLE_NAME " + Environment.NewLine);

            sb.Append("      ,T.TAX_CHANGE_ID " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS TAX_CHANGE_NM " + Environment.NewLine);
            sb.Append("      ,T.BUSINESS_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS BISNESS_DIVISON_NM " + Environment.NewLine);

            sb.Append("      ,T.SEND_ID " + Environment.NewLine);
            sb.Append("      ,NM8.DESCRIPTION AS SEND_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.CUSTOMER_ID END AS CUSTOMER_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);

            sb.Append("      ,T.SUPPLIER_ID " + Environment.NewLine);
            sb.Append("      ,SU.NAME AS SUPPLIER_NM " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS SUPPLY_YMD" + Environment.NewLine);
            sb.Append("      ,CASE WHEN replace(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') = '' THEN '' " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(date_format(T.SUPPLY_YMD , " + ExEscape.SQL_YMD + "), ' まで') END AS SUPPLY_YMD_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);

            sb.Append("      ,IFNULL(IV.NO, '未') AS PAYMENT_NO " + Environment.NewLine);
            sb.Append("      ,PU.PAYMENT_CREDIT_PRICE " + Environment.NewLine);
            sb.Append("      ,PU.UNIT_KIND_ID " + Environment.NewLine);
            sb.Append("      ,NM7.DESCRIPTION AS UNIT_KIND_NM " + Environment.NewLine);
            sb.Append("      ,CS.CREDIT_RATE " + Environment.NewLine);

            sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);

            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PURCHASE_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_PURCHASE_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.PURCHASE_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
            sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PU.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 納入先
            sb.Append("  LEFT JOIN M_SUPPLIER AS SU" + Environment.NewLine);
            sb.Append("    ON SU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND SU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = SU.CUSTOMER_ID" + Environment.NewLine);
            sb.Append("   AND T.SUPPLIER_ID = SU.ID" + Environment.NewLine);

            // 支払データ
            sb.Append("  LEFT JOIN T_PAYMENT AS IV" + Environment.NewLine);
            sb.Append("    ON IV.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND IV.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND IV.NO = T.PAYMENT_NO" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 税転換
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_CHANGE_PU_ID + Environment.NewLine);
            sb.Append("   AND T.TAX_CHANGE_ID = NM4.ID" + Environment.NewLine);

            // 取引区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BUSINESS_DIVISION_PU_ID + Environment.NewLine);
            sb.Append("   AND T.BUSINESS_DIVISION_ID = NM5.ID" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM1.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM2.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM3.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            // 単価種類
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM7" + Environment.NewLine);
            sb.Append("    ON NM7.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM7.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM7.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_PRICE_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM7.ID = PU.UNIT_KIND_ID" + Environment.NewLine);

            // 発送区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM8" + Environment.NewLine);
            sb.Append("    ON NM8.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM8.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM8.DIVISION_ID = " + (int)CommonUtl.geNameKbn.SEND_KBN + Environment.NewLine);
            sb.Append("   AND T.SEND_ID = NM8.ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetPurchaseDDMMTotalReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _group_kbn = "";
            int group_kbn = strWhereSql.IndexOf("<group kbn>");
            if (group_kbn != -1)
            {
                _group_kbn = strWhereSql.Substring(group_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, group_kbn);
            }

            sb.Append("SELECT T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (_group_kbn == "1")
            {
                sb.Append(GetTitleNm3("仕入", "日報"));
            }
            else
            {
                sb.Append(GetTitleNm3("仕入", "月報"));
            }
            sb.Append(GetTotalKbnDDMM3(ExCast.zCInt(_group_kbn)));

            sb.Append("      ,SUM(T.SUM_ENTER_NUMBER) AS SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_CASE_NUMBER) AS SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_NUMBER) AS SUM_NUMBER" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_UNIT_PRICE) AS SUM_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,SUM(T.SUM_TAX) AS SUM_TAX" + Environment.NewLine);

            sb.Append("      ,IFNULL(SUM(OD_RETURN.UNIT_PRICE), 0) * -1 AS RETURN_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.TAX), 0) * -1 AS RETURN_TAX" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.NO_TAX_PRICE), 0) * -1 AS RETURN_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_RETURN.PRICE), 0) * -1 AS RETURN_PRICE" + Environment.NewLine);

            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.UNIT_PRICE), 0) * -1 AS NEBIKI_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.TAX), 0) * -1 AS NEBIKI_TAX" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.NO_TAX_PRICE), 0) * -1 AS NEBIKI_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NEBIKI.PRICE), 0) * -1 AS NEBIKI_PRICE" + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PURCHASE_H AS T" + Environment.NewLine);

            #region Join

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_RETURN" + Environment.NewLine);
            sb.Append("    ON OD_RETURN.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.BREAKDOWN_ID = 6" + Environment.NewLine);
            sb.Append("   AND OD_RETURN.PURCHASE_ID = T.ID" + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_NEBIKI" + Environment.NewLine);
            sb.Append("    ON OD_NEBIKI.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.BREAKDOWN_ID = 2" + Environment.NewLine);
            sb.Append("   AND OD_NEBIKI.PURCHASE_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.PURCHASE_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            //sb.Append(" LIMIT 0, 1000");

            #region Group by


            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 1:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PURCHASE_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE T.PURCHASE_ID END" + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PERSON_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY T.PERSON_ID " + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        if (_group_kbn == "1")
                        {
                            // 日報時
                            sb.Append("         ,T.PURCHASE_YMD " + Environment.NewLine);
                            sb.Append(" ORDER BY T.PURCHASE_YMD " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("         ,date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                            sb.Append(" ORDER BY date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,CG.NAME " + Environment.NewLine);
                if (_group_kbn == "1")
                {
                    // 日報時
                    sb.Append("         ,T.PURCHASE_YMD " + Environment.NewLine);
                    sb.Append(" ORDER BY T.PURCHASE_YMD " + Environment.NewLine);
                }
                else
                {
                    // 月報時
                    sb.Append("         ,date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                    sb.Append(" ORDER BY date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                }
            }

            #endregion

            #endregion

            return sb.ToString();

        }

        public string GetPurchaseChangeReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _group_kbn = "";
            int group_kbn = strWhereSql.IndexOf("<group kbn>");
            if (group_kbn != -1)
            {
                _group_kbn = strWhereSql.Substring(group_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, group_kbn);
            }
            int total_kbn = 1;

            sb.Append("SELECT T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            sb.Append(GetTitleNm3("仕入", "推移表"));
            sb.Append(GetTotalKbnDDMM3(3));

            sb.Append("      ,IFNULL(SUM(OD_JANUARY.NO_TAX_PRICE), 0) AS JANUARY_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_FEBRUARY.NO_TAX_PRICE), 0) AS FEBRUARY_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_MARCH.NO_TAX_PRICE), 0) AS MARCH_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_APRIL.NO_TAX_PRICE), 0) AS APRIL_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_MAY.NO_TAX_PRICE), 0) AS MAY_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_JUNE.NO_TAX_PRICE), 0) AS JUNE_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_JULY.NO_TAX_PRICE), 0) AS JULY_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_AUGUST.NO_TAX_PRICE), 0) AS AUGUST_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_SEPTEMBER.NO_TAX_PRICE), 0) AS SEPTEMBER_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_OCTOBER.NO_TAX_PRICE), 0) AS OCTOBER_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_NOVEMBER.NO_TAX_PRICE), 0) AS NOVEMBER_SUM_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_DECEMBER.NO_TAX_PRICE), 0) AS DECEMBER_SUM_PRICE" + Environment.NewLine);

            sb.Append("      ,IFNULL(SUM(OD_JANUARY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_FEBRUARY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_MARCH.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_APRIL.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_MAY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_JUNE.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_JULY.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_AUGUST.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_SEPTEMBER.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_OCTOBER.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_NOVEMBER.NO_TAX_PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_DECEMBER.NO_TAX_PRICE), 0) AS TOTAL_PRICE" + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PURCHASE_H AS T" + Environment.NewLine);

            #region Join

            #region 月別集計

            if (this.entitySetting.total_kbn == 3)
            {
                // 商品用
                sb.Append("  CROSS JOIN (SELECT DISTINCT COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,COMMODITY_NAME" + Environment.NewLine);
                sb.Append("               FROM T_PURCHASE_D AS SD" + Environment.NewLine);
                sb.Append("              WHERE SD.DELETE_FLG = 0) AS CD" + Environment.NewLine);

                //sb.Append("  LEFT JOIN T_PURCHASE_D AS CD" + Environment.NewLine);
                //sb.Append("    ON CD.DELETE_FLG = 0" + Environment.NewLine);
                //sb.Append("   AND CD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
                //sb.Append("   AND CD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
                //sb.Append("   AND CD.PURCHASE_ID = T.ID" + Environment.NewLine);

                //sb.Append("  LEFT JOIN M_COMMODITY AS CD" + Environment.NewLine);
                //sb.Append("    ON CD.DELETE_FLG = 0 " + Environment.NewLine);
                //sb.Append("   AND CD.DISPLAY_FLG = 1 " + Environment.NewLine);
                //sb.Append("   AND CD.COMPANY_ID = " + companyId + Environment.NewLine);
            }

            #region January

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_JANUARY" + Environment.NewLine);
            sb.Append("    ON HD_JANUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_JANUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_JANUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_JANUARY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_JANUARY.PURCHASE_YMD) = 1 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_JANUARY" + Environment.NewLine);
            sb.Append("    ON OD_JANUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_JANUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_JANUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_JANUARY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_JANUARY.PURCHASE_ID = HD_JANUARY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_JANUARY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region February

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_FEBRUARY" + Environment.NewLine);
            sb.Append("    ON HD_FEBRUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_FEBRUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_FEBRUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_FEBRUARY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_FEBRUARY.PURCHASE_YMD) = 2 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_FEBRUARY" + Environment.NewLine);
            sb.Append("    ON OD_FEBRUARY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_FEBRUARY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_FEBRUARY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_FEBRUARY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_FEBRUARY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_FEBRUARY.PURCHASE_ID = HD_FEBRUARY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_FEBRUARY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region March

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_MARCH" + Environment.NewLine);
            sb.Append("    ON HD_MARCH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_MARCH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_MARCH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_MARCH.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_MARCH.PURCHASE_YMD) = 3 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_MARCH" + Environment.NewLine);
            sb.Append("    ON OD_MARCH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_MARCH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_MARCH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_MARCH.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_MARCH.PURCHASE_ID = HD_MARCH.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_MARCH.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region April

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_APRIL" + Environment.NewLine);
            sb.Append("    ON HD_APRIL.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_APRIL.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_APRIL.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_APRIL.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_APRIL.PURCHASE_YMD) = 4 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_APRIL" + Environment.NewLine);
            sb.Append("    ON OD_APRIL.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_APRIL.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_APRIL.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_APRIL.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_APRIL.PURCHASE_ID = HD_APRIL.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_APRIL.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region May

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_MAY" + Environment.NewLine);
            sb.Append("    ON HD_MAY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_MAY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_MAY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_MAY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_MAY.PURCHASE_YMD) = 5 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_MAY" + Environment.NewLine);
            sb.Append("    ON OD_MAY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_MAY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_MAY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_MAY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_MAY.PURCHASE_ID = HD_MAY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_MAY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region June

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_JUNE" + Environment.NewLine);
            sb.Append("    ON HD_JUNE.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_JUNE.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_JUNE.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_JUNE.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_JUNE.PURCHASE_YMD) = 6 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_JUNE" + Environment.NewLine);
            sb.Append("    ON OD_JUNE.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_JUNE.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_JUNE.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_JUNE.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_JUNE.PURCHASE_ID = HD_JUNE.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_JUNE.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region July

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_JULY" + Environment.NewLine);
            sb.Append("    ON HD_JULY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_JULY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_JULY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_JULY.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_JULY.PURCHASE_YMD) = 7 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_JULY" + Environment.NewLine);
            sb.Append("    ON OD_JULY.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_JULY.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_JULY.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_JULY.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_JULY.PURCHASE_ID = HD_JULY.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_JULY.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region August

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_AUGUST" + Environment.NewLine);
            sb.Append("    ON HD_AUGUST.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_AUGUST.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_AUGUST.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_AUGUST.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_AUGUST.PURCHASE_YMD) = 8 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_AUGUST" + Environment.NewLine);
            sb.Append("    ON OD_AUGUST.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_AUGUST.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_AUGUST.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_AUGUST.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_AUGUST.PURCHASE_ID = HD_AUGUST.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_AUGUST.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region September

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_SEPTEMBER" + Environment.NewLine);
            sb.Append("    ON HD_SEPTEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_SEPTEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_SEPTEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_SEPTEMBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_SEPTEMBER.PURCHASE_YMD) = 9 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_SEPTEMBER" + Environment.NewLine);
            sb.Append("    ON OD_SEPTEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_SEPTEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_SEPTEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_SEPTEMBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_SEPTEMBER.PURCHASE_ID = HD_SEPTEMBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_SEPTEMBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region October

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_OCTOBER" + Environment.NewLine);
            sb.Append("    ON HD_OCTOBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_OCTOBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_OCTOBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_OCTOBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_OCTOBER.PURCHASE_YMD) = 10 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_OCTOBER" + Environment.NewLine);
            sb.Append("    ON OD_OCTOBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_OCTOBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_OCTOBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_OCTOBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_OCTOBER.PURCHASE_ID = HD_OCTOBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_OCTOBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region November

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_NOVEMBER" + Environment.NewLine);
            sb.Append("    ON HD_NOVEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_NOVEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_NOVEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_NOVEMBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_NOVEMBER.PURCHASE_YMD) = 11 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_NOVEMBER" + Environment.NewLine);
            sb.Append("    ON OD_NOVEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_NOVEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_NOVEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_NOVEMBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_NOVEMBER.PURCHASE_ID = HD_NOVEMBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_NOVEMBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #region December

            sb.Append("  LEFT JOIN T_PURCHASE_H AS HD_DECEMBER" + Environment.NewLine);
            sb.Append("    ON HD_DECEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND HD_DECEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND HD_DECEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND HD_DECEMBER.ID = T.ID" + Environment.NewLine);
            sb.Append("   AND MONTH(HD_DECEMBER.PURCHASE_YMD) = 10 " + Environment.NewLine);

            sb.Append("  LEFT JOIN T_PURCHASE_D AS OD_DECEMBER" + Environment.NewLine);
            sb.Append("    ON OD_DECEMBER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_DECEMBER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_DECEMBER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            if (total_kbn == 2)
            {
                // 純仕入用
                sb.Append("   AND OD_DECEMBER.BREAKDOWN_ID NOT IN (2, 6)" + Environment.NewLine);     // 値引・返品を除く
            }
            sb.Append("   AND OD_DECEMBER.PURCHASE_ID = HD_DECEMBER.ID" + Environment.NewLine);
            if (this.entitySetting.total_kbn == 3)
            {
                sb.Append("   AND OD_DECEMBER.COMMODITY_ID = CD.COMMODITY_ID" + Environment.NewLine);
            }

            #endregion

            #endregion

            // 担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.PURCHASE_ID = CS.ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            //sb.Append(" LIMIT 0, 1000");

            #region Group by

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 1:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PURCHASE_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE T.PURCHASE_ID END" + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PERSON_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY T.PERSON_ID " + Environment.NewLine);
                        break;
                    case 3:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,CD.COMMODITY_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(CD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE CD.COMMODITY_ID END" + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
                        sb.Append(" ORDER BY date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
                        break;
                }
            }
            else
            {
                sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,CG.NAME " + Environment.NewLine);
                sb.Append("         ,date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
                sb.Append(" ORDER BY date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + ") " + Environment.NewLine);
            }

            #endregion

            #endregion

            return sb.ToString();

        }

        public string GetPaymentListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _issue_ymd = "";
            int issue_index = strWhereSql.IndexOf("<issue ymd>");
            if (issue_index != -1)
            {
                _issue_ymd = strWhereSql.Substring(issue_index + 11, 10);
                strWhereSql = strWhereSql.Substring(0, issue_index);
            }

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,'" + _issue_ymd + "' AS ISSUE_YMD " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS PURCHASE_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS PURCHASE_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS PURCHASE_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS PURCHASE_PERSON_NM_TITLE_NAME " + Environment.NewLine);

            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);
            sb.Append("      ,CG.BANK_NAME AS COMPANY_BANK_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_BRANCH_NAME AS COMPANY_BANK_BRANCH_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NO AS COMPANY_BANK_ACCOUNT_NO " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NAME AS COMPANY_BANK_ACCOUNT_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KANA AS COMPANY_BANK_ACCOUNT_KANA " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KBN AS BANK_ACCOUNT_KBN " + Environment.NewLine);
            sb.Append("      ,IFNULL(NM2.DESCRIPTION, '普通') AS BANK_ACCOUNT_KBN_NM " + Environment.NewLine);

            sb.Append("      ,CONCAT('口座名義：', CG.BANK_ACCOUNT_NAME, ' カナ表記：', CG.BANK_ACCOUNT_KANA) AS ACCOUNT_INF1 " + Environment.NewLine);
            sb.Append("      ,CONCAT(CG.BANK_NAME, ' ', CG.BANK_BRANCH_NAME, ' ', IFNULL(NM2.DESCRIPTION, '普通'), ' ', CG.BANK_ACCOUNT_NO) AS ACCOUNT_INF2 " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.SUMMING_UP_GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.BEFORE_PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS BEFORE_PAYMENT_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_CYCLE_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS PAYMENT_CYCLE_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_DAY " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.PAYMENT_PLAN_DAY , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,T.BEFORE_PAYMENT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.PAYMENT_CASH_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TRANSFER_PRICE " + Environment.NewLine);
            sb.Append("      ,T.PURCHASE_PRICE " + Environment.NewLine);
            sb.Append("      ,T.NO_TAX_PURCHASE_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TAX " + Environment.NewLine);
            sb.Append("      ,T.PAYMENT_PRICE " + Environment.NewLine);

            // 外税額＝税抜金額＋消費税額－金額(外税を含めない金額)
            sb.Append("      ,T.NO_TAX_PURCHASE_PRICE + T.TAX - T.PURCHASE_PRICE AS OUT_TAX " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_PRINT_FLG " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.PAYMENT_PRINT_FLG, 0) = 0 THEN '発行未' " + Environment.NewLine);
            sb.Append("            ELSE '発行済' END AS PAYMENT_PRINT_FLG_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_KBN " + Environment.NewLine);
            //sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_KBN, 0) = 0 THEN '締処理' " + Environment.NewLine);
            //sb.Append("            ELSE '都度請求' END AS INVOICE_KBN_NM " + Environment.NewLine);

            sb.Append("      ,RP_SUM.SUM_PRICE AS THIS_PAYMENT_CASH_PRICE " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN 0 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.PAYMENT_PRICE, 0) THEN 1 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.PAYMENT_PRICE, 0) THEN 2 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.PAYMENT_PRICE, 0) THEN 3 " + Environment.NewLine);
            sb.Append("       END AS PAYMENT_RECEIVABLE_KBN" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN '消込未' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.PAYMENT_PRICE, 0) THEN '一部消込' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.PAYMENT_PRICE, 0) THEN '消込済' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.PAYMENT_PRICE, 0) THEN '超過消込' " + Environment.NewLine);
            sb.Append("       END AS PAYMENT_RECEIVABLE_KBN_NM" + Environment.NewLine);

            sb.Append("      ,IFNULL(T.PAYMENT_PRICE, 0) - IFNULL(RP_SUM.SUM_PRICE, 0) AS PAYMENT_ZAN_PRICE " + Environment.NewLine);

            sb.Append("      ,T.MEMO " + Environment.NewLine);

            sb.Append("      ,CONCAT(lpad(CAST(SH.NO as char), " + this.idFigureSlipNo.ToString() + ", '0'),'-',lpad(CAST(OD.REC_NO as char), 2, '0')) AS NO_NM" + Environment.NewLine);

            sb.Append("      ,SH.NO AS H_NO " + Environment.NewLine);
            sb.Append("      ,replace(date_format(SH.PURCHASE_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PURCHASE_YMD" + Environment.NewLine);
            sb.Append("      ,SH.SUM_ENTER_NUMBER AS PURCHASE_SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH.SUM_CASE_NUMBER AS PURCHASE_SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH.SUM_NUMBER AS PURCHASE_SUM_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH.SUM_UNIT_PRICE AS PURCHASE_SUM_UNIT_PRICE" + Environment.NewLine);
            sb.Append("      ,SH.SUM_TAX AS PURCHASE_SUM_TAX" + Environment.NewLine);
            sb.Append("      ,SH.SUM_NO_TAX_PRICE AS PURCHASE_SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,SH.SUM_PRICE AS PURCHASE_SUM_PRICE" + Environment.NewLine);
            //sb.Append("      ,SH.SUM_PROFITS AS SALES_SUM_PROFITS" + Environment.NewLine);
            //sb.Append("      ,SH.PROFITS_PERCENT AS SALES_PROFITS_PERCENT" + Environment.NewLine);

            sb.Append("      ,SH_SUM.SUM_ENTER_NUMBER AS SH_PURCHASE_SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_CASE_NUMBER AS SH_PURCHASE_SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_NUMBER AS SH_PURCHASE_SUM_NUMBER" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_UNIT_PRICE AS SH_PURCHASE_SUM_UNIT_PRICE" + Environment.NewLine);
            //sb.Append("      ,SH_SUM.SUM_SALES_COST AS SH_PURCHASE_SUM_SALES_COST" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_TAX AS SH_PURCHASE_SUM_TAX" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_NO_TAX_PRICE AS SH_PURCHASE_SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,SH_SUM.SUM_PRICE AS SH_PURCHASE_SUM_PRICE" + Environment.NewLine);
            //sb.Append("      ,SH_SUM.SUM_PROFITS AS SH_PURCHASE_SUM_PROFITS" + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.BREAKDOWN_ID " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS BREAKDOWN_NM " + Environment.NewLine);
            sb.Append("      ,OD.DELIVER_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS DELIVER_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM5.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,CONCAT('入数 ', OD.ENTER_NUMBER) AS ENTER_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,CONCAT('ｹｰｽ数 ', OD.CASE_NUMBER) AS CASE_NUMBER_PRINT " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_PRICE " + Environment.NewLine);
            //sb.Append("      ,OD.SALES_COST " + Environment.NewLine);
            sb.Append("      ,OD.TAX AS D_TAX " + Environment.NewLine);
            sb.Append("      ,OD.NO_TAX_PRICE " + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            //sb.Append("      ,OD.PROFITS " + Environment.NewLine);
            //sb.Append("      ,OD.PROFITS_PERCENT AS D_PROFITS_PERCENT" + Environment.NewLine);
            sb.Append("      ,OD.TAX_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,NM6.DESCRIPTION AS TAX_DIVISION_NM " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PAYMENT AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_PURCHASE_H AS SH" + Environment.NewLine);
            sb.Append("    ON SH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND SH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH.PAYMENT_NO = T.NO" + Environment.NewLine);
            sb.Append("   AND SH.DELETE_FLG = 0" + Environment.NewLine);

            sb.Append(" INNER JOIN T_PURCHASE_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.PURCHASE_ID = SH.ID" + Environment.NewLine);
            sb.Append("   AND OD.DELETE_FLG = 0" + Environment.NewLine);

            sb.Append(" INNER JOIN (SELECT SH.COMPANY_ID" + Environment.NewLine);
            sb.Append("                   ,SH.GROUP_ID" + Environment.NewLine);
            sb.Append("                   ,SH.PAYMENT_NO" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_ENTER_NUMBER) AS SUM_ENTER_NUMBER" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_CASE_NUMBER) AS SUM_CASE_NUMBER" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NUMBER) AS SUM_NUMBER" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_UNIT_PRICE) AS SUM_UNIT_PRICE" + Environment.NewLine);
            //sb.Append("                   ,SUM(SH.SUM_PURCHASE_COST) AS SUM_PURCHASE_COST" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            //sb.Append("                   ,SUM(SH.SUM_PROFITS) AS SUM_PROFITS" + Environment.NewLine);
            sb.Append("               FROM T_PURCHASE_H AS SH" + Environment.NewLine);
            sb.Append("              WHERE SH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("              GROUP BY SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,SH.PAYMENT_NO " + Environment.NewLine);
            sb.Append("            ) AS SH_SUM" + Environment.NewLine);
            sb.Append("    ON SH_SUM.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND SH_SUM.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH_SUM.PAYMENT_NO = T.NO" + Environment.NewLine);

            // 出金済額
            sb.Append("  LEFT JOIN (SELECT RP.PAYMENT_NO" + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP " + Environment.NewLine);
            sb.Append("              WHERE RP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RP.COMPANY_ID = " + companyId + Environment.NewLine);

            sb.Append(GetGroupWhere(groupId, "RP"));

            sb.Append("              GROUP BY PAYMENT_NO " + Environment.NewLine);
            sb.Append("            ) AS RP_SUM " + Environment.NewLine);
            sb.Append("    ON RP_SUM.PAYMENT_NO = T.NO " + Environment.NewLine);


            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = T.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            // 回収サイクル
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = T.PAYMENT_CYCLE_ID" + Environment.NewLine);

            // 預金種別
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.ACCOUNT_KBN + Environment.NewLine);
            sb.Append("   AND NM2.ID = CG.BANK_ACCOUNT_KBN" + Environment.NewLine);

            // 内訳
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.BREAKDOWN_ID + Environment.NewLine);
            sb.Append("   AND OD.BREAKDOWN_ID = NM3.ID" + Environment.NewLine);

            // 納品区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.DELIVER_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND OD.DELIVER_DIVISION_ID = NM4.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM5" + Environment.NewLine);
            sb.Append("    ON NM5.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM5.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM5.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM5.ID" + Environment.NewLine);

            // 課税区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM6" + Environment.NewLine);
            sb.Append("    ON NM6.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM6.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM6.DIVISION_ID = " + (int)CommonUtl.geNameKbn.TAX_DIVISION_ID + Environment.NewLine);
            sb.Append("   AND NM6.ID = OD.TAX_DIVISION_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("         ,T.NO " + Environment.NewLine);
            sb.Append("         ,SH.NO " + Environment.NewLine);
            sb.Append("         ,OD.REC_NO " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetPaymentCashListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);

            sb.Append(GetTitleNm4("出金"));
            sb.Append(GetTotalKbn4());

            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), 10, '0') AS NO" + Environment.NewLine);
            sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_CASH_YMD" + Environment.NewLine);
            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS2.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_NO " + Environment.NewLine);
            sb.Append("      ,IV.PAYMENT_KBN " + Environment.NewLine);
            //sb.Append("      ,CASE WHEN IFNULL(IV.PAYMENT_KBN, 9999) = 9999 THEN '' " + Environment.NewLine);
            //sb.Append("            WHEN IFNULL(IV.PAYMENT_KBN, 9999) = 0 THEN '締処理' " + Environment.NewLine);
            //sb.Append("            ELSE '都度請求' END AS PAYMENT_KBN_NM " + Environment.NewLine);
            sb.Append("      ,date_format(IV.PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + ") AS PAYMENT_YYYYMMDD" + Environment.NewLine);
            sb.Append("      ,date_format(IV.PAYMENT_PLAN_DAY , " + ExEscape.SQL_YMD + ") AS PAYMENT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS PURCHASE_NM " + Environment.NewLine);
            sb.Append("      ,CS.PAYMENT_DIVISION_ID AS H_PAYMENT_DIVISION_ID" + Environment.NewLine);
            sb.Append("      ,RD.DESCRIPTION AS H_PAYMENT_DIVISION_NM" + Environment.NewLine);
            sb.Append("      ,IV.SUMMING_UP_GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);

            sb.Append("      ,T.SUM_PRICE " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);
            sb.Append("      ,T.UPDATE_PERSON_ID " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS UPDATE_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,OD.PAYMENT_CASH_DIVISION_ID AS PAYMENT_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,OD.DESCRIPTION AS PAYMENT_DIVISION_NM" + Environment.NewLine);
            sb.Append("      ,replace(date_format(OD.BILL_DUE_DATE , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS BILL_DUE_DATE" + Environment.NewLine);
            sb.Append("      ,OD.PRICE " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PAYMENT_CASH_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_PAYMENT_CASH_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // 入力担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS2" + Environment.NewLine);
            sb.Append("    ON PS2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS2.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS2.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS2.ID = T.PERSON_ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            //sb.Append("   AND PS.GROUP_ID = " + groupId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.UPDATE_PERSON_ID" + Environment.NewLine);

            //仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 請求
            sb.Append("  LEFT JOIN T_PAYMENT AS IV" + Environment.NewLine);
            sb.Append("    ON IV.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND IV.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND IV.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND IV.NO = T.PAYMENT_NO" + Environment.NewLine);

            // 出金区分
            sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
            sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND RD.COMPANY_ID = CS.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND RD.ID = CS.PAYMENT_DIVISION_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = IV.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetPaymentCashDDMMTotalReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _group_kbn = "";
            int group_kbn = strWhereSql.IndexOf("<group kbn>");
            if (group_kbn != -1)
            {
                _group_kbn = strWhereSql.Substring(group_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, group_kbn);
            }

            sb.Append("SELECT T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (_group_kbn == "1")
            {
                sb.Append(GetTitleNm4("出金", "日報"));
            }
            else
            {
                sb.Append(GetTitleNm4("出金", "月報"));
            }
            sb.Append(GetTotalKbnDDMM4(ExCast.zCInt(_group_kbn)));

            sb.Append("      ,IFNULL(SUM(OD_CASH.PRICE), 0) AS CASH_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_CHECK.PRICE), 0) AS CHECK_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_BANK_ACCOUNT.PRICE), 0) AS BANK_ACCOUNT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_COMMISSION.PRICE), 0) AS COMMISSION_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_BILL.PRICE), 0) AS BILL_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_ANOTHER.PRICE), 0) AS ANOTHER_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SUM(OD_CASH.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_CHECK.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_BANK_ACCOUNT.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_COMMISSION.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_BILL.PRICE), 0) + " + Environment.NewLine);
            sb.Append("       IFNULL(SUM(OD_ANOTHER.PRICE), 0) AS SUM_PRICE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PAYMENT_CASH_H AS T" + Environment.NewLine);

            #region Join

            // 現金
            sb.Append("  LEFT JOIN T_PAYMENT_CASH_D AS OD_CASH" + Environment.NewLine);
            sb.Append("    ON OD_CASH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_CASH.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_CASH.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_CASH.PAYMENT_CASH_DIVISION_ID = '101'" + Environment.NewLine);
            sb.Append("   AND OD_CASH.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // 小切手
            sb.Append("  LEFT JOIN T_PAYMENT_CASH_D AS OD_CHECK" + Environment.NewLine);
            sb.Append("    ON OD_CHECK.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_CHECK.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_CHECK.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_CHECK.PAYMENT_CASH_DIVISION_ID = '102'" + Environment.NewLine);
            sb.Append("   AND OD_CHECK.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // 振込
            sb.Append("  LEFT JOIN T_PAYMENT_CASH_D AS OD_BANK_ACCOUNT" + Environment.NewLine);
            sb.Append("    ON OD_BANK_ACCOUNT.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.PAYMENT_CASH_DIVISION_ID = '201'" + Environment.NewLine);
            sb.Append("   AND OD_BANK_ACCOUNT.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // 手数料
            sb.Append("  LEFT JOIN T_PAYMENT_CASH_D AS OD_COMMISSION" + Environment.NewLine);
            sb.Append("    ON OD_COMMISSION.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.PAYMENT_CASH_DIVISION_ID = '301'" + Environment.NewLine);
            sb.Append("   AND OD_COMMISSION.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // 手形
            sb.Append("  LEFT JOIN T_PAYMENT_CASH_D AS OD_BILL" + Environment.NewLine);
            sb.Append("    ON OD_BILL.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_BILL.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_BILL.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_BILL.PAYMENT_CASH_DIVISION_ID IN ('401','402')" + Environment.NewLine);
            sb.Append("   AND OD_BILL.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // その他
            sb.Append("  LEFT JOIN T_PAYMENT_CASH_D AS OD_ANOTHER" + Environment.NewLine);
            sb.Append("    ON OD_ANOTHER.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.PAYMENT_CASH_DIVISION_ID NOT IN ('101','102','201','301','401','402')" + Environment.NewLine);
            sb.Append("   AND OD_ANOTHER.PAYMENT_CASH_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            //sb.Append(" LIMIT 0, 1000");

            #region Group by


            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 1:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PURCHASE_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("               ELSE T.PURCHASE_ID END" + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        sb.Append("         ,T.PERSON_ID " + Environment.NewLine);
                        sb.Append(" ORDER BY T.PERSON_ID " + Environment.NewLine);
                        break;
                    default:
                        sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                        sb.Append("         ,CG.NAME " + Environment.NewLine);
                        if (_group_kbn == "1")
                        {
                            // 日報時
                            sb.Append("         ,T.PAYMENT_CASH_YMD " + Environment.NewLine);
                            sb.Append(" ORDER BY T.PAYMENT_CASH_YMD " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("         ,date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                            sb.Append(" ORDER BY date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                sb.Append(" GROUP BY T.GROUP_ID " + Environment.NewLine);
                sb.Append("         ,CG.NAME " + Environment.NewLine);
                if (_group_kbn == "1")
                {
                    // 日報時
                    sb.Append("         ,T.PAYMENT_CASH_YMD " + Environment.NewLine);
                    sb.Append(" ORDER BY T.PAYMENT_CASH_YMD " + Environment.NewLine);
                }
                else
                {
                    // 月報時
                    sb.Append("         ,date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                    sb.Append(" ORDER BY date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + ") " + Environment.NewLine);
                }
            }

            #endregion

            #endregion

            return sb.ToString();

        }

        public string GetPaymentPlanReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string rep_strWhereSql = "";
            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";

            rep_strWhereSql = _strWhereSql.Replace("<<@escape_comma@>>", ",");

            GetStringWhere(rep_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);

            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS PURCHASE_NM " + Environment.NewLine);
            sb.Append("      ,CS.PAYMENT_DIVISION_ID " + Environment.NewLine);
            sb.Append("      ,RD.DESCRIPTION AS PAYMENT_DIVISION_NM" + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.PAYMENT_PLAN_DAY , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_PRICE " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PAYMENT AS T" + Environment.NewLine);

            #region Join

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 入金区分
            sb.Append("  LEFT JOIN SYS_M_RECEIPT_DIVISION AS RD" + Environment.NewLine);
            sb.Append("    ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND RD.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND RD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND RD.ID = CS.PAYMENT_DIVISION_ID " + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("         ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("               ELSE T.PURCHASE_ID END" + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetPaymentCreditBalanaceListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string proc_ym = "";
            int proc_ym_index = strWhereSql.IndexOf("<proc ym>");
            if (proc_ym_index != -1)
            {
                proc_ym = strWhereSql.Substring(proc_ym_index + 9, 7);
                strWhereSql = strWhereSql.Substring(0, proc_ym_index);
            }

            DateTime before_ym_ = ExCast.zConvertToDate(proc_ym + "/01");
            DateTime before_ym = before_ym_.AddDays(-1);
            DateTime this_ym_from = ExCast.zConvertToDate(proc_ym + "/01");
            DateTime this_ym_to = this_ym_from.AddMonths(1).AddDays(-1);

            sb.Append("SELECT CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,CG.ID AS GROUP_ID " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);
            sb.Append("      ," + ExEscape.zRepStr(proc_ym) + " AS YM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CM.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CM.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE CM.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,CM.PURCHASE_NAME " + Environment.NewLine);

            sb.Append("      ,IFNULL(SB.PAYMENT_CREDIT_INIT_PRICE, 0) AS PAYMENT_CREDIT_INIT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_BEFORE.SUM_TAX, 0) AS SUM_TAX" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_BEFORE.SUM_PRICE, 0) AS SUM_PRICE " + Environment.NewLine);

            sb.Append("      ,IFNULL(SB.PAYMENT_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0)) AS BEFORE_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS.SUM_PRICE, 0) AS THIS_PAYMENT_CASH_PRICE " + Environment.NewLine);
            sb.Append("      ,IFNULL(((IFNULL(RP_THIS.SUM_PRICE, 0)) / (IFNULL(SB.PAYMENT_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0)))) * 100, 0) AS THIS_PAYMENT_CASH_PERCENT " + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_THIS.SUM_NO_TAX_PRICE, 0) AS THIS_PURCHASE_PRICE " + Environment.NewLine);
            sb.Append("      ,IFNULL(SH_THIS.SUM_TAX, 0) AS THIS_PURCHASE_TAX " + Environment.NewLine);
            sb.Append("      ,(IFNULL(SB.PAYMENT_CREDIT_INIT_PRICE, 0) + (IFNULL(SH_BEFORE.SUM_NO_TAX_PRICE, 0) + IFNULL(SH_BEFORE.SUM_TAX, 0)) - (IFNULL(RP_BEFORE.SUM_PRICE, 0))) - (IFNULL(RP_THIS.SUM_PRICE, 0)) + IFNULL(SH_THIS.SUM_TAX, 0) + IFNULL(SH_THIS.SUM_NO_TAX_PRICE, 0) AS THIS_PAYMENT_CREDIT_BALANCE " + Environment.NewLine);

            sb.Append("      ,IFNULL(RP_THIS_CASH.SUM_PRICE, 0) AS THIS_CASH_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_ACCOUNT.SUM_PRICE, 0) AS THIS_ACCOUNT_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_COMMISSION.SUM_PRICE, 0) AS THIS_COMMISSION_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_BILL.SUM_PRICE, 0) AS THIS_BILL_PRICE" + Environment.NewLine);
            sb.Append("      ,IFNULL(RP_THIS_ANOTHER.SUM_PRICE, 0) AS THIS_ANOTHER_PRICE" + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM (SELECT DISTINCT " + Environment.NewLine);
            sb.Append("               CM.ID AS PURCHASE_ID" + Environment.NewLine);
            sb.Append("              ,CM.ID" + Environment.NewLine);
            sb.Append("              ,CM.ID2 " + Environment.NewLine);
            sb.Append("              ,CM.NAME AS PURCHASE_NAME" + Environment.NewLine);
            sb.Append("              ,CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("              ,CM.DELETE_FLG " + Environment.NewLine);
            sb.Append("          FROM M_PURCHASE AS CM" + Environment.NewLine);
            sb.Append("         WHERE CM.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("           AND CM.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("           AND CM.DISPLAY_FLG = 1" + Environment.NewLine);
            sb.Append("        ) AS CM" + Environment.NewLine);

            #region Join

            #region 初期買掛残高

            sb.Append("  LEFT JOIN M_PAYMENT_CREDIT_BALANCE AS SB" + Environment.NewLine);
            sb.Append("    ON SB.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND SB.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "SB"));    // グループ集計条件
            sb.Append("   AND SB.ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月以前の掛仕入

            sb.Append("  LEFT JOIN (SELECT SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,SH.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("               FROM T_PURCHASE_H AS SH" + Environment.NewLine);
            sb.Append("              WHERE SH.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "SH", "             "));    // グループ集計条件
            sb.Append("                AND SH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND SH.BUSINESS_DIVISION_ID IN (1, 3)" + Environment.NewLine);  // 掛仕入,都度請求
            sb.Append("                AND SH.PURCHASE_YMD <= " + ExEscape.zRepStr(before_ym.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,SH.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS SH_BEFORE" + Environment.NewLine);
            sb.Append("    ON SH_BEFORE.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND SH_BEFORE.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH_BEFORE.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月以前の出金

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(before_ym.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_BEFORE" + Environment.NewLine);
            sb.Append("    ON RP_BEFORE.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_BEFORE.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_BEFORE.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の掛仕入

            sb.Append("  LEFT JOIN (SELECT SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,SH.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_NO_TAX_PRICE) AS SUM_NO_TAX_PRICE" + Environment.NewLine);
            sb.Append("                   ,SUM(SH.SUM_TAX) AS SUM_TAX" + Environment.NewLine);
            sb.Append("               FROM T_PURCHASE_H AS SH" + Environment.NewLine);
            sb.Append("              WHERE SH.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "SH", "             "));    // グループ集計条件
            sb.Append("                AND SH.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND SH.BUSINESS_DIVISION_ID IN (1, 3)" + Environment.NewLine);  // 掛仕入,都度請求
            sb.Append("                AND SH.PURCHASE_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND SH.PURCHASE_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY SH.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,SH.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,SH.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS SH_THIS" + Environment.NewLine);
            sb.Append("    ON SH_THIS.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND SH_THIS.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND SH_THIS.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の出金

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS" + Environment.NewLine);
            sb.Append("    ON RP_THIS.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の出金(現金)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_PAYMENT_CASH_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_DIVISION_ID = '101' " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_CASH" + Environment.NewLine);
            sb.Append("    ON RP_THIS_CASH.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_CASH.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_CASH.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の出金(振込)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_PAYMENT_CASH_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_DIVISION_ID = '201' " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_ACCOUNT" + Environment.NewLine);
            sb.Append("    ON RP_THIS_ACCOUNT.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_ACCOUNT.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_ACCOUNT.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の出金(手数料)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_PAYMENT_CASH_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_DIVISION_ID = '301' " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_COMMISSION" + Environment.NewLine);
            sb.Append("    ON RP_THIS_COMMISSION.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_COMMISSION.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_COMMISSION.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の出金(手形)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_PAYMENT_CASH_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_DIVISION_ID IN ('401','402') " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_BILL" + Environment.NewLine);
            sb.Append("    ON RP_THIS_BILL.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_BILL.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_BILL.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            #region 対象年月の出金(その他)

            sb.Append("  LEFT JOIN (SELECT RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                   ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                   ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("                   ,SUM(RD.PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP" + Environment.NewLine);
            sb.Append("              INNER JOIN T_PAYMENT_CASH_D AS RD" + Environment.NewLine);
            sb.Append("                 ON RD.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RD.COMPANY_ID = RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                AND RD.GROUP_ID = RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_ID = RP.ID " + Environment.NewLine);
            sb.Append("                AND RD.PAYMENT_CASH_DIVISION_ID NOT IN ('101','201','301','401','402') " + Environment.NewLine);
            sb.Append("              WHERE RP.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "RP", "             "));    // グループ集計条件
            sb.Append("                AND RP.DELETE_FLG = 0" + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("                AND RP.PAYMENT_CASH_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
            sb.Append("              GROUP BY RP.COMPANY_ID " + Environment.NewLine);
            sb.Append("                      ,RP.GROUP_ID " + Environment.NewLine);
            sb.Append("                      ,RP.PURCHASE_ID " + Environment.NewLine);
            sb.Append("             ) AS RP_THIS_ANOTHER" + Environment.NewLine);
            sb.Append("    ON RP_THIS_ANOTHER.COMPANY_ID = CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("   AND RP_THIS_ANOTHER.GROUP_ID = SB.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND RP_THIS_ANOTHER.PURCHASE_ID = CM.PURCHASE_ID" + Environment.NewLine);

            #endregion

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = CM.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = CM.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = SB.GROUP_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE CM.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CM.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            sb.Append(" ORDER BY CM.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,CG.ID " + Environment.NewLine);
            sb.Append("         ,CM.ID2 " + Environment.NewLine);
            sb.Append("         ,CM.ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }

        public string GetPaymentBalanceListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS PURCHASE_NM " + Environment.NewLine);
            sb.Append("      ,CS.PERSON_NAME AS PURCHASE_PERSON_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CONCAT(CS.NAME, ' ', CS.TITLE_NAME) " + Environment.NewLine);
            sb.Append("            ELSE CS.NAME END AS PURCHASE_NM_TITLE_NAME " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(CS.PERSON_NAME, '') = '' THEN CS.PERSON_NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CS.PERSON_NAME, ' ', CS.TITLE_NAME) END AS PURCHASE_PERSON_NM_TITLE_NAME " + Environment.NewLine);

            sb.Append("      ,CASE WHEN CP.NAME = CG.NAME THEN CP.NAME " + Environment.NewLine);
            sb.Append("            ELSE CONCAT(CP.NAME, ' ', CG.NAME) END AS COMPANY_NM " + Environment.NewLine);
            sb.Append("      ,CONCAT(SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),1,3),'-',SUBSTR(LPAD(CG.ZIP_CODE, 7, '0'),4,4)) AS COMPANY_ZIP_CODE " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS1 AS COMPANY_ADRESS1 " + Environment.NewLine);
            sb.Append("      ,CG.ADRESS2 AS COMPANY_ADRESS2 " + Environment.NewLine);
            sb.Append("      ,CG.TEL AS COMPANY_TEL " + Environment.NewLine);
            sb.Append("      ,CG.FAX AS COMPANY_FAX " + Environment.NewLine);
            sb.Append("      ,CG.MAIL_ADRESS AS COMPANY_MAIL_ADRESS " + Environment.NewLine);
            sb.Append("      ,CG.URL AS COMPANY_URL " + Environment.NewLine);
            sb.Append("      ,CG.BANK_NAME AS COMPANY_BANK_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_BRANCH_NAME AS COMPANY_BANK_BRANCH_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NO AS COMPANY_BANK_ACCOUNT_NO " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_NAME AS COMPANY_BANK_ACCOUNT_NAME " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KANA AS COMPANY_BANK_ACCOUNT_KANA " + Environment.NewLine);
            sb.Append("      ,CG.BANK_ACCOUNT_KBN AS BANK_ACCOUNT_KBN " + Environment.NewLine);
            sb.Append("      ,IFNULL(NM2.DESCRIPTION, '普通') AS BANK_ACCOUNT_KBN_NM " + Environment.NewLine);

            sb.Append("      ,CONCAT('口座名義：', CG.BANK_ACCOUNT_NAME, ' カナ表記：', CG.BANK_ACCOUNT_KANA) AS ACCOUNT_INF1 " + Environment.NewLine);
            sb.Append("      ,CONCAT(CG.BANK_NAME, ' ', CG.BANK_BRANCH_NAME, ' ', IFNULL(NM2.DESCRIPTION, '普通'), ' ', CG.BANK_ACCOUNT_NO) AS ACCOUNT_INF2 " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.SUMMING_UP_GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CN.NAME AS SUMMING_UP_GROUP_NM " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.BEFORE_PAYMENT_YYYYMMDD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS BEFORE_PAYMENT_YYYYMMDD" + Environment.NewLine);

            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_CYCLE_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS PAYMENT_CYCLE_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_DAY " + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.PAYMENT_PLAN_DAY , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS PAYMENT_PLAN_DAY" + Environment.NewLine);

            sb.Append("      ,T.BEFORE_PAYMENT_PRICE " + Environment.NewLine);
            sb.Append("      ,T.PAYMENT_CASH_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TRANSFER_PRICE " + Environment.NewLine);
            sb.Append("      ,T.PURCHASE_PRICE " + Environment.NewLine);
            sb.Append("      ,T.NO_TAX_PURCHASE_PRICE " + Environment.NewLine);
            sb.Append("      ,T.TAX " + Environment.NewLine);
            sb.Append("      ,T.PAYMENT_PRICE " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_PRINT_FLG " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.PAYMENT_PRINT_FLG, 0) = 0 THEN '発行未' " + Environment.NewLine);
            sb.Append("            ELSE '発行済' END AS PAYMENT_PRINT_FLG_NM " + Environment.NewLine);

            sb.Append("      ,T.PAYMENT_KBN " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.PAYMENT_KBN, 0) = 0 THEN '締処理' " + Environment.NewLine);
            sb.Append("            ELSE '都度請求' END AS PAYMENT_KBN_NM " + Environment.NewLine);

            sb.Append("      ,RP_SUM.SUM_PRICE AS THIS_PAYMENT_CASH_PRICE " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN 0 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.PAYMENT_PRICE, 0) THEN 1 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.PAYMENT_PRICE, 0) THEN 2 " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.PAYMENT_PRICE, 0) THEN 3 " + Environment.NewLine);
            sb.Append("       END AS PAYMENT_CASH_RECEIVABLE_KBN" + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = 0 THEN '消込未' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) < IFNULL(T.PAYMENT_PRICE, 0) THEN '一部消込' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) = IFNULL(T.PAYMENT_PRICE, 0) THEN '消込済' " + Environment.NewLine);
            sb.Append("            WHEN IFNULL(RP_SUM.SUM_PRICE, 0) > IFNULL(T.PAYMENT_PRICE, 0) THEN '超過消込' " + Environment.NewLine);
            sb.Append("       END AS PAYMENT_CASH_RECEIVABLE_KBN_NM" + Environment.NewLine);

            sb.Append("      ,IFNULL(T.PAYMENT_PRICE, 0) - IFNULL(RP_SUM.SUM_PRICE, 0) AS PAYMENT_ZAN_PRICE " + Environment.NewLine);

            sb.Append("      ,T.MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_PAYMENT AS T" + Environment.NewLine);

            #region Join

            // 出金済額
            sb.Append("  LEFT JOIN (SELECT RP.PAYMENT_NO" + Environment.NewLine);
            sb.Append("                   ,SUM(RP.SUM_PRICE) AS SUM_PRICE" + Environment.NewLine);
            sb.Append("               FROM T_PAYMENT_CASH_H AS RP " + Environment.NewLine);
            sb.Append("              WHERE RP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("                AND RP.COMPANY_ID = " + companyId + Environment.NewLine);

            sb.Append(GetGroupWhere(groupId, "RP"));

            sb.Append("              GROUP BY PAYMENT_NO " + Environment.NewLine);
            sb.Append("            ) AS RP_SUM " + Environment.NewLine);
            sb.Append("    ON RP_SUM.PAYMENT_NO = T.NO " + Environment.NewLine);


            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND CS.ID = T.PURCHASE_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 締グループ
            sb.Append("  LEFT JOIN M_CONDITION AS CN" + Environment.NewLine);
            sb.Append("    ON CN.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CN.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CN.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CN.CONDITION_DIVISION_ID = 1" + Environment.NewLine);
            sb.Append("   AND CN.ID = T.SUMMING_UP_GROUP_ID" + Environment.NewLine);

            // 支払サイクル
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.COLLECT_CYCLE_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = T.PAYMENT_CYCLE_ID" + Environment.NewLine);

            // 預金種別
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.ACCOUNT_KBN + Environment.NewLine);
            sb.Append("   AND NM2.ID = CG.BANK_ACCOUNT_KBN" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("         ,CS.ID2 " + Environment.NewLine);
            sb.Append("         ,CS.ID " + Environment.NewLine);
            sb.Append("         ,T.NO " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            return sb.ToString();

        }
        
        #endregion

        #region Inventory

        public string GetInOutDeliveryListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            sb.Append("SELECT 0 AS RECORD_NO " + Environment.NewLine);
            sb.Append("      ,T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,T.GROUP_ID " + Environment.NewLine);
            sb.Append("      ,CG.NAME AS GROUP_NAME " + Environment.NewLine);
            sb.Append("      ,T.ID " + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS NO" + Environment.NewLine);
            sb.Append("      ,lpad(CAST(T.CAUSE_NO as char), " + this.idFigureSlipNo.ToString() + ", '0') AS CAUSE_NO" + Environment.NewLine);

            sb.Append("      ,replace(date_format(T.IN_OUT_DELIVERY_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS IN_OUT_DELIVERY_YMD" + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);

            sb.Append("      ,T.IN_OUT_DELIVERY_KBN " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS IN_OUT_DELIVERY_KBN_NM " + Environment.NewLine);

            sb.Append("      ,T.IN_OUT_DELIVERY_PROC_KBN " + Environment.NewLine);
            sb.Append("      ,NM2.DESCRIPTION AS IN_OUT_DELIVERY_PROC_KBN_NM " + Environment.NewLine);

            sb.Append("      ,T.PERSON_ID AS INPUT_PERSON " + Environment.NewLine);
            sb.Append("      ,PS.NAME AS INPUT_PERSON_NM " + Environment.NewLine);

            sb.Append("      ,T.IN_OUT_DELIVERY_TO_KBN " + Environment.NewLine);
            sb.Append("      ,NM3.DESCRIPTION AS IN_OUT_DELIVERY_TO_KBN_NM " + Environment.NewLine);

            sb.Append("      ,T.GROUP_ID_TO " + Environment.NewLine);
            sb.Append("      ,CG2.NAME AS GROUP_ID_TO_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.CUSTOMER_ID END AS CUSTOMER_ID " + Environment.NewLine);
            sb.Append("      ,CS.NAME AS CUSTOMER_NM " + Environment.NewLine);

            sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.PURCHASE_ID END AS PURCHASE_ID " + Environment.NewLine);
            sb.Append("      ,PU.NAME AS PURCHASE_NM " + Environment.NewLine);

            sb.Append("      ,T.SUM_ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.SUM_NUMBER " + Environment.NewLine);
            sb.Append("      ,T.MEMO " + Environment.NewLine);

            sb.Append("      ,OD.REC_NO " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(OD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(OD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE OD.COMMODITY_ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,OD.COMMODITY_NAME " + Environment.NewLine);
            sb.Append("      ,OD.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM4.DESCRIPTION AS UNIT_NM " + Environment.NewLine);
            sb.Append("      ,OD.ENTER_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.CASE_NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.NUMBER " + Environment.NewLine);
            sb.Append("      ,OD.MEMO AS D_MEMO " + Environment.NewLine);

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM T_IN_OUT_DELIVERY_H AS T" + Environment.NewLine);

            #region Join

            sb.Append(" INNER JOIN T_IN_OUT_DELIVERY_D AS OD" + Environment.NewLine);
            sb.Append("    ON OD.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND OD.GROUP_ID = T.GROUP_ID" + Environment.NewLine);
            sb.Append("   AND OD.IN_OUT_DELIVERY_ID = T.ID" + Environment.NewLine);

            // 更新担当者(ヘッダ)
            sb.Append("  LEFT JOIN M_PERSON AS PS" + Environment.NewLine);
            sb.Append("    ON PS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND PS.ID = T.PERSON_ID" + Environment.NewLine);

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = T.GROUP_ID" + Environment.NewLine);

            // 会社グループ2
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG2" + Environment.NewLine);
            sb.Append("    ON CG2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG2.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG2.ID = T.GROUP_ID_TO" + Environment.NewLine);

            // 得意先
            sb.Append("  LEFT JOIN M_CUSTOMER AS CS" + Environment.NewLine);
            sb.Append("    ON CS.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CS.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND CS.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.CUSTOMER_ID = CS.ID" + Environment.NewLine);

            // 仕入先
            sb.Append("  LEFT JOIN M_PURCHASE AS PU" + Environment.NewLine);
            sb.Append("    ON PU.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND PU.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND PU.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.PURCHASE_ID = PU.ID" + Environment.NewLine);

            // 入出庫区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.IN_OUT_DELIVERY_KBN + Environment.NewLine);
            sb.Append("   AND T.IN_OUT_DELIVERY_KBN = NM1.ID" + Environment.NewLine);

            // 入出庫処理区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM2" + Environment.NewLine);
            sb.Append("    ON NM2.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM2.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM2.DIVISION_ID = " + (int)CommonUtl.geNameKbn.IN_OUT_DELIVERY_PROC_KBN + Environment.NewLine);
            sb.Append("   AND T.IN_OUT_DELIVERY_PROC_KBN = NM2.ID" + Environment.NewLine);

            // 入出庫先区分
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM3" + Environment.NewLine);
            sb.Append("    ON NM3.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM3.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM3.DIVISION_ID = " + (int)CommonUtl.geNameKbn.IN_OUT_DELIVERY_TO_KBN + Environment.NewLine);
            sb.Append("   AND T.IN_OUT_DELIVERY_TO_KBN = NM3.ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM4" + Environment.NewLine);
            sb.Append("    ON NM4.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM4.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM4.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND OD.UNIT_ID = NM4.ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            // グループ集計条件
            sb.Append(GetGroupWhere(groupId, "T"));

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,T.GROUP_ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion


            return sb.ToString();

        }

        public string GetStockInventoryListReportSQL(string companyId, string groupId, string _strWhereSql, string strOrderBySql)
        {
            StringBuilder sb = new StringBuilder();

            #region SQL

            string strWhereSql = "";
            string stringWhere1 = "";
            string stringWhere2 = "";
            GetStringWhere(_strWhereSql, ref strWhereSql, ref stringWhere2, ref stringWhere1);

            string _print_kbn = "";
            int print_kbn = strWhereSql.IndexOf("<print kbn>");
            if (print_kbn != -1)
            {
                _print_kbn = strWhereSql.Substring(print_kbn + 11, 1);
                strWhereSql = strWhereSql.Substring(0, print_kbn);
            }

            string proc_ym = "";
            DateTime before_ym_ = new DateTime();
            DateTime before_ym = new DateTime();
            DateTime this_ym_from = new DateTime();
            DateTime this_ym_to = new DateTime();
            if (_print_kbn == "1")
            {
                int proc_ym_index = strWhereSql.IndexOf("<proc ym>");
                if (proc_ym_index != -1)
                {
                    proc_ym = strWhereSql.Substring(proc_ym_index + 9, 7);
                    strWhereSql = strWhereSql.Substring(0, proc_ym_index);
                }

                before_ym_ = ExCast.zConvertToDate(proc_ym + "/01");
                before_ym = before_ym_.AddDays(-1);
                this_ym_from = ExCast.zConvertToDate(proc_ym + "/01");
                this_ym_to = this_ym_from.AddMonths(1).AddDays(-1);
            }

            sb.Append("SELECT T.COMPANY_ID " + Environment.NewLine);
            sb.Append("      ,CG.ID AS GROUP_ID " + Environment.NewLine);
            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine);
            sb.Append("      ,'' AS YM " + Environment.NewLine);
            sb.Append("      ,CASE WHEN IFNULL(T.ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
            sb.Append("            ELSE T.ID END AS COMMODITY_ID " + Environment.NewLine);
            sb.Append("      ,T.NAME AS COMMODITY_NAME " + Environment.NewLine);

            sb.Append("      ,T.UNIT_ID " + Environment.NewLine);
            sb.Append("      ,NM1.DESCRIPTION AS UNIT_NM " + Environment.NewLine);

            
            //sb.Append("      ,T.INVENTORY_NUMBER " + Environment.NewLine);
            sb.Append("      ,CI.INVENTORY_NUMBER " + Environment.NewLine);

            if (_print_kbn == "1")
            {
                sb.Append("      ,IFNULL(BEFORE_IN.SUM_NUMBER, 0) AS BEFORE_IN_NUMBER" + Environment.NewLine);                         // 対象年月以前の入庫
                sb.Append("      ,IFNULL(BEFORE_OUT.SUM_NUMBER, 0) * -1 AS BEFORE_OUT_NUMBER" + Environment.NewLine);                  // 対象年月以前の出庫
                sb.Append("      ,IFNULL(THIS_PURCHASE_IN.SUM_NUMBER, 0) AS THIS_PURCHASE_IN_NUMBER" + Environment.NewLine);           // 対象年月の仕入入庫
                sb.Append("      ,IFNULL(THIS_SALES_OUT.SUM_NUMBER, 0) * -1 AS THIS_SALES_OUT_NUMBER" + Environment.NewLine);          // 対象年月の売上出庫
                sb.Append("      ,IFNULL(THIS_ANOTHER_IN.SUM_NUMBER, 0) AS THIS_ANOTHER_IN_NUMBER" + Environment.NewLine);             // 対象年月のその他入庫
                sb.Append("      ,IFNULL(THIS_ANOTHER_OUT.SUM_NUMBER, 0) * -1 AS THIS_ANOTHER_OUT_NUMBER" + Environment.NewLine);      // 対象年月のその他出庫
                sb.Append("      ,IFNULL(THIS_ANOTHER_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       - IFNULL(THIS_ANOTHER_OUT.SUM_NUMBER, 0) AS THIS_ANOTHER_IN_OUT_NUMBER" + Environment.NewLine);      // 対象年月のその他入出庫

                // 前月在庫
                sb.Append("      ,IFNULL(BEFORE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       - IFNULL(BEFORE_OUT.SUM_NUMBER, 0) AS BEFORE_NUMBER" + Environment.NewLine);

                // 現在庫
                sb.Append("      ,IFNULL(BEFORE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       - IFNULL(BEFORE_OUT.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       + IFNULL(THIS_PURCHASE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       - IFNULL(THIS_SALES_OUT.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       + IFNULL(THIS_ANOTHER_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("       - IFNULL(THIS_ANOTHER_OUT.SUM_NUMBER, 0) AS THIS_INVENTORY_NUMBER" + Environment.NewLine);

                // 在庫単価
                sb.Append("      ,IFNULL(T.PURCHASE_UNIT_PRICE_SKIP_TAX, 0) AS PURCHASE_UNIT_PRICE_SKIP_TAX" + Environment.NewLine);        // 仕入単価税抜
                sb.Append("      ,IFNULL(T.PURCHASE_UNIT_PRICE_BEFORE_TAX, 0) AS PURCHASE_UNIT_PRICE_BEFORE_TAX" + Environment.NewLine);    // 仕入単価税込

                // 在庫金額
                sb.Append("      ,(IFNULL(BEFORE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        - IFNULL(BEFORE_OUT.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        + IFNULL(THIS_PURCHASE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        - IFNULL(THIS_SALES_OUT.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        + IFNULL(THIS_ANOTHER_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        - IFNULL(THIS_ANOTHER_OUT.SUM_NUMBER, 0)) " + Environment.NewLine);
                sb.Append("        * IFNULL(T.PURCHASE_UNIT_PRICE_SKIP_TAX, 0) AS INVENTORY_PRICE_SKIP_TAX" + Environment.NewLine);         // 在庫金額税抜

                sb.Append("      ,(IFNULL(BEFORE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        - IFNULL(BEFORE_OUT.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        + IFNULL(THIS_PURCHASE_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        - IFNULL(THIS_SALES_OUT.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        + IFNULL(THIS_ANOTHER_IN.SUM_NUMBER, 0) " + Environment.NewLine);
                sb.Append("        - IFNULL(THIS_ANOTHER_OUT.SUM_NUMBER, 0)) " + Environment.NewLine);
                sb.Append("        * IFNULL(T.PURCHASE_UNIT_PRICE_BEFORE_TAX, 0) AS INVENTORY_PRICE_BEFORE_TAX" + Environment.NewLine);     // 在庫金額税込

                sb.Append("      ,IFNULL(MAX_IN_YMD.IN_OUT_DELIVERY_YMD, '') AS MAX_IN_DELIVERY_YMD" + Environment.NewLine);                // 最終入庫日
                sb.Append("      ,IFNULL(MAX_OUT_YMD.IN_OUT_DELIVERY_YMD, '') AS MAX_OUT_DELIVERY_YMD" + Environment.NewLine);              // 最終出庫日
            }

            sb.Append("      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.USER_NM]) + "' AS USER_NAME " + Environment.NewLine);
            sb.Append("      ,CONCAT('" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "：'" +
                                    ",CG.NAME) AS GROUP_RANGE " + Environment.NewLine);

            if (entitySetting != null)
            {
                if (entitySetting.group_id_from != entitySetting.group_id_to)
                {
                    stringWhere1 = "[" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + " " +
                                                      entitySetting.group_id_from + "：" + entitySetting.group_nm_from + "～" +
                                                      entitySetting.group_id_to + "：" + entitySetting.group_nm_to + "] " + stringWhere1;
                }
            }

            sb.Append("      ,'" + stringWhere1 + "' AS STRING_WHERE1 " + Environment.NewLine);
            sb.Append("      ,'" + stringWhere2 + "' AS STRING_WHERE2 " + Environment.NewLine);

            sb.Append("  FROM M_COMMODITY AS T" + Environment.NewLine);

            #region Join

            // 商品在庫
            sb.Append("  LEFT JOIN M_COMMODITY_INVENTORY AS CI" + Environment.NewLine);
            sb.Append("    ON CI.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CI.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append(GetGroupWhere(groupId, "CI"));    // グループ集計条件
            sb.Append("   AND CI.ID = T.ID " + Environment.NewLine);

            if (_print_kbn == "1")
            {
                #region 対象年月以前の入庫

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IH.IN_OUT_DELIVERY_KBN " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,SUM(IND.NUMBER) AS SUM_NUMBER" + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(before_ym.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_KBN = 1" + Environment.NewLine);  // 入庫
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IH.IN_OUT_DELIVERY_KBN " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS BEFORE_IN" + Environment.NewLine);
                sb.Append("    ON BEFORE_IN.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND BEFORE_IN.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND BEFORE_IN.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 対象年月以前の出庫

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IH.IN_OUT_DELIVERY_KBN " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,SUM(IND.NUMBER) AS SUM_NUMBER" + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(before_ym.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_KBN = 2" + Environment.NewLine);  // 出庫
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IH.IN_OUT_DELIVERY_KBN " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS BEFORE_OUT" + Environment.NewLine);
                sb.Append("    ON BEFORE_OUT.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND BEFORE_OUT.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND BEFORE_OUT.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 対象年月の仕入入庫

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,SUM(IND.NUMBER) AS SUM_NUMBER" + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_PROC_KBN = 3" + Environment.NewLine);  // 仕入
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS THIS_PURCHASE_IN" + Environment.NewLine);
                sb.Append("    ON THIS_PURCHASE_IN.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND THIS_PURCHASE_IN.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND THIS_PURCHASE_IN.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 対象年月の売上出庫

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,SUM(IND.NUMBER) AS SUM_NUMBER" + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_PROC_KBN = 2" + Environment.NewLine);  // 売上
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS THIS_SALES_OUT" + Environment.NewLine);
                sb.Append("    ON THIS_SALES_OUT.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND THIS_SALES_OUT.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND THIS_SALES_OUT.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 対象年月のその他入庫

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,SUM(IND.NUMBER) AS SUM_NUMBER" + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_PROC_KBN NOT IN (2, 3)" + Environment.NewLine);  // 売上,仕入以外
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_KBN = 1" + Environment.NewLine);  // 入庫
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS THIS_ANOTHER_IN" + Environment.NewLine);
                sb.Append("    ON THIS_ANOTHER_IN.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND THIS_ANOTHER_IN.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND THIS_ANOTHER_IN.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 対象年月のその他出庫

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,SUM(IND.NUMBER) AS SUM_NUMBER" + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_PROC_KBN NOT IN (2, 3)" + Environment.NewLine);  // 売上,仕入以外
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD >= " + ExEscape.zRepStr(this_ym_from.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_YMD <= " + ExEscape.zRepStr(this_ym_to.ToString("yyyy/MM/dd")) + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_KBN = 2" + Environment.NewLine);  // 出庫
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS THIS_ANOTHER_OUT" + Environment.NewLine);
                sb.Append("    ON THIS_ANOTHER_OUT.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND THIS_ANOTHER_OUT.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND THIS_ANOTHER_OUT.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 最終入庫日

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,MAX(IH.IN_OUT_DELIVERY_YMD) AS IN_OUT_DELIVERY_YMD " + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_KBN = 1" + Environment.NewLine);  // 入庫
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS MAX_IN_YMD" + Environment.NewLine);
                sb.Append("    ON MAX_IN_YMD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND MAX_IN_YMD.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND MAX_IN_YMD.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion

                #region 最終出庫日

                sb.Append("  LEFT JOIN (SELECT IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                   ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                   ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("                   ,MAX(IH.IN_OUT_DELIVERY_YMD) AS IN_OUT_DELIVERY_YMD " + Environment.NewLine);
                sb.Append("               FROM T_IN_OUT_DELIVERY_H AS IH" + Environment.NewLine);
                sb.Append("              INNER JOIN T_IN_OUT_DELIVERY_D AS IND" + Environment.NewLine);
                sb.Append("                 ON IND.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IND.COMPANY_ID = IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                AND IND.GROUP_ID = IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                AND IND.IN_OUT_DELIVERY_ID = IH.ID " + Environment.NewLine);
                sb.Append("              WHERE IH.COMPANY_ID = " + companyId + Environment.NewLine);
                sb.Append(GetGroupWhere(groupId, "IH", "           "));    // グループ集計条件
                sb.Append("                AND IH.DELETE_FLG = 0" + Environment.NewLine);
                sb.Append("                AND IH.IN_OUT_DELIVERY_KBN = 2" + Environment.NewLine);  // 出庫
                sb.Append("              GROUP BY IH.COMPANY_ID " + Environment.NewLine);
                sb.Append("                      ,IH.GROUP_ID " + Environment.NewLine);
                sb.Append("                      ,IND.COMMODITY_ID " + Environment.NewLine);
                sb.Append("             ) AS MAX_OUT_YMD" + Environment.NewLine);
                sb.Append("    ON MAX_OUT_YMD.COMPANY_ID = T.COMPANY_ID " + Environment.NewLine);
                sb.Append("   AND MAX_OUT_YMD.GROUP_ID = CI.GROUP_ID" + Environment.NewLine);
                sb.Append("   AND MAX_OUT_YMD.COMMODITY_ID = T.ID" + Environment.NewLine);

                #endregion
            }

            // 会社
            sb.Append("  LEFT JOIN SYS_M_COMPANY AS CP" + Environment.NewLine);
            sb.Append("    ON CP.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CP.ID = T.COMPANY_ID" + Environment.NewLine);

            // 会社グループ
            sb.Append("  LEFT JOIN SYS_M_COMPANY_GROUP AS CG" + Environment.NewLine);
            sb.Append("    ON CG.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND CG.COMPANY_ID = T.COMPANY_ID" + Environment.NewLine);
            sb.Append("   AND CG.ID = CI.GROUP_ID" + Environment.NewLine);

            // 単位
            sb.Append("  LEFT JOIN SYS_M_NAME AS NM1" + Environment.NewLine);
            sb.Append("    ON NM1.DELETE_FLG = 0 " + Environment.NewLine);
            sb.Append("   AND NM1.DISPLAY_FLG = 1 " + Environment.NewLine);
            sb.Append("   AND NM1.DIVISION_ID = " + (int)CommonUtl.geNameKbn.UNIT_ID + Environment.NewLine);
            sb.Append("   AND NM1.ID = T.UNIT_ID" + Environment.NewLine);

            #endregion

            sb.Append(" WHERE T.COMPANY_ID = " + companyId + Environment.NewLine);
            sb.Append("   AND T.DELETE_FLG = 0 " + Environment.NewLine);

            // 抽出条件
            if (strWhereSql != "")
            {
                sb.Append(strWhereSql + Environment.NewLine);
            }

            sb.Append(" ORDER BY T.COMPANY_ID " + Environment.NewLine);
            sb.Append("         ,CG.ID " + Environment.NewLine);
            sb.Append("         ,T.ID2 " + Environment.NewLine);
            sb.Append("         ,T.ID " + Environment.NewLine);
            if (strOrderBySql != "")
            {
                sb.Append(strOrderBySql + Environment.NewLine);
            }
            //sb.Append(" LIMIT 0, 1000");

            #endregion

            CommonUtl.ExLogger.Info(sb.ToString());

            return sb.ToString();

        }

        #endregion

        #endregion

        #region Method

        private string GetTitleNm(string nm, string title)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'得意先別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 3:
                        str_ret = "      ,'商品別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTitleNm2(string nm, string title)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'請求先別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 3:
                        str_ret = "      ,'商品別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTitleNm3(string nm, string title)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'仕入先別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 3:
                        str_ret = "      ,'商品別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTitleNm4(string nm, string title)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'仕入先別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + title + "' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTitleNm2(string nm)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'請求先別" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTitleNm3(string nm)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'仕入先別" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTitleNm4(string nm)
        {
            string str_ret = "";

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 1:
                        str_ret = "      ,'仕入先別" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    case 2:
                        str_ret = "      ,'担当別" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                    default:
                        str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
                        break;
                }
            }
            else
            {
                str_ret = "      ,'" + nm + "明細表' AS TITEL_NM " + Environment.NewLine;
            }

            str_ret += "      ,'" + ExCast.zCStr(HttpContext.Current.Session[ExSession.GROUP_DISPLAY_NAME]) + "計' AS GROUP_SUM " + Environment.NewLine;

            return str_ret;
        }

        private string GetTotalKbn()
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 1:
                        sb.Append("      ,CONCAT('【得意先 ',CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("                               ELSE T.CUSTOMER_ID END, '：', CS.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'得意先計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CONCAT('【担当 ',T.PERSON_ID, '：', PS.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'担当計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                }
            }
            else
            {
                sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
            }

            return sb.ToString();
        }

        private string GetTotalKbn2()
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 1:
                        sb.Append("      ,CONCAT('【請求先 ',CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("                               ELSE T.INVOICE_ID END, '：', CS.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'請求先計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CONCAT('【担当 ',T.PERSON_ID, '：', PS2.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'担当計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                }
            }
            else
            {
                sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
            }

            return sb.ToString();
        }

        private string GetTotalKbn3()
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 1:
                        sb.Append("      ,CONCAT('【仕入先 ',CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("                               ELSE T.PURCHASE_ID END, '：', PU.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'仕入先計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CONCAT('【担当 ',T.PERSON_ID, '：', PS.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'担当計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                }
            }
            else
            {
                sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
            }

            return sb.ToString();
        }

        private string GetTotalKbn4()
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 1:
                        sb.Append("      ,CONCAT('【仕入先 ',CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("                               ELSE T.PURCHASE_ID END, '：', CS.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'仕入先計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CONCAT('【担当 ',T.PERSON_ID, '：', PS.NAME, '】')" + " AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'担当計' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,'' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                }
            }
            else
            {
                sb.Append("      ,'' AS TOTAL_KBN " + Environment.NewLine);
            }

            return sb.ToString();
        }

        #region 日報・月報・推移表用

        // 売上用
        private string GetTotalKbnDDMM(int kbn)
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else if (kbn == 2)
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 推移表
                            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_Y + "), '0000', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年度順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                    case 1:
                            sb.Append("      ,CASE WHEN IFNULL(T.CUSTOMER_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.CUSTOMER_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                            sb.Append("            ELSE T.CUSTOMER_ID END AS TOTAL_KBN " + Environment.NewLine);
                            sb.Append("      ,CS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'得意先順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CASE WHEN IFNULL(T.PERSON_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PERSON_ID, 3, '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.PERSON_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,PS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'担当順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 3:
                        sb.Append("      ,CASE WHEN IFNULL(CD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("            ELSE CD.COMMODITY_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,CD.COMMODITY_NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'商品順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else if (kbn == 2)
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 推移表
                            sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_Y + "), '0000', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年度順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                if (kbn == 1)
                {
                    // 日報時
                    sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
                else if (kbn == 2)
                {
                    // 月報時
                    sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
                else
                {
                    // 推移表
                    sb.Append("      ,replace(date_format(T.SALES_YMD , " + ExEscape.SQL_Y + "), '0000', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'年度順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        // 入金用
        private string GetTotalKbnDDMM2(int kbn)
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                    case 1:
                        sb.Append("      ,CASE WHEN IFNULL(T.INVOICE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.INVOICE_ID, " + this.idFigureCustomer.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.INVOICE_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,CS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'請求先順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CASE WHEN IFNULL(T.PERSON_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PERSON_ID, 3, '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.PERSON_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,PS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'担当順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                if (kbn == 1)
                {
                    // 日報時
                    sb.Append("      ,replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
                else
                {
                    // 月報時
                    sb.Append("      ,replace(date_format(T.RECEIPT_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        // 仕入用
        private string GetTotalKbnDDMM3(int kbn)
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else if (kbn == 2)
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 推移表時
                            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + "), '0000', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                    case 1:
                        sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.PURCHASE_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,CS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'仕入先順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CASE WHEN IFNULL(T.PERSON_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PERSON_ID, 3, '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.PERSON_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,PS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'担当順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 3:
                        sb.Append("      ,CASE WHEN IFNULL(CD.COMMODITY_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(CD.COMMODITY_ID, " + this.idFigureCommodity.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("            ELSE CD.COMMODITY_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,CD.COMMODITY_NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'商品順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else if (kbn == 2)
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 推移表時
                            sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + "), '0000', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                if (kbn == 1)
                {
                    // 日報時
                    sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
                else if (kbn == 2)
                {
                    // 月報時
                    sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
                else
                {
                    // 推移表時
                    sb.Append("      ,replace(date_format(T.PURCHASE_YMD , " + ExEscape.SQL_Y + "), '0000', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'年順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        // 出金用
        private string GetTotalKbnDDMM4(int kbn)
        {
            string str_ret = "";
            StringBuilder sb = new StringBuilder();

            if (this.entitySetting != null)
            {
                switch (this.entitySetting.total_kbn)
                {
                    case 0:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                    case 1:
                        sb.Append("      ,CASE WHEN IFNULL(T.PURCHASE_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PURCHASE_ID, " + this.idFigurePurchase.ToString() + ", '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.PURCHASE_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,CS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'請求先順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    case 2:
                        sb.Append("      ,CASE WHEN IFNULL(T.PERSON_ID, '') REGEXP '^-?[0-9]+$' THEN lpad(T.PERSON_ID, 3, '0') " + Environment.NewLine);
                        sb.Append("            ELSE T.PERSON_ID END AS TOTAL_KBN " + Environment.NewLine);
                        sb.Append("      ,PS.NAME AS TOTAL_KBN_NM " + Environment.NewLine);
                        sb.Append("      ,'担当順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        break;
                    default:
                        if (kbn == 1)
                        {
                            // 日報時
                            sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        else
                        {
                            // 月報時
                            sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                            sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                            sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                        }
                        break;
                }
            }
            else
            {
                if (kbn == 1)
                {
                    // 日報時
                    sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YMD + "), '0000/00/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'日付順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
                else
                {
                    // 月報時
                    sb.Append("      ,replace(date_format(T.PAYMENT_CASH_YMD , " + ExEscape.SQL_YM + "), '0000/00', '') AS TOTAL_KBN" + Environment.NewLine);
                    sb.Append("      ,'' AS TOTAL_KBN_NM " + Environment.NewLine);
                    sb.Append("      ,'年月順' AS TOTAL_KBN_SUM_NM " + Environment.NewLine);
                }
            }

            return sb.ToString();
        }

        #endregion

        private string GetGroupWhere(string groupId, string tblAnotherName)
        {
            StringBuilder sb = new StringBuilder();

            // グループ集計条件
            if (this.entitySetting != null)
            {
                if (this.entitySetting.group_id_from == entitySetting.group_id_to)
                {
                    sb.Append("   AND " + tblAnotherName + ".GROUP_ID = " + ExCast.zCLng(this.entitySetting.group_id_from) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND " + tblAnotherName + ".GROUP_ID >= " + ExCast.zCLng(this.entitySetting.group_id_from) + Environment.NewLine);
                    sb.Append("   AND " + tblAnotherName + ".GROUP_ID <= " + ExCast.zCLng(this.entitySetting.group_id_to) + Environment.NewLine);
                }
            }
            else
            {
                sb.Append("   AND " + tblAnotherName + ".GROUP_ID = " + groupId + Environment.NewLine);
            }

            return sb.ToString();

        }

        private string GetGroupWhere(string groupId, string tblAnotherName, string strSpace)
        {
            StringBuilder sb = new StringBuilder();

            // グループ集計条件
            if (this.entitySetting != null)
            {
                if (this.entitySetting.group_id_from == entitySetting.group_id_to)
                {
                    sb.Append(strSpace + "   AND " + tblAnotherName + ".GROUP_ID = " + ExCast.zCLng(this.entitySetting.group_id_from) + Environment.NewLine);
                }
                else
                {
                    sb.Append(strSpace + "   AND " + tblAnotherName + ".GROUP_ID >= " + ExCast.zCLng(this.entitySetting.group_id_from) + Environment.NewLine);
                    sb.Append(strSpace + "   AND " + tblAnotherName + ".GROUP_ID <= " + ExCast.zCLng(this.entitySetting.group_id_to) + Environment.NewLine);
                }
            }
            else
            {
                sb.Append(strSpace + "   AND " + tblAnotherName + ".GROUP_ID = " + groupId + Environment.NewLine);
            }

            return sb.ToString();

        }

        private string GetGroupWhereID(string groupId, string tblAnotherName)
        {
            StringBuilder sb = new StringBuilder();

            // グループ集計条件
            if (this.entitySetting != null)
            {
                if (this.entitySetting.group_id_from == entitySetting.group_id_to)
                {
                    sb.Append("   AND " + tblAnotherName + ".ID = " + ExCast.zCLng(this.entitySetting.group_id_from) + Environment.NewLine);
                }
                else
                {
                    sb.Append("   AND " + tblAnotherName + ".ID >= " + ExCast.zCLng(this.entitySetting.group_id_from) + Environment.NewLine);
                    sb.Append("   AND " + tblAnotherName + ".ID <= " + ExCast.zCLng(this.entitySetting.group_id_to) + Environment.NewLine);
                }
            }
            else
            {
                sb.Append("   AND " + tblAnotherName + ".ID = " + groupId + Environment.NewLine);
            }

            return sb.ToString();

        }

        private void GetStringWhere(string _strWhereSql, ref string strWhereSql, ref string stringWhere2, ref string stringWhere1)
        {
            string stringWhere = "";
            string[] array_stringWhere;
            if (_strWhereSql.IndexOf("WhereString =>") != -1)
            {
                if (_strWhereSql.IndexOf("WhereString =>") == 0)
                {
                    strWhereSql = "";
                    stringWhere = _strWhereSql.Substring(14);
                    array_stringWhere = stringWhere.Split(';');
                    stringWhere1 = array_stringWhere[0];
                    if (array_stringWhere.Length > 1) stringWhere2 = array_stringWhere[1];
                }
                else
                {
                    strWhereSql = _strWhereSql.Substring(0, _strWhereSql.IndexOf("WhereString =>"));
                    stringWhere = _strWhereSql.Substring(_strWhereSql.IndexOf("WhereString =>")+ 14);
                    array_stringWhere = stringWhere.Split(';');
                    stringWhere1 = array_stringWhere[0];
                    if (array_stringWhere.Length > 1) stringWhere2 = array_stringWhere[1];
                }
            }
            else
            {
                strWhereSql = _strWhereSql;
            }
            strWhereSql = strWhereSql.Replace("<<@escape_comma@>>", ",");
            strWhereSql = strWhereSql.Replace("<<@escape_single_quotation@>>", "'");
        }

        private DataSet SetDefualtRecord(DataSet ds)
        {
            // row[0] → No
            // グループフッター用
            // row[1] → 御見積(注文)合計金額
            // row[2] → 摘要
            // row[3] → 税抜金額計
            // row[4] → 税額計
            // row[5] → 税転換
            string _before_no = "";
            string _before_memo = "";
            double _before_sum_no_tax_price = 0;
            double _before_sum_tax = 0;
            int _tax_change_id = 0;
            int _cnt = 0;
            int _index = 0;
            int _add_cnt = 0;

            try
            {
                DataTable dt = ds.Tables[0];
                DataTable new_dt = dt.Clone();
                DataRow _new_row = dt.NewRow();
                DataRow row = null;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    row = dt.Rows[i];

                    string _no = ExCast.zCStr(row[0]);
                    if (string.IsNullOrEmpty(_before_no))
                    {
                        _before_no = _no;
                        _before_memo = ExCast.zCStr(row[2]);
                        _before_sum_no_tax_price = ExCast.zCDbl(row[3]);
                        _before_sum_tax = ExCast.zCDbl(row[4]);
                        _tax_change_id = ExCast.zCInt(row[5]);
                    }

                    if (!_before_no.Equals(_no))
                    {
                        _add_cnt = 0;
                        if (19 <= _cnt)
                        {
                            _add_cnt = _cnt % 19;
                        }
                        else
                        {
                            _add_cnt = 19 - _cnt;
                        }
                        _new_row[0] = _before_no;

                        for (int _i = 1; _i <= _add_cnt; _i++)
                        {
                            // 御見積合計金額クリア
                            _new_row[1] = "";

                            // グループフッター用
                            _new_row[2] = _before_memo;
                            _new_row[3] = _before_sum_no_tax_price;
                            _new_row[4] = _before_sum_tax;
                            _new_row[5] = _tax_change_id;

                            // 追加
                            new_dt.Rows.Add(_new_row.ItemArray);
                            _index += 1;
                        }
                        _cnt = 1;
                        _before_no = _no;
                        _before_memo = ExCast.zCStr(row[2]);
                        _before_sum_no_tax_price = ExCast.zCDbl(row[3]);
                        _before_sum_tax = ExCast.zCDbl(row[4]);
                        _tax_change_id = ExCast.zCInt(row[5]);
                    }
                    else
                    {
                        if (_cnt == 19)
                        {
                            row[1] = "";
                            _cnt = 0;
                        }
                        _cnt += 1;
                    }
                    _index += 1;

                    if (_cnt != 1)
                    {
                        // 御見積合計金額クリア
                        row[1] = "";
                    }

                    // 追加
                    new_dt.Rows.Add(row.ItemArray);
                }

                _add_cnt = 0;
                _add_cnt = 19 - _cnt;
                _new_row[0] = row[0];
                for (int _i = 1; _i <= _add_cnt; _i++)
                {
                    // 御見積合計金額クリア
                    _new_row[1] = "";

                    // グループフッター用
                    _new_row[2] = row[2];
                    _new_row[3] = row[3];
                    _new_row[4] = row[4];
                    _new_row[5] = row[5];

                    // 追加
                    new_dt.Rows.Add(_new_row.ItemArray);
                    _index += 1;
                }

                DataSet _ds = ds.Clone();
                _ds.Tables.Clear();
                _ds.Tables.Add(new_dt);
                return _ds;
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".SerDefualtRecord", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".SerDefualtRecord" + Environment.NewLine + ex.Message;
                throw;
            }
        }

        private void SetRecordNoRecord(ref DataSet ds, int index, int group_no_index)
        {
            string before_no = "";
            int cnt = 1;
            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(before_no))
                    {
                        before_no = ExCast.zCStr(ds.Tables[0].Rows[i][group_no_index]);
                    }

                    if (before_no != ExCast.zCStr(ds.Tables[0].Rows[i][group_no_index]))
                    {
                        cnt = 1;
                    }

                    ds.Tables[0].Rows[i][index] = cnt;

                    before_no = ExCast.zCStr(ds.Tables[0].Rows[i][group_no_index]);
                    cnt += 1;
                }
            }
            catch (Exception ex)
            {
                CommonUtl.ExLogger.Error(CLASS_NM + ".SetRecordNoRecord", ex);
                CommonUtl.gstrErrMsg = CLASS_NM + ".SetRecordNoRecord" + Environment.NewLine + ex.Message;
                throw;
            }
        }

        #endregion

    }
}