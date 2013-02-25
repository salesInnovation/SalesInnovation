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
    public class EntityPurchaseOrder : EntityBase
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
        private string purchase_order_ymd;
        public string PURCHASE_ORDER_YMD 
        { 
            set 
            {
                this.purchase_order_ymd = value;
                base.PropertyChangedHandler(this, "PURCHASE_ORDER");
            }
            get { return this.purchase_order_ymd; } 
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
                base.PropertyChangedHandler(this, "PURCHASE_ID ");
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
        private int send_kbn_id = 0;
        public int SEND_KBN_ID
        { 
            set 
            { 
                this.send_kbn_id = value;
                base.PropertyChangedHandler(this, "SEND_KBN_ID");
            } 
            get { return this.send_kbn_id; } 
        }

        [DataMember()]
        private string send_kbn_nm = "";
        public string SEND_KBN_NM 
        { 
            set 
            { 
                this.send_kbn_nm = value;
                base.PropertyChangedHandler(this, "SEND_KBN_NM");
            } 
            get { return this.send_kbn_nm; } 
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

        public EntityPurchaseOrder()
        {
        }
 
    }
}