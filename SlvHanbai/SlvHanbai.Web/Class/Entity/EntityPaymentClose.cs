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
    public class EntityPaymentClose : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private string _no = "";
        public string no
        {
            set
            {
                this._no = value;
                base.PropertyChangedHandler(this, "no");
            }
            get
            {
                return this._no;
            }
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
            get
            {
                return this._purchase_id;
            }
        }

        [DataMember()]
        private string _purchase_nm = "";
        public string purchase_nm
        {
            set
            {
                this._purchase_nm = value;
                base.PropertyChangedHandler(this, "purchase_nm");
            }
            get
            {
                return this._purchase_nm;
            }
        }

        [DataMember()]
        private string _payment_close_yyyymmdd = "";
        public string payment_close_yyyymmdd
        {
            set
            {
                this._payment_close_yyyymmdd = value;
                base.PropertyChangedHandler(this, "payment_close_yyyymmdd");
            }
            get
            {
                return this._payment_close_yyyymmdd;
            }
        }

        [DataMember()]
        private int _summing_up_day = 0;
        public int summing_up_day
        {
            set
            {
                this._summing_up_day = value;
                base.PropertyChangedHandler(this, "summing_up_day");
            }
            get
            {
                return this._summing_up_day;
            }
        }

        [DataMember()]
        private string _summing_up_group_id = "";
        public string summing_up_group_id
        {
            set
            {
                this._summing_up_group_id = value;
                base.PropertyChangedHandler(this, "summing_up_group_id");
            }
            get
            {
                return this._summing_up_group_id;
            }
        }

        [DataMember()]
        private string _summing_up_group_nm = "";
        public string summing_up_group_nm
        {
            set
            {
                this._summing_up_group_nm = value;
                base.PropertyChangedHandler(this, "summing_up_group_nm");
            }
            get
            {
                return this._summing_up_group_nm;
            }
        }

        // 前回支払締日
        [DataMember()]
        private string _before_payment_yyyymmdd = "";
        public string before_payment_yyyymmdd
        {
            set
            {
                this._before_payment_yyyymmdd = value;
                base.PropertyChangedHandler(this, "before_payment_yyyymmdd");
            }
            get
            {
                return this._before_payment_yyyymmdd;
            }
        }

        [DataMember()]
        private int _person_id = 0;
        public int person_id
        {
            set
            {
                this._person_id = value;
                base.PropertyChangedHandler(this, "person_id");
            }
            get
            {
                return this._person_id;
            }
        }

        [DataMember()]
        private string _person_nm = "";
        public string person_nm
        {
            set
            {
                this._person_nm = value;
                base.PropertyChangedHandler(this, "person_nm");
            }
            get
            {
                return this._person_nm;
            }
        }

        [DataMember()]
        private int _payment_cycle_id = 0;
        public int payment_cycle_id
        {
            set
            {
                this._payment_cycle_id = value;
                base.PropertyChangedHandler(this, "payment_cycle_id");
            }
            get
            {
                return this._payment_cycle_id;
            }
        }

        [DataMember()]
        private int _payment_day = 0;
        public int payment_day
        {
            set
            {
                this._payment_day = value;
                base.PropertyChangedHandler(this, "payment_day");
            }
            get
            {
                return this._payment_day;
            }
        }

        [DataMember()]
        private string _payment_plan_day = "";
        public string payment_plan_day
        {
            set
            {
                this._payment_plan_day = value;
                base.PropertyChangedHandler(this, "payment_plan_day");
            }
            get
            {
                return this._payment_plan_day;
            }
        }

        [DataMember()]
        private double _before_payment_price = 0;
        public double before_payment_price
        {
            set
            {
                this._before_payment_price = value;
                base.PropertyChangedHandler(this, "before_payment_price");
            }
            get
            {
                return this._before_payment_price;
            }
        }

        [DataMember()]
        private double _before_payment_price_upd = 0;
        public double before_payment_price_upd
        {
            set
            {
                this._before_payment_price_upd = value;
                base.PropertyChangedHandler(this, "before_payment_price_upd");
            }
            get
            {
                return this._before_payment_price_upd;
            }
        }

        [DataMember()]
        private double _payment_cash_price = 0;
        public double payment_cash_price
        {
            set
            {
                this._payment_cash_price = value;
                base.PropertyChangedHandler(this, "payment_cash_price");
            }
            get
            {
                return this._payment_cash_price;
            }
        }

        [DataMember()]
        private double _transfer_price = 0;
        public double transfer_price
        {
            set
            {
                this._transfer_price = value;
                base.PropertyChangedHandler(this, "transfer_price");
            }
            get
            {
                return this._transfer_price;
            }
        }

        [DataMember()]
        private double _purchase_price = 0;
        public double purchase_price
        {
            set
            {
                this._purchase_price = value;
                base.PropertyChangedHandler(this, "purchase_price");
            }
            get
            {
                return this._purchase_price;
            }
        }

        [DataMember()]
        private double _no_tax_purchase_price = 0;
        public double no_tax_purchase_price
        {
            set
            {
                this._no_tax_purchase_price = value;
                base.PropertyChangedHandler(this, "no_tax_purchase_price");
            }
            get
            {
                return this._no_tax_purchase_price;
            }
        }

        [DataMember()]
        private double _tax = 0;
        public double tax
        {
            set
            {
                this._tax = value;
                base.PropertyChangedHandler(this, "tax");
            }
            get
            {
                return this._tax;
            }
        }

        [DataMember()]
        private double _out_tax = 0;
        public double out_tax
        {
            set
            {
                this._out_tax = value;
                base.PropertyChangedHandler(this, "out_tax");
            }
            get
            {
                return this._out_tax;
            }
        }

        [DataMember()]
        private double _payment_price = 0;
        public double payment_price
        {
            set
            {
                this._payment_price = value;
                base.PropertyChangedHandler(this, "payment_price");
            }
            get
            {
                return this._payment_price;
            }
        }

        [DataMember()]
        private double _this_payment_cash_price = 0;
        public double this_payment_cash_price
        {
            set
            {
                this._this_payment_cash_price = value;
                base.PropertyChangedHandler(this, "this_payment_cash_price");
            }
            get
            {
                return this._this_payment_cash_price;
            }
        }

        [DataMember()]
        private double _payment_zan_price = 0;
        public double payment_zan_price
        {
            set
            {
                this._payment_zan_price = value;
                base.PropertyChangedHandler(this, "payment_zan_price");
            }
            get
            {
                return this._payment_zan_price;
            }
        }

        [DataMember()]
        private int _payment_kbn = 0;
        public int payment_kbn
        {
            set
            {
                this._payment_kbn = value;
                base.PropertyChangedHandler(this, "payment_kbn");
            }
            get
            {
                return this._payment_kbn;
            }
        }

        [DataMember()]
        private string _payment_kbn_nm = "";
        public string payment_kbn_nm
        {
            set
            {
                this._payment_kbn_nm = value;
                base.PropertyChangedHandler(this, "payment_kbn_nm");
            }
            get
            {
                return this._payment_kbn_nm;
            }
        }

        [DataMember()]
        private int _payment_print_flg = 0;
        public int payment_print_flg
        {
            set
            {
                this._payment_print_flg = value;
                base.PropertyChangedHandler(this, "payment_print_flg");
            }
            get
            {
                return this._payment_print_flg;
            }
        }

        [DataMember()]
        private string _payment_print_flg_nm = "";
        public string payment_print_flg_nm
        {
            set
            {
                this._payment_print_flg_nm = value;
                base.PropertyChangedHandler(this, "payment_print_flg_nm");
            }
            get
            {
                return this._payment_print_flg_nm;
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
        private string _payment_cash_receivable_kbn_nm = "";
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
        private int _payment_exists_flg = 0;
        public int payment_exists_flg
        {
            set
            {
                this._payment_exists_flg = value;
                base.PropertyChangedHandler(this, "payment_exists_flg");
            }
            get
            {
                return this._payment_exists_flg;
            }
        }

        [DataMember()]
        private bool _exec_flg = false;
        public bool exec_flg
        {
            set
            {
                this._exec_flg = value;
                base.PropertyChangedHandler(this, "exec_flg");
            }
            get
            {
                return this._exec_flg;
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

        public EntityPaymentClose()
        {
        }

    }
}