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
    public class EntityInvoiceClose : EntityBase
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
        private string _invoice_id = "";
        public string invoice_id
        {
            set
            {
                this._invoice_id = value;
                base.PropertyChangedHandler(this, "invoice_id");
            }
            get
            {
                return this._invoice_id;
            }
        }

        [DataMember()]
        private string _invoice_nm = "";
        public string invoice_nm
        {
            set
            {
                this._invoice_nm = value;
                base.PropertyChangedHandler(this, "invoice_nm");
            }
            get
            {
                return this._invoice_nm;
            }
        }

        [DataMember()]
        private string _invoice_yyyymmdd = "";
        public string invoice_yyyymmdd
        {
            set
            {
                this._invoice_yyyymmdd = value;
                base.PropertyChangedHandler(this, "invoice_yyyymmdd");
            }
            get
            {
                return this._invoice_yyyymmdd;
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

        // 前回請求締日
        [DataMember()]
        private string _before_invoice_yyyymmdd = "";
        public string before_invoice_yyyymmdd
        {
            set
            {
                this._before_invoice_yyyymmdd = value;
                base.PropertyChangedHandler(this, "before_invoice_yyyymmdd");
            }
            get
            {
                return this._before_invoice_yyyymmdd;
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
        private int _collect_cycle_id = 0;
        public int collect_cycle_id
        {
            set
            {
                this._collect_cycle_id = value;
                base.PropertyChangedHandler(this, "collect_cycle_id");
            }
            get
            {
                return this._collect_cycle_id;
            }
        }

        [DataMember()]
        private int _collect_day = 0;
        public int collect_day
        {
            set
            {
                this._collect_day = value;
                base.PropertyChangedHandler(this, "collect_day");
            }
            get
            {
                return this._collect_day;
            }
        }

        [DataMember()]
        private string _collect_plan_day = "";
        public string collect_plan_day
        {
            set
            {
                this._collect_plan_day = value;
                base.PropertyChangedHandler(this, "collect_plan_day");
            }
            get
            {
                return this._collect_plan_day;
            }
        }

        [DataMember()]
        private double _before_invoice_price = 0;
        public double before_invoice_price
        {
            set
            {
                this._before_invoice_price = value;
                base.PropertyChangedHandler(this, "before_invoice_price");
            }
            get
            {
                return this._before_invoice_price;
            }
        }

        [DataMember()]
        private double _before_invoice_price_upd = 0;
        public double before_invoice_price_upd
        {
            set
            {
                this._before_invoice_price_upd = value;
                base.PropertyChangedHandler(this, "before_invoice_price_upd");
            }
            get
            {
                return this._before_invoice_price_upd;
            }
        }

        [DataMember()]
        private double _receipt_price = 0;
        public double receipt_price
        {
            set
            {
                this._receipt_price = value;
                base.PropertyChangedHandler(this, "receipt_price");
            }
            get
            {
                return this._receipt_price;
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
        private double _sales_price = 0;
        public double sales_price
        {
            set
            {
                this._sales_price = value;
                base.PropertyChangedHandler(this, "sales_price");
            }
            get
            {
                return this._sales_price;
            }
        }

        [DataMember()]
        private double _no_tax_sales_price = 0;
        public double no_tax_sales_price
        {
            set
            {
                this._no_tax_sales_price = value;
                base.PropertyChangedHandler(this, "no_tax_sales_price");
            }
            get
            {
                return this._no_tax_sales_price;
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
        private double _invoice_price = 0;
        public double invoice_price
        {
            set
            {
                this._invoice_price = value;
                base.PropertyChangedHandler(this, "invoice_price");
            }
            get
            {
                return this._invoice_price;
            }
        }

        [DataMember()]
        private double _this_receipt_price = 0;
        public double this_receipt_price
        {
            set
            {
                this._this_receipt_price = value;
                base.PropertyChangedHandler(this, "this_receipt_price");
            }
            get
            {
                return this._this_receipt_price;
            }
        }

        [DataMember()]
        private double _invoice_zan_price = 0;
        public double invoice_zan_price
        {
            set
            {
                this._invoice_zan_price = value;
                base.PropertyChangedHandler(this, "invoice_zan_price");
            }
            get
            {
                return this._invoice_zan_price;
            }
        }

        [DataMember()]
        private int _invoice_kbn = 0;
        public int invoice_kbn
        {
            set
            {
                this._invoice_kbn = value;
                base.PropertyChangedHandler(this, "invoice_kbn");
            }
            get
            {
                return this._invoice_kbn;
            }
        }

        [DataMember()]
        private string _invoice_kbn_nm = "";
        public string invoice_kbn_nm
        {
            set
            {
                this._invoice_kbn_nm = value;
                base.PropertyChangedHandler(this, "invoice_kbn_nm");
            }
            get
            {
                return this._invoice_kbn_nm;
            }
        }

        [DataMember()]
        private int _invoice_print_flg = 0;
        public int invoice_print_flg
        {
            set
            {
                this._invoice_print_flg = value;
                base.PropertyChangedHandler(this, "invoice_print_flg");
            }
            get
            {
                return this._invoice_print_flg;
            }
        }

        [DataMember()]
        private string _invoice_print_flg_nm = "";
        public string invoice_print_flg_nm
        {
            set
            {
                this._invoice_print_flg_nm = value;
                base.PropertyChangedHandler(this, "invoice_print_flg_nm");
            }
            get
            {
                return this._invoice_print_flg_nm;
            }
        }

        [DataMember()]
        private int _receipt_receivable_kbn = 0;
        public int receipt_receivable_kbn
        {
            set
            {
                this._receipt_receivable_kbn = value;
                base.PropertyChangedHandler(this, "receipt_receivable_kbn");
            }
            get
            {
                return this._receipt_receivable_kbn;
            }
        }

        [DataMember()]
        private string _receipt_receivable_kbn_nm = "";
        public string receipt_receivable_kbn_nm
        {
            set
            {
                this._receipt_receivable_kbn_nm = value;
                base.PropertyChangedHandler(this, "receipt_receivable_kbn_nm");
            }
            get
            {
                return this._receipt_receivable_kbn_nm;
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
        private int _invoice_exists_flg = 0;
        public int invoice_exists_flg
        {
            set
            {
                this._invoice_exists_flg = value;
                base.PropertyChangedHandler(this, "invoice_exists_flg");
            }
            get
            {
                return this._invoice_exists_flg;
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

        public EntityInvoiceClose()
        {
        }

    }
}