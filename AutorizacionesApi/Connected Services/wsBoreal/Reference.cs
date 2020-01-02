//------------------------------------------------------------------------------
// <generado automáticamente>
//     Este código fue generado por una herramienta.
//     //
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </generado automáticamente>
//------------------------------------------------------------------------------

namespace wsBoreal
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="WsBoreal", ConfigurationName="wsBoreal.WsBorealSoapPort")]
    public interface WsBorealSoapPort
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="WsBorealaction/AWSBOREAL.Execute", ReplyAction="*")]
        System.Threading.Tasks.Task<wsBoreal.ExecuteResponse> ExecuteAsync(wsBoreal.ExecuteRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="WsBoreal.Execute", WrapperNamespace="WsBoreal", IsWrapped=true)]
    public partial class ExecuteRequest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="WsBoreal", Order=0)]
        public string Ingresoxml;
        
        public ExecuteRequest()
        {
        }
        
        public ExecuteRequest(string Ingresoxml)
        {
            this.Ingresoxml = Ingresoxml;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="WsBoreal.ExecuteResponse", WrapperNamespace="WsBoreal", IsWrapped=true)]
    public partial class ExecuteResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="WsBoreal", Order=0)]
        public string Egresoxml;
        
        public ExecuteResponse()
        {
        }
        
        public ExecuteResponse(string Egresoxml)
        {
            this.Egresoxml = Egresoxml;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface WsBorealSoapPortChannel : wsBoreal.WsBorealSoapPort, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class WsBorealSoapPortClient : System.ServiceModel.ClientBase<wsBoreal.WsBorealSoapPort>, wsBoreal.WsBorealSoapPort
    {
        
    /// <summary>
    /// Implemente este método parcial para configurar el punto de conexión de servicio.
    /// </summary>
    /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
    /// <param name="clientCredentials">Credenciales de cliente</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public WsBorealSoapPortClient() : 
                base(WsBorealSoapPortClient.GetDefaultBinding(), WsBorealSoapPortClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.WsBorealSoapPort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsBorealSoapPortClient(EndpointConfiguration endpointConfiguration) : 
                base(WsBorealSoapPortClient.GetBindingForEndpoint(endpointConfiguration), WsBorealSoapPortClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsBorealSoapPortClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(WsBorealSoapPortClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsBorealSoapPortClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(WsBorealSoapPortClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public WsBorealSoapPortClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<wsBoreal.ExecuteResponse> wsBoreal.WsBorealSoapPort.ExecuteAsync(wsBoreal.ExecuteRequest request)
        {
            return base.Channel.ExecuteAsync(request);
        }
        
        public System.Threading.Tasks.Task<wsBoreal.ExecuteResponse> ExecuteAsync(string Ingresoxml)
        {
            wsBoreal.ExecuteRequest inValue = new wsBoreal.ExecuteRequest();
            inValue.Ingresoxml = Ingresoxml;
            return ((wsBoreal.WsBorealSoapPort)(this)).ExecuteAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.WsBorealSoapPort))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.WsBorealSoapPort))
            {
                return new System.ServiceModel.EndpointAddress("http://sistemasboreal.com.ar:5480/WsBoreal/servlet/awsboreal");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return WsBorealSoapPortClient.GetBindingForEndpoint(EndpointConfiguration.WsBorealSoapPort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return WsBorealSoapPortClient.GetEndpointAddress(EndpointConfiguration.WsBorealSoapPort);
        }
        
        public enum EndpointConfiguration
        {
            
            WsBorealSoapPort,
        }
    }
}
