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
using System.ComponentModel.DataAnnotations;
using System.Windows.Shapes;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Themes;
using SlvHanbaiClient.View.UserControl;
using SlvHanbaiClient.View.Dlg;

#endregion

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_FunctionKey : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_FunctionKey";

        private geFunctionKeyEnable FunctionKeyEnable = geFunctionKeyEnable.Init;
        public geFunctionKeyEnable gFunctionKeyEnable 
        { 
            get 
            { 
                return this.FunctionKeyEnable; 
            } 
            set 
            { 
                this.FunctionKeyEnable = value;
                SetFunctionKeyEnable();
                ExVisualTreeHelper.SetMode(this.FunctionKeyEnable, this);
            } 
        }

        private bool IsPerentPage;      // 親要素がPageか?(True:Page False:ChildWindow)

        private bool _IsDataForm;       // 明細DataFormか?
        public bool IsDataForm { get { return this._IsDataForm; } set { this._IsDataForm = value; } }

        public Visibility btnF1_Visibility { get { return this.btnF1.Visibility; } set { this.btnF1.Visibility = value; } }
        public Visibility btnF2_Visibility { get { return this.btnF1.Visibility; } set { this.btnF2.Visibility = value; } }
        public Visibility btnF3_Visibility { get { return this.btnF1.Visibility; } set { this.btnF3.Visibility = value; } }
        public Visibility btnF4_Visibility { get { return this.btnF1.Visibility; } set { this.btnF4.Visibility = value; } }
        public Visibility btnF5_Visibility { get { return this.btnF1.Visibility; } set { this.btnF5.Visibility = value; } }
        public Visibility btnF6_Visibility { get { return this.btnF1.Visibility; } set { this.btnF6.Visibility = value; } }
        public Visibility btnF7_Visibility { get { return this.btnF1.Visibility; } set { this.btnF7.Visibility = value; } }
        public Visibility btnF8_Visibility { get { return this.btnF1.Visibility; } set { this.btnF8.Visibility = value; } }
        public Visibility btnF9_Visibility { get { return this.btnF1.Visibility; } set { this.btnF9.Visibility = value; } }
        public Visibility btnF11_Visibility { get { return this.btnF1.Visibility; } set { this.btnF11.Visibility = value; } }
        public Visibility btnF12_Visibility { get { return this.btnF1.Visibility; } set { this.btnF12.Visibility = value; } }

        public string btnF1_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF1.Content = value; } }
        public string btnF2_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF2.Content = value; } }
        public string btnF3_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF3.Content = value; } }
        public string btnF4_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF4.Content = value; } }
        public string btnF5_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF5.Content = value; } }
        public string btnF6_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF6.Content = value; } }
        public string btnF7_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF7.Content = value; } }
        public string btnF8_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF8.Content = value; } }
        public string btnF9_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF9.Content = value; } }
        public string btnF11_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF11.Content = value; } }
        public string btnF12_Content { get { return this.btnF1.Content.ToString(); } set { this.btnF12.Content = value; } }


        #endregion

        #region Enum

        public enum geFunctionKeyEnable
        {
            Init = 0,   // 初期モード
            InitKbn,    // 新規モード(区分有り)
            New,        // 新規モード
            Upd,        // 更新モード
            Sel         // 参照モード
        };

        #endregion

        #region Constructor

        public Utl_FunctionKey()
        {
            InitializeComponent();

            this.Resources = new SlvHanbaiClient.Themes.MargedResourceDictionary();

            this.btnF1.IsTabStop = false;
            this.btnF2.IsTabStop = false;
            this.btnF3.IsTabStop = false;
            this.btnF4.IsTabStop = false;
            this.btnF5.IsTabStop = false;
            this.btnF6.IsTabStop = false;
            this.btnF7.IsTabStop = false;
            this.btnF8.IsTabStop = false;
            this.btnF9.IsTabStop = false;
            this.btnF11.IsTabStop = false;
            this.btnF12.IsTabStop = false;

            this.Tag = "FunctionKeys";
        }

        #endregion

        #region Page Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // 親要素がPageか?(True:Page False:ChildWindow)
            IsPerentPage = ExVisualTreeHelper.CheckPerentIsPage(this);

            // ファンクションキー表示名設定
            SetFunctionKeyName();

            // ファンクションキー(非)有効化設定
            SetFunctionKeyEnable();
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {                    
                // ファンクションキーを受け取る
                case Key.F1: this.btnFKey_Click(this.btnF1, null); break;
                case Key.F2: this.btnFKey_Click(this.btnF2, null); break;
                case Key.F3: this.btnFKey_Click(this.btnF3, null); break;
                case Key.F4: this.btnFKey_Click(this.btnF4, null); break;
                case Key.F5: this.btnFKey_Click(this.btnF5, null); break;
                case Key.F6: this.btnFKey_Click(this.btnF6, null); break;
                case Key.F7: this.btnFKey_Click(this.btnF7, null); break;
                case Key.F8: this.btnFKey_Click(this.btnF8, null); break;
                case Key.F9: this.btnFKey_Click(this.btnF9, null); break;
                case Key.F11: this.btnFKey_Click(this.btnF11, null); break;
                case Key.F12: this.btnFKey_Click(this.btnF12, null); break;
                default: break;
            }
        }

        private void ExUserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Method

        public void Init()
        {
            // ファンクションキー表示名設定
            SetFunctionKeyName();

            // ファンクションキー(非)有効化設定
            SetFunctionKeyEnable();
        }

        // ファンクションキー(非)有効化設定
        protected void SetFunctionKeyEnable()
        {
            // ファンクションキー有効化設定 初期化
            InitFunctionKeyEnable();

            #region DataForm

            if (IsDataForm == true)
            {
                switch (this.gFunctionKeyEnable)
                {
                    case geFunctionKeyEnable.New:    // 新規モード
                        this.btnF1.IsEnabled = false;       // OK
                        this.btnF2.IsEnabled = false;       // 追加
                        this.btnF3.IsEnabled = false;       // 削除
                        this.btnF4.IsEnabled = true;        // クリア
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF6.IsEnabled = true;        // 保存
                        this.btnF12.IsEnabled = true;       // キャンセル
                        break;
                    case geFunctionKeyEnable.Upd:    // 更新モード
                        this.btnF1.IsEnabled = true;   　   // OK
                        this.btnF2.IsEnabled = true;        // 追加
                        this.btnF3.IsEnabled = true;        // 削除
                        this.btnF4.IsEnabled = false;       // クリア
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF6.IsEnabled = true;        // 保存
                        this.btnF12.IsEnabled = true;       // キャンセル
                        break;
                    default:
                        break;
                }

            }

            #endregion

            #region Page

            else if (IsPerentPage == true)
            {
                switch (Common.gPageGroupType)
                {
                    case Common.gePageGroupType.Inp:   // 伝票入力
                        switch (Common.gPageType)
                        {
                            case Common.gePageType.InpEstimate:
                            case Common.gePageType.InpOrder:
                            case Common.gePageType.InpPurchaseOrder:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = false;       // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.gePageType.InpSales:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = true;        // 赤伝発行
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = false;       // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = true;       // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.gePageType.InpInvoiceClose:
                            case Common.gePageType.InpPaymentClose:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = false;       // 締切
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 集計
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード(締切未)
                                        this.btnF1.IsEnabled = true;        // 締切
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 集計
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード(締切済)
                                        this.btnF1.IsEnabled = false;       // 締切
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 集計
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 締切
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 集計
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.gePageType.InpReceipt:
                            case Common.gePageType.InpPaymentCash:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = false;       // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.gePageType.InpPurchase:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = true;        // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = false;       // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.gePageType.InpInOutDelivery:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = true;        // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF6.IsEnabled = false;       // 明細画面
                                        this.btnF7.IsEnabled = false;       // 明細追加
                                        this.btnF8.IsEnabled = false;       // 明細削除
                                        this.btnF9.IsEnabled = false;       // 赤伝発行
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.gePageType.InpStockInventory:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:   // 初期モード
                                        this.btnF1.IsEnabled = false;       // 登録
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 集計
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:    // 新規モード(締切未)
                                        this.btnF1.IsEnabled = true;        // 登録
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 集計
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:    // 更新モード(締切済)
                                        this.btnF1.IsEnabled = true;        // 登録
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 集計
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:    // 参照モード
                                        this.btnF1.IsEnabled = false;       // 登録
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 集計
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF11.IsEnabled = false;      // 印刷
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion

            #region Window

            else if (IsPerentPage == false)
            {
                switch (Common.gWinGroupType)
                {
                    case Common.geWinGroupType.InpList:
                        this.btnF1.IsEnabled = true;        // OK
                        this.btnF2.IsEnabled = true;        // 条件クリア
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF6.IsEnabled = true;        // 検索
                        this.btnF12.IsEnabled = true;       // メニュー
                        break;
                    case Common.geWinGroupType.InpListUpd:
                        this.btnF1.IsEnabled = true;        // 保存
                        this.btnF2.IsEnabled = true;        // 条件クリア
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF6.IsEnabled = true;        // 検索
                        this.btnF12.IsEnabled = true;       // メニュー
                        break;
                    case Common.geWinGroupType.InpListReport:
                    case Common.geWinGroupType.InpDetailReport:
                        this.btnF1.IsEnabled = true;        // 出力
                        this.btnF2.IsEnabled = true;        // 条件クリア
                        this.btnF3.IsEnabled = true;        // ダウンロード
                        //this.btnF4.IsEnabled = true;        // CSV
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF6.IsEnabled = true;        // 検索
                        this.btnF11.IsEnabled = true;       // 出力設定
                        this.btnF12.IsEnabled = true;       // メニュー
                        break;
                    case Common.geWinGroupType.InpMaster:
                        switch (Common.gWinMsterType)
                        {
                            case Common.geWinMsterType.Authority:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:      // 初期モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 検索
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.New:       // 新規モード
                                    case geFunctionKeyEnable.Upd:       // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 検索
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:       // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 検索
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case Common.geWinMsterType.Company:
                                this.btnF1.IsEnabled = true;        // 保存
                                this.btnF2.IsEnabled = true;        // クリア
                                this.btnF5.IsEnabled = true;        // 参照
                                this.btnF12.IsEnabled = true;       // メニュー
                                break;
                            case Common.geWinMsterType.User:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.Init:      // 初期モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Upd:       // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    case geFunctionKeyEnable.Sel:       // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                    default:
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー
                                        break;
                                }
                                break;
                            default:
                                switch (this.gFunctionKeyEnable)
                                {
                                    case geFunctionKeyEnable.InitKbn:   // 初期モード(区分有り)
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー

                                        // 印刷
                                        if (Common.gWinMsterType == Common.geWinMsterType.CompanyGroup) this.btnF11.IsEnabled = false;
                                        else this.btnF11.IsEnabled = true;       

                                        break;
                                    case geFunctionKeyEnable.Init:      // 初期モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = true;        // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー

                                        // 印刷
                                        if (Common.gWinMsterType == Common.geWinMsterType.CompanyGroup) this.btnF11.IsEnabled = false;
                                        else this.btnF11.IsEnabled = true;

                                        break;
                                    case geFunctionKeyEnable.New:       // 新規モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = false;       // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー

                                        // 印刷
                                        if (Common.gWinMsterType == Common.geWinMsterType.CompanyGroup) this.btnF11.IsEnabled = false;
                                        else this.btnF11.IsEnabled = true;       

                                        break;
                                    case geFunctionKeyEnable.Upd:       // 更新モード
                                        this.btnF1.IsEnabled = true;        // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = true;        // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー

                                        // 印刷
                                        if (Common.gWinMsterType == Common.geWinMsterType.CompanyGroup) this.btnF11.IsEnabled = false;
                                        else this.btnF11.IsEnabled = true;       

                                        break;
                                    case geFunctionKeyEnable.Sel:       // 参照モード
                                        this.btnF1.IsEnabled = false;       // 保存
                                        this.btnF2.IsEnabled = true;        // クリア
                                        this.btnF3.IsEnabled = true;        // 複写
                                        this.btnF4.IsEnabled = false;       // 削除
                                        this.btnF5.IsEnabled = false;       // 参照
                                        this.btnF12.IsEnabled = true;       // メニュー

                                        // 印刷
                                        if (Common.gWinMsterType == Common.geWinMsterType.CompanyGroup) this.btnF11.IsEnabled = false;
                                        else this.btnF11.IsEnabled = true;       

                                        break;
                                    default:
                                        break;
                                }
                                break;
                        }
                        break;
                    case Common.geWinGroupType.InpMasterDetail:
                        switch (this.gFunctionKeyEnable)
                        {
                            case geFunctionKeyEnable.Init:   // 初期モード
                            case geFunctionKeyEnable.New:    // 新規モード
                                this.btnF1.IsEnabled = false;       // 保存
                                this.btnF2.IsEnabled = true;        // クリア
                                this.btnF3.IsEnabled = false;       // 行挿入
                                this.btnF4.IsEnabled = false;       // 行削除
                                this.btnF5.IsEnabled = false;       // 参照
                                this.btnF11.IsEnabled = true;       // 印刷
                                this.btnF12.IsEnabled = true;       // メニュー
                                break;
                            case geFunctionKeyEnable.Upd:    // 更新モード
                                this.btnF1.IsEnabled = true;        // 保存
                                this.btnF2.IsEnabled = true;        // クリア
                                this.btnF3.IsEnabled = true;        // 行挿入
                                this.btnF4.IsEnabled = true;        // 行削除
                                this.btnF5.IsEnabled = false;       // 参照
                                this.btnF11.IsEnabled = true;       // 印刷
                                this.btnF12.IsEnabled = true;       // メニュー
                                break;
                            case geFunctionKeyEnable.Sel:    // 参照モード
                                this.btnF1.IsEnabled = false;       // 保存
                                this.btnF2.IsEnabled = true;        // クリア
                                this.btnF3.IsEnabled = false;       // 行挿入
                                this.btnF4.IsEnabled = false;       // 行削除
                                this.btnF5.IsEnabled = false;       // 参照
                                this.btnF11.IsEnabled = true;       // 印刷
                                this.btnF12.IsEnabled = true;       // メニュー
                                break;
                            default:
                                break;
                        }
                        break;
                    case Common.geWinGroupType.Report:
                        this.btnF1.IsEnabled = false;       // 出力
                        this.btnF2.IsEnabled = false;       // ﾀﾞｳﾝﾛｰﾄﾞ
                        this.btnF3.IsEnabled = true;        // CSV
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF11.IsEnabled = false;      // 出力設定
                        this.btnF12.IsEnabled = true;       // メニュー
                        break;
                    case Common.geWinGroupType.ReportSetting:
                        this.btnF1.IsEnabled = true;        // OK
                        this.btnF2.IsEnabled = true;        // クリア
                        this.btnF5.IsEnabled = false;       // 参照
                        this.btnF12.IsEnabled = true;       // メニュー
                        break;
                    default:
                        break;
                }
            }

            #endregion

        }

        // ファンクションキー(非)有効化設定 ボタン別
        public void SetFunctionKeyEnable(string strFunctionKeyName, bool enable)
        {
            switch (strFunctionKeyName)
            {
                case "F1": this.btnF1.IsEnabled = enable; break;
                case "F2": this.btnF2.IsEnabled = enable; break;
                case "F3": this.btnF3.IsEnabled = enable; break;
                case "F4": this.btnF4.IsEnabled = enable; break;
                case "F5": this.btnF5.IsEnabled = enable; break;
                case "F6": this.btnF6.IsEnabled = enable; break;
                case "F7": this.btnF7.IsEnabled = enable; break;
                case "F8": this.btnF8.IsEnabled = enable; break;
                case "F9": this.btnF9.IsEnabled = enable; break;
                case "F11": this.btnF11.IsEnabled = enable; break;
                case "F12": this.btnF12.IsEnabled = enable; break;
                default: break;
            }
        }

        // ファンクションキー表示名設定
        public void SetFunctionKeyName()
        {
            // ファンクションキー表示名初期化
            InitFunctionKeyName();

            #region DataForm

            if (IsDataForm == true)
            {
                this.btnF1.Content = "     F1     " + Environment.NewLine + "    O K";
                this.btnF2.Content = "     F2     " + Environment.NewLine + "   追  加";
                this.btnF3.Content = "     F3     " + Environment.NewLine + "   削  除";
                this.btnF4.Content = "     F4     " + Environment.NewLine + "  クリア";
                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参  照";
                this.btnF6.Content = "     F6     " + Environment.NewLine + "   保  存";
                this.btnF12.Content = "    F12   " + Environment.NewLine + "  Cancel";
            }

            #endregion

            #region Page

            else if (IsPerentPage == true)
            {
                // 親要素がPage
                switch (Common.gPageGroupType)
                {
                    case Common.gePageGroupType.Inp:   // 伝票入力
                        switch (Common.gPageType)
                        {
                            case Common.gePageType.InpEstimate:
                            case Common.gePageType.InpOrder:
                            case Common.gePageType.InpPurchaseOrder:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF6.Content = "     F6     " + Environment.NewLine + " 明細画面";
                                this.btnF7.Content = "     F7     " + Environment.NewLine + " 明細追加";
                                this.btnF8.Content = "     F8     " + Environment.NewLine + " 明細削除";
                                this.btnF11.Content = "    F11    " + Environment.NewLine + " レポート";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                            case Common.gePageType.InpSales:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF6.Content = "     F6     " + Environment.NewLine + " 明細画面";
                                this.btnF7.Content = "     F7     " + Environment.NewLine + " 明細追加";
                                this.btnF8.Content = "     F8     " + Environment.NewLine + " 明細削除";
                                this.btnF9.Content = "     F9     " + Environment.NewLine + " 赤伝発行";
                                this.btnF11.Content = "    F11    " + Environment.NewLine + " レポート";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                            case Common.gePageType.InpInvoiceClose:
                            case Common.gePageType.InpPaymentClose:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   締 切";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   集 計";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                            case Common.gePageType.InpReceipt:
                            case Common.gePageType.InpPaymentCash:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF6.Content = "     F6     " + Environment.NewLine + " 明細画面";
                                this.btnF7.Content = "     F7     " + Environment.NewLine + " 明細追加";
                                this.btnF8.Content = "     F8     " + Environment.NewLine + " 明細削除";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                            case Common.gePageType.InpPurchase:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF6.Content = "     F6     " + Environment.NewLine + " 明細画面";
                                this.btnF7.Content = "     F7     " + Environment.NewLine + " 明細追加";
                                this.btnF8.Content = "     F8     " + Environment.NewLine + " 明細削除";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                            case Common.gePageType.InpInOutDelivery:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF7.Content = "     F7     " + Environment.NewLine + " 明細追加";
                                this.btnF8.Content = "     F8     " + Environment.NewLine + " 明細削除";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                            case Common.gePageType.InpStockInventory:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   集 計";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF12.Content = "     F12   " + Environment.NewLine + "  メニュー ";
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion

            #region ChildWindow

            else
            {
                // 親要素がChildWindow
                switch (Common.gWinGroupType)
                {
                    case Common.geWinGroupType.InpList:             // 伝票一覧
                        this.btnF1.Content = "     F1     " + Environment.NewLine + "    O K";
                        this.btnF2.Content = "     F2     " + Environment.NewLine + "  条件ｸﾘｱ";
                        this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                        this.btnF6.Content = "     F6     " + Environment.NewLine + "   検 索";
                        this.btnF12.Content = "     F12    " + Environment.NewLine + "   Cancel";
                        break;
                    case Common.geWinGroupType.InpListUpd:          // 一覧更新
                        this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                        this.btnF2.Content = "     F2     " + Environment.NewLine + "  条件ｸﾘｱ";
                        this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                        this.btnF6.Content = "     F6     " + Environment.NewLine + "   検 索";
                        this.btnF12.Content = "     F12    " + Environment.NewLine + "   Cancel";
                        break;
                    case Common.geWinGroupType.InpListReport:       // 伝票一覧(印刷)
                    case Common.geWinGroupType.InpDetailReport:     // 伝票一覧(明細印刷)
                        this.btnF1.Content = "     F1     " + Environment.NewLine + "   出 力";
                        this.btnF2.Content = "     F2     " + Environment.NewLine + "  条件ｸﾘｱ";
                        this.btnF3.Content = "     F3     " + Environment.NewLine + "  ﾀﾞｳﾝﾛｰﾄﾞ"; 
                        //this.btnF4.Content = "     F4     " + Environment.NewLine + "  ＣＳＶ";
                        this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                        this.btnF6.Content = "     F6     " + Environment.NewLine + "   検 索";
                        this.btnF11.Content = "     F11    " + Environment.NewLine + " 出力設定";
                        this.btnF12.Content = "     F12    " + Environment.NewLine + "   Cancel";
                        break;
                    case Common.geWinGroupType.NameList:            // 名称一覧
                        break;
                    case Common.geWinGroupType.MstList:             // マスタ一覧
                        break;
                    case Common.geWinGroupType.InpMaster:           // マスタ登録
                        switch (Common.gWinMsterType)
                        { 
                            case Common.geWinMsterType.Authority:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   検 索";
                                this.btnF12.Content = "     F12    " + Environment.NewLine + "  メニュー";
                                break;
                            case Common.geWinMsterType.Company:
                            case Common.geWinMsterType.User:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF12.Content = "     F12    " + Environment.NewLine + "  メニュー";
                                break;
                            case Common.geWinMsterType.CompanyGroup:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF12.Content = "     F12    " + Environment.NewLine + "  メニュー";
                                break;
                            default:
                                this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                                this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                                this.btnF3.Content = "     F3     " + Environment.NewLine + "   複 写";
                                this.btnF4.Content = "     F4     " + Environment.NewLine + "   削 除";
                                this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                                this.btnF11.Content = "     F11    " + Environment.NewLine + "  レポート";
                                this.btnF12.Content = "     F12    " + Environment.NewLine + "  メニュー";
                                break;
                        }
                        break;
                    case Common.geWinGroupType.InpMasterDetail:     // マスタ登録明細式
                        this.btnF1.Content = "     F1     " + Environment.NewLine + "   登 録";
                        this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                        this.btnF3.Content = "     F3     " + Environment.NewLine + "  行挿入";
                        this.btnF4.Content = "     F4     " + Environment.NewLine + "  行削除";
                        this.btnF5.Content = "     F5     " + Environment.NewLine + "   参 照";
                        this.btnF11.Content = "     F11    " + Environment.NewLine + "  レポート";
                        this.btnF12.Content = "     F12    " + Environment.NewLine + "  メニュー";
                        break;
                    case Common.geWinGroupType.Report:              // レポート出力
                        //this.btnF1.Content = "     F1     " + Environment.NewLine + "   出  力";
                        //this.btnF2.Content = "     F2     " + Environment.NewLine + "  ﾀﾞｳﾝﾛｰﾄﾞ"; 
                        this.btnF3.Content = "     F3     " + Environment.NewLine + "  ＣＳＶ";
                        this.btnF5.Content = "     F5     " + Environment.NewLine + "   参  照";
                        //this.btnF11.Content = "    F11    " + Environment.NewLine + "  出力設定";
                        this.btnF12.Content = "    F12    " + Environment.NewLine + "  Cancel";
                        break;
                    case Common.geWinGroupType.ReportSetting:       // レポート出力設定
                        this.btnF1.Content = "     F1     " + Environment.NewLine + "   Ｏ Ｋ";
                        this.btnF2.Content = "     F2     " + Environment.NewLine + "  クリア";
                        this.btnF5.Content = "     F5     " + Environment.NewLine + "   参  照";
                        this.btnF12.Content = "    F12    " + Environment.NewLine + "  Cancel";
                        break;
                    default:
                        break;
                }
            }

            #endregion

            for (int i = 1; i <= 12; i++)
            {
                if (i != 10)
                {
                    Button btn = (Button)this.FindName("btnF" + i.ToString());
                    btn.Width = 65;
                }
            }
        }

        // ファンクションキー表示名初期化
        public void InitFunctionKeyName()
        {
            // 伝票入力系共通
            this.btnF1.Content = "     F1     " + Environment.NewLine;
            this.btnF2.Content = "     F2     " + Environment.NewLine;
            this.btnF3.Content = "     F3     " + Environment.NewLine;
            this.btnF4.Content = "     F4     " + Environment.NewLine;
            this.btnF5.Content = "     F5     " + Environment.NewLine;
            this.btnF6.Content = "     F6     " + Environment.NewLine;
            this.btnF7.Content = "     F7     " + Environment.NewLine;
            this.btnF8.Content = "     F8     " + Environment.NewLine;
            this.btnF9.Content = "     F9     " + Environment.NewLine;
            this.btnF11.Content = "     F11    " + Environment.NewLine;
            this.btnF12.Content = "     F12    " + Environment.NewLine;

            for (int i = 1; i <= 12; i++)
            {
                if (i != 10)
                {
                    Button btn = (Button)this.FindName("btnF" + i.ToString());
                    btn.Content = "     F" + i.ToString() + "     " + Environment.NewLine;
                }
            }
        }

        // ファンクションキー有効化設定 初期化
        public void InitFunctionKeyEnable()
        {
            for (int i = 1; i <= 12; i++)
            {
                if (i != 10)
                {
                    Button btn = (Button)this.FindName("btnF" + i.ToString());
                    int index = 0;
                    index = btn.Content.ToString().IndexOf(Environment.NewLine);
                    if (index == btn.Content.ToString().Length - 2)
                    {
                        btn.IsEnabled = false;
                    }
                }
            }
        }

        // ファンクションキークリック処理
        // (処理実装は各種入力ページにて行う)
        public void btnFKey_Click(object sender, RoutedEventArgs e)
        {
            #region 参照モードからメニューに戻ると非有効化のままになる為、戻す

            try
            {
                if (this.FunctionKeyEnable == geFunctionKeyEnable.Sel)
                {
                    Button btn = (Button)sender;

                    if (btn.IsEnabled == true && btn.Name == "btnF12")
                    {
                        this.FunctionKeyEnable = geFunctionKeyEnable.Init;
                        ExVisualTreeHelper.SetMode(this.FunctionKeyEnable, this);
                    }
                }
            }
            catch
            {
            }

            #endregion

            ExUserControl utl;
            ExChildWindow win;
            try
            {
                if (IsDataForm == true)
                {
                    utl = ExVisualTreeHelper.GetMainUserControlForWindow(this);
                    utl.FunctionKey_Click(sender, e);
                }
                else if (IsPerentPage == true)
                {
                    // 親要素がPage
                    switch (Common.gPageGroupType)
                    {
                        case Common.gePageGroupType.Inp:        // 伝票入力
                            utl = ExVisualTreeHelper.GetMainUserControlForPage(this);
                            utl.FunctionKey_Click(sender, e);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    // 親要素がChildWindow
                    switch (Common.gWinGroupType)
                    {
                        case Common.geWinGroupType.InpList:             // 伝票一覧
                        case Common.geWinGroupType.InpListUpd:          // 一覧更新
                        case Common.geWinGroupType.InpListReport:       // 伝票一覧(印刷)
                        case Common.geWinGroupType.InpDetailReport:     // 伝票一覧(明細印刷)
                        case Common.geWinGroupType.NameList:            // 名称一覧
                        case Common.geWinGroupType.MstList:             // マスタ一覧
                        case Common.geWinGroupType.InpMaster:           // マスタ登録
                        case Common.geWinGroupType.InpMasterDetail:     // マスタ登録(明細式)
                        case Common.geWinGroupType.Report:              // レポート出力
                            utl = ExVisualTreeHelper.GetMainUserControlForWindow(this);
                            utl.FunctionKey_Click(sender, e);
                            break;
                        case Common.geWinGroupType.ReportSetting:       // レポート出力設定
                            win = (ExChildWindow)ExVisualTreeHelper.FindPerentChildWindow(this);
                            win.FunctionKey_Click(sender, e);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ExMessageBox.Show(CLASS_NM + ".btnFKey_Click" + ex.Message);
            }
        }

        #endregion

        #region GotFocus

        private void btnF1_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF2_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF3_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF4_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF6_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF7_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF8_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF9_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF11_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        private void btnF12_GotFocus(object sender, RoutedEventArgs e)
        {
            btnF5.IsEnabled = false;
        }

        #endregion

    }
}
