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
    public class EntitySysLogin : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;
        public enum geLoginReturn { Normal = 0, Again, Failure, Warmn, Error }

        [DataMember()]
        private int _user_id = 0;
        public int user_id { set { this._user_id = value; } get { return this._user_id; } }

        [DataMember()]
        private string _user_nm = "";
        public string user_nm { set { this._user_nm = value; } get { return this._user_nm; } }

        [DataMember()]
        private int _login_flg = 0;
        public int login_flg { set { this._login_flg = value; } get { return this._login_flg; } }

        [DataMember()]
        private string _login_message = "";
        public string login_message { set { this._login_message = value; } get { return this._login_message; } }

        [DataMember()]
        private int _company_id = 0;
        public int company_id { set { this._company_id = value; } get { return this._company_id; } }

        [DataMember()]
        private string _company_nm = "";
        public string company_nm { set { this._company_nm = value; } get { return this._company_nm; } }

        [DataMember()]
        private int _group_id = 0;
        public int group_id { set { this._group_id = value; } get { return this._group_id; } }

        [DataMember()]
        private string _group_nm = "";
        public string group_nm { set { this._group_nm = value; } get { return this._group_nm; } }

        [DataMember()]
        private int _defult_person_id = 0;
        public int defult_person_id { set { this._defult_person_id = value; } get { return this._defult_person_id; } }

        [DataMember()]
        private string _defult_person_nm = "";
        public string defult_person_nm { set { this._defult_person_nm = value; } get { return this._defult_person_nm; } }

        [DataMember()]
        private string _group_display_name = "";
        public string group_display_name { set { this._group_display_name = value; } get { return this._group_display_name; } }

        [DataMember()]
        private int _evidence_flg = 0;
        public int evidence_flg { set { this._evidence_flg = value; } get { return this._evidence_flg; } }

        [DataMember()]
        private int _idFigureSlipNo = 0;
        public int idFigureSlipNo { set { this._idFigureSlipNo = value; } get { return this._idFigureSlipNo; } }

        [DataMember()]
        private int _idFigureCustomer = 0;
        public int idFigureCustomer { set { this._idFigureCustomer = value; } get { return this._idFigureCustomer; } }

        [DataMember()]
        private int _idFigurePurchase = 0;
        public int idFigurePurchase { set { this._idFigurePurchase = value; } get { return this._idFigurePurchase; } }

        [DataMember()]
        private int _idFigureGoods = 0;
        public int idFigureGoods { set { this._idFigureGoods = value; } get { return this._idFigureGoods; } }

        [DataMember()]
        private int _estimate_approval_flg = 0;
        public int estimate_approval_flg { set { this._estimate_approval_flg = value; } get { return this._estimate_approval_flg; } }

        [DataMember()]
        private int _receipt_account_invoice_print_flg = 0;
        public int receipt_account_invoice_print_flg { set { this._receipt_account_invoice_print_flg = value; } get { return this._receipt_account_invoice_print_flg; } }

        [DataMember()]
        private int _demo_flg = 0;
        public int demo_flg { set { this._demo_flg = value; } get { return this._demo_flg; } }

        [DataMember()]
        private string _sys_ver = "";
        public string sys_ver { set { this._sys_ver = value; } get { return this._sys_ver; } }

        [DataMember()]
        private string _session_string = "";
        public string session_string { set { this._session_string = value; } get { return this._session_string; } }

        public EntitySysLogin(int login_flg
                         , string login_message
                         , int company_id
                         , string company_nm
                         , int group_id
                         , string group_nm
                         , int defult_person_id
                         , string defult_person_nm
                         , string group_display_name
                         , int evidence_flg
                         , int idFigureSlipNo
                         , int idFigureCustomer
                         , int idFigurePurchase
                         , int idFigureGoods
                         , string session_string)
        {
            this.login_flg = login_flg;
            this.login_message = login_message;
            this.company_id = company_id;
            this.company_nm = company_nm;
            this.group_id = group_id;
            this.group_nm = group_nm;
            this.defult_person_id = defult_person_id;
            this.defult_person_nm = defult_person_nm;
            this.group_display_name = group_display_name;
            this.evidence_flg = evidence_flg;
            this.idFigureSlipNo = idFigureSlipNo;
            this.idFigureCustomer = idFigureCustomer;
            this.idFigurePurchase = idFigurePurchase;
            this.idFigureGoods = idFigureGoods;
            this.session_string = session_string;
        }

        public EntitySysLogin(int login_flg
                            , string login_message)
        {
            this.login_flg = login_flg;
            this.login_message = login_message;
        }

    }
}