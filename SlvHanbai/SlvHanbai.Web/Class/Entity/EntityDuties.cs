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
    public class EntityDuties : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private long _no = 0;
        public long no
        {
            set
            {
                this._no = value;
                base.PropertyChangedHandler(this, "no");
            }
            get
            {
                return this._no;
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
        private int _gropu_id = 0;
        public int gropu_id
        {
            set
            {
                this._gropu_id = value;
                base.PropertyChangedHandler(this, "gropu_id");
            }
            get
            {
                return this._gropu_id;
            }
        }

        [DataMember()]
        private string _gropu_nm = "";
        public string gropu_nm
        {
            set
            {
                this._gropu_nm = value;
                base.PropertyChangedHandler(this, "gropu_nm");
            }
            get
            {
                return this._gropu_nm;
            }
        }

        [DataMember()]
        private string _duties_ymd;
        public string duties_ymd
        {
            set
            {
                this._duties_ymd = value;
                base.PropertyChangedHandler(this, "duties_ymd");
            }
            get { return this._duties_ymd; }
        }

        [DataMember()]
        private string _duties_time;
        public string duties_time
        {
            set
            {
                this._duties_time = value;
                base.PropertyChangedHandler(this, "duties_time");
            }
            get { return this._duties_time; }
        }

        [DataMember()]
        private string _duties_date_time;
        public string duties_date_time
        {
            set
            {
                this._duties_date_time = value;
                base.PropertyChangedHandler(this, "duties_date_time");
            }
            get { return this._duties_date_time; }
        }

        [DataMember()]
        private int _person_id = 0;
        public int person_id
        {
            set
            {
                this._person_id = value;
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
        private int _duties_level_id = 0;
        public int duties_level_id
        {
            set
            {
                this._duties_level_id = value;
                base.PropertyChangedHandler(this, "duties_level_id");
            }
            get
            {
                return this._duties_level_id;
            }
        }

        [DataMember()]
        private string _duties_level_nm = "";
        public string duties_level_nm
        {
            set
            {
                this._duties_level_nm = value;
                base.PropertyChangedHandler(this, "duties_level_nm");
            }
            get
            {
                return this._duties_level_nm;
            }
        }

        [DataMember()]
        private int _duties_state_id = 0;
        public int duties_state_id
        {
            set
            {
                this._duties_state_id = value;
                base.PropertyChangedHandler(this, "duties_state_id");
            }
            get
            {
                return this._duties_state_id;
            }
        }

        [DataMember()]
        private string _duties_state_nm = "";
        public string duties_state_nm
        {
            set
            {
                this._duties_state_nm = value;
                base.PropertyChangedHandler(this, "duties_state_nm");
            }
            get
            {
                return this._duties_state_nm;
            }
        }

        [DataMember()]
        private string _title;
        public string title
        {
            set
            {
                this._title = value;
                base.PropertyChangedHandler(this, "title");
            }
            get { return this._title; }
        }

        [DataMember()]
        private string _content = "";
        public string content
        {
            set
            {
                this._content = value;
                base.PropertyChangedHandler(this, "content");
            }
            get
            {
                return this._content;
            }
        }

        [DataMember()]
        private string _upload_file_path1 = "";
        public string upload_file_path1
        {
            set
            {
                this._upload_file_path1 = value;
                base.PropertyChangedHandler(this, "upload_file_path1");
            }
            get
            {
                return this._upload_file_path1;
            }
        }

        [DataMember()]
        private string _upload_file_path2 = "";
        public string upload_file_path2
        {
            set
            {
                this._upload_file_path2 = value;
                base.PropertyChangedHandler(this, "upload_file_path2");
            }
            get
            {
                return this._upload_file_path2;
            }
        }

        [DataMember()]
        private string _upload_file_path3 = "";
        public string upload_file_path3
        {
            set
            {
                this._upload_file_path3 = value;
                base.PropertyChangedHandler(this, "upload_file_path3");
            }
            get
            {
                return this._upload_file_path3;
            }
        }

        [DataMember()]
        private string _upload_file_path4 = "";
        public string upload_file_path4
        {
            set
            {
                this._upload_file_path4 = value;
                base.PropertyChangedHandler(this, "upload_file_path4");
            }
            get
            {
                return this._upload_file_path4;
            }
        }

        [DataMember()]
        private string _upload_file_path5 = "";
        public string upload_file_path5
        {
            set
            {
                this._upload_file_path5 = value;
                base.PropertyChangedHandler(this, "upload_file_path5");
            }
            get
            {
                return this._upload_file_path5;
            }
        }

        [DataMember()]
        private string _upload_file_name1 = "";
        public string upload_file_name1
        {
            set
            {
                this._upload_file_name1 = value;
                base.PropertyChangedHandler(this, "upload_file_name1");
            }
            get
            {
                return this._upload_file_name1;
            }
        }

        [DataMember()]
        private string _upload_file_name2 = "";
        public string upload_file_name2
        {
            set
            {
                this._upload_file_name2 = value;
                base.PropertyChangedHandler(this, "upload_file_name2");
            }
            get
            {
                return this._upload_file_name2;
            }
        }

        [DataMember()]
        private string _upload_file_name3 = "";
        public string upload_file_name3
        {
            set
            {
                this._upload_file_name3 = value;
                base.PropertyChangedHandler(this, "upload_file_name3");
            }
            get
            {
                return this._upload_file_name3;
            }
        }

        [DataMember()]
        private string _upload_file_name4 = "";
        public string upload_file_name4
        {
            set
            {
                this._upload_file_name4 = value;
                base.PropertyChangedHandler(this, "upload_file_name4");
            }
            get
            {
                return this._upload_file_name4;
            }
        }

        [DataMember()]
        private string _upload_file_name5 = "";
        public string upload_file_name5
        {
            set
            {
                this._upload_file_name5 = value;
                base.PropertyChangedHandler(this, "upload_file_name5");
            }
            get
            {
                return this._upload_file_name5;
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

        public EntityDuties()
        {
        }

    }
}