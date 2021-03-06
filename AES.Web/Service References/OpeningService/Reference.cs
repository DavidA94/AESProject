﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AES.Web.OpeningService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JobOpeningContract", Namespace="http://schemas.datacontract.org/2004/07/AES.OpeningsSvc.Contracts")]
    [System.SerializableAttribute()]
    public partial class JobOpeningContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int JobIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LongDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int OpeningIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PositionsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RequestNotesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShortDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int StoreIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StoreManagerNotesField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string titleField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int JobID {
            get {
                return this.JobIDField;
            }
            set {
                if ((this.JobIDField.Equals(value) != true)) {
                    this.JobIDField = value;
                    this.RaisePropertyChanged("JobID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LongDescription {
            get {
                return this.LongDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.LongDescriptionField, value) != true)) {
                    this.LongDescriptionField = value;
                    this.RaisePropertyChanged("LongDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OpeningID {
            get {
                return this.OpeningIDField;
            }
            set {
                if ((this.OpeningIDField.Equals(value) != true)) {
                    this.OpeningIDField = value;
                    this.RaisePropertyChanged("OpeningID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Positions {
            get {
                return this.PositionsField;
            }
            set {
                if ((this.PositionsField.Equals(value) != true)) {
                    this.PositionsField = value;
                    this.RaisePropertyChanged("Positions");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RequestNotes {
            get {
                return this.RequestNotesField;
            }
            set {
                if ((object.ReferenceEquals(this.RequestNotesField, value) != true)) {
                    this.RequestNotesField = value;
                    this.RaisePropertyChanged("RequestNotes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ShortDescription {
            get {
                return this.ShortDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.ShortDescriptionField, value) != true)) {
                    this.ShortDescriptionField = value;
                    this.RaisePropertyChanged("ShortDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int StoreID {
            get {
                return this.StoreIDField;
            }
            set {
                if ((this.StoreIDField.Equals(value) != true)) {
                    this.StoreIDField = value;
                    this.RaisePropertyChanged("StoreID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StoreManagerNotes {
            get {
                return this.StoreManagerNotesField;
            }
            set {
                if ((object.ReferenceEquals(this.StoreManagerNotesField, value) != true)) {
                    this.StoreManagerNotesField = value;
                    this.RaisePropertyChanged("StoreManagerNotes");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string title {
            get {
                return this.titleField;
            }
            set {
                if ((object.ReferenceEquals(this.titleField, value) != true)) {
                    this.titleField = value;
                    this.RaisePropertyChanged("title");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="OpeningService.IOpeningSvc")]
    public interface IOpeningSvc {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetApprovedOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/GetApprovedOpeningsResponse")]
        AES.Web.OpeningService.JobOpeningContract[] GetApprovedOpenings(int StoreID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetApprovedOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/GetApprovedOpeningsResponse")]
        System.Threading.Tasks.Task<AES.Web.OpeningService.JobOpeningContract[]> GetApprovedOpeningsAsync(int StoreID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetJobName", ReplyAction="http://tempuri.org/IOpeningSvc/GetJobNameResponse")]
        string GetJobName(int jobID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetJobName", ReplyAction="http://tempuri.org/IOpeningSvc/GetJobNameResponse")]
        System.Threading.Tasks.Task<string> GetJobNameAsync(int jobID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/RequestOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/RequestOpeningsResponse")]
        bool RequestOpenings(int StoreID, AES.Web.OpeningService.JobOpeningContract opening, int number);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/RequestOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/RequestOpeningsResponse")]
        System.Threading.Tasks.Task<bool> RequestOpeningsAsync(int StoreID, AES.Web.OpeningService.JobOpeningContract opening, int number);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetPendingOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/GetPendingOpeningsResponse")]
        AES.Web.OpeningService.JobOpeningContract[] GetPendingOpenings(int StoreID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetPendingOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/GetPendingOpeningsResponse")]
        System.Threading.Tasks.Task<AES.Web.OpeningService.JobOpeningContract[]> GetPendingOpeningsAsync(int StoreID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetRejectedOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/GetRejectedOpeningsResponse")]
        AES.Web.OpeningService.JobOpeningContract[] GetRejectedOpenings(int StoreID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/GetRejectedOpenings", ReplyAction="http://tempuri.org/IOpeningSvc/GetRejectedOpeningsResponse")]
        System.Threading.Tasks.Task<AES.Web.OpeningService.JobOpeningContract[]> GetRejectedOpeningsAsync(int StoreID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/ApproveOpening", ReplyAction="http://tempuri.org/IOpeningSvc/ApproveOpeningResponse")]
        bool ApproveOpening(AES.Web.OpeningService.JobOpeningContract opening, string notes);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/ApproveOpening", ReplyAction="http://tempuri.org/IOpeningSvc/ApproveOpeningResponse")]
        System.Threading.Tasks.Task<bool> ApproveOpeningAsync(AES.Web.OpeningService.JobOpeningContract opening, string notes);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/RejectOpening", ReplyAction="http://tempuri.org/IOpeningSvc/RejectOpeningResponse")]
        bool RejectOpening(AES.Web.OpeningService.JobOpeningContract opening, string notes);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IOpeningSvc/RejectOpening", ReplyAction="http://tempuri.org/IOpeningSvc/RejectOpeningResponse")]
        System.Threading.Tasks.Task<bool> RejectOpeningAsync(AES.Web.OpeningService.JobOpeningContract opening, string notes);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IOpeningSvcChannel : AES.Web.OpeningService.IOpeningSvc, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class OpeningSvcClient : System.ServiceModel.ClientBase<AES.Web.OpeningService.IOpeningSvc>, AES.Web.OpeningService.IOpeningSvc {
        
        public OpeningSvcClient() {
        }
        
        public OpeningSvcClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public OpeningSvcClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OpeningSvcClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public OpeningSvcClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public AES.Web.OpeningService.JobOpeningContract[] GetApprovedOpenings(int StoreID) {
            return base.Channel.GetApprovedOpenings(StoreID);
        }
        
        public System.Threading.Tasks.Task<AES.Web.OpeningService.JobOpeningContract[]> GetApprovedOpeningsAsync(int StoreID) {
            return base.Channel.GetApprovedOpeningsAsync(StoreID);
        }
        
        public string GetJobName(int jobID) {
            return base.Channel.GetJobName(jobID);
        }
        
        public System.Threading.Tasks.Task<string> GetJobNameAsync(int jobID) {
            return base.Channel.GetJobNameAsync(jobID);
        }
        
        public bool RequestOpenings(int StoreID, AES.Web.OpeningService.JobOpeningContract opening, int number) {
            return base.Channel.RequestOpenings(StoreID, opening, number);
        }
        
        public System.Threading.Tasks.Task<bool> RequestOpeningsAsync(int StoreID, AES.Web.OpeningService.JobOpeningContract opening, int number) {
            return base.Channel.RequestOpeningsAsync(StoreID, opening, number);
        }
        
        public AES.Web.OpeningService.JobOpeningContract[] GetPendingOpenings(int StoreID) {
            return base.Channel.GetPendingOpenings(StoreID);
        }
        
        public System.Threading.Tasks.Task<AES.Web.OpeningService.JobOpeningContract[]> GetPendingOpeningsAsync(int StoreID) {
            return base.Channel.GetPendingOpeningsAsync(StoreID);
        }
        
        public AES.Web.OpeningService.JobOpeningContract[] GetRejectedOpenings(int StoreID) {
            return base.Channel.GetRejectedOpenings(StoreID);
        }
        
        public System.Threading.Tasks.Task<AES.Web.OpeningService.JobOpeningContract[]> GetRejectedOpeningsAsync(int StoreID) {
            return base.Channel.GetRejectedOpeningsAsync(StoreID);
        }
        
        public bool ApproveOpening(AES.Web.OpeningService.JobOpeningContract opening, string notes) {
            return base.Channel.ApproveOpening(opening, notes);
        }
        
        public System.Threading.Tasks.Task<bool> ApproveOpeningAsync(AES.Web.OpeningService.JobOpeningContract opening, string notes) {
            return base.Channel.ApproveOpeningAsync(opening, notes);
        }
        
        public bool RejectOpening(AES.Web.OpeningService.JobOpeningContract opening, string notes) {
            return base.Channel.RejectOpening(opening, notes);
        }
        
        public System.Threading.Tasks.Task<bool> RejectOpeningAsync(AES.Web.OpeningService.JobOpeningContract opening, string notes) {
            return base.Channel.RejectOpeningAsync(opening, notes);
        }
    }
}
