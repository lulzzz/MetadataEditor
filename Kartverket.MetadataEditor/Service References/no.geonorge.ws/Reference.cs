﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kartverket.MetadataEditor.no.geonorge.ws {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://ws.geonorge.no/SKWS2/services/SokKomData", ConfigurationName="no.geonorge.ws.SokKomData")]
    public interface SokKomData {
        
        // CODEGEN: Generating message contract since the wrapper namespace (http://ws.ngiskomws.sk.hosledata.no) of message kommuneSok1Request does not match the default value (https://ws.geonorge.no/SKWS2/services/SokKomData)
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(Style=System.ServiceModel.OperationFormatStyle.Rpc, SupportFaults=true, Use=System.ServiceModel.OperationFormatUse.Encoded)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(KommuneData))]
        Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Response kommuneSok1(Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Response> kommuneSok1Async(Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:komsok2.ws.statkart.no")]
    public partial class KomSokRes : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string errorMsgField;
        
        private bool okField;
        
        private KommuneData[] resultatField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string errorMsg {
            get {
                return this.errorMsgField;
            }
            set {
                this.errorMsgField = value;
                this.RaisePropertyChanged("errorMsg");
            }
        }
        
        /// <remarks/>
        public bool ok {
            get {
                return this.okField;
            }
            set {
                this.okField = value;
                this.RaisePropertyChanged("ok");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public KommuneData[] resultat {
            get {
                return this.resultatField;
            }
            set {
                this.resultatField = value;
                this.RaisePropertyChanged("resultat");
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.79.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="urn:komsok2.ws.statkart.no")]
    public partial class KommuneData : object, System.ComponentModel.INotifyPropertyChanged {
        
        private double boxAmaxField;
        
        private double boxAminField;
        
        private double boxNmaxField;
        
        private double boxNminField;
        
        private double gaustField;
        
        private double gnordField;
        
        private string kNavnField;
        
        private string kSenterField;
        
        private int kommNrField;
        
        private double senterAField;
        
        private double senterNField;
        
        private int ssbNrField;
        
        /// <remarks/>
        public double boxAmax {
            get {
                return this.boxAmaxField;
            }
            set {
                this.boxAmaxField = value;
                this.RaisePropertyChanged("boxAmax");
            }
        }
        
        /// <remarks/>
        public double boxAmin {
            get {
                return this.boxAminField;
            }
            set {
                this.boxAminField = value;
                this.RaisePropertyChanged("boxAmin");
            }
        }
        
        /// <remarks/>
        public double boxNmax {
            get {
                return this.boxNmaxField;
            }
            set {
                this.boxNmaxField = value;
                this.RaisePropertyChanged("boxNmax");
            }
        }
        
        /// <remarks/>
        public double boxNmin {
            get {
                return this.boxNminField;
            }
            set {
                this.boxNminField = value;
                this.RaisePropertyChanged("boxNmin");
            }
        }
        
        /// <remarks/>
        public double gaust {
            get {
                return this.gaustField;
            }
            set {
                this.gaustField = value;
                this.RaisePropertyChanged("gaust");
            }
        }
        
        /// <remarks/>
        public double gnord {
            get {
                return this.gnordField;
            }
            set {
                this.gnordField = value;
                this.RaisePropertyChanged("gnord");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string kNavn {
            get {
                return this.kNavnField;
            }
            set {
                this.kNavnField = value;
                this.RaisePropertyChanged("kNavn");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string kSenter {
            get {
                return this.kSenterField;
            }
            set {
                this.kSenterField = value;
                this.RaisePropertyChanged("kSenter");
            }
        }
        
        /// <remarks/>
        public int kommNr {
            get {
                return this.kommNrField;
            }
            set {
                this.kommNrField = value;
                this.RaisePropertyChanged("kommNr");
            }
        }
        
        /// <remarks/>
        public double senterA {
            get {
                return this.senterAField;
            }
            set {
                this.senterAField = value;
                this.RaisePropertyChanged("senterA");
            }
        }
        
        /// <remarks/>
        public double senterN {
            get {
                return this.senterNField;
            }
            set {
                this.senterNField = value;
                this.RaisePropertyChanged("senterN");
            }
        }
        
        /// <remarks/>
        public int ssbNr {
            get {
                return this.ssbNrField;
            }
            set {
                this.ssbNrField = value;
                this.RaisePropertyChanged("ssbNr");
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="kommuneSok1", WrapperNamespace="http://ws.ngiskomws.sk.hosledata.no", IsWrapped=true)]
    public partial class kommuneSok1Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public string brukerid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=1)]
        public string passord;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=2)]
        public string aliasId;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=3)]
        public int kommNr;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=4)]
        public string kommNavn;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=5)]
        public int koordSysUt;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=6)]
        public int koordSysInn;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=7)]
        public double nordMin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=8)]
        public double austMin;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=9)]
        public double nordMax;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=10)]
        public double austMax;
        
        public kommuneSok1Request() {
        }
        
        public kommuneSok1Request(string brukerid, string passord, string aliasId, int kommNr, string kommNavn, int koordSysUt, int koordSysInn, double nordMin, double austMin, double nordMax, double austMax) {
            this.brukerid = brukerid;
            this.passord = passord;
            this.aliasId = aliasId;
            this.kommNr = kommNr;
            this.kommNavn = kommNavn;
            this.koordSysUt = koordSysUt;
            this.koordSysInn = koordSysInn;
            this.nordMin = nordMin;
            this.austMin = austMin;
            this.nordMax = nordMax;
            this.austMax = austMax;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="kommuneSok1Response", WrapperNamespace="https://ws.geonorge.no/SKWS2/services/SokKomData", IsWrapped=true)]
    public partial class kommuneSok1Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="", Order=0)]
        public Kartverket.MetadataEditor.no.geonorge.ws.KomSokRes kommuneSok1Return;
        
        public kommuneSok1Response() {
        }
        
        public kommuneSok1Response(Kartverket.MetadataEditor.no.geonorge.ws.KomSokRes kommuneSok1Return) {
            this.kommuneSok1Return = kommuneSok1Return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SokKomDataChannel : Kartverket.MetadataEditor.no.geonorge.ws.SokKomData, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SokKomDataClient : System.ServiceModel.ClientBase<Kartverket.MetadataEditor.no.geonorge.ws.SokKomData>, Kartverket.MetadataEditor.no.geonorge.ws.SokKomData {
        
        public SokKomDataClient() {
        }
        
        public SokKomDataClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SokKomDataClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SokKomDataClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SokKomDataClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Response Kartverket.MetadataEditor.no.geonorge.ws.SokKomData.kommuneSok1(Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request request) {
            return base.Channel.kommuneSok1(request);
        }
        
        public Kartverket.MetadataEditor.no.geonorge.ws.KomSokRes kommuneSok1(string brukerid, string passord, string aliasId, int kommNr, string kommNavn, int koordSysUt, int koordSysInn, double nordMin, double austMin, double nordMax, double austMax) {
            Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request inValue = new Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request();
            inValue.brukerid = brukerid;
            inValue.passord = passord;
            inValue.aliasId = aliasId;
            inValue.kommNr = kommNr;
            inValue.kommNavn = kommNavn;
            inValue.koordSysUt = koordSysUt;
            inValue.koordSysInn = koordSysInn;
            inValue.nordMin = nordMin;
            inValue.austMin = austMin;
            inValue.nordMax = nordMax;
            inValue.austMax = austMax;
            Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Response retVal = ((Kartverket.MetadataEditor.no.geonorge.ws.SokKomData)(this)).kommuneSok1(inValue);
            return retVal.kommuneSok1Return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Response> Kartverket.MetadataEditor.no.geonorge.ws.SokKomData.kommuneSok1Async(Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request request) {
            return base.Channel.kommuneSok1Async(request);
        }
        
        public System.Threading.Tasks.Task<Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Response> kommuneSok1Async(string brukerid, string passord, string aliasId, int kommNr, string kommNavn, int koordSysUt, int koordSysInn, double nordMin, double austMin, double nordMax, double austMax) {
            Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request inValue = new Kartverket.MetadataEditor.no.geonorge.ws.kommuneSok1Request();
            inValue.brukerid = brukerid;
            inValue.passord = passord;
            inValue.aliasId = aliasId;
            inValue.kommNr = kommNr;
            inValue.kommNavn = kommNavn;
            inValue.koordSysUt = koordSysUt;
            inValue.koordSysInn = koordSysInn;
            inValue.nordMin = nordMin;
            inValue.austMin = austMin;
            inValue.nordMax = nordMax;
            inValue.austMax = austMax;
            return ((Kartverket.MetadataEditor.no.geonorge.ws.SokKomData)(this)).kommuneSok1Async(inValue);
        }
    }
}
