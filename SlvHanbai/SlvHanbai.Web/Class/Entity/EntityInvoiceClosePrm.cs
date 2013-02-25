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
    public class EntityInvoiceClosePrm : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

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
        private string _collect_plan_yyyymmdd = "";
        public string collect_plan_yyyymmdd
        {
            set
            {
                this._collect_plan_yyyymmdd = value;
                base.PropertyChangedHandler(this, "collect_plan_yyyymmdd");
            }
            get
            {
                return this._collect_plan_yyyymmdd;
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

        public EntityInvoiceClosePrm()
        {
        }

    }
}