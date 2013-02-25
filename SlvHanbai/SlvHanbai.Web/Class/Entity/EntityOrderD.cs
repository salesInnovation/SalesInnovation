#region using

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

#endregion

namespace SlvHanbai.Web.Class.Entity
{
    [DataContract]
    public class EntityOrderD : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private long _id;
        public long id
        { 
            set 
            {
                this._id = value;
                base.PropertyChangedHandler(this, "id");
            }
            get { return this._id; } 
        }

        [DataMember()]
        private int _rec_no;
        public int rec_no
        { 
            set 
            {
                this._rec_no = value;
                base.PropertyChangedHandler(this, "rec_no");
            }
            get { return this._rec_no; } 
        }

        [DataMember()]
        private int _breakdown_id;
        public int breakdown_id
        { 
            set 
            {
                this._breakdown_id = value;
                base.PropertyChangedHandler(this, "breakdown_id");
            }
            get { return this._breakdown_id; } 
        }

        [DataMember()]
        private string _breakdown_nm;
        public string breakdown_nm
        { 
            set 
            {
                this._breakdown_nm = value;
                base.PropertyChangedHandler(this, "breakdown_nm");
            }
            get { return this._breakdown_nm; } 
        }

        [DataMember()]
        private int _deliver_division_id;
        public int deliver_division_id
        { 
            set 
            {
                this._deliver_division_id = value;
                base.PropertyChangedHandler(this, "deliver_division_id");
            }
            get { return this._deliver_division_id; } 
        }

        [DataMember()]
        private string _deliver_division_nm;
        public string deliver_division_nm
        { 
            set 
            {
                this._deliver_division_nm = value;
                base.PropertyChangedHandler(this, "deliver_division_nm");
            }
            get { return this._deliver_division_nm; } 
        }

        [DataMember()]
        private string _commodity_id;
        public string commodity_id
        { 
            set 
            {
                this._commodity_id = value;
                base.PropertyChangedHandler(this, "commodity_id");
            }
            get { return this._commodity_id; } 
        }

        [DataMember()]
        private string _commodity_name;
        public string commodity_name
        { 
            set 
            {
                this._commodity_name = value;
                base.PropertyChangedHandler(this, "commodity_name");
            }
            get { return this._commodity_name; } 
        }

        [DataMember()]
        private int _unit_id;
        public int unit_id
        { 
            set 
            {
                this._unit_id = value;
                base.PropertyChangedHandler(this, "unit_id");
            }
            get { return this._unit_id; } 
        }

        [DataMember()]
        private string _unit_nm;
        public string unit_nm
        { 
            set 
            {
                this._unit_nm = value;
                base.PropertyChangedHandler(this, "unit_nm");
            }
            get { return this._unit_nm; } 
        }

        [DataMember()]
        private double _enter_number;
        public double enter_number
        { 
            set 
            {
                this._enter_number = value;
                base.PropertyChangedHandler(this, "enter_number");
            }
            get { return this._enter_number; } 
        }

        [DataMember()]
        private double _case_number;
        public double case_number
        {
            set
            {
                this._case_number = value;
                base.PropertyChangedHandler(this, "case_number");
            }
            get { return this._case_number; }
        }

        [DataMember()]
        private double _number;
        public double number
        { 
            set 
            {
                this._number = value;
                base.PropertyChangedHandler(this, "number");
            }
            get { return this._number; } 
        }

        [DataMember()]
        private double _unit_price;
        public double unit_price
        { 
            set 
            {
                this._unit_price = value;
                base.PropertyChangedHandler(this, "unit_price");
            }
            get { return this._unit_price; } 
        }

        [DataMember()]
        private double _sales_cost;
        public double sales_cost
        {
            set
            {
                this._sales_cost = value;
                base.PropertyChangedHandler(this, "sales_cost");
            }
            get { return this._sales_cost; }
        }

        [DataMember()]
        private double _tax;
        public double tax
        {
            set
            {
                this._tax = value;
                base.PropertyChangedHandler(this, "tax");
            }
            get { return this._tax; }
        }

        [DataMember()]
        private double _no_tax_price;
        public double no_tax_price
        {
            set
            {
                this._no_tax_price = value;
                base.PropertyChangedHandler(this, "no_tax_price");
            }
            get { return this._no_tax_price; }
        }

        [DataMember()]
        private double _price;
        public double price
        { 
            set 
            {
                this._price = value;
                base.PropertyChangedHandler(this, "price");
            }
            get { return this._price; } 
        }

        [DataMember()]
        private double _profits;
        public double profits
        {
            set
            {
                this._profits = value;
                base.PropertyChangedHandler(this, "profits");
            }
            get { return this._profits; }
        }

        [DataMember()]
        private double _profits_percent = 0;
        public double profits_percent
        {
            set
            {
                this._profits_percent = value;
                base.PropertyChangedHandler(this, "profits_percent");
            }
            get { return this._profits_percent; }
        }

        [DataMember()]
        private int _tax_division_id;
        public int tax_division_id
        {
            set
            {
                this._tax_division_id = value;
                base.PropertyChangedHandler(this, "tax_division_id");
            }
            get { return this._tax_division_id; }
        }

