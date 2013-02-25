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
    public class EntityInOutDeliveryH : EntityBase
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
        private string _in_out_delivery_ymd = "";
        public string in_out_delivery_ymd
        { 
            set 
            {
                this._in_out_delivery_ymd = value;
                base.PropertyChangedHandler(this, "in_out_delivery_ymd");
            }
            get { return this._in_out_delivery_ymd; } 
        }

        [DataMember()]
        private int _in_out_delivery_kbn = 0;
        public int in_out_delivery_kbn
        {
            set
            {
                this._in_out_delivery_kbn = value;
                base.PropertyChangedHandler(this, "in_out_delivery_kbn");
            }
            get { return this._in_out_delivery_kbn; }
        }

        [DataMember()]
        private string _in_out_delivery_kbn_nm = "";
        public string in_out_delivery_kbn_nm
        {
            set
            {
                this._in_out_delivery_kbn_nm = value;
                base.PropertyChangedHandler(this, "in_out_delivery_kbn_nm");
            }
            get { return this._in_out_delivery_kbn_nm; }
        }

        [DataMember()]
        private int _in_out_delivery_proc_kbn = 0;
        public int in_out_delivery_proc_kbn
        {
            set
            {
                this._in_out_delivery_proc_kbn = value;
                base.PropertyChangedHandler(this, "in_out_delivery_proc_kbn");
            }
            get { return this._in_out_delivery_proc_kbn; }
        }

        [DataMember()]
        private string _in_out_delivery_proc_kbn_nm = "";
        public string in_out_delivery_proc_kbn_nm
        {
            set
            {
                this._in_out_delivery_proc_kbn_nm = value;
                base.PropertyChangedHandler(this, "in_out_delivery_proc_kbn_nm");
            }
            get { return this._in_out_delivery_proc_kbn_nm; }
        }

        [DataMember()]
        private long _cause_no;
        public long cause_no
        {
            set
            {
                this._cause_no = value;
                base.PropertyChangedHandler(this, "cause_no");
            }
            get { return this._cause_no; }
        }

        [DataMember()]
        private int _in_out_delivery_to_kbn = 0;
        public int in_out_delivery_to_kbn
        {
            set
            {
                this._in_out_delivery_to_kbn = value;
                base.PropertyChangedHandler(this, "in_out_delivery_to_kbn");
            }
            get { return this._in_out_delivery_to_kbn; }
        }

        [DataMember()]
        private string _in_out_delivery_to_kbn_nm = "";
        public string in_out_delivery_to_kbn_nm
        {
            set
            {
                this._in_out_delivery_to_kbn_nm = value;
                base.PropertyChangedHandler(this, "in_out_delivery_to_kbn_nm");
            }
            get { return this._in_out_delivery_to_kbn_nm; }
        }

        [DataMember()]
        private string _group_id_to = "";
        public string group_id_to
        {
            set
            {
                this._group_id_to = value;
                base.PropertyChangedHandler(this, "group_id_to");
            }
            get
            {
                return this._group_id_to;
            }
        }

        [DataMember()]
        private string _group_id_to_nm = "";
        public string group_id_to_nm
        {
            set
            {
                this._group_id_to_nm = value;
                base.PropertyChangedHandler(this, "group_id_to_nm");
            }
            get
            {
                return this._group_id_to_nm;
            }
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

        public EntityInOutDeliveryH()
        {
        }

    }
}