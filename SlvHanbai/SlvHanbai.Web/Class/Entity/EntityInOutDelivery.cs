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
    public class EntityInOutDelivery : EntityBase
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
        private long cause_no = 0;
        public long CAUSE_NO
        {
            set
            {
                this.cause_no = value;
                base.PropertyChangedHandler(this, "CAUSE_NO");
            }
            get { return this.cause_no; }
        }

        [DataMember()]
        private string in_out_delivery_ymd;
        public string IN_OUT_DELIVERY_YMD 
        { 
            set 
            {
                this.in_out_delivery_ymd = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_YMD");
            }
            get { return this.in_out_delivery_ymd; } 
        }

        [DataMember()]
        private int in_out_delivery_kbn = 0;
        public int IN_OUT_DELIVERY_KBN
        {
            set
            {
                this.in_out_delivery_kbn = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_KBN");
            }
            get { return this.in_out_delivery_kbn; }
        }

        [DataMember()]
        private string in_out_delivery_kbn_nm = "";
        public string IN_OUT_DELIVERY_KBN_NM
        {
            set
            {
                this.in_out_delivery_kbn_nm = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_KBN_NM");
            }
            get { return this.in_out_delivery_kbn_nm; }
        }

        [DataMember()]
        private int in_out_delivery_proc_kbn = 0;
        public int IN_OUT_DELIVERY_PROC_KBN
        {
            set
            {
                this.in_out_delivery_proc_kbn = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_PROC_KBN");
            }
            get { return this.in_out_delivery_proc_kbn; }
        }

        [DataMember()]
        private string in_out_delivery_proc_kbn_nm = "";
        public string IN_OUT_DELIVERY_PROC_KBN_NM
        {
            set
            {
                this.in_out_delivery_proc_kbn_nm = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_PROC_KBN_NM");
            }
            get { return this.in_out_delivery_proc_kbn_nm; }
        }

        [DataMember()]
        private int in_out_delivery_to_kbn = 0;
        public int IN_OUT_DELIVERY_TO_KBN
        {
            set
            {
                this.in_out_delivery_to_kbn = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_TO_KBN");
            }
            get { return this.in_out_delivery_to_kbn; }
        }

        [DataMember()]
        private string in_out_delivery_to_kbn_nm = "";
        public string IN_OUT_DELIVERY_TO_KBN_NM
        {
            set
            {
                this.in_out_delivery_to_kbn_nm = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_TO_KBN_NM");
            }
            get { return this.in_out_delivery_to_kbn_nm; }
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
        private string in_out_delivery_to_id = "";
        public string IN_OUT_DELIVERY_TO_ID
        {
            set
            {
                this.in_out_delivery_to_id = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_TO_ID");
            }
            get { return this.in_out_delivery_to_id; }
        }

        [DataMember()]
        private string in_out_delivery_to_nm = "";
        public string IN_OUT_DELIVERY_TO_NM
        {
            set
            {
                this.in_out_delivery_to_nm = value;
                base.PropertyChangedHandler(this, "IN_OUT_DELIVERY_TO_NM");
            }
            get { return this.in_out_delivery_to_nm; }
        }

        [DataMember()]
        private int group_id_to = 0;
        public int GROUP_ID_TO
        {
            set
            {
                this.group_id_to = value;
                base.PropertyChangedHandler(this, "GROUP_ID_TO");
            }
            get
            {
                return this.group_id_to;
            }
        }

        [DataMember()]
        private string group_id_to_nm = "";
        public string GROUP_ID_TO_NM
        {
            set
            {
                this.group_id_to_nm = value;
                base.PropertyChangedHandler(this, "GROUP_ID_TO_NM");
            }
            get
            {
                return this.group_id_to_nm;
            }
        }

        [DataMember()]
        private string customer_id = "";
        public string CUSTOMER_ID 
        { 
            set 
            { 
                this.customer_id = value;
                base.PropertyChangedHandler(this, "CUSTOMER_ID");
            } 
            get { return this.customer_id; } 
        }

        [DataMember()]
        private string customer_nm = "";
        public string CUSTOMER_NM 
        { 
            set 
            { 
                this.customer_nm = value;
                base.PropertyChangedHandler(this, "CUSTOMER_NM");
            } 
            get { return this.customer_nm; } 
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
        private string purchase_name = "";
        public string PURCHASE_NAME
        {
            set
            {
                this.purchase_name = value;
                base.PropertyChangedHandler(this, "PURCHASE_NAME");
            }
            get { return this.purchase_name; }
        }

        [DataMember()]
        private double sum_enter_number;
        public double SUM_ENTER_NUMBER
        {
            set
            {
                this.sum_enter_number = value;
                base.PropertyChangedHandler(this, "SUM_ENTER_NUMBER");
            }
            get { return this.sum_enter_number; }
        }

        [DataMember()]
        private double sum_case_number;
        public double SUM_CASE_NUMBER
        {
            set
            {
                this.sum_case_number = value;
                base.PropertyChangedHandler(this, "SUM_CASE_NUMBER");
            }
            get { return this.sum_case_number; }
        }

        [DataMember()]
        private double sum_number;
        public double SUM_NUMBER
        {
            set
            {
                this.sum_number = value;
                base.PropertyChangedHandler(this, "SUM_NUMBER");
            }
            get { return this.sum_number; }
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
        private string commodity_id;
        public string COMMODITY_ID 
        { 
            set 
            {
                this.commodity_id = value;
                base.PropertyChangedHandler(this, "COMMODITY_ID");
            }
            get { return this.commodity_id; } 
        }

        [DataMember()]
        private string commodity_name;
        public string COMMODITY_NAME 
        { 
            set 
            {
                this.commodity_name = value;
                base.PropertyChangedHandler(this, "COMMODITY_NAME");
            }
            get { return this.commodity_name; } 
        }

        [DataMember()]
        private int unit_id;
        public int UNIT_ID 
        { 
            set 
            { 
                this.unit_id = value;
                base.PropertyChangedHandler(this, "UNIT_ID");
            } 
            get { return this.unit_id; } 
        }

        [DataMember()]
        private string unit_nm;
        public string UNIT_NM 
        { 
            set 
            { 
                this.unit_nm = value;
                base.PropertyChangedHandler(this, "UNIT_NM");
            } 
            get { return this.unit_nm; } 
        }

        [DataMember()]
        private double enter_number;
        public double ENTER_NUMBER 
        { 
            set 
            { 
                this.enter_number = value;
                base.PropertyChangedHandler(this, "ENTER_NUMBER");
            } 
            get { return this.enter_number; } 
        }

        [DataMember()]
        private double case_number;
        public double CASE_NUMBER
        {
            set
            {
                this.case_number = value;
                base.PropertyChangedHandler(this, "CASE_NUMBER");
            }
            get { return this.case_number; }
        }

        [DataMember()]
        private double number;
        public double NUMBER 
        { 
            set 
            { 
                this.number = value;
                base.PropertyChangedHandler(this, "NUMBER");
            } 
            get { return this.number; } 
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
                
        public EntityInOutDelivery()
        {
        }
 
    }
}