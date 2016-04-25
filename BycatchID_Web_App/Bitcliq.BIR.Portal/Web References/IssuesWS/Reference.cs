﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18444.
// 
#pragma warning disable 1591

namespace Bitcliq.BIR.Portal.IssuesWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="IssuesWebServiceSoap", Namespace="http://tempuri.org/")]
    public partial class IssuesWebService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback RegisterReporterOperationCompleted;
        
        private System.Threading.SendOrPostCallback LoginReporterOperationCompleted;
        
        private System.Threading.SendOrPostCallback LoginUserOperationCompleted;
        
        private System.Threading.SendOrPostCallback ReportIssueOperationCompleted;
        
        private System.Threading.SendOrPostCallback ReportIssueAndRegisterUserOperationCompleted;
        
        private System.Threading.SendOrPostCallback CloseIssueOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetMyIssuesOperationCompleted;
        
        private System.Threading.SendOrPostCallback GetTypesOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public IssuesWebService() {
            this.Url = global::Bitcliq.BIR.Portal.Properties.Settings.Default.Bitcliq_BIR_Portal_IssuesWS_IssuesWebService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event RegisterReporterCompletedEventHandler RegisterReporterCompleted;
        
        /// <remarks/>
        public event LoginReporterCompletedEventHandler LoginReporterCompleted;
        
        /// <remarks/>
        public event LoginUserCompletedEventHandler LoginUserCompleted;
        
        /// <remarks/>
        public event ReportIssueCompletedEventHandler ReportIssueCompleted;
        
        /// <remarks/>
        public event ReportIssueAndRegisterUserCompletedEventHandler ReportIssueAndRegisterUserCompleted;
        
        /// <remarks/>
        public event CloseIssueCompletedEventHandler CloseIssueCompleted;
        
        /// <remarks/>
        public event GetMyIssuesCompletedEventHandler GetMyIssuesCompleted;
        
        /// <remarks/>
        public event GetTypesCompletedEventHandler GetTypesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/RegisterReporter", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string RegisterReporter(string Name, string Email, string Password, string PhoneNumber, string IPAddress) {
            object[] results = this.Invoke("RegisterReporter", new object[] {
                        Name,
                        Email,
                        Password,
                        PhoneNumber,
                        IPAddress});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void RegisterReporterAsync(string Name, string Email, string Password, string PhoneNumber, string IPAddress) {
            this.RegisterReporterAsync(Name, Email, Password, PhoneNumber, IPAddress, null);
        }
        
        /// <remarks/>
        public void RegisterReporterAsync(string Name, string Email, string Password, string PhoneNumber, string IPAddress, object userState) {
            if ((this.RegisterReporterOperationCompleted == null)) {
                this.RegisterReporterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRegisterReporterOperationCompleted);
            }
            this.InvokeAsync("RegisterReporter", new object[] {
                        Name,
                        Email,
                        Password,
                        PhoneNumber,
                        IPAddress}, this.RegisterReporterOperationCompleted, userState);
        }
        
        private void OnRegisterReporterOperationCompleted(object arg) {
            if ((this.RegisterReporterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RegisterReporterCompleted(this, new RegisterReporterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LoginReporter", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LoginReporter(string Email, string Password) {
            object[] results = this.Invoke("LoginReporter", new object[] {
                        Email,
                        Password});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LoginReporterAsync(string Email, string Password) {
            this.LoginReporterAsync(Email, Password, null);
        }
        
        /// <remarks/>
        public void LoginReporterAsync(string Email, string Password, object userState) {
            if ((this.LoginReporterOperationCompleted == null)) {
                this.LoginReporterOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoginReporterOperationCompleted);
            }
            this.InvokeAsync("LoginReporter", new object[] {
                        Email,
                        Password}, this.LoginReporterOperationCompleted, userState);
        }
        
        private void OnLoginReporterOperationCompleted(object arg) {
            if ((this.LoginReporterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoginReporterCompleted(this, new LoginReporterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/LoginUser", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string LoginUser(string Email, string Password) {
            object[] results = this.Invoke("LoginUser", new object[] {
                        Email,
                        Password});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void LoginUserAsync(string Email, string Password) {
            this.LoginUserAsync(Email, Password, null);
        }
        
        /// <remarks/>
        public void LoginUserAsync(string Email, string Password, object userState) {
            if ((this.LoginUserOperationCompleted == null)) {
                this.LoginUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnLoginUserOperationCompleted);
            }
            this.InvokeAsync("LoginUser", new object[] {
                        Email,
                        Password}, this.LoginUserOperationCompleted, userState);
        }
        
        private void OnLoginUserOperationCompleted(object arg) {
            if ((this.LoginUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.LoginUserCompleted(this, new LoginUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ReportIssue", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ReportIssue([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> AccountID, int ReporterID, string Subject, string Message, string FileName, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] File, string FileType, int FileLength, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<decimal> Latitude, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<decimal> Longitude, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> TypeID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> ImageRotation, string Device) {
            object[] results = this.Invoke("ReportIssue", new object[] {
                        AccountID,
                        ReporterID,
                        Subject,
                        Message,
                        FileName,
                        File,
                        FileType,
                        FileLength,
                        Latitude,
                        Longitude,
                        TypeID,
                        ImageRotation,
                        Device});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ReportIssueAsync(System.Nullable<int> AccountID, int ReporterID, string Subject, string Message, string FileName, byte[] File, string FileType, int FileLength, System.Nullable<decimal> Latitude, System.Nullable<decimal> Longitude, System.Nullable<int> TypeID, System.Nullable<int> ImageRotation, string Device) {
            this.ReportIssueAsync(AccountID, ReporterID, Subject, Message, FileName, File, FileType, FileLength, Latitude, Longitude, TypeID, ImageRotation, Device, null);
        }
        
        /// <remarks/>
        public void ReportIssueAsync(System.Nullable<int> AccountID, int ReporterID, string Subject, string Message, string FileName, byte[] File, string FileType, int FileLength, System.Nullable<decimal> Latitude, System.Nullable<decimal> Longitude, System.Nullable<int> TypeID, System.Nullable<int> ImageRotation, string Device, object userState) {
            if ((this.ReportIssueOperationCompleted == null)) {
                this.ReportIssueOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReportIssueOperationCompleted);
            }
            this.InvokeAsync("ReportIssue", new object[] {
                        AccountID,
                        ReporterID,
                        Subject,
                        Message,
                        FileName,
                        File,
                        FileType,
                        FileLength,
                        Latitude,
                        Longitude,
                        TypeID,
                        ImageRotation,
                        Device}, this.ReportIssueOperationCompleted, userState);
        }
        
        private void OnReportIssueOperationCompleted(object arg) {
            if ((this.ReportIssueCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReportIssueCompleted(this, new ReportIssueCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/ReportIssueAndRegisterUser", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ReportIssueAndRegisterUser(
                    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> ReporterID, 
                    string Name, 
                    string Email, 
                    string Password, 
                    string PhoneNumber, 
                    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> AccountID, 
                    string Subject, 
                    string Message, 
                    string FileName, 
                    [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] File, 
                    string FileType, 
                    int FileLength, 
                    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<decimal> Latitude, 
                    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<decimal> Longitude, 
                    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> TypeID, 
                    [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> ImageRotation, 
                    string Device) {
            object[] results = this.Invoke("ReportIssueAndRegisterUser", new object[] {
                        ReporterID,
                        Name,
                        Email,
                        Password,
                        PhoneNumber,
                        AccountID,
                        Subject,
                        Message,
                        FileName,
                        File,
                        FileType,
                        FileLength,
                        Latitude,
                        Longitude,
                        TypeID,
                        ImageRotation,
                        Device});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ReportIssueAndRegisterUserAsync(
                    System.Nullable<int> ReporterID, 
                    string Name, 
                    string Email, 
                    string Password, 
                    string PhoneNumber, 
                    System.Nullable<int> AccountID, 
                    string Subject, 
                    string Message, 
                    string FileName, 
                    byte[] File, 
                    string FileType, 
                    int FileLength, 
                    System.Nullable<decimal> Latitude, 
                    System.Nullable<decimal> Longitude, 
                    System.Nullable<int> TypeID, 
                    System.Nullable<int> ImageRotation, 
                    string Device) {
            this.ReportIssueAndRegisterUserAsync(ReporterID, Name, Email, Password, PhoneNumber, AccountID, Subject, Message, FileName, File, FileType, FileLength, Latitude, Longitude, TypeID, ImageRotation, Device, null);
        }
        
        /// <remarks/>
        public void ReportIssueAndRegisterUserAsync(
                    System.Nullable<int> ReporterID, 
                    string Name, 
                    string Email, 
                    string Password, 
                    string PhoneNumber, 
                    System.Nullable<int> AccountID, 
                    string Subject, 
                    string Message, 
                    string FileName, 
                    byte[] File, 
                    string FileType, 
                    int FileLength, 
                    System.Nullable<decimal> Latitude, 
                    System.Nullable<decimal> Longitude, 
                    System.Nullable<int> TypeID, 
                    System.Nullable<int> ImageRotation, 
                    string Device, 
                    object userState) {
            if ((this.ReportIssueAndRegisterUserOperationCompleted == null)) {
                this.ReportIssueAndRegisterUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OnReportIssueAndRegisterUserOperationCompleted);
            }
            this.InvokeAsync("ReportIssueAndRegisterUser", new object[] {
                        ReporterID,
                        Name,
                        Email,
                        Password,
                        PhoneNumber,
                        AccountID,
                        Subject,
                        Message,
                        FileName,
                        File,
                        FileType,
                        FileLength,
                        Latitude,
                        Longitude,
                        TypeID,
                        ImageRotation,
                        Device}, this.ReportIssueAndRegisterUserOperationCompleted, userState);
        }
        
        private void OnReportIssueAndRegisterUserOperationCompleted(object arg) {
            if ((this.ReportIssueAndRegisterUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ReportIssueAndRegisterUserCompleted(this, new ReportIssueAndRegisterUserCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CloseIssue", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string CloseIssue([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> AccountID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> UserID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> ReporterID, int IssueID, string InternalNotes, string FileName, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] File, string FileType, int FileLength, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<decimal> Latitude, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<decimal> Longitude, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> ImageRotation) {
            object[] results = this.Invoke("CloseIssue", new object[] {
                        AccountID,
                        UserID,
                        ReporterID,
                        IssueID,
                        InternalNotes,
                        FileName,
                        File,
                        FileType,
                        FileLength,
                        Latitude,
                        Longitude,
                        ImageRotation});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void CloseIssueAsync(System.Nullable<int> AccountID, System.Nullable<int> UserID, System.Nullable<int> ReporterID, int IssueID, string InternalNotes, string FileName, byte[] File, string FileType, int FileLength, System.Nullable<decimal> Latitude, System.Nullable<decimal> Longitude, System.Nullable<int> ImageRotation) {
            this.CloseIssueAsync(AccountID, UserID, ReporterID, IssueID, InternalNotes, FileName, File, FileType, FileLength, Latitude, Longitude, ImageRotation, null);
        }
        
        /// <remarks/>
        public void CloseIssueAsync(System.Nullable<int> AccountID, System.Nullable<int> UserID, System.Nullable<int> ReporterID, int IssueID, string InternalNotes, string FileName, byte[] File, string FileType, int FileLength, System.Nullable<decimal> Latitude, System.Nullable<decimal> Longitude, System.Nullable<int> ImageRotation, object userState) {
            if ((this.CloseIssueOperationCompleted == null)) {
                this.CloseIssueOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCloseIssueOperationCompleted);
            }
            this.InvokeAsync("CloseIssue", new object[] {
                        AccountID,
                        UserID,
                        ReporterID,
                        IssueID,
                        InternalNotes,
                        FileName,
                        File,
                        FileType,
                        FileLength,
                        Latitude,
                        Longitude,
                        ImageRotation}, this.CloseIssueOperationCompleted, userState);
        }
        
        private void OnCloseIssueOperationCompleted(object arg) {
            if ((this.CloseIssueCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CloseIssueCompleted(this, new CloseIssueCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetMyIssues", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetMyIssues(int ReporterID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> AccountID, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> TypeID) {
            object[] results = this.Invoke("GetMyIssues", new object[] {
                        ReporterID,
                        AccountID,
                        TypeID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetMyIssuesAsync(int ReporterID, System.Nullable<int> AccountID, System.Nullable<int> TypeID) {
            this.GetMyIssuesAsync(ReporterID, AccountID, TypeID, null);
        }
        
        /// <remarks/>
        public void GetMyIssuesAsync(int ReporterID, System.Nullable<int> AccountID, System.Nullable<int> TypeID, object userState) {
            if ((this.GetMyIssuesOperationCompleted == null)) {
                this.GetMyIssuesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetMyIssuesOperationCompleted);
            }
            this.InvokeAsync("GetMyIssues", new object[] {
                        ReporterID,
                        AccountID,
                        TypeID}, this.GetMyIssuesOperationCompleted, userState);
        }
        
        private void OnGetMyIssuesOperationCompleted(object arg) {
            if ((this.GetMyIssuesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetMyIssuesCompleted(this, new GetMyIssuesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetTypes", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetTypes([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] System.Nullable<int> AccountID) {
            object[] results = this.Invoke("GetTypes", new object[] {
                        AccountID});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetTypesAsync(System.Nullable<int> AccountID) {
            this.GetTypesAsync(AccountID, null);
        }
        
        /// <remarks/>
        public void GetTypesAsync(System.Nullable<int> AccountID, object userState) {
            if ((this.GetTypesOperationCompleted == null)) {
                this.GetTypesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetTypesOperationCompleted);
            }
            this.InvokeAsync("GetTypes", new object[] {
                        AccountID}, this.GetTypesOperationCompleted, userState);
        }
        
        private void OnGetTypesOperationCompleted(object arg) {
            if ((this.GetTypesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetTypesCompleted(this, new GetTypesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void RegisterReporterCompletedEventHandler(object sender, RegisterReporterCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RegisterReporterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RegisterReporterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void LoginReporterCompletedEventHandler(object sender, LoginReporterCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoginReporterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoginReporterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void LoginUserCompletedEventHandler(object sender, LoginUserCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class LoginUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal LoginUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void ReportIssueCompletedEventHandler(object sender, ReportIssueCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReportIssueCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReportIssueCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void ReportIssueAndRegisterUserCompletedEventHandler(object sender, ReportIssueAndRegisterUserCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ReportIssueAndRegisterUserCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ReportIssueAndRegisterUserCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void CloseIssueCompletedEventHandler(object sender, CloseIssueCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CloseIssueCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CloseIssueCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void GetMyIssuesCompletedEventHandler(object sender, GetMyIssuesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetMyIssuesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetMyIssuesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void GetTypesCompletedEventHandler(object sender, GetTypesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetTypesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetTypesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591