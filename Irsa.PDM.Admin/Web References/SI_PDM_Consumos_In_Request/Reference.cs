﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Irsa.PDM.Admin.SI_PDM_Consumos_In_Request {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SI_PDM_Consumos_In_RequestBinding", Namespace="http://irsa.com/pi/pdm/consumos")]
    public partial class SI_PDM_Consumos_In_RequestService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback SI_PDM_Consumos_In_RequestOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SI_PDM_Consumos_In_RequestService() {
            this.Url = global::Irsa.PDM.Admin.Properties.Settings.Default.Irsa_PDM_Admin_SI_PDM_Consumos_In_Request_SI_PDM_Consumos_In_RequestService;
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
        public event SI_PDM_Consumos_In_RequestCompletedEventHandler SI_PDM_Consumos_In_RequestCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://sap.com/xi/WebService/soap1.1", OneWay=true, Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public void SI_PDM_Consumos_In_Request([System.Xml.Serialization.XmlElementAttribute(Namespace="http://irsa.com/pi/pdm/consumos")] DT_PDM_Consumos_In_Request MT_PDM_Consumos_In_Request) {
            this.Invoke("SI_PDM_Consumos_In_Request", new object[] {
                        MT_PDM_Consumos_In_Request});
        }
        
        /// <remarks/>
        public void SI_PDM_Consumos_In_RequestAsync(DT_PDM_Consumos_In_Request MT_PDM_Consumos_In_Request) {
            this.SI_PDM_Consumos_In_RequestAsync(MT_PDM_Consumos_In_Request, null);
        }
        
        /// <remarks/>
        public void SI_PDM_Consumos_In_RequestAsync(DT_PDM_Consumos_In_Request MT_PDM_Consumos_In_Request, object userState) {
            if ((this.SI_PDM_Consumos_In_RequestOperationCompleted == null)) {
                this.SI_PDM_Consumos_In_RequestOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSI_PDM_Consumos_In_RequestOperationCompleted);
            }
            this.InvokeAsync("SI_PDM_Consumos_In_Request", new object[] {
                        MT_PDM_Consumos_In_Request}, this.SI_PDM_Consumos_In_RequestOperationCompleted, userState);
        }
        
        private void OnSI_PDM_Consumos_In_RequestOperationCompleted(object arg) {
            if ((this.SI_PDM_Consumos_In_RequestCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.SI_PDM_Consumos_In_RequestCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://irsa.com/pi/pdm/consumos")]
    public partial class DT_PDM_Consumos_In_Request {
        
        private DT_PDM_Consumos_In_RequestItem[] zMMIM_CONMED_F001Field;
        
        private DT_PDM_Consumos_In_RequestItem1[] zMMIM_CONMED_F002Field;
        
        private DT_PDM_Consumos_In_RequestItem2[] zMMIM_CONMED_F003Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public DT_PDM_Consumos_In_RequestItem[] ZMMIM_CONMED_F001 {
            get {
                return this.zMMIM_CONMED_F001Field;
            }
            set {
                this.zMMIM_CONMED_F001Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public DT_PDM_Consumos_In_RequestItem1[] ZMMIM_CONMED_F002 {
            get {
                return this.zMMIM_CONMED_F002Field;
            }
            set {
                this.zMMIM_CONMED_F002Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public DT_PDM_Consumos_In_RequestItem2[] ZMMIM_CONMED_F003 {
            get {
                return this.zMMIM_CONMED_F003Field;
            }
            set {
                this.zMMIM_CONMED_F003Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://irsa.com/pi/pdm/consumos")]
    public partial class DT_PDM_Consumos_In_RequestItem {
        
        private string idConsumeField;
        
        private string bankField;
        
        private string materialNumberField;
        
        private string quantityField;
        
        private string plantField;
        
        private string storageLocationField;
        
        private string documentHeaderTextField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idConsume {
            get {
                return this.idConsumeField;
            }
            set {
                this.idConsumeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bank {
            get {
                return this.bankField;
            }
            set {
                this.bankField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string materialNumber {
            get {
                return this.materialNumberField;
            }
            set {
                this.materialNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string plant {
            get {
                return this.plantField;
            }
            set {
                this.plantField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storageLocation {
            get {
                return this.storageLocationField;
            }
            set {
                this.storageLocationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentHeaderText {
            get {
                return this.documentHeaderTextField;
            }
            set {
                this.documentHeaderTextField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://irsa.com/pi/pdm/consumos")]
    public partial class DT_PDM_Consumos_In_RequestItem1 {
        
        private string idConsumeField;
        
        private string bankField;
        
        private string materialNumberField;
        
        private string quantityField;
        
        private string plantField;
        
        private string storageLocationField;
        
        private string documentHeaderTextField;
        
        private string orderNumberField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idConsume {
            get {
                return this.idConsumeField;
            }
            set {
                this.idConsumeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bank {
            get {
                return this.bankField;
            }
            set {
                this.bankField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string materialNumber {
            get {
                return this.materialNumberField;
            }
            set {
                this.materialNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string plant {
            get {
                return this.plantField;
            }
            set {
                this.plantField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storageLocation {
            get {
                return this.storageLocationField;
            }
            set {
                this.storageLocationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentHeaderText {
            get {
                return this.documentHeaderTextField;
            }
            set {
                this.documentHeaderTextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string orderNumber {
            get {
                return this.orderNumberField;
            }
            set {
                this.orderNumberField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2102.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://irsa.com/pi/pdm/consumos")]
    public partial class DT_PDM_Consumos_In_RequestItem2 {
        
        private string idConsumeField;
        
        private string bankField;
        
        private string materialNumberField;
        
        private string quantityField;
        
        private string plant_OField;
        
        private string purchasingGroupField;
        
        private string purchasingDocumentTypeField;
        
        private string purchasingOrganizationField;
        
        private string storageLocation_OField;
        
        private string documentHeaderTextField;
        
        private string orderNumberField;
        
        private string plant_DField;
        
        private string storageLocation_DField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idConsume {
            get {
                return this.idConsumeField;
            }
            set {
                this.idConsumeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bank {
            get {
                return this.bankField;
            }
            set {
                this.bankField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string materialNumber {
            get {
                return this.materialNumberField;
            }
            set {
                this.materialNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string plant_O {
            get {
                return this.plant_OField;
            }
            set {
                this.plant_OField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string purchasingGroup {
            get {
                return this.purchasingGroupField;
            }
            set {
                this.purchasingGroupField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string purchasingDocumentType {
            get {
                return this.purchasingDocumentTypeField;
            }
            set {
                this.purchasingDocumentTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string purchasingOrganization {
            get {
                return this.purchasingOrganizationField;
            }
            set {
                this.purchasingOrganizationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storageLocation_O {
            get {
                return this.storageLocation_OField;
            }
            set {
                this.storageLocation_OField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentHeaderText {
            get {
                return this.documentHeaderTextField;
            }
            set {
                this.documentHeaderTextField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string orderNumber {
            get {
                return this.orderNumberField;
            }
            set {
                this.orderNumberField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string plant_D {
            get {
                return this.plant_DField;
            }
            set {
                this.plant_DField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string storageLocation_D {
            get {
                return this.storageLocation_DField;
            }
            set {
                this.storageLocation_DField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.2053.0")]
    public delegate void SI_PDM_Consumos_In_RequestCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591