        [DataMember()]
        private string _tax_division_nm;
        public string tax_division_nm
        {
            set
            {
                this._tax_division_nm = value;
                base.PropertyChangedHandler(this, "tax_division_nm");
            }
            get { return this._tax_division_nm; }
        }

        [DataMember()]
        private int _tax_percent;
        public int tax_percent
        {
            set
            {
                this._tax_percent = value;
                base.PropertyChangedHandler(this, "tax_percent");
            }
            get { return this._tax_percent; }
        }

        // 在庫管理区分
        [DataMember()]
        private int _inventory_management_division_id;
        public int inventory_management_division_id
        {
            set
            {
                this._inventory_management_division_id = value;
                base.PropertyChangedHandler(this, "inventory_management_division_id");
            }
            get { return this._inventory_management_division_id; }
        }

        // 現在庫
        [DataMember()]
        private double _inventory_number;
        public double inventory_number
        {
            set
            {
                this._inventory_number = value;
                base.PropertyChangedHandler(this, "inventory_number");
            }
            get { return this._inventory_number; }
        }

        // 上代税抜
        [DataMember()]
        private double _retail_price_skip_tax;
        public double retail_price_skip_tax
        {
            set
            {
                this._retail_price_skip_tax = value;
                base.PropertyChangedHandler(this, "retail_price_skip_tax");
            }
            get { return this._retail_price_skip_tax; }
        }

        // 上代税込
        [DataMember()]
        private double _retail_price_before_tax;
        public double retail_price_before_tax
        {
            set
            {
                this._retail_price_before_tax = value;
                base.PropertyChangedHandler(this, "retail_price_before_tax");
            }
            get { return this._retail_price_before_tax; }
        }

        // 売上単価税抜
        [DataMember()]
        private double _sales_unit_price_skip_tax;
        public double sales_unit_price_skip_tax
        {
            set
            {
                this._sales_unit_price_skip_tax = value;
                base.PropertyChangedHandler(this, "sales_unit_price_skip_tax");
            }
            get { return this._sales_unit_price_skip_tax; }
        }

        // 売上単価税込
        [DataMember()]
        private double _sales_unit_price_before_tax;
        public double sales_unit_price_before_tax
        {
            set
            {
                this._sales_unit_price_before_tax = value;
                base.PropertyChangedHandler(this, "sales_unit_price_before_tax");
            }
            get { return this._sales_unit_price_before_tax; }
        }

        // 売上原価税抜
        [DataMember()]
        private double _sales_cost_price_skip_tax;
        public double sales_cost_price_skip_tax
        {
            set
            {
                this._sales_cost_price_skip_tax = value;
                base.PropertyChangedHandler(this, "sales_cost_price_skip_tax");
            }
            get { return this._sales_cost_price_skip_tax; }
        }

        // 売上原価税込
        [DataMember()]
        private double _sales_cost_price_before_tax;
        public double sales_cost_price_before_tax
        {
            set
            {
                this._sales_cost_price_before_tax = value;
                base.PropertyChangedHandler(this, "sales_cost_price_before_tax");
            }
            get { return this._sales_cost_price_before_tax; }
        }

        // 数量小数桁
        [DataMember()]
        private int _number_decimal_digit;
        public int number_decimal_digit
        {
            set
            {
                this._number_decimal_digit = value;
                base.PropertyChangedHandler(this, "number_decimal_digit");
            }
            get { return this._number_decimal_digit; }
        }

        // 単価小数桁
        [DataMember()]
        private int _unit_decimal_digit;
        public int unit_decimal_digit
        {
            set
            {
                this._unit_decimal_digit = value;
                base.PropertyChangedHandler(this, "unit_decimal_digit");
            }
            get { return this._unit_decimal_digit; }
        }

        // 受注数
        [DataMember()]
        private double _order_number;
        public double order_number
        {
            set
            {
                this._order_number = value;
                base.PropertyChangedHandler(this, "order_number");
            }
            get { return this._order_number; }
        }


        // 受注残数
        [DataMember()]
        private double _order_stay_number;
        public double order_stay_number
        {
            set
            {
                this._order_stay_number = value;
                base.PropertyChangedHandler(this, "order_stay_number");
            }
            get { return this._order_stay_number; }
        }

        [DataMember()]
        private string _memo;
        public string memo
        { 
            set 
            {
                this._memo = value;
                base.PropertyChangedHandler(this, "memo");
            }
            get { return this._memo; } 
        }

        [DataMember()]
        private int _lock_flg = 0;
        public int lock_flg
        {
            set
            {
                this._lock_flg = value;
                base.PropertyChangedHandler(this, "lock_flg");
            }
            get
            {
                return this._lock_flg;
            }
        }

        [DataMember()]
        private string _message;
        public string message
        {
            set
            {
                this._message = value;
                base.PropertyChangedHandler(this, "message");
            }
            get { return this._message; }
        }

        public EntityOrderD()
        {
        }
    }
}