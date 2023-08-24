using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HttpClientSettings
{
    [Serializable]
    public class InvalidAppSettingsException : ApplicationException
    {
        public InvalidAppSettingsException(List<string> errors)
            : base($"Invalid http client settings found: {string.Join(",", errors)}")
        {
            
        }

        private InvalidAppSettingsException() : base()
        {

        }

        protected InvalidAppSettingsException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            throw new InvalidAppSettingsException();
        }
    }
}
