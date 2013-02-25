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
using System.Collections.ObjectModel;
using SlvHanbaiClient.Class;
using SlvHanbaiClient.Class.UI;
using SlvHanbaiClient.Class.Data;
using SlvHanbaiClient.Class.WebService;
using SlvHanbaiClient.Class.Utility;
using SlvHanbaiClient.svcMstData;
using SlvHanbaiClient.View.Dlg;
using SlvHanbaiClient.View.UserControl.Custom;

#endregion

namespace SlvHanbaiClient.View.Dlg
{
    public partial class Dlg_UnitPriceSetting : ExChildWindow
    {

        #region Filed Const

        private const String CLASS_NM = "Dlg_UnitPriceSetting";

        public double return_unit_price = 0;        

        public double retail_price = 0;
        public double sales_unit_price = 0;
        public double sales_cost_price = 0;
        public double credit_rate = 0;
        public int unit_decimal_digit = 0;

        private List<EntityUnitPriceSetting> _lstUnit = new List<EntityUnitPriceSetting>();

        public enum eKbn { Sales = 0, Purchase };
        public eKbn kbn = eKbn.Sales;

        #endregion

        #region Constructor

        public Dlg_UnitPriceSetting()
        {
            InitializeComponent();
            this.Tag = "Main";      // ファンクションキーを受けつけ用
        }

        #endregion

        #region Page Events

        private void ExChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 画面初期化
            ExVisualTreeHelper.initDisplay(this.LayoutRoot);

            EntityUnitPriceSetting _entity = null;

            switch (this.kbn)
            {
                case eKbn.Sales:
                    _entity = new EntityUnitPriceSetting();
                    _entity.unit_kind_nm = "上代";
                    _entity.unit_price = this.retail_price;
                    _lstUnit.Add(_entity);
                    _entity = new EntityUnitPriceSetting();
                    _entity.unit_kind_nm = "売上単価";
                    _entity.unit_price = this.sales_unit_price;
                    _lstUnit.Add(_entity);
                    _entity = new EntityUnitPriceSetting();
                    _entity.unit_kind_nm = "売上原価";
                    _entity.unit_price = this.sales_cost_price;
                    _lstUnit.Add(_entity);
                    break;
                case eKbn.Purchase:
                    _entity = new EntityUnitPriceSetting();
                    _entity.unit_kind_nm = "上代";
                    _entity.unit_price = this.retail_price;
                    _lstUnit.Add(_entity);
                    _entity = new EntityUnitPriceSetting();
                    _entity.unit_kind_nm = "仕入単価";
                    _entity.unit_price = this.sales_unit_price;
                    _lstUnit.Add(_entity);
                    _entity = new EntityUnitPriceSetting();
                    _entity.unit_kind_nm = "売上原価";
                    _entity.unit_price = this.sales_cost_price;
                    _lstUnit.Add(_entity);
                    break;
            }
            this.dg.ItemsSource = _lstUnit;
            this.dg.SelectedIndex = 0;

            this.numUpCreditRate.SetValue(this.credit_rate);
            //this.numUpCreditRate.Value = this.credit_rate;

            this.txtUnitPrice.Text = ExCast.zCStr(ExMath.zFloor(this.retail_price * ExCast.zCInt(this.numUpCreditRate.Value) / 100, this.unit_decimal_digit));
            this.txtUnitPrice.OnFormatString();
        }

        private void ExChildWindow_Closed(object sender, EventArgs e)
        {
        }

        #endregion

        #region LostFocus Events

        #endregion

        #region Text Events

        #endregion

        #region Function Key Button Method

        // F1ボタン(OK) クリック
        public override void btnF1_Click(object sender, RoutedEventArgs e)
        {
            this.return_unit_price = ExCast.zCDbl(this.txtUnitPrice.Text);
            this.DialogResult = true;
        }

        // F12ボタン(キャンセル) クリック
        public override void btnF12_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #endregion

        #region DataGrid Events

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcUnitPrice();
        }

        #endregion

        #region CreditRate Change

        private void numUpCreditRate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CalcUnitPrice();
        }

        #endregion

        #region Method

        private void CalcUnitPrice()
        {
            if (this.dg == null) return;
            if (this.dg.SelectedIndex == -1) return;

            switch (this.dg.SelectedIndex)
            {
                case 0:     // 上代
                    this.txtUnitPrice.Text = ExCast.zCStr(ExMath.zFloor(this.retail_price * ExCast.zCInt(this.numUpCreditRate.Value) / 100, this.unit_decimal_digit));
                    break;
                case 1:     // 売上単価
                    this.txtUnitPrice.Text = ExCast.zCStr(ExMath.zFloor(this.sales_unit_price * ExCast.zCInt(this.numUpCreditRate.Value) / 100, this.unit_decimal_digit));
                    break;
                case 2:     // 売上原価
                    this.txtUnitPrice.Text = ExCast.zCStr(ExMath.zFloor(this.sales_cost_price * ExCast.zCInt(this.numUpCreditRate.Value) / 100, this.unit_decimal_digit));
                    break;
            }
        }

        #endregion

        public class EntityUnitPriceSetting
        {
            private string _unit_kind_nm = "";
            public string unit_kind_nm { set { this._unit_kind_nm = value; } get { return this._unit_kind_nm; } }

            private double _unit_price = 0;
            public double unit_price { set { this._unit_price = value; } get { return this._unit_price; } }

            public EntityUnitPriceSetting()
            {
            }
        }

    }
}

