using System;
using System.Runtime.Serialization;

namespace HttpClientSettings
{
    [Serializable]
    public class ClientNotFoundException : ApplicationException
    {
        public ClientNotFoundException(string clientName)
            :base($"Client: '{clientName}' not found")
        { 
        
        }


        private ClientNotFoundException() : base()
        {

        }

        protected ClientNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            throw new ClientNotFoundException();
        }
    }
}
