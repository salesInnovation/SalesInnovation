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
    public class EntityMstList : EntityBase
    {
        public override event PropertyChangedEventHandler  PropertyChanged;

        [DataMember()]
        private string id = "";
        public string ID { set { this.id = value; } get { return this.id; } }

        private string id2 = "";
        public string ID2 { set { this.id = value; } get { return this.id; } }

        [DataMember()]
        private string name = "";
        public string NAME { set { this.name = value; } get { return this.name; } }

        [DataMember()]
        private string attribute1 = "";
        public string ATTRIBUTE1 { set { this.attribute1 = value; } get { return this.attribute1; } }

        [DataMember()]
        private string attribute2 = "";
        public string ATTRIBUTE2 { set { this.attribute2 = value; } get { return this.attribute2; } }

        [DataMember()]
        private string attribute3 = "";
        public string ATTRIBUTE3 { set { this.attribute3 = value; } get { return this.attribute3; } }

        [DataMember()]
        private string message;
        public string MESSAGE { set { this.message = value; } get { return this.message; } }

        public EntityMstList(string id
                           , string id2
                           , string name
            )
        {
            this.id = id;
            this.id2 = id2;
            this.name = name;
        }

        public EntityMstList()
        {
        }

    }
}