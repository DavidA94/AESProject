﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AES.Web.JobbingService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="JobContract", Namespace="http://schemas.datacontract.org/2004/07/AES.JobbingSvc.Contracts")]
    [System.SerializableAttribute()]
    public partial class JobContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int JobIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LongDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ShortDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;
        
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
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="JobbingService.IJobbingSvc")]
    public interface IJobbingSvc {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/AddJob", ReplyAction="http://tempuri.org/IJobbingSvc/AddJobResponse")]
        AES.Shared.JobbingResponse AddJob(AES.Web.JobbingService.JobContract job);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/AddJob", ReplyAction="http://tempuri.org/IJobbingSvc/AddJobResponse")]
        System.Threading.Tasks.Task<AES.Shared.JobbingResponse> AddJobAsync(AES.Web.JobbingService.JobContract job);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/AddQuestion", ReplyAction="http://tempuri.org/IJobbingSvc/AddQuestionResponse")]
        AES.Shared.JobbingResponse AddQuestion(AES.Shared.Contracts.QAContract question);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/AddQuestion", ReplyAction="http://tempuri.org/IJobbingSvc/AddQuestionResponse")]
        System.Threading.Tasks.Task<AES.Shared.JobbingResponse> AddQuestionAsync(AES.Shared.Contracts.QAContract question);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/AddQuestionToJob", ReplyAction="http://tempuri.org/IJobbingSvc/AddQuestionToJobResponse")]
        AES.Shared.JobbingResponse AddQuestionToJob(int jobID, int questionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/AddQuestionToJob", ReplyAction="http://tempuri.org/IJobbingSvc/AddQuestionToJobResponse")]
        System.Threading.Tasks.Task<AES.Shared.JobbingResponse> AddQuestionToJobAsync(int jobID, int questionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/EditJob", ReplyAction="http://tempuri.org/IJobbingSvc/EditJobResponse")]
        AES.Shared.JobbingResponse EditJob(AES.Web.JobbingService.JobContract job);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/EditJob", ReplyAction="http://tempuri.org/IJobbingSvc/EditJobResponse")]
        System.Threading.Tasks.Task<AES.Shared.JobbingResponse> EditJobAsync(AES.Web.JobbingService.JobContract job);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/EditQuestion", ReplyAction="http://tempuri.org/IJobbingSvc/EditQuestionResponse")]
        AES.Shared.JobbingResponse EditQuestion(AES.Shared.Contracts.QAContract question);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/EditQuestion", ReplyAction="http://tempuri.org/IJobbingSvc/EditQuestionResponse")]
        System.Threading.Tasks.Task<AES.Shared.JobbingResponse> EditQuestionAsync(AES.Shared.Contracts.QAContract question);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/GetJobs", ReplyAction="http://tempuri.org/IJobbingSvc/GetJobsResponse")]
        AES.Web.JobbingService.JobContract[] GetJobs();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/GetJobs", ReplyAction="http://tempuri.org/IJobbingSvc/GetJobsResponse")]
        System.Threading.Tasks.Task<AES.Web.JobbingService.JobContract[]> GetJobsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/GetQuestions", ReplyAction="http://tempuri.org/IJobbingSvc/GetQuestionsResponse")]
        AES.Shared.Contracts.QAContract[] GetQuestions(int jobID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/GetQuestions", ReplyAction="http://tempuri.org/IJobbingSvc/GetQuestionsResponse")]
        System.Threading.Tasks.Task<AES.Shared.Contracts.QAContract[]> GetQuestionsAsync(int jobID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/RemoveQuestionFromJob", ReplyAction="http://tempuri.org/IJobbingSvc/RemoveQuestionFromJobResponse")]
        AES.Shared.JobbingResponse RemoveQuestionFromJob(int jobID, int questionID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJobbingSvc/RemoveQuestionFromJob", ReplyAction="http://tempuri.org/IJobbingSvc/RemoveQuestionFromJobResponse")]
        System.Threading.Tasks.Task<AES.Shared.JobbingResponse> RemoveQuestionFromJobAsync(int jobID, int questionID);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJobbingSvcChannel : AES.Web.JobbingService.IJobbingSvc, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JobbingSvcClient : System.ServiceModel.ClientBase<AES.Web.JobbingService.IJobbingSvc>, AES.Web.JobbingService.IJobbingSvc {
        
        public JobbingSvcClient() {
        }
        
        public JobbingSvcClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JobbingSvcClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobbingSvcClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JobbingSvcClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public AES.Shared.JobbingResponse AddJob(AES.Web.JobbingService.JobContract job) {
            return base.Channel.AddJob(job);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.JobbingResponse> AddJobAsync(AES.Web.JobbingService.JobContract job) {
            return base.Channel.AddJobAsync(job);
        }
        
        public AES.Shared.JobbingResponse AddQuestion(AES.Shared.Contracts.QAContract question) {
            return base.Channel.AddQuestion(question);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.JobbingResponse> AddQuestionAsync(AES.Shared.Contracts.QAContract question) {
            return base.Channel.AddQuestionAsync(question);
        }
        
        public AES.Shared.JobbingResponse AddQuestionToJob(int jobID, int questionID) {
            return base.Channel.AddQuestionToJob(jobID, questionID);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.JobbingResponse> AddQuestionToJobAsync(int jobID, int questionID) {
            return base.Channel.AddQuestionToJobAsync(jobID, questionID);
        }
        
        public AES.Shared.JobbingResponse EditJob(AES.Web.JobbingService.JobContract job) {
            return base.Channel.EditJob(job);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.JobbingResponse> EditJobAsync(AES.Web.JobbingService.JobContract job) {
            return base.Channel.EditJobAsync(job);
        }
        
        public AES.Shared.JobbingResponse EditQuestion(AES.Shared.Contracts.QAContract question) {
            return base.Channel.EditQuestion(question);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.JobbingResponse> EditQuestionAsync(AES.Shared.Contracts.QAContract question) {
            return base.Channel.EditQuestionAsync(question);
        }
        
        public AES.Web.JobbingService.JobContract[] GetJobs() {
            return base.Channel.GetJobs();
        }
        
        public System.Threading.Tasks.Task<AES.Web.JobbingService.JobContract[]> GetJobsAsync() {
            return base.Channel.GetJobsAsync();
        }
        
        public AES.Shared.Contracts.QAContract[] GetQuestions(int jobID) {
            return base.Channel.GetQuestions(jobID);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.Contracts.QAContract[]> GetQuestionsAsync(int jobID) {
            return base.Channel.GetQuestionsAsync(jobID);
        }
        
        public AES.Shared.JobbingResponse RemoveQuestionFromJob(int jobID, int questionID) {
            return base.Channel.RemoveQuestionFromJob(jobID, questionID);
        }
        
        public System.Threading.Tasks.Task<AES.Shared.JobbingResponse> RemoveQuestionFromJobAsync(int jobID, int questionID) {
            return base.Channel.RemoveQuestionFromJobAsync(jobID, questionID);
        }
    }
}
