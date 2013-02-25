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
    public class EntityPurchaseH : EntityBase
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
        private long _no;
        public long no
        { 
            set 
            {
                this._no = value;
                base.PropertyChangedHandler(this, "no");
            }
            get { return this._no; }                 
        }

        //// 赤伝元伝票番号
        //[DataMember()]
        //private long _red_before_no = 0;
        //public long red_before_no
        //{
        //    set
        //    {
        //        this._red_before_no = value;
        //        base.PropertyChangedHandler(this, "red_before_no");
        //    }
        //    get { return this._red_before_no; }
        //}

        [DataMember()]
        private long _purchase_order_no;
        public long purchase_order_no
        {
            set
            {
                this._purchase_order_no = value;
                base.PropertyChangedHandler(this, "purchase_order_no");
            }
            get { return this._purchase_order_no; }
        }

        [DataMember()]
        private string _purchase_ymd;
        public string purchase_ymd
        { 
            set 
            {
                this._purchase_ymd = value;
                base.PropertyChangedHandler(this, "purchase_ymd");
            }
            get { return this._purchase_ymd; } 
        }

        [DataMember()]
        private string _purchase_id = "";
        public string purchase_id
        { 
            set 
            {
                this._purchase_id = value;
                base.PropertyChangedHandler(this, "purchase_id");
            }
            get { return this._purchase_id; } 
        }

        [DataMember()]
        private string _purchase_name = "";
        public string purchase_name
        {
            set
            {
                this._purchase_name = value;
                base.PropertyChangedHandler(this, "purchase_name");
            }
            get { return this._purchase_name; }
        }

        [DataMember()]
        private int _tax_change_id;
        public int tax_change_id
        { 
            set 
            {
                this._tax_change_id = value;
                base.PropertyChangedHandler(this, "tax_change_id");
            }
            get { return this._tax_change_id; } 
        }

        [DataMember()]
        private string _tax_change_name;
        public string tax_change_name
        {
            set
            {
                this._tax_change_name = value;
                base.PropertyChangedHandler(this, "tax_change_name");
            }
            get { return this._tax_change_name; }
        }

        [DataMember()]
        private int _business_division_id;
        public int business_division_id
        { 
            set 
            {
                this._business_division_id = value;
                base.PropertyChangedHandler(this, "business_division_id");
            }
            get { return this._business_division_id; } 
        }

        [DataMember()]
        private string _business_division_name;
        public string business_division_name
        {
            set
            {
                this._business_division_name = value;
                base.PropertyChangedHandler(this, "business_division_name");
            }
            get { return this._business_division_name; }
        }

        [DataMember()]
        private int _send_kbn_id = 0;
        public int send_kbn_id
        {
            set
            {
                this._send_kbn_id = value;
                base.PropertyChangedHandler(this, "send_kbn_id");
            }
            get { return this._send_kbn_id; }
        }

        [DataMember()]
        private string _send_kbn_nm = "";
        public string send_kbn_nm
        {
            set
            {
                this._send_kbn_nm = value;
                base.PropertyChangedHandler(this, "send_kbn_nm");
            }
            get { return this._send_kbn_nm; }
        }

        [DataMember()]
        private string _customer_id = "";
        public string customer_id
        {
            set
            {
                this._customer_id = value;
                base.PropertyChangedHandler(this, "customer_id");
            }
            get { return this._customer_id; }
        }

        [DataMember()]
        private string _customer_name = "";
        public string customer_name
        {
            set
            {
                this._customer_name = value;
                base.PropertyChangedHandler(this, "customer_name");
            }
            get { return this._customer_name; }
        }

        [DataMember()]
        private string _supplier_id = "";
        public string supplier_id
        { 
            set 
            {
                this._supplier_id = value;
                base.PropertyChangedHandler(this, "supplier_id");
            }
            get { return this._supplier_id; } 
        }

        [DataMember()]
        private string _supplier_name = "";
        public string supplier_name
        {
            set
            {
                this._supplier_name = value;
                base.PropertyChangedHandler(this, "supplier_name");
            }
            get { return this._supplier_name; }
        }

        [DataMember()]
        private string _supply_ymd;
        public string supply_ymd
        { 
            set 
            {
                this._supply_ymd = value;
                base.PropertyChangedHandler(this, "supply_ymd");
            }
            get { return this._supply_ymd; } 
        }

        // 入数計
        [DataMember()]
        private double _sum_enter_number = 0;
        public double sum_enter_number
        {
            set
            {
                this._sum_enter_number = value;
                base.PropertyChangedHandler(this, "sum_enter_number");
            }
            get { return this._sum_enter_number; }
        }

        // ケース数計
        [DataMember()]
        private double _sum_case_number = 0;
        public double sum_case_number
        {
            set
            {
                this._sum_case_number = value;
                base.PropertyChangedHandler(this, "sum_case_number");
            }
            get { return this._sum_case_number; }
        }

        // 数量計
        [DataMember()]
        private double _sum_number = 0;
        public double sum_number
        {
            set
            {
                this._sum_number = value;
                base.PropertyChangedHandler(this, "sum_number");
            }
            get { return this._sum_number; }
        }

        // 単価計
        [DataMember()]
        private double _sum_unit_price = 0;
        public double sum_unit_price
        {
            set
            {
                this._sum_unit_price = value;
                base.PropertyChangedHandler(this, "sum_unit_price");
            }
            get { return this._sum_unit_price; }
        }

        // 消費税額計
        [DataMember()]
        private double _sum_tax = 0;
        public double sum_tax
        {
            set
            {
                this._sum_tax = value;
                base.PropertyChangedHandler(this, "sum_tax");
            }
            get { return this._sum_tax; }
        }

        // 税抜金額計
        [DataMember()]
        private double _sum_no_tax_price = 0;
        public double sum_no_tax_price
        {
            set
            {
                this._sum_no_tax_price = value;
                base.PropertyChangedHandler(this, "sum_no_tax_price");
            }
            get { return this._sum_no_tax_price; }
        }

        // 金額計
        [DataMember()]
        private double _sum_price = 0;
        public double sum_price
        {
            set
            {
                this._sum_price = value;
                base.PropertyChangedHandler(this, "sum_price");
            }
            get { return this._sum_price; }
        }

        // 支払締フラグ
        [DataMember()]
        private int _payment_close_flg = 0;
        public int payment_close_flg
        {
            set
            {
                this._payment_close_flg = value;
                base.PropertyChangedHandler(this, "payment_close_flg");
            }
            get { return this._payment_close_flg; }
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
            get { return this._memo; } 
        }

        [DataMember()]
        private int _update_flg = 0;
        public int update_flg
        { 
            set 
            {
                this._update_flg = value;
                base.PropertyChangedHandler(this, "update_flg");
            }
            get { return this._update_flg; } 
        }

        [DataMember()]
        private int _update_person_id = 0;
        public int update_person_id
        {
            set
            {
                this._update_person_id = value;
                base.PropertyChangedHandler(this, "update_person_id");
            }
            get { return this._update_person_id; }
        }

        [DataMember()]
        private string _update_person_nm = "";
        public string update_person_nm
        {
            set
            {
                this._update_person_nm = value;
                base.PropertyChangedHandler(this, "update_person_nm");
            }
            get { return this._update_person_nm; }
        }

        [DataMember()]
        private string _update_date = "";
        public string update_date
        { 
            set 
            {
                this._update_date = value;
                base.PropertyChangedHandler(this, "update_date");
            }
            get { return this._update_date; } 
        }

        [DataMember()]
        private DateTime _update_time;
        public DateTime update_time
        {
            set
            {
                this._update_time = value;
                base.PropertyChangedHandler(this, "update_time");
            }
            get { return this._update_time; }
        }

        // 金額端数処理ID
        [DataMember()]
        private int _price_fraction_proc_id;
        public int price_fraction_proc_id
        {
            set
            {
                this._price_fraction_proc_id = value;
                base.PropertyChangedHandler(this, "price_fraction_proc_id");
            }
            get { return this._price_fraction_proc_id; }
        }

        // 税端数処理ID
        [DataMember()]
        private int _tax_fraction_proc_id = 0;
        public int tax_fraction_proc_id
        {
            set
            {
                this._tax_fraction_proc_id = value;
                base.PropertyChangedHandler(this, "tax_fraction_proc_id");
            }
            get { return this._tax_fraction_proc_id; }
        }

        // 単価種類ID
        [DataMember()]
        private int _unit_kind_id = 0;
        public int unit_kind_id
        {
            set
            {
                this._unit_kind_id = value;
                base.PropertyChangedHandler(this, "unit_kind_id");
            }
            get { return this._unit_kind_id; }
        }

        // 買掛残高
        [DataMember()]
        private double _payment_credit_price = 0;
        public double payment_credit_price
        {
            set
            {
                this._payment_credit_price = value;
                base.PropertyChangedHandler(this, "payment_credit_price");
            }
            get { return this._payment_credit_price; }
        }

        // 買掛残高
        [DataMember()]
        private double _before_payment_credit_price = 0;
        public double before_payment_credit_price
        {
            set
            {
                this._before_payment_credit_price = value;
                base.PropertyChangedHandler(this, "before_payment_credit_price");
            }
            get { return this._before_payment_credit_price; }
        }

        // 払締年月日
        [DataMember()]
        private string _payment_yyyymmdd;
        public string payment_yyyymmdd
        {
            set
            {
                this._payment_yyyymmdd = value;
                base.PropertyChangedHandler(this, "payment_yyyymmdd");
            }
            get { return this._payment_yyyymmdd; }
        }

        // 掛率
        [DataMember()]
        private int _credit_rate = 0;
        public int credit_rate
        {
            set
            {
                this._credit_rate = value;
                base.PropertyChangedHandler(this, "credit_rate");
            }
            get
            {
                return this._credit_rate;
            }
        }

        // 支払日
        [DataMember()]
        private int _payment_day;
        public int payment_day
        {
            set
            {
                this._payment_day = value;
                base.PropertyChangedHandler(this, "payment_day");
            }
            get { return this._payment_day; }
        }

        // 支払サイクルID
        [DataMember()]
        private int _payment_cycle_id;
        public int payment_cycle_id
        {
            set
            {
                this._payment_cycle_id = value;
                base.PropertyChangedHandler(this, "payment_cycle_id");
            }
            get { return this._payment_cycle_id; }
        }

        // 支払書番号
        [DataMember()]
        private long _payment_no;
        public long payment_no
        {
            set
            {
                this._payment_no = value;
                base.PropertyChangedHandler(this, "payment_no");
            }
            get { return this._payment_no; }
        }

        // 払締区分
        [DataMember()]
        private string _payment_state = "";
        public string payment_state
        {
            set
            {
                this._payment_state = value;
                base.PropertyChangedHandler(this, "payment_state");
            }
            get
            {
                return this._payment_state;
            }
        }

        [DataMember()]
        private int _payment_cash_receivable_kbn = 0;
        public int payment_cash_receivable_kbn
        {
            set
            {
                this._payment_cash_receivable_kbn = value;
                base.PropertyChangedHandler(this, "payment_cash_receivable_kbn");
            }
            get
            {
                return this._payment_cash_receivable_kbn;
            }
        }

        [DataMember()]
        private string _payment_cash_receivable_kbn_nm = "消込未";
        public string payment_cash_receivable_kbn_nm
        {
            set
            {
                this._payment_cash_receivable_kbn_nm = value;
                base.PropertyChangedHandler(this, "payment_cash_receivable_kbn_nm");
            }
            get
            {
                return this._payment_cash_receivable_kbn_nm;
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
        private string _message = "";
        public string message
        {
            set
            {
                this._message = value;
                base.PropertyChangedHandler(this, "message");
            }
            get { return this._message; }
        }

        public EntityPurchaseH()
        {
        }

    }
}