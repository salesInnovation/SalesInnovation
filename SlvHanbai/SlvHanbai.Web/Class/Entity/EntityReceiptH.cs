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
    public class EntityReceiptH : EntityBase
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

        [DataMember()]
        private long _invoice_no;
        public long invoice_no
        {
            set
            {
                this._invoice_no = value;
                base.PropertyChangedHandler(this, "invoice_no");
            }
            get { return this._invoice_no; }
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
            get { return this._invoice_id; }
        }

        [DataMember()]
        private string _invoice_name = "";
        public string invoice_name
        {
            set
            {
                this._invoice_name = value;
                base.PropertyChangedHandler(this, "invoice_name");
            }
            get { return this._invoice_name; }
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
            get { return this._invoice_yyyymmdd; }
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
            get { return this._summing_up_day; }
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
            get { return this._person_id; }
        }

        [DataMember()]
        private string _person_name = "";
        public string person_name
        {
            set
            {
                this._person_name = value;
                base.PropertyChangedHandler(this, "person_name");
            }
            get { return this._person_name; }
        }

        [DataMember()]
        private string _receipt_ymd;
        public string receipt_ymd
        { 
            set 
            {
                this._receipt_ymd = value;
                base.PropertyChangedHandler(this, "receipt_ymd");
            }
            get { return this._receipt_ymd; } 
        }
        
        [DataMember()]
        private double _sum_price;
        public double sum_price
        { 
            set 
            {
                this._sum_price = value;
                base.PropertyChangedHandler(this, "sum_price");
            }
            get { return this._sum_price; } 
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

        // 入金前売掛残高
        [DataMember()]
        private double _before_credit_price;
        public double before_credit_price
        {
            set
            {
                this._before_credit_price = value;
                base.PropertyChangedHandler(this, "before_credit_price");
            }
            get { return this._before_credit_price; }
        }

        // 請求締フラグ
        [DataMember()]
        private int _invoice_close_flg = 0;
        public int invoice_close_flg
        {
            set
            {
                this._invoice_close_flg = value;
                base.PropertyChangedHandler(this, "invoice_close_flg");
            }
            get { return this._invoice_close_flg; }
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
        private int _invoice_kbn = 0;
        public int invoice_kbn
        {
            set
            {
                this._invoice_kbn = value;
                base.PropertyChangedHandler(this, "invoice_kbn");
            }
            get { return this._invoice_kbn; }
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
            get { return this._invoice_kbn_nm; }
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
            get { return this._summing_up_group_id; }
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
            get { return this._summing_up_group_nm; }
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
            get { return this._collect_plan_day; }
        }

        [DataMember()]
        private double _invoice_price;
        public double invoice_price
        {
            set
            {
                this._invoice_price = value;
                base.PropertyChangedHandler(this, "invoice_price");
            }
            get { return this._invoice_price; }
        }

        [DataMember()]
        private double _before_receipt_price;
        public double before_receipt_price
        {
            set
            {
                this._before_receipt_price = value;
                base.PropertyChangedHandler(this, "before_receipt_price");
            }
            get { return this._before_receipt_price; }
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

        public EntityReceiptH()
        {
        }

    }
}