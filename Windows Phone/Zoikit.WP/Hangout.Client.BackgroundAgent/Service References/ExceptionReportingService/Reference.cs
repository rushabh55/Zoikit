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
namespace Hangout.Client.BackgroundAgent.ExceptionReportingService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ExceptionReportingService.IExceptionReportingService")]
    public interface IExceptionReportingService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IExceptionReportingService/AddAnException", ReplyAction="http://tempuri.org/IExceptionReportingService/AddAnExceptionResponse")]
        System.IAsyncResult BeginAddAnException(int userId, string clientType, string message, string stackTrace, System.AsyncCallback callback, object asyncState);
        
        void EndAddAnException(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IExceptionReportingServiceChannel : Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ExceptionReportingServiceClient : System.ServiceModel.ClientBase<Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService>, Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService {
        
        private BeginOperationDelegate onBeginAddAnExceptionDelegate;
        
        private EndOperationDelegate onEndAddAnExceptionDelegate;
        
        private System.Threading.SendOrPostCallback onAddAnExceptionCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public ExceptionReportingServiceClient() {
        }
        
        public ExceptionReportingServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ExceptionReportingServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExceptionReportingServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ExceptionReportingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> AddAnExceptionCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService.BeginAddAnException(int userId, string clientType, string message, string stackTrace, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginAddAnException(userId, clientType, message, stackTrace, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService.EndAddAnException(System.IAsyncResult result) {
            base.Channel.EndAddAnException(result);
        }
        
        private System.IAsyncResult OnBeginAddAnException(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int userId = ((int)(inValues[0]));
            string clientType = ((string)(inValues[1]));
            string message = ((string)(inValues[2]));
            string stackTrace = ((string)(inValues[3]));
            return ((Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService)(this)).BeginAddAnException(userId, clientType, message, stackTrace, callback, asyncState);
        }
        
        private object[] OnEndAddAnException(System.IAsyncResult result) {
            ((Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService)(this)).EndAddAnException(result);
            return null;
        }
        
        private void OnAddAnExceptionCompleted(object state) {
            if ((this.AddAnExceptionCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.AddAnExceptionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void AddAnExceptionAsync(int userId, string clientType, string message, string stackTrace) {
            this.AddAnExceptionAsync(userId, clientType, message, stackTrace, null);
        }
        
        public void AddAnExceptionAsync(int userId, string clientType, string message, string stackTrace, object userState) {
            if ((this.onBeginAddAnExceptionDelegate == null)) {
                this.onBeginAddAnExceptionDelegate = new BeginOperationDelegate(this.OnBeginAddAnException);
            }
            if ((this.onEndAddAnExceptionDelegate == null)) {
                this.onEndAddAnExceptionDelegate = new EndOperationDelegate(this.OnEndAddAnException);
            }
            if ((this.onAddAnExceptionCompletedDelegate == null)) {
                this.onAddAnExceptionCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnAddAnExceptionCompleted);
            }
            base.InvokeAsync(this.onBeginAddAnExceptionDelegate, new object[] {
                        userId,
                        clientType,
                        message,
                        stackTrace}, this.onEndAddAnExceptionDelegate, this.onAddAnExceptionCompletedDelegate, userState);
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
        
        protected override Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService CreateChannel() {
            return new ExceptionReportingServiceClientChannel(this);
        }
        
        private class ExceptionReportingServiceClientChannel : ChannelBase<Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService>, Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService {
            
            public ExceptionReportingServiceClientChannel(System.ServiceModel.ClientBase<Hangout.Client.BackgroundAgent.ExceptionReportingService.IExceptionReportingService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginAddAnException(int userId, string clientType, string message, string stackTrace, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[4];
                _args[0] = userId;
                _args[1] = clientType;
                _args[2] = message;
                _args[3] = stackTrace;
                System.IAsyncResult _result = base.BeginInvoke("AddAnException", _args, callback, asyncState);
                return _result;
            }
            
            public void EndAddAnException(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("AddAnException", _args, result);
            }
        }
    }
}
