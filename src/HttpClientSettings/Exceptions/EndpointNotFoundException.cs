using System;
using System.Runtime.Serialization;

namespace HttpClientSettings
{
    [Serializable]
    public class EndpointNotFoundException : ApplicationException
    {
        public EndpointNotFoundException(string endpointName)
            :base($"Endpoint: '{endpointName}' not found")
        { 
        
        }


        private EndpointNotFoundException() : base()
        {

        }

        protected EndpointNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            throw new EndpointNotFoundException();
        }
    }
}
