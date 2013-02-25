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
        public class EntityDataFormReceiptD
    {

        private long _id = 0;
        public long id
        {
            set
            {
                this._id = value;
            }
            get { return this._id; }
        }

        private int _rec_no = 0;
        public int rec_no
        {
            set
            {
                this._rec_no = value;
            }
            get { return this._rec_no; }
        }

        private string _receipt_division_id = "";
        public string receipt_division_id
        {
            set
            {
                this._receipt_division_id = value;
            }
            get { return this._receipt_division_id; }
        }

        private string _receipt_division_nm = "";
        public string receipt_division_nm
        {
            set
            {
                this._receipt_division_nm = value;
            }
            get { return this._receipt_division_nm; }
        }

        private string _bill_site_day;
        public string bill_site_day
        {
            set
            {
                this._bill_site_day = value;
            }
            get { return this._bill_site_day; }
        }

        private double _price;
        public double price
        { 
            set 
            {
                this._price = value;
            }
            get { return this._price; } 
        }

        private string _memo;
        public string memo
        {
            set
            {
                this._memo = value;
            }
            get { return this._memo; }
        }

        public EntityDataFormReceiptD()
        {
        }
    }
}