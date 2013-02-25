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
    public class EntityPaymentCash : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private long id = 0;
        public long ID 
        { 
            set 
            { 
                this.id = value;
                base.PropertyChangedHandler(this, "ID");
            } 
            get { return this.id; } 
        }

        [DataMember()]
        private long no = 0;
        public long NO 
        { 
            set 
            { 
                this.no = value;
                base.PropertyChangedHandler(this, "NO");
            } 
            get { return this.no; } 
        }

        [DataMember()]
        private string payment_cash_ymd;
        public string PAYMENT_CASH_YMD 
        { 
            set 
            {
                this.payment_cash_ymd = value;
                base.PropertyChangedHandler(this, "PAYMENT_CASH_YMD");
            }
            get { return this.payment_cash_ymd; } 
        }


        [DataMember()]
        private int input_person = 0;
        public int INPUT_PERSON 
        { 
            set 
            { 
                this.input_person = value;
                base.PropertyChangedHandler(this, "INPUT_PERSON");
            } 
            get { return this.input_person; } 
        }

        [DataMember()]
        private string input_person_nm = "";
        public string INPUT_PERSON_NM 
        { 
            set 
            { 
                this.input_person_nm = value;
                base.PropertyChangedHandler(this, "INPUT_PERSON_NM");
            } 
            get { return this.input_person_nm; } 
        }

        [DataMember()]
        private string purchase_id = "";
        public string PURCHASE_ID 
        { 
            set 
            {
                this.purchase_id = value;
                base.PropertyChangedHandler(this, "PURCHASE_ID");
            }
            get { return this.purchase_id; } 
        }

        [DataMember()]
        private string purchase_nm = "";
        public string PURCHASE_NM 
        { 
            set 
            {
                this.purchase_nm = value;
                base.PropertyChangedHandler(this, "PURCHASE_NM");
            }
            get { return this.purchase_nm; } 
        }

        [DataMember()]
        private double sum_price;
        public double SUM_PRICE
        {
            set
            {
                this.sum_price = value;
                base.PropertyChangedHandler(this, "SUM_PRICE");
            }
            get { return this.sum_price; }
        }

        [DataMember()]
        private string memo = "";
        public string MEMO 
        { 
            set 
            { 
                this.memo = value;
                base.PropertyChangedHandler(this, "MEMO");
            } 
            get { return this.memo; } 
        }

        [DataMember()]
        private int update_person_id = 0;
        public int UPDATE_PERSON_ID 
        { 
            set 
            {
                this.update_person_id = value;
                base.PropertyChangedHandler(this, "UPDATE_PERSON_ID");
            }
            get { return this.update_person_id; } 
        }

        [DataMember()]
        private string update_person_nm = "";
        public string UPDATE_PERSON_NM
        {
            set
            {
                this.update_person_nm = value;
                base.PropertyChangedHandler(this, "UPDATE_PERSON_NM");
            }
            get { return this.update_person_nm; }
        }

        [DataMember()]
        private int rec_no;
        public int REC_NO 
        { 
            set 
            { 
                this.rec_no = value;
                base.PropertyChangedHandler(this, "REC_NO");
            } 
            get { return this.rec_no; } 
        }

        [DataMember()]
        private int payment_cash_division_id;
        public int PAYMENT_CASH_DIVISION_ID 
        { 
            set 
            {
                this.payment_cash_division_id = value;
                base.PropertyChangedHandler(this, "PAYMENT_CASH_DIVISION_ID");
            }
            get { return this.payment_cash_division_id; } 
        }

        [DataMember()]
        private string payment_cash_division_nm;
        public string PAYMENT_CASH_DIVISION_NM
        {
            set
            {
                this.payment_cash_division_nm = value;
                base.PropertyChangedHandler(this, "PAYMENT_CASH_DIVISION_NM");
            }
            get { return this.payment_cash_division_nm; }
        }

        [DataMember()]
        private double price;
        public double PRICE 
        { 
            set 
            { 
                this.price = value;
                base.PropertyChangedHandler(this, "PRICE");
            } 
            get { return this.price; } 
        }

        [DataMember()]
        private string bill_site_day;
        public string BILL_SITE_DAY
        {
            set
            {
                this.bill_site_day = value;
                base.PropertyChangedHandler(this, "BILL_SITE_DAY");
            }
            get { return this.bill_site_day; }
        }

        [DataMember()]
        private string d_memo;
        public string D_MEMO 
        { 
            set 
            { 
                this.d_memo = value;
                base.PropertyChangedHandler(this, "D_MEMO");
            } 
            get { return this.d_memo; } 
        }

        [DataMember()]
        private string message;
        public string MESSAGE
        {
            set
            {
                this.message = value;
                base.PropertyChangedHandler(this, "MESSAGE");
            }
            get { return this.message; }
        }

        public EntityPaymentCash()
        {
        }
 
    }
}