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
    public class EntityMstData : EntityBase
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
        private string attribute4 = "";
        public string ATTRIBUTE4 { set { this.attribute4 = value; } get { return this.attribute4; } }

        [DataMember()]
        private string attribute5 = "";
        public string ATTRIBUTE5 { set { this.attribute5 = value; } get { return this.attribute5; } }

        [DataMember()]
        private string attribute6 = "";
        public string ATTRIBUTE6 { set { this.attribute6 = value; } get { return this.attribute6; } }

        [DataMember()]
        private string attribute7 = "";
        public string ATTRIBUTE7 { set { this.attribute7 = value; } get { return this.attribute7; } }

        [DataMember()]
        private string attribute8 = "";
        public string ATTRIBUTE8 { set { this.attribute8 = value; } get { return this.attribute8; } }

        [DataMember()]
        private string attribute9 = "";
        public string ATTRIBUTE9 { set { this.attribute9 = value; } get { return this.attribute9; } }

        [DataMember()]
        private string attribute10 = "";
        public string ATTRIBUTE10 { set { this.attribute10 = value; } get { return this.attribute10; } }

        [DataMember()]
        private string attribute11 = "";
        public string ATTRIBUTE11 { set { this.attribute11 = value; } get { return this.attribute11; } }

        [DataMember()]
        private string attribute12 = "";
        public string ATTRIBUTE12 { set { this.attribute12 = value; } get { return this.attribute12; } }

        [DataMember()]
        private string attribute13 = "";
        public string ATTRIBUTE13 { set { this.attribute13 = value; } get { return this.attribute13; } }

        [DataMember()]
        private string attribute14 = "";
        public string ATTRIBUTE14 { set { this.attribute14 = value; } get { return this.attribute14; } }

        [DataMember()]
        private string attribute15 = "";
        public string ATTRIBUTE15 { set { this.attribute15 = value; } get { return this.attribute15; } }

        [DataMember()]
        private string attribute16 = "";
        public string ATTRIBUTE16 { set { this.attribute16 = value; } get { return this.attribute16; } }

        [DataMember()]
        private string attribute17 = "";
        public string ATTRIBUTE17 { set { this.attribute17 = value; } get { return this.attribute17; } }

        [DataMember()]
        private string attribute18 = "";
        public string ATTRIBUTE18 { set { this.attribute18 = value; } get { return this.attribute18; } }

        [DataMember()]
        private string attribute19 = "";
        public string ATTRIBUTE19 { set { this.attribute19 = value; } get { return this.attribute19; } }

        [DataMember()]
        private string attribute20 = "";
        public string ATTRIBUTE20 { set { this.attribute20 = value; } get { return this.attribute20; } }

        [DataMember()]
        private string message;
        public string MESSAGE { set { this.message = value; } get { return this.message; } }

        public EntityMstData(string id
                           , string id2
                           , string name
            )
        {
            this.id = id;
            this.id2 = id2;
            this.name = name;
        }

        public EntityMstData()
        {
        }

    }
}