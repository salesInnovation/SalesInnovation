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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SlvHanbaiClient.svcSysName;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.View.UserControl.Custom;

namespace SlvHanbaiClient.Class.Data
{
    public class MstData
    {
        public static List<MstNameData> _listData = new List<MstNameData>();
        //private static ExWebServiceMst webService = new ExWebServiceMst();
        private static ExUserControl _page = null;
        public static ExWebService.geDialogDisplayFlg dialogDisplayFlg = ExWebService.geDialogDisplayFlg.No;
        public static ExWebService.geDialogCloseFlg dialogCloseFlg = ExWebService.geDialogCloseFlg.No;

        public enum geMDataKbn
        {
            None = 0,
            Company,                    // 会社
            CompanyGroup,               // 会社グループ
            CompanyGroup_F,             // 会社グループ
            CompanyGroup_T,             // 会社グループ
            SalesBalance,               // 売掛残高
            PaymentBalance,             // 買掛残高
            User,                       // ユーザ
            Zip,                        // 郵便番号
            Invoice,                    // 請求先
            Invoice_F,                  // 請求先
            Invoice_T,                  // 請求先
            Customer,                   // 得意先
            Customer_F,                 // 得意先
            Customer_T,                 // 得意先
            Supplier,                   // 納入先
            Supplier_F,                 // 納入先
            Supplier_T,                 // 納入先
            Purchase,                   // 仕入先
            Purchase_F,                 // 仕入先
            Purchase_T,                 // 仕入先
            Person,                     // 担当
            Person_F,                   // 担当
            Person_T,                   // 担当
            Inventory,                  // 在庫
            Commodity,                  // 商品
            Commodity_F,                // 商品
            Commodity_T,                // 商品
            Condition,                  // 締区分
            Condition_F,                // 締区分
            Condition_T,                // 締区分
            RecieptDivision,            // 入金区分
            PaymentCahsDivision,        // 出金区分
            Group,                      // 分類
            Group_F,                    // 分類
            Group_T                     // 分類
        };

        public enum geMGroupKbn
        {
            None = 0,
            CustomerGrouop1,            // 得意先分類1
            CustomerGrouop2,            // 得意先分類2
            CustomerGrouop3,            // 得意先分類2
            CommodityGrouop1,           // 商品分類1
            CommodityGrouop2,           // 商品分類2
            CommodityGrouop3,           // 商品分類2
            PurchaseGrouop1,            // 仕入先分類1
            PurchaseGrouop2,            // 仕入先分類2
            PurchaseGrouop3             // 仕入先分類3
        };

        // マスタ名称取得
        public void GetMData(geMDataKbn mstKbn, string[] id, ExUserControl page)
        {
            ExWebServiceMst webService = new ExWebServiceMst();

            object[] prm;
            _page = page;

            switch (mstKbn)
            {
                #region 得意先

                case geMDataKbn.Customer:               // 得意先
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Customer_F:               // 得意先
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Customer_T:               // 得意先
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 納入先

                case geMDataKbn.Supplier:               // 納入先
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplier,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Supplier_F:               // 納入先
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplier_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Supplier_T:               // 納入先
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplier_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 担当

                case geMDataKbn.Person:                 // 担当
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPerson,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Person_F:                 // 担当
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPerson_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Person_T:                 // 担当
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPerson_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 商品

                case geMDataKbn.Commodity:               // 商品
                    prm = new object[2];
                    prm[0] = id[0];
                    if (id.Length > 1) prm[1] = id[1];
                    else prm[1] = "";
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity,
                                                 MstData.dialogDisplayFlg,
                                                 MstData.dialogCloseFlg,
                                                 prm);
                    MstData.dialogDisplayFlg = ExWebService.geDialogDisplayFlg.No;
                    MstData.dialogCloseFlg = ExWebService.geDialogCloseFlg.No;
                    break;
                case geMDataKbn.Commodity_F:               // 商品
                    prm = new object[2];
                    prm[0] = id[0];
                    if (id.Length > 1) prm[1] = id[1];
                    else prm[1] = "";
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Commodity_T:               // 商品
                    prm = new object[2];
                    prm[0] = id[0];
                    if (id.Length > 1) prm[1] = id[1];
                    else prm[1] = "";
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 会社グループ

                case geMDataKbn.CompanyGroup:           // 会社グループ
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroup,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.CompanyGroup_F:           // 会社グループ
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroup_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.CompanyGroup_T:           // 会社グループ
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroup_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 郵便番号

                case geMDataKbn.Zip:                    // 郵便番号
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetZip,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 締区分

                case geMDataKbn.Condition:              // 締区分
                    prm = new object[2];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCondition,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Condition_F:              // 締区分
                    prm = new object[2];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCondition_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Condition_T:              // 締区分
                    prm = new object[2];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetCondition_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 入金区分

                case geMDataKbn.RecieptDivision:        // 入金区分
                    prm = new object[2];
                    prm[0] = id[0];
                    if (id.Length > 1) prm[1] = id[1];
                    else prm[1] = "";
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetRecieptDivision,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 分類

                case geMDataKbn.Group:                  // 分類
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroup,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                case geMDataKbn.Group_F:                  // 分類
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroup_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                case geMDataKbn.Group_T:                  // 分類
                    prm = new object[2];
                    prm[0] = id[0];
                    prm[1] = id[1];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroup_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                #region 仕入先

                case geMDataKbn.Purchase:               // 仕入先
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchase,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Purchase_F:               // 仕入先
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchase_F,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;
                case geMDataKbn.Purchase_T:               // 仕入先
                    prm = new object[1];
                    prm[0] = id[0];
                    webService.objPerent = page;
                    webService.CallWebServiceMst(ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchase_T,
                                                 ExWebService.geDialogDisplayFlg.No,
                                                 ExWebService.geDialogCloseFlg.No,
                                                 prm);
                    break;

                #endregion

                default:
                    break;
            }

            if (page is Utl_MstText)
            {
                Utl_MstText utl = (Utl_MstText)page;
                utl.Is_Call_MstID_Changed = true;
            }
        }
    }

    public class MstNameData
    {
        private int id;
        public int ID { set { this.id = value; } get { return this.id; } }
        private string name;
        public string NAME { set { this.name = value; } get { return this.name; } }

        public MstNameData(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
