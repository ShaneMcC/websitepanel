using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers.Virtualization
{
    public enum PerformanceType 
    { 
        Processor = 0,
        Memory = 1,
        Network = 2,
        DiskIO = 4
    }

    public partial class PerformanceDataValue : object, System.ComponentModel.INotifyPropertyChanged
    {
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private System.Nullable<double> SampleValueField;

        private System.DateTime TimeAddedField;

        private System.DateTime TimeSampledField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        public System.Nullable<double> SampleValue
        {
            get
            {
                return this.SampleValueField;
            }
            set
            {
                if ((this.SampleValueField.Equals(value) != true))
                {
                    this.SampleValueField = value;
                    this.RaisePropertyChanged("SampleValue");
                }
            }
        }

        public System.DateTime TimeAdded
        {
            get
            {
                return this.TimeAddedField;
            }
            set
            {
                if ((this.TimeAddedField.Equals(value) != true))
                {
                    this.TimeAddedField = value;
                    this.RaisePropertyChanged("TimeAdded");
                }
            }
        }

        public System.DateTime TimeSampled
        {
            get
            {
                return this.TimeSampledField;
            }
            set
            {
                if ((this.TimeSampledField.Equals(value) != true))
                {
                    this.TimeSampledField = value;
                    this.RaisePropertyChanged("TimeSampled");
                }
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
