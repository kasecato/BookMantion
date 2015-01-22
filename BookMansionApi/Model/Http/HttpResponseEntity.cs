using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BookMansionApi.Model.Http
{
    [DataContract(Name = "httpResponseEntity")]
    public sealed class HttpResponseEntity
    {
        [DataMember(Order = 1000, IsRequired = false, Name = "content")]
        public string Content { get; set; }

        [DataMember(Order = 1050, IsRequired = false, Name = "contentLength")]
        public long ContentLength { get; set; }

        [DataMember(Order = 1100, IsRequired = false, Name = "contentType")]
        public string ContentType { get; set; }

        [DataMember(Order = 1150, IsRequired = false, Name = "responseUri")]
        public Uri ResponseUri { get; set; }

        [DataMember(Order = 1200, IsRequired = false, Name = "headers")]
        public IDictionary<string, IEnumerable<string>> Headers { get; set; }

        [DataMember(Order = 1250, IsRequired = false, Name = "isSuccessStatusCode")]
        public bool IsSuccessStatusCode { get; set; }

        [DataMember(Order = 1300, IsRequired = false, Name = "reasonPhrase")]
        public string ReasonPhrase { get; set; }

        [DataMember(Order = 1350, IsRequired = false, Name = "requestMessage")]
        public HttpRequestEntity RequestMessage { get; set; }

        [DataMember(Order = 1400, IsRequired = false, Name = "statusCode")]
        public int StatusCode { get; set; }

        [DataMember(Order = 1450, IsRequired = false, Name = "version")]
        public string Version { get; set; }
    }
}
