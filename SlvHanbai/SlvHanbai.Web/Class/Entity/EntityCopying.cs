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
    public class EntityCopying : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private bool _ret = false;
        public bool ret
        {
            set
            {
                this._ret = value;
                base.PropertyChangedHandler(this, "ret");
            }
            get
            {
                return this._ret;
            }
        }

        [DataMember()]
        private bool _is_exists_data = false;
        public bool is_exists_data
        {
            set
            {
                this._is_exists_data = value;
                base.PropertyChangedHandler(this, "is_exists_data");
            }
            get
            {
                return this._is_exists_data;
            }
        }

        [DataMember()]
        private bool _is_lock_success = false;
        public bool is_lock_success
        {
            set
            {
                this._is_lock_success = value;
                base.PropertyChangedHandler(this, "is_lock_success");
            }
            get
            {
                return this._is_lock_success;
            }
        }


        [DataMember()]
        private string _message = "";
        public string MESSAGE
        {
            set
            {
                this._message = value;
                base.PropertyChangedHandler(this, "MESSAGE");
            }
            get { return this._message; }
        }

        public EntityCopying()
        {
        }

    }
}