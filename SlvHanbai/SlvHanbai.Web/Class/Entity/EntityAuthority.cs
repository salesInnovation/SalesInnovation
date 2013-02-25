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
    public class EntityAuthority : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private int _user_id = 0;
        public int user_id
        {
            set
            {
                this._user_id = value;
                base.PropertyChangedHandler(this, "user_id");
            }
            get
            {
                return this._user_id;
            }
        }

        [DataMember()]
        private string _pg_id = "";
        public string pg_id
        {
            set
            {
                this._pg_id = value;
                base.PropertyChangedHandler(this, "pg_id");
            }
            get
            {
                return this._pg_id;
            }
        }

        [DataMember()]
        private int _authority_kbn = 0;
        public int authority_kbn
        {
            set
            {
                this._authority_kbn = value;
                base.PropertyChangedHandler(this, "authority_kbn");
            }
            get
            {
                return this._authority_kbn;
            }
        }

        [DataMember()]
        private int _display_index = 0;
        public int display_index
        {
            set
            {
                this._display_index = value;
                base.PropertyChangedHandler(this, "display_index");
            }
            get
            {
                return this._display_index;
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

        public EntityAuthority()
        {
        }

    }
}