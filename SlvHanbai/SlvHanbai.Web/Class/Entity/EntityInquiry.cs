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
    public class EntityInquiry : EntityBase
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
        private string _date_time;
        public string date_time
        {
            set
            {
                this._date_time = value;
                base.PropertyChangedHandler(this, "date_time");
            }
            get { return this._date_time; }
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
        private int _inquiry_division_id = 0;
        public int inquiry_division_id
        {
            set
            {
                this._inquiry_division_id = value;
                base.PropertyChangedHandler(this, "inquiry_division_id");
            }
            get
            {
                return this._inquiry_division_id;
            }
        }

        [DataMember()]
        private string _inquiry_division_nm = "";
        public string inquiry_division_nm
        {
            set
            {
                this._inquiry_division_nm = value;
                base.PropertyChangedHandler(this, "inquiry_division_nm");
            }
            get
            {
                return this._inquiry_division_nm;
            }
        }

        [DataMember()]
        private int _inquiry_level_id = 0;
        public int inquiry_level_id
        {
            set
            {
                this._inquiry_level_id = value;
                base.PropertyChangedHandler(this, "inquiry_level_id");
            }
            get
            {
                return this._inquiry_level_id;
            }
        }

        [DataMember()]
        private string _inquiry_level_nm = "";
        public string inquiry_level_nm
        {
            set
            {
                this._inquiry_level_nm = value;
                base.PropertyChangedHandler(this, "inquiry_level_nm");
            }
            get
            {
                return this._inquiry_level_nm;
            }
        }

        [DataMember()]
        private int _inquiry_level_state_id = 0;
        public int inquiry_level_state_id
        {
            set
            {
                this._inquiry_level_state_id = value;
                base.PropertyChangedHandler(this, "inquiry_level_state_id");
            }
            get
            {
                return this._inquiry_level_state_id;
            }
        }

        [DataMember()]
        private string _inquiry_level_state_nm = "";
        public string inquiry_level_state_nm
        {
            set
            {
                this._inquiry_level_state_nm = value;
                base.PropertyChangedHandler(this, "inquiry_level_state_nm");
            }
            get
            {
                return this._inquiry_level_state_nm;
            }
        }

        [DataMember()]
        private int _rec_no = 0;
        public int rec_no
        {
            set
            {
                this._rec_no = value;
                base.PropertyChangedHandler(this, "rec_no");
            }
            get
            {
                return this._rec_no;
            }
        }

        [DataMember()]
        private string _d_date_time;
        public string d_date_time
        {
            set
            {
                this._d_date_time = value;
                base.PropertyChangedHandler(this, "d_date_time");
            }
            get { return this._d_date_time; }
        }

        [DataMember()]
        private int _kbn = 0;
        public int kbn
        {
            set
            {
                this._kbn = value;
                base.PropertyChangedHandler(this, "kbn");
            }
            get
            {
                return this._kbn;
            }
        }

        [DataMember()]
        private int _d_person_id = 0;
        public int d_person_id
        {
            set
            {
                this._d_person_id = value;
                base.PropertyChangedHandler(this, "d_person_id");
            }
            get
            {
                return this._d_person_id;
            }
        }

        [DataMember()]
        private string _d_person_nm = "";
        public string d_person_nm
        {
            set
            {
                this._d_person_nm = value;
                base.PropertyChangedHandler(this, "d_person_nm");
            }
            get
            {
                return this._d_person_nm;
            }
        }

        [DataMember()]
        private string _support_person_nm = "";
        public string support_person_nm
        {
            set
            {
                this._support_person_nm = value;
                base.PropertyChangedHandler(this, "support_person_nm");
            }
            get
            {
                return this._support_person_nm;
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

        public EntityInquiry()
        {
        }

    }
}