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
    public class EntityReport : EntityBase
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
        private string _downLoadUrl = "";
        public string downLoadUrl
        {
            set
            {
                this._downLoadUrl = value;
                base.PropertyChangedHandler(this, "downLoadUrl");
            }
            get
            {
                return this._downLoadUrl;
            }
        }

        [DataMember()]
        private string _downLoadFilePath = "";
        public string downLoadFilePath
        {
            set
            {
                this._downLoadFilePath = value;
                base.PropertyChangedHandler(this, "downLoadFilePath");
            }
            get
            {
                return this._downLoadFilePath;
            }
        }

        [DataMember()]
        private string _downLoadFileName = "";
        public string downLoadFileName
        {
            set
            {
                this._downLoadFileName = value;
                base.PropertyChangedHandler(this, "downLoadFileName");
            }
            get
            {
                return this._downLoadFileName;
            }
        }

        [DataMember()]
        private string _downLoadFileSize = "";
        public string downLoadFileSize
        {
            set
            {
                this._downLoadFileSize = value;
                base.PropertyChangedHandler(this, "downLoadFileSize");
            }
            get
            {
                return this._downLoadFileSize;
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

        public EntityReport()
        {
        }

    }
}