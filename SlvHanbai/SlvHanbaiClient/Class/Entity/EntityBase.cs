using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SlvHanbaiClient.Class.Entity
{
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