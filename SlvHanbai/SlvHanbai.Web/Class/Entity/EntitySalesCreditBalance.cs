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
    public class EntitySalesCreditBalance : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private long _rec_no = 0;
        public long rec_no
        {
            set
            {
                this._rec_no = value;
                base.PropertyChangedHandler(this, "rec_no");
            }
            get
            {
                return this._rec_no;
            }
        }

        [DataMember()]
        private string _ym = "";
        public string ym
        {
            set
            {
                this._ym = value;
                base.PropertyChangedHandler(this, "ym");
            }
            get
            {
                return this._ym;
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
        private double _before_sales_credit_balacne = 0;
        public double before_sales_credit_balacne
        {
            set
            {
                this._before_sales_credit_balacne = value;
                base.PropertyChangedHandler(this, "before_sales_credit_balacne");
            }
            get
            {
                return this._before_sales_credit_balacne;
            }
        }

        [DataMember()]
        private double _before_sales_credit_balacne_upd = 0;
        public double before_sales_credit_balacne_upd
        {
            set
            {
                this._before_sales_credit_balacne_upd = value;
                base.PropertyChangedHandler(this, "before_sales_credit_balacne_upd");
            }
            get
            {
                return this._before_sales_credit_balacne_upd;
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
        private double _this_receipt_percent = 0;
        public double this_receipt_percent
        {
            set
            {
                this._this_receipt_percent = value;
                base.PropertyChangedHandler(this, "this_receipt_percent");
            }
            get
            {
                return this._this_receipt_percent;
            }
        }

        [DataMember()]
        private double _this_sales_credit_price = 0;
        public double this_sales_credit_price
        {
            set
            {
                this._this_sales_credit_price = value;
                base.PropertyChangedHandler(this, "this_sales_credit_price");
            }
            get
            {
                return this._this_sales_credit_price;
            }
        }

        [DataMember()]
        private double _this_tax = 0;
        public double this_tax
        {
            set
            {
                this._this_tax = value;
                base.PropertyChangedHandler(this, "this_tax");
            }
            get
            {
                return this._this_tax;
            }
        }

        [DataMember()]
        private double _this_sales_credit_balance = 0;
        public double this_sales_credit_balance
        {
            set
            {
                this._this_sales_credit_balance = value;
                base.PropertyChangedHandler(this, "this_sales_credit_balance");
            }
            get
            {
                return this._this_sales_credit_balance;
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

        public EntitySalesCreditBalance()
        {
        }

    }
}