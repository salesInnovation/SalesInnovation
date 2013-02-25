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

namespace SlvHanbaiClient.Class.Entity
{
    public class EntityDataFormOrderD
    {

        private long _id = 0;
        public long id
        {
            set
            {
                this._id = value;
            }
            get { return this._id; }
        }

        private int _rec_no = 0;
        public int rec_no
        {
            set
            {
                this._rec_no = value;
            }
            get { return this._rec_no; }
        }

        private int _breakdown_id = 0;
        public int breakdown_id
        {
            set
            {
                this._breakdown_id = value;
            }
            get { return this._breakdown_id; }
        }

        private string _breakdown_nm;
        public string breakdown_nm
        {
            set
            {
                this._breakdown_nm = value;
            }
            get { return this._breakdown_nm; }
        }

        private int _deliver_division_id = 0;
        public int deliver_division_id
        {
            set
            {
                this._deliver_division_id = value;
            }
            get { return this._deliver_division_id; }
        }

        private string _deliver_division_nm;
        public string deliver_division_nm
        {
            set
            {
                this._deliver_division_nm = value;
            }
            get { return this._deliver_division_nm; }
        }

        private string _commodity_id;
        public string commodity_id
        {
            set
            {
                this._commodity_id = value;
            }
            get { return this._commodity_id; }
        }

        private string _commodity_name;
        public string commodity_name
        {
            set
            {
                this._commodity_name = value;
            }
            get { return this._commodity_name; }
        }

        private int _unit_id = 0;
        public int unit_id
        {
            set
            {
                this._unit_id = value;
            }
            get { return this._unit_id; }
        }

        private string _unit_nm;
        public string unit_nm
        {
            set
            {
                this._unit_nm = value;
            }
            get { return this._unit_nm; }
        }

        private double _enter_number = 0;
        public double enter_number
        {
            set
            {
                this._enter_number = value;
            }
            get { return this._enter_number; }
        }

        private double _case_number;
        public double case_number
        {
            set
            {
                this._case_number = value;
            }
            get { return this._case_number; }
        }

        private double _number = 0;
        public double number
        {
            set
            {
                this._number = value;
            }
            get { return this._number; }
        }

        private double _unit_price = 0;
        public double unit_price
        {
            set
            {
                this._unit_price = value;
            }
            get { return this._unit_price; }
        }

        private double _sales_cost;
        public double sales_cost
        {
            set
            {
                this._sales_cost = value;
            }
            get { return this._sales_cost; }
        }

        private double _tax;
        public double tax
        {
            set
            {
                this._tax = value;
            }
            get { return this._tax; }
        }

        private double _no_tax_price;
        public double no_tax_price
        {
            set
            {
                this._no_tax_price = value;
            }
            get { return this._no_tax_price; }
        }

        private double _price = 0;
        public double price
        {
            set
            {
                this._price = value;
            }
            get { return this._price; }
        }

        private double _profits;
        public double profits
        {
            set
            {
                this._profits = value;
            }
            get { return this._profits; }
        }

        private double _profits_percent;
        public double profits_percent
        {
            set
            {
                this._profits_percent = value;
            }
            get { return this._profits_percent; }
        }

        private int _tax_division_id;
        public int tax_division_id
        {
            set
            {
                this._tax_division_id = value;
            }
            get { return this._tax_division_id; }
        }

        private string _tax_division_nm;
        public string tax_division_nm
        {
            set
            {
                this._tax_division_nm = value;
            }
            get { return this._tax_division_nm; }
        }

        private int _tax_percent;
        public int tax_percent
        {
            set
            {
                this._tax_percent = value;
            }
            get { return this._tax_percent; }
        }

        // 在庫管理区分
        private int _inventory_management_division_id;
        public int inventory_management_division_id
        {
            set
            {
                this._inventory_management_division_id = value;
            }
            get { return this._inventory_management_division_id; }
        }

        // 現在庫
        private double _inventory_number;
        public double inventory_number
        {
            set
            {
                this._inventory_number = value;
            }
            get { return this._inventory_number; }
        }

        // 上代税抜
        private double _retail_price_skip_tax;
        public double retail_price_skip_tax
        {
            set
            {
                this._retail_price_skip_tax = value;
            }
            get { return this._retail_price_skip_tax; }
        }

        // 上代税込
        private double _retail_price_before_tax;
        public double retail_price_before_tax
        {
            set
            {
                this._retail_price_before_tax = value;
            }
            get { return this._retail_price_before_tax; }
        }

        // 売上単価税抜
        private double _sales_unit_price_skip_tax;
        public double sales_unit_price_skip_tax
        {
            set
            {
                this._sales_unit_price_skip_tax = value;
            }
            get { return this._sales_unit_price_skip_tax; }
        }

        // 売上単価税込
        private double _sales_unit_price_before_tax;
        public double sales_unit_price_before_tax
        {
            set
            {
                this._sales_unit_price_before_tax = value;
            }
            get { return this._sales_unit_price_before_tax; }
        }

