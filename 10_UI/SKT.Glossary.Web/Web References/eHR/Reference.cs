﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 이 소스 코드가 Microsoft.VSDesigner, 버전 4.0.30319.42000에서 자동으로 생성되었습니다.
// 
#pragma warning disable 1591

namespace SKT.Glossary.Web.eHR {
<<<<<<< HEAD
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
=======
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    
    
    /// <remarks/>
    // CODEGEN: 네임스페이스 'http://schemas.xmlsoap.org/ws/2004/09/policy'에서 선택적 WSDL 확장 요소 'Policy'이(가) 처리되지 않았습니다.
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1099.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ZEHR_DISP_CARRER_1", Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZEHR_DISP_CARRER_1 : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ZehrDispCarrerOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ZEHR_DISP_CARRER_1() {
            this.Url = global::SKT.Glossary.Web.Properties.Settings.Default.SKT_Glossary_Web_eHR_ZEHR_DISP_CARRER_1;
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
        public event ZehrDispCarrerCompletedEventHandler ZehrDispCarrerCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:sap-com:document:sap:soap:functions:mc-style:ZEHR_DISP_CARRER_1:ZehrDispCarre" +
            "rRequest", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("ZehrDispCarrerResponse", Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
        public ZehrDispCarrerResponse ZehrDispCarrer([System.Xml.Serialization.XmlElementAttribute("ZehrDispCarrer", Namespace="urn:sap-com:document:sap:soap:functions:mc-style")] ZehrDispCarrer ZehrDispCarrer1) {
            object[] results = this.Invoke("ZehrDispCarrer", new object[] {
                        ZehrDispCarrer1});
            return ((ZehrDispCarrerResponse)(results[0]));
        }
        
        /// <remarks/>
        public void ZehrDispCarrerAsync(ZehrDispCarrer ZehrDispCarrer1) {
            this.ZehrDispCarrerAsync(ZehrDispCarrer1, null);
        }
        
        /// <remarks/>
        public void ZehrDispCarrerAsync(ZehrDispCarrer ZehrDispCarrer1, object userState) {
            if ((this.ZehrDispCarrerOperationCompleted == null)) {
                this.ZehrDispCarrerOperationCompleted = new System.Threading.SendOrPostCallback(this.OnZehrDispCarrerOperationCompleted);
            }
            this.InvokeAsync("ZehrDispCarrer", new object[] {
                        ZehrDispCarrer1}, this.ZehrDispCarrerOperationCompleted, userState);
        }
        
        private void OnZehrDispCarrerOperationCompleted(object arg) {
            if ((this.ZehrDispCarrerCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ZehrDispCarrerCompleted(this, new ZehrDispCarrerCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2634.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZehrDispCarrer {
        
        private string iGubunField;
        
        private string iPernrField;
        
        private string iSelecField;
        
        private Zehrs0002[] tOut1Field;
        
        private Zehrs0003[] tOut2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IGubun {
            get {
                return this.iGubunField;
            }
            set {
                this.iGubunField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string IPernr {
            get {
                return this.iPernrField;
            }
            set {
                this.iPernrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ISelec {
            get {
                return this.iSelecField;
            }
            set {
                this.iSelecField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Zehrs0002[] TOut1 {
            get {
                return this.tOut1Field;
            }
            set {
                this.tOut1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Zehrs0003[] TOut2 {
            get {
                return this.tOut2Field;
            }
            set {
                this.tOut2Field = value;
            }
        }
    }
    
    /// <remarks/>
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2634.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Zehrs0002 {
        
        private string massntField;
        
        private string mgtxtField;
        
        private string vglstField;
        
        private string jtextField;
        
        private string pernrField;
        
        private string begdaField;
        
        private string massnField;
        
        private string mntxtField;
        
        private string massgField;
        
        private string plansField;
        
        private string orgehField;
        
        private string orgtxField;
        
        private string jikchakField;
        
        private string jctxtField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Massnt {
            get {
                return this.massntField;
            }
            set {
                this.massntField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Mgtxt {
            get {
                return this.mgtxtField;
            }
            set {
                this.mgtxtField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Vglst {
            get {
                return this.vglstField;
            }
            set {
                this.vglstField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Jtext {
            get {
                return this.jtextField;
            }
            set {
                this.jtextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Pernr {
            get {
                return this.pernrField;
            }
            set {
                this.pernrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Begda {
            get {
                return this.begdaField;
            }
            set {
                this.begdaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Massn {
            get {
                return this.massnField;
            }
            set {
                this.massnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Mntxt {
            get {
                return this.mntxtField;
            }
            set {
                this.mntxtField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Massg {
            get {
                return this.massgField;
            }
            set {
                this.massgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Plans {
            get {
                return this.plansField;
            }
            set {
                this.plansField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Orgeh {
            get {
                return this.orgehField;
            }
            set {
                this.orgehField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Orgtx {
            get {
                return this.orgtxField;
            }
            set {
                this.orgtxField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Jikchak {
            get {
                return this.jikchakField;
            }
            set {
                this.jikchakField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Jctxt {
            get {
                return this.jctxtField;
            }
            set {
                this.jctxtField = value;
            }
        }
    }
    
    /// <remarks/>
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2634.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Bapireturn1 {
        
        private string typeField;
        
        private string idField;
        
        private string numberField;
        
        private string messageField;
        
        private string logNoField;
        
        private string logMsgNoField;
        
        private string messageV1Field;
        
        private string messageV2Field;
        
        private string messageV3Field;
        
        private string messageV4Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Number {
            get {
                return this.numberField;
            }
            set {
                this.numberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LogNo {
            get {
                return this.logNoField;
            }
            set {
                this.logNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LogMsgNo {
            get {
                return this.logMsgNoField;
            }
            set {
                this.logMsgNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV1 {
            get {
                return this.messageV1Field;
            }
            set {
                this.messageV1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV2 {
            get {
                return this.messageV2Field;
            }
            set {
                this.messageV2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV3 {
            get {
                return this.messageV3Field;
            }
            set {
                this.messageV3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MessageV4 {
            get {
                return this.messageV4Field;
            }
            set {
                this.messageV4Field = value;
            }
        }
    }
    
    /// <remarks/>
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2634.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class Zehrs0003 {
        
        private string pernrField;
        
        private string compyField;
        
        private string zdeptField;
        
        private string begdaField;
        
        private string enddaField;
        
        private string zgmjmField;
        
        private string lstjwField;
        
        private string skgynField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Pernr {
            get {
                return this.pernrField;
            }
            set {
                this.pernrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Compy {
            get {
                return this.compyField;
            }
            set {
                this.compyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Zdept {
            get {
                return this.zdeptField;
            }
            set {
                this.zdeptField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Begda {
            get {
                return this.begdaField;
            }
            set {
                this.begdaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Endda {
            get {
                return this.enddaField;
            }
            set {
                this.enddaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Zgmjm {
            get {
                return this.zgmjmField;
            }
            set {
                this.zgmjmField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Lstjw {
            get {
                return this.lstjwField;
            }
            set {
                this.lstjwField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Skgyn {
            get {
                return this.skgynField;
            }
            set {
                this.skgynField = value;
            }
        }
    }
    
    /// <remarks/>
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2634.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:sap-com:document:sap:soap:functions:mc-style")]
    public partial class ZehrDispCarrerResponse {
        
        private Bapireturn1 eReturnField;
        
        private Zehrs0002[] tOut1Field;
        
        private Zehrs0003[] tOut2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Bapireturn1 EReturn {
            get {
                return this.eReturnField;
            }
            set {
                this.eReturnField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Zehrs0002[] TOut1 {
            get {
                return this.tOut1Field;
            }
            set {
                this.tOut1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public Zehrs0003[] TOut2 {
            get {
                return this.tOut2Field;
            }
            set {
                this.tOut2Field = value;
            }
        }
    }
    
    /// <remarks/>
<<<<<<< HEAD
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1099.0")]
    public delegate void ZehrDispCarrerCompletedEventHandler(object sender, ZehrDispCarrerCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1099.0")]
=======
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ZehrDispCarrerCompletedEventHandler(object sender, ZehrDispCarrerCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ZehrDispCarrerCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ZehrDispCarrerCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ZehrDispCarrerResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ZehrDispCarrerResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591