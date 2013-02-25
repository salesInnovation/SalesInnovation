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
    public class EntityPerson : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private int _id = 0;
        public int id 
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
        private string _group_id = "";
        public string group_id
        {
            set
            {
                this._group_id = value;
                base.PropertyChangedHandler(this, "_group_id");
            }
            get
            {
                return this._group_id;
            }
        }

        [DataMember()]
        private string _group_nm = "";
        public string group_nm
        {
            set
            {
                this._group_nm = value;
                base.PropertyChangedHandler(this, "group_nm");
            }
            get
            {
                return this._group_nm;
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

        public EntityPerson(int id
                          , string name
                          , string group_id
                          , string group_nm
                          , string memo
                          , int display_division_id
                          , string display_division_nm
                          )
        {
            this.id = id;
            this.name = name;
            this.group_id = group_id;
            this.group_nm = group_nm;
            this.memo = memo;
            this.display_division_id = display_division_id;
            this.display_division_nm = display_division_nm;
        }

        public EntityPerson()
        {
        }

    }
}