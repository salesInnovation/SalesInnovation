#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_MeiSearch : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_MeiSearch";

        // リターン名称ID
        public int id;

        // リターン名称
        public string description;

        // 名称リスト
        public static List<ListData> lst;

        #endregion

        #region Constructor

        public Dlg_MeiSearch()
        {
            InitializeComponent();
            this.SetWindowsResource();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lst = null;
            this.id = 0;
            this.Name = "";

            switch (MeiNameList.gNameKbn)
            {
                case MeiNameList.geNameKbn.TAX_CHANGE_ID:               // 1:税転換ID
                    lst = MeiNameList.glstTaxChange;
                    this.Title = "税転換一覧";
                    break;
                case MeiNameList.geNameKbn.BUSINESS_DIVISION_ID:        // 2:取引区分ID
                    lst = MeiNameList.glstBusinessDivison;
                    this.Title = "取引区分一覧";
                    break;
                case MeiNameList.geNameKbn.BREAKDOWN_ID:                // 3:内訳ID	
                    lst = MeiNameList.glstBreakdown;
                    this.Title = "内訳一覧";
                    break;
                case MeiNameList.geNameKbn.DELIVER_DIVISION_ID:         // 4:納品区分ID	
                    lst = MeiNameList.glstDeliverDivision;
                    this.Title = "納品区分一覧";
                    break;
                case MeiNameList.geNameKbn.UNIT_ID:                     // 5:単位ID	
                    lst = MeiNameList.glstUnit;
                    this.Title = "単位一覧";
                    break;
                case MeiNameList.geNameKbn.TAX_DIVISION_ID:             // 6:課税区分ID
                    lst = MeiNameList.glstTaxDivision;
                    this.Title = "課税区分一覧";
                    break;
                case MeiNameList.geNameKbn.INVENTORY_DIVISION_ID:       // 7:在庫管理区分ID
                    lst = MeiNameList.glstInventoryDivison;
                    this.Title = "課税区分一覧";
                    break;
                case MeiNameList.geNameKbn.UNIT_PRICE_DIVISION_ID:      // 8:単価区分ID
                    lst = MeiNameList.glstUnitPriceDivision;
                    this.Title = "単価種類一覧";
                    break;
                case MeiNameList.geNameKbn.DISPLAY_DIVISION_ID:         // 9:表示区分ID	
                    lst = MeiNameList.glstDisplayDivision;
                    this.Title = "表示区分一覧";
                    break;
                case MeiNameList.geNameKbn.TITLE_ID:                    // 10:敬称ID	
                    lst = MeiNameList.glstTitle;
                    this.Title = "敬称一覧";
                    break;
                case MeiNameList.geNameKbn.FRACTION_PROC_ID:            // 11:端数処理ID	
                    lst = MeiNameList.glstFractionProc;
                    this.Title = "端数処理一覧";
                    break;
                case MeiNameList.geNameKbn.COLLECT_CYCLE_ID:            // 12:回収サイクルID	
                    lst = MeiNameList.glstCollectCycle;
                    if (MeiNameList.gIsPaymentCycle == false)
                    {
                        this.Title = "回収サイクル一覧";
                    }
                    else
                    {
                        this.Title = "支払サイクル一覧";
                    }
                    break;
                case MeiNameList.geNameKbn.CLASS:                       // 13:分類区分ID	
                    lst = MeiNameList.glstClass;
                    this.Title = "分類区分一覧";
                    break;
                case MeiNameList.geNameKbn.DIVIDE_PERMISSION_ID:        // 14:分納許可ID
                    lst = MeiNameList.glstDividePermission;
                    this.Title = "分納許可一覧";
                    break;
                case MeiNameList.geNameKbn.INQUIRY_DIVISION_ID:         // 15:問い合わせ区分ID
                    lst = MeiNameList.glstInquiryDivision;
                    this.Title = "問い合わせ区分一覧";
                    break;
                case MeiNameList.geNameKbn.LEVEL_ID:                    // 16:レベルID
                    lst = MeiNameList.glstLevel;
                    this.Title = "レベル一覧";
                    break;
                case MeiNameList.geNameKbn.INQUIRY_STATE_ID:            // 17:問い合わせ状態ID
                    lst = MeiNameList.glstInquiryState;
                    this.Title = "問い合わせ状態一覧";
                    break;
                case MeiNameList.geNameKbn.APPROVAL_STATE_ID:           // 18:承認状態ID
                    lst = MeiNameList.glstApprovalState;
                    this.Title = "承認状態一覧";
                    break;
                case MeiNameList.geNameKbn.ACCOUNT_KBN:                 // 19:預金種別
                    lst = MeiNameList.glstAccountKbn;
                    this.Title = "預金種別一覧";
                    break;
                case MeiNameList.geNameKbn.OPEN_CLOSE_STATE_ID:         // 20:状態ID
                    lst = MeiNameList.glstOpenCloseState;
                    this.Title = "状態一覧";
                    break;
                case MeiNameList.geNameKbn.BUSINESS_DIVISION_PU_ID:     // 21:取引区分ID(仕入)
                    lst = MeiNameList.glstBusinessDivisonPu;
                    this.Title = "取引区分一覧";
                    break;
                case MeiNameList.geNameKbn.SEND_KBN:                    // 22:発送区分
                    lst = MeiNameList.glstSendkbn;
                    this.Title = "発送区分一覧";
                    break;
                case MeiNameList.geNameKbn.TAX_CHANGE_PU_ID:            // 23:税転換ID(仕入)
                    lst = MeiNameList.glstTaxChangePu;
                    this.Title = "税転換一覧";
                    break;
                case MeiNameList.geNameKbn.UNIT_PRICE_DIVISION_PU_ID:   // 24:単価区分ID(仕入)
                    lst = MeiNameList.glstUnitPriceDivisionPu;
                    this.Title = "単価種類一覧";
                    break;
                case MeiNameList.geNameKbn.IN_OUT_DELIVERY_KBN:         // 25:入出庫区分
                    lst = MeiNameList.glstInOutDeliveryKbn;
                    this.Title = "入出庫区分一覧";
                    break;
                case MeiNameList.geNameKbn.IN_OUT_DELIVERY_PROC_KBN:    // 26:入出庫処理区分
                    lst = MeiNameList.glstInOutDeliveryProcKbn;
                    this.Title = "入出庫処理区分一覧";
                    break;
                case MeiNameList.geNameKbn.IN_OUT_DELIVERY_TO_KBN:      // 27:入出庫先区分
                    lst = MeiNameList.glstInOutDeliveryToKbn;
                    this.Title = "入出庫先区分";
                    break;
                default:
                    break;
            }
            if (lst != null)
            {
                dg.ItemsSource = lst;
                dg.Focus();
                dg.SelectedIndex = 0;
            }
        }

        private void ExChildWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // ファンクションキーを受け取る
                case Key.F1: this.btnF1_Click(this.btnF1, null); break;
                case Key.Enter: this.btnF1_Click(this.btnF1, null); break;
                case Key.F12: this.btnF12_Click(this.btnF12, null); break;
                default: break;
            }

        }

        #endregion

        #region Function Key Button Method

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            if (lst == null)
            {
                ExMessageBox.Show("データが登録されていません。");
                return;
            }
            if (lst.Count == 0)
            {
                ExMessageBox.Show("データが登録されていません。");
                return;
            }

            int intIndex = this.dg.SelectedIndex;
            if (intIndex < 0)
            {
                ExMessageBox.Show("行が選択されていません。");
                return;
            }

            this.id = lst[intIndex].ID;
            this.description = lst[intIndex].DESCRIPTION;
            this.DialogResult = true;
        }

        // F10ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion

        #region DataGrid DoubleClick Events

        private void dg_DoubleClick(object sender, EventArgs e)
        {
            btnF1_Click(null, null);
        }

        #endregion

    }
}

