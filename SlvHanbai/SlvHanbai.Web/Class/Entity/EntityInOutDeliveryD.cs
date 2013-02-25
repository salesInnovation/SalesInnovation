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
    public class EntityInOutDeliveryD : EntityBase
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
        private long _rec_no;
        public long rec_no
        { 
            set 
            {
                this._rec_no = value;
                base.PropertyChangedHandler(this, "rec_no");
            }
            get { return this._rec_no; } 
        }

        [DataMember()]
        private string _commodity_id;
        public string commodity_id
        { 
            set 
            {
                this._commodity_id = value;
                base.PropertyChangedHandler(this, "commodity_id");
            }
            get { return this._commodity_id; } 
        }

        [DataMember()]
        private string _commodity_name;
        public string commodity_name
        { 
            set 
            {
                this._commodity_name = value;
                base.PropertyChangedHandler(this, "commodity_name");
            }
            get { return this._commodity_name; } 
        }

        [DataMember()]
        private int _unit_id;
        public int unit_id
        { 
            set 
            {
                this._unit_id = value;
                base.PropertyChangedHandler(this, "unit_id");
            }
            get { return this._unit_id; } 
        }

        [DataMember()]
        private string _unit_nm;
        public string unit_nm
        { 
            set 
            {
                this._unit_nm = value;
                base.PropertyChangedHandler(this, "unit_nm");
            }
            get { return this._unit_nm; } 
        }

        [DataMember()]
        private double _enter_number;
        public double enter_number
        { 
            set 
            {
                this._enter_number = value;
                base.PropertyChangedHandler(this, "enter_number");
            }
            get { return this._enter_number; } 
        }

        [DataMember()]
        private double _case_number;
        public double case_number
        {
            set
            {
                this._case_number = value;
                base.PropertyChangedHandler(this, "case_number");
            }
            get { return this._case_number; }
        }

        [DataMember()]
        private double _number;
        public double number
        { 
            set 
            {
                this._number = value;
                base.PropertyChangedHandler(this, "number");
            }
            get { return this._number; } 
        }

        // 在庫管理区分
        [DataMember()]
        private int _inventory_management_division_id;
        public int inventory_management_division_id
        {
            set
            {
                this._inventory_management_division_id = value;
                base.PropertyChangedHandler(this, "inventory_management_division_id");
            }
            get { return this._inventory_management_division_id; }
        }

        // 現在庫
        [DataMember()]
        private double _inventory_number;
        public double inventory_number
        {
            set
            {
                this._inventory_number = value;
                base.PropertyChangedHandler(this, "inventory_number");
            }
            get { return this._inventory_number; }
        }

        // 数量小数桁
        [DataMember()]
        private int _number_decimal_digit;
        public int number_decimal_digit
        {
            set
            {
                this._number_decimal_digit = value;
                base.PropertyChangedHandler(this, "number_decimal_digit");
            }
            get { return this._number_decimal_digit; }
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

        public EntityInOutDeliveryD()
        {
        }
    }
}