﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.237
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace SlvHanbaiClient.svcPerson {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EntityBase", Namespace="http://schemas.datacontract.org/2004/07/SlvHanbai.Web.Class.Entity")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(SlvHanbaiClient.svcPerson.EntityPerson))]
    public partial class EntityBase : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EntityPerson", Namespace="http://schemas.datacontract.org/2004/07/SlvHanbai.Web.Class.Entity")]
    public partial class EntityPerson : SlvHanbaiClient.svcPerson.EntityBase {
        
        private int _display_division_idField;
        
        private string _display_division_nmField;
        
        private string _group_idField;
        
        private string _group_nmField;
        
        private int _idField;
        
        private int _lock_flgField;
        
        private string _memoField;
        
        private string _nameField;
        
        private string messageField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int _display_division_id {
            get {
                return this._display_division_idField;
            }
            set {
                if ((this._display_division_idField.Equals(value) != true)) {
                    this._display_division_idField = value;
                    this.RaisePropertyChanged("_display_division_id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string _display_division_nm {
            get {
                return this._display_division_nmField;
            }
            set {
                if ((object.ReferenceEquals(this._display_division_nmField, value) != true)) {
                    this._display_division_nmField = value;
                    this.RaisePropertyChanged("_display_division_nm");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string _group_id {
            get {
                return this._group_idField;
            }
            set {
                if ((object.ReferenceEquals(this._group_idField, value) != true)) {
                    this._group_idField = value;
                    this.RaisePropertyChanged("_group_id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string _group_nm {
            get {
                return this._group_nmField;
            }
            set {
                if ((object.ReferenceEquals(this._group_nmField, value) != true)) {
                    this._group_nmField = value;
                    this.RaisePropertyChanged("_group_nm");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int _id {
            get {
                return this._idField;
            }
            set {
                if ((this._idField.Equals(value) != true)) {
                    this._idField = value;
                    this.RaisePropertyChanged("_id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int _lock_flg {
            get {
                return this._lock_flgField;
            }
            set {
                if ((this._lock_flgField.Equals(value) != true)) {
                    this._lock_flgField = value;
                    this.RaisePropertyChanged("_lock_flg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string _memo {
            get {
                return this._memoField;
            }
            set {
                if ((object.ReferenceEquals(this._memoField, value) != true)) {
                    this._memoField = value;
                    this.RaisePropertyChanged("_memo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string _name {
            get {
                return this._nameField;
            }
            set {
                if ((object.ReferenceEquals(this._nameField, value) != true)) {
                    this._nameField = value;
                    this.RaisePropertyChanged("_name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string message {
            get {
                return this.messageField;
            }
            set {
                if ((object.ReferenceEquals(this.messageField, value) != true)) {
                    this.messageField = value;
                    this.RaisePropertyChanged("message");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="svcPerson.svcPerson")]
    public interface svcPerson {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:svcPerson/GetPerson", ReplyAction="urn:svcPerson/GetPersonResponse")]
        System.IAsyncResult BeginGetPerson(string random, int Id, System.AsyncCallback callback, object asyncState);
        
        SlvHanbaiClient.svcPerson.EntityPerson EndGetPerson(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:svcPerson/UpdatePerson", ReplyAction="urn:svcPerson/UpdatePersonResponse")]
        System.IAsyncResult BeginUpdatePerson(string random, int type, long Id, SlvHanbaiClient.svcPerson.EntityPerson entity, System.AsyncCallback callback, object asyncState);
        
        string EndUpdatePerson(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface svcPersonChannel : SlvHanbaiClient.svcPerson.svcPerson, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetPersonCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetPersonCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public SlvHanbaiClient.svcPerson.EntityPerson Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((SlvHanbaiClient.svcPerson.EntityPerson)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UpdatePersonCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public UpdatePersonCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class svcPersonClient : System.ServiceModel.ClientBase<SlvHanbaiClient.svcPerson.svcPerson>, SlvHanbaiClient.svcPerson.svcPerson {
        
        private BeginOperationDelegate onBeginGetPersonDelegate;
        
        private EndOperationDelegate onEndGetPersonDelegate;
        
        private System.Threading.SendOrPostCallback onGetPersonCompletedDelegate;
        
        private BeginOperationDelegate onBeginUpdatePersonDelegate;
        
        private EndOperationDelegate onEndUpdatePersonDelegate;
        
        private System.Threading.SendOrPostCallback onUpdatePersonCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public svcPersonClient() {
        }
        
        public svcPersonClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public svcPersonClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public svcPersonClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public svcPersonClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("CookieContainer を設定できません。バインドに HttpCookieContainerBindingElement が含まれていることを確認してくだ" +
                            "さい。");
                }
            }
        }
        
        public event System.EventHandler<GetPersonCompletedEventArgs> GetPersonCompleted;
        
        public event System.EventHandler<UpdatePersonCompletedEventArgs> UpdatePersonCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SlvHanbaiClient.svcPerson.svcPerson.BeginGetPerson(string random, int Id, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetPerson(random, Id, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        SlvHanbaiClient.svcPerson.EntityPerson SlvHanbaiClient.svcPerson.svcPerson.EndGetPerson(System.IAsyncResult result) {
            return base.Channel.EndGetPerson(result);
        }
        
        private System.IAsyncResult OnBeginGetPerson(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string random = ((string)(inValues[0]));
            int Id = ((int)(inValues[1]));
            return ((SlvHanbaiClient.svcPerson.svcPerson)(this)).BeginGetPerson(random, Id, callback, asyncState);
        }
        
        private object[] OnEndGetPerson(System.IAsyncResult result) {
            SlvHanbaiClient.svcPerson.EntityPerson retVal = ((SlvHanbaiClient.svcPerson.svcPerson)(this)).EndGetPerson(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetPersonCompleted(object state) {
            if ((this.GetPersonCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetPersonCompleted(this, new GetPersonCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetPersonAsync(string random, int Id) {
            this.GetPersonAsync(random, Id, null);
        }
        
        public void GetPersonAsync(string random, int Id, object userState) {
            if ((this.onBeginGetPersonDelegate == null)) {
                this.onBeginGetPersonDelegate = new BeginOperationDelegate(this.OnBeginGetPerson);
            }
            if ((this.onEndGetPersonDelegate == null)) {
                this.onEndGetPersonDelegate = new EndOperationDelegate(this.OnEndGetPerson);
            }
            if ((this.onGetPersonCompletedDelegate == null)) {
                this.onGetPersonCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetPersonCompleted);
            }
            base.InvokeAsync(this.onBeginGetPersonDelegate, new object[] {
                        random,
                        Id}, this.onEndGetPersonDelegate, this.onGetPersonCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SlvHanbaiClient.svcPerson.svcPerson.BeginUpdatePerson(string random, int type, long Id, SlvHanbaiClient.svcPerson.EntityPerson entity, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginUpdatePerson(random, type, Id, entity, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string SlvHanbaiClient.svcPerson.svcPerson.EndUpdatePerson(System.IAsyncResult result) {
            return base.Channel.EndUpdatePerson(result);
        }
        
        private System.IAsyncResult OnBeginUpdatePerson(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string random = ((string)(inValues[0]));
            int type = ((int)(inValues[1]));
            long Id = ((long)(inValues[2]));
            SlvHanbaiClient.svcPerson.EntityPerson entity = ((SlvHanbaiClient.svcPerson.EntityPerson)(inValues[3]));
            return ((SlvHanbaiClient.svcPerson.svcPerson)(this)).BeginUpdatePerson(random, type, Id, entity, callback, asyncState);
        }
        
        private object[] OnEndUpdatePerson(System.IAsyncResult result) {
            string retVal = ((SlvHanbaiClient.svcPerson.svcPerson)(this)).EndUpdatePerson(result);
            return new object[] {
                    retVal};
        }
        
        private void OnUpdatePersonCompleted(object state) {
            if ((this.UpdatePersonCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.UpdatePersonCompleted(this, new UpdatePersonCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void UpdatePersonAsync(string random, int type, long Id, SlvHanbaiClient.svcPerson.EntityPerson entity) {
            this.UpdatePersonAsync(random, type, Id, entity, null);
        }
        
        public void UpdatePersonAsync(string random, int type, long Id, SlvHanbaiClient.svcPerson.EntityPerson entity, object userState) {
            if ((this.onBeginUpdatePersonDelegate == null)) {
                this.onBeginUpdatePersonDelegate = new BeginOperationDelegate(this.OnBeginUpdatePerson);
            }
            if ((this.onEndUpdatePersonDelegate == null)) {
                this.onEndUpdatePersonDelegate = new EndOperationDelegate(this.OnEndUpdatePerson);
            }
            if ((this.onUpdatePersonCompletedDelegate == null)) {
                this.onUpdatePersonCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnUpdatePersonCompleted);
            }
            base.InvokeAsync(this.onBeginUpdatePersonDelegate, new object[] {
                        random,
                        type,
                        Id,
                        entity}, this.onEndUpdatePersonDelegate, this.onUpdatePersonCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override SlvHanbaiClient.svcPerson.svcPerson CreateChannel() {
            return new svcPersonClientChannel(this);
        }
        
        private class svcPersonClientChannel : ChannelBase<SlvHanbaiClient.svcPerson.svcPerson>, SlvHanbaiClient.svcPerson.svcPerson {
            
            public svcPersonClientChannel(System.ServiceModel.ClientBase<SlvHanbaiClient.svcPerson.svcPerson> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetPerson(string random, int Id, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = random;
                _args[1] = Id;
                System.IAsyncResult _result = base.BeginInvoke("GetPerson", _args, callback, asyncState);
                return _result;
            }
            
            public SlvHanbaiClient.svcPerson.EntityPerson EndGetPerson(System.IAsyncResult result) {
                object[] _args = new object[0];
                SlvHanbaiClient.svcPerson.EntityPerson _result = ((SlvHanbaiClient.svcPerson.EntityPerson)(base.EndInvoke("GetPerson", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginUpdatePerson(string random, int type, long Id, SlvHanbaiClient.svcPerson.EntityPerson entity, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = random;
                _args[1] = type;
                _args[2] = Id;
                _args[3] = entity;
                System.IAsyncResult _result = base.BeginInvoke("UpdatePerson", _args, callback, asyncState);
                return _result;
            }
            
            public string EndUpdatePerson(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("UpdatePerson", _args, result)));
                return _result;
            }
        }
    }
}
