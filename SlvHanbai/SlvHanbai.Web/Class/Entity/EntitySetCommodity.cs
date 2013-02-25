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
    public class EntitySetCommodity : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private string _set_commodity_id = "";
        public string set_commodity_id
        {
            set
            {
                this._set_commodity_id = value;
                base.PropertyChangedHandler(this, "set_commodity_id");
            }
            get
            {
                return this._set_commodity_id;
            }
        }

        [DataMember()]
        private string _set_commodity_name = "";
        public string set_commodity_name
        {
            set
            {
                this._set_commodity_name = value;
                base.PropertyChangedHandler(this, "set_commodity_name");
            }
            get
            {
                return this._set_commodity_name;
            }
        }
        
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
        private double _enter_number = 0;
        public double enter_number
        {
            set
            {
                this._enter_number = value;
                base.PropertyChangedHandler(this, "enter_number");
            }
            get
            {
                return this._enter_number;
            }
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

        public EntitySetCommodity()
        {
        }

    }
}