        // 売上原価税抜
        private double _sales_cost_price_skip_tax;
        public double sales_cost_price_skip_tax
        {
            set
            {
                this._sales_cost_price_skip_tax = value;
            }
            get { return this._sales_cost_price_skip_tax; }
        }

        // 売上原価税込
        private double _sales_cost_price_before_tax;
        public double sales_cost_price_before_tax
        {
            set
            {
                this._sales_cost_price_before_tax = value;
            }
            get { return this._sales_cost_price_before_tax; }
        }

        // 仕入単価税抜
        private double _purchase_unit_price_skip_tax;
        public double purchase_unit_price_skip_tax
        {
            set
            {
                this._purchase_unit_price_skip_tax = value;
            }
            get { return this._purchase_unit_price_skip_tax; }
        }

        // 仕入単価税込
        private double _purchase_unit_price_before_tax;
        public double purchase_unit_price_before_tax
        {
            set
            {
                this._purchase_unit_price_before_tax = value;
            }
            get { return this._purchase_unit_price_before_tax; }
        }

        // 数量小数桁
        private int _number_decimal_digit;
        public int number_decimal_digit
        {
            set
            {
                this._number_decimal_digit = value;
            }
            get { return this._number_decimal_digit; }
        }

        // 単価小数桁
        private int _unit_decimal_digit;
        public int unit_decimal_digit
        {
            set
            {
                this._unit_decimal_digit = value;
            }
            get { return this._unit_decimal_digit; }
        }

        // 受注ID
        private double _order_id;
        public double order_id
        {
            set
            {
                this._order_id = value;
            }
            get { return this._order_id; }
        }

        // 受注数
        private double _order_number;
        public double order_number
        {
            set
            {
                this._order_number = value;
            }
            get { return this._order_number; }
        }


        // 受注残数
        private double _order_stay_number;
        public double order_stay_number
        {
            set
            {
                this._order_stay_number = value;
            }
            get { return this._order_stay_number; }
        }

        private string _memo;
        public string memo
        {
            set
            {
                this._memo = value;
            }
            get { return this._memo; }
        }

        public EntityDataFormOrderD(long id
                                  , int rec_no
                                  , int breakdown_id
                                  , string breakdown_nm
                                  , int deliver_division_id
                                  , string deliver_division_nm
                                  , string commodity_id
                                  , string commodity_name
                                  , int unit_id
                                  , string unit_nm
                                  , double enter_number
                                  , double case_number
                                  , double number
                                  , double unit_price
                                  , double salse_cost
                                  , double tax
                                  , double no_tax_price
                                  , double price
                                  , double profits
                                  , double profits_percent
                                  , string memo
                                  , int tax_division_id
                                  , string tax_division_nm
                                  , int tax_percent
                                  , int inventory_management_division_id
                                  , double inventory_number
                                  , double retail_price_skip_tax
                                  , double retail_price_before_tax
                                  , double sales_unit_price_skip_tax
                                  , double sales_unit_price_before_tax
                                  , double sales_cost_price_skip_tax
                                  , double sales_cost_price_before_tax
                                  , int number_decimal_digit
                                  , int unit_decimal_digit
                                  , double order_id
                                  , double order_number
                                  , double order_stay_number
            )
        {
            this.id = id;
            this.rec_no = rec_no;
            this.breakdown_id = breakdown_id;
            this.breakdown_nm = breakdown_nm;
            this.deliver_division_id = deliver_division_id;
            this.deliver_division_nm = deliver_division_nm;
            this.commodity_id = commodity_id;
            this.commodity_name = commodity_name;
            this.unit_id = unit_id;
            this.unit_nm = unit_nm;
            this.enter_number = enter_number;
            this.case_number = case_number;
            this.number = number;
            this.unit_price = unit_price;
            this.sales_cost = salse_cost;
            this.tax = tax;
            this.no_tax_price = no_tax_price;
            this.price = price;
            this.profits = profits;
            this.profits_percent = profits_percent;
            this.memo = memo;
            this.tax_division_id = tax_division_id;
            this.tax_division_nm = tax_division_nm;
            this.tax_percent = tax_percent;
            this.inventory_management_division_id = inventory_management_division_id;
            this.inventory_number = inventory_number;
            this.retail_price_skip_tax = retail_price_skip_tax;
            this.retail_price_before_tax = retail_price_before_tax;
            this.sales_unit_price_skip_tax = sales_unit_price_skip_tax;
            this.sales_unit_price_before_tax = sales_unit_price_before_tax;
            this.sales_cost_price_skip_tax = sales_cost_price_skip_tax;
            this.sales_cost_price_before_tax = sales_cost_price_before_tax;
            this.number_decimal_digit = number_decimal_digit;
            this.unit_decimal_digit = unit_decimal_digit;
            this.order_id = order_id;
            this.order_number = order_number;
            this.order_stay_number = order_stay_number;
        }

        public EntityDataFormOrderD()
        {
        }
    }
}
