﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRE.Sisow {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="ArrayOfString", Namespace="http://tempuri.org/", ItemName="string")]
    [System.SerializableAttribute()]
    public class ArrayOfString : System.Collections.Generic.List<string> {
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Sisow.AssurePaySoap")]
    public interface AssurePaySoap {
        
        // CODEGEN: Generating message contract since element name GetBankenResult from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetBanken", ReplyAction="*")]
        HRE.Sisow.GetBankenResponse GetBanken(HRE.Sisow.GetBankenRequest request);
        
        // CODEGEN: Generating message contract since element name GetIssuersResult from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetIssuers", ReplyAction="*")]
        HRE.Sisow.GetIssuersResponse GetIssuers(HRE.Sisow.GetIssuersRequest request);
        
        // CODEGEN: Generating message contract since element name gebruiker from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetStatus", ReplyAction="*")]
        HRE.Sisow.GetStatusResponse GetStatus(HRE.Sisow.GetStatusRequest request);
        
        // CODEGEN: Generating message contract since element name gebruiker from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetTransaction", ReplyAction="*")]
        HRE.Sisow.GetTransactionResponse GetTransaction(HRE.Sisow.GetTransactionRequest request);
        
        // CODEGEN: Generating message contract since element name gebruiker from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetURL", ReplyAction="*")]
        HRE.Sisow.GetURLResponse GetURL(HRE.Sisow.GetURLRequest request);
        
        // CODEGEN: Generating message contract since element name gebruiker from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetURL2", ReplyAction="*")]
        HRE.Sisow.GetURL2Response GetURL2(HRE.Sisow.GetURL2Request request);
        
        // CODEGEN: Generating message contract since element name PingResult from namespace http://tempuri.org/ is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/Ping", ReplyAction="*")]
        HRE.Sisow.PingResponse Ping(HRE.Sisow.PingRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetBankenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetBanken", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetBankenRequestBody Body;
        
        public GetBankenRequest() {
        }
        
        public GetBankenRequest(HRE.Sisow.GetBankenRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class GetBankenRequestBody {
        
        public GetBankenRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetBankenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetBankenResponse", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetBankenResponseBody Body;
        
        public GetBankenResponse() {
        }
        
        public GetBankenResponse(HRE.Sisow.GetBankenResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetBankenResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public HRE.Sisow.ArrayOfString GetBankenResult;
        
        public GetBankenResponseBody() {
        }
        
        public GetBankenResponseBody(HRE.Sisow.ArrayOfString GetBankenResult) {
            this.GetBankenResult = GetBankenResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetIssuersRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetIssuers", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetIssuersRequestBody Body;
        
        public GetIssuersRequest() {
        }
        
        public GetIssuersRequest(HRE.Sisow.GetIssuersRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetIssuersRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=0)]
        public bool test;
        
        public GetIssuersRequestBody() {
        }
        
        public GetIssuersRequestBody(bool test) {
            this.test = test;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetIssuersResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetIssuersResponse", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetIssuersResponseBody Body;
        
        public GetIssuersResponse() {
        }
        
        public GetIssuersResponse(HRE.Sisow.GetIssuersResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetIssuersResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public HRE.Sisow.ArrayOfString GetIssuersResult;
        
        public GetIssuersResponseBody() {
        }
        
        public GetIssuersResponseBody(HRE.Sisow.ArrayOfString GetIssuersResult) {
            this.GetIssuersResult = GetIssuersResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStatusRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStatus", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetStatusRequestBody Body;
        
        public GetStatusRequest() {
        }
        
        public GetStatusRequest(HRE.Sisow.GetStatusRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetStatusRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string gebruiker;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string wachtwoord;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string transaction;
        
        public GetStatusRequestBody() {
        }
        
        public GetStatusRequestBody(string gebruiker, string wachtwoord, string transaction) {
            this.gebruiker = gebruiker;
            this.wachtwoord = wachtwoord;
            this.transaction = transaction;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetStatusResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetStatusResponse", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetStatusResponseBody Body;
        
        public GetStatusResponse() {
        }
        
        public GetStatusResponse(HRE.Sisow.GetStatusResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetStatusResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetStatusResult;
        
        public GetStatusResponseBody() {
        }
        
        public GetStatusResponseBody(string GetStatusResult) {
            this.GetStatusResult = GetStatusResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetTransactionRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetTransaction", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetTransactionRequestBody Body;
        
        public GetTransactionRequest() {
        }
        
        public GetTransactionRequest(HRE.Sisow.GetTransactionRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetTransactionRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string gebruiker;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string wachtwoord;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string transaction;
        
        public GetTransactionRequestBody() {
        }
        
        public GetTransactionRequestBody(string gebruiker, string wachtwoord, string transaction) {
            this.gebruiker = gebruiker;
            this.wachtwoord = wachtwoord;
            this.transaction = transaction;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetTransactionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetTransactionResponse", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetTransactionResponseBody Body;
        
        public GetTransactionResponse() {
        }
        
        public GetTransactionResponse(HRE.Sisow.GetTransactionResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetTransactionResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetTransactionResult;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string stamp;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=2)]
        public double amount;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string account;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string name;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string city;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string purchaseid;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string description;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string message;
        
        public GetTransactionResponseBody() {
        }
        
        public GetTransactionResponseBody(string GetTransactionResult, string stamp, double amount, string account, string name, string city, string purchaseid, string description, string message) {
            this.GetTransactionResult = GetTransactionResult;
            this.stamp = stamp;
            this.amount = amount;
            this.account = account;
            this.name = name;
            this.city = city;
            this.purchaseid = purchaseid;
            this.description = description;
            this.message = message;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetURLRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetURL", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetURLRequestBody Body;
        
        public GetURLRequest() {
        }
        
        public GetURLRequest(HRE.Sisow.GetURLRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetURLRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string gebruiker;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string wachtwoord;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string bank;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public long bedrag;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string kenmerk;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string omschrijving;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string returnURL;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string entrance;
        
        public GetURLRequestBody() {
        }
        
        public GetURLRequestBody(string gebruiker, string wachtwoord, string bank, long bedrag, string kenmerk, string omschrijving, string returnURL, string entrance) {
            this.gebruiker = gebruiker;
            this.wachtwoord = wachtwoord;
            this.bank = bank;
            this.bedrag = bedrag;
            this.kenmerk = kenmerk;
            this.omschrijving = omschrijving;
            this.returnURL = returnURL;
            this.entrance = entrance;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetURLResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetURLResponse", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetURLResponseBody Body;
        
        public GetURLResponse() {
        }
        
        public GetURLResponse(HRE.Sisow.GetURLResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetURLResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetURLResult;
        
        public GetURLResponseBody() {
        }
        
        public GetURLResponseBody(string GetURLResult) {
            this.GetURLResult = GetURLResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetURL2Request {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetURL2", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetURL2RequestBody Body;
        
        public GetURL2Request() {
        }
        
        public GetURL2Request(HRE.Sisow.GetURL2RequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetURL2RequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string gebruiker;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string wachtwoord;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string bank;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=3)]
        public long bedrag;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string kenmerk;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string omschrijving;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string returnURL;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string entrance;
        
        public GetURL2RequestBody() {
        }
        
        public GetURL2RequestBody(string gebruiker, string wachtwoord, string bank, long bedrag, string kenmerk, string omschrijving, string returnURL, string entrance) {
            this.gebruiker = gebruiker;
            this.wachtwoord = wachtwoord;
            this.bank = bank;
            this.bedrag = bedrag;
            this.kenmerk = kenmerk;
            this.omschrijving = omschrijving;
            this.returnURL = returnURL;
            this.entrance = entrance;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class GetURL2Response {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="GetURL2Response", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.GetURL2ResponseBody Body;
        
        public GetURL2Response() {
        }
        
        public GetURL2Response(HRE.Sisow.GetURL2ResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class GetURL2ResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string GetURL2Result;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string txid;
        
        public GetURL2ResponseBody() {
        }
        
        public GetURL2ResponseBody(string GetURL2Result, string txid) {
            this.GetURL2Result = GetURL2Result;
            this.txid = txid;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PingRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Ping", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.PingRequestBody Body;
        
        public PingRequest() {
        }
        
        public PingRequest(HRE.Sisow.PingRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class PingRequestBody {
        
        public PingRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class PingResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="PingResponse", Namespace="http://tempuri.org/", Order=0)]
        public HRE.Sisow.PingResponseBody Body;
        
        public PingResponse() {
        }
        
        public PingResponse(HRE.Sisow.PingResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://tempuri.org/")]
    public partial class PingResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string PingResult;
        
        public PingResponseBody() {
        }
        
        public PingResponseBody(string PingResult) {
            this.PingResult = PingResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AssurePaySoapChannel : HRE.Sisow.AssurePaySoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AssurePaySoapClient : System.ServiceModel.ClientBase<HRE.Sisow.AssurePaySoap>, HRE.Sisow.AssurePaySoap {
        
        public AssurePaySoapClient() {
        }
        
        public AssurePaySoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AssurePaySoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AssurePaySoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AssurePaySoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.GetBankenResponse HRE.Sisow.AssurePaySoap.GetBanken(HRE.Sisow.GetBankenRequest request) {
            return base.Channel.GetBanken(request);
        }
        
        public HRE.Sisow.ArrayOfString GetBanken() {
            HRE.Sisow.GetBankenRequest inValue = new HRE.Sisow.GetBankenRequest();
            inValue.Body = new HRE.Sisow.GetBankenRequestBody();
            HRE.Sisow.GetBankenResponse retVal = ((HRE.Sisow.AssurePaySoap)(this)).GetBanken(inValue);
            return retVal.Body.GetBankenResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.GetIssuersResponse HRE.Sisow.AssurePaySoap.GetIssuers(HRE.Sisow.GetIssuersRequest request) {
            return base.Channel.GetIssuers(request);
        }
        
        public HRE.Sisow.ArrayOfString GetIssuers(bool test) {
            HRE.Sisow.GetIssuersRequest inValue = new HRE.Sisow.GetIssuersRequest();
            inValue.Body = new HRE.Sisow.GetIssuersRequestBody();
            inValue.Body.test = test;
            HRE.Sisow.GetIssuersResponse retVal = ((HRE.Sisow.AssurePaySoap)(this)).GetIssuers(inValue);
            return retVal.Body.GetIssuersResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.GetStatusResponse HRE.Sisow.AssurePaySoap.GetStatus(HRE.Sisow.GetStatusRequest request) {
            return base.Channel.GetStatus(request);
        }
        
        public string GetStatus(string gebruiker, string wachtwoord, string transaction) {
            HRE.Sisow.GetStatusRequest inValue = new HRE.Sisow.GetStatusRequest();
            inValue.Body = new HRE.Sisow.GetStatusRequestBody();
            inValue.Body.gebruiker = gebruiker;
            inValue.Body.wachtwoord = wachtwoord;
            inValue.Body.transaction = transaction;
            HRE.Sisow.GetStatusResponse retVal = ((HRE.Sisow.AssurePaySoap)(this)).GetStatus(inValue);
            return retVal.Body.GetStatusResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.GetTransactionResponse HRE.Sisow.AssurePaySoap.GetTransaction(HRE.Sisow.GetTransactionRequest request) {
            return base.Channel.GetTransaction(request);
        }
        
        public string GetTransaction(string gebruiker, string wachtwoord, string transaction, out string stamp, out double amount, out string account, out string name, out string city, out string purchaseid, out string description, out string message) {
            HRE.Sisow.GetTransactionRequest inValue = new HRE.Sisow.GetTransactionRequest();
            inValue.Body = new HRE.Sisow.GetTransactionRequestBody();
            inValue.Body.gebruiker = gebruiker;
            inValue.Body.wachtwoord = wachtwoord;
            inValue.Body.transaction = transaction;
            HRE.Sisow.GetTransactionResponse retVal = ((HRE.Sisow.AssurePaySoap)(this)).GetTransaction(inValue);
            stamp = retVal.Body.stamp;
            amount = retVal.Body.amount;
            account = retVal.Body.account;
            name = retVal.Body.name;
            city = retVal.Body.city;
            purchaseid = retVal.Body.purchaseid;
            description = retVal.Body.description;
            message = retVal.Body.message;
            return retVal.Body.GetTransactionResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.GetURLResponse HRE.Sisow.AssurePaySoap.GetURL(HRE.Sisow.GetURLRequest request) {
            return base.Channel.GetURL(request);
        }
        
        public string GetURL(string gebruiker, string wachtwoord, string bank, long bedrag, string kenmerk, string omschrijving, string returnURL, string entrance) {
            HRE.Sisow.GetURLRequest inValue = new HRE.Sisow.GetURLRequest();
            inValue.Body = new HRE.Sisow.GetURLRequestBody();
            inValue.Body.gebruiker = gebruiker;
            inValue.Body.wachtwoord = wachtwoord;
            inValue.Body.bank = bank;
            inValue.Body.bedrag = bedrag;
            inValue.Body.kenmerk = kenmerk;
            inValue.Body.omschrijving = omschrijving;
            inValue.Body.returnURL = returnURL;
            inValue.Body.entrance = entrance;
            HRE.Sisow.GetURLResponse retVal = ((HRE.Sisow.AssurePaySoap)(this)).GetURL(inValue);
            return retVal.Body.GetURLResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.GetURL2Response HRE.Sisow.AssurePaySoap.GetURL2(HRE.Sisow.GetURL2Request request) {
            return base.Channel.GetURL2(request);
        }
        
        public string GetURL2(string gebruiker, string wachtwoord, string bank, long bedrag, string kenmerk, string omschrijving, string returnURL, string entrance, out string txid) {
            HRE.Sisow.GetURL2Request inValue = new HRE.Sisow.GetURL2Request();
            inValue.Body = new HRE.Sisow.GetURL2RequestBody();
            inValue.Body.gebruiker = gebruiker;
            inValue.Body.wachtwoord = wachtwoord;
            inValue.Body.bank = bank;
            inValue.Body.bedrag = bedrag;
            inValue.Body.kenmerk = kenmerk;
            inValue.Body.omschrijving = omschrijving;
            inValue.Body.returnURL = returnURL;
            inValue.Body.entrance = entrance;
            HRE.Sisow.GetURL2Response retVal = ((HRE.Sisow.AssurePaySoap)(this)).GetURL2(inValue);
            txid = retVal.Body.txid;
            return retVal.Body.GetURL2Result;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        HRE.Sisow.PingResponse HRE.Sisow.AssurePaySoap.Ping(HRE.Sisow.PingRequest request) {
            return base.Channel.Ping(request);
        }
        
        public string Ping() {
            HRE.Sisow.PingRequest inValue = new HRE.Sisow.PingRequest();
            inValue.Body = new HRE.Sisow.PingRequestBody();
            HRE.Sisow.PingResponse retVal = ((HRE.Sisow.AssurePaySoap)(this)).Ping(inValue);
            return retVal.Body.PingResult;
        }
    }
}
