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
    public class EntityPaymentCashD : EntityBase
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
        private int _rec_no;
        public int rec_no
        { 
            set 
            {
                this._rec_no = value;
                base.PropertyChangedHandler(this, "rec_no");
            }
            get { return this._rec_no; } 
        }

        [DataMember()]
        private string _payment_cash_division_id;
        public string payment_cash_division_id
        { 
            set 
            {
                this._payment_cash_division_id = value;
                base.PropertyChangedHandler(this, "payment_cash_division_id");
            }
            get { return this._payment_cash_division_id; } 
        }

        [DataMember()]
        private string _payment_cash_division_nm;
        public string payment_cash_division_nm
        {
            set
            {
                this._payment_cash_division_nm = value;
                base.PropertyChangedHandler(this, "payment_cash_division_nm");
            }
            get { return this._payment_cash_division_nm; }
        }

        [DataMember()]
        private string _bill_site_day;
        public string bill_site_day
        {
            set
            {
                this._bill_site_day = value;
                base.PropertyChangedHandler(this, "bill_site_day");
            }
            get { return this._bill_site_day; }
        }

        [DataMember()]
        private double _price;
        public double price
        { 
            set 
            {
                this._price = value;
                base.PropertyChangedHandler(this, "price");
            }
            get { return this._price; } 
        }

        [DataMember()]
        private string _memo;
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
        private string _message;
        public string message
        {
            set
            {
                this._message = value;
                base.PropertyChangedHandler(this, "message");
            }
            get { return this._message; }
        }

        public EntityPaymentCashD()
        {
        }
    }
}