using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SlvHanbaiClient.Class.Entity
{
    public class EntitySearch : EntityBase
    {
        private string _customer_id = "";
        public string customer_id
        {
            set
            {
                this._customer_id = value;
                base.PropertyChangedHandler(this, "customer_id");
            }
            get { return this._customer_id; }
        }

        private string _customer_id_from = "";
        public string customer_id_from
        {
            set
            {
                this._customer_id_from = value;
                base.PropertyChangedHandler(this, "customer_id_from");
            }
            get { return this._customer_id_from; }
        }

        private string _customer_id_to = "";
        public string customer_id_to
        {
            set
            {
                this._customer_id_to = value;
                base.PropertyChangedHandler(this, "customer_id_to");
            }
            get { return this._customer_id_to; }
        }

        private string _invoice_id = "";
        public string invoice_id
        {
            set
            {
                this._invoice_id = value;
                base.PropertyChangedHandler(this, "invoice_id");
            }
            get { return this._invoice_id; }
        }

        private string _invoice_id_from = "";
        public string invoice_id_from
        {
            set
            {
                this._invoice_id_from = value;
                base.PropertyChangedHandler(this, "invoice_id_from");
            }
            get { return this._invoice_id_from; }
        }

        private string _invoice_id_to = "";
        public string invoice_id_to
        {
            set
            {
                this._invoice_id_to = value;
                base.PropertyChangedHandler(this, "invoice_id_to");
            }
            get { return this._invoice_id_to; }
        }



        private string _purchase_id = "";
        public string purchase_id
        {
            set
            {
                this._purchase_id = value;
                base.PropertyChangedHandler(this, "purchase_id");
            }
            get { return this._purchase_id; }
        }

        private string _purchase_id_from = "";
        public string purchase_id_from
        {
            set
            {
                this._purchase_id_from = value;
                base.PropertyChangedHandler(this, "purchase_id_from");
            }
            get { return this._purchase_id_from; }
        }

        private string _purchase_id_to = "";
        public string purchase_id_to
        {
            set
            {
                this._purchase_id_to = value;
                base.PropertyChangedHandler(this, "purchase_id_to");
            }
            get { return this._purchase_id_to; }
        }



        private string _commodity_id = "";
        public string commodity_id
        {
            set
            {
                this._commodity_id = value;
                base.PropertyChangedHandler(this, "commodity_id");
            }
            get { return this._commodity_id; }
        }

        private string _commodity_id_from = "";
        public string commodity_id_from
        {
            set
            {
                this._commodity_id_from = value;
                base.PropertyChangedHandler(this, "commodity_id_from");
            }
            get { return this._commodity_id_from; }
        }

        private string _commodity_id_to = "";
        public string commodity_id_to
        {
            set
            {
                this._commodity_id_to = value;
                base.PropertyChangedHandler(this, "commodity_id_to");
            }
            get { return this._commodity_id_to; }
        }

        private string _supplier_id = "";
        public string supplier_id
        {
            set
            {
                this._supplier_id = value;
                base.PropertyChangedHandler(this, "supplier_id");
            }
            get { return this._supplier_id; }
        }

        private string _supplier_id_from = "";
        public string supplier_id_from
        {
            set
            {
                this._supplier_id_from = value;
                base.PropertyChangedHandler(this, "supplier_id_from");
            }
            get { return this._supplier_id_from; }
        }

        private string _supplier_id_to = "";
        public string supplier_id_to
        {
            set
            {
                this._supplier_id_to = value;
                base.PropertyChangedHandler(this, "supplier_id_to");
            }
            get { return this._supplier_id_to; }
        }

        private string _update_person_id = "";
        public string update_person_id
        {
            set
            {
                this._update_person_id = value;
                base.PropertyChangedHandler(this, "update_person_id");
            }
            get { return this._update_person_id; }
        }

        private string _person_id_from = "";
        public string person_id_from
        {
            set
            {
                this._person_id_from = value;
                base.PropertyChangedHandler(this, "person_id_from");
            }
            get { return this._person_id_from; }
        }

        private string _person_id_to = "";
        public string person_id_to
        {
            set
            {
                this._person_id_to = value;
                base.PropertyChangedHandler(this, "person_id_to");
            }
            get { return this._person_id_to; }
        }

        private string _person_id = "";
        public string person_id
        {
            set
            {
                this._person_id = value;
                base.PropertyChangedHandler(this, "person_id");
            }
            get { return this._person_id; }
        }

        private string _group_id = "";
        public string group_id
        {
            set
            {
                this._group_id = value;
                base.PropertyChangedHandler(this, "group_id");
            }
            get { return this._group_id; }
        }

        private string _summing_up_group_id = "";
        public string summing_up_group_id
        {
            set
            {
                this._summing_up_group_id = value;
                base.PropertyChangedHandler(this, "summing_up_group_id");
            }
            get { return this._summing_up_group_id; }
        }

        private string _receipt_division_id = "";
        public string receipt_division_id
        {
            set
            {
                this._receipt_division_id = value;
                base.PropertyChangedHandler(this, "receipt_division_id");
            }
            get { return this._receipt_division_id; }
        }

        private string _group1_id = "";
        public string group1_id
        {
            set
            {
                this._group1_id = value;
                base.PropertyChangedHandler(this, "group1_id");
            }
            get { return this._group1_id; }
        }

        private string _main_purchase_id = "";
        public string main_purchase_id
        {
            set
            {
                this._main_purchase_id = value;
                base.PropertyChangedHandler(this, "main_purchase_id");
            }
            get { return this._main_purchase_id; }
        }

        private string _condition_id_from = "";
        public string condition_id_from
        {
            set
            {
                this._condition_id_from = value;
                base.PropertyChangedHandler(this, "condition_id_from");
            }
            get { return this._condition_id_from; }
        }

        private string _condition_id_to = "";
        public string condition_id_to
        {
            set
            {
                this._condition_id_to = value;
                base.PropertyChangedHandler(this, "condition_id_to");
            }
            get { return this._condition_id_to; }
        }

        private string _class_id_from = "";
        public string class_id_from
        {
            set
            {
                this._class_id_from = value;
                base.PropertyChangedHandler(this, "class_id_from");
            }
            get { return this._class_id_from; }
        }

        private string _class_id_to = "";
        public string class_id_to
        {
            set
            {
                this._class_id_to = value;
                base.PropertyChangedHandler(this, "class_id_to");
            }
            get { return this._class_id_to; }
        }

        public EntitySearch() { }
    }
}
