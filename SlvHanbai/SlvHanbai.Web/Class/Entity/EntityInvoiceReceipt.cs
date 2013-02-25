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
    public class EntityInvoiceReceipt : EntityBase
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
        private double _before_receipt_price = 0;
        public double before_receipt_price
        {
            set
            {
                this._before_receipt_price = value;
                base.PropertyChangedHandler(this, "before_receipt_price");
            }
            get
            {
                return this._before_receipt_price;
            }
        }

        // 売掛残高
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
        private string _receipt_division_id;
        public string receipt_division_id
        {
            set
            {
                this._receipt_division_id = value;
                base.PropertyChangedHandler(this, "receipt_division_id");
            }
            get { return this._receipt_division_id; }
        }

        [DataMember()]
        private string _receipt_division_nm;
        public string receipt_division_nm
        {
            set
            {
                this._receipt_division_nm = value;
                base.PropertyChangedHandler(this, "receipt_division_nm");
            }
            get { return this._receipt_division_nm; }
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

        public EntityInvoiceReceipt()
        {
        }

    }
}