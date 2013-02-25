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
    public class EntityEstimate : EntityBase
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
        private string no = "0";
        public string NO 
        { 
            set 
            { 
                this.no = value;
                base.PropertyChangedHandler(this, "NO");
            } 
            get { return this.no; } 
        }

        [DataMember()]
        private string estimate_ymd;
        public string ESTIMATE_YMD 
        { 
            set 
            {
                this.estimate_ymd = value;
                base.PropertyChangedHandler(this, "ESTIMATE_YMD");
            }
            get { return this.estimate_ymd; } 
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
        private string customer_person_name = "";
        public string CUSTOMER_PERSON_NAME
        {
            set
            {
                this.customer_person_name = value;
                base.PropertyChangedHandler(this, "CUSTOMER_PERSON_NAME");
            }
            get { return this.customer_person_name; }
        }
        
        [DataMember()]
        private string title_name = "";
        public string TITLE_NAME
        {
            set
            {
                this.title_name = value;
                base.PropertyChangedHandler(this, "TITLE_NAME");
            }
            get { return this.title_name; }
        }

        [DataMember()]
        private string title_name2 = "";
        public string TITLE_NAME2
        {
            set
            {
                this.title_name2 = value;
                base.PropertyChangedHandler(this, "TITLE_NAME2");
            }
            get { return this.title_name2; }
        }

        [DataMember()]
        private int tax_change_id = 0;
        public int TAX_CHANGE_ID 
        { 
            set 
            { 
                this.tax_change_id = value;
                base.PropertyChangedHandler(this, "TAX_CHANGE_ID");
            } 
            get { return this.tax_change_id; } 
        }

        [DataMember()]
        private string tax_change_nm = "";
        public string TAX_CHANGE_NM 
        { 
            set 
            { 
                this.tax_change_nm = value;
                base.PropertyChangedHandler(this, "TAX_CHANGE_NM");
            } 
            get { return this.tax_change_nm; } 
        }

        [DataMember()]
        private int business_division_id = 0;
        public int BUSINESS_DIVISION_ID 
        { 
            set 
            { 
                this.business_division_id = value;
                base.PropertyChangedHandler(this, "BUSINESS_DIVISION_ID");
            } 
            get { return this.business_division_id; } 
        }

        [DataMember()]
        private string business_division_nm = "";
        public string BUSINESS_DIVISION_NM 
        { 
            set 
            { 
                this.business_division_nm = value;
                base.PropertyChangedHandler(this, "BUSINESS_DIVISION_NM");
            } 
            get { return this.business_division_nm; } 
        }

        [DataMember()]
        private string supplier_id = "";
        public string SUPPLIER_ID 
        { 
            set 
            { 
                this.supplier_id = value;
                base.PropertyChangedHandler(this, "SUPPLIER_ID");
            } 
            get { return this.supplier_id; } 
        }

        [DataMember()]
        private string supplier_nm = "";
        public string SUPPLIER_NM 
        { 
            set 
            { 
                this.supplier_nm = value;
                base.PropertyChangedHandler(this, "SUPPLIER_NM");
            } 
            get { return this.supplier_nm; } 
        }

        [DataMember()]
        private string supply_ymd;
        public string SUPPLY_YMD 
        { 
            set 
            { 
                this.supply_ymd = value;
                base.PropertyChangedHandler(this, "SUPPLY_YMD");
            } 
            get { return this.supply_ymd; } 
        }

        [DataMember()]
        private string time_limit_ymd;
        public string TIME_LIMIT_YMD
        {
            set
            {
                this.time_limit_ymd = value;
                base.PropertyChangedHandler(this, "TIME_LIMIT_YMD");
            }
            get { return this.time_limit_ymd; }
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
        private string company_nm = "";
        public string COMPANY_NM
        {
            set
            {
                this.company_nm = value;
                base.PropertyChangedHandler(this, "COMPANY_NM");
            }
            get { return this.company_nm; }
        }

        [DataMember()]
        private string company_zip_code = "";
        public string COMPANY_ZIP_CODE
        {
            set
            {
                this.company_zip_code = value;
                base.PropertyChangedHandler(this, "COMPANY_ZIP_CODE");
            }
            get { return this.company_zip_code; }
        }

        [DataMember()]
        private string company_adress1 = "";
        public string COMPANY_ADRESS1
        {
            set
            {
                this.company_adress1 = value;
                base.PropertyChangedHandler(this, "COMPANY_ADRESS1");
            }
            get { return this.company_adress1; }
        }

        [DataMember()]
        private string company_adress2 = "";
        public string COMPANY_ADRESS2
        {
            set
            {
                this.company_adress2 = value;
                base.PropertyChangedHandler(this, "COMPANY_ADRESS2");
            }
            get { return this.company_adress2; }
        }

        [DataMember()]
        private string company_tel = "";
        public string COMPANY_TEL
        {
            set
            {
                this.company_tel = value;
                base.PropertyChangedHandler(this, "COMPANY_TEL");
            }
            get { return this.company_tel; }
        }

        [DataMember()]
        private string company_fax = "";
        public string COMPANY_FAX
        {
            set
            {
                this.company_fax = value;
                base.PropertyChangedHandler(this, "COMPANY_FAX");
            }
            get { return this.company_fax; }
        }

        [DataMember()]
        private string company_mail_adress = "";
        public string COMPANY_MAIL_ADRESS
        {
            set
            {
                this.company_mail_adress = value;
                base.PropertyChangedHandler(this, "COMPANY_MAIL_ADRESS");
            }
            get { return this.company_mail_adress; }
        }

        [DataMember()]
        private string company_url = "";
        public string COMPANY_URL
        {
            set
            {
                this.company_url = value;
                base.PropertyChangedHandler(this, "COMPANY_URL");
            }
            get { return this.company_url; }
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
        private int breakdown_id;
        public int BREAKDOWN_ID 
        { 
            set 
            { 
                this.breakdown_id = value;
                base.PropertyChangedHandler(this, "BREAKDOWN_ID");
            } 
            get { return this.breakdown_id; } 
        }

        [DataMember()]
        private string breakdown_nm;
        public string BREAKDOWN_NM 
        { 
            set 
            { 
                this.breakdown_nm = value;
                base.PropertyChangedHandler(this, "BREAKDOWN_NM");
            }
            get { return this.breakdown_nm; } 
        }

        [DataMember()]
        private int deliver_division_id;
        public int DELIVER_DIVISION_ID 
        { 
            set 
            { 
                this.deliver_division_id = value;
                base.PropertyChangedHandler(this, "DELIVER_DIVISION_ID");
            } 
            get { return this.deliver_division_id; } 
        }

        [DataMember()]
        private string deliver_division_nm;
        public string DELIVER_DIVISION_NM 
        { 
            set 
            { 
                this.deliver_division_nm = value;
                base.PropertyChangedHandler(this, "DELIVER_DIVISION_NM");
            } 
            get { return this.deliver_division_nm; } 
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
        private double unit_price;
        public double UNIT_PRICE 
        { 
            set 
            { 
                this.unit_price = value;
                base.PropertyChangedHandler(this, "UNIT_PRICE");
            } 
            get { return this.unit_price; } 
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

        // 受注数
        [DataMember()]
        private double _order_number;
        public double order_number
        {
            set
            {
                this._order_number = value;
                base.PropertyChangedHandler(this, "order_number");
            }
            get { return this._order_number; }
        }


        // 受注残数
        [DataMember()]
        private double _order_stay_number;
        public double order_stay_number
        {
            set
            {
                this._order_stay_number = value;
                base.PropertyChangedHandler(this, "order_stay_number");
            }
            get { return this._order_stay_number; }
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

        public EntityEstimate()
        {
        }
 
    }
}