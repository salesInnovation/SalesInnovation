using System;
using System.ComponentModel;
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
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Input;
using SlvHanbaiClient.View.UserControl.Input.Sales;
using SlvHanbaiClient.View.UserControl.Input.Purchase;
using SlvHanbaiClient.View.UserControl.Master;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.svcOrder;
using SlvHanbaiClient.svcEstimate;
using SlvHanbaiClient.svcSales;
using SlvHanbaiClient.svcReceipt;
using SlvHanbaiClient.svcPurchaseOrder;
using SlvHanbaiClient.svcPurchase;
using SlvHanbaiClient.svcPaymentCash;
using SlvHanbaiClient.svcMstData;

namespace SlvHanbaiClient.View.UserControl.Custom
{
    public partial class Utl_MstText : ExUserControl
    {

        #region Filed Const

        private const String CLASS_NM = "Utl_MstText";

        private bool _IsNextControl = true;
        public bool IsNextControl
        {
            set
            {
                this._IsNextControl = value;
            }
            get
            {
                return this._IsNextControl;
            }
        }

        private object _ParentData;
        public object ParentData
        {
            set
            {
                this._ParentData = value;
            }
            get
            {
                return this._ParentData;
            }
        }

        private EntityMstData _MasterData;
        public EntityMstData MasterData
        {
            set
            {
                this._MasterData = value;
            }
            get
            {
                return this._MasterData;
            }
        }

        private MstData.geMDataKbn _MstKbn;
        public MstData.geMDataKbn MstKbn
        {
            set
            {
                this._MstKbn = value;
            }
            get
            {
                return this._MstKbn;
            }
        }

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

        public double id_Width 
        { 
            set 
            { 
                this.txtID.Width = value;
                this.Width = this.txtID.Width + txtNm.Width;
            } 
            get { return this.txtID.Width; } 
        }
        public double nm_Width 
        { 
            set 
            { 
                this.txtNm.Width = value;
                this.Width = this.txtID.Width + txtNm.Width;
            } 
            get { return this.txtNm.Width; } 
        }
        public ExTextBox.geInputMode id_InputMode
        {
            set
            {
                this.txtID.InputMode = value;
            }
            get { return this.txtID.InputMode; }
        }
        public string id_FormatString
        {
            set
            {
                if (value != "") this.txtID.FormatString = value;
            }
            get { return this.txtID.FormatString; }
        }
        public double id_MaxNumber
        {
            set
            {
                this.txtID.MaxNumber = value;
            }
            get { return this.txtID.MaxNumber; }
        }
        public double id_MinNumber
        {
            set
            {
                this.txtID.MinNumber = value;
            }
            get { return this.txtID.MinNumber; }
        }
        public bool id_IsReadOnly
        {
            set
            {
                this.txtID.IsReadOnly = value;
                if (value == true)
                {
                    this.txtID.IsTabStop = false;
                    this.Tag = "";
                }
                else
                {
                    this.txtID.IsTabStop = true;
                    this.Tag = "TabSearchOn";
                }
            }
            get { return this.txtID.IsReadOnly; }
        }

        public string txtID_Text
        {
            set
            {
                this.txtID.Text = value;
            }
            get
            {
                return this.txtID.Text;
            }
        }

        public bool txtID_IsReadOnly
        {
            set
            {
                this.txtID.IsReadOnly = value;
                if (value == true)
                {
                    this.txtID.IsTabStop = false;
                    this.IsTabStop = false;
                    this.txtID.Background = new SolidColorBrush(Colors.LightGray);
                }
                else
                {
                    this.txtID.IsTabStop = true;
                    this.IsTabStop = true;
                    this.txtID.Background = new SolidColorBrush(Colors.White);
                }
                //SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                //mySolidColorBrush.Color = Color.FromArgb(255, 97, 117, 132);
                //this.txtID.Background = mySolidColorBrush;
                
            }
            get
            {
                return this.txtID.IsReadOnly;
            }
        }

        private Dlg_MstSearch searchDlg = null;
        private Dlg_MstSearchGroup searchGroupDlg = null;
        private bool IsSearchDlgOpen = false;

        private bool _IsLockCheck = false;
        public bool IsLockCheck
        {
            set
            {
                this._IsLockCheck = value;
            }
            get
            {
                return this._IsLockCheck;
            }
        }

        public Utl_MstText utlSupplier = null;

        public bool Is_MstID_Changed = true;

