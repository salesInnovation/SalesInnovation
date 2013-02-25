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
    public class EntityName : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

         [DataMember()]
        private int division_id = 0;
         public int DIVISION_ID { set { this.division_id = value; } get { return this.division_id; } }

        [DataMember()]
        private int id = 0;
        public int ID { set { this.id = value; } get { return this.id; } }

        [DataMember()]
        private string description = "";
        public string DESCRIPTION { set { this.description = value; } get { return this.description; } }

        [DataMember()]
        private string message;
        public string MESSAGE { set { this.message = value; } get { return this.message; } }

        public EntityName(int division_id
                        , int id
                        , string description)
        {
            this.division_id = division_id;
            this.id = id;
            this.description = description;
        }

        public EntityName()
        {
        }

    }
}