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
    public class EntityStockInventory : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

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
        private double _account_inventory_number = 0;
        public double account_inventory_number
        {
            set
            {
                this._account_inventory_number = value;
                base.PropertyChangedHandler(this, "account_inventory_number");
            }
            get
            {
                return this._account_inventory_number;
            }
        }

        [DataMember()]
        private double _practice_inventory_number = 0;
        public double practice_inventory_number
        {
            set
            {
                this._practice_inventory_number = value;
                base.PropertyChangedHandler(this, "practice_inventory_number");
            }
            get
            {
                return this._practice_inventory_number;
            }
        }

        [DataMember()]
        private double _diff_number = 0;
        public double diff_number
        {
            set
            {
                this._diff_number = value;
                base.PropertyChangedHandler(this, "diff_number");
            }
            get
            {
                return this._diff_number;
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
        private int _payment_exists_flg = 0;
        public int payment_exists_flg
        {
            set
            {
                this._payment_exists_flg = value;
                base.PropertyChangedHandler(this, "payment_exists_flg");
            }
            get
            {
                return this._payment_exists_flg;
            }
        }

        [DataMember()]
        private bool _exec_flg = false;
        public bool exec_flg
        {
            set
            {
                this._exec_flg = value;
                base.PropertyChangedHandler(this, "exec_flg");
            }
            get
            {
                return this._exec_flg;
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

        public EntityStockInventory()
        {
        }

    }
}