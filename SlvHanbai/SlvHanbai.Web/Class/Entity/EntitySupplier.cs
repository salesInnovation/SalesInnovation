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
    public class EntitySupplier : EntityBase
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
        private string _customer_id = "";
        public string customer_id
        {
            set
            {
                this._customer_id = value;
                base.PropertyChangedHandler(this, "customer_id");
            }
            get
            {
                return this._customer_id;
            }
        }

        [DataMember()]
        private string _customer_nm = "";
        public string customer_nm
        {
            set
            {
                this._customer_nm = value;
                base.PropertyChangedHandler(this, "customer_nm");
            }
            get
            {
                return this._customer_nm;
            }
        }

        [DataMember()]
        private int _divide_permission_id = 0;
        public int divide_permission_id
        {
            set
            {
                this._divide_permission_id = value;
                base.PropertyChangedHandler(this, "divide_permission_id");
            }
            get
            {
                return this._divide_permission_id;
            }
        }

        [DataMember()]
        private string _divide_permission_nm = "";
        public string divide_permission_nm
        {
            set
            {
                this._divide_permission_nm = value;
                base.PropertyChangedHandler(this, "divide_permission_nm");
            }
            get
            {
                return this._divide_permission_nm;
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

        public EntitySupplier()
        {
        }

    }
}