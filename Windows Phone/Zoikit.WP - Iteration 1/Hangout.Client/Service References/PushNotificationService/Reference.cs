﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.Phone.ServiceReference, version 3.7.0.0
// 
namespace Hangout.Client.PushNotificationService {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TableEntity", Namespace="http://schemas.datacontract.org/2004/07/Microsoft.WindowsAzure.Storage.Table")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Hangout.Client.PushNotificationService.PushNotification))]
    public partial class TableEntity : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string ETagField;
        
        private string PartitionKeyField;
        
        private string RowKeyField;
        
        private Hangout.Client.PushNotificationService.DateTimeOffset TimestampField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ETag {
            get {
                return this.ETagField;
            }
            set {
                if ((object.ReferenceEquals(this.ETagField, value) != true)) {
                    this.ETagField = value;
                    this.RaisePropertyChanged("ETag");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PartitionKey {
            get {
                return this.PartitionKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.PartitionKeyField, value) != true)) {
                    this.PartitionKeyField = value;
                    this.RaisePropertyChanged("PartitionKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RowKey {
            get {
                return this.RowKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.RowKeyField, value) != true)) {
                    this.RowKeyField = value;
                    this.RaisePropertyChanged("RowKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Hangout.Client.PushNotificationService.DateTimeOffset Timestamp {
            get {
                return this.TimestampField;
            }
            set {
                if ((this.TimestampField.Equals(value) != true)) {
                    this.TimestampField = value;
                    this.RaisePropertyChanged("Timestamp");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DateTimeOffset", Namespace="http://schemas.datacontract.org/2004/07/System")]
    public partial struct DateTimeOffset : System.ComponentModel.INotifyPropertyChanged {
        
        private System.DateTime DateTimeField;
        
        private short OffsetMinutesField;
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public System.DateTime DateTime {
            get {
                return this.DateTimeField;
            }
            set {
                if ((this.DateTimeField.Equals(value) != true)) {
                    this.DateTimeField = value;
                    this.RaisePropertyChanged("DateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public short OffsetMinutes {
            get {
                return this.OffsetMinutesField;
            }
            set {
                if ((this.OffsetMinutesField.Equals(value) != true)) {
                    this.OffsetMinutesField = value;
                    this.RaisePropertyChanged("OffsetMinutes");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PushNotification", Namespace="http://schemas.datacontract.org/2004/07/Hangout.Web.Model")]
    public partial class PushNotification : Hangout.Client.PushNotificationService.TableEntity {
        
        private string PushNotificationIDField;
        
        private string URIField;
        
        private System.Guid UserIDField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PushNotificationID {
            get {
                return this.PushNotificationIDField;
            }
            set {
                if ((object.ReferenceEquals(this.PushNotificationIDField, value) != true)) {
                    this.PushNotificationIDField = value;
                    this.RaisePropertyChanged("PushNotificationID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string URI {
            get {
                return this.URIField;
            }
            set {
                if ((object.ReferenceEquals(this.URIField, value) != true)) {
                    this.URIField = value;
                    this.RaisePropertyChanged("URI");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Guid UserID {
            get {
                return this.UserIDField;
            }
            set {
                if ((this.UserIDField.Equals(value) != true)) {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PushNotificationService.IPushNotifications")]
    public interface IPushNotifications {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPushNotifications/Subscribe", ReplyAction="http://tempuri.org/IPushNotifications/SubscribeResponse")]
        System.IAsyncResult BeginSubscribe(System.Guid userId, string channelUri, string zat, System.AsyncCallback callback, object asyncState);
        
        void EndSubscribe(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPushNotifications/Unsubscribe", ReplyAction="http://tempuri.org/IPushNotifications/UnsubscribeResponse")]
        System.IAsyncResult BeginUnsubscribe(System.Guid userId, string zat, System.AsyncCallback callback, object asyncState);
        
        void EndUnsubscribe(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPushNotifications/GetPushNotification", ReplyAction="http://tempuri.org/IPushNotifications/GetPushNotificationResponse")]
        System.IAsyncResult BeginGetPushNotification(string zat, System.AsyncCallback callback, object asyncState);
        
        Hangout.Client.PushNotificationService.PushNotification EndGetPushNotification(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IPushNotifications/Exists", ReplyAction="http://tempuri.org/IPushNotifications/ExistsResponse")]
        System.IAsyncResult BeginExists(System.Guid userId, string zat, System.AsyncCallback callback, object asyncState);
        
        bool EndExists(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPushNotificationsChannel : Hangout.Client.PushNotificationService.IPushNotifications, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetPushNotificationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetPushNotificationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public Hangout.Client.PushNotificationService.PushNotification Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((Hangout.Client.PushNotificationService.PushNotification)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExistsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public ExistsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public bool Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PushNotificationsClient : System.ServiceModel.ClientBase<Hangout.Client.PushNotificationService.IPushNotifications>, Hangout.Client.PushNotificationService.IPushNotifications {
        
        private BeginOperationDelegate onBeginSubscribeDelegate;
        
        private EndOperationDelegate onEndSubscribeDelegate;
        
        private System.Threading.SendOrPostCallback onSubscribeCompletedDelegate;
        
        private BeginOperationDelegate onBeginUnsubscribeDelegate;
        
        private EndOperationDelegate onEndUnsubscribeDelegate;
        
        private System.Threading.SendOrPostCallback onUnsubscribeCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetPushNotificationDelegate;
        
        private EndOperationDelegate onEndGetPushNotificationDelegate;
        
        private System.Threading.SendOrPostCallback onGetPushNotificationCompletedDelegate;
        
        private BeginOperationDelegate onBeginExistsDelegate;
        
        private EndOperationDelegate onEndExistsDelegate;
        
        private System.Threading.SendOrPostCallback onExistsCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public PushNotificationsClient() {
        }
        
        public PushNotificationsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PushNotificationsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PushNotificationsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PushNotificationsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> SubscribeCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> UnsubscribeCompleted;
        
        public event System.EventHandler<GetPushNotificationCompletedEventArgs> GetPushNotificationCompleted;
        
        public event System.EventHandler<ExistsCompletedEventArgs> ExistsCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hangout.Client.PushNotificationService.IPushNotifications.BeginSubscribe(System.Guid userId, string channelUri, string zat, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSubscribe(userId, channelUri, zat, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void Hangout.Client.PushNotificationService.IPushNotifications.EndSubscribe(System.IAsyncResult result) {
            base.Channel.EndSubscribe(result);
        }
        
        private System.IAsyncResult OnBeginSubscribe(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Guid userId = ((System.Guid)(inValues[0]));
            string channelUri = ((string)(inValues[1]));
            string zat = ((string)(inValues[2]));
            return ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).BeginSubscribe(userId, channelUri, zat, callback, asyncState);
        }
        
        private object[] OnEndSubscribe(System.IAsyncResult result) {
            ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).EndSubscribe(result);
            return null;
        }
        
        private void OnSubscribeCompleted(object state) {
            if ((this.SubscribeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SubscribeCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SubscribeAsync(System.Guid userId, string channelUri, string zat) {
            this.SubscribeAsync(userId, channelUri, zat, null);
        }
        
        public void SubscribeAsync(System.Guid userId, string channelUri, string zat, object userState) {
            if ((this.onBeginSubscribeDelegate == null)) {
                this.onBeginSubscribeDelegate = new BeginOperationDelegate(this.OnBeginSubscribe);
            }
            if ((this.onEndSubscribeDelegate == null)) {
                this.onEndSubscribeDelegate = new EndOperationDelegate(this.OnEndSubscribe);
            }
            if ((this.onSubscribeCompletedDelegate == null)) {
                this.onSubscribeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSubscribeCompleted);
            }
            base.InvokeAsync(this.onBeginSubscribeDelegate, new object[] {
                        userId,
                        channelUri,
                        zat}, this.onEndSubscribeDelegate, this.onSubscribeCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hangout.Client.PushNotificationService.IPushNotifications.BeginUnsubscribe(System.Guid userId, string zat, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginUnsubscribe(userId, zat, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void Hangout.Client.PushNotificationService.IPushNotifications.EndUnsubscribe(System.IAsyncResult result) {
            base.Channel.EndUnsubscribe(result);
        }
        
        private System.IAsyncResult OnBeginUnsubscribe(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Guid userId = ((System.Guid)(inValues[0]));
            string zat = ((string)(inValues[1]));
            return ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).BeginUnsubscribe(userId, zat, callback, asyncState);
        }
        
        private object[] OnEndUnsubscribe(System.IAsyncResult result) {
            ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).EndUnsubscribe(result);
            return null;
        }
        
        private void OnUnsubscribeCompleted(object state) {
            if ((this.UnsubscribeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.UnsubscribeCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void UnsubscribeAsync(System.Guid userId, string zat) {
            this.UnsubscribeAsync(userId, zat, null);
        }
        
        public void UnsubscribeAsync(System.Guid userId, string zat, object userState) {
            if ((this.onBeginUnsubscribeDelegate == null)) {
                this.onBeginUnsubscribeDelegate = new BeginOperationDelegate(this.OnBeginUnsubscribe);
            }
            if ((this.onEndUnsubscribeDelegate == null)) {
                this.onEndUnsubscribeDelegate = new EndOperationDelegate(this.OnEndUnsubscribe);
            }
            if ((this.onUnsubscribeCompletedDelegate == null)) {
                this.onUnsubscribeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnUnsubscribeCompleted);
            }
            base.InvokeAsync(this.onBeginUnsubscribeDelegate, new object[] {
                        userId,
                        zat}, this.onEndUnsubscribeDelegate, this.onUnsubscribeCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hangout.Client.PushNotificationService.IPushNotifications.BeginGetPushNotification(string zat, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetPushNotification(zat, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Hangout.Client.PushNotificationService.PushNotification Hangout.Client.PushNotificationService.IPushNotifications.EndGetPushNotification(System.IAsyncResult result) {
            return base.Channel.EndGetPushNotification(result);
        }
        
        private System.IAsyncResult OnBeginGetPushNotification(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string zat = ((string)(inValues[0]));
            return ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).BeginGetPushNotification(zat, callback, asyncState);
        }
        
        private object[] OnEndGetPushNotification(System.IAsyncResult result) {
            Hangout.Client.PushNotificationService.PushNotification retVal = ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).EndGetPushNotification(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetPushNotificationCompleted(object state) {
            if ((this.GetPushNotificationCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetPushNotificationCompleted(this, new GetPushNotificationCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetPushNotificationAsync(string zat) {
            this.GetPushNotificationAsync(zat, null);
        }
        
        public void GetPushNotificationAsync(string zat, object userState) {
            if ((this.onBeginGetPushNotificationDelegate == null)) {
                this.onBeginGetPushNotificationDelegate = new BeginOperationDelegate(this.OnBeginGetPushNotification);
            }
            if ((this.onEndGetPushNotificationDelegate == null)) {
                this.onEndGetPushNotificationDelegate = new EndOperationDelegate(this.OnEndGetPushNotification);
            }
            if ((this.onGetPushNotificationCompletedDelegate == null)) {
                this.onGetPushNotificationCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetPushNotificationCompleted);
            }
            base.InvokeAsync(this.onBeginGetPushNotificationDelegate, new object[] {
                        zat}, this.onEndGetPushNotificationDelegate, this.onGetPushNotificationCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hangout.Client.PushNotificationService.IPushNotifications.BeginExists(System.Guid userId, string zat, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginExists(userId, zat, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        bool Hangout.Client.PushNotificationService.IPushNotifications.EndExists(System.IAsyncResult result) {
            return base.Channel.EndExists(result);
        }
        
        private System.IAsyncResult OnBeginExists(object[] inValues, System.AsyncCallback callback, object asyncState) {
            System.Guid userId = ((System.Guid)(inValues[0]));
            string zat = ((string)(inValues[1]));
            return ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).BeginExists(userId, zat, callback, asyncState);
        }
        
        private object[] OnEndExists(System.IAsyncResult result) {
            bool retVal = ((Hangout.Client.PushNotificationService.IPushNotifications)(this)).EndExists(result);
            return new object[] {
                    retVal};
        }
        
        private void OnExistsCompleted(object state) {
            if ((this.ExistsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.ExistsCompleted(this, new ExistsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void ExistsAsync(System.Guid userId, string zat) {
            this.ExistsAsync(userId, zat, null);
        }
        
        public void ExistsAsync(System.Guid userId, string zat, object userState) {
            if ((this.onBeginExistsDelegate == null)) {
                this.onBeginExistsDelegate = new BeginOperationDelegate(this.OnBeginExists);
            }
            if ((this.onEndExistsDelegate == null)) {
                this.onEndExistsDelegate = new EndOperationDelegate(this.OnEndExists);
            }
            if ((this.onExistsCompletedDelegate == null)) {
                this.onExistsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnExistsCompleted);
            }
            base.InvokeAsync(this.onBeginExistsDelegate, new object[] {
                        userId,
                        zat}, this.onEndExistsDelegate, this.onExistsCompletedDelegate, userState);
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
        
        protected override Hangout.Client.PushNotificationService.IPushNotifications CreateChannel() {
            return new PushNotificationsClientChannel(this);
        }
        
        private class PushNotificationsClientChannel : ChannelBase<Hangout.Client.PushNotificationService.IPushNotifications>, Hangout.Client.PushNotificationService.IPushNotifications {
            
            public PushNotificationsClientChannel(System.ServiceModel.ClientBase<Hangout.Client.PushNotificationService.IPushNotifications> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginSubscribe(System.Guid userId, string channelUri, string zat, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = userId;
                _args[1] = channelUri;
                _args[2] = zat;
                System.IAsyncResult _result = base.BeginInvoke("Subscribe", _args, callback, asyncState);
                return _result;
            }
            
            public void EndSubscribe(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("Subscribe", _args, result);
            }
            
            public System.IAsyncResult BeginUnsubscribe(System.Guid userId, string zat, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = userId;
                _args[1] = zat;
                System.IAsyncResult _result = base.BeginInvoke("Unsubscribe", _args, callback, asyncState);
                return _result;
            }
            
            public void EndUnsubscribe(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("Unsubscribe", _args, result);
            }
            
            public System.IAsyncResult BeginGetPushNotification(string zat, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = zat;
                System.IAsyncResult _result = base.BeginInvoke("GetPushNotification", _args, callback, asyncState);
                return _result;
            }
            
            public Hangout.Client.PushNotificationService.PushNotification EndGetPushNotification(System.IAsyncResult result) {
                object[] _args = new object[0];
                Hangout.Client.PushNotificationService.PushNotification _result = ((Hangout.Client.PushNotificationService.PushNotification)(base.EndInvoke("GetPushNotification", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginExists(System.Guid userId, string zat, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = userId;
                _args[1] = zat;
                System.IAsyncResult _result = base.BeginInvoke("Exists", _args, callback, asyncState);
                return _result;
            }
            
            public bool EndExists(System.IAsyncResult result) {
                object[] _args = new object[0];
                bool _result = ((bool)(base.EndInvoke("Exists", _args, result)));
                return _result;
            }
        }
    }
}
