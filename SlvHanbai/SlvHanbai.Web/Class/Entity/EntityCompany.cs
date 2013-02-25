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
    public class EntityCompany : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

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
        private string _group_display_name = "";
        public string group_display_name
        {
            set
            {
                this._group_display_name = value;
                base.PropertyChangedHandler(this, "group_display_name");
            }
            get
            {
                return this._group_display_name;
            }
        }

        [DataMember()]
        private int _id_figure_slip_no = 10;
        public int id_figure_slip_no
        {
            set
            {
                this._id_figure_slip_no = value;
                base.PropertyChangedHandler(this, "id_figure_slip_no");
            }
            get
            {
                return this._id_figure_slip_no;
            }
        }

        [DataMember()]
        private int _id_figure_customer = 10;
        public int id_figure_customer
        {
            set
            {
                this._id_figure_customer = value;
                base.PropertyChangedHandler(this, "id_figure_customer");
            }
            get
            {
                return this._id_figure_customer;
            }
        }

        [DataMember()]
        private int _id_figure_purchase = 10;
        public int id_figure_purchase
        {
            set
            {
                this._id_figure_purchase = value;
                base.PropertyChangedHandler(this, "id_figure_purchase");
            }
            get
            {
                return this._id_figure_purchase;
            }
        }

        [DataMember()]
        private int _id_figure_commodity = 10;
        public int id_figure_commodity
        {
            set
            {
                this._id_figure_commodity = value;
                base.PropertyChangedHandler(this, "id_figure_commodity");
            }
            get
            {
                return this._id_figure_commodity;
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
        private string _estimate_ymd = "";
        public string estimate_ymd
        {
            set
            {
                this._estimate_ymd = value;
                base.PropertyChangedHandler(this, "estimate_ymd");
            }
            get
            {
                return this._estimate_ymd;
            }
        }

        [DataMember()]
        private string _order_ymd = "";
        public string order_ymd
        {
            set
            {
                this._order_ymd = value;
                base.PropertyChangedHandler(this, "order_ymd");
            }
            get
            {
                return this._order_ymd;
            }
        }

        [DataMember()]
        private string _sales_ymd = "";
        public string sales_ymd
        {
            set
            {
                this._sales_ymd = value;
                base.PropertyChangedHandler(this, "sales_ymd");
            }
            get
            {
                return this._sales_ymd;
            }
        }

        [DataMember()]
        private string _receipt_ymd = "";
        public string receipt_ymd
        {
            set
            {
                this._receipt_ymd = value;
                base.PropertyChangedHandler(this, "receipt_ymd");
            }
            get
            {
                return this._receipt_ymd;
            }
        }

        [DataMember()]
        private string _purchase_order_ymd = "";
        public string purchase_order_ymd
        {
            set
            {
                this._purchase_order_ymd = value;
                base.PropertyChangedHandler(this, "purchase_order_ymd");
            }
            get
            {
                return this._purchase_order_ymd;
            }
        }

        [DataMember()]
        private string _purchase_ymd = "";
        public string purchase_ymd
        {
            set
            {
                this._purchase_ymd = value;
                base.PropertyChangedHandler(this, "purchase_ymd");
            }
            get
            {
                return this._purchase_ymd;
            }
        }

        [DataMember()]
        private string _cash_payment_ymd = "";
        public string cash_payment_ymd
        {
            set
            {
                this._cash_payment_ymd = value;
                base.PropertyChangedHandler(this, "cash_payment_ymd");
            }
            get
            {
                return this._cash_payment_ymd;
            }
        }

        [DataMember()]
        private string _produce_ymd = "";
        public string produce_ymd
        {
            set
            {
                this._produce_ymd = value;
                base.PropertyChangedHandler(this, "produce_ymd");
            }
            get
            {
                return this._produce_ymd;
            }
        }

        [DataMember()]
        private string _ship_ymd = "";
        public string ship_ymd
        {
            set
            {
                this._ship_ymd = value;
                base.PropertyChangedHandler(this, "ship_ymd");
            }
            get
            {
                return this._ship_ymd;
            }
        }

        [DataMember()]
        private long _estimate_no = 0;
        public long estimate_no
        {
            set
            {
                this._estimate_no = value;
                base.PropertyChangedHandler(this, "estimate_no");
            }
            get
            {
                return this._estimate_no;
            }
        }

        [DataMember()]
        private long _order_no = 0;
        public long order_no
        {
            set
            {
                this._order_no = value;
                base.PropertyChangedHandler(this, "order_no");
            }
            get
            {
                return this._order_no;
            }
        }

        [DataMember()]
        private long _sales_no = 0;
        public long sales_no
        {
            set
            {
                this._sales_no = value;
                base.PropertyChangedHandler(this, "sales_no");
            }
            get
            {
                return this._sales_no;
            }
        }

        [DataMember()]
        private long _receipt_no = 0;
        public long receipt_no
        {
            set
            {
                this._receipt_no = value;
                base.PropertyChangedHandler(this, "receipt_no");
            }
            get
            {
                return this._receipt_no;
            }
        }

        [DataMember()]
        private long _purchase_order_no = 0;
        public long purchase_order_no
        {
            set
            {
                this._purchase_order_no = value;
                base.PropertyChangedHandler(this, "purchase_order_no");
            }
            get
            {
                return this._purchase_order_no;
            }
        }

        [DataMember()]
        private long _purchase_no = 0;
        public long purchase_no
        {
            set
            {
                this._purchase_no = value;
                base.PropertyChangedHandler(this, "purchase_no");
            }
            get
            {
                return this._purchase_no;
            }
        }

        [DataMember()]
        private long _cash_payment_no = 0;
        public long cash_payment_no
        {
            set
            {
                this._cash_payment_no = value;
                base.PropertyChangedHandler(this, "cash_payment_no");
            }
            get
            {
                return this._cash_payment_no;
            }
        }

        [DataMember()]
        private long _produce_no = 0;
        public long produce_no
        {
            set
            {
                this._produce_no = value;
                base.PropertyChangedHandler(this, "produce_no");
            }
            get
            {
                return this._produce_no;
            }
        }

        [DataMember()]
        private long _ship_no = 0;
        public long ship_no
        {
            set
            {
                this._ship_no = value;
                base.PropertyChangedHandler(this, "ship_no");
            }
            get
            {
                return this._ship_no;
            }
        }

        [DataMember()]
        private long _estimate_cnt = 0;
        public long estimate_cnt
        {
            set
            {
                this._estimate_cnt = value;
                base.PropertyChangedHandler(this, "estimate_cnt");
            }
            get
            {
                return this._estimate_cnt;
            }
        }

        [DataMember()]
        private long _order_cnt = 0;
        public long order_cnt
        {
            set
            {
                this._order_cnt = value;
                base.PropertyChangedHandler(this, "order_cnt");
            }
            get
            {
                return this._order_cnt;
            }
        }

        [DataMember()]
        private long _sales_cnt = 0;
        public long sales_cnt
        {
            set
            {
                this._sales_cnt = value;
                base.PropertyChangedHandler(this, "sales_cnt");
            }
            get
            {
                return this._sales_cnt;
            }
        }

        [DataMember()]
        private long _receipt_cnt = 0;
        public long receipt_cnt
        {
            set
            {
                this._receipt_cnt = value;
                base.PropertyChangedHandler(this, "receipt_cnt");
            }
            get
            {
                return this._receipt_cnt;
            }
        }

        [DataMember()]
        private long _purchase_order_cnt = 0;
        public long purchase_order_cnt
        {
            set
            {
                this._purchase_order_cnt = value;
                base.PropertyChangedHandler(this, "purchase_order_cnt");
            }
            get
            {
                return this._purchase_order_cnt;
            }
        }

        [DataMember()]
        private long _purchase_cnt = 0;
        public long purchase_cnt
        {
            set
            {
                this._purchase_cnt = value;
                base.PropertyChangedHandler(this, "purchase_cnt");
            }
            get
            {
                return this._purchase_cnt;
            }
        }

        [DataMember()]
        private long _cash_payment_cnt = 0;
        public long cash_payment_cnt
        {
            set
            {
                this._cash_payment_cnt = value;
                base.PropertyChangedHandler(this, "cash_payment_cnt");
            }
            get
            {
                return this._cash_payment_cnt;
            }
        }

        [DataMember()]
        private long _produce_cnt = 0;
        public long produce_cnt
        {
            set
            {
                this._produce_cnt = value;
                base.PropertyChangedHandler(this, "produce_cnt");
            }
            get
            {
                return this._produce_cnt;
            }
        }

        [DataMember()]
        private long _ship_cnt = 0;
        public long ship_cnt
        {
            set
            {
                this._ship_cnt = value;
                base.PropertyChangedHandler(this, "ship_cnt");
            }
            get
            {
                return this._ship_cnt;
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

        public EntityCompany()
        {
        }

    }
}