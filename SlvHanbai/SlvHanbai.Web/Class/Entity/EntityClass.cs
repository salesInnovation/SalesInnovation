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
    public class EntityClass : EntityBase
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
        private int _rec_no;
        public int rec_no
        {
            set
            {
                this._rec_no = value;
                base.PropertyChangedHandler(this, "rec_no");
            }
            get { return this._rec_no; }
        }

        [DataMember()]
        private int _class_divition_id = 0;
        public int class_divition_id
        {
            set
            {
                this._class_divition_id = value;
                base.PropertyChangedHandler(this, "class_divition_id");
            }
            get
            {
                return this._class_divition_id;
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

        public EntityClass()
        {
        }

    }
}