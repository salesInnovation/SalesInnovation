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
    public class EntityDatePicker : EntityBase
    {

        private string _ymd_1 = "";
        public string ymd_1
        {
            set
            {
                this._ymd_1 = value;
                base.PropertyChangedHandler(this, "ymd_1");
            }
            get { return this._ymd_1; }
        }

        private string _ymd_2 = "";
        public string ymd_2
        {
            set
            {
                this._ymd_2 = value;
                base.PropertyChangedHandler(this, "ymd_2");
            }
            get { return this._ymd_2; }
        }

        private string _ymd_3 = "";
        public string ymd_3
        {
            set
            {
                this._ymd_3 = value;
                base.PropertyChangedHandler(this, "ymd_3");
            }
            get { return this._ymd_3; }
        }

        private string _ymd_4 = "";
        public string ymd_4
        {
            set
            {
                this._ymd_4 = value;
                base.PropertyChangedHandler(this, "ymd_4");
            }
            get { return this._ymd_4; }
        }

        private string _ymd_5 = "";
        public string ymd_5
        {
            set
            {
                this._ymd_5 = value;
                base.PropertyChangedHandler(this, "ymd_5");
            }
            get { return this._ymd_5; }
        }

        public EntityDatePicker() { }
    }
}
