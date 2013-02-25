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
    public class EntityPaymentClosePrm : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private string _payment_yyyymmdd = "";
        public string payment_yyyymmdd
        {
            set
            {
                this._payment_yyyymmdd = value;
                base.PropertyChangedHandler(this, "payment_yyyymmdd");
            }
            get
            {
                return this._payment_yyyymmdd;
            }
        }

        [DataMember()]
        private string _payment_plan_yyyymmdd = "";
        public string payment_plan_yyyymmdd
        {
            set
            {
                this._payment_plan_yyyymmdd = value;
                base.PropertyChangedHandler(this, "payment_plan_yyyymmdd");
            }
            get
            {
                return this._payment_plan_yyyymmdd;
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

        public EntityPaymentClosePrm()
        {
        }

    }
}