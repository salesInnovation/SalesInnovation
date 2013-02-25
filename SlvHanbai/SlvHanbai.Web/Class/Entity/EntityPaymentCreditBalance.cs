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
    public class EntityPaymentCreditBalance : EntityBase
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
        private double _before_payment_credit_balacne = 0;
        public double before_payment_credit_balacne
        {
            set
            {
                this._before_payment_credit_balacne = value;
                base.PropertyChangedHandler(this, "before_payment_credit_balacne");
            }
            get
            {
                return this._before_payment_credit_balacne;
            }
        }

        [DataMember()]
        private double _before_payment_credit_balacne_upd = 0;
        public double before_payment_credit_balacne_upd
        {
            set
            {
                this._before_payment_credit_balacne_upd = value;
                base.PropertyChangedHandler(this, "before_payment_credit_balacne_upd");
            }
            get
            {
                return this._before_payment_credit_balacne_upd;
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
        private double _this_payment_cash_percent = 0;
        public double this_payment_cash_percent
        {
            set
            {
                this._this_payment_cash_percent = value;
                base.PropertyChangedHandler(this, "this_payment_cash_percent");
            }
            get
            {
                return this._this_payment_cash_percent;
            }
        }

        [DataMember()]
        private double _this_purchase_price = 0;
        public double this_purchase_price
        {
            set
            {
                this._this_purchase_price = value;
                base.PropertyChangedHandler(this, "this_purchase_price");
            }
            get
            {
                return this._this_purchase_price;
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
        private double _this_payment_credit_balance = 0;
        public double this_payment_credit_balance
        {
            set
            {
                this._this_payment_credit_balance = value;
                base.PropertyChangedHandler(this, "this_payment_credit_balance");
            }
            get
            {
                return this._this_payment_credit_balance;
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

        public EntityPaymentCreditBalance()
        {
        }

    }
}