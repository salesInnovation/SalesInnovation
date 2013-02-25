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
    public class EntityReportSetting : EntityBase
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
        private int _size = 0;
        public int size
        {
            set
            {
                this._size = value;
                base.PropertyChangedHandler(this, "size");
            }
            get
            {
                return this._size;
            }
        }

        [DataMember()]
        private int _orientation = 0;
        public int orientation
        {
            set
            {
                this._orientation = value;
                base.PropertyChangedHandler(this, "orientation");
            }
            get
            {
                return this._orientation;
            }
        }

        [DataMember()]
        private double _left_margin = 0;
        public double left_margin
        {
            set
            {
                this._left_margin = value;
                base.PropertyChangedHandler(this, "left_margin");
            }
            get
            {
                return this._left_margin;
            }
        }

        [DataMember()]
        private double _right_margin = 0;
        public double right_margin
        {
            set
            {
                this._right_margin = value;
                base.PropertyChangedHandler(this, "right_margin");
            }
            get
            {
                return this._right_margin;
            }
        }

        [DataMember()]
        private double _top_margin = 0;
        public double top_margin
        {
            set
            {
                this._top_margin = value;
                base.PropertyChangedHandler(this, "top_margin");
            }
            get
            {
                return this._top_margin;
            }
        }

        [DataMember()]
        private double _bottom_margin = 0;
        public double bottom_margin
        {
            set
            {
                this._bottom_margin = value;
                base.PropertyChangedHandler(this, "bottom_margin");
            }
            get
            {
                return this._bottom_margin;
            }
        }

        [DataMember()]
        private string _group_id_from = "";
        public string group_id_from
        {
            set
            {
                this._group_id_from = value;
                base.PropertyChangedHandler(this, "group_id_from");
            }
            get
            {
                return this._group_id_from;
            }
        }

        [DataMember()]
        private string _group_nm_from = "";
        public string group_nm_from
        {
            set
            {
                this._group_nm_from = value;
                base.PropertyChangedHandler(this, "group_nm_from");
            }
            get
            {
                return this._group_nm_from;
            }
        }

        [DataMember()]
        private string _group_id_to = "";
        public string group_id_to
        {
            set
            {
                this._group_id_to = value;
                base.PropertyChangedHandler(this, "group_id_to");
            }
            get
            {
                return this._group_id_to;
            }
        }

        [DataMember()]
        private string _group_nm_to = "";
        public string group_nm_to
        {
            set
            {
                this._group_nm_to = value;
                base.PropertyChangedHandler(this, "group_nm_to");
            }
            get
            {
                return this._group_nm_to;
            }
        }

        [DataMember()]
        private int _group_total = 0;
        public int group_total
        {
            set
            {
                this._group_total = value;
                base.PropertyChangedHandler(this, "group_total");
            }
            get
            {
                return this._group_total;
            }
        }

        [DataMember()]
        private int _total_kbn = 0;
        public int total_kbn
        {
            set
            {
                this._total_kbn = value;
                base.PropertyChangedHandler(this, "total_kbn");
            }
            get
            {
                return this._total_kbn;
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

        public EntityReportSetting()
        {
        }

    }
}