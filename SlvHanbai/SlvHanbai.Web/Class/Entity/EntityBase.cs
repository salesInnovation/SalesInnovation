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
    public abstract class EntityBase : INotifyPropertyChanged
    {
        protected void PropertyChangedHandler(EntityBase sender,
            string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, new PropertyChangedEventArgs(
                    propertyName));
        }
        public virtual event PropertyChangedEventHandler PropertyChanged;
    }

}