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
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.Utility;

namespace SlvHanbaiClient.Class.WebService
{
    // マスタ名称取得用Webサービスクラス(MstNameListから呼出)
    public class ExWebServiceMst : ExWebService
    {
        #region Filed Enum Const

        private const String CLASS_NM = "ExWebServiceMst";

        // Webサービス呼出区分
        public enum geWebServiceMstNmCallKbn
        {
            GetCustomer = 0,
            GetCustomer_F,
            GetCustomer_T,
            GetSupplier,
            GetSupplier_F,
            GetSupplier_T,
            GetPerson,
            GetPerson_F,
            GetPerson_T,
            GetCommodity,
            GetCommodity_F,
            GetCommodity_T,
            GetCompanyGroup,
            GetCompanyGroup_F,
            GetCompanyGroup_T,
            GetZip,
            GetCondition,
            GetCondition_F,
            GetCondition_T,
            GetRecieptDivision,
            GetGroup,
            GetGroup_F,
            GetGroup_T,
            GetUser,
            GetPurchase,
            GetPurchase_F,
            GetPurchase_T,
            GetCustomerList,
            GetSupplierList,
            GetPersonList,
            GetCommodityList,
            GetZipList,
            GetCompanyGroupList,
            GetConditionList,
            GetRecieptDivisionList,
            GetGroupList,
            GetUserList,
            GetPurchaseList,
            GetInventoryList,
            GetSalesBalanceList,
            GetPaymentBalanceList
        }
        public static geWebServiceMstNmCallKbn gWebServiceMstNmCallKbn;

        private geWebServiceMstNmCallKbn WebServiceMstNmCallKbn;

        private MstData.geMGroupKbn _MstGroupKbn = MstData.geMGroupKbn.None;
        public MstData.geMGroupKbn MstGroupKbn
        {
            set
            {
                this._MstGroupKbn = value;
            }
            get
            {
                return this._MstGroupKbn;
            }
        }

        // 取得データ
        private EntityMstData objMstName;　　       // マスタ名
        private ObservableCollection<EntityMstList> objMstList;　　       // マスタ一覧
        
        #endregion

        #region Method

        #region Web Service Call

