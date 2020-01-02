//------------------------------------------------------------------------------
// <generado automáticamente>
//     Este código fue generado por una herramienta.
//     //
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </generado automáticamente>
//------------------------------------------------------------------------------

namespace Sancor
{
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/")]
    public partial class Exception
    {
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", ConfigurationName="Sancor.HL7v24")]
    public interface HL7v24
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(Sancor.Exception), Action="", Name="Exception")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<Sancor.MessageTestResponse> MessageTestAsync(Sancor.MessageTest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [System.ServiceModel.FaultContractAttribute(typeof(Sancor.Exception), Action="", Name="Exception")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Threading.Tasks.Task<Sancor.MessageResponse> MessageAsync(Sancor.Message request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MessageTest", WrapperNamespace="http://servicio.hl7v24.sancorsalud.com.ar/", IsWrapped=true)]
    public partial class MessageTest
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int pasaporte;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mensaje;
        
        public MessageTest()
        {
        }
        
        public MessageTest(int pasaporte, string mensaje)
        {
            this.pasaporte = pasaporte;
            this.mensaje = mensaje;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MessageTestResponse", WrapperNamespace="http://servicio.hl7v24.sancorsalud.com.ar/", IsWrapped=true)]
    public partial class MessageTestResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resultado;
        
        public MessageTestResponse()
        {
        }
        
        public MessageTestResponse(string resultado)
        {
            this.resultado = resultado;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="Message", WrapperNamespace="http://servicio.hl7v24.sancorsalud.com.ar/", IsWrapped=true)]
    public partial class Message
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int pasaporte;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mensaje;
        
        public Message()
        {
        }
        
        public Message(int pasaporte, string mensaje)
        {
            this.pasaporte = pasaporte;
            this.mensaje = mensaje;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="MessageResponse", WrapperNamespace="http://servicio.hl7v24.sancorsalud.com.ar/", IsWrapped=true)]
    public partial class MessageResponse
    {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://servicio.hl7v24.sancorsalud.com.ar/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resultado;
        
        public MessageResponse()
        {
        }
        
        public MessageResponse(string resultado)
        {
            this.resultado = resultado;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public interface HL7v24Channel : Sancor.HL7v24, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("dotnet-svcutil", "1.0.0.1")]
    public partial class HL7v24Client : System.ServiceModel.ClientBase<Sancor.HL7v24>, Sancor.HL7v24
    {
        
    /// <summary>
    /// Implemente este método parcial para configurar el punto de conexión de servicio.
    /// </summary>
    /// <param name="serviceEndpoint">El punto de conexión para configurar</param>
    /// <param name="clientCredentials">Credenciales de cliente</param>
    static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public HL7v24Client() : 
                base(HL7v24Client.GetDefaultBinding(), HL7v24Client.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.HL7v24Port.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HL7v24Client(EndpointConfiguration endpointConfiguration) : 
                base(HL7v24Client.GetBindingForEndpoint(endpointConfiguration), HL7v24Client.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HL7v24Client(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(HL7v24Client.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HL7v24Client(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(HL7v24Client.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public HL7v24Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Sancor.MessageTestResponse> Sancor.HL7v24.MessageTestAsync(Sancor.MessageTest request)
        {
            return base.Channel.MessageTestAsync(request);
        }
        
        public System.Threading.Tasks.Task<Sancor.MessageTestResponse> MessageTestAsync(int pasaporte, string mensaje)
        {
            Sancor.MessageTest inValue = new Sancor.MessageTest();
            inValue.pasaporte = pasaporte;
            inValue.mensaje = mensaje;
            return ((Sancor.HL7v24)(this)).MessageTestAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Sancor.MessageResponse> Sancor.HL7v24.MessageAsync(Sancor.Message request)
        {
            return base.Channel.MessageAsync(request);
        }
        
        public System.Threading.Tasks.Task<Sancor.MessageResponse> MessageAsync(int pasaporte, string mensaje)
        {
            Sancor.Message inValue = new Sancor.Message();
            inValue.pasaporte = pasaporte;
            inValue.mensaje = mensaje;
            return ((Sancor.HL7v24)(this)).MessageAsync(inValue);
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
            if ((endpointConfiguration == EndpointConfiguration.HL7v24Port))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                result.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.HL7v24Port))
            {
                return new System.ServiceModel.EndpointAddress("https://servicios.sancorsalud.com.ar/Autorizador/HL7v24");
            }
            throw new System.InvalidOperationException(string.Format("No se pudo encontrar un punto de conexión con el nombre \"{0}\".", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return HL7v24Client.GetBindingForEndpoint(EndpointConfiguration.HL7v24Port);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return HL7v24Client.GetEndpointAddress(EndpointConfiguration.HL7v24Port);
        }
        
        public enum EndpointConfiguration
        {
            
            HL7v24Port,
        }
    }
}
