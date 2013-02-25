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
    public class EntityPaymentCashOut : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private long _no = 0;
        public long no
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
        private double _before_payment_cash_price = 0;
        public double before_payment_cash_price
        {
            set
            {
                this._before_payment_cash_price = value;
                base.PropertyChangedHandler(this, "before_payment_cash_price");
            }
            get
            {
                return this._before_payment_cash_price;
            }
        }

        // 買掛残高
        [DataMember()]
        private double _credit_price;
        public double credit_price
        {
            set
            {
                this._credit_price = value;
                base.PropertyChangedHandler(this, "credit_price");
            }
            get { return this._credit_price; }
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
        private string _payment_division_id;
        public string payment_division_id
        {
            set
            {
                this._payment_division_id = value;
                base.PropertyChangedHandler(this, "payment_division_id");
            }
            get { return this._payment_division_id; }
        }

        [DataMember()]
        private string _payment_division_nm;
        public string payment_division_nm
        {
            set
            {
                this._payment_division_nm = value;
                base.PropertyChangedHandler(this, "payment_division_nm");
            }
            get { return this._payment_division_nm; }
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

        public EntityPaymentCashOut()
        {
        }

    }
}