        // WebサービスCall(データ参照時)
        public void CallWebServiceMst(geWebServiceMstNmCallKbn callKbn, 
                                      geDialogDisplayFlg dialogDisplayFlg,
                                      geDialogCloseFlg dialogCloseFlg,
                                      object[] prm)
        {
            try
            {
                if (Common.gblnAppStart == false)
                {
                    return;
                }

                gWebServiceMstNmCallKbn = callKbn;
                WebServiceMstNmCallKbn = callKbn;

                // Web Service Call
                switch (callKbn)
                {
                    case geWebServiceMstNmCallKbn.GetCustomer:
                    case geWebServiceMstNmCallKbn.GetCustomer_F:
                    case geWebServiceMstNmCallKbn.GetCustomer_T:
                        GetCustomer(ExCast.zCStr(prm[0]));
                        break;
                    case geWebServiceMstNmCallKbn.GetSupplier:
                        GetSupplier(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetPerson:
                    case geWebServiceMstNmCallKbn.GetPerson_F:
                    case geWebServiceMstNmCallKbn.GetPerson_T:
                        GetPerson(ExCast.zCStr(prm[0]));
                        break;
                    case geWebServiceMstNmCallKbn.GetCommodity:
                    case geWebServiceMstNmCallKbn.GetCommodity_F:
                    case geWebServiceMstNmCallKbn.GetCommodity_T:
                        GetCommodity(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetCompanyGroup:
                    case geWebServiceMstNmCallKbn.GetCompanyGroup_F:
                    case geWebServiceMstNmCallKbn.GetCompanyGroup_T:
                        GetCompanyGroup(ExCast.zCStr(prm[0]));
                        break;
                    case geWebServiceMstNmCallKbn.GetZip:
                        GetZip(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetCondition:
                    case geWebServiceMstNmCallKbn.GetCondition_F:
                    case geWebServiceMstNmCallKbn.GetCondition_T:
                        GetCondition(1, ExCast.zCStr(prm[0]));
                        break;
                    case geWebServiceMstNmCallKbn.GetRecieptDivision:
                        GetReceitpDivision(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetGroup:
                    case geWebServiceMstNmCallKbn.GetGroup_F:
                    case geWebServiceMstNmCallKbn.GetGroup_T:
                        GetGroup(ExCast.zCInt(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetPurchase:
                    case geWebServiceMstNmCallKbn.GetPurchase_F:
                    case geWebServiceMstNmCallKbn.GetPurchase_T:
                        GetPurchase(ExCast.zCStr(prm[0]));
                        break;
                    case geWebServiceMstNmCallKbn.GetCustomerList:
                        GetCustomerList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]), ExCast.zCStr(prm[3]));
                        break;
                    case geWebServiceMstNmCallKbn.GetSupplierList:
                        GetSupplierList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]), ExCast.zCStr(prm[3]));
                        break;
                    case geWebServiceMstNmCallKbn.GetPersonList:
                        GetPersonList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]));
                        break;
                    case geWebServiceMstNmCallKbn.GetCommodityList:
                        GetCommodityList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]), ExCast.zCStr(prm[3]));
                        break;
                    case geWebServiceMstNmCallKbn.GetCompanyGroupList:
                        GetCompanyGroupList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]));
                        break;
                    case geWebServiceMstNmCallKbn.GetZipList:
                        GetZipList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetConditionList:
                        GetConditionList(1, ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]));
                        break;
                    case geWebServiceMstNmCallKbn.GetRecieptDivisionList:
                        GetReceitpDivisionList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]));
                        break;
                    case geWebServiceMstNmCallKbn.GetGroupList:
                        GetGroupList((int)this.MstGroupKbn, ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]));
                        break;
                    case geWebServiceMstNmCallKbn.GetPurchaseList:
                        GetPurchaseList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]), ExCast.zCStr(prm[3]));
                        break;
                    case geWebServiceMstNmCallKbn.GetUserList:
                        GetUserList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]), ExCast.zCStr(prm[2]));
                        break;
                    case geWebServiceMstNmCallKbn.GetInventoryList:
                        GetInventoryList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetSalesBalanceList:
                        GetSalesBalanceList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    case geWebServiceMstNmCallKbn.GetPaymentBalanceList:
                        GetPaymentBalanceList(ExCast.zCStr(prm[0]), ExCast.zCStr(prm[1]));
                        break;
                    default:
                        break;
                }

                // 処理中ダイアログ表示
                if (dialogDisplayFlg == geDialogDisplayFlg.Yes)
                {
                    win = new Dlg_Progress();
                    win.Show();
                }
                
            }
            catch (Exception ex)
            {
                ExMessageBox.Show(CLASS_NM + ".CallWebService" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        #endregion

        #region 得意先取得

        private void GetCustomer(string CustomerID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetCustomerCompleted += new EventHandler<GetCustomerCompletedEventArgs>(this.GetCustomerCompleted);
                svc.GetCustomerAsync(Common.gstrSessionString, CustomerID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCustomer" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCustomerCompleted(Object sender, GetCustomerCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 納入先取得

        private void GetSupplier(string ID, string CustomerID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetSupplierCompleted += new EventHandler<GetSupplierCompletedEventArgs>(this.GetSupplierCompleted);
                svc.GetSupplierAsync(Common.gstrSessionString, CustomerID, ID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetSupplier" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSupplierCompleted(Object sender, GetSupplierCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    { 
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 担当取得

        private void GetPerson(string ID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetPersonCompleted += new EventHandler<GetPersonCompletedEventArgs>(this.GetPersonCompleted);
                svc.GetPersonAsync(Common.gstrSessionString, ID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPerson" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPersonCompleted(Object sender, GetPersonCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 商品取得

        private void GetCommodity(string ID, string dataGirdSelectedIndex)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetCommodityCompleted += new EventHandler<GetCommodityCompletedEventArgs>(this.GetCommodityCompleted);
                svc.GetCommodityAsync(Common.gstrSessionString, ID, dataGirdSelectedIndex);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCommodity" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCommodityCompleted(Object sender, GetCommodityCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 会社グループ取得

        private void GetCompanyGroup(string ID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetCompanyGroupCompleted += new EventHandler<GetCompanyGroupCompletedEventArgs>(this.GetCompanyGroupCompleted);
                svc.GetCompanyGroupAsync(Common.gstrSessionString, ID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCompanyGroup" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCompanyGroupCompleted(Object sender, GetCompanyGroupCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 郵便番号取得

        private void GetZip(string zipFrom, string zipTo)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetZipCompleted += new EventHandler<GetZipCompletedEventArgs>(this.GetZipCompleted);
                svc.GetZipAsync(Common.gstrSessionString, zipFrom, zipTo);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetZip" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetZipCompleted(Object sender, GetZipCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(geWebServiceMstNmCallKbn.GetZip, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(geWebServiceMstNmCallKbn.GetZip, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(geWebServiceMstNmCallKbn.GetZip, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetZipCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 条件（支払・回収）取得

        private void GetCondition(int kbn, string Id)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetConditionCompleted += new EventHandler<GetConditionCompletedEventArgs>(this.GetConditionCompleted);
                svc.GetConditionAsync(Common.gstrSessionString, kbn, Id);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCondition" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetConditionCompleted(Object sender, GetConditionCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 入金区分取得

        private void GetReceitpDivision(string Id, string dataGirdSelectedIndex)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetReceitpDivisionCompleted += new EventHandler<GetReceitpDivisionCompletedEventArgs>(this.GetReceitpDivisionCompleted);
                svc.GetReceitpDivisionAsync(Common.gstrSessionString, Id, dataGirdSelectedIndex);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetReceitpDivision" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetReceitpDivisionCompleted(Object sender, GetReceitpDivisionCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(geWebServiceMstNmCallKbn.GetRecieptDivision, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(geWebServiceMstNmCallKbn.GetRecieptDivision, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(geWebServiceMstNmCallKbn.GetRecieptDivision, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetReceitpDivisionCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 分類取得

        private void GetGroup(int kbn, string Id)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetGroupCompleted += new EventHandler<GetGroupCompletedEventArgs>(this.GetGroupCompleted);
                svc.GetGroupAsync(Common.gstrSessionString, kbn, Id);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetGroup" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetGroupCompleted(Object sender, GetGroupCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetGroupCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 仕入先取得

        private void GetPurchase(string PurchaseID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetPurchaseCompleted += new EventHandler<GetPurchaseCompletedEventArgs>(this.GetPurchaseCompleted);
                svc.GetPurchaseAsync(Common.gstrSessionString, PurchaseID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPurchase" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseCompleted(Object sender, GetPurchaseCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstName = e.Result;
                if (objMstName != null)
                {
                    if (objMstName.message != "" && objMstName.message != null)
                    {
                        // 認証失敗
                        ExMessageBox.Show(objMstName.message);
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                    }
                    else
                    {
                        // 認証成功
                        objPerent.MstDataSelect(WebServiceMstNmCallKbn, objMstName);
                    }
                }
                else
                {
                    objPerent.MstDataSelect(WebServiceMstNmCallKbn, null);
                }
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 得意先一覧取得

        private void GetCustomerList(string ID, string Name, string Kana, string GroupID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetCustomerListCompleted += new EventHandler<GetCustomerListCompletedEventArgs>(this.GetCustomerListCompleted);
                svc.GetCustomerListAsync(Common.gstrSessionString, ID, Name, Kana, GroupID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCustomerList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCustomerListCompleted(Object sender, GetCustomerListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCustomerList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCustomerList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCustomerList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCustomerList, objMstList);
                    return;
                }
                else 
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCustomerList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCustomerListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 納入先一覧取得

        private void GetSupplierList(string ID, string Name, string Kana, string CustomerID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetSupplierListCompleted += new EventHandler<GetSupplierListCompletedEventArgs>(this.GetSupplierListCompleted);
                svc.GetSupplierListAsync(Common.gstrSessionString, CustomerID, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetSupplierList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSupplierListCompleted(Object sender, GetSupplierListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSupplierList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSupplierList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSupplierList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSupplierList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSupplierList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetSupplierListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 担当一覧取得

        private void GetPersonList(string ID, string Name, string Kana)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetPersonListCompleted += new EventHandler<GetPersonListCompletedEventArgs>(this.GetPersonListCompleted);
                svc.GetPersonListAsync(Common.gstrSessionString, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPersonList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPersonListCompleted(Object sender, GetPersonListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPersonList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPersonList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPersonList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPersonList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPersonList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPersonListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 商品一覧取得

        private void GetCommodityList(string ID, string Name, string Kana, string GroupID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetCommodityListCompleted += new EventHandler<GetCommodityListCompletedEventArgs>(this.GetCommodityListCompleted);
                svc.GetCommodityListAsync(Common.gstrSessionString, ID, Name, Kana, GroupID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCommodityList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCommodityListCompleted(Object sender, GetCommodityListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCommodityList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCommodityList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCommodityList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCommodityList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCommodityList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCommodityListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 会社グループ一覧取得

        private void GetCompanyGroupList(string ID, string Name, string Kana)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetCompanyGroupListCompleted += new EventHandler<GetCompanyGroupListCompletedEventArgs>(this.GetCompanyGroupListCompleted);
                svc.GetCompanyGroupListAsync(Common.gstrSessionString, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCompanyGroupList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetCompanyGroupListCompleted(Object sender, GetCompanyGroupListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCompanyGroupList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCompanyGroupList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCompanyGroupList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCompanyGroupList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCompanyGroupList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetCompanyGroupListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 郵便番号一覧取得

        private void GetZipList(string zipFrom, string zipTo)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetZipListCompleted += new EventHandler<GetZipListCompletedEventArgs>(this.GetZipListCompleted);
                svc.GetZipListAsync(Common.gstrSessionString, zipFrom, zipTo);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetZipList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetZipListCompleted(Object sender, GetZipListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetZipList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetZipList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetZipList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetZipList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetZipList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetZipListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 条件（支払・回収）一覧取得

        private void GetConditionList(int kbn, string ID, string Name, string Kana)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetConditionListCompleted += new EventHandler<GetConditionListCompletedEventArgs>(this.GetConditionListCompleted);
                svc.GetConditionListAsync(Common.gstrSessionString, kbn, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetConditionList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetConditionListCompleted(Object sender, GetConditionListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCondition, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetCondition, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetConditionList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetConditionList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetConditionList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetConditionListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 郵便番号一覧取得

        private void GetReceitpDivisionList(string ID, string Name, string Kana)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetReceitpDivisionListCompleted += new EventHandler<GetReceitpDivisionListCompletedEventArgs>(this.GetReceitpDivisionListCompleted);
                svc.GetReceitpDivisionListAsync(Common.gstrSessionString, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetReceitpDivisionList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetReceitpDivisionListCompleted(Object sender, GetReceitpDivisionListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetRecieptDivisionList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetRecieptDivisionList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetRecieptDivisionList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetRecieptDivisionList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetRecieptDivisionList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetReceitpDivisionListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 分類一覧取得

        private void GetGroupList(int kbn, string ID, string Name, string Kana)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetGroupListCompleted += new EventHandler<GetGroupListCompletedEventArgs>(this.GetGroupListCompleted);
                svc.GetGroupListAsync(Common.gstrSessionString, kbn, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetGroupList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetGroupListCompleted(Object sender, GetGroupListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetGroupList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetGroupList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetGroupList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetGroupList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetGroupList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetGroupListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region ユーザー一覧取得

        private void GetUserList(string ID, string Name, string Kana)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetUserListCompleted += new EventHandler<GetUserListCompletedEventArgs>(this.GetUserListCompleted);
                svc.GetUserListAsync(Common.gstrSessionString, ID, Name, Kana);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetUserList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetUserListCompleted(Object sender, GetUserListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetUserList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetUserList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetUserList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetUserList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetUserList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetUserListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 仕入先一覧取得

        private void GetPurchaseList(string ID, string Name, string Kana, string GroupID)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetPurchaseListCompleted += new EventHandler<GetPurchaseListCompletedEventArgs>(this.GetPurchaseListCompleted);
                svc.GetPurchaseListAsync(Common.gstrSessionString, ID, Name, Kana, GroupID);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPurchaseList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPurchaseListCompleted(Object sender, GetPurchaseListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPurchaseList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPurchaseList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPurchaseList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPurchaseList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPurchaseList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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

        #region 商品在庫一覧取得

        private void GetInventoryList(string ID, string Name)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetInventoryListCompleted += new EventHandler<GetInventoryListCompletedEventArgs>(this.GetInventoryListCompleted);
                svc.GetInventoryListAsync(Common.gstrSessionString, ID, Name);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetInventoryList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetInventoryListCompleted(Object sender, GetInventoryListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetInventoryList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetInventoryList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetInventoryList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetInventoryList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetInventoryList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetInventoryListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 売掛残高一覧取得

        private void GetSalesBalanceList(string ID, string Name)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetSalesBalanceListCompleted += new EventHandler<GetSalesBalanceListCompletedEventArgs>(this.GetSalesBalanceListCompleted);
                svc.GetSalesBalanceListAsync(Common.gstrSessionString, ID, Name);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetSalesBalanceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetSalesBalanceListCompleted(Object sender, GetSalesBalanceListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSalesBalanceList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSalesBalanceList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSalesBalanceList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSalesBalanceList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetSalesBalanceList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetSalesBalanceListCompleted" + Environment.NewLine + ex.ToString(), "エラー確認");
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

        #region 買掛残高一覧取得

        private void GetPaymentBalanceList(string ID, string Name)
        {
            try
            {
                objMstName = null;   // 初期化
                svcMstDataClient svc = new svcMstDataClient();
                svc.GetPaymentBalanceListCompleted += new EventHandler<GetPaymentBalanceListCompletedEventArgs>(this.GetPaymentBalanceListCompleted);
                svc.GetPaymentBalanceListAsync(Common.gstrSessionString, ID, Name);
            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
                ExMessageBox.Show(CLASS_NM + ".GetPaymentBalanceList" + Environment.NewLine + ex.ToString(), "エラー確認");
            }
        }

        private void GetPaymentBalanceListCompleted(Object sender, GetPaymentBalanceListCompletedEventArgs e)
        {
            try
            {
                if (DialogCloseFlg == geDialogCloseFlg.Yes & win != null)
                {
                    win.Close();
                    win = null;
                }

                objMstList = e.Result;
                if (objMstList == null)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPaymentBalanceList, null);
                    return;
                }

                if (objMstList.Count == 0)
                {
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPaymentBalanceList, null);
                    return;
                }

                if (objMstList[0].message == null)
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPaymentBalanceList, objMstList);
                    return;
                }

                if (objMstList[0].message == "")
                {
                    // 認証成功
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPaymentBalanceList, objMstList);
                    return;
                }
                else
                {
                    // 認証失敗
                    ExMessageBox.Show(objMstList[0].message);
                    objPerent.DataSelect((int)geWebServiceMstNmCallKbn.GetPaymentBalanceList, null);
                }

            }
            catch (Exception ex)
            {
                Common.gblnDesynchronizeLock = false;
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
    }
}