        private bool _IsZeroNull = false;
        public bool IsZeroNull
        {
            set
            {
                this._IsZeroNull = value;
            }
            get
            {
                return this._IsZeroNull;
            }
        }

        public bool Is_Call_MstID_Changed = false;

        #endregion

        #region DependencyProperty

        #region UserControlIdProperty

        public static readonly DependencyProperty UserControlIdProperty =
            DependencyProperty.Register("UserControlId",
                                        typeof(string),
                                        typeof(Utl_MstText),
                                        new PropertyMetadata("UserControlId", new PropertyChangedCallback(Utl_MstText.OnUserControlIdPropertyChanged)));

        public string UserControlId
        {
            get
            {
                return (string)GetValue(UserControlIdProperty);
            }
            set
            {
                SetValue(UserControlIdProperty, value);
            }
        }

        private static void OnUserControlIdPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Utl_MstText)d).OnUserControlIdChanged(e);
        }

        private void OnUserControlIdChanged(DependencyPropertyChangedEventArgs e)
        {
            //if (this.txtID != null)
            //{
            //    if (e.NewValue != null)
            //    {
            //        this.txtID.Text = e.NewValue.ToString();
            //    }
            //}
        }

        #endregion

        #region UserControlNameProperty

        public static readonly DependencyProperty UserControlNameProperty =
            DependencyProperty.Register("UserControlName",
                                        typeof(string),
                                        typeof(Utl_MstText),
                                        new PropertyMetadata("UserControlName", new PropertyChangedCallback(Utl_MstText.OnUserControlNamePropertyChanged)));

        public string UserControlName
        {
            get
            {
                return (string)GetValue(UserControlNameProperty);
            }
            set
            {
                SetValue(UserControlNameProperty, value);
            }
        }

        private static void OnUserControlNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Utl_MstText)d).OnUserControlNameChanged(e);
        }

        private void OnUserControlNameChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.txtNm != null)
            {
                if (e.NewValue != null)
                {
                    this.txtNm.Text = e.NewValue.ToString();
                }
            }
        }

        #endregion

        #endregion

        #region Constructor

        public Utl_MstText()
        {
            InitializeComponent();
            this.txtID.FormatString = "";
        }

        #endregion

        #region Page Events

        private void ExUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (this.MstKbn)
            {
                case MstData.geMDataKbn.Customer:
                case MstData.geMDataKbn.Customer_F:
                case MstData.geMDataKbn.Customer_T:
                case MstData.geMDataKbn.Invoice:
                case MstData.geMDataKbn.Invoice_F:
                case MstData.geMDataKbn.Invoice_T:
                case MstData.geMDataKbn.Supplier:
                case MstData.geMDataKbn.Supplier_F:
                case MstData.geMDataKbn.Supplier_T:
                    this.txtID.InputMode = ExTextBox.geInputMode.ID;
                    this.txtID.MaxLengthB = Common.gintidFigureCustomer;
                    break;
                case MstData.geMDataKbn.Purchase:
                case MstData.geMDataKbn.Purchase_F:
                case MstData.geMDataKbn.Purchase_T:
                    this.txtID.InputMode = ExTextBox.geInputMode.ID;
                    this.txtID.MaxLengthB = Common.gintidFigurePurchase;
                    break;
                case MstData.geMDataKbn.Commodity:
                case MstData.geMDataKbn.Commodity_F:
                case MstData.geMDataKbn.Commodity_T:
                    this.txtID.InputMode = ExTextBox.geInputMode.ID;
                    this.txtID.MaxLengthB = Common.gintidFigureCommodity;
                    break;
            }
            //if (this.txtID.InputMode == ExTextBox.geInputMode.Number) this.txtID.TextAlignment = TextAlignment.Right;
        }

        private void ExUserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            this.txtID.Focus();
        }

        #endregion

        #region TextBox Events

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 会社グループID 0:全グループを反映させない為の処置
            if (txtID.InputMode == ExTextBox.geInputMode.Number && this.IsZeroNull == true)
            {
                if (this.txtID.Text.Trim() == "0")
                {
                    this.txtID.Text = "";
                }
            }

            if (string.IsNullOrEmpty(this.txtID.Text.Trim()))
            {
                this.txtNm.Text = "";
            }
        }

        private void txtID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.txtID_IsReadOnly == true) return;
            if (this.IsSearchDlgOpen == true) return;

            switch (this.MstKbn)
            {
                case MstData.geMDataKbn.Customer:
                case MstData.geMDataKbn.Customer_F:
                case MstData.geMDataKbn.Customer_T:
                case MstData.geMDataKbn.Invoice:
                case MstData.geMDataKbn.Invoice_F:
                case MstData.geMDataKbn.Invoice_T:
                case MstData.geMDataKbn.Commodity:
                case MstData.geMDataKbn.Commodity_F:
                case MstData.geMDataKbn.Commodity_T:
                case MstData.geMDataKbn.Purchase:
                case MstData.geMDataKbn.Purchase_F:
                case MstData.geMDataKbn.Purchase_T:
                    searchGroupDlg = new Dlg_MstSearchGroup();
                    searchGroupDlg.MstKbn = this.MstKbn;
                    searchGroupDlg.MstGroupKbn = this.MstGroupKbn;

                    Dlg_MstSearchGroup.this_id2 = this.txtID2.Text.Trim();

                    searchGroupDlg.Show();
                    this.IsSearchDlgOpen = true;
                    searchGroupDlg.Closed -= searchGroupDlg_Closed;
                    searchGroupDlg.Closed += searchGroupDlg_Closed;
                    break;
                default:
                    searchDlg = new Dlg_MstSearch();
                    searchDlg.MstKbn = this.MstKbn;
                    searchDlg.MstGroupKbn = this.MstGroupKbn;

                    Dlg_MstSearch.this_id2 = this.txtID2.Text.Trim();

                    searchDlg.Show();
                    this.IsSearchDlgOpen = true;
                    searchDlg.Closed -= searchDlg_Closed;
                    searchDlg.Closed += searchDlg_Closed;
                    break;
            }

        }

        private void searchGroupDlg_Closed(object sender, EventArgs e)
        {
            try
            {
                Dlg_MstSearchGroup dlg = (Dlg_MstSearchGroup)sender;
                if (Dlg_MstSearchGroup.this_DialogResult == true)
                {
                    this.txtID.Text = Dlg_MstSearchGroup.this_id.ToString();
                    this.txtNm.Text = Dlg_MstSearchGroup.this_name;
                    dlg = null;

                    if (this.IsNextControl == true)
                    {
                        // 次コントロールフォーカスセット
                        ExVisualTreeHelper.FoucsNextControl(this);
                    }
                }
                else
                {
                    this.txtID.Focus();
                }
            }
            finally
            {
                this.IsSearchDlgOpen = false;
            }
        }

        private void searchDlg_Closed(object sender, EventArgs e)
        {
            try
            {
                Dlg_MstSearch dlg = (Dlg_MstSearch)sender;
                if (Dlg_MstSearch.this_DialogResult == true)
                {
                    this.txtID.Text = Dlg_MstSearch.this_id.ToString();
                    this.txtNm.Text = Dlg_MstSearch.this_name;
                    dlg = null;

                    if (this.IsNextControl == true)
                    {
                        // 次コントロールフォーカスセット
                        ExVisualTreeHelper.FoucsNextControl(this);
                    }
                }
                else
                {
                    this.txtID.Focus();
                }
            }
            finally
            {
                this.IsSearchDlgOpen = false;
            }
        }

        private void txtID_LostFocus(object sender, RoutedEventArgs e)
        {
            // 数値型をバインドして空白にして元の値に戻した場合、
            // PropertyChangedが実行されない為、下記処理を実行する
            if (txtID.InputMode == ExTextBox.geInputMode.Number)
            {
                if (!string.IsNullOrEmpty(txtID.Text.Trim()) && string.IsNullOrEmpty(txtNm.Text.Trim()))
                {
                    string beforeValue = txtID.Text.Trim();
                    
                    //var sync = new System.Windows.Threading.DispatcherSynchronizationContext();
                    //new System.Threading.Thread(() =>
                    //{
                    //    sync.Send(_ =>
                    //    {
                    //        if (string.IsNullOrEmpty(txtNm.Text.Trim()))
                    //        {
                    //            txtID.Text = "0";
                    //        }
                    //    }, null);
                    //}).Start();

                    //// txtID.Text = beforeValueとする前に
                    //// イベントキューにある処理(txtID.Text = "0"によるPropertyChanged)を実行する
                    //var sync2 = new System.Windows.Threading.DispatcherSynchronizationContext();
                    //new System.Threading.Thread(() =>
                    //{
                    //    sync2.Send(_ =>
                    //    {
                    //        if (txtID.Text == "0")
                    //        {
                    //            txtID.Text = beforeValue;
                    //            txtID.OnFormatString();
                    //        }
                    //    }, null);
                    //}).Start();
                    Action foo = null;
                    foo = () =>
                    {
                        if (this.Is_Call_MstID_Changed != true)
                        {
                            if (string.IsNullOrEmpty(txtNm.Text.Trim()))
                            {
                                txtID.Text = "0";
                            }
                        }
                    };
                    Dispatcher.BeginInvoke(foo);

                    Action foo2 = null;
                    foo2 = () =>
                    {
                        System.Threading.Thread.Sleep(100);
                        if (txtID.Text == "0")
                        {
                            txtID.Text = beforeValue;
                            txtID.OnFormatString();
                        }
                    };
                    Dispatcher.BeginInvoke(foo2);

                }
            }
        }

        private void txtID_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        #endregion

        #region Method

        #region Parent DataPropety Change Events

        public void MstID_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (this.Is_MstID_Changed == false) return;

            string _prop = e.PropertyName;
            if (_prop == null) _prop = "";
            if (_prop.Length > 0)
            {
                if (_prop.Substring(0, 1) == "_")
                {
                    _prop = _prop.Substring(1, _prop.Length - 1);
                }
            }

            MstData _mstData = new MstData();
            int mstgrp = 0;
            string _id = "";

            switch (_prop)
            {
                case "purchase_id":
                    if (this.MstKbn == MstData.geMDataKbn.Purchase)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Purchase, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;

                case "purchase_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Purchase_F)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Purchase_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;

                case "purchase_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Purchase_T)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Purchase_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;

                case "customer_id":
                    if (this.MstKbn == MstData.geMDataKbn.Customer)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Customer, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                        if (this.utlSupplier != null)
                        {
                            _mstData.GetMData(MstData.geMDataKbn.Supplier, new string[] { (utlSupplier.txtID.Text.Trim() != "" ? utlSupplier.txtID.Text.Trim() : "0"), 
                                                                                          (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, utlSupplier);
                        }
                    }
                    break;
                case "customer_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Customer_F)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Customer_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;
                case "customer_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Customer_T)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Customer_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;
                case "invoice_id":
                    if (this.MstKbn == MstData.geMDataKbn.Invoice)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Customer, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;
                case "invoice_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Invoice_F)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Customer_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;
                case "invoice_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Invoice_T)
                    {
                        if (this.txtID.Text.Trim() != "")
                        {
                            this.txtNm.Text = "";
                        }
                        _mstData.GetMData(MstData.geMDataKbn.Customer_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;
                case "supplier_id":
                    if (this.MstKbn == MstData.geMDataKbn.Supplier)
                    {
                        _mstData.GetMData(MstData.geMDataKbn.Supplier, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0"), 
                                                                                        (this.txtID2.Text.Trim() != "" ? this.txtID2.Text.Trim() : "0") }, this);
                    }
                    break;
                case "supplier_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Supplier_F)
                    {
                        _mstData.GetMData(MstData.geMDataKbn.Supplier_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0"), 
                                                                                        (this.txtID2.Text.Trim() != "" ? this.txtID2.Text.Trim() : "0") }, this);
                    }
                    break;
                case "supplier_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Supplier_T)
                    {
                        _mstData.GetMData(MstData.geMDataKbn.Supplier_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0"), 
                                                                                        (this.txtID2.Text.Trim() != "" ? this.txtID2.Text.Trim() : "0") }, this);
                    }
                    break;
                case "commodity_id":
                    if (this.MstKbn == MstData.geMDataKbn.Commodity)
                        _mstData.GetMData(MstData.geMDataKbn.Commodity, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "commodity_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Commodity_F)
                        _mstData.GetMData(MstData.geMDataKbn.Commodity_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "commodity_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Commodity_T)
                        _mstData.GetMData(MstData.geMDataKbn.Commodity_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "update_person_id":
                case "person_id":
                    if (this.MstKbn == MstData.geMDataKbn.Person)
                    {
                        _mstData.GetMData(MstData.geMDataKbn.Person, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    }
                    break;
                case "person_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Person_F)
                        _mstData.GetMData(MstData.geMDataKbn.Person_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "person_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Person_T)
                        _mstData.GetMData(MstData.geMDataKbn.Person_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "group_id":
                    if (this.MstKbn == MstData.geMDataKbn.CompanyGroup)
                        _mstData.GetMData(MstData.geMDataKbn.CompanyGroup, new string[] { this.txtID.Text.Trim() }, this);
                    break;
                case "group_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.CompanyGroup_F)
                        _mstData.GetMData(MstData.geMDataKbn.CompanyGroup_F, new string[] { this.txtID.Text.Trim() }, this);
                    break;
                case "group_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.CompanyGroup_T)
                        _mstData.GetMData(MstData.geMDataKbn.CompanyGroup_T, new string[] { this.txtID.Text.Trim() }, this);
                    break;
                case "summing_up_group_id":
                    if (this.MstKbn == MstData.geMDataKbn.Condition)
                        _mstData.GetMData(MstData.geMDataKbn.Condition, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "receipt_division_id":
                case "payment_division_id":
                    if (this.MstKbn == MstData.geMDataKbn.RecieptDivision || this.MstKbn == MstData.geMDataKbn.PaymentCahsDivision)
                        _mstData.GetMData(MstData.geMDataKbn.RecieptDivision, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "group1_id":
                    mstgrp = (int)this.MstGroupKbn;
                    _id = (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0");

                    if (this.MstKbn == MstData.geMDataKbn.Group)
                        _mstData.GetMData(MstData.geMDataKbn.Group, new string[] { mstgrp.ToString(), _id }, this);
                    break;
                case "main_purchase_id":
                    if (this.MstKbn == MstData.geMDataKbn.Purchase)
                        _mstData.GetMData(MstData.geMDataKbn.Purchase, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "condition_id_from":
                    if (this.MstKbn == MstData.geMDataKbn.Condition_F)
                        _mstData.GetMData(MstData.geMDataKbn.Condition_F, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "condition_id_to":
                    if (this.MstKbn == MstData.geMDataKbn.Condition_T)
                        _mstData.GetMData(MstData.geMDataKbn.Condition_T, new string[] { (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0") }, this);
                    break;
                case "class_id_from":
                    mstgrp = (int)this.MstGroupKbn;
                    _id = (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0");
                    
                    if (this.MstKbn == MstData.geMDataKbn.Group_F)
                        _mstData.GetMData(MstData.geMDataKbn.Group_F, new string[] { mstgrp.ToString(), _id }, this);
                    break;
                case "class_id_to":
                    mstgrp = (int)this.MstGroupKbn;
                    _id = (this.txtID.Text.Trim() != "" ? this.txtID.Text.Trim() : "0");

                    if (this.MstKbn == MstData.geMDataKbn.Group_T)
                        _mstData.GetMData(MstData.geMDataKbn.Group_T, new string[] { mstgrp.ToString(), _id }, this);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Master Name Set

        /// <summary>
        /// マスタ名称設定(ExWebServiceMstNameから非同期呼出)
        /// </summary>
        /// <param name="intKbn"></param>
        /// <param name="name"></param>
        public override void MstDataSelect(ExWebServiceMst.geWebServiceMstNmCallKbn intKbn, EntityMstData mst)
        {
            //if (this.IsLockCheck == true) Common.gblnProcLock = false;
            this.Is_Call_MstID_Changed = false;

            bool _set_flg = false;
            switch (intKbn)
            {
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer:
                    if (this.MstKbn == MstData.geMDataKbn.Customer || this.MstKbn == MstData.geMDataKbn.Invoice) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer_F:
                    if (this.MstKbn == MstData.geMDataKbn.Customer_F || this.MstKbn == MstData.geMDataKbn.Invoice_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCustomer_T:
                    if (this.MstKbn == MstData.geMDataKbn.Customer_T || this.MstKbn == MstData.geMDataKbn.Invoice_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplier:
                    if (this.MstKbn == MstData.geMDataKbn.Supplier) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplier_F:
                    if (this.MstKbn == MstData.geMDataKbn.Supplier_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetSupplier_T:
                    if (this.MstKbn == MstData.geMDataKbn.Supplier_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPerson:
                    if (this.MstKbn == MstData.geMDataKbn.Person) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPerson_F:
                    if (this.MstKbn == MstData.geMDataKbn.Person_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPerson_T:
                    if (this.MstKbn == MstData.geMDataKbn.Person_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroup:
                    if (this.MstKbn == MstData.geMDataKbn.CompanyGroup) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroup_F:
                    if (this.MstKbn == MstData.geMDataKbn.CompanyGroup_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCompanyGroup_T:
                    if (this.MstKbn == MstData.geMDataKbn.CompanyGroup_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCondition:
                    if (this.MstKbn == MstData.geMDataKbn.Condition) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCondition_F:
                    if (this.MstKbn == MstData.geMDataKbn.Condition_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCondition_T:
                    if (this.MstKbn == MstData.geMDataKbn.Condition_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetRecieptDivision:
                    if (this.MstKbn == MstData.geMDataKbn.RecieptDivision || this.MstKbn == MstData.geMDataKbn.PaymentCahsDivision) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroup:
                    if (this.MstKbn == MstData.geMDataKbn.Group) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchase:
                    if (this.MstKbn == MstData.geMDataKbn.Purchase) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchase_F:
                    if (this.MstKbn == MstData.geMDataKbn.Purchase_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetPurchase_T:
                    if (this.MstKbn == MstData.geMDataKbn.Purchase_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity:
                    if (this.MstKbn == MstData.geMDataKbn.Commodity) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity_F:
                    if (this.MstKbn == MstData.geMDataKbn.Commodity_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetCommodity_T:
                    if (this.MstKbn == MstData.geMDataKbn.Commodity_T) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroup_F:
                    if (this.MstKbn == MstData.geMDataKbn.Group_F) _set_flg = true;
                    break;
                case ExWebServiceMst.geWebServiceMstNmCallKbn.GetGroup_T:
                    if (this.MstKbn == MstData.geMDataKbn.Group_T) _set_flg = true;
                    break;
            }

            if (_set_flg == true)
            {
                if (mst == null)
                {
                    this.txtNm.Text = "";
                    this.MasterData = null;
                }
                else
                {
                    this.txtNm.Text = mst.name;
                    this.MasterData = mst;
                    ParentDataSet();
                }
            }

            switch (Common.gWinMsterType)
            {
                case Common.geWinMsterType.Supplier:
                    DependencyObject obj = ExVisualTreeHelper.FindPerentUserControl(this);
                    if (obj == null) return;

                    Utl_MstSupplier utl = null;
                    try
                    {
                        utl = (Utl_MstSupplier)obj;
                        utl.MstDataSelect(intKbn, mst);
                    }
                    catch
                    {
                    }
                    break;
            }

            Common.gblnDesynchronizeLock = false;
        }

        #endregion

        #region Parent Data Set

        private void ParentDataSet()
        {
            if (this.ParentData == null) return;

            switch (Common.gPageType)
            {
                #region 見積入力

                case Common.gePageType.InpEstimate:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Customer:
                            EntityEstimateH orderEstimateH = null;
                            try
                            {
                                orderEstimateH = (EntityEstimateH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }

                            Utl_InpEstimate utlEstimate = (Utl_InpEstimate)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlEstimate != null)
                            {
                                utlEstimate.utlBusiness.txtID.Text = ExCast.zCInt(this.MasterData.attribute1).ToString();
                                utlEstimate.utlTax.txtID.Text = ExCast.zCInt(this.MasterData.attribute2).ToString();
                            }

                            orderEstimateH._business_division_id = ExCast.zCInt(this.MasterData.attribute1);
                            orderEstimateH._tax_change_id = ExCast.zCInt(this.MasterData.attribute2);
                            orderEstimateH._price_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute3);
                            orderEstimateH._tax_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute4);
                            orderEstimateH._unit_kind_id = ExCast.zCInt(this.MasterData.attribute5);
                            //orderH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute6);
                            //orderH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute7);
                            orderEstimateH._invoice_id = ExCast.zNumZeroNothingFormat(this.MasterData.attribute8);
                            orderEstimateH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute17);
                            orderEstimateH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderEstimateH._credit_rate = ExCast.zCInt(this.MasterData.attribute19);
                            this.ParentData = orderEstimateH;
                            break;
                    }
                    break;

                #endregion

                #region 受注入力

                case Common.gePageType.InpOrder:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Customer:
                            EntityOrderH orderH = null;
                            try
                            {
                                orderH = (EntityOrderH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }

                            Utl_InpOrder utlOrder = (Utl_InpOrder)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlOrder != null)
                            {
                                utlOrder.utlBusiness.txtID.Text = ExCast.zCInt(this.MasterData.attribute1).ToString();
                                utlOrder.utlTax.txtID.Text = ExCast.zCInt(this.MasterData.attribute2).ToString();
                            }

                            orderH._business_division_id = ExCast.zCInt(this.MasterData.attribute1);
                            orderH._tax_change_id = ExCast.zCInt(this.MasterData.attribute2);
                            orderH._price_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute3);
                            orderH._tax_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute4);
                            orderH._unit_kind_id = ExCast.zCInt(this.MasterData.attribute5);
                            //orderH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute6);
                            //orderH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute7);
                            orderH._invoice_id = ExCast.zNumZeroNothingFormat(this.MasterData.attribute8);
                            orderH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute17);
                            orderH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderH._credit_rate = ExCast.zCInt(this.MasterData.attribute19);
                            this.ParentData = orderH;
                            break;
                    }
                    break;

                #endregion

                #region 売上入力

                case Common.gePageType.InpSales:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Customer:
                            EntitySalesH orderSalesH = null;
                            try
                            {
                                orderSalesH = (EntitySalesH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }

                            Utl_InpSales utlSales = (Utl_InpSales)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlSales != null)
                            {
                                utlSales.utlBusiness.txtID.Text = ExCast.zCInt(this.MasterData.attribute1).ToString();
                                utlSales.utlTax.txtID.Text = ExCast.zCInt(this.MasterData.attribute2).ToString();
                                utlSales.utlUnitKind.txtID.Text = ExCast.zCInt(this.MasterData.attribute5).ToString();
                                utlSales.utlInvoice.txtID.Text = ExCast.zFormatForID(this.MasterData.attribute8, Common.gintidFigureCustomer);
                            }

                            orderSalesH._business_division_id = ExCast.zCInt(this.MasterData.attribute1);
                            orderSalesH._tax_change_id = ExCast.zCInt(this.MasterData.attribute2);
                            orderSalesH._price_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute3);
                            orderSalesH._tax_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute4);
                            orderSalesH._unit_kind_id = ExCast.zCInt(this.MasterData.attribute5);
                            //orderSalesH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute6);
                            //orderSalesH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute7);
                            orderSalesH._invoice_id = ExCast.zNumZeroNothingFormat(this.MasterData.attribute8);
                            orderSalesH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute17);
                            orderSalesH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderSalesH._before_sales_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderSalesH._invoice_id = ExCast.zFormatForID(this.MasterData.attribute8, Common.gintidFigureCustomer);
                            orderSalesH._invoice_name = ExCast.zCStr(this.MasterData.attribute9);
                            orderSalesH._credit_rate = ExCast.zCInt(this.MasterData.attribute19);
                            this.ParentData = orderSalesH;
                            break;
                    }
                    break;

                #endregion

                #region 入金入力

                case Common.gePageType.InpReceipt:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Invoice:
                            EntityReceiptH orderReceiptH = null;
                            try
                            {
                                orderReceiptH = (EntityReceiptH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }
                            orderReceiptH._credit_price = ExCast.zCDbl(this.MasterData.attribute7);
                            orderReceiptH._before_credit_price = ExCast.zCDbl(this.MasterData.attribute7);

                            Utl_InpReceipt utlReceipt = (Utl_InpReceipt)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlReceipt != null)
                            {
                                utlReceipt.utlSummingUp.txtID.Text = ExCast.zCStr(this.MasterData.attribute10);
                                utlReceipt.utlReceiptDivision.txtID.Text = ExCast.zCStr(this.MasterData.attribute11);
                                utlReceipt.DetailSumPrice();
                            }
                            orderReceiptH._summing_up_group_id = ExCast.zCStr(this.MasterData.attribute10);
                            orderReceiptH._receipt_division_id = ExCast.zCStr(this.MasterData.attribute11);
                            this.ParentData = orderReceiptH;
                            break;
                    }
                    break;

                #endregion

                #region 発注入力

                case Common.gePageType.InpPurchaseOrder:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Purchase:
                            EntityPurchaseOrderH orderPurchaseOrderH = null;
                            try
                            {
                                orderPurchaseOrderH = (EntityPurchaseOrderH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }

                            Utl_InpPurchaseOrder utlPurchaseOrder = (Utl_InpPurchaseOrder)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlPurchaseOrder != null)
                            {
                                utlPurchaseOrder.utlBusiness.txtID.Text = ExCast.zCInt(this.MasterData.attribute1).ToString();
                                utlPurchaseOrder.utlTax.txtID.Text = ExCast.zCInt(this.MasterData.attribute2).ToString();
                            }

                            orderPurchaseOrderH._business_division_id = ExCast.zCInt(this.MasterData.attribute1);
                            orderPurchaseOrderH._tax_change_id = ExCast.zCInt(this.MasterData.attribute2);
                            orderPurchaseOrderH._price_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute3);
                            orderPurchaseOrderH._tax_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute4);
                            orderPurchaseOrderH._unit_kind_id = ExCast.zCInt(this.MasterData.attribute5);
                            orderPurchaseOrderH._payment_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderPurchaseOrderH._before_payment_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderPurchaseOrderH._credit_rate = ExCast.zCInt(this.MasterData.attribute19);
                            this.ParentData = orderPurchaseOrderH;
                            break;
                    }
                    break;

                #endregion

                #region 発注入力

                case Common.gePageType.InpPurchase:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Purchase:
                            EntityPurchaseH orderPurchaseH = null;
                            try
                            {
                                orderPurchaseH = (EntityPurchaseH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }

                            Utl_InpPurchase utlPurchase = (Utl_InpPurchase)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlPurchase != null)
                            {
                                utlPurchase.utlBusiness.txtID.Text = ExCast.zCInt(this.MasterData.attribute1).ToString();
                                utlPurchase.utlTax.txtID.Text = ExCast.zCInt(this.MasterData.attribute2).ToString();
                                utlPurchase.utlUnitKind.txtID.Text = ExCast.zCInt(this.MasterData.attribute5).ToString();
                            }

                            orderPurchaseH._business_division_id = ExCast.zCInt(this.MasterData.attribute1);
                            orderPurchaseH._tax_change_id = ExCast.zCInt(this.MasterData.attribute2);
                            orderPurchaseH._price_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute3);
                            orderPurchaseH._tax_fraction_proc_id = ExCast.zCInt(this.MasterData.attribute4);
                            orderPurchaseH._unit_kind_id = ExCast.zCInt(this.MasterData.attribute5);
                            //orderSalesH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute6);
                            //orderSalesH._sales_credit_price = ExCast.zCDbl(this.MasterData.attribute7);
                            //orderPurchaseH._invoice_id = ExCast.zNumZeroNothingFormat(this.MasterData.attribute8);
                            //orderPurchaseH._credit_limit_price = ExCast.zCDbl(this.MasterData.attribute17);
                            orderPurchaseH._payment_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            orderPurchaseH._before_payment_credit_price = ExCast.zCDbl(this.MasterData.attribute18);
                            //orderPurchaseH._invoice_id = ExCast.zFormatForID(this.MasterData.attribute8, Common.gintidFigureCustomer);
                            //orderPurchaseH._invoice_name = ExCast.zCStr(this.MasterData.attribute9);
                            orderPurchaseH._credit_rate = ExCast.zCInt(this.MasterData.attribute19);
                            this.ParentData = orderPurchaseH;
                            break;
                    }
                    break;

                #endregion

                #region 出金入力

                case Common.gePageType.InpPaymentCash:
                    switch (this.MstKbn)
                    {
                        case MstData.geMDataKbn.Purchase:
                            EntityPaymentCashH orderPaymentCashH = null;
                            try
                            {
                                orderPaymentCashH = (EntityPaymentCashH)this.ParentData;
                            }
                            catch
                            {
                                return;
                            }
                            orderPaymentCashH._credit_price = ExCast.zCDbl(this.MasterData.attribute7);
                            orderPaymentCashH._before_credit_price = ExCast.zCDbl(this.MasterData.attribute7);

                            Utl_InpPaymentCash utlPaymentCash = (Utl_InpPaymentCash)ExVisualTreeHelper.FindPerentUserControl(this);
                            if (utlPaymentCash != null)
                            {
                                utlPaymentCash.utlSummingUp.txtID.Text = ExCast.zCStr(this.MasterData.attribute10);
                                utlPaymentCash.utlPaymentDivision.txtID.Text = ExCast.zCStr(this.MasterData.attribute11);
                                utlPaymentCash.DetailSumPrice();
                            }
                            orderPaymentCashH._summing_up_group_id = ExCast.zCStr(this.MasterData.attribute10);
                            orderPaymentCashH._payment_division_id = ExCast.zCStr(this.MasterData.attribute11);
                            this.ParentData = orderPaymentCashH;
                            break;
                    }
                    break;

                #endregion
            }
        }

        #endregion

        public void ShowList()
        {
            txtID_MouseDoubleClick(this.txtID, null);
        }

        #endregion

    }
}
