//------------------------------------------------------------------------------
// <generado automáticamente>
//     Este código fue generado por una herramienta.
//     //
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </generado automáticamente>
//------------------------------------------------------------------------------

namespace wsTraditum
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws1.traditum.com/", ConfigurationName="wsTraditum.WebService_IASoap")]
    public interface WebService_IASoap
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws1.traditum.com/Enviar", ReplyAction="*")]
        System.Threading.Tasks.Task<wsTraditum.EnviarResponse> EnviarAsync(wsTraditum.EnviarRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws1.traditum.com/Enviar20", ReplyAction="*")]
        System.Threading.Tasks.Task<wsTraditum.Enviar20Response> Enviar20Async(wsTraditum.Enviar20Request request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws1.traditum.com/EnviarAES", ReplyAction="*")]
        System.Threading.Tasks.Task<wsTraditum.EnviarAESResponse> EnviarAESAsync(wsTraditum.EnviarAESRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws1.traditum.com/Echo", ReplyAction="*")]
        System.Threading.Tasks.Task<wsTraditum.EchoResponse> EchoAsync(wsTraditum.EchoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws1.traditum.com/Test", ReplyAction="*")]
        System.Threading.Tasks.Task<wsTraditum.TestResponse> TestAsync(wsTraditum.TestRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Enviar", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.EnviarRequestBody Body;
        
        public EnviarRequest()
        {
        }
        
        public EnviarRequest(wsTraditum.EnviarRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class EnviarRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pszMsg;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string pszUser;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pszPwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string pszMsgType;
        
        public EnviarRequestBody()
        {
        }
        
        public EnviarRequestBody(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            this.pszMsg = pszMsg;
            this.pszUser = pszUser;
            this.pszPwd = pszPwd;
            this.pszMsgType = pszMsgType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarResponse", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.EnviarResponseBody Body;
        
        public EnviarResponse()
        {
        }
        
        public EnviarResponse(wsTraditum.EnviarResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class EnviarResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string EnviarResult;
        
        public EnviarResponseBody()
        {
        }
        
        public EnviarResponseBody(string EnviarResult)
        {
            this.EnviarResult = EnviarResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Enviar20Request
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Enviar20", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.Enviar20RequestBody Body;
        
        public Enviar20Request()
        {
        }
        
        public Enviar20Request(wsTraditum.Enviar20RequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class Enviar20RequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pszMsg;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string pszUser;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pszPwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string pszMsgType;
        
        public Enviar20RequestBody()
        {
        }
        
        public Enviar20RequestBody(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            this.pszMsg = pszMsg;
            this.pszUser = pszUser;
            this.pszPwd = pszPwd;
            this.pszMsgType = pszMsgType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class Enviar20Response
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Enviar20Response", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.Enviar20ResponseBody Body;
        
        public Enviar20Response()
        {
        }
        
        public Enviar20Response(wsTraditum.Enviar20ResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class Enviar20ResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string Enviar20Result;
        
        public Enviar20ResponseBody()
        {
        }
        
        public Enviar20ResponseBody(string Enviar20Result)
        {
            this.Enviar20Result = Enviar20Result;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarAESRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarAES", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.EnviarAESRequestBody Body;
        
        public EnviarAESRequest()
        {
        }
        
        public EnviarAESRequest(wsTraditum.EnviarAESRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class EnviarAESRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pszMsg;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string pszUser;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pszPwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string pszMsgType;
        
        public EnviarAESRequestBody()
        {
        }
        
        public EnviarAESRequestBody(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            this.pszMsg = pszMsg;
            this.pszUser = pszUser;
            this.pszPwd = pszPwd;
            this.pszMsgType = pszMsgType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EnviarAESResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EnviarAESResponse", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.EnviarAESResponseBody Body;
        
        public EnviarAESResponse()
        {
        }
        
        public EnviarAESResponse(wsTraditum.EnviarAESResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class EnviarAESResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string EnviarAESResult;
        
        public EnviarAESResponseBody()
        {
        }
        
        public EnviarAESResponseBody(string EnviarAESResult)
        {
            this.EnviarAESResult = EnviarAESResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EchoRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Echo", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.EchoRequestBody Body;
        
        public EchoRequest()
        {
        }
        
        public EchoRequest(wsTraditum.EchoRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class EchoRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pszMsg;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string pszUser;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string pszPwd;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string pszMsgType;
        
        public EchoRequestBody()
        {
        }
        
        public EchoRequestBody(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            this.pszMsg = pszMsg;
            this.pszUser = pszUser;
            this.pszPwd = pszPwd;
            this.pszMsgType = pszMsgType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class EchoResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="EchoResponse", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.EchoResponseBody Body;
        
        public EchoResponse()
        {
        }
        
        public EchoResponse(wsTraditum.EchoResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class EchoResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string EchoResult;
        
        public EchoResponseBody()
        {
        }
        
        public EchoResponseBody(string EchoResult)
        {
            this.EchoResult = EchoResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class TestRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="Test", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.TestRequestBody Body;
        
        public TestRequest()
        {
        }
        
        public TestRequest(wsTraditum.TestRequestBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class TestRequestBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string pszRes;
        
        public TestRequestBody()
        {
        }
        
        public TestRequestBody(string pszRes)
        {
            this.pszRes = pszRes;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class TestResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="TestResponse", Namespace="http://ws1.traditum.com/", Order=0)]
        public wsTraditum.TestResponseBody Body;
        
        public TestResponse()
        {
        }
        
        public TestResponse(wsTraditum.TestResponseBody Body)
        {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://ws1.traditum.com/")]
    public partial class TestResponseBody
    {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string TestResult;
        
        public TestResponseBody()
        {
        }
        
        public TestResponseBody(string TestResult)
        {
            this.TestResult = TestResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface WebService_IASoapChannel : wsTraditum.WebService_IASoap, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class WebService_IASoapClient : System.ServiceModel.ClientBase<wsTraditum.WebService_IASoap>, wsTraditum.WebService_IASoap
    {
        
    /// <summary>
    /// Implemente este método parcial para configurar el punto de conexión de servicio.
    /// </summary>
    /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
    /// <param name="clientCredentials">Credenciales de cliente</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WebService_IASoapClient(EndpointConfiguration endpointConfiguration) : 
                base(WebService_IASoapClient.GetBindingForEndpoint(endpointConfiguration), WebService_IASoapClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WebService_IASoapClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WebService_IASoapClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WebService_IASoapClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WebService_IASoapClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WebService_IASoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wsTraditum.EnviarResponse> wsTraditum.WebService_IASoap.EnviarAsync(wsTraditum.EnviarRequest request)
        {
            return base.Channel.EnviarAsync(request);
        }
        
        public System.Threading.Tasks.Task<wsTraditum.EnviarResponse> EnviarAsync(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            wsTraditum.EnviarRequest inValue = new wsTraditum.EnviarRequest();
            inValue.Body = new wsTraditum.EnviarRequestBody();
            inValue.Body.pszMsg = pszMsg;
            inValue.Body.pszUser = pszUser;
            inValue.Body.pszPwd = pszPwd;
            inValue.Body.pszMsgType = pszMsgType;
            return ((wsTraditum.WebService_IASoap)(this)).EnviarAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wsTraditum.Enviar20Response> wsTraditum.WebService_IASoap.Enviar20Async(wsTraditum.Enviar20Request request)
        {
            return base.Channel.Enviar20Async(request);
        }
        
        public System.Threading.Tasks.Task<wsTraditum.Enviar20Response> Enviar20Async(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            wsTraditum.Enviar20Request inValue = new wsTraditum.Enviar20Request();
            inValue.Body = new wsTraditum.Enviar20RequestBody();
            inValue.Body.pszMsg = pszMsg;
            inValue.Body.pszUser = pszUser;
            inValue.Body.pszPwd = pszPwd;
            inValue.Body.pszMsgType = pszMsgType;
            return ((wsTraditum.WebService_IASoap)(this)).Enviar20Async(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wsTraditum.EnviarAESResponse> wsTraditum.WebService_IASoap.EnviarAESAsync(wsTraditum.EnviarAESRequest request)
        {
            return base.Channel.EnviarAESAsync(request);
        }
        
        public System.Threading.Tasks.Task<wsTraditum.EnviarAESResponse> EnviarAESAsync(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            wsTraditum.EnviarAESRequest inValue = new wsTraditum.EnviarAESRequest();
            inValue.Body = new wsTraditum.EnviarAESRequestBody();
            inValue.Body.pszMsg = pszMsg;
            inValue.Body.pszUser = pszUser;
            inValue.Body.pszPwd = pszPwd;
            inValue.Body.pszMsgType = pszMsgType;
            return ((wsTraditum.WebService_IASoap)(this)).EnviarAESAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wsTraditum.EchoResponse> wsTraditum.WebService_IASoap.EchoAsync(wsTraditum.EchoRequest request)
        {
            return base.Channel.EchoAsync(request);
        }
        
        public System.Threading.Tasks.Task<wsTraditum.EchoResponse> EchoAsync(string pszMsg, string pszUser, string pszPwd, string pszMsgType)
        {
            wsTraditum.EchoRequest inValue = new wsTraditum.EchoRequest();
            inValue.Body = new wsTraditum.EchoRequestBody();
            inValue.Body.pszMsg = pszMsg;
            inValue.Body.pszUser = pszUser;
            inValue.Body.pszPwd = pszPwd;
            inValue.Body.pszMsgType = pszMsgType;
            return ((wsTraditum.WebService_IASoap)(this)).EchoAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wsTraditum.TestResponse> wsTraditum.WebService_IASoap.TestAsync(wsTraditum.TestRequest request)
        {
            return base.Channel.TestAsync(request);
        }
        
        public System.Threading.Tasks.Task<wsTraditum.TestResponse> TestAsync(string pszRes)
        {
            wsTraditum.TestRequest inValue = new wsTraditum.TestRequest();
            inValue.Body = new wsTraditum.TestRequestBody();
            inValue.Body.pszRes = pszRes;
            return ((wsTraditum.WebService_IASoap)(this)).TestAsync(inValue);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WebService_IASoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.WebService_IASoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpsTransportBindingElement httpsBindingElement = new System.ServiceModel.Channels.HttpsTransportBindingElement();
                httpsBindingElement.AllowCookies = true;
                httpsBindingElement.MaxBufferSize = int.MaxValue;
                httpsBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpsBindingElement);
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WebService_IASoap))
            {
                return new System.ServiceModel.EndpointAddress("https://canalws.traditum.com/WebService_IA.asmx");
            }
            if ((endpointConfiguration == EndpointConfiguration.WebService_IASoap12))
            {
                return new System.ServiceModel.EndpointAddress("https://canalws.traditum.com/WebService_IA.asmx");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        public enum EndpointConfiguration
        {
            
            WebService_IASoap,
            
            WebService_IASoap12,
        }
    }
}
