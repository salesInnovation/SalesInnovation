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
namespace SlvHanbaiClient.svcAuthority {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EntityAuthority", Namespace="http://schemas.datacontract.org/2004/07/SlvHanbai.Web.Class.Entity")]
    public partial class EntityAuthority : SlvHanbaiClient.svcAuthority.EntityBase {
        
        private int _authority_kbnField;
        
        private int _display_indexField;
        
        private int _lock_flgField;
        
        private string _memoField;
        
        private string _pg_idField;
        
        private int _user_idField;
        
        private string messageField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int _authority_kbn {
            get {
                return this._authority_kbnField;
            }
            set {
                if ((this._authority_kbnField.Equals(value) != true)) {
                    this._authority_kbnField = value;
                    this.RaisePropertyChanged("_authority_kbn");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int _display_index {
            get {
                return this._display_indexField;
            }
            set {
                if ((this._display_indexField.Equals(value) != true)) {
                    this._display_indexField = value;
                    this.RaisePropertyChanged("_display_index");
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
        public string _pg_id {
            get {
                return this._pg_idField;
            }
            set {
                if ((object.ReferenceEquals(this._pg_idField, value) != true)) {
                    this._pg_idField = value;
                    this.RaisePropertyChanged("_pg_id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int _user_id {
            get {
                return this._user_idField;
            }
            set {
                if ((this._user_idField.Equals(value) != true)) {
                    this._user_idField = value;
                    this.RaisePropertyChanged("_user_id");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EntityBase", Namespace="http://schemas.datacontract.org/2004/07/SlvHanbai.Web.Class.Entity")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(SlvHanbaiClient.svcAuthority.EntityAuthority))]
    public partial class EntityBase : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="svcAuthority.svcAuthority")]
    public interface svcAuthority {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:svcAuthority/GetAuthority", ReplyAction="urn:svcAuthority/GetAuthorityResponse")]
        System.IAsyncResult BeginGetAuthority(string random, int _user_id, System.AsyncCallback callback, object asyncState);
        
        System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> EndGetAuthority(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:svcAuthority/UpdateAuthority", ReplyAction="urn:svcAuthority/UpdateAuthorityResponse")]
        System.IAsyncResult BeginUpdateAuthority(string random, System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> entity, int _user_id, System.AsyncCallback callback, object asyncState);
        
        string EndUpdateAuthority(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface svcAuthorityChannel : SlvHanbaiClient.svcAuthority.svcAuthority, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetAuthorityCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetAuthorityCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority>)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UpdateAuthorityCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public UpdateAuthorityCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public partial class svcAuthorityClient : System.ServiceModel.ClientBase<SlvHanbaiClient.svcAuthority.svcAuthority>, SlvHanbaiClient.svcAuthority.svcAuthority {
        
        private BeginOperationDelegate onBeginGetAuthorityDelegate;
        
        private EndOperationDelegate onEndGetAuthorityDelegate;
        
        private System.Threading.SendOrPostCallback onGetAuthorityCompletedDelegate;
        
        private BeginOperationDelegate onBeginUpdateAuthorityDelegate;
        
        private EndOperationDelegate onEndUpdateAuthorityDelegate;
        
        private System.Threading.SendOrPostCallback onUpdateAuthorityCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public svcAuthorityClient() {
        }
        
        public svcAuthorityClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public svcAuthorityClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public svcAuthorityClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public svcAuthorityClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetAuthorityCompletedEventArgs> GetAuthorityCompleted;
        
        public event System.EventHandler<UpdateAuthorityCompletedEventArgs> UpdateAuthorityCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SlvHanbaiClient.svcAuthority.svcAuthority.BeginGetAuthority(string random, int _user_id, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetAuthority(random, _user_id, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> SlvHanbaiClient.svcAuthority.svcAuthority.EndGetAuthority(System.IAsyncResult result) {
            return base.Channel.EndGetAuthority(result);
        }
        
        private System.IAsyncResult OnBeginGetAuthority(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string random = ((string)(inValues[0]));
            int _user_id = ((int)(inValues[1]));
            return ((SlvHanbaiClient.svcAuthority.svcAuthority)(this)).BeginGetAuthority(random, _user_id, callback, asyncState);
        }
        
        private object[] OnEndGetAuthority(System.IAsyncResult result) {
            System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> retVal = ((SlvHanbaiClient.svcAuthority.svcAuthority)(this)).EndGetAuthority(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetAuthorityCompleted(object state) {
            if ((this.GetAuthorityCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetAuthorityCompleted(this, new GetAuthorityCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetAuthorityAsync(string random, int _user_id) {
            this.GetAuthorityAsync(random, _user_id, null);
        }
        
        public void GetAuthorityAsync(string random, int _user_id, object userState) {
            if ((this.onBeginGetAuthorityDelegate == null)) {
                this.onBeginGetAuthorityDelegate = new BeginOperationDelegate(this.OnBeginGetAuthority);
            }
            if ((this.onEndGetAuthorityDelegate == null)) {
                this.onEndGetAuthorityDelegate = new EndOperationDelegate(this.OnEndGetAuthority);
            }
            if ((this.onGetAuthorityCompletedDelegate == null)) {
                this.onGetAuthorityCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetAuthorityCompleted);
            }
            base.InvokeAsync(this.onBeginGetAuthorityDelegate, new object[] {
                        random,
                        _user_id}, this.onEndGetAuthorityDelegate, this.onGetAuthorityCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult SlvHanbaiClient.svcAuthority.svcAuthority.BeginUpdateAuthority(string random, System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> entity, int _user_id, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginUpdateAuthority(random, entity, _user_id, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        string SlvHanbaiClient.svcAuthority.svcAuthority.EndUpdateAuthority(System.IAsyncResult result) {
            return base.Channel.EndUpdateAuthority(result);
        }
        
        private System.IAsyncResult OnBeginUpdateAuthority(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string random = ((string)(inValues[0]));
            System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> entity = ((System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority>)(inValues[1]));
            int _user_id = ((int)(inValues[2]));
            return ((SlvHanbaiClient.svcAuthority.svcAuthority)(this)).BeginUpdateAuthority(random, entity, _user_id, callback, asyncState);
        }
        
        private object[] OnEndUpdateAuthority(System.IAsyncResult result) {
            string retVal = ((SlvHanbaiClient.svcAuthority.svcAuthority)(this)).EndUpdateAuthority(result);
            return new object[] {
                    retVal};
        }
        
        private void OnUpdateAuthorityCompleted(object state) {
            if ((this.UpdateAuthorityCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.UpdateAuthorityCompleted(this, new UpdateAuthorityCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void UpdateAuthorityAsync(string random, System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> entity, int _user_id) {
            this.UpdateAuthorityAsync(random, entity, _user_id, null);
        }
        
        public void UpdateAuthorityAsync(string random, System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> entity, int _user_id, object userState) {
            if ((this.onBeginUpdateAuthorityDelegate == null)) {
                this.onBeginUpdateAuthorityDelegate = new BeginOperationDelegate(this.OnBeginUpdateAuthority);
            }
            if ((this.onEndUpdateAuthorityDelegate == null)) {
                this.onEndUpdateAuthorityDelegate = new EndOperationDelegate(this.OnEndUpdateAuthority);
            }
            if ((this.onUpdateAuthorityCompletedDelegate == null)) {
                this.onUpdateAuthorityCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnUpdateAuthorityCompleted);
            }
            base.InvokeAsync(this.onBeginUpdateAuthorityDelegate, new object[] {
                        random,
                        entity,
                        _user_id}, this.onEndUpdateAuthorityDelegate, this.onUpdateAuthorityCompletedDelegate, userState);
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
        
        protected override SlvHanbaiClient.svcAuthority.svcAuthority CreateChannel() {
            return new svcAuthorityClientChannel(this);
        }
        
        private class svcAuthorityClientChannel : ChannelBase<SlvHanbaiClient.svcAuthority.svcAuthority>, SlvHanbaiClient.svcAuthority.svcAuthority {
            
            public svcAuthorityClientChannel(System.ServiceModel.ClientBase<SlvHanbaiClient.svcAuthority.svcAuthority> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetAuthority(string random, int _user_id, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = random;
                _args[1] = _user_id;
                System.IAsyncResult _result = base.BeginInvoke("GetAuthority", _args, callback, asyncState);
                return _result;
            }
            
            public System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> EndGetAuthority(System.IAsyncResult result) {
                object[] _args = new object[0];
                System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> _result = ((System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority>)(base.EndInvoke("GetAuthority", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginUpdateAuthority(string random, System.Collections.ObjectModel.ObservableCollection<SlvHanbaiClient.svcAuthority.EntityAuthority> entity, int _user_id, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = random;
                _args[1] = entity;
                _args[2] = _user_id;
                System.IAsyncResult _result = base.BeginInvoke("UpdateAuthority", _args, callback, asyncState);
                return _result;
            }
            
            public string EndUpdateAuthority(System.IAsyncResult result) {
                object[] _args = new object[0];
                string _result = ((string)(base.EndInvoke("UpdateAuthority", _args, result)));
                return _result;
            }
        }
    }
}
