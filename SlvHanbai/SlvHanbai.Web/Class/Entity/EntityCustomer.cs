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
    public class EntityCustomer : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private string _id = "";
        public string id
        {
            set
            {
                this._id = value;
                base.PropertyChangedHandler(this, "id");
            }
            get
            {
                return this._id;
            }
        }

        [DataMember()]
        private string _name = "";
        public string name
        {
            set
            {
                this._name = value;
                base.PropertyChangedHandler(this, "name");
            }
            get
            {
                return this._name;
            }
        }

        [DataMember()]
        private string _kana = "";
        public string kana
        {
            set
            {
                this._kana = value;
                base.PropertyChangedHandler(this, "kana");
            }
            get
            {
                return this._kana;
            }
        }

        [DataMember()]
        private string _about_name = "";
        public string about_name
        {
            set
            {
                this._about_name = value;
                base.PropertyChangedHandler(this, "about_name");
            }
            get
            {
                return this._about_name;
            }
        }

        [DataMember()]
        private string _zip_code_from = "";
        public string zip_code_from
        {
            set
            {
                this._zip_code_from = value;
                base.PropertyChangedHandler(this, "zip_code_from");
            }
            get
            {
                return this._zip_code_from;
            }
        }

        [DataMember()]
        private string _zip_code_to = "";
        public string zip_code_to
        {
            set
            {
                this._zip_code_to = value;
                base.PropertyChangedHandler(this, "zip_code_to");
            }
            get
            {
                return this._zip_code_to;
            }
        }

        [DataMember()]
        private int _prefecture_id = 0;
        public int prefecture_id
        {
            set
            {
                this._prefecture_id = value;
                base.PropertyChangedHandler(this, "prefecture_id");
            }
            get
            {
                return this._prefecture_id;
            }
        }

        [DataMember()]
        private int _city_id = 0;
        public int city_id
        {
            set
            {
                this._city_id = value;
                base.PropertyChangedHandler(this, "city_id");
            }
            get
            {
                return this._city_id;
            }
        }

        [DataMember()]
        private int _town_id = 0;
        public int town_id
        {
            set
            {
                this._town_id = value;
                base.PropertyChangedHandler(this, "town_id");
            }
            get
            {
                return this._town_id;
            }
        }

        [DataMember()]
        private string _adress_city = "";
        public string adress_city
        {
            set
            {
                this._adress_city = value;
                base.PropertyChangedHandler(this, "adress_city");
            }
            get
            {
                return this._adress_city;
            }
        }

        [DataMember()]
        private string _adress_town = "";
        public string adress_town
        {
            set
            {
                this._adress_town = value;
                base.PropertyChangedHandler(this, "adress_town");
            }
            get
            {
                return this._adress_town;
            }
        }

        [DataMember()]
        private string _adress1 = "";
        public string adress1
        {
            set
            {
                this._adress1 = value;
                base.PropertyChangedHandler(this, "adress1");
            }
            get
            {
                return this._adress1;
            }
        }

        [DataMember()]
        private string _adress2 = "";
        public string adress2
        {
            set
            {
                this._adress2 = value;
                base.PropertyChangedHandler(this, "adress2");
            }
            get
            {
                return this._adress2;
            }
        }

        [DataMember()]
        private string _station_name = "";
        public string station_name
        {
            set
            {
                this._station_name = value;
                base.PropertyChangedHandler(this, "station_name");
            }
            get
            {
                return this._station_name;
            }
        }

        [DataMember()]
        private string _post_name = "";
        public string post_name
        {
            set
            {
                this._post_name = value;
                base.PropertyChangedHandler(this, "post_name");
            }
            get
            {
                return this._post_name;
            }
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
            get
            {
                return this._person_name;
            }
        }

        [DataMember()]
        private int _title_id = 0;
        public int title_id
        {
            set
            {
                this._title_id = value;
                base.PropertyChangedHandler(this, "title_id");
            }
            get
            {
                return this._title_id;
            }
        }

        [DataMember()]
        private string _title_name = "";
        public string title_name
        {
            set
            {
                this._title_name = value;
                base.PropertyChangedHandler(this, "title_name");
            }
            get
            {
                return this._title_name;
            }
        }

        [DataMember()]
        private string _tel = "";
        public string tel
        {
            set
            {
                this._tel = value;
                base.PropertyChangedHandler(this, "tel");
            }
            get
            {
                return this._tel;
            }
        }

        [DataMember()]
        private string _fax = "";
        public string fax
        {
            set
            {
                this._fax = value;
                base.PropertyChangedHandler(this, "fax");
            }
            get
            {
                return this._fax;
            }
        }

        [DataMember()]
        private string _mail_adress = "";
        public string mail_adress
        {
            set
            {
                this._mail_adress = value;
                base.PropertyChangedHandler(this, "mail_adress");
            }
            get
            {
                return this._mail_adress;
            }
        }

        [DataMember()]
        private string _mobile_tel = "";
        public string mobile_tel
        {
            set
            {
                this._mobile_tel = value;
                base.PropertyChangedHandler(this, "mobile_tel");
            }
            get
            {
                return this._mobile_tel;
            }
        }

        [DataMember()]
        private string _mobile_adress = "";
        public string mobile_adress
        {
            set
            {
                this._mobile_adress = value;
                base.PropertyChangedHandler(this, "mobile_adress");
            }
            get
            {
                return this._mobile_adress;
            }
        }

        [DataMember()]
        private string _url = "";
        public string url
        {
            set
            {
                this._url = value;
                base.PropertyChangedHandler(this, "url");
            }
            get
            {
                return this._url;
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
        private int _business_division_id = 0;
        public int business_division_id
        {
            set
            {
                this._business_division_id = value;
                base.PropertyChangedHandler(this, "business_division_id");
            }
            get
            {
                return this._business_division_id;
            }
        }

        [DataMember()]
        private string _business_division_nm = "";
        public string business_division_nm
        {
            set
            {
                this._business_division_nm = value;
                base.PropertyChangedHandler(this, "business_division_nm");
            }
            get
            {
                return this._business_division_nm;
            }
        }

        [DataMember()]
        private int _unit_kind_id = 0;
        public int unit_kind_id
        {
            set
            {
                this._unit_kind_id = value;
                base.PropertyChangedHandler(this, "unit_kind_id");
            }
            get
            {
                return this._unit_kind_id;
            }
        }

        [DataMember()]
        private string _unit_kind_nm = "";
        public string unit_kind_nm
        {
            set
            {
                this._unit_kind_nm = value;
                base.PropertyChangedHandler(this, "unit_kind_nm");
            }
            get
            {
                return this._unit_kind_nm;
            }
        }

        [DataMember()]
        private int _credit_rate = 0;
        public int credit_rate
        {
            set
            {
                this._credit_rate = value;
                base.PropertyChangedHandler(this, "credit_rate");
            }
            get
            {
                return this._credit_rate;
            }
        }

        [DataMember()]
        private int _tax_change_id = 0;
        public int tax_change_id
        {
            set
            {
                this._tax_change_id = value;
                base.PropertyChangedHandler(this, "tax_change_id");
            }
            get
            {
                return this._tax_change_id;
            }
        }

        [DataMember()]
        private string _tax_change_nm = "";
        public string tax_change_nm
        {
            set
            {
                this._tax_change_nm = value;
                base.PropertyChangedHandler(this, "tax_change_nm");
            }
            get
            {
                return this._tax_change_nm;
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
        private int _price_fraction_proc_id = 0;
        public int price_fraction_proc_id
        {
            set
            {
                this._price_fraction_proc_id = value;
                base.PropertyChangedHandler(this, "price_fraction_proc_id");
            }
            get
            {
                return this._price_fraction_proc_id;
            }
        }

        [DataMember()]
        private string _price_fraction_proc_nm = "";
        public string price_fraction_proc_nm
        {
            set
            {
                this._price_fraction_proc_nm = value;
                base.PropertyChangedHandler(this, "price_fraction_proc_nm");
            }
            get
            {
                return this._price_fraction_proc_nm;
            }
        }

        [DataMember()]
        private int _tax_fraction_proc_id = 0;
        public int tax_fraction_proc_id
        {
            set
            {
                this._tax_fraction_proc_id = value;
                base.PropertyChangedHandler(this, "tax_fraction_proc_id");
            }
            get
            {
                return this._tax_fraction_proc_id;
            }
        }

        [DataMember()]
        private string _tax_fraction_proc_nm = "";
        public string tax_fraction_proc_nm
        {
            set
            {
                this._tax_fraction_proc_nm = value;
                base.PropertyChangedHandler(this, "tax_fraction_proc_nm");
            }
            get
            {
                return this._tax_fraction_proc_nm;
            }
        }

        [DataMember()]
        private double _credit_limit_price = 0;
        public double credit_limit_price
        {
            set
            {
                this._credit_limit_price = value;
                base.PropertyChangedHandler(this, "credit_limit_price");
            }
            get
            {
                return this._credit_limit_price;
            }
        }

        [DataMember()]
        private double _sales_credit_price = 0;
        public double sales_credit_price
        {
            set
            {
                this._sales_credit_price = value;
                base.PropertyChangedHandler(this, "sales_credit_price");
            }
            get
            {
                return this._sales_credit_price;
            }
        }

        [DataMember()]
        private string _receipt_division_id = "";
        public string receipt_division_id
        {
            set
            {
                this._receipt_division_id = value;
                base.PropertyChangedHandler(this, "receipt_division_id");
            }
            get
            {
                return this._receipt_division_id;
            }
        }

        [DataMember()]
        private string _receipt_division_nm = "";
        public string receipt_division_nm
        {
            set
            {
                this._receipt_division_nm = value;
                base.PropertyChangedHandler(this, "receipt_division_nm");
            }
            get
            {
                return this._receipt_division_nm;
            }
        }

        [DataMember()]
        private int _collect_cycle_id = 0;
        public int collect_cycle_id
        {
            set
            {
                this._collect_cycle_id = value;
                base.PropertyChangedHandler(this, "collect_cycle_id");
            }
            get
            {
                return this._collect_cycle_id;
            }
        }

        [DataMember()]
        private string _collect_cycle_nm = "";
        public string collect_cycle_nm
        {
            set
            {
                this._collect_cycle_nm = value;
                base.PropertyChangedHandler(this, "collect_cycle_nm");
            }
            get
            {
                return this._collect_cycle_nm;
            }
        }

        [DataMember()]
        private int _collect_day = 0;
        public int collect_day
        {
            set
            {
                this._collect_day = value;
                base.PropertyChangedHandler(this, "collect_day");
            }
            get
            {
                return this._collect_day;
            }
        }

        [DataMember()]
        private int _bill_site = 0;
        public int bill_site
        {
            set
            {
                this._bill_site = value;
                base.PropertyChangedHandler(this, "bill_site");
            }
            get
            {
                return this._bill_site;
            }
        }

        [DataMember()]
        private string _group1_id = "";
        public string group1_id
        {
            set
            {
                this._group1_id = value;
                base.PropertyChangedHandler(this, "group1_id");
            }
            get
            {
                return this._group1_id;
            }
        }

        [DataMember()]
        private string _group1_nm = "";
        public string group1_nm
        {
            set
            {
                this._group1_nm = value;
                base.PropertyChangedHandler(this, "group1_nm");
            }
            get
            {
                return this._group1_nm;
            }
        }

        [DataMember()]
        private string _group2_id = "";
        public string group2_id
        {
            set
            {
                this._group2_id = value;
                base.PropertyChangedHandler(this, "group2_id");
            }
            get
            {
                return this._group2_id;
            }
        }

        [DataMember()]
        private string _group2_nm = "";
        public string group2_nm
        {
            set
            {
                this._group2_nm = value;
                base.PropertyChangedHandler(this, "group2_nm");
            }
            get
            {
                return this._group2_nm;
            }
        }

        [DataMember()]
        private string _group3_id = "";
        public string group3_id
        {
            set
            {
                this._group3_id = value;
                base.PropertyChangedHandler(this, "group3_id");
            }
            get
            {
                return this._group3_id;
            }
        }

        [DataMember()]
        private string _group3_nm = "";
        public string group3_nm
        {
            set
            {
                this._group3_nm = value;
                base.PropertyChangedHandler(this, "group3_nm");
            }
            get
            {
                return this._group3_nm;
            }
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
        private int _display_division_id = 0;
        public int display_division_id
        {
            set
            {
                this._display_division_id = value;
                base.PropertyChangedHandler(this, "display_division_id");
            }
            get
            {
                return this._display_division_id;
            }
        }

        [DataMember()]
        private string _display_division_nm = "";
        public string display_division_nm
        {
            set
            {
                this._display_division_nm = value;
                base.PropertyChangedHandler(this, "display_division_nm");
            }
            get
            {
                return this._display_division_nm;
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

        public EntityCustomer()
        {
        }

    }
}