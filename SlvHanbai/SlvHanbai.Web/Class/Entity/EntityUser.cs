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
using SlvHanbai.Web.Class.Utility;

#endregion

namespace SlvHanbai.Web.Class.Entity
{
    [DataContract]
    public class EntityUser : EntityBase
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
        private int _company_id = 0;
        public int company_id
        {
            set
            {
                this._company_id = value;
                base.PropertyChangedHandler(this, "company_id");
            }
            get
            {
                return this._company_id;
            }
        }

        [DataMember()]
        private string _company_nm = "";
        public string company_nm
        {
            set
            {
                this._company_nm = value;
                base.PropertyChangedHandler(this, "company_nm");
            }
            get
            {
                return this._company_nm;
            }
        }

        [DataMember()]
        private int _group_id = 0;
        public int group_id
        {
            set
            {
                this._group_id = ExCast.zCInt(value);
                base.PropertyChangedHandler(this, "group_id");
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
        private string _login_id = "";
        public string login_id
        {
            set
            {
                this._login_id = value;
                base.PropertyChangedHandler(this, "login_id");
            }
            get
            {
                return this._login_id;
            }
        }

        [DataMember()]
        private string _after_login_id = "";
        public string after_login_id
        {
            set
            {
                this._after_login_id = value;
                base.PropertyChangedHandler(this, "after_login_id");
            }
            get
            {
                return this._after_login_id;
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
        private string _login_password = "";
        public string login_password
        {
            set
            {
                this._login_password = value;
                base.PropertyChangedHandler(this, "login_password");
            }
            get
            {
                return this._login_password;
            }
        }

        [DataMember()]
        private int _person_id = 0;
        public int person_id
        {
            set
            {
                this._person_id = ExCast.zCInt(value);
                base.PropertyChangedHandler(this, "person_id");
            }
            get
            {
                return this._person_id;
            }
        }

        [DataMember()]
        private string _person_nm = "";
        public string person_nm
        {
            set
            {
                this._person_nm = value;
                base.PropertyChangedHandler(this, "person_nm");
            }
            get
            {
                return this._person_nm;
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

        public EntityUser()
        {
        }

    }
}