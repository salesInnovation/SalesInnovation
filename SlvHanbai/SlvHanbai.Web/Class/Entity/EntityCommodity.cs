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
    public class EntityCommodity : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private string _id = "";
        public string id
        {
            set
            {
                this._id = value;
                base.PropertyChangedHandler(this, "id");
            }
            get
            {
                return this._id;
            }
        }

        [DataMember()]
        private string _name = "";
        public string name
        {
            set
            {
                this._name = value;
                base.PropertyChangedHandler(this, "name");
            }
            get
            {
                return this._name;
            }
        }

        [DataMember()]
        private string _kana = "";
        public string kana
        {
            set
            {
                this._kana = value;
                base.PropertyChangedHandler(this, "kana");
            }
            get
            {
                return this._kana;
            }
        }

        [DataMember()]
        private int _unit_id = 0;
        public int unit_id
        {
            set
            {
                this._unit_id = value;
                base.PropertyChangedHandler(this, "unit_id");
            }
            get
            {
                return this._unit_id;
            }
        }

        [DataMember()]
        private string _unit_nm = "";
        public string unit_nm
        {
            set
            {
                this._unit_nm = value;
                base.PropertyChangedHandler(this, "unit_nm");
            }
            get
            {
                return this._unit_nm;
            }
        }

        [DataMember()]
        private double _enter_number = 0;
        public double enter_number
        {
            set
            {
                this._enter_number = value;
                base.PropertyChangedHandler(this, "enter_number");
            }
            get
            {
                return this._enter_number;
            }
        }

        [DataMember()]
        private int _number_decimal_digit = 0;
        public int number_decimal_digit
        {
            set
            {
                this._number_decimal_digit = value;
                base.PropertyChangedHandler(this, "number_decimal_digit");
            }
            get
            {
                return this._number_decimal_digit;
            }
        }

        [DataMember()]
        private int _unit_decimal_digit = 0;
        public int unit_decimal_digit
        {
            set
            {
                this._unit_decimal_digit = value;
                base.PropertyChangedHandler(this, "unit_decimal_digit");
            }
            get
            {
                return this._unit_decimal_digit;
            }
        }

        [DataMember()]
        private int _taxation_divition_id = 0;
        public int taxation_divition_id
        {
            set
            {
                this._taxation_divition_id = value;
                base.PropertyChangedHandler(this, "taxation_divition_id");
            }
            get
            {
                return this._taxation_divition_id;
            }
        }

        [DataMember()]
        private string _taxation_divition_nm = "";
        public string taxation_divition_nm
        {
            set
            {
                this._taxation_divition_nm = value;
                base.PropertyChangedHandler(this, "taxation_divition_nm");
            }
            get
            {
                return this._taxation_divition_nm;
            }
        }

        [DataMember()]
        private int _inventory_management_division_id = 0;
        public int inventory_management_division_id
        {
            set
            {
                this._inventory_management_division_id = value;
                base.PropertyChangedHandler(this, "inventory_management_division_id");
            }
            get
            {
                return this._inventory_management_division_id;
            }
        }

        [DataMember()]
        private string _inventory_management_division_nm = "";
        public string inventory_management_division_nm
        {
            set
            {
                this._inventory_management_division_nm = value;
                base.PropertyChangedHandler(this, "inventory_management_division_nm");
            }
            get
            {
                return this._inventory_management_division_nm;
            }
        }

        [DataMember()]
        private double _purchase_lot = 0;
        public double purchase_lot
        {
            set
            {
                this._purchase_lot = value;
                base.PropertyChangedHandler(this, "purchase_lot");
            }
            get
            {
                return this._purchase_lot;
            }
        }

        [DataMember()]
        private int _lead_time = 0;
        public int lead_time
        {
            set
            {
                this._lead_time = value;
                base.PropertyChangedHandler(this, "lead_time");
            }
            get
            {
                return this._lead_time;
            }
        }

        [DataMember()]
        private double _just_inventory_number = 0;
        public double just_inventory_number
        {
            set
            {
                this._just_inventory_number = value;
                base.PropertyChangedHandler(this, "just_inventory_number");
            }
            get
            {
                return this._just_inventory_number;
            }
        }

        [DataMember()]
        private double _inventory_number = 0;
        public double inventory_number
        {
            set
            {
                this._inventory_number = value;
                base.PropertyChangedHandler(this, "inventory_number");
            }
            get
            {
                return this._inventory_number;
            }
        }

        [DataMember()]
        private int _inventory_evaluation_id = 0;
        public int inventory_evaluation_id
        {
            set
            {
                this._inventory_evaluation_id = value;
                base.PropertyChangedHandler(this, "inventory_evaluation_id");
            }
            get
            {
                return this._inventory_evaluation_id;
            }
        }

        [DataMember()]
        private string _inventory_evaluation_nm = "";
        public string inventory_evaluation_nm
        {
            set
            {
                this._inventory_evaluation_nm = value;
                base.PropertyChangedHandler(this, "inventory_evaluation_nm");
            }
            get
            {
                return this._inventory_evaluation_nm;
            }
        }

        [DataMember()]
        private string _main_purchase_id = "";
        public string main_purchase_id
        {
            set
            {
                this._main_purchase_id = value;
                base.PropertyChangedHandler(this, "main_purchase_id");
            }
            get
            {
                return this._main_purchase_id;
            }
        }

        [DataMember()]
        private string _main_purchase_nm = "";
        public string main_purchase_nm
        {
            set
            {
                this._main_purchase_nm = value;
                base.PropertyChangedHandler(this, "main_purchase_nm");
            }
            get
            {
                return this._main_purchase_nm;
            }
        }

        [DataMember()]
        private double _retail_price_skip_tax = 0;
        public double retail_price_skip_tax
        {
            set
            {
                this._retail_price_skip_tax = value;
                base.PropertyChangedHandler(this, "retail_price_skip_tax");
            }
            get
            {
                return this._retail_price_skip_tax;
            }
        }

        [DataMember()]
        private double _retail_price_before_tax = 0;
        public double retail_price_before_tax
        {
            set
            {
                this._retail_price_before_tax = value;
                base.PropertyChangedHandler(this, "retail_price_before_tax");
            }
            get
            {
                return this._retail_price_before_tax;
            }
        }

        [DataMember()]
        private double _sales_unit_price_skip_tax = 0;
        public double sales_unit_price_skip_tax
        {
            set
            {
                this._sales_unit_price_skip_tax = value;
                base.PropertyChangedHandler(this, "sales_unit_price_skip_tax");
            }
            get
            {
                return this._sales_unit_price_skip_tax;
            }
        }

        [DataMember()]
        private double _sales_unit_price_before_tax = 0;
        public double sales_unit_price_before_tax
        {
            set
            {
                this._sales_unit_price_before_tax = value;
                base.PropertyChangedHandler(this, "sales_unit_price_before_tax");
            }
            get
            {
                return this._sales_unit_price_before_tax;
            }
        }

        [DataMember()]
        private double _sales_cost_price_skip_tax = 0;
        public double sales_cost_price_skip_tax
        {
            set
            {
                this._sales_cost_price_skip_tax = value;
                base.PropertyChangedHandler(this, "sales_cost_price_skip_tax");
            }
            get
            {
                return this._sales_cost_price_skip_tax;
            }
        }

        [DataMember()]
        private double _sales_cost_price_before_tax = 0;
        public double sales_cost_price_before_tax
        {
            set
            {
                this._sales_cost_price_before_tax = value;
                base.PropertyChangedHandler(this, "sales_cost_price_before_tax");
            }
            get
            {
                return this._sales_cost_price_before_tax;
            }
        }

        [DataMember()]
        private double _purchase_unit_price_skip_tax = 0;
        public double purchase_unit_price_skip_tax
        {
            set
            {
                this._purchase_unit_price_skip_tax = value;
                base.PropertyChangedHandler(this, "purchase_unit_price_skip_tax");
            }
            get
            {
                return this._purchase_unit_price_skip_tax;
            }
        }

        [DataMember()]
        private double _purchase_unit_price_before_tax = 0;
        public double purchase_unit_price_before_tax
        {
            set
            {
                this._purchase_unit_price_before_tax = value;
                base.PropertyChangedHandler(this, "purchase_unit_price_before_tax");
            }
            get
            {
                return this._purchase_unit_price_before_tax;
            }
        }

        [DataMember()]
        private string _group1_id = "";
        public string group1_id
        {
            set
            {
                this._group1_id = value;
                base.PropertyChangedHandler(this, "group1_id");
            }
            get
            {
                return this._group1_id;
            }
        }

        [DataMember()]
        private string _group1_nm = "";
        public string group1_nm
        {
            set
            {
                this._group1_nm = value;
                base.PropertyChangedHandler(this, "group1_nm");
            }
            get
            {
                return this._group1_nm;
            }
        }

        [DataMember()]
        private string _group2_id = "";
        public string group2_id
        {
            set
            {
                this._group2_id = value;
                base.PropertyChangedHandler(this, "group2_id");
            }
            get
            {
                return this._group2_id;
            }
        }

        [DataMember()]
        private string _group2_nm = "";
        public string group2_nm
        {
            set
            {
                this._group2_nm = value;
                base.PropertyChangedHandler(this, "group2_nm");
            }
            get
            {
                return this._group2_nm;
            }
        }

        [DataMember()]
        private string _group3_id = "";
        public string group3_id
        {
            set
            {
                this._group3_id = value;
                base.PropertyChangedHandler(this, "group3_id");
            }
            get
            {
                return this._group3_id;
            }
        }

        [DataMember()]
        private string _group3_nm = "";
        public string group3_nm
        {
            set
            {
                this._group3_nm = value;
                base.PropertyChangedHandler(this, "group3_nm");
            }
            get
            {
                return this._group3_nm;
            }
        }

        [DataMember()]
        private string _memo = "";
        public string memo
        {
            set
            {
                this._memo = value;
                base.PropertyChangedHandler(this, "memo");
            }
            get
            {
                return this._memo;
            }
        }

        [DataMember()]
        private int _display_division_id = 0;
        public int display_division_id
        {
            set
            {
                this._display_division_id = value;
                base.PropertyChangedHandler(this, "display_division_id");
            }
            get
            {
                return this._display_division_id;
            }
        }

        [DataMember()]
        private string _display_division_nm = "";
        public string display_division_nm
        {
            set
            {
                this._display_division_nm = value;
                base.PropertyChangedHandler(this, "display_division_nm");
            }
            get
            {
                return this._display_division_nm;
            }
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
        private string message = "";
        public string MESSAGE
        {
            set
            {
                this.message = value;
                base.PropertyChangedHandler(this, "MESSAGE");
            }
            get { return this.message; }
        }

        public EntityCommodity()
        {
        }

    